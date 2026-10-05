// Decompiled with JetBrains decompiler
// Type: PlantBranchGrowerBase`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class PlantBranchGrowerBase<StateMachineType, StateMachineInstanceType, MasterType, DefType> : 
  GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>
  where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>
  where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameInstance
  where MasterType : IStateMachineTarget
  where DefType : PlantBranchGrowerBase<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantBranchGrowerBaseDef
{
  public class PlantBranchGrowerBaseDef : StateMachine.BaseDef, IPlantBranchGrower
  {
    public int MAX_BRANCH_COUNT;
    public string BRANCH_PREFAB_NAME;

    public string GetPlantBranchPrefabName() => this.BRANCH_PREFAB_NAME;

    public int GetMaxBranchCount() => this.MAX_BRANCH_COUNT;
  }
}
