// Decompiled with JetBrains decompiler
// Type: RoboPilotModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class RoboPilotModule : KMonoBehaviour
{
  private MeterController meter;
  private Storage databankStorage;
  private ManualDeliveryKG manualDeliveryChore;
  public int dataBankConsumption = 2;
  public bool consumeDataBanksOnLand;
  private static CellOffset[] dataDeliveryOffsets = new CellOffset[7]
  {
    new CellOffset(0, 0),
    new CellOffset(1, 0),
    new CellOffset(2, 0),
    new CellOffset(3, 0),
    new CellOffset(-1, 0),
    new CellOffset(-2, 0),
    new CellOffset(-3, 0)
  };

  protected override void OnSpawn()
  {
    this.databankStorage = this.GetComponent<Storage>();
    this.manualDeliveryChore = this.GetComponent<ManualDeliveryKG>();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_fill",
      "meter_frame"
    });
    this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
    this.UpdateMeter();
    this.databankStorage.SetOffsets(RoboPilotModule.dataDeliveryOffsets);
    this.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
    this.Subscribe(-778359855, new Action<object>(this.PlayDeliveryAnimation));
    this.Subscribe(-887025858, new Action<object>(this.OnRocketLanded));
    RocketModuleCluster component = this.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.CraftInterface.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
      component.CraftInterface.Subscribe(543433792, new Action<object>(this.RequestDataBanksForDestination));
    }
    else
    {
      this.Subscribe(705820818, new Action<object>(this.OnRocketLaunched));
      this.GetComponent<RocketModule>().FindLaunchConditionManager().Subscribe(929158128, new Action<object>(this.RequestDataBanksForDestination));
    }
    this.RequestDataBanksForDestination();
  }

  private void RequestDataBanksForDestination(object _ = null)
  {
    int distance = -1;
    RocketModuleCluster component1 = this.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      ClusterTraveler component2 = component1.CraftInterface.GetComponent<ClusterTraveler>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.CurrentPath != null)
        distance = component2.RemainingTravelNodes() * 2;
    }
    else
    {
      LaunchConditionManager conditionManager = this.GetComponent<RocketModule>().FindLaunchConditionManager();
      if ((UnityEngine.Object) conditionManager != (UnityEngine.Object) null)
      {
        SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(conditionManager);
        if (spacecraftDestination != null)
          distance = spacecraftDestination.OneBasedDistance * 2;
      }
    }
    if (distance <= 0 || this.HasResourcesToMove(distance))
      return;
    this.manualDeliveryChore.refillMass = MathF.Min(this.ResourcesRequiredToMove(distance), this.databankStorage.Capacity() - this.databankStorage.UnitsStored());
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe(-1697596308, new Action<object>(this.UpdateMeter));
    this.Unsubscribe(-887025858, new Action<object>(this.OnRocketLanded));
    this.Unsubscribe(-778359855, new Action<object>(this.PlayDeliveryAnimation));
    RocketModuleCluster component = this.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.CraftInterface.Unsubscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
      component.CraftInterface.Unsubscribe(543433792, new Action<object>(this.RequestDataBanksForDestination));
    }
    else
    {
      this.Unsubscribe(705820818, new Action<object>(this.OnRocketLaunched));
      this.GetComponent<RocketModule>().FindLaunchConditionManager().Unsubscribe(929158128, new Action<object>(this.RequestDataBanksForDestination));
    }
    base.OnCleanUp();
  }

  private void OnLaunchConditionChanged(object data)
  {
    RocketModuleCluster component = this.GetComponent<RocketModuleCluster>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.CraftInterface.IsLaunchRequested())
      return;
    component.CraftInterface.GetComponent<Clustercraft>().Launch();
  }

  private void OnRocketLanded(object o)
  {
    if (this.consumeDataBanksOnLand)
    {
      LaunchConditionManager conditionManager1 = this.GetComponent<RocketModule>().FindLaunchConditionManager();
      Spacecraft conditionManager2 = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(conditionManager1);
      this.databankStorage.ConsumeIgnoringDisease(DatabankHelper.TAG, Math.Min((float) (SpacecraftManager.instance.GetSpacecraftDestination(conditionManager2.id).OneBasedDistance * this.dataBankConsumption * 2), this.databankStorage.MassStored()));
    }
    this.RequestDataBanksForDestination();
  }

  private void OnRocketLaunched(object o)
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) "launch_pre");
    component.Queue((HashedString) "launch");
    component.Queue((HashedString) "launch_pst");
  }

  public void ConsumeDataBanksInFlight()
  {
    if (!((UnityEngine.Object) this.databankStorage != (UnityEngine.Object) null))
      return;
    this.databankStorage.ConsumeIgnoringDisease(DatabankHelper.TAG, (float) this.dataBankConsumption);
  }

  private void PlayDeliveryAnimation(object data = null)
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    HashedString currentAnim = component.currentAnim;
    component.Play((HashedString) "databank_delivery_reaction");
    component.Queue(currentAnim);
  }

  private void UpdateMeter(object data = null)
  {
    this.meter.SetPositionPercent(this.databankStorage.MassStored() / this.databankStorage.Capacity());
  }

  public bool HasResourcesToMove(int distance)
  {
    return (double) this.databankStorage.UnitsStored() >= (double) (distance * this.dataBankConsumption);
  }

  public float ResourcesRequiredToMove(int distance)
  {
    return (float) (distance * this.dataBankConsumption);
  }

  public bool IsFull()
  {
    return (double) this.databankStorage.MassStored() >= (double) this.databankStorage.Capacity();
  }

  public float GetDataBanksStored()
  {
    return !((UnityEngine.Object) this.databankStorage != (UnityEngine.Object) null) ? 0.0f : this.databankStorage.UnitsStored();
  }

  public float GetDataBankRange()
  {
    if ((UnityEngine.Object) this.databankStorage == (UnityEngine.Object) null)
      return 0.0f;
    return this.consumeDataBanksOnLand ? this.databankStorage.UnitsStored() / (float) this.dataBankConsumption * RoboPilotCommandModuleConfig.DATABANKRANGE : (float) ((double) this.databankStorage.UnitsStored() / (double) this.dataBankConsumption * 600.0);
  }

  public float GetMaxDataBankRange()
  {
    if ((UnityEngine.Object) this.databankStorage == (UnityEngine.Object) null)
      return 0.0f;
    return this.consumeDataBanksOnLand ? this.databankStorage.Capacity() / (float) this.dataBankConsumption * RoboPilotCommandModuleConfig.DATABANKRANGE : (float) ((double) this.databankStorage.Capacity() / (double) this.dataBankConsumption * 600.0);
  }
}
