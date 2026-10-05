// Decompiled with JetBrains decompiler
// Type: ArtifactPOIStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIStates")]
public class ArtifactPOIStates : 
  GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>
{
  public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State destroyOnArtifactSpawned;
  public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State enter;
  public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State waitingForPickup;
  public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State recharging;
  public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State spawnArtifact;
  public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Signal OnHexCellInventoryChangedSignal;
  public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter poiCharge = new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter(1f);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.enter;
    this.root.Enter((StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State.Callback) (smi =>
    {
      if (smi.configuration != null && !(smi.configuration.typeId == HashedString.Invalid))
        return;
      smi.configuration = smi.GetComponent<ArtifactPOIConfigurator>().MakeConfiguration();
      smi.poiCharge = 1f;
    }));
    this.enter.ParamTransition<float>((StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>) this.poiCharge, this.spawnArtifact, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>.Callback(ArtifactPOIStates.IsFullyCharged)).ParamTransition<float>((StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>) this.poiCharge, this.waitingForPickup, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>.Callback(ArtifactPOIStates.IsNotFullyCharge));
    this.spawnArtifact.Enter(new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State.Callback(ArtifactPOIStates.SpawnArtifactOnHexCellIfFullyCharged)).EnterGoTo(this.waitingForPickup);
    this.waitingForPickup.OnSignal(this.OnHexCellInventoryChangedSignal, this.recharging, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter>.Callback(ArtifactPOIStates.ThereIsNoArtifactInHexCell)).EnterTransition(this.destroyOnArtifactSpawned, (StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Transition.ConditionCallback) (smi => ArtifactPOIStates.MarkedForDestroyAfterArtifactSpawned(smi) && ArtifactPOIStates.IsArtifactAvailableInHexCell(smi)));
    this.recharging.OnSignal(this.OnHexCellInventoryChangedSignal, this.waitingForPickup, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter>.Callback(ArtifactPOIStates.IsArtifactAvailableInHexCell)).ParamTransition<float>((StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>) this.poiCharge, this.spawnArtifact, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>.Callback(ArtifactPOIStates.IsFullyCharged)).EventHandler(GameHashes.NewDay, (Func<ArtifactPOIStates.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State.Callback(ArtifactPOIStates.AddDayWothOfCharge));
    this.destroyOnArtifactSpawned.Enter(new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State.Callback(ArtifactPOIStates.SelfDestroy));
  }

  public static bool IsNotFullyCharge(ArtifactPOIStates.Instance smi, float f)
  {
    return !ArtifactPOIStates.IsFullyCharge(smi);
  }

  public static bool IsNotFullyCharge(ArtifactPOIStates.Instance smi)
  {
    return !ArtifactPOIStates.IsFullyCharge(smi);
  }

  public static bool IsFullyCharge(ArtifactPOIStates.Instance smi)
  {
    return (double) smi.sm.poiCharge.Get(smi) >= 1.0;
  }

  public static bool IsFullyCharged(ArtifactPOIStates.Instance smi, float f)
  {
    return (double) smi.sm.poiCharge.Get(smi) >= 1.0;
  }

  public static bool ThereIsNoArtifactInHexCell(
    ArtifactPOIStates.Instance smi,
    StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter param)
  {
    return ArtifactPOIStates.ThereIsNoArtifactInHexCell(smi);
  }

  public static bool ThereIsNoArtifactInHexCell(ArtifactPOIStates.Instance smi)
  {
    return !smi.HasArtifactAvailableInHexCell();
  }

  public static bool IsArtifactAvailableInHexCell(
    ArtifactPOIStates.Instance smi,
    StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter param)
  {
    return ArtifactPOIStates.IsArtifactAvailableInHexCell(smi);
  }

  public static bool IsArtifactAvailableInHexCell(ArtifactPOIStates.Instance smi)
  {
    return smi.HasArtifactAvailableInHexCell();
  }

  public static bool MarkedForDestroyAfterArtifactSpawned(ArtifactPOIStates.Instance smi)
  {
    return smi.configuration.DestroyOnHarvest();
  }

  public static void ResetRechargeProgress(ArtifactPOIStates.Instance smi) => smi.poiCharge = 0.0f;

  public static void IncreaseArtifactSpawnedCount(ArtifactPOIStates.Instance smi)
  {
    smi.IncreaseArtifactsSpawnedCount();
  }

  public static void SelfDestroy(ArtifactPOIStates.Instance smi) => smi.gameObject.DeleteObject();

  public static void AddDayWothOfCharge(ArtifactPOIStates.Instance smi) => smi.RechargePOI(600f);

  public static void SpawnArtifactOnHexCellIfFullyCharged(ArtifactPOIStates.Instance smi)
  {
    if (!ArtifactPOIStates.IsFullyCharge(smi))
      return;
    smi.SpawnArtifactOnHexCell();
    ArtifactPOIStates.ResetRechargeProgress(smi);
    ArtifactPOIStates.IncreaseArtifactSpawnedCount(smi);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget target, ArtifactPOIStates.Def def) : 
    GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.GameInstance(target, def),
    IGameObjectEffectDescriptor
  {
    [Serialize]
    public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration configuration;
    [Serialize]
    private float _poiCharge;
    [Serialize]
    private int numHarvests;
    [Serialize]
    public string artifactToHarvest;

    public StarmapHexCellInventory HexCellInventory => this.GetHexCellInventory();

    public void IncreaseArtifactsSpawnedCount() => ++this.numHarvests;

    public float poiCharge
    {
      get => this._poiCharge;
      set
      {
        this._poiCharge = value;
        double num = (double) this.smi.sm.poiCharge.Set(value, this.smi);
      }
    }

    public override void StartSM()
    {
      this.HexCellInventory.Subscribe(-1697596308, new System.Action<object>(this.OnHexCellInventoryChanged));
      base.StartSM();
    }

    protected override void OnCleanUp()
    {
      this.HexCellInventory.Unsubscribe(-1697596308, new System.Action<object>(this.OnHexCellInventoryChanged));
      base.OnCleanUp();
    }

    private void OnHexCellInventoryChanged(object o)
    {
      this.sm.OnHexCellInventoryChangedSignal.Trigger(this);
    }

    public StarmapHexCellInventory GetHexCellInventory()
    {
      ClusterGridEntity component = this.GetComponent<ClusterGridEntity>();
      return ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location);
    }

    public bool HasArtifactAvailableInHexCell()
    {
      return this.HexCellInventory.Items.Find((Predicate<StarmapHexCellInventory.SerializedItem>) (i => i.IsEntity && Assets.GetPrefab(i.ID).HasTag(GameTags.Artifact))) != null;
    }

    public void SpawnArtifactOnHexCell()
    {
      Tag itemID = (Tag) (this.artifactToHarvest != null ? this.artifactToHarvest : this.PickNewArtifactToHarvest());
      this.artifactToHarvest = (string) null;
      this.HexCellInventory.AddItem(itemID, 1f, Element.State.Vacuum);
    }

    public string PickNewArtifactToHarvest()
    {
      string artifactID;
      if (this.numHarvests <= 0 && !string.IsNullOrEmpty(this.configuration.GetArtifactID()))
      {
        artifactID = this.configuration.GetArtifactID();
        ArtifactSelector.Instance.ReserveArtifactID(artifactID);
      }
      else
        artifactID = ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Space);
      return artifactID;
    }

    public void RechargePOI(float dt)
    {
      this.poiCharge += dt / this.configuration.GetRechargeTime();
      this.poiCharge = Mathf.Min(1f, this.poiCharge);
    }

    public float RechargeTimeRemaining()
    {
      return (float) Mathf.CeilToInt((float) (((double) this.configuration.GetRechargeTime() - (double) this.configuration.GetRechargeTime() * (double) this.poiCharge) / 600.0)) * 600f;
    }

    public List<Descriptor> GetDescriptors(GameObject go) => new List<Descriptor>();
  }
}
