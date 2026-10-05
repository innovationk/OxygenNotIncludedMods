// Decompiled with JetBrains decompiler
// Type: PlantBranch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class PlantBranch : 
  GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>
{
  private StateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.TargetParameter Trunk;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.root;
  }

  public class Def : StateMachine.BaseDef
  {
    public System.Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback;
    public System.Action<PlantBranch.Instance> onEarlySpawn;
  }

  public new class Instance : 
    GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.GameInstance,
    IWiltCause
  {
    public PlantBranchGrower.Instance trunk;
    private int trunkWiltHandle = -1;
    private int trunkWiltRecoverHandle = -1;

    public bool HasTrunk
    {
      get => this.trunk != null && !this.trunk.IsNullOrDestroyed() && !this.trunk.isMasterNull;
    }

    public Instance(IStateMachineTarget master, PlantBranch.Def def)
      : base(master, def)
    {
      this.SetOccupyGridSpace(true);
      this.Subscribe(1272413801, new System.Action<object>(this.OnHarvest));
    }

    public override void StartSM()
    {
      base.StartSM();
      System.Action<PlantBranch.Instance> onEarlySpawn = this.def.onEarlySpawn;
      if (onEarlySpawn != null)
        onEarlySpawn(this);
      this.trunk = this.GetTrunk();
      if (!this.HasTrunk)
      {
        Debug.LogWarning((object) "Tree Branch loaded with missing trunk reference. Destroying...");
        Util.KDestroyGameObject(this.gameObject);
      }
      else
      {
        this.SubscribeToTrunk();
        System.Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = this.def.animationSetupCallback;
        if (animationSetupCallback == null)
          return;
        animationSetupCallback(this.trunk, this);
      }
    }

    private void OnHarvest(object data)
    {
      if (!this.HasTrunk)
        return;
      this.trunk.OnBrancHarvested(this);
    }

    protected override void OnCleanUp()
    {
      this.UnsubscribeToTrunk();
      this.SetOccupyGridSpace(false);
      base.OnCleanUp();
    }

    private void SetOccupyGridSpace(bool active)
    {
      int cell = Grid.PosToCell(this.gameObject);
      if (active)
      {
        GameObject gameObject = Grid.Objects[cell, 5];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) this.gameObject)
        {
          Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "PlantBranch.SetOccupyGridSpace already occupied by {0}", (object) gameObject);
          Util.KDestroyGameObject(this.gameObject);
        }
        else
          Grid.Objects[cell, 5] = this.gameObject;
      }
      else
      {
        if (!((UnityEngine.Object) Grid.Objects[cell, 5] == (UnityEngine.Object) this.gameObject))
          return;
        Grid.Objects[cell, 5] = (GameObject) null;
      }
    }

    public void SetTrunk(PlantBranchGrower.Instance trunk)
    {
      this.trunk = trunk;
      this.smi.sm.Trunk.Set(trunk.gameObject, this, false);
      this.SubscribeToTrunk();
      System.Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = this.def.animationSetupCallback;
      if (animationSetupCallback == null)
        return;
      animationSetupCallback(trunk, this);
    }

    public PlantBranchGrower.Instance GetTrunk()
    {
      return this.smi.sm.Trunk.IsNull(this) ? (PlantBranchGrower.Instance) null : this.sm.Trunk.Get(this).GetSMI<PlantBranchGrower.Instance>();
    }

    private void SubscribeToTrunk()
    {
      if (!this.HasTrunk)
        return;
      if (this.trunkWiltHandle == -1)
        this.trunkWiltHandle = this.trunk.gameObject.Subscribe(-724860998, new System.Action<object>(this.OnTrunkWilt));
      if (this.trunkWiltRecoverHandle == -1)
        this.trunkWiltRecoverHandle = this.trunk.gameObject.Subscribe(712767498, new System.Action<object>(this.OnTrunkRecover));
      this.BoxingTrigger(912965142, !this.trunk.GetComponent<WiltCondition>().IsWilting());
      this.GetComponent<ReceptacleMonitor>().SetReceptacle(this.trunk.GetComponent<ReceptacleMonitor>().GetReceptacle());
      this.trunk.RefreshBranchZPositionOffset(this.gameObject);
      this.GetComponent<BudUprootedMonitor>().SetParentObject(this.trunk.GetComponent<KPrefabID>());
    }

    private void UnsubscribeToTrunk()
    {
      if (!this.HasTrunk)
        return;
      this.trunk.gameObject.Unsubscribe(this.trunkWiltHandle);
      this.trunk.gameObject.Unsubscribe(this.trunkWiltRecoverHandle);
      this.trunkWiltHandle = -1;
      this.trunkWiltRecoverHandle = -1;
      this.trunk.OnBranchRemoved(this.gameObject);
    }

    private void OnTrunkWilt(object data = null) => this.BoxingTrigger(912965142, false);

    private void OnTrunkRecover(object data = null) => this.BoxingTrigger(912965142, true);

    public string WiltStateString => "    • " + (string) DUPLICANTS.STATS.TRUNKHEALTH.NAME;

    public WiltCondition.Condition[] Conditions
    {
      get
      {
        return new WiltCondition.Condition[1]
        {
          WiltCondition.Condition.UnhealthyRoot
        };
      }
    }
  }
}
