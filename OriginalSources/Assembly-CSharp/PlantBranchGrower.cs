// Decompiled with JetBrains decompiler
// Type: PlantBranchGrower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlantBranchGrower : 
  PlantBranchGrowerBase<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>
{
  public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State worldgen;
  public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State wilt;
  public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State maturing;
  public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State growingBranches;
  public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State fullyGrown;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.wilt;
    this.worldgen.Update(new System.Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.WorldGenUpdate), UpdateRate.RENDER_EVERY_TICK);
    this.wilt.TagTransition(GameTags.Wilting, this.maturing, true);
    this.maturing.TagTransition(GameTags.Wilting, this.wilt).EnterTransition(this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature)).EventTransition(GameHashes.Grow, this.growingBranches);
    this.growingBranches.TagTransition(GameTags.Wilting, this.wilt).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.fullyGrown, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.AllBranchesCreated)).ToggleStatusItem((Func<PlantBranchGrower.Instance, StatusItem>) (smi => smi.def.growingBranchesStatusItem)).Update(new System.Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.GrowBranchUpdate), UpdateRate.SIM_4000ms);
    this.fullyGrown.TagTransition(GameTags.Wilting, this.wilt).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.NotAllBranchesCreated));
  }

  public static bool NotAllBranchesCreated(PlantBranchGrower.Instance smi)
  {
    return smi.CurrentBranchCount < smi.MaxBranchesAllowedAtOnce;
  }

  public static bool AllBranchesCreated(PlantBranchGrower.Instance smi)
  {
    return smi.CurrentBranchCount >= smi.MaxBranchesAllowedAtOnce;
  }

  public static bool IsMature(PlantBranchGrower.Instance smi) => smi.IsGrown;

  public static void GrowBranchUpdate(PlantBranchGrower.Instance smi, float dt)
  {
    smi.SpawnRandomBranch();
  }

  public static void WorldGenUpdate(PlantBranchGrower.Instance smi, float dt)
  {
    float growth_percentage = UnityEngine.Random.Range(0.0f, 1f);
    if (smi.SpawnRandomBranch(growth_percentage))
      return;
    smi.GoTo(smi.sm.defaultState);
  }

  public class Def : 
    PlantBranchGrowerBase<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.PlantBranchGrowerBaseDef
  {
    public CellOffset[] BRANCH_OFFSETS;
    public bool harvestOnDrown;
    public bool propagateHarvestDesignation = true;
    public Func<int, bool> additionalBranchGrowRequirements;
    public System.Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested;
    public System.Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned;
    public StatusItem growingBranchesStatusItem = Db.Get().MiscStatusItems.GrowingBranches;
    public System.Action<PlantBranchGrower.Instance> onEarlySpawn;
  }

  public new class Instance : 
    GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.GameInstance
  {
    private IManageGrowingStates growing;
    [MyCmpGet]
    private UprootedMonitor uprootMonitor;
    [Serialize]
    private Ref<KPrefabID>[] branches;
    private static List<int> spawn_choices = new List<int>();

    public bool IsUprooted
    {
      get => (UnityEngine.Object) this.uprootMonitor != (UnityEngine.Object) null && this.uprootMonitor.IsUprooted;
    }

    public bool IsGrown => this.growing == null || (double) this.growing.PercentGrown() >= 1.0;

    public int MaxBranchesAllowedAtOnce
    {
      get
      {
        return this.def.MAX_BRANCH_COUNT >= 0 ? Mathf.Min(this.def.MAX_BRANCH_COUNT, this.def.BRANCH_OFFSETS.Length) : this.def.BRANCH_OFFSETS.Length;
      }
    }

    public int CurrentBranchCount
    {
      get
      {
        int currentBranchCount = 0;
        if (this.branches != null)
        {
          int num = 0;
          while (num < this.branches.Length)
            currentBranchCount += (UnityEngine.Object) this.GetBranch(num++) != (UnityEngine.Object) null ? 1 : 0;
        }
        return currentBranchCount;
      }
    }

    public GameObject GetBranch(int idx)
    {
      if (this.branches != null && this.branches[idx] != null)
      {
        KPrefabID kprefabId = this.branches[idx].Get();
        if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null)
          return kprefabId.gameObject;
      }
      return (GameObject) null;
    }

    protected override void OnCleanUp()
    {
      this.SetTrunkOccupyingCellsAsPlant(false);
      base.OnCleanUp();
    }

    public Instance(IStateMachineTarget master, PlantBranchGrower.Def def)
      : base(master, def)
    {
      this.growing = this.GetComponent<IManageGrowingStates>();
      this.growing = this.growing != null ? this.growing : this.gameObject.GetSMI<IManageGrowingStates>();
      this.SetTrunkOccupyingCellsAsPlant(true);
      this.Subscribe(1119167081, new System.Action<object>(this.OnNewGameSpawn));
      this.Subscribe(144050788, new System.Action<object>(this.OnUpdateRoom));
    }

    public override void StartSM()
    {
      base.StartSM();
      System.Action<PlantBranchGrower.Instance> onEarlySpawn = this.def.onEarlySpawn;
      if (onEarlySpawn != null)
        onEarlySpawn(this);
      this.DefineBranchArray();
      this.Subscribe(-216549700, new System.Action<object>(this.OnUprooted));
      this.Subscribe(-266953818, (System.Action<object>) (obj => this.UpdateAutoHarvestValue()));
      if (!this.def.harvestOnDrown)
        return;
      this.Subscribe(-750750377, new System.Action<object>(this.OnUprooted));
    }

    private void OnUpdateRoom(object data)
    {
      if (this.branches == null)
        return;
      this.ActionPerBranch((System.Action<GameObject>) (branch => branch.Trigger(144050788, data)));
    }

    private void SetTrunkOccupyingCellsAsPlant(bool doSet)
    {
      CellOffset[] occupiedCellsOffsets = this.GetComponent<OccupyArea>().OccupiedCellsOffsets;
      int cell1 = Grid.PosToCell(this.gameObject);
      for (int index = 0; index < occupiedCellsOffsets.Length; ++index)
      {
        int cell2 = Grid.OffsetCell(cell1, occupiedCellsOffsets[index]);
        if (doSet)
          Grid.Objects[cell2, 5] = this.gameObject;
        else if ((UnityEngine.Object) Grid.Objects[cell2, 5] == (UnityEngine.Object) this.gameObject)
          Grid.Objects[cell2, 5] = (GameObject) null;
      }
    }

    private void OnNewGameSpawn(object data)
    {
      this.DefineBranchArray();
      float percentage = 1f;
      if ((double) UnityEngine.Random.value < 0.1)
        percentage = UnityEngine.Random.Range(0.75f, 0.99f);
      else
        this.GoTo((StateMachine.BaseState) this.sm.worldgen);
      this.growing.OverrideMaturityLevel(percentage);
    }

    public void ManuallyDefineBranchArray(KPrefabID[] _branches)
    {
      this.DefineBranchArray();
      for (int index = 0; index < Mathf.Min(this.branches.Length, _branches.Length); ++index)
      {
        KPrefabID branch = _branches[index];
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
        {
          if (this.branches[index] == null)
            this.branches[index] = new Ref<KPrefabID>();
          this.branches[index].Set(branch);
        }
        else
          this.branches[index] = (Ref<KPrefabID>) null;
      }
    }

    private void DefineBranchArray()
    {
      if (this.branches != null)
        return;
      this.branches = new Ref<KPrefabID>[this.def.BRANCH_OFFSETS.Length];
    }

    public void ActionPerBranch(System.Action<GameObject> action)
    {
      for (int idx = 0; idx < this.branches.Length; ++idx)
      {
        GameObject branch = this.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null && action != null)
          action(branch.gameObject);
      }
    }

    public GameObject[] GetExistingBranches()
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int idx = 0; idx < this.branches.Length; ++idx)
      {
        GameObject branch = this.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
          gameObjectList.Add(branch.gameObject);
      }
      return gameObjectList.ToArray();
    }

    public void OnBranchRemoved(GameObject _branch)
    {
      for (int idx = 0; idx < this.branches.Length; ++idx)
      {
        GameObject branch = this.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null && (UnityEngine.Object) branch == (UnityEngine.Object) _branch)
          this.branches[idx] = (Ref<KPrefabID>) null;
      }
      this.gameObject.Trigger(-1586842875);
    }

    public void OnBrancHarvested(PlantBranch.Instance branch)
    {
      System.Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested = this.def.onBranchHarvested;
      if (onBranchHarvested == null)
        return;
      onBranchHarvested(branch, this);
    }

    private void OnUprooted(object data = null)
    {
      for (int idx = 0; idx < this.branches.Length; ++idx)
      {
        GameObject branch = this.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
          branch.Trigger(-216549700);
      }
    }

    public List<int> GetAvailableSpawnPositions()
    {
      PlantBranchGrower.Instance.spawn_choices.Clear();
      int cell1 = Grid.PosToCell((StateMachine.Instance) this);
      for (int idx = 0; idx < this.def.BRANCH_OFFSETS.Length; ++idx)
      {
        int cell2 = Grid.OffsetCell(cell1, this.def.BRANCH_OFFSETS[idx]);
        if ((UnityEngine.Object) this.GetBranch(idx) == (UnityEngine.Object) null && this.CanBranchGrowInCell(cell2))
          PlantBranchGrower.Instance.spawn_choices.Add(idx);
      }
      return PlantBranchGrower.Instance.spawn_choices;
    }

    public void RefreshBranchZPositionOffset(GameObject _branch)
    {
      if (this.branches == null)
        return;
      for (int idx = 0; idx < this.branches.Length; ++idx)
      {
        GameObject branch = this.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null && (UnityEngine.Object) branch == (UnityEngine.Object) _branch)
        {
          Vector3 position = branch.transform.position with
          {
            z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.8f / (float) this.branches.Length * (float) idx
          };
          branch.transform.SetPosition(position);
        }
      }
    }

    public bool SpawnRandomBranch(float growth_percentage = 0.0f)
    {
      if (this.IsUprooted || this.CurrentBranchCount >= this.MaxBranchesAllowedAtOnce)
        return false;
      List<int> availableSpawnPositions = this.GetAvailableSpawnPositions();
      availableSpawnPositions.Shuffle<int>();
      if (availableSpawnPositions.Count <= 0)
        return false;
      PlantBranch.Instance data = this.SpawnBranchAtIndex(availableSpawnPositions[0]);
      (data.GetComponent<IManageGrowingStates>() ?? data.gameObject.GetSMI<IManageGrowingStates>())?.OverrideMaturityLevel(growth_percentage);
      data.StartSM();
      this.gameObject.Trigger(-1586842875, (object) data);
      System.Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned = this.def.onBranchSpawned;
      if (onBranchSpawned != null)
        onBranchSpawned(data, this);
      return true;
    }

    private PlantBranch.Instance SpawnBranchAtIndex(int idx)
    {
      if (idx < 0 || idx >= this.branches.Length)
        return (PlantBranch.Instance) null;
      GameObject branch = this.GetBranch(idx);
      if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
        return branch.GetSMI<PlantBranch.Instance>();
      Vector3 posCbc = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) this), this.def.BRANCH_OFFSETS[idx]), Grid.SceneLayer.BuildingFront);
      GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) this.def.BRANCH_PREFAB_NAME), posCbc);
      go.SetActive(true);
      PlantBranch.Instance smi = go.GetSMI<PlantBranch.Instance>();
      MutantPlant component1 = this.GetComponent<MutantPlant>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        MutantPlant component2 = smi.GetComponent<MutantPlant>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          component1.CopyMutationsTo(component2);
          PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = component2.GetSubSpeciesInfo();
          PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(subSpeciesInfo, component2);
          PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(subSpeciesInfo.ID);
        }
      }
      this.UpdateAutoHarvestValue(smi);
      smi.SetTrunk(this);
      this.branches[idx] = new Ref<KPrefabID>();
      this.branches[idx].Set(smi.GetComponent<KPrefabID>());
      return smi;
    }

    private bool CanBranchGrowInCell(int cell)
    {
      if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || (UnityEngine.Object) Grid.Objects[cell, 1] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[cell, 5] != (UnityEngine.Object) null || Grid.Foundation[cell])
        return false;
      int cell1 = Grid.CellAbove(cell);
      return Grid.IsValidCell(cell1) && !Grid.IsSubstantialLiquid(cell1) && (this.def.additionalBranchGrowRequirements == null || this.def.additionalBranchGrowRequirements(cell));
    }

    public void UpdateAutoHarvestValue(PlantBranch.Instance specificBranch = null)
    {
      HarvestDesignatable component1 = this.GetComponent<HarvestDesignatable>();
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || this.branches == null)
        return;
      if (specificBranch != null)
      {
        HarvestDesignatable component2 = specificBranch.GetComponent<HarvestDesignatable>();
        if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
          return;
        component2.SetHarvestWhenReady(component1.HarvestWhenReady);
      }
      else
      {
        if (!this.def.propagateHarvestDesignation)
          return;
        for (int idx = 0; idx < this.branches.Length; ++idx)
        {
          GameObject branch = this.GetBranch(idx);
          if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
          {
            HarvestDesignatable component3 = branch.GetComponent<HarvestDesignatable>();
            if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
              component3.SetHarvestWhenReady(component1.HarvestWhenReady);
          }
        }
      }
    }
  }
}
