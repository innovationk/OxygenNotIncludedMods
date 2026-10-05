// Decompiled with JetBrains decompiler
// Type: JetSuitTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/JetSuitTank")]
public class JetSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, IDevQuickAction
{
  [MyCmpGet]
  private ElementEmitter elementConverter;
  [Serialize]
  public SimHashes lastFuelUsed = SimHashes.Vacuum;
  [Serialize]
  public float amount;
  public const float FUEL_CAPACITY = 100f;
  public const float FUEL_BURN_RATE = 0.2f;
  public const float CO2_EMITTED_PER_FUEL_BURNED = 0.25f;
  public const float EMIT_TEMPERATURE = 373.15f;
  public const float REFILL_PERCENT = 0.2f;
  private JetSuitMonitor.Instance jetSuitMonitor;
  private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>((Action<JetSuitTank, object>) ((component, data) => component.OnEquipped(data)));
  private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>((Action<JetSuitTank, object>) ((component, data) => component.OnUnequipped(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<JetSuitTank>(-1617557748, JetSuitTank.OnEquippedDelegate);
    this.Subscribe<JetSuitTank>(-170173755, JetSuitTank.OnUnequippedDelegate);
  }

  public float PercentFull() => this.amount / 100f;

  public bool IsEmpty() => (double) this.amount <= 0.0;

  public bool IsFull() => (double) this.PercentFull() >= 1.0;

  public bool NeedsRecharging() => (double) this.PercentFull() < 0.20000000298023224;

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    string str = string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.JETSUIT_TANK, (object) GameUtil.GetFormattedMass(this.amount));
    descriptors.Add(new Descriptor(str, str));
    return descriptors;
  }

  private void OnEquipped(object data)
  {
    Equipment equipment = (Equipment) data;
    NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
    this.jetSuitMonitor = new JetSuitMonitor.Instance((IStateMachineTarget) this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
    this.jetSuitMonitor.StartSM();
    if (!this.IsEmpty())
      return;
    equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.JetSuitOutOfFuel);
  }

  private void OnUnequipped(object data)
  {
    Equipment equipment = (Equipment) data;
    if (!equipment.destroyed)
    {
      equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.JetSuitOutOfFuel);
      NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), (Func<float>) null, false);
      Navigator component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Navigator>();
      if ((bool) (UnityEngine.Object) component && component.CurrentNavType == NavType.Hover)
        component.SetCurrentNavType(NavType.Floor);
    }
    if (this.jetSuitMonitor == null)
      return;
    this.jetSuitMonitor.StopSM("Removed jetsuit tank");
    this.jetSuitMonitor = (JetSuitMonitor.Instance) null;
  }

  [ContextMenu("Empty")]
  public void Empty() => this.amount = 0.0f;

  [ContextMenu("Fill Tank")]
  public void FillTank()
  {
    this.amount = 100f;
    if (this.jetSuitMonitor == null || !((UnityEngine.Object) this.jetSuitMonitor.sm.owner.Get(this.jetSuitMonitor) != (UnityEngine.Object) null))
      return;
    this.jetSuitMonitor.sm.owner.Get(this.jetSuitMonitor).RemoveTag(GameTags.JetSuitOutOfFuel);
  }

  public List<DevQuickActionInstruction> GetDevInstructions()
  {
    return new List<DevQuickActionInstruction>()
    {
      new DevQuickActionInstruction(IDevQuickAction.CommonMenusNames.Storage, "Fill Fuel", new System.Action(this.FillTank)),
      new DevQuickActionInstruction(IDevQuickAction.CommonMenusNames.Storage, "Empty Fuel", new System.Action(this.Empty))
    };
  }
}
