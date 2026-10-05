// Decompiled with JetBrains decompiler
// Type: Durability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using KSerialization;
using System;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Durability")]
public class Durability : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Durability> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Durability>((Action<Durability, object>) ((component, data) => component.OnEquipped()));
  private static readonly EventSystem.IntraObjectHandler<Durability> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<Durability>((Action<Durability, object>) ((component, data) => component.OnUnequipped()));
  [Serialize]
  private bool isEquipped;
  [Serialize]
  private float timeEquipped;
  [Serialize]
  private float durability = 1f;
  public float durabilityLossPerCycle = -0.1f;
  public string wornEquipmentPrefabID;
  private float difficultySettingMod = 1f;

  public float TimeEquipped
  {
    get => this.timeEquipped;
    set => this.timeEquipped = value;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Durability>(-1617557748, Durability.OnEquippedDelegate);
    this.Subscribe<Durability>(-170173755, Durability.OnUnequippedDelegate);
  }

  protected override void OnSpawn()
  {
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.Durability, (object) this.gameObject);
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Durability);
    if (currentQualitySetting == null)
      return;
    switch (currentQualitySetting.id)
    {
      case "Indestructible":
        this.difficultySettingMod = EQUIPMENT.SUITS.INDESTRUCTIBLE_DURABILITY_MOD;
        break;
      case "Reinforced":
        this.difficultySettingMod = EQUIPMENT.SUITS.REINFORCED_DURABILITY_MOD;
        break;
      case "Flimsy":
        this.difficultySettingMod = EQUIPMENT.SUITS.FLIMSY_DURABILITY_MOD;
        break;
      case "Threadbare":
        this.difficultySettingMod = EQUIPMENT.SUITS.THREADBARE_DURABILITY_MOD;
        break;
    }
  }

  private void OnEquipped()
  {
    if (this.isEquipped)
      return;
    this.isEquipped = true;
    this.timeEquipped = GameClock.Instance.GetTimeInCycles();
  }

  private void OnUnequipped()
  {
    if (!this.isEquipped)
      return;
    this.isEquipped = false;
    this.DeltaDurability((GameClock.Instance.GetTimeInCycles() - this.timeEquipped) * this.durabilityLossPerCycle);
  }

  private void DeltaDurability(float delta)
  {
    delta *= this.difficultySettingMod;
    this.durability = Mathf.Clamp01(this.durability + delta);
  }

  public void ConvertToWornObject()
  {
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag) this.wornEquipmentPrefabID), Grid.SceneLayer.Ore);
    gameObject.transform.SetPosition(this.transform.GetPosition());
    gameObject.GetComponent<PrimaryElement>().SetElement(this.GetComponent<PrimaryElement>().ElementID, false);
    gameObject.SetActive(true);
    EquippableFacade component1 = this.GetComponent<EquippableFacade>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      gameObject.GetComponent<RepairableEquipment>().facadeID = component1.FacadeID;
    Storage component2 = this.gameObject.GetComponent<Storage>();
    if ((bool) (UnityEngine.Object) component2)
    {
      JetSuitTank component3 = this.gameObject.GetComponent<JetSuitTank>();
      if ((bool) (UnityEngine.Object) component3)
        component2.AddLiquid(component3.lastFuelUsed != SimHashes.Vacuum ? component3.lastFuelUsed : SimHashes.Petroleum, component3.amount, this.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0);
      component2.DropAll();
    }
    Util.KDestroyGameObject(this.gameObject);
  }

  public float GetDurability()
  {
    return this.isEquipped ? this.durability - (GameClock.Instance.GetTimeInCycles() - this.timeEquipped) * this.durabilityLossPerCycle : this.durability;
  }

  public bool IsWornOut() => (double) this.GetDurability() <= 0.0;
}
