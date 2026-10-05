// Decompiled with JetBrains decompiler
// Type: OvercrowdingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class OvercrowdingMonitor : 
  GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
  public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;
  public static readonly Tag[] CONFINEMENT_IMMUNITY_TAGS = new Tag[2]
  {
    GameTags.Creatures.Burrowed,
    GameTags.Creatures.Digger
  };
  private static readonly Dictionary<Tag, int> personalSpaces = new Dictionary<Tag, int>();
  private static readonly Dictionary<Tag, bool> isFish = new Dictionary<Tag, bool>();

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Update(new System.Action<OvercrowdingMonitor.Instance, float>(OvercrowdingMonitor.UpdateState), UpdateRate.SIM_1000ms, true);
  }

  private static int FetchCreaturePersonalSpace(KPrefabID creature)
  {
    int requiredPerCreature;
    if (OvercrowdingMonitor.personalSpaces.TryGetValue(creature.PrefabTag, out requiredPerCreature))
      return requiredPerCreature;
    OvercrowdingMonitor.Instance smi = creature.GetSMI<OvercrowdingMonitor.Instance>();
    if (smi != null)
      requiredPerCreature = smi.def.spaceRequiredPerCreature;
    OvercrowdingMonitor.personalSpaces[creature.PrefabTag] = requiredPerCreature;
    return requiredPerCreature;
  }

  private static int FetchEggPersonalSpace(KPrefabID egg)
  {
    int requiredPerCreature;
    if (OvercrowdingMonitor.personalSpaces.TryGetValue(egg.PrefabTag, out requiredPerCreature))
      return requiredPerCreature;
    IncubationMonitor.Instance smi = egg.GetSMI<IncubationMonitor.Instance>();
    if (OvercrowdingMonitor.personalSpaces.TryGetValue(smi.def.spawnedCreature, out requiredPerCreature))
    {
      OvercrowdingMonitor.personalSpaces[egg.PrefabTag] = requiredPerCreature;
      return requiredPerCreature;
    }
    GameObject prefab = Assets.GetPrefab(smi.def.spawnedCreature);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      requiredPerCreature = prefab.GetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature;
    OvercrowdingMonitor.personalSpaces[egg.PrefabTag] = requiredPerCreature;
    OvercrowdingMonitor.personalSpaces[smi.def.spawnedCreature] = requiredPerCreature;
    return requiredPerCreature;
  }

  private static void UpdateState(OvercrowdingMonitor.Instance smi, float dt)
  {
    OvercrowdingMonitor.UpdateRegion(smi);
    if (smi.def.spaceRequiredPerCreature == 0)
      return;
    OvercrowdingMonitor.Occupancy occupancy = (OvercrowdingMonitor.Occupancy) null;
    List<KPrefabID> creatures = (List<KPrefabID>) null;
    List<KPrefabID> eggs = (List<KPrefabID>) null;
    if (smi.IsFish)
    {
      if (smi.pond != null)
      {
        occupancy = smi.pond.occupancy;
        creatures = smi.pond.fishes;
        eggs = smi.pond.eggs;
      }
    }
    else if (smi.cavity != null)
    {
      occupancy = smi.cavity.occupancy;
      creatures = smi.cavity.creatures;
      eggs = smi.cavity.eggs;
    }
    if (occupancy != null && occupancy.dirty)
      occupancy.Analyze(creatures, eggs, !smi.IsFish);
    if (smi.regionAnalysis.IsDirty)
    {
      smi.regionAnalysis.SetOccupancy(occupancy);
      smi.OnRegionAnalysisDirtied();
    }
    OvercrowdingMonitor.AlignTagsAndEffects(smi);
  }

  private static void AlignTagsAndEffects(OvercrowdingMonitor.Instance smi)
  {
    bool isConfined = smi.regionAnalysis.IsConfined;
    bool isOvercrowded = smi.regionAnalysis.IsOvercrowded;
    int overcrowdedModifier = smi.regionAnalysis.OvercrowdedModifier;
    bool futureOvercrowded = smi.regionAnalysis.IsFutureOvercrowded;
    int num1 = isOvercrowded ? overcrowdedModifier : 0;
    if (isOvercrowded)
      smi.overcrowded.Modifier.SetValue((float) num1);
    bool flag1 = smi.kpid.HasTag(GameTags.Creatures.Overcrowded);
    int num2 = smi.kpid.HasTag(GameTags.Creatures.Expecting) ? 1 : 0;
    bool flag2 = smi.kpid.HasTag(GameTags.Creatures.Confined);
    bool flag3 = smi.effects.HasEffect(smi.futureOvercrowded.Effect);
    if (flag1 != isOvercrowded)
      smi.kpid.SetTag(GameTags.Creatures.Overcrowded, isOvercrowded);
    bool set1 = !smi.isBaby && !futureOvercrowded;
    int num3 = set1 ? 1 : 0;
    if (num2 != num3)
      smi.kpid.SetTag(GameTags.Creatures.Expecting, set1);
    if (flag2 != isConfined)
      smi.kpid.SetTag(GameTags.Creatures.Confined, isConfined);
    bool set2 = isConfined;
    bool set3 = isOvercrowded && !set2;
    bool set4 = futureOvercrowded && !set2;
    if (set2 != flag2)
      smi.confined.instance = OvercrowdingMonitor.SetEffect(smi, smi.confined.Effect, set2, smi.confined.Tooltip);
    if (set3 != flag1)
      smi.overcrowded.instance = OvercrowdingMonitor.SetEffect(smi, smi.overcrowded.Effect, set3, smi.overcrowded.Tooltip);
    if (set4 == flag3)
      return;
    smi.futureOvercrowded.instance = OvercrowdingMonitor.SetEffect(smi, smi.futureOvercrowded.Effect, set4, smi.futureOvercrowded.Tooltip);
  }

  private static EffectInstance SetEffect(
    OvercrowdingMonitor.Instance smi,
    Effect effect,
    bool set,
    Func<string, object, string> tooltip)
  {
    if (set)
      return smi.effects.Add(effect, false, tooltip);
    smi.effects.Remove(effect);
    return (EffectInstance) null;
  }

  private static bool FetchIsFish(KPrefabID creature)
  {
    bool flag1;
    if (OvercrowdingMonitor.isFish.TryGetValue(creature.PrefabTag, out flag1))
      return flag1;
    bool flag2 = creature.GetDef<FishOvercrowdingMonitor.Def>() != null;
    OvercrowdingMonitor.isFish[creature.PrefabTag] = flag2;
    return flag2;
  }

  private static bool FetchIsFishEgg(KPrefabID egg)
  {
    bool flag;
    if (OvercrowdingMonitor.isFish.TryGetValue(egg.PrefabTag, out flag))
      return flag;
    IncubationMonitor.Instance smi = egg.GetSMI<IncubationMonitor.Instance>();
    if (OvercrowdingMonitor.isFish.TryGetValue(smi.def.spawnedCreature, out flag))
    {
      OvercrowdingMonitor.isFish[egg.PrefabTag] = flag;
      return flag;
    }
    GameObject prefab = Assets.GetPrefab(smi.def.spawnedCreature);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      flag = prefab.GetDef<FishOvercrowdingMonitor.Def>() != null;
    OvercrowdingMonitor.isFish[egg.PrefabTag] = flag;
    OvercrowdingMonitor.isFish[smi.def.spawnedCreature] = flag;
    return flag;
  }

  private static void UpdateRegion(OvercrowdingMonitor.Instance smi)
  {
    int cell = Grid.PosToCell((StateMachine.Instance) smi);
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
    if (cavityForCell != smi.cavity)
    {
      if (smi.cavity != null)
      {
        smi.RemoveFromCavity();
        Game.Instance.roomProber.UpdateRoom(cavityForCell);
        smi.regionAnalysis.ForceDirty();
      }
      smi.cavity = cavityForCell;
      if (smi.cavity != null)
      {
        smi.AddToCavity();
        Game.Instance.roomProber.UpdateRoom(smi.cavity);
        smi.regionAnalysis.ForceDirty();
      }
    }
    if (!smi.IsFish)
      return;
    FishOvercrowingManager.Pond pond = FishOvercrowingManager.Instance.GetPond(cell);
    if (pond == smi.pond)
      return;
    if (smi.pond != null)
      smi.regionAnalysis.ForceDirty();
    smi.pond = pond;
    if (smi.pond == null)
      return;
    smi.regionAnalysis.ForceDirty();
  }

  public class Def : StateMachine.BaseDef
  {
    public int spaceRequiredPerCreature;
  }

  public class Occupancy
  {
    public bool dirty = true;

    public Dictionary<Tag, int> CritterCounts { get; } = new Dictionary<Tag, int>();

    public int OccupiedCellCount { get; private set; }

    public int HatchedEggOccupiedCellCount { get; private set; }

    public int Generation { get; private set; }

    public void Analyze(List<KPrefabID> creatures, List<KPrefabID> eggs, bool excludeFish = false)
    {
      DebugUtil.DevAssert(this.dirty, "Only incur Analyze overhead when dirty");
      this.CritterCounts.EnsureCapacity(creatures.Count);
      this.CritterCounts.Clear();
      this.OccupiedCellCount = 0;
      this.HatchedEggOccupiedCellCount = 0;
      creatures.RemoveAll(Util.IsNullOrDestroyedPredicate);
      foreach (KPrefabID creature in creatures)
      {
        if (!excludeFish || !OvercrowdingMonitor.FetchIsFish(creature))
        {
          this.OccupiedCellCount += OvercrowdingMonitor.FetchCreaturePersonalSpace(creature);
          int num;
          this.CritterCounts[creature.PrefabTag] = !this.CritterCounts.TryGetValue(creature.PrefabTag, out num) ? 1 : num + 1;
        }
      }
      eggs.RemoveAll(Util.IsNullOrDestroyedPredicate);
      foreach (KPrefabID egg in eggs)
      {
        if (!excludeFish || !OvercrowdingMonitor.FetchIsFishEgg(egg))
          this.HatchedEggOccupiedCellCount += OvercrowdingMonitor.FetchEggPersonalSpace(egg);
      }
      ++this.Generation;
      this.dirty = false;
    }
  }

  public struct RegionAnalysis(OvercrowdingMonitor.Instance smi)
  {
    private OvercrowdingMonitor.Occupancy occupancy = (OvercrowdingMonitor.Occupancy) null;
    private int occupancyGeneration = -1;
    private readonly OvercrowdingMonitor.Instance smi = smi;

    public readonly bool IsDirty
    {
      get => this.occupancy == null || this.occupancyGeneration != this.occupancy.Generation;
    }

    public readonly int CellCount => this.smi.RegionSize;

    public readonly bool IsPond => this.smi.IsInPond;

    public readonly Dictionary<Tag, int> CritterCounts
    {
      get => this.occupancy == null ? (Dictionary<Tag, int>) null : this.occupancy.CritterCounts;
    }

    public readonly int OccupiedCellCount
    {
      get => this.occupancy == null ? 0 : this.occupancy.OccupiedCellCount;
    }

    public readonly int HatchedEggOccupiedCellCount
    {
      get => this.occupancy == null ? 0 : this.occupancy.HatchedEggOccupiedCellCount;
    }

    public readonly bool IsOvercrowded
    {
      get => !this.IsDegeneratePersonalSpace && this.UnoccupiedCellCount < 0;
    }

    public readonly bool IsConfined
    {
      get
      {
        return !this.ConfinementImmunity && !this.IsDegeneratePersonalSpace && this.CellCount < this.PersonalSpace;
      }
    }

    public readonly bool IsFutureOvercrowded
    {
      get
      {
        return !this.IsDegeneratePersonalSpace && this.FutureUnoccupiedCellCount < 0 && this.HatchedEggOccupiedCellCount > 0;
      }
    }

    public readonly int OvercrowdedModifier
    {
      get => !this.IsOvercrowded ? 0 : -this.OverOccupiedCritterCount;
    }

    public readonly bool IsDegenerate => this.CellCount <= 0;

    private readonly int PersonalSpace => this.smi.def.spaceRequiredPerCreature;

    private readonly bool ConfinementImmunity
    {
      get => this.smi.kpid.HasAnyTags(OvercrowdingMonitor.CONFINEMENT_IMMUNITY_TAGS);
    }

    private readonly int UnoccupiedCellCount => this.CellCount - this.OccupiedCellCount;

    private readonly int OverOccupiedCellCount => this.OccupiedCellCount - this.CellCount;

    private readonly int FutureUnoccupiedCellCount
    {
      get => this.UnoccupiedCellCount - this.HatchedEggOccupiedCellCount;
    }

    private readonly bool IsDegeneratePersonalSpace => this.PersonalSpace == 0;

    private readonly int OverOccupiedCritterCount
    {
      get => this.ComputeOverOccupiedCritterCount(this.PersonalSpace);
    }

    public void SetOccupancy(OvercrowdingMonitor.Occupancy occupancy)
    {
      this.occupancy = occupancy;
      this.occupancyGeneration = occupancy != null ? occupancy.Generation : -1;
    }

    public void ForceDirty() => this.occupancyGeneration = -1;

    public readonly string Substitute(string s)
    {
      LocString newValue = this.IsPond ? (this.CellCount == 0 ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION_AQUATIC.NO_CELLS : (this.CellCount == 1 ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION_AQUATIC.SINGLE_CELL : CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION_AQUATIC.MULTIPLE_CELLS)) : (this.CellCount == 0 ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION.NO_CELLS : (this.CellCount == 1 ? CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION.SINGLE_CELL : CREATURES.MODIFIERS.OVERCROWDED.EXPLANATION.MULTIPLE_CELLS));
      StringBuilder sb = GlobalStringBuilderPool.Alloc();
      sb.Append(s);
      sb.Replace("{explanation}", (string) newValue);
      sb.Replace("{contextCritterType}", this.smi.kpid.PrefabTag.ProperName());
      int num = this.PersonalSpace;
      sb.Replace("{personalSpace}", num.ToString());
      num = this.CellCount;
      sb.Replace("{cellCount}", num.ToString());
      num = this.OccupiedCellCount;
      sb.Replace("{occupiedCellCount}", num.ToString());
      sb.Replace("{unoccupiedCellCount}", (this.IsOvercrowded ? 0 : this.UnoccupiedCellCount).ToString());
      sb.Replace("{overOccupiedCellCount}", (this.IsOvercrowded ? this.OverOccupiedCellCount : 0).ToString());
      sb.Replace("{bullets}", this.BuildCritterOccupancies());
      return GlobalStringBuilderPool.ReturnAndFree(sb);
    }

    private readonly int ComputeOverOccupiedCritterCount(int personalSpace)
    {
      int result;
      return personalSpace == 0 ? 0 : Math.DivRem(this.OverOccupiedCellCount, personalSpace, out result) + (result == 0 ? 0 : 1);
    }

    private readonly string BuildCritterOccupancies()
    {
      if (this.CritterCounts == null)
        return string.Empty;
      StringBuilder sb1 = GlobalStringBuilderPool.Alloc();
      ListPool<OvercrowdingMonitor.RegionAnalysis.CritterOccupancy, OvercrowdingMonitor.RegionAnalysis>.PooledList pooledList = ListPool<OvercrowdingMonitor.RegionAnalysis.CritterOccupancy, OvercrowdingMonitor.RegionAnalysis>.Allocate();
      pooledList.Capacity = this.CritterCounts.Count;
      foreach (Tag key in this.CritterCounts.Keys)
      {
        int occupiedCritterCount = this.ComputeOverOccupiedCritterCount(OvercrowdingMonitor.personalSpaces[key]);
        int critterCount = this.CritterCounts[key];
        bool flag = occupiedCritterCount <= critterCount;
        int num = Math.Min(this.CritterCounts[key], occupiedCritterCount);
        pooledList.Add(new OvercrowdingMonitor.RegionAnalysis.CritterOccupancy()
        {
          critterType = key,
          overOccupancy = num,
          canFix = flag
        });
      }
      Dictionary<Tag, int> capturedCritterCounts = this.CritterCounts;
      Dictionary<Tag, int> capturedPersonalSpaces = OvercrowdingMonitor.personalSpaces;
      pooledList.Sort((Comparison<OvercrowdingMonitor.RegionAnalysis.CritterOccupancy>) ((a, b) =>
      {
        int num = capturedCritterCounts[a.critterType] * capturedPersonalSpaces[a.critterType];
        return (capturedCritterCounts[b.critterType] * capturedPersonalSpaces[b.critterType]).CompareTo(num);
      }));
      foreach (OvercrowdingMonitor.RegionAnalysis.CritterOccupancy critterOccupancy in (List<OvercrowdingMonitor.RegionAnalysis.CritterOccupancy>) pooledList)
      {
        int critterCount = this.CritterCounts[critterOccupancy.critterType];
        LocString locString = critterCount == 1 ? (critterOccupancy.canFix ? CREATURES.MODIFIERS.OVERCROWDED.BULLET.CAN_FIX.SINGULAR : CREATURES.MODIFIERS.OVERCROWDED.BULLET.CANNOT_FIX.SINGULAR) : (critterOccupancy.canFix ? CREATURES.MODIFIERS.OVERCROWDED.BULLET.CAN_FIX.MULTIPLE : CREATURES.MODIFIERS.OVERCROWDED.BULLET.CANNOT_FIX.MULTIPLE);
        int personalSpace = OvercrowdingMonitor.personalSpaces[critterOccupancy.critterType];
        int num = this.OccupiedCellCount - critterOccupancy.overOccupancy * personalSpace;
        StringBuilder sb2 = GlobalStringBuilderPool.Alloc();
        sb2.Append((string) locString);
        sb2.Replace("{critterType}", critterOccupancy.critterType.ProperName());
        sb2.Replace("{critterCount}", critterCount.ToString());
        sb2.Replace("{personalSpace}", personalSpace.ToString());
        sb2.Replace("{overOccupancy}", critterOccupancy.overOccupancy.ToString());
        sb2.Replace("{cellCountWithFix}", num.ToString());
        sb1.AppendLine(GlobalStringBuilderPool.ReturnAndFree(sb2));
      }
      if (this.CritterCounts.Count > 0)
        --sb1.Length;
      pooledList.Recycle();
      return GlobalStringBuilderPool.ReturnAndFree(sb1);
    }

    private struct CritterOccupancy
    {
      public Tag critterType;
      public int overOccupancy;
      public bool canFix;
    }
  }

  public struct OvercrowdEffect
  {
    public EffectInstance instance;

    public Effect Effect { get; private set; }

    public AttributeModifier Modifier { get; private set; }

    public Func<string, object, string> Tooltip { get; private set; }

    public string TooltipText { get; private set; }

    public OvercrowdEffect(
      string id,
      string name,
      AttributeModifier modifier,
      Func<string> GenerateTooltip)
    {
      this.Effect = new Effect(id, name, string.Empty, 0.0f, true, false, true);
      this.instance = (EffectInstance) null;
      this.Modifier = modifier;
      this.Tooltip = (Func<string, object, string>) null;
      this.Effect.Add(modifier);
      this.TooltipText = (string) null;
      OvercrowdingMonitor.OvercrowdEffect capturedThis = this;
      this.Tooltip = (Func<string, object, string>) ((_tooltip, untypedEffectInstance) =>
      {
        if (capturedThis.TooltipText == null)
          capturedThis.TooltipText = ((EffectInstance) untypedEffectInstance).ResolveTooltip(GenerateTooltip(), untypedEffectInstance);
        return capturedThis.TooltipText;
      });
    }

    public OvercrowdEffect(
      string id,
      string name,
      AttributeModifier modifier,
      string tooltipFormat)
      : this(id, name, modifier, (Func<string>) (() => tooltipFormat))
    {
    }

    public void ClearTooltip() => this.TooltipText = (string) null;
  }

  public new class Instance : 
    GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.GameInstance
  {
    public CavityInfo cavity;
    public FishOvercrowingManager.Pond pond;
    public bool isBaby;
    public OvercrowdingMonitor.OvercrowdEffect futureOvercrowded;
    public OvercrowdingMonitor.OvercrowdEffect overcrowded;
    public OvercrowdingMonitor.OvercrowdEffect confined;
    public OvercrowdingMonitor.RegionAnalysis regionAnalysis;
    [MyCmpReq]
    public KPrefabID kpid;
    [MyCmpReq]
    public Effects effects;
    private int onRoomUpdatedHandle = -1;

    public int RegionSize
    {
      get
      {
        return !this.IsFish ? (this.cavity == null ? 0 : this.cavity.NumCells) : (this.pond == null ? 0 : this.pond.cellCount);
      }
    }

    public bool IsInPond => this.pond != null;

    public bool IsFish => OvercrowdingMonitor.FetchIsFish(this.kpid);

    public bool IsEgg => this.kpid.HasTag(GameTags.Egg);

    public Instance(IStateMachineTarget master, OvercrowdingMonitor.Def def)
      : base(master, def)
    {
      this.isBaby = master.gameObject.GetDef<BabyMonitor.Def>() != null;
      this.futureOvercrowded = new OvercrowdingMonitor.OvercrowdEffect("FutureOvercrowded", (string) CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.NAME, new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.NAME, true), new Func<string>(FutureOvercrowdedTooltip));
      this.overcrowded = new OvercrowdingMonitor.OvercrowdEffect("Overcrowded", (string) CREATURES.MODIFIERS.OVERCROWDED.CROWDED.NAME, new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 0.0f, (string) CREATURES.MODIFIERS.OVERCROWDED.CROWDED.NAME, is_readonly: false), new Func<string>(OvercrowdedTooltip));
      this.confined = new OvercrowdingMonitor.OvercrowdEffect("Confined", (string) CREATURES.MODIFIERS.OVERCROWDED.CONFINED.NAME, new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, (string) CREATURES.MODIFIERS.OVERCROWDED.CONFINED.NAME, is_readonly: false), new Func<string>(ConfinedTooltip));
      this.confined.Effect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.OVERCROWDED.CONFINED.NAME, true));
      this.onRoomUpdatedHandle = this.Subscribe(144050788, new System.Action<object>(this.OnRoomUpdated));
      this.regionAnalysis = new OvercrowdingMonitor.RegionAnalysis(this);
      OvercrowdingMonitor.UpdateState(this, 0.0f);

      string FutureOvercrowdedTooltip()
      {
        return this.regionAnalysis.Substitute((string) (this.IsFish ? CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.FISHTOOLTIP : CREATURES.MODIFIERS.OVERCROWDED.CRAMPED.TOOLTIP));
      }

      string OvercrowdedTooltip()
      {
        return this.regionAnalysis.Substitute((string) (this.IsFish ? CREATURES.MODIFIERS.OVERCROWDED.CROWDED.FISHTOOLTIP : CREATURES.MODIFIERS.OVERCROWDED.CROWDED.TOOLTIP));
      }

      string ConfinedTooltip()
      {
        return this.regionAnalysis.Substitute((string) (this.regionAnalysis.IsDegenerate ? CREATURES.MODIFIERS.OVERCROWDED.CONFINED.TOOLTIP_NO_SUBSTITUTIONS : CREATURES.MODIFIERS.OVERCROWDED.CONFINED.TOOLTIP));
      }
    }

    public void OnRegionAnalysisDirtied()
    {
      this.futureOvercrowded.ClearTooltip();
      this.overcrowded.ClearTooltip();
      this.confined.ClearTooltip();
    }

    public void AddToCavity()
    {
      if (this.IsEgg)
      {
        this.cavity.eggs.Add(this.kpid);
        if (OvercrowdingMonitor.FetchIsFishEgg(this.kpid))
          this.cavity.fish_eggs.Add(this.kpid);
      }
      else
      {
        this.cavity.creatures.Add(this.kpid);
        if (this.IsFish)
          this.cavity.fishes.Add(this.kpid);
      }
      this.cavity.occupancy.dirty = true;
    }

    public void RemoveFromCavity()
    {
      if (this.IsEgg)
      {
        this.cavity.RemoveFromCavity(this.kpid, this.cavity.eggs);
        if (OvercrowdingMonitor.FetchIsFishEgg(this.kpid))
          this.cavity.RemoveFromCavity(this.kpid, this.cavity.fish_eggs);
      }
      else
      {
        this.cavity.RemoveFromCavity(this.kpid, this.cavity.creatures);
        if (this.IsFish)
          this.cavity.RemoveFromCavity(this.kpid, this.cavity.fishes);
      }
      this.cavity.occupancy.dirty = true;
    }

    protected override void OnCleanUp()
    {
      this.Unsubscribe(ref this.onRoomUpdatedHandle);
      if (this.cavity == null)
        return;
      this.RemoveFromCavity();
    }

    public void OnRoomUpdated(object o)
    {
      if (o != null)
        return;
      this.RoomRefreshUpdateCavity();
    }

    public void RoomRefreshUpdateCavity() => OvercrowdingMonitor.UpdateState(this, 0.0f);
  }
}
