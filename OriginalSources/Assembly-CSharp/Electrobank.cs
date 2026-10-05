// Decompiled with JetBrains decompiler
// Type: Electrobank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Electrobank : 
  KMonoBehaviour,
  ISim1000ms,
  ISim200ms,
  IConsumableUIItem,
  IGameObjectEffectDescriptor
{
  private static float capacity = 120000f;
  [Serialize]
  private float charge = Electrobank.capacity;
  private const float MAX_HEALTH = 10f;
  [Serialize]
  private float currentHealth = 10f;
  [Serialize]
  private float timeSincePowerDrawn = 0.5f;
  private const float RADIATION_EMITTER_TIMEOUT = 0.5f;
  public float radioactivityTuning;
  private RadiationEmitter radiationEmitter;
  private float lastDamageTime;
  public ProgressBar healthBar;
  public bool rechargeable;
  public bool keepEmpty;
  [MyCmpGet]
  private Pickupable pickupable;

  public string ID { private set; get; }

  public bool IsFullyCharged => (double) this.charge == (double) Electrobank.capacity;

  public float Charge => this.charge;

  protected override void OnPrefabInit()
  {
    this.ID = this.gameObject.PrefabID().ToString();
    this.Subscribe(748399584, new Action<object>(this.OnCraft));
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe(856640610, new Action<object>(this.ClearHealthBar));
    Components.Electrobanks.Add(this.gameObject.GetMyWorldId(), this);
    this.radiationEmitter = this.GetComponent<RadiationEmitter>();
    this.UpdateRadiationEmitter();
  }

  private void OnCraft(object data)
  {
    WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountProduced(this.Charge);
  }

  private void UpdateRadiationEmitter()
  {
    if ((UnityEngine.Object) this.radiationEmitter == (UnityEngine.Object) null)
      return;
    this.radiationEmitter.emitRads = (double) this.timeSincePowerDrawn < 0.5 ? this.radioactivityTuning : 0.0f;
    this.radiationEmitter.Refresh();
  }

  private static GameObject Replace(GameObject electrobank, Tag replacement, bool dropFromStorage = false)
  {
    Vector3 position = electrobank.transform.GetPosition();
    GameObject go = Util.KInstantiate(Assets.GetPrefab(replacement), position);
    go.GetComponent<PrimaryElement>().SetElement(electrobank.GetComponent<PrimaryElement>().Element.id);
    go.SetActive(true);
    Storage storage = electrobank.GetComponent<Pickupable>().storage;
    if ((UnityEngine.Object) storage != (UnityEngine.Object) null)
      storage.Remove(electrobank);
    electrobank.DeleteObject();
    if ((UnityEngine.Object) storage != (UnityEngine.Object) null && !dropFromStorage)
      storage.Store(go);
    return go;
  }

  public static GameObject ReplaceEmptyWithCharged(
    GameObject EmptyElectrobank,
    bool dropFromStorage = false)
  {
    return Electrobank.Replace(EmptyElectrobank, (Tag) nameof (Electrobank), dropFromStorage);
  }

  public static GameObject ReplaceChargedWithEmpty(
    GameObject ChargedElectrobank,
    bool dropFromStorage = false)
  {
    return Electrobank.Replace(ChargedElectrobank, (Tag) "EmptyElectrobank", dropFromStorage);
  }

  public static GameObject ReplaceEmptyWithGarbage(
    GameObject ChargedElectrobank,
    bool dropFromStorage = false)
  {
    return Electrobank.Replace(ChargedElectrobank, (Tag) "GarbageElectrobank", dropFromStorage);
  }

  public float AddPower(float joules)
  {
    if ((double) joules < 0.0)
      joules = 0.0f;
    float num = Mathf.Min(joules, Electrobank.capacity - this.charge);
    this.charge += num;
    return num;
  }

  public float RemovePower(float joules, bool dropWhenEmpty)
  {
    float num = Mathf.Min(this.charge, joules);
    this.charge -= num;
    if ((double) this.charge <= 0.0)
      this.OnEmpty(dropWhenEmpty);
    if ((double) num > 0.0)
      this.timeSincePowerDrawn = 0.0f;
    return num;
  }

  protected virtual void OnEmpty(bool dropWhenEmpty)
  {
    if (this.rechargeable)
    {
      Electrobank.ReplaceChargedWithEmpty(this.gameObject, dropWhenEmpty);
    }
    else
    {
      if (this.keepEmpty)
        return;
      if ((UnityEngine.Object) this.pickupable.storage != (UnityEngine.Object) null)
        this.pickupable.storage.Remove(this.gameObject);
      Util.KDestroyGameObject(this.gameObject);
    }
  }

  public void FullyCharge() => this.charge = Electrobank.capacity;

  public virtual void Explode()
  {
    int cell = Grid.PosToCell(this.gameObject.transform.position);
    float temperature = Mathf.Clamp(Grid.Temperature[cell] + this.charge / (Grid.Mass[cell] * Grid.Element[cell].specificHeatCapacity), 1f, 9999f);
    SimMessages.ReplaceElement(cell, Grid.Element[cell].id, CellEventLogger.Instance.SandBoxTool, Grid.Mass[cell], temperature, Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell]);
    Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, this.gameObject.transform.position, 0.0f);
    KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode"), this.gameObject.transform.position);
    if (this.rechargeable)
      Electrobank.ReplaceEmptyWithGarbage(this.gameObject);
    else
      this.gameObject.DeleteObject();
  }

  protected void LaunchNearbyStuff()
  {
    ListPool<ScenePartitionerEntry, Comet>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, Comet>.Allocate();
    Vector3 position = this.transform.position;
    GameScenePartitioner.Instance.GatherEntries((int) position.x - 3, (int) position.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      GameObject gameObject = (partitionerEntry.obj as Pickupable).gameObject;
      if (!((UnityEngine.Object) gameObject.GetComponent<MinionIdentity>() != (UnityEngine.Object) null) && !((UnityEngine.Object) gameObject.GetComponent<CreatureBrain>() != (UnityEngine.Object) null) && gameObject.GetDef<RobotAi.Def>() == null)
      {
        Vector2 initial_velocity = (Vector2) (gameObject.transform.GetPosition() - position);
        initial_velocity = initial_velocity.normalized;
        initial_velocity *= (float) UnityEngine.Random.Range(4, 6);
        initial_velocity.y += (float) UnityEngine.Random.Range(2, 4);
        if (GameComps.Fallers.Has((object) gameObject))
          GameComps.Fallers.Remove(gameObject);
        if (GameComps.Gravities.Has((object) gameObject))
          GameComps.Gravities.Remove(gameObject);
        GameComps.Fallers.Add(gameObject, initial_velocity);
      }
    }
    gathered_entries.Recycle();
  }

  public void Sim1000ms(float dt)
  {
    if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
      return;
    this.EvaluateWaterDamage(dt);
    this.UpdateHealthBar();
  }

  public virtual void Sim200ms(float dt)
  {
    this.UpdateRadiationEmitter();
    this.timeSincePowerDrawn = Mathf.Min(this.timeSincePowerDrawn + dt, 10f);
  }

  private void EvaluateWaterDamage(float dt)
  {
    if (!Grid.IsValidCell(this.pickupable.cachedCell) || !Grid.Element[this.pickupable.cachedCell].HasTag(GameTags.AnyWater) || UnityEngine.Random.Range(1, 101) <= 75)
      return;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.POWER_BANK_WATER_DAMAGE, this.transform);
    this.Damage(UnityEngine.Random.Range(0.0f, dt));
  }

  public void Damage(float amount)
  {
    Game.Instance.SpawnFX(SpawnFXHashes.ElectrobankDamage, Grid.PosToCell(this.gameObject), 0.0f);
    KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_sparks_short"), this.gameObject.transform.position);
    this.currentHealth -= amount;
    if ((UnityEngine.Object) this.healthBar == (UnityEngine.Object) null)
      this.CreateHealthBar();
    this.healthBar.Update();
    this.lastDamageTime = Time.time;
    if ((double) this.currentHealth > 0.0)
      return;
    this.Explode();
  }

  protected override void OnCleanUp()
  {
    this.ClearHealthBar();
    Components.Electrobanks.Remove(this.gameObject.GetMyWorldId(), this);
    base.OnCleanUp();
  }

  public void CreateHealthBar()
  {
    this.healthBar = ProgressBar.CreateProgressBar(this.gameObject, (Func<float>) (() => this.currentHealth / 10f));
    this.healthBar.SetVisibility(true);
    this.healthBar.barColor = Util.ColorFromHex("CC3333");
  }

  public void UpdateHealthBar()
  {
    if (!((UnityEngine.Object) this.healthBar != (UnityEngine.Object) null) || (double) Time.time - (double) this.lastDamageTime <= 5.0)
      return;
    this.ClearHealthBar();
  }

  public void ClearHealthBar(object _ = null)
  {
    if (!((UnityEngine.Object) this.healthBar != (UnityEngine.Object) null))
      return;
    Util.KDestroyGameObject((Component) this.healthBar);
    this.healthBar = (ProgressBar) null;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELECTROBANKS, (object) GameUtil.GetFormattedJoules(this.Charge)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELECTROBANKS, (object) GameUtil.GetFormattedJoules(this.Charge)));
    descriptors.Add(descriptor);
    return descriptors;
  }

  public string ConsumableId => this.PrefabID().Name;

  public string ConsumableName => this.GetProperName();

  public int MajorOrder => 500;

  public int MinorOrder => 0;

  public bool Display => true;
}
