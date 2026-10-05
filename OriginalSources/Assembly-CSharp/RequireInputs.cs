// Decompiled with JetBrains decompiler
// Type: RequireInputs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireInputs")]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
  [SerializeField]
  private bool requirePower = true;
  [SerializeField]
  private bool requireConduit;
  public bool requireConduitHasMass = true;
  public RequireInputs.Requirements visualizeRequirements = RequireInputs.Requirements.All;
  private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);
  private static readonly Operational.Flag pipesHaveMass = new Operational.Flag(nameof (pipesHaveMass), Operational.Flag.Type.Requirement);
  private Guid noWireStatusGuid = Guid.Empty;
  private Guid needPowerStatusGuid = Guid.Empty;
  private Guid liquidConduitEmptyStatusGuid = Guid.Empty;
  private Guid gasConduitEmptyStatusGuid = Guid.Empty;
  private Guid noLiquidConduitStatusGuid = Guid.Empty;
  private Guid noGasConduitStatusGuid = Guid.Empty;
  private bool requirementsMet;
  private BuildingEnabledButton button;
  private IEnergyConsumer energy;
  public ConduitConsumer conduitConsumer;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpGet]
  private Operational operational;
  private bool previouslyConnectedOpFlag = true;
  private bool previouslySatisfiedOpFlag = true;

  public bool RequiresPower
  {
    get => this.requirePower;
    set => this.requirePower = value;
  }

  public bool RequiresInputConduit
  {
    get => this.requireConduit;
    set => this.requireConduit = value;
  }

  public void SetRequirements(bool power, bool conduit)
  {
    this.requirePower = power;
    this.requireConduit = conduit;
  }

  public bool RequirementsMet => this.requirementsMet;

  protected override void OnPrefabInit() => this.Bind();

  protected override void OnSpawn()
  {
    this.CheckRequirements();
    this.Bind();
  }

  [ContextMenu("Bind")]
  private void Bind()
  {
    if (this.requirePower)
    {
      this.energy = this.GetComponent<IEnergyConsumer>();
      this.button = this.GetComponent<BuildingEnabledButton>();
    }
    if (this.requireConduit && !(bool) (UnityEngine.Object) this.conduitConsumer)
      this.conduitConsumer = this.GetComponent<ConduitConsumer>();
    Operational component = this.GetComponent<Operational>();
    bool flag = (UnityEngine.Object) component != (UnityEngine.Object) null;
    this.previouslyConnectedOpFlag = flag && component.GetFlag(RequireInputs.inputConnectedFlag);
    this.previouslySatisfiedOpFlag = flag && component.GetFlag(RequireInputs.pipesHaveMass);
  }

  public void Sim200ms(float dt) => this.CheckRequirements();

  private void CheckRequirements()
  {
    bool flag1 = true;
    bool show1 = false;
    bool show2 = false;
    if (this.requirePower)
    {
      bool isConnected = this.energy.IsConnected;
      bool isPowered = this.energy.IsPowered;
      flag1 &= isPowered & isConnected;
      show1 = this.VisualizeRequirement(RequireInputs.Requirements.NeedPower) & isConnected && !isPowered && ((UnityEngine.Object) this.button == (UnityEngine.Object) null || this.button.IsEnabled);
      show2 = this.VisualizeRequirement(RequireInputs.Requirements.NoWire) && !isConnected;
    }
    int num1 = 0 | (flag1 == this.requirementsMet ? 0 : ((UnityEngine.Object) this.GetComponent<Light2D>() != (UnityEngine.Object) null ? 1 : 0));
    this.needPowerStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, this.needPowerStatusGuid, show1, (object) this);
    this.noWireStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, this.noWireStatusGuid, show2, (object) this);
    int num2 = !((UnityEngine.Object) this.conduitConsumer != (UnityEngine.Object) null) ? 0 : (this.conduitConsumer.conduitType == ConduitType.Liquid ? 1 : 0);
    bool flag2 = num2 != 0 && this.conduitConsumer.IsConnected;
    bool flag3 = num2 != 0 && this.conduitConsumer.IsSatisfied;
    bool flag4 = num2 != 0 && this.conduitConsumer.enabled && this.requireConduitHasMass && this.requireConduit && this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty);
    bool flag5 = num2 != 0 && this.conduitConsumer.enabled && this.requireConduit && this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected);
    bool flag6 = flag1 & (!flag4 | flag3) & (!flag5 | flag2);
    this.liquidConduitEmptyStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.LiquidPipeEmpty, this.liquidConduitEmptyStatusGuid, this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty) & flag4 && !flag3, (object) this);
    this.noLiquidConduitStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedLiquidIn, this.noLiquidConduitStatusGuid, this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected) & flag5 && !flag2, (object) this.conduitConsumer);
    if (num2 != 0)
    {
      bool flag7 = !flag5 | flag2;
      bool flag8 = !flag4 | flag3;
      if (flag7 != this.previouslyConnectedOpFlag)
      {
        this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag7);
        this.previouslyConnectedOpFlag = flag7;
      }
      if (flag8 != this.previouslySatisfiedOpFlag)
      {
        this.operational.SetFlag(RequireInputs.pipesHaveMass, flag8);
        this.previouslySatisfiedOpFlag = flag8;
      }
    }
    int num3 = !((UnityEngine.Object) this.conduitConsumer != (UnityEngine.Object) null) ? 0 : (this.conduitConsumer.conduitType == ConduitType.Gas ? 1 : 0);
    bool flag9 = num3 != 0 && this.conduitConsumer.IsConnected;
    bool flag10 = num3 != 0 && this.conduitConsumer.IsSatisfied;
    bool flag11 = num3 != 0 && this.conduitConsumer.enabled && this.requireConduitHasMass && this.requireConduit && this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty);
    bool flag12 = num3 != 0 && this.conduitConsumer.enabled && this.requireConduit && this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected);
    bool flag13 = flag6 & (!flag11 | flag10) & (!flag12 | flag9);
    this.gasConduitEmptyStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.GasPipeEmpty, this.gasConduitEmptyStatusGuid, this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty) & flag11 && !flag10, (object) this);
    this.noGasConduitStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedGasIn, this.noGasConduitStatusGuid, this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected) & flag12 && !flag9, (object) this.conduitConsumer);
    if (num3 != 0)
    {
      bool flag14 = !flag12 | flag9;
      bool flag15 = !flag11 | flag10;
      if (flag14 != this.previouslyConnectedOpFlag)
      {
        this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag14);
        this.previouslyConnectedOpFlag = flag14;
      }
      if (flag15 != this.previouslySatisfiedOpFlag)
      {
        this.operational.SetFlag(RequireInputs.pipesHaveMass, flag15);
        this.previouslySatisfiedOpFlag = flag15;
      }
    }
    this.requirementsMet = flag13;
    if (num1 == 0)
      return;
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject);
    if (roomOfGameObject == null)
      return;
    Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
  }

  public bool VisualizeRequirement(RequireInputs.Requirements r)
  {
    return (this.visualizeRequirements & r) == r;
  }

  [Flags]
  public enum Requirements
  {
    None = 0,
    NoWire = 1,
    NeedPower = 2,
    ConduitConnected = 4,
    ConduitEmpty = 8,
    AllPower = NeedPower | NoWire, // 0x00000003
    AllConduit = ConduitEmpty | ConduitConnected, // 0x0000000C
    All = AllConduit | AllPower, // 0x0000000F
  }
}
