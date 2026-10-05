// Decompiled with JetBrains decompiler
// Type: BottleEmptier
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
public class BottleEmptier : 
  StateMachineComponent<BottleEmptier.StatesInstance>,
  IGameObjectEffectDescriptor
{
  public float emptyRate = 10f;
  [Serialize]
  public bool allowManualPumpingStationFetching;
  [Serialize]
  public bool emit = true;
  public bool isGasEmptier;
  private static Dictionary<bool, string[]> manualPumpingAffectedBuildings = new Dictionary<bool, string[]>();
  private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>((Action<BottleEmptier, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>((Action<BottleEmptier, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.DefineManualPumpingAffectedBuildings();
    this.Subscribe<BottleEmptier>(493375141, BottleEmptier.OnRefreshUserMenuDelegate);
    this.Subscribe<BottleEmptier>(-905833192, BottleEmptier.OnCopySettingsDelegate);
  }

  public List<Descriptor> GetDescriptors(GameObject go) => (List<Descriptor>) null;

  private void DefineManualPumpingAffectedBuildings()
  {
    if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
      return;
    List<string> stringList = new List<string>();
    Tag tag = this.isGasEmptier ? GameTags.GasSource : GameTags.LiquidSource;
    foreach (BuildingDef buildingDef in Assets.BuildingDefs)
    {
      if (buildingDef.BuildingComplete.HasTag(tag))
        stringList.Add(buildingDef.Name);
    }
    BottleEmptier.manualPumpingAffectedBuildings.Add(this.isGasEmptier, stringList.ToArray());
  }

  private void OnChangeAllowManualPumpingStationFetching()
  {
    this.allowManualPumpingStationFetching = !this.allowManualPumpingStationFetching;
    this.smi.RefreshChore();
  }

  private void OnRefreshUserMenu(object data)
  {
    string tooltipText1 = (string) (this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.TOOLTIP);
    string tooltipText2 = (string) (this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.TOOLTIP);
    if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
    {
      foreach (string str1 in BottleEmptier.manualPumpingAffectedBuildings[this.isGasEmptier])
      {
        string str2 = string.Format((string) UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.ITEM, (object) str1);
        tooltipText1 += str2;
        tooltipText2 += str2;
      }
    }
    if (this.isGasEmptier)
      Game.Instance.userMenu.AddButton(this.gameObject, this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", (string) UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), tooltipText: tooltipText2) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", (string) UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), tooltipText: tooltipText1), 0.4f);
    else
      Game.Instance.userMenu.AddButton(this.gameObject, this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", (string) UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), tooltipText: tooltipText2) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", (string) UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), tooltipText: tooltipText1), 0.4f);
  }

  private void OnCopySettings(object data)
  {
    this.allowManualPumpingStationFetching = ((GameObject) data).GetComponent<BottleEmptier>().allowManualPumpingStationFetching;
    this.smi.RefreshChore();
  }

  public class StatesInstance : 
    GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.GameInstance
  {
    [MyCmpGet]
    public Storage storage;
    private FetchChore chore;

    public MeterController meter { get; private set; }

    public StatesInstance(BottleEmptier smi)
      : base(smi)
    {
      this.master.GetComponent<TreeFilterable>().OnFilterChanged += new Action<HashSet<Tag>>(this.OnFilterChanged);
      this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", nameof (meter), Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
      {
        "meter_target",
        "meter_arrow",
        "meter_scale"
      });
      this.meter.meterController.GetComponent<KBatchedAnimTracker>().synchronizeEnabledState = false;
      this.meter.meterController.enabled = false;
      this.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
      this.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
    }

    public void CreateChore()
    {
      HashSet<Tag> tags = this.GetComponent<TreeFilterable>().GetTags();
      Tag[] forbidden_tags;
      if (!this.master.allowManualPumpingStationFetching)
        forbidden_tags = new Tag[2]
        {
          GameTags.LiquidSource,
          GameTags.GasSource
        };
      else
        forbidden_tags = new Tag[0];
      Storage component = this.GetComponent<Storage>();
      this.chore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, component, component.Capacity(), tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags);
    }

    public void CancelChore()
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Storage Changed");
      this.chore = (FetchChore) null;
    }

    public void RefreshChore() => this.GoTo((StateMachine.BaseState) this.sm.unoperational);

    private void OnFilterChanged(HashSet<Tag> tags) => this.RefreshChore();

    private void OnStorageChange(object data)
    {
      this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.RemainingCapacity() / this.storage.capacityKg));
      this.meter.meterController.enabled = (double) this.storage.ExactMassStored() > 0.0;
    }

    private void OnOnlyFetchMarkedItemsSettingChanged(object data) => this.RefreshChore();

    public void StartMeter()
    {
      PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
      if ((UnityEngine.Object) firstPrimaryElement == (UnityEngine.Object) null)
        return;
      this.GetComponent<KBatchedAnimController>().SetSymbolTint(new KAnimHashedString("leak_ceiling"), (Color) firstPrimaryElement.Element.substance.colour);
      this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
      this.meter.meterController.Play((HashedString) "empty", KAnim.PlayMode.Paused);
      Color32 colour = firstPrimaryElement.Element.substance.colour with
      {
        a = byte.MaxValue
      };
      GameUtil.TintLiquidSymbolOnBuilding("meter_fill", this.meter.meterController, firstPrimaryElement.Element);
      GameUtil.TintLiquidSymbolOnBuilding("water1", this.meter.meterController, firstPrimaryElement.Element);
      GameUtil.TintLiquidSymbolOnBuilding("substance_tinter", this.meter.meterController, firstPrimaryElement.Element);
      this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
      this.OnStorageChange((object) null);
    }

    private PrimaryElement GetFirstPrimaryElement()
    {
      for (int idx = 0; idx < this.storage.Count; ++idx)
      {
        GameObject gameObject = this.storage[idx];
        if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
        {
          PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
          if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && (double) component.Mass > 0.0)
            return component;
        }
      }
      return (PrimaryElement) null;
    }

    public void Emit(float dt)
    {
      if (!this.smi.master.emit)
        return;
      PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
      if ((UnityEngine.Object) firstPrimaryElement == (UnityEngine.Object) null)
        return;
      float amount = Mathf.Min(firstPrimaryElement.Mass, this.master.emptyRate * dt);
      if ((double) amount <= 0.0)
        return;
      float amount_consumed;
      SimUtil.DiseaseInfo disease_info;
      float aggregate_temperature;
      this.storage.ConsumeAndGetDisease(firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag, amount, out amount_consumed, out disease_info, out aggregate_temperature);
      Vector3 position = this.transform.GetPosition();
      position.y += 1.8f;
      bool flag = this.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH;
      position.x += flag ? -0.2f : 0.2f;
      int num = Grid.PosToCell(position) + (flag ? -1 : 1);
      if (Grid.Solid[num])
        num += flag ? 1 : -1;
      Element element = firstPrimaryElement.Element;
      ushort idx = element.idx;
      if (element.IsLiquid)
        FallingWater.instance.AddParticle(num, idx, amount_consumed, aggregate_temperature, disease_info.idx, disease_info.count, true);
      else
        SimMessages.ModifyCell(num, idx, aggregate_temperature, amount_consumed, disease_info.idx, disease_info.count);
    }
  }

  public class States : 
    GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier>
  {
    private StatusItem statusItem;
    public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State unoperational;
    public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State waitingfordelivery;
    public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State emptying;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.waitingfordelivery;
      this.statusItem = new StatusItem(nameof (BottleEmptier), "", "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.statusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        BottleEmptier bottleEmptier = (BottleEmptier) data;
        if ((UnityEngine.Object) bottleEmptier == (UnityEngine.Object) null)
          return str;
        return bottleEmptier.allowManualPumpingStationFetching ? (string) (bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.NAME) : (string) (bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.NAME);
      });
      this.statusItem.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        BottleEmptier bottleEmptier = (BottleEmptier) data;
        if ((UnityEngine.Object) bottleEmptier == (UnityEngine.Object) null)
          return str;
        return !bottleEmptier.allowManualPumpingStationFetching ? (!bottleEmptier.isGasEmptier ? (string) BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.TOOLTIP : (string) BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.TOOLTIP) : (!bottleEmptier.isGasEmptier ? (string) BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.TOOLTIP : (string) BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.TOOLTIP);
      });
      this.root.ToggleStatusItem(this.statusItem, (Func<BottleEmptier.StatesInstance, object>) (smi => (object) smi.master));
      this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery).PlayAnim("off");
      this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.emptying, (StateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.Transition.ConditionCallback) (smi => (double) smi.GetComponent<Storage>().ExactMassStored() > 0.0)).Enter("CreateChore", (StateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State.Callback) (smi => smi.CreateChore())).Exit("CancelChore", (StateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State.Callback) (smi => smi.CancelChore())).PlayAnim("on");
      this.emptying.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (StateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.Transition.ConditionCallback) (smi => (double) smi.GetComponent<Storage>().ExactMassStored() == 0.0)).Enter("StartMeter", (StateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State.Callback) (smi => smi.StartMeter())).Update("Emit", (Action<BottleEmptier.StatesInstance, float>) ((smi, dt) => smi.Emit(dt))).PlayAnim("working_loop", KAnim.PlayMode.Loop);
    }
  }
}
