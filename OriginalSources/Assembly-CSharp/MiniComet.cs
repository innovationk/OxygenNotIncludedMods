// Decompiled with JetBrains decompiler
// Type: MiniComet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MiniComet")]
public class MiniComet : KMonoBehaviour, ISim33ms
{
  [MyCmpGet]
  private PrimaryElement pe;
  public Vector2 spawnVelocity = new Vector2(7f, 9f);
  public Vector2 spawnAngle = new Vector2(30f, 150f);
  public SpawnFXHashes explosionEffectHash;
  public int addDiseaseCount;
  public byte diseaseIdx = byte.MaxValue;
  public Vector2I explosionOreCount = new Vector2I(1, 1);
  public Vector2 explosionSpeedRange = new Vector2(0.0f, 0.0f);
  public string impactSound;
  public string flyingSound;
  public int flyingSoundID;
  private HashedString FLYING_SOUND_ID_PARAMETER = (HashedString) "meteorType";
  public bool Targeted;
  [Serialize]
  protected Vector3 offsetPosition;
  [Serialize]
  protected Vector2 velocity;
  private Vector3 previousPosition;
  private bool hasExploded;
  public string[] craterPrefabs;
  public bool spawnWithOffset;
  private float age;
  public System.Action OnImpact;
  public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();
  [MyCmpGet]
  private KBatchedAnimController anim;
  [MyCmpGet]
  private KSelectable selectable;
  public Tag typeID;
  private LoopingSounds loopingSounds;
  private List<GameObject> damagedEntities = new List<GameObject>();
  private List<int> destroyedCells = new List<int>();
  private const float MAX_DISTANCE_TEST = 6f;

  public Vector3 TargetPosition => this.anim.PositionIncludingOffset;

  public Vector2 Velocity
  {
    get => this.velocity;
    set => this.velocity = value;
  }

  private float GetVolume(GameObject gameObject)
  {
    float volume = 1f;
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.selectable != (UnityEngine.Object) null && this.selectable.IsSelected)
      volume = 1f;
    return volume;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.loopingSounds = this.gameObject.GetComponent<LoopingSounds>();
    this.flyingSound = GlobalAssets.GetSound("Meteor_LP");
    this.RandomizeVelocity();
  }

  protected override void OnSpawn()
  {
    this.anim.Offset = this.offsetPosition;
    if (this.spawnWithOffset)
      this.SetupOffset();
    base.OnSpawn();
    this.StartLoopingSound();
    this.selectable.enabled = (double) this.offsetPosition.x == 0.0 && (double) this.offsetPosition.y == 0.0;
    this.typeID = this.GetComponent<KPrefabID>().PrefabTag;
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  protected void SetupOffset()
  {
    Vector3 position1 = this.transform.GetPosition();
    Vector3 position2 = this.transform.GetPosition() with
    {
      z = 0.0f
    };
    Vector3 vector3_1 = new Vector3(this.velocity.x, this.velocity.y, 0.0f);
    WorldContainer myWorld = this.gameObject.GetMyWorld();
    float num1 = Mathf.Abs(((float) (myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y) / Mathf.Cos(Vector3.Angle(Vector3.up, -vector3_1) * ((float) Math.PI / 180f)));
    Vector3 vector3_2 = position2 - vector3_1.normalized * num1;
    float num2 = (float) (myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
    if (((double) vector3_2.x < (double) myWorld.WorldOffset.x * (double) Grid.CellSizeInMeters ? 0 : ((double) vector3_2.x <= (double) num2 ? 1 : 0)) == 0)
      num1 = Mathf.Abs(((double) vector3_1.x < 0.0 ? num2 - position2.x : position2.x - (float) myWorld.WorldOffset.x * Grid.CellSizeInMeters) / Mathf.Cos(Vector3.Angle((double) vector3_1.x < 0.0 ? Vector3.right : Vector3.left, -vector3_1) * ((float) Math.PI / 180f)));
    Vector3 vector3_3 = -vector3_1.normalized * num1;
    (position2 + vector3_3).z = position1.z;
    this.offsetPosition = vector3_3;
    this.anim.Offset = this.offsetPosition;
  }

  public virtual void RandomizeVelocity()
  {
    float num1 = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
    float f = (float) ((double) num1 * 3.1415927410125732 / 180.0);
    float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
    this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
    this.GetComponent<KBatchedAnimController>().Rotation = (float) (-(double) num1 - 90.0);
  }

  public int GetRandomNumOres()
  {
    return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
  }

  [ContextMenu("Explode")]
  private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
  {
    int num = (int) Grid.WorldIdx[cell];
    this.PlayImpactSound(pos);
    Vector3 pos1 = pos with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2)
    };
    if (this.explosionEffectHash != SpawnFXHashes.None)
      Game.Instance.SpawnFX(this.explosionEffectHash, pos1, 0.0f);
    if (element != null)
    {
      Substance substance = element.substance;
      int randomNumOres = this.GetRandomNumOres();
      Vector2 vector2_1 = -this.velocity.normalized;
      Vector2 vector2_2 = new Vector2(vector2_1.y, -vector2_1.x);
      float mass = randomNumOres > 0 ? this.pe.Mass / (float) randomNumOres : 1f;
      for (int index = 0; index < randomNumOres; ++index)
      {
        Vector2 normalized = (vector2_1 + vector2_2 * UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector3 initial_velocity = (Vector3) (normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y));
        Vector3 position = pos1 + (Vector3) (normalized.normalized * 1.25f);
        GameObject go = substance.SpawnResource(position, mass, this.pe.Temperature, this.pe.DiseaseIdx, this.pe.DiseaseCount / randomNumOres);
        if (GameComps.Fallers.Has((object) go))
          GameComps.Fallers.Remove(go);
        GameComps.Fallers.Add(go, (Vector2) initial_velocity);
      }
    }
    if (this.OnImpact == null)
      return;
    this.OnImpact();
  }

  public float GetDistanceFromImpact()
  {
    float num1 = this.velocity.x / this.velocity.y;
    Vector3 position = this.transform.GetPosition();
    float y = 0.0f;
    while ((double) y > -6.0)
    {
      float num2 = y - 1f;
      y = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
      Vector3 vector3 = new Vector3(y * num1, y, 0.0f);
      int cell = Grid.PosToCell(position + vector3);
      if (Grid.IsValidCell(cell) && Grid.Solid[cell])
        return vector3.magnitude;
    }
    return 6f;
  }

  public float GetSoundDistance() => this.GetDistanceFromImpact();

  public void Sim33ms(float dt)
  {
    if (this.hasExploded)
      return;
    if ((double) this.offsetPosition.y > 0.0)
    {
      this.offsetPosition += new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0.0f);
      this.anim.Offset = this.offsetPosition;
    }
    else
    {
      if (this.anim.Offset != Vector3.zero)
        this.anim.Offset = Vector3.zero;
      if (!this.selectable.enabled)
        this.selectable.enabled = true;
      Vector2 vector2_1 = new Vector2((float) Grid.WidthInCells, (float) Grid.HeightInCells) * -0.1f;
      Vector2 vector2_2 = new Vector2((float) Grid.WidthInCells, (float) Grid.HeightInCells) * 1.1f;
      Vector3 position = this.transform.GetPosition();
      Vector3 vector3 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0.0f);
      Grid.PosToCell(vector3);
      this.loopingSounds.UpdateVelocity(this.flyingSound, (Vector2) (vector3 - position));
      if ((double) vector3.x < (double) vector2_1.x || (double) vector2_2.x < (double) vector3.x || (double) vector3.y < (double) vector2_1.y)
        Util.KDestroyGameObject(this.gameObject);
      int cell1 = Grid.PosToCell((KMonoBehaviour) this);
      int cell2 = Grid.PosToCell(this.previousPosition);
      if (cell1 != cell2 && Grid.IsValidCell(cell1) && Grid.Solid[cell1])
      {
        PrimaryElement component = this.GetComponent<PrimaryElement>();
        this.Explode(position, cell1, cell2, component.Element);
        this.hasExploded = true;
        Util.KDestroyGameObject(this.gameObject);
        return;
      }
      this.previousPosition = position;
      this.transform.SetPosition(vector3);
    }
    this.age += dt;
  }

  private void PlayImpactSound(Vector3 pos)
  {
    if (this.impactSound == null)
      this.impactSound = "Meteor_Large_Impact";
    this.loopingSounds.StopSound(this.flyingSound);
    string sound = GlobalAssets.GetSound(this.impactSound);
    int cell = Grid.PosToCell(pos);
    if (!Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
      return;
    float volume = this.GetVolume(this.gameObject);
    pos.z = 0.0f;
    EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
    int num = (int) instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
    KFMOD.EndOneShot(instance);
  }

  private void StartLoopingSound()
  {
    this.loopingSounds.StartSound(this.flyingSound);
    this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float) this.flyingSoundID);
  }

  public void Explode()
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    Vector3 position = this.transform.GetPosition();
    int cell = Grid.PosToCell(position);
    this.Explode(position, cell, cell, component.Element);
    this.hasExploded = true;
    Util.KDestroyGameObject(this.gameObject);
  }
}
