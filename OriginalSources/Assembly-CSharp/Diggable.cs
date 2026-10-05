// Decompiled with JetBrains decompiler
// Type: Diggable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Diggable")]
public class Diggable : Workable
{
  private HandleVector<int>.Handle partitionerEntry;
  private HandleVector<int>.Handle unstableEntry;
  private HandleVector<int>.Handle backwallEntry;
  private MeshRenderer childRenderer;
  private bool isReachable;
  private int cached_cell = -1;
  private Element originalDigElement;
  [MyCmpAdd]
  private Prioritizable prioritizable;
  [SerializeField]
  public HashedString choreTypeIdHash;
  [SerializeField]
  public Material[] materials;
  [SerializeField]
  public MeshRenderer materialDisplay;
  private bool isDigComplete;
  [Serialize]
  public int digTypeFlags = 1;
  private static List<Tuple<string, Tag>> lasersForHardness = new List<Tuple<string, Tag>>()
  {
    new Tuple<string, Tag>("dig", (Tag) "fx_dig_splash"),
    new Tuple<string, Tag>("specialistdig", (Tag) "fx_dig_splash")
  };
  private static readonly EventSystem.IntraObjectHandler<Diggable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Diggable>((Action<Diggable, object>) ((component, data) => component.OnReachableChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Diggable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Diggable>((Action<Diggable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  public Chore chore;

  public bool Reachable => this.isReachable;

  public bool HasDigType(Diggable.DiggableType type)
  {
    return ((Diggable.DiggableType) this.digTypeFlags & type) != 0;
  }

  public bool WillDigBackwall() => this.HasDigType(Diggable.DiggableType.Backwall);

  public bool WillDigTile() => this.HasDigType(Diggable.DiggableType.Tile);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Digging;
    this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;
    this.faceTargetWhenWorking = true;
    this.Subscribe<Diggable>(-1432940121, Diggable.OnReachableChangedDelegate);
    this.attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.multitoolContext = (HashedString) "dig";
    this.multitoolHitEffectTag = (Tag) "fx_dig_splash";
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
    Prioritizable.AddRef(this.gameObject);
  }

  private Diggable() => this.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.cached_cell = Grid.PosToCell((KMonoBehaviour) this);
    this.originalDigElement = this.GetTargetElement();
    if (this.originalDigElement.hardness == byte.MaxValue)
      this.OnCancel();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForDig);
    this.UpdateColor(this.isReachable);
    Grid.Objects[this.cached_cell, 7] = this.gameObject;
    ChoreType chore_type = Db.Get().ChoreTypes.Dig;
    if (this.choreTypeIdHash.IsValid)
      chore_type = Db.Get().ChoreTypes.GetByHash(this.choreTypeIdHash);
    this.chore = (Chore) new WorkChore<Diggable>(chore_type, (IStateMachineTarget) this, is_preemptable: true);
    this.SetWorkTime(float.PositiveInfinity);
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Diggable.OnSpawn", (object) this.gameObject, Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
    this.backwallEntry = GameScenePartitioner.Instance.Add("Diggable.OnSpawn", (object) this.gameObject, Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.backwallChangedLayer, new Action<object>(this.OnSolidChanged));
    this.OnSolidChanged((object) null);
    new ReachabilityMonitor.Instance((Workable) this).StartSM();
    this.Subscribe<Diggable>(493375141, Diggable.OnRefreshUserMenuDelegate);
    if (this.skillsUpdateHandle == -1)
      this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, (object) this);
    Components.Diggables.Add(this);
  }

  public override int GetCell() => this.cached_cell;

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    Workable.AnimInfo anim = new Workable.AnimInfo();
    if (this.overrideAnims != null && this.overrideAnims.Length != 0)
      anim.overrideAnims = this.overrideAnims;
    if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
      anim.smi = (StateMachine.Instance) new MultitoolController.Instance((Workable) this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
    return anim;
  }

  private static bool IsCellBuildable(int cell)
  {
    bool flag = false;
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<Constructable>() != (UnityEngine.Object) null)
      flag = true;
    return flag;
  }

  private IEnumerator PeriodicUnstableFallingRecheck()
  {
    yield return (object) SequenceUtil.WaitForSeconds(2f);
    this.OnSolidChanged((object) null);
  }

  private void OnSolidChanged(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    GameScenePartitioner.Instance.Free(ref this.unstableEntry);
    int num = -1;
    Element element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
    if (this.WillDigTile() && Grid.IsSolidCell(this.cached_cell))
      element = Grid.Element[this.cached_cell];
    else if (this.WillDigBackwall() && BackwallManager.HasBackwall(this.cached_cell))
      element = BackwallManager.At(this.cached_cell).Element;
    if (element.hardness == byte.MaxValue)
    {
      this.UpdateColor(false);
      this.requiredSkillPerk = (string) null;
      this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigUnobtanium);
    }
    else if (element.hardness >= (byte) 251)
    {
      bool flag = false;
      foreach (Chore.PreconditionInstance precondition in this.chore.GetPreconditions())
      {
        if (precondition.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigRadioactiveMaterials);
      this.requiredSkillPerk = Db.Get().SkillPerks.CanDigRadioactiveMaterials.Id;
      this.materialDisplay.sharedMaterial = this.materials[3];
    }
    else if (element.hardness >= (byte) 200)
    {
      bool flag = false;
      foreach (Chore.PreconditionInstance precondition in this.chore.GetPreconditions())
      {
        if (precondition.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigSuperDuperHard);
      this.requiredSkillPerk = Db.Get().SkillPerks.CanDigSuperDuperHard.Id;
      this.materialDisplay.sharedMaterial = this.materials[3];
    }
    else if (element.hardness >= (byte) 150)
    {
      bool flag = false;
      foreach (Chore.PreconditionInstance precondition in this.chore.GetPreconditions())
      {
        if (precondition.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigNearlyImpenetrable);
      this.requiredSkillPerk = Db.Get().SkillPerks.CanDigNearlyImpenetrable.Id;
      this.materialDisplay.sharedMaterial = this.materials[2];
    }
    else if (element.hardness >= (byte) 50)
    {
      bool flag = false;
      foreach (Chore.PreconditionInstance precondition in this.chore.GetPreconditions())
      {
        if (precondition.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigVeryFirm);
      this.requiredSkillPerk = Db.Get().SkillPerks.CanDigVeryFirm.Id;
      this.materialDisplay.sharedMaterial = this.materials[1];
    }
    else
    {
      this.requiredSkillPerk = (string) null;
      List<Chore.PreconditionInstance> preconditions = this.chore.GetPreconditions();
      for (int index = preconditions.Count - 1; index >= 0; --index)
      {
        if (preconditions[index].condition.id == ChorePreconditions.instance.HasSkillPerk.id)
          preconditions.RemoveAtSwap<Chore.PreconditionInstance>(index);
      }
    }
    this.UpdateColor(this.isReachable);
    this.UpdateStatusItem();
    bool flag1 = false;
    if (this.WillDigTile())
    {
      num = Diggable.GetUnstableCellAbove(this.cached_cell);
      if (!Grid.Solid[this.cached_cell] && num == -1)
      {
        this.digTypeFlags &= -2;
        if (!this.WillDigBackwall() || !BackwallManager.HasBackwall(this.cached_cell))
          flag1 = true;
      }
      else if (num != -1)
        this.StartCoroutine("PeriodicUnstableFallingRecheck");
    }
    else if (this.WillDigBackwall() && !BackwallManager.HasBackwall(this.cached_cell))
      flag1 = true;
    else if (Grid.Foundation[this.cached_cell])
      flag1 = true;
    if (this.WillDigBackwall() && !BackwallManager.HasBackwall(this.cached_cell))
    {
      this.digTypeFlags &= -3;
      flag1 |= !this.WillDigTile();
    }
    if (flag1)
    {
      this.isDigComplete = true;
      if (this.chore == null || !this.chore.InProgress())
        Util.KDestroyGameObject(this.gameObject);
      else
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    else
    {
      if (num == -1)
        return;
      Extents extents = new Extents();
      Grid.CellToXY(this.cached_cell, out extents.x, out extents.y);
      extents.width = 1;
      extents.height = (num - this.cached_cell + Grid.WidthInCells - 1) / Grid.WidthInCells + 1;
      this.unstableEntry = GameScenePartitioner.Instance.Add("Diggable.OnSolidChanged", (object) this.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
    }
  }

  public Element GetTargetElement()
  {
    return !this.WillDigTile() && this.WillDigBackwall() && BackwallManager.HasBackwall(this.cached_cell) ? BackwallManager.At(this.cached_cell).Element : Grid.Element[this.cached_cell];
  }

  public override string GetConversationTopic() => this.originalDigElement.tag.Name;

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if (Grid.Solid[this.cached_cell] || this.WillDigBackwall())
      Diggable.DoDigTick(this.cached_cell, dt);
    this.isDigComplete = ((this.isDigComplete ? 1 : 0) | (this.WillDigTile() ? 0 : (!this.WillDigBackwall() ? 1 : 0))) != 0;
    return this.isDigComplete;
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    if (!this.isDigComplete)
      return;
    Util.KDestroyGameObject(this.gameObject);
  }

  public override bool InstantlyFinish(WorkerBase worker)
  {
    if (Grid.Element[this.cached_cell].hardness == byte.MaxValue)
      return false;
    float approximateDigTime = Diggable.GetApproximateDigTime(this.cached_cell);
    int num = (int) worker.Work(approximateDigTime);
    return true;
  }

  public static void DoDigTick(int cell, float dt)
  {
    Diggable.DoDigTick(cell, dt, WorldDamage.DamageType.Absolute);
  }

  public static void DoDigTick(int cell, float dt, WorldDamage.DamageType damageType)
  {
    float approximateDigTime = Diggable.GetApproximateDigTime(cell);
    float amount = dt / approximateDigTime;
    double num = (double) WorldDamage.Instance.ApplyDamage(cell, amount, -1, damageType);
  }

  public static float GetApproximateDigTime(int cell)
  {
    float mass = Grid.Mass[cell];
    float hardness = (float) Grid.Element[cell].hardness;
    if (!Grid.Solid[cell] && BackwallManager.HasBackwall(cell))
    {
      BackwallManager.BackwallIndexer backwallIndexer = BackwallManager.At(cell);
      hardness = (float) backwallIndexer.Element.hardness;
      backwallIndexer = BackwallManager.At(cell);
      mass = backwallIndexer.Mass;
    }
    if ((double) hardness == (double) byte.MaxValue)
      return float.MaxValue;
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Ice);
    float num1 = hardness / (float) elementByHash.hardness;
    float num2 = 4f * (Mathf.Min(mass, 400f) / 400f);
    return num2 + num1 * num2;
  }

  public static Diggable GetDiggable(int cell)
  {
    GameObject gameObject = Grid.Objects[cell, 7];
    return (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<Diggable>() : (Diggable) null;
  }

  public static bool IsDiggable(int cell)
  {
    return Grid.Solid[cell] ? !Grid.Foundation[cell] : Diggable.GetUnstableCellAbove(cell) != Grid.InvalidCell;
  }

  private static int GetUnstableCellAbove(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    List<int> containingFallingAbove = World.Instance.GetComponent<UnstableGroundManager>().GetCellsContainingFallingAbove(xy);
    if (containingFallingAbove.Contains(cell))
      return cell;
    byte num = Grid.WorldIdx[cell];
    for (int unstableCellAbove = Grid.CellAbove(cell); Grid.IsValidCell(unstableCellAbove) && (int) Grid.WorldIdx[unstableCellAbove] == (int) num && !Grid.Foundation[unstableCellAbove]; unstableCellAbove = Grid.CellAbove(unstableCellAbove))
    {
      if (Grid.Solid[unstableCellAbove])
        return Grid.Element[unstableCellAbove].IsUnstable ? unstableCellAbove : Grid.InvalidCell;
      if (containingFallingAbove.Contains(unstableCellAbove))
        return unstableCellAbove;
    }
    return Grid.InvalidCell;
  }

  public static bool RequiresTool(Element e) => false;

  public static bool Undiggable(Element e) => e.id == SimHashes.Unobtanium;

  private void OnReachableChanged(object data)
  {
    if ((UnityEngine.Object) this.childRenderer == (UnityEngine.Object) null)
      this.childRenderer = this.GetComponentInChildren<MeshRenderer>();
    Material material = this.childRenderer.material;
    this.isReachable = ((Boxed<bool>) data).value;
    if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
      return;
    this.UpdateColor(this.isReachable);
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.isReachable)
    {
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.DigUnreachable);
    }
    else
    {
      component.AddStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, (object) this);
      GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, (Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion)), (object) null, (SchedulerGroup) null);
    }
  }

  private void UpdateColor(bool reachable)
  {
    if (!((UnityEngine.Object) this.childRenderer != (UnityEngine.Object) null))
      return;
    Material material = this.childRenderer.material;
    if (Diggable.RequiresTool(Grid.Element[Grid.PosToCell(this.gameObject)]) || Diggable.Undiggable(Grid.Element[Grid.PosToCell(this.gameObject)]))
      material.color = Game.Instance.uiColours.Dig.invalidLocation;
    else if (Grid.Element[Grid.PosToCell(this.gameObject)].hardness >= (byte) 50)
    {
      material.color = !reachable ? Game.Instance.uiColours.Dig.unreachable : Game.Instance.uiColours.Dig.validLocation;
      this.multitoolContext = (HashedString) Diggable.lasersForHardness[1].first;
      this.multitoolHitEffectTag = Diggable.lasersForHardness[1].second;
    }
    else
    {
      material.color = !reachable ? Game.Instance.uiColours.Dig.unreachable : Game.Instance.uiColours.Dig.validLocation;
      this.multitoolContext = (HashedString) Diggable.lasersForHardness[0].first;
      this.multitoolHitEffectTag = Diggable.lasersForHardness[0].second;
    }
  }

  public override float GetPercentComplete() => Grid.Damage[Grid.PosToCell((KMonoBehaviour) this)];

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.backwallEntry);
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.unstableEntry);
    GameScenePartitioner.Instance.TriggerEvent(Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.digDestroyedLayer, (object) null);
    Components.Diggables.Remove(this);
  }

  private void OnCancel()
  {
    if ((UnityEngine.Object) DetailsScreen.Instance != (UnityEngine.Object) null)
      DetailsScreen.Instance.Show(false);
    this.gameObject.Trigger(2127324410);
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", (string) UI.USERMENUACTIONS.CANCELDIG.NAME, new System.Action(this.OnCancel), tooltipText: (string) UI.USERMENUACTIONS.CANCELDIG.TOOLTIP));
  }

  public enum DiggableType
  {
    Tile = 1,
    Backwall = 2,
  }
}
