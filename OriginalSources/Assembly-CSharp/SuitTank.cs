// Decompiled with JetBrains decompiler
// Type: SuitTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SuitTank")]
public class SuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, OxygenBreather.IGasProvider
{
  public SafeCellQuery.SafeFlags SafeCellFlagsToIgnoreOnEquipped = SafeCellQuery.SafeFlags.CorrectTemperature | SafeCellQuery.SafeFlags.IsBreathable | SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace | SafeCellQuery.SafeFlags.IsNotLiquid;
  [Serialize]
  public string element;
  [Serialize]
  public float amount;
  public Tag elementTag;
  [MyCmpReq]
  public Storage storage;
  public float capacity;
  public const float REFILL_PERCENT = 0.25f;
  public bool underwaterSupport;
  private Equippable equippable;
  private static readonly EventSystem.IntraObjectHandler<SuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>((Action<SuitTank, object>) ((component, data) => component.OnEquipped(data)));
  private static readonly EventSystem.IntraObjectHandler<SuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>((Action<SuitTank, object>) ((component, data) => component.OnUnequipped(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<SuitTank>(-1617557748, SuitTank.OnEquippedDelegate);
    this.Subscribe<SuitTank>(-170173755, SuitTank.OnUnequippedDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((double) this.amount != 0.0)
    {
      this.storage.AddGasChunk(SimHashes.Oxygen, this.amount, this.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false);
      this.amount = 0.0f;
    }
    this.equippable = this.GetComponent<Equippable>();
  }

  public float GetTankAmount()
  {
    if ((UnityEngine.Object) this.storage == (UnityEngine.Object) null)
      this.storage = this.GetComponent<Storage>();
    return this.storage.GetMassAvailable(this.elementTag);
  }

  public float PercentFull() => this.GetTankAmount() / this.capacity;

  public bool IsEmpty() => (double) this.GetTankAmount() <= 0.0;

  public bool IsFull() => (double) this.PercentFull() >= 1.0;

  public bool NeedsRecharging() => (double) this.PercentFull() < 0.25;

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    if (this.elementTag == GameTags.Breathable)
    {
      string str = this.underwaterSupport ? string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK_UNDERWATER, (object) GameUtil.GetFormattedMass(this.GetTankAmount())) : string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK, (object) GameUtil.GetFormattedMass(this.GetTankAmount()));
      descriptors.Add(new Descriptor(str, str));
    }
    return descriptors;
  }

  private void OnEquipped(object data)
  {
    Equipment equipment = (Equipment) data;
    NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
    GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.GetComponent<Sensors>().GetSensor<SafeCellSensor>().AddIgnoredFlagsSet(nameof (SuitTank), this.SafeCellFlagsToIgnoreOnEquipped);
      component.AddGasProvider((OxygenBreather.IGasProvider) this);
    }
    targetGameObject.AddTag(GameTags.HasSuitTank);
  }

  private void OnUnequipped(object data)
  {
    Equipment equipment = (Equipment) data;
    if (equipment.destroyed)
      return;
    NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), false);
    GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.GetComponent<Sensors>().GetSensor<SafeCellSensor>().RemoveIgnoredFlagsSet(nameof (SuitTank));
      component.RemoveGasProvider((OxygenBreather.IGasProvider) this);
    }
    targetGameObject.RemoveTag(GameTags.HasSuitTank);
  }

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
  {
    if (this.IsEmpty())
      return false;
    float aggregate_temperature = 0.0f;
    SimHashes mostRelevantItemElement = SimHashes.Vacuum;
    float amount_consumed;
    SimUtil.DiseaseInfo disease_info;
    this.storage.ConsumeAndGetDisease(this.elementTag, amount, out amount_consumed, out disease_info, out aggregate_temperature, out mostRelevantItemElement);
    OxygenBreather.BreathableGasConsumed(oxygen_breather, mostRelevantItemElement, amount_consumed, aggregate_temperature, disease_info.idx, disease_info.count);
    this.Trigger(608245985, (object) this.gameObject);
    return true;
  }

  public bool ShouldEmitCO2()
  {
    bool flag1 = this.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
    if (flag1)
      return false;
    bool flag2 = this.IsOwnerBionic();
    return !flag1 && !flag2;
  }

  public bool ShouldStoreCO2()
  {
    bool flag1 = this.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
    if (!flag1)
      return false;
    bool flag2 = this.IsOwnerBionic();
    return flag1 && !flag2;
  }

  public bool IsOwnerBionic()
  {
    bool flag = false;
    if ((UnityEngine.Object) this.equippable != (UnityEngine.Object) null && this.equippable.IsAssigned() && this.equippable.isEquipped)
    {
      Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
      if ((UnityEngine.Object) soleOwner != (UnityEngine.Object) null)
      {
        GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
        if ((bool) (UnityEngine.Object) targetGameObject)
          flag = targetGameObject.PrefabID() == (Tag) BionicMinionConfig.ID;
      }
    }
    return flag;
  }

  public bool IsLowOxygen() => this.NeedsRecharging();

  [ContextMenu("SetToRefillAmount")]
  public void SetToRefillAmount()
  {
    float tankAmount = this.GetTankAmount();
    float num = 0.25f * this.capacity;
    if ((double) tankAmount <= (double) num)
      return;
    this.storage.ConsumeIgnoringDisease(this.elementTag, tankAmount - num);
  }

  [ContextMenu("Empty")]
  public void Empty() => this.storage.ConsumeIgnoringDisease(this.elementTag, this.GetTankAmount());

  [ContextMenu("Fill Tank")]
  public void FillTank()
  {
    this.Empty();
    this.storage.AddGasChunk(SimHashes.Oxygen, this.capacity, 15f, (byte) 0, 0, false, false);
  }

  public bool HasOxygen() => !this.IsEmpty();

  public bool IsBlocked() => false;
}
