// Decompiled with JetBrains decompiler
// Type: ReorderableBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReorderableBuilding : KMonoBehaviour
{
  private bool cancelShield;
  private bool reorderingAnimUnderway;
  private KBatchedAnimController animController;
  public List<SelectModuleCondition> buildConditions = new List<SelectModuleCondition>();
  private KBatchedAnimController reorderArmController;
  private KAnimLink m_animLink;
  [MyCmpAdd]
  private LoopingSounds loopingSounds;
  private string reorderSound = "RocketModuleSwitchingArm_moving_LP";
  private static List<ReorderableBuilding> toBeRemoved = new List<ReorderableBuilding>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.Subscribe(2127324410, new Action<object>(this.OnCancel));
    GameObject go = new GameObject();
    go.name = "ReorderArm";
    go.transform.SetParent(this.transform);
    go.transform.SetLocalPosition(Vector3.up * Grid.CellSizeInMeters * (float) ((double) this.GetComponent<Building>().Def.HeightInCells / 2.0 - 0.5));
    go.transform.SetPosition(new Vector3(go.transform.position.x, go.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
    go.SetActive(false);
    this.reorderArmController = go.AddComponent<KBatchedAnimController>();
    this.reorderArmController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "rocket_module_switching_arm_kanim")
    };
    this.reorderArmController.initialAnim = "off";
    go.SetActive(true);
    this.ShowReorderArm(Grid.IsValidCell(Grid.PosToCell(go)));
    RocketModuleCluster component = this.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      LaunchPad currentPad = component.CraftInterface.CurrentPad;
      if ((UnityEngine.Object) currentPad != (UnityEngine.Object) null)
        this.m_animLink = new KAnimLink(currentPad.GetComponent<KAnimControllerBase>(), (KAnimControllerBase) this.reorderArmController);
    }
    if (this.m_animLink != null)
      return;
    this.m_animLink = new KAnimLink(this.GetComponent<KAnimControllerBase>(), (KAnimControllerBase) this.reorderArmController);
  }

  private void OnCancel(object _)
  {
    if (!((UnityEngine.Object) this.GetComponent<BuildingUnderConstruction>() != (UnityEngine.Object) null) || this.cancelShield || ReorderableBuilding.toBeRemoved.Contains(this))
      return;
    ReorderableBuilding.toBeRemoved.Add(this);
  }

  public GameObject AddModule(BuildingDef def, IList<Tag> buildMaterials)
  {
    return Assets.GetPrefab(this.GetComponent<KPrefabID>().PrefabID()).GetComponent<ReorderableBuilding>().buildConditions.Find((Predicate<SelectModuleCondition>) (match => match is TopOnly)) != null || def.BuildingComplete.GetComponent<ReorderableBuilding>().buildConditions.Find((Predicate<SelectModuleCondition>) (match => match is EngineOnBottom)) != null ? this.AddModuleBelow(def, buildMaterials) : this.AddModuleAbove(def, buildMaterials);
  }

  private GameObject AddModuleAbove(BuildingDef def, IList<Tag> buildMaterials)
  {
    BuildingAttachPoint component = this.GetComponent<BuildingAttachPoint>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return (GameObject) null;
    BuildingAttachPoint.HardPoint point = component.points[0];
    int cell = Grid.OffsetCell(Grid.PosToCell(this.gameObject), point.position);
    int heightInCells = def.HeightInCells;
    if ((UnityEngine.Object) point.attachedBuilding != (UnityEngine.Object) null)
    {
      if (!point.attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(heightInCells))
        return (GameObject) null;
      point.attachedBuilding.GetComponent<ReorderableBuilding>().MoveVertical(heightInCells);
    }
    return this.AddModuleCommon(def, buildMaterials, cell);
  }

  private GameObject AddModuleBelow(BuildingDef def, IList<Tag> buildMaterials)
  {
    int cell = Grid.PosToCell(this.gameObject);
    int heightInCells = def.HeightInCells;
    if (!this.CanMoveVertically(heightInCells))
      return (GameObject) null;
    this.MoveVertical(heightInCells);
    return this.AddModuleCommon(def, buildMaterials, cell);
  }

  private GameObject AddModuleCommon(BuildingDef def, IList<Tag> buildMaterials, int cell)
  {
    GameObject gameObject = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild ? def.Build(cell, Orientation.Neutral, (Storage) null, buildMaterials, 273.15f, timeBuilt: GameClock.Instance.GetTime()) : def.TryPlace((GameObject) null, Grid.CellToPosCBC(cell, def.SceneLayer), Orientation.Neutral, buildMaterials);
    ReorderableBuilding.RebuildNetworks();
    this.RocketSpecificPostAdd(gameObject, cell);
    return gameObject;
  }

  private void RocketSpecificPostAdd(GameObject obj, int cell)
  {
    RocketModuleCluster component1 = this.GetComponent<RocketModuleCluster>();
    RocketModuleCluster component2 = obj.GetComponent<RocketModuleCluster>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || !((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component1.CraftInterface.AddModule(component2);
  }

  public void RemoveModule()
  {
    BuildingAttachPoint component1 = this.GetComponent<BuildingAttachPoint>();
    AttachableBuilding attachableBuilding = (AttachableBuilding) null;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.points[0].attachedBuilding != (UnityEngine.Object) null)
      attachableBuilding = component1.points[0].attachedBuilding;
    int heightInCells = this.GetComponent<Building>().Def.HeightInCells;
    if ((UnityEngine.Object) this.GetComponent<Deconstructable>() != (UnityEngine.Object) null)
      this.GetComponent<Deconstructable>().CompleteWork((WorkerBase) null);
    if ((UnityEngine.Object) this.GetComponent<BuildingUnderConstruction>() != (UnityEngine.Object) null)
      this.DeleteObject();
    Building component2 = this.GetComponent<Building>();
    component2.Def.UnmarkArea(Grid.PosToCell((KMonoBehaviour) this), component2.Orientation, component2.Def.ObjectLayer, this.gameObject);
    if (!((UnityEngine.Object) attachableBuilding != (UnityEngine.Object) null))
      return;
    ReorderableBuilding component3 = attachableBuilding.GetComponent<ReorderableBuilding>();
    if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null))
      return;
    component3.MoveVertical(-heightInCells);
  }

  public void LateUpdate()
  {
    this.cancelShield = false;
    ReorderableBuilding.ProcessToBeRemoved();
    if (!this.reorderingAnimUnderway)
      return;
    float num = 10f;
    if ((double) Mathf.Abs(this.animController.Offset.y) < (double) Time.unscaledDeltaTime * (double) num)
    {
      this.animController.Offset = new Vector3(this.animController.Offset.x, 0.0f, this.animController.Offset.z);
      this.reorderingAnimUnderway = false;
      string anim_name = $"{this.GetComponent<Building>().Def.WidthInCells.ToString()}x{this.GetComponent<Building>().Def.HeightInCells.ToString()}_ungrab";
      if (!this.reorderArmController.HasAnimation((HashedString) anim_name))
        anim_name = "3x3_ungrab";
      this.reorderArmController.Play((HashedString) anim_name);
      this.reorderArmController.Queue((HashedString) "off");
      this.loopingSounds.StopSound(GlobalAssets.GetSound(this.reorderSound));
    }
    else if ((double) this.animController.Offset.y > 0.0)
      this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y - Time.unscaledDeltaTime * num, this.animController.Offset.z);
    else if ((double) this.animController.Offset.y < 0.0)
      this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y + Time.unscaledDeltaTime * num, this.animController.Offset.z);
    this.reorderArmController.Offset = this.animController.Offset;
  }

  private static void ProcessToBeRemoved()
  {
    if (ReorderableBuilding.toBeRemoved.Count <= 0)
      return;
    ReorderableBuilding.toBeRemoved.Sort((Comparison<ReorderableBuilding>) ((a, b) => (double) a.transform.position.y < (double) b.transform.position.y ? -1 : 1));
    for (int index = 0; index < ReorderableBuilding.toBeRemoved.Count; ++index)
      ReorderableBuilding.toBeRemoved[index].RemoveModule();
    ReorderableBuilding.toBeRemoved.Clear();
  }

  public void MoveVertical(int amount)
  {
    if (amount == 0)
      return;
    this.cancelShield = true;
    List<GameObject> buildings = new List<GameObject>();
    buildings.Add(this.gameObject);
    AttachableBuilding.GetAttachedAbove(this.GetComponent<AttachableBuilding>(), ref buildings);
    if (amount > 0)
      buildings.Reverse();
    foreach (GameObject go in buildings)
    {
      ReorderableBuilding.UnmarkBuilding(go, (AttachableBuilding) null);
      go.transform.SetPosition(Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(go), 0, amount), CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
      ReorderableBuilding.MarkBuilding(go, (AttachableBuilding) null);
      go.GetComponent<ReorderableBuilding>().ApplyAnimOffset((float) -amount);
    }
    if (amount <= 0)
      return;
    foreach (GameObject gameObject in buildings)
      gameObject.GetComponent<AttachableBuilding>().RegisterWithAttachPoint(true);
  }

  public void SwapWithAbove(bool selectOnComplete = true)
  {
    BuildingAttachPoint component1 = this.GetComponent<BuildingAttachPoint>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || (UnityEngine.Object) component1.points[0].attachedBuilding == (UnityEngine.Object) null)
      return;
    int cell = Grid.PosToCell(this.gameObject);
    ReorderableBuilding.UnmarkBuilding(this.gameObject, (AttachableBuilding) null);
    AttachableBuilding attachedBuilding1 = component1.points[0].attachedBuilding;
    BuildingAttachPoint component2 = attachedBuilding1.GetComponent<BuildingAttachPoint>();
    AttachableBuilding attachedBuilding2 = (UnityEngine.Object) component2 != (UnityEngine.Object) null ? component2.points[0].attachedBuilding : (AttachableBuilding) null;
    ReorderableBuilding.UnmarkBuilding(attachedBuilding1.gameObject, attachedBuilding2);
    Building component3 = attachedBuilding1.GetComponent<Building>();
    attachedBuilding1.transform.SetPosition(Grid.CellToPos(cell, CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
    ReorderableBuilding.MarkBuilding(attachedBuilding1.gameObject, (AttachableBuilding) null);
    this.transform.SetPosition(Grid.CellToPos(Grid.OffsetCell(cell, 0, component3.Def.HeightInCells), CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
    ReorderableBuilding.MarkBuilding(this.gameObject, attachedBuilding2);
    ReorderableBuilding.RebuildNetworks();
    this.ApplyAnimOffset((float) -component3.Def.HeightInCells);
    Building component4 = this.GetComponent<Building>();
    component3.GetComponent<ReorderableBuilding>().ApplyAnimOffset((float) component4.Def.HeightInCells);
    if (!selectOnComplete)
      return;
    SelectTool.Instance.Select(component4.GetComponent<KSelectable>());
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.GetComponent<BuildingUnderConstruction>() == (UnityEngine.Object) null && !this.HasTag(GameTags.RocketInSpace))
      this.RemoveModule();
    if (this.m_animLink != null)
      this.m_animLink.Unregister();
    base.OnCleanUp();
  }

  private void ApplyAnimOffset(float amount)
  {
    this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y + amount, this.animController.Offset.z);
    this.reorderArmController.Offset = this.animController.Offset;
    string anim_name = $"{this.GetComponent<Building>().Def.WidthInCells.ToString()}x{this.GetComponent<Building>().Def.HeightInCells.ToString()}_grab";
    if (!this.reorderArmController.HasAnimation((HashedString) anim_name))
      anim_name = "3x3_grab";
    this.reorderArmController.Play((HashedString) anim_name);
    this.reorderArmController.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.StartReorderingAnim);
  }

  private void StartReorderingAnim(HashedString data)
  {
    this.loopingSounds.StartSound(GlobalAssets.GetSound(this.reorderSound));
    this.reorderingAnimUnderway = true;
    this.reorderArmController.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.StartReorderingAnim);
    this.gameObject.Trigger(-1447108533);
  }

  public void SwapWithBelow(bool selectOnComplete = true)
  {
    if ((UnityEngine.Object) this.GetComponent<AttachableBuilding>() == (UnityEngine.Object) null || (UnityEngine.Object) this.GetComponent<AttachableBuilding>().GetAttachedTo() == (UnityEngine.Object) null)
      return;
    this.GetComponent<AttachableBuilding>().GetAttachedTo().GetComponent<ReorderableBuilding>().SwapWithAbove(!selectOnComplete);
    if (!selectOnComplete)
      return;
    SelectTool.Instance.Select(this.GetComponent<KSelectable>());
  }

  public bool CanMoveVertically(int moveAmount, GameObject ignoreBuilding = null)
  {
    if (moveAmount == 0)
      return true;
    BuildingAttachPoint component1 = this.GetComponent<BuildingAttachPoint>();
    AttachableBuilding component2 = this.GetComponent<AttachableBuilding>();
    if (moveAmount > 0)
    {
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.points[0].attachedBuilding != (UnityEngine.Object) null && (UnityEngine.Object) component1.points[0].attachedBuilding.gameObject != (UnityEngine.Object) ignoreBuilding && component1.points[0].attachedBuilding.HasTag(GameTags.RocketModule) && !component1.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(moveAmount))
        return false;
    }
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      BuildingAttachPoint attachedTo = component2.GetAttachedTo();
      if ((UnityEngine.Object) attachedTo != (UnityEngine.Object) null && (UnityEngine.Object) attachedTo.gameObject != (UnityEngine.Object) ignoreBuilding && !component2.GetAttachedTo().GetComponent<ReorderableBuilding>().CanMoveVertically(moveAmount))
        return false;
    }
    foreach (CellOffset occupiedOffset in this.GetOccupiedOffsets())
    {
      if (!ReorderableBuilding.CheckCellClear(Grid.OffsetCell(Grid.OffsetCell(Grid.PosToCell(this.gameObject), occupiedOffset), 0, moveAmount), this.gameObject))
        return false;
    }
    return true;
  }

  public static bool CheckCellClear(int checkCell, GameObject ignoreObject = null)
  {
    return Grid.IsValidCell(checkCell) && Grid.IsValidBuildingCell(checkCell) && !Grid.Solid[checkCell] && Grid.WorldIdx[checkCell] != byte.MaxValue && (!((UnityEngine.Object) Grid.Objects[checkCell, 1] != (UnityEngine.Object) null) || !((UnityEngine.Object) Grid.Objects[checkCell, 1] != (UnityEngine.Object) ignoreObject) || !((UnityEngine.Object) Grid.Objects[checkCell, 1].GetComponent<ReorderableBuilding>() == (UnityEngine.Object) null));
  }

  public GameObject ConvertModule(BuildingDef toModule, IList<Tag> materials)
  {
    int cell = Grid.PosToCell(this.gameObject);
    int amount1 = toModule.HeightInCells - this.GetComponent<Building>().Def.HeightInCells;
    this.gameObject.GetComponent<Building>();
    BuildingAttachPoint component1 = this.gameObject.GetComponent<BuildingAttachPoint>();
    GameObject gameObject1 = (GameObject) null;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.points[0].attachedBuilding != (UnityEngine.Object) null)
    {
      gameObject1 = component1.points[0].attachedBuilding.gameObject;
      component1.points[0].attachedBuilding = (AttachableBuilding) null;
      Components.BuildingAttachPoints.Remove(component1);
    }
    ReorderableBuilding.UnmarkBuilding(this.gameObject, (AttachableBuilding) null);
    if (amount1 != 0 && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
      gameObject1.GetComponent<ReorderableBuilding>().MoveVertical(amount1);
    string fail_reason;
    if (!DebugHandler.InstantBuildMode && !toModule.IsValidPlaceLocation(this.gameObject, cell, Orientation.Neutral, out fail_reason))
    {
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, fail_reason, this.transform);
      if (amount1 != 0 && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
      {
        int amount2 = amount1 * -1;
        gameObject1.GetComponent<ReorderableBuilding>().MoveVertical(amount2);
      }
      ReorderableBuilding.MarkBuilding(this.gameObject, (UnityEngine.Object) gameObject1 != (UnityEngine.Object) null ? gameObject1.GetComponent<AttachableBuilding>() : (AttachableBuilding) null);
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
      {
        component1.points[0].attachedBuilding = gameObject1.GetComponent<AttachableBuilding>();
        Components.BuildingAttachPoints.Add(component1);
      }
      return (GameObject) null;
    }
    if (materials == null)
      materials = (IList<Tag>) toModule.DefaultElements();
    GameObject gameObject2 = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild ? toModule.Build(cell, Orientation.Neutral, (Storage) null, materials, 273.15f, timeBuilt: GameClock.Instance.GetTime()) : toModule.TryPlace(this.gameObject, Grid.CellToPosCBC(cell, toModule.SceneLayer), Orientation.Neutral, materials);
    RocketModuleCluster component2 = this.GetComponent<RocketModuleCluster>();
    RocketModuleCluster component3 = gameObject2.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component3 != (UnityEngine.Object) null)
      component2.CraftInterface.AddModule(component3);
    Deconstructable component4 = this.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
    {
      component4.SetAllowDeconstruction(true);
      component4.ForceDestroyAndGetMaterials();
    }
    else
      Util.KDestroyGameObject(this.gameObject);
    return gameObject2;
  }

  private CellOffset[] GetOccupiedOffsets()
  {
    OccupyArea component = this.GetComponent<OccupyArea>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? component.OccupiedCellsOffsets : this.GetComponent<BuildingUnderConstruction>().Def.PlacementOffsets;
  }

  public bool CanChangeModule()
  {
    if ((UnityEngine.Object) this.GetComponent<BuildingUnderConstruction>() != (UnityEngine.Object) null)
    {
      string prefabId1 = this.GetComponent<BuildingUnderConstruction>().Def.PrefabID;
    }
    else
    {
      string prefabId2 = this.GetComponent<Building>().Def.PrefabID;
    }
    RocketModuleCluster component = this.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) component.CraftInterface != (UnityEngine.Object) null)
      {
        if (component.CraftInterface.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
          return false;
      }
      else if ((UnityEngine.Object) component.conditionManager != (UnityEngine.Object) null && SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component.conditionManager).state != Spacecraft.MissionState.Grounded)
        return false;
    }
    return true;
  }

  public bool CanRemoveModule() => true;

  public bool CanSwapUp(bool alsoCheckAboveCanSwapDown = true)
  {
    BuildingAttachPoint component = this.GetComponent<BuildingAttachPoint>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || (UnityEngine.Object) this.GetComponent<AttachableBuilding>() == (UnityEngine.Object) null || (UnityEngine.Object) this.GetComponent<RocketEngineCluster>() != (UnityEngine.Object) null)
      return false;
    AttachableBuilding attachedBuilding = component.points[0].attachedBuilding;
    return !((UnityEngine.Object) attachedBuilding == (UnityEngine.Object) null) && !((UnityEngine.Object) attachedBuilding.GetComponent<BuildingAttachPoint>() == (UnityEngine.Object) null) && !attachedBuilding.HasTag(GameTags.NoseRocketModule) && this.CanMoveVertically(attachedBuilding.GetComponent<Building>().Def.HeightInCells, attachedBuilding.gameObject) && (!alsoCheckAboveCanSwapDown || attachedBuilding.GetComponent<ReorderableBuilding>().CanSwapDown(false));
  }

  public bool CanSwapDown(bool alsoCheckBelowCanSwapUp = true)
  {
    if (this.gameObject.HasTag(GameTags.NoseRocketModule))
      return false;
    AttachableBuilding component = this.GetComponent<AttachableBuilding>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return false;
    BuildingAttachPoint attachedTo = component.GetAttachedTo();
    return !((UnityEngine.Object) attachedTo == (UnityEngine.Object) null) && !((UnityEngine.Object) this.GetComponent<BuildingAttachPoint>() == (UnityEngine.Object) null) && !((UnityEngine.Object) attachedTo.GetComponent<AttachableBuilding>() == (UnityEngine.Object) null) && !((UnityEngine.Object) attachedTo.GetComponent<RocketEngineCluster>() != (UnityEngine.Object) null) && this.CanMoveVertically(attachedTo.GetComponent<Building>().Def.HeightInCells * -1, attachedTo.gameObject) && (!alsoCheckBelowCanSwapUp || attachedTo.GetComponent<ReorderableBuilding>().CanSwapUp(false));
  }

  public void ShowReorderArm(bool show)
  {
    if (!((UnityEngine.Object) this.reorderArmController != (UnityEngine.Object) null))
      return;
    this.reorderArmController.gameObject.SetActive(show);
  }

  private static void RebuildNetworks()
  {
    Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
    Game.Instance.gasConduitSystem.ForceRebuildNetworks();
    Game.Instance.liquidConduitSystem.ForceRebuildNetworks();
    Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
    Game.Instance.solidConduitSystem.ForceRebuildNetworks();
  }

  private static void UnmarkBuilding(GameObject go, AttachableBuilding aboveBuilding)
  {
    int cell = Grid.PosToCell(go);
    Building component1 = go.GetComponent<Building>();
    component1.Def.UnmarkArea(cell, component1.Orientation, component1.Def.ObjectLayer, go);
    AttachableBuilding component2 = go.GetComponent<AttachableBuilding>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.RegisterWithAttachPoint(false);
    if ((UnityEngine.Object) aboveBuilding != (UnityEngine.Object) null)
      aboveBuilding.RegisterWithAttachPoint(false);
    RocketModule component3 = go.GetComponent<RocketModule>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      component3.DeregisterComponents();
    RocketConduitSender[] components1 = go.GetComponents<RocketConduitSender>();
    if (components1.Length != 0)
    {
      foreach (RocketConduitSender rocketConduitSender in components1)
        rocketConduitSender.RemoveConduitPortFromNetwork();
    }
    RocketConduitReceiver[] components2 = go.GetComponents<RocketConduitReceiver>();
    if (components2.Length == 0)
      return;
    foreach (RocketConduitReceiver rocketConduitReceiver in components2)
      rocketConduitReceiver.RemoveConduitPortFromNetwork();
  }

  private static void MarkBuilding(GameObject go, AttachableBuilding aboveBuilding)
  {
    int cell = Grid.PosToCell(go);
    Building component1 = go.GetComponent<Building>();
    component1.Def.MarkArea(cell, component1.Orientation, component1.Def.ObjectLayer, go);
    if ((UnityEngine.Object) component1.GetComponent<OccupyArea>() != (UnityEngine.Object) null)
      component1.GetComponent<OccupyArea>().UpdateOccupiedArea();
    LogicPorts component2 = component1.GetComponent<LogicPorts>();
    if ((bool) (UnityEngine.Object) component2 && (UnityEngine.Object) go.GetComponent<BuildingComplete>() != (UnityEngine.Object) null)
      component2.OnMove();
    component1.GetComponent<AttachableBuilding>().RegisterWithAttachPoint(true);
    if ((UnityEngine.Object) aboveBuilding != (UnityEngine.Object) null)
      aboveBuilding.RegisterWithAttachPoint(true);
    RocketModule component3 = go.GetComponent<RocketModule>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      component3.RegisterComponents();
    VerticalModuleTiler component4 = go.GetComponent<VerticalModuleTiler>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      component4.PostReorderMove();
    RocketConduitSender[] components1 = go.GetComponents<RocketConduitSender>();
    if (components1.Length != 0)
    {
      foreach (RocketConduitSender rocketConduitSender in components1)
        rocketConduitSender.AddConduitPortToNetwork();
    }
    RocketConduitReceiver[] components2 = go.GetComponents<RocketConduitReceiver>();
    if (components2.Length == 0)
      return;
    foreach (RocketConduitReceiver rocketConduitReceiver in components2)
      rocketConduitReceiver.AddConduitPortToNetwork();
  }

  public enum MoveSource
  {
    Push,
    Pull,
  }
}
