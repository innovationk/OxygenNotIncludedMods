// Decompiled with JetBrains decompiler
// Type: RocketModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/RocketModule")]
public class RocketModule : KMonoBehaviour
{
  public LaunchConditionManager conditionManager;
  public Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleConditions = new Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>();
  public static readonly Operational.Flag landedFlag = new Operational.Flag("landed", Operational.Flag.Type.Requirement);
  public bool operationalLandedRequired = true;
  private string rocket_module_bg_base_string = "{0}{1}";
  private string rocket_module_bg_affix = "BG";
  private string rocket_module_bg_anim = "on";
  [SerializeField]
  private KAnimFile bgAnimFile;
  protected string parentRocketName = (string) UI.STARMAP.DEFAULT_NAME;
  private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketOnGround, (Action<RocketModule, object>) ((component, data) => component.OnRocketOnGroundTag(data)));
  private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketNotOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketNotOnGround, (Action<RocketModule, object>) ((component, data) => component.OnRocketNotOnGroundTag(data)));

  public ProcessCondition AddModuleCondition(
    ProcessCondition.ProcessConditionType conditionType,
    ProcessCondition condition)
  {
    if (!this.moduleConditions.ContainsKey(conditionType))
      this.moduleConditions.Add(conditionType, new List<ProcessCondition>());
    if (!this.moduleConditions[conditionType].Contains(condition))
      this.moduleConditions[conditionType].Add(condition);
    return condition;
  }

  public List<ProcessCondition> GetConditionSet(
    ProcessCondition.ProcessConditionType conditionType)
  {
    List<ProcessCondition> conditionSet = new List<ProcessCondition>();
    if (conditionType == ProcessCondition.ProcessConditionType.All)
    {
      foreach (KeyValuePair<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleCondition in this.moduleConditions)
        conditionSet.AddRange((IEnumerable<ProcessCondition>) moduleCondition.Value);
    }
    else if (this.moduleConditions.ContainsKey(conditionType))
      conditionSet = this.moduleConditions[conditionType];
    return conditionSet;
  }

  public int PopulateConditionSet(
    ProcessCondition.ProcessConditionType conditionType,
    List<ProcessCondition> conditions)
  {
    int num = 0;
    if (conditionType == ProcessCondition.ProcessConditionType.All)
    {
      foreach (KeyValuePair<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleCondition in this.moduleConditions)
      {
        conditions.AddRange((IEnumerable<ProcessCondition>) moduleCondition.Value);
        num += moduleCondition.Value.Count;
      }
    }
    else
    {
      List<ProcessCondition> collection;
      if (this.moduleConditions.TryGetValue(conditionType, out collection))
      {
        conditions.AddRange((IEnumerable<ProcessCondition>) collection);
        num += collection.Count;
      }
    }
    return num;
  }

  public void SetBGKAnim(KAnimFile anim_file) => this.bgAnimFile = anim_file;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketOnGroundTagDelegate, false);
    GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketNotOnGroundTagDelegate, false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!DlcManager.FeatureClusterSpaceEnabled())
    {
      this.conditionManager = this.FindLaunchConditionManager();
      Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
      if (conditionManager != null)
        this.SetParentRocketName(conditionManager.GetRocketName());
      this.RegisterWithConditionManager();
    }
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.AddStatusItem(Db.Get().BuildingStatusItems.RocketName, (object) this);
    this.FixSorting();
    this.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged += new Action<object>(this.OnAttachmentNetworkChanged);
    if (!((UnityEngine.Object) this.bgAnimFile != (UnityEngine.Object) null))
      return;
    this.AddBGGantry();
  }

  public void FixSorting()
  {
    int num = 0;
    AttachableBuilding component1 = this.GetComponent<AttachableBuilding>();
    while ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      BuildingAttachPoint attachedTo = component1.GetAttachedTo();
      if ((UnityEngine.Object) attachedTo != (UnityEngine.Object) null)
      {
        component1 = attachedTo.GetComponent<AttachableBuilding>();
        ++num;
      }
      else
        break;
    }
    this.transform.SetLocalPosition(this.transform.GetLocalPosition() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.Building) - (float) num * 0.01f
    });
    KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
    if (!component2.enabled)
      return;
    component2.enabled = false;
    component2.enabled = true;
  }

  private void OnAttachmentNetworkChanged(object ab) => this.FixSorting();

  private void AddBGGantry()
  {
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    GameObject go = new GameObject();
    go.name = string.Format(this.rocket_module_bg_base_string, (object) this.name, (object) this.rocket_module_bg_affix);
    go.SetActive(false);
    Vector3 position = component.transform.GetPosition() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.InteriorWall)
    };
    go.transform.SetPosition(position);
    go.transform.parent = this.transform;
    KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      this.bgAnimFile
    };
    kbatchedAnimController.initialAnim = this.rocket_module_bg_anim;
    kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
    kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
    kbatchedAnimController.FlipX = component.FlipX;
    kbatchedAnimController.FlipY = component.FlipY;
    go.SetActive(true);
  }

  private void OnRocketOnGroundTag(object data)
  {
    this.RegisterComponents();
    Operational component = this.GetComponent<Operational>();
    if (!this.operationalLandedRequired || !((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetFlag(RocketModule.landedFlag, true);
  }

  private void OnRocketNotOnGroundTag(object data)
  {
    this.DeregisterComponents();
    Operational component = this.GetComponent<Operational>();
    if (!this.operationalLandedRequired || !((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetFlag(RocketModule.landedFlag, false);
  }

  public void DeregisterComponents()
  {
    KSelectable component1 = this.GetComponent<KSelectable>();
    component1.IsSelectable = false;
    BuildingComplete component2 = this.GetComponent<BuildingComplete>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.UpdatePosition();
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component1)
      SelectTool.Instance.Select((KSelectable) null);
    Deconstructable component3 = this.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      component3.SetAllowDeconstruction(false);
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    if (handle.IsValid())
      GameComps.StructureTemperatures.Disable(handle);
    FakeFloorAdder component4 = this.GetComponent<FakeFloorAdder>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      component4.SetFloor(false);
    AccessControl component5 = this.GetComponent<AccessControl>();
    if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
      component5.SetRegistered(false);
    foreach (ManualDeliveryKG component6 in this.GetComponents<ManualDeliveryKG>())
    {
      DebugUtil.DevAssert(!component6.IsPaused, "RocketModule ManualDeliver chore was already paused, when this rocket lands it will re-enable it.");
      component6.Pause(true, "Rocket heading to space");
    }
    foreach (BuildingConduitEndpoints component7 in this.GetComponents<BuildingConduitEndpoints>())
      component7.RemoveEndPoint();
    ReorderableBuilding component8 = this.GetComponent<ReorderableBuilding>();
    if ((UnityEngine.Object) component8 != (UnityEngine.Object) null)
      component8.ShowReorderArm(false);
    Workable component9 = this.GetComponent<Workable>();
    if ((UnityEngine.Object) component9 != (UnityEngine.Object) null)
      component9.RefreshReachability();
    Structure component10 = this.GetComponent<Structure>();
    if ((UnityEngine.Object) component10 != (UnityEngine.Object) null)
      component10.UpdatePosition();
    WireUtilitySemiVirtualNetworkLink component11 = this.GetComponent<WireUtilitySemiVirtualNetworkLink>();
    if ((UnityEngine.Object) component11 != (UnityEngine.Object) null)
      component11.SetLinkConnected(false);
    PartialLightBlocking component12 = this.GetComponent<PartialLightBlocking>();
    if (!((UnityEngine.Object) component12 != (UnityEngine.Object) null))
      return;
    component12.ClearLightBlocking();
  }

  public void RegisterComponents()
  {
    this.GetComponent<KSelectable>().IsSelectable = true;
    BuildingComplete component1 = this.GetComponent<BuildingComplete>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.UpdatePosition();
    Deconstructable component2 = this.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.SetAllowDeconstruction(true);
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    if (handle.IsValid())
      GameComps.StructureTemperatures.Enable(handle);
    foreach (Storage component3 in this.GetComponents<Storage>())
      component3.UpdateStoredItemCachedCells();
    FakeFloorAdder component4 = this.GetComponent<FakeFloorAdder>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      component4.SetFloor(true);
    AccessControl component5 = this.GetComponent<AccessControl>();
    if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
      component5.SetRegistered(true);
    foreach (ManualDeliveryKG component6 in this.GetComponents<ManualDeliveryKG>())
      component6.Pause(false, "Landing on world");
    foreach (BuildingConduitEndpoints component7 in this.GetComponents<BuildingConduitEndpoints>())
      component7.AddEndpoint();
    ReorderableBuilding component8 = this.GetComponent<ReorderableBuilding>();
    if ((UnityEngine.Object) component8 != (UnityEngine.Object) null)
      component8.ShowReorderArm(true);
    Workable component9 = this.GetComponent<Workable>();
    if ((UnityEngine.Object) component9 != (UnityEngine.Object) null)
      component9.RefreshReachability();
    Structure component10 = this.GetComponent<Structure>();
    if ((UnityEngine.Object) component10 != (UnityEngine.Object) null)
      component10.UpdatePosition();
    WireUtilitySemiVirtualNetworkLink component11 = this.GetComponent<WireUtilitySemiVirtualNetworkLink>();
    if ((UnityEngine.Object) component11 != (UnityEngine.Object) null)
      component11.SetLinkConnected(true);
    PartialLightBlocking component12 = this.GetComponent<PartialLightBlocking>();
    if (!((UnityEngine.Object) component12 != (UnityEngine.Object) null))
      return;
    component12.SetLightBlocking();
  }

  private void ToggleComponent(System.Type cmpType, bool enabled)
  {
    MonoBehaviour component = (MonoBehaviour) this.GetComponent(cmpType);
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.enabled = enabled;
  }

  public void RegisterWithConditionManager()
  {
    Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
    if (!((UnityEngine.Object) this.conditionManager != (UnityEngine.Object) null))
      return;
    this.conditionManager.RegisterRocketModule(this);
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.conditionManager != (UnityEngine.Object) null)
      this.conditionManager.UnregisterRocketModule(this);
    base.OnCleanUp();
  }

  public virtual LaunchConditionManager FindLaunchConditionManager()
  {
    if (!DlcManager.FeatureClusterSpaceEnabled())
    {
      foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
      {
        LaunchConditionManager component = gameObject.GetComponent<LaunchConditionManager>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          return component;
      }
    }
    return (LaunchConditionManager) null;
  }

  public void SetParentRocketName(string newName)
  {
    this.parentRocketName = newName;
    NameDisplayScreen.Instance.UpdateName(this.gameObject);
  }

  public virtual string GetParentRocketName() => this.parentRocketName;

  public void MoveToSpace()
  {
    Prioritizable component1 = this.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.GetMyWorld() != (UnityEngine.Object) null)
      component1.GetMyWorld().RemoveTopPriorityPrioritizable(component1);
    int cell = Grid.PosToCell(this.transform.GetPosition());
    Building component2 = this.GetComponent<Building>();
    component2.Def.UnmarkArea(cell, component2.Orientation, component2.Def.ObjectLayer, this.gameObject);
    this.gameObject.transform.SetPosition(new Vector3(-1f, -1f, 0.0f));
    LogicPorts component3 = this.GetComponent<LogicPorts>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      component3.OnMove();
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, false, (object) this);
  }

  public void MoveToPad(int newCell)
  {
    this.gameObject.transform.SetPosition(Grid.CellToPos(newCell, CellAlignment.Bottom, Grid.SceneLayer.Building));
    int cell = Grid.PosToCell(this.transform.GetPosition());
    Building component1 = this.GetComponent<Building>();
    component1.RefreshCells();
    component1.Def.MarkArea(cell, component1.Orientation, component1.Def.ObjectLayer, this.gameObject);
    LogicPorts component2 = this.GetComponent<LogicPorts>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.OnMove();
    Prioritizable component3 = this.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null) || !component3.IsTopPriority())
      return;
    component3.GetMyWorld().AddTopPriorityPrioritizable(component3);
  }
}
