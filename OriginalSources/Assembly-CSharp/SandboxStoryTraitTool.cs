// Decompiled with JetBrains decompiler
// Type: SandboxStoryTraitTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxStoryTraitTool : InterfaceTool
{
  private System.Action setupPreviewFn;
  private StampToolPreview preview;
  private bool isPlacingTemplate;
  private string prevError;
  private const float ERROR_UPDATE_FREQUENCY = 0.1f;
  private float timeUntilNextErrorUpdate = -1f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.preview = new StampToolPreview((InterfaceTool) this, new IStampToolPreviewPlugin[3]
    {
      (IStampToolPreviewPlugin) new StampToolPreview_Area(),
      (IStampToolPreviewPlugin) new StampToolPreview_SolidLiquidGas(),
      (IStampToolPreviewPlugin) new StampToolPreview_Prefabs()
    });
    this.setupPreviewFn = (System.Action) (() =>
    {
      this.preview.Cleanup();
      TemplateContainer stampTemplate;
      if (!SandboxStoryTraitTool.TryGetStoryAndTemplate(out Story _, out stampTemplate))
        return;
      this.StartCoroutine(this.preview.Setup(stampTemplate));
      this.preview.OnErrorChange(this.prevError);
    });
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.storySelector.row.SetActive(true);
    this.setupPreviewFn();
    SandboxToolParameterMenu.instance.settings.OnChangeStory -= this.setupPreviewFn;
    SandboxToolParameterMenu.instance.settings.OnChangeStory += this.setupPreviewFn;
  }

  public void Update()
  {
    Vector3 cursorPos = PlayerController.GetCursorPos(KInputManager.GetMousePos());
    this.preview.Refresh(Grid.PosToCell(cursorPos));
    this.timeUntilNextErrorUpdate -= Time.unscaledDeltaTime;
    if ((double) this.timeUntilNextErrorUpdate > 0.0)
      return;
    this.timeUntilNextErrorUpdate = 0.1f;
    string error = this.GetError(cursorPos, out Story _, out TemplateContainer _);
    if (!(this.prevError != error))
      return;
    this.preview.OnErrorChange(error);
    this.prevError = error;
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
    SandboxToolParameterMenu.instance.settings.OnChangeStory -= this.setupPreviewFn;
    this.preview.Cleanup();
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    TemplateContainer stampTemplate;
    Story story;
    if (this.isPlacingTemplate || this.GetError(cursor_pos, out story, out stampTemplate) != null)
      return;
    this.isPlacingTemplate = true;
    SandboxStoryTraitTool.Stamp((Vector2) cursor_pos, stampTemplate, (System.Action) (() =>
    {
      this.isPlacingTemplate = false;
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(story);
      StoryInstance.State currentState = storyInstance.CurrentState;
      storyInstance.CurrentState = StoryInstance.State.RETROFITTED;
      storyInstance.CurrentState = currentState;
    }));
  }

  public static void Stamp(Vector2 pos, TemplateContainer stampTemplate, System.Action onCompleteFn)
  {
    bool shouldPauseOnComplete = SpeedControlScreen.Instance.IsPaused;
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause(false);
    if (stampTemplate.cells != null)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < stampTemplate.cells.Count; ++index)
      {
        for (int layer = 0; layer < 34; ++layer)
        {
          int cell = Grid.XYToCell((int) ((double) pos.x + (double) stampTemplate.cells[index].location_x), (int) ((double) pos.y + (double) stampTemplate.cells[index].location_y));
          GameObject gameObject = Grid.Objects[cell, layer];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && !gameObjectList.Contains(gameObject))
            gameObjectList.Add(gameObject);
        }
      }
      foreach (GameObject original in gameObjectList)
      {
        if ((UnityEngine.Object) original != (UnityEngine.Object) null)
          Util.KDestroyGameObject(original);
      }
    }
    TemplateLoader.Stamp(stampTemplate, pos, (System.Action) (() =>
    {
      if (shouldPauseOnComplete)
        SpeedControlScreen.Instance.Pause(false);
      onCompleteFn();
    }));
    KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Stamp"));
  }

  public static bool TryGetStoryAndTemplate(out Story story, out TemplateContainer stampTemplate)
  {
    stampTemplate = (TemplateContainer) null;
    string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedStory");
    story = Db.Get().Stories.TryGet(stringSetting);
    if (story == null || story.sandboxStampTemplateId == null)
      return false;
    stampTemplate = TemplateCache.GetTemplate(story.sandboxStampTemplateId);
    return stampTemplate != null;
  }

  public string GetError(Vector3 stampPos, out Story story, out TemplateContainer stampTemplate)
  {
    if (!SandboxStoryTraitTool.TryGetStoryAndTemplate(out story, out stampTemplate))
      return "-";
    TemplateContainer _stampTemplate = stampTemplate;
    if (StoryManager.Instance.GetStoryInstance(story) != null)
      return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_ALREADY_EXISTS.Replace("{StoryName}", (string) Strings.Get(story.StoryTrait.name));
    int cell1 = Grid.PosToCell(stampPos);
    Vector2f size = stampTemplate.info.size;
    int x1 = Mathf.FloorToInt((float) (-(double) size.X / 2.0));
    size = stampTemplate.info.size;
    int y1 = Mathf.FloorToInt((float) (-(double) size.Y / 2.0)) + 1;
    int cell2 = Grid.OffsetCell(cell1, x1, y1);
    int cell3 = Grid.PosToCell(stampPos);
    size = stampTemplate.info.size;
    int x2 = Mathf.FloorToInt(size.X / 2f);
    size = stampTemplate.info.size;
    int y2 = Mathf.FloorToInt(size.Y / 2f) + 1;
    int cell4 = Grid.OffsetCell(cell3, x2, y2);
    if ((!Grid.IsValidBuildingCell(cell2) || ClusterManager.Instance.activeWorldId != (int) Grid.WorldIdx[cell2] || !Grid.IsValidBuildingCell(cell4) || ClusterManager.Instance.activeWorldId != (int) Grid.WorldIdx[cell4] ? 0 : (!IsTrueForAnyStampCell((Func<TemplateClasses.Cell, int, bool>) ((cellInfo, cellIndex) => Grid.Element[cellIndex].id == SimHashes.Unobtanium)) ? 1 : 0)) == 0)
      return (string) UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_INVALID_LOCATION;
    WorldContainer world = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId);
    if ((UnityEngine.Object) world == (UnityEngine.Object) null || world.IsModuleInterior)
      return (string) UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_INVALID_LOCATION;
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    foreach (Brain brain1 in Components.Brains)
    {
      Brain brain = brain1;
      if (IsTrueForAnyStampCell((Func<TemplateClasses.Cell, int, bool>) ((cellInfo, cellIndex) =>
      {
        int cell5 = Grid.PosToCell(brain.gameObject);
        if (cell5 == cellIndex)
          return true;
        for (int x3 = -1; x3 <= 1; ++x3)
        {
          for (int y3 = -1; y3 <= 2; ++y3)
          {
            if (Grid.OffsetCell(cell5, x3, y3) == cellIndex)
              return true;
          }
        }
        return false;
      })))
      {
        if (brain.HasTag(GameTags.BaseMinion))
        {
          flag1 = true;
          break;
        }
        if (brain.HasTag(GameTags.Robot))
        {
          flag3 = true;
          break;
        }
        if (brain.HasTag(GameTags.Creature))
        {
          flag2 = true;
          break;
        }
        break;
      }
    }
    if (flag1)
      return (string) UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_DUPE_HAZARD;
    if (flag3)
      return (string) UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_ROBOT_HAZARD;
    if (flag2)
      return (string) UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_CREATURE_HAZARD;
    GameObject gameObject;
    return IsTrueForAnyStampCell((Func<TemplateClasses.Cell, int, bool>) ((cellInfo, cellIndex) => Grid.ObjectLayers[1].TryGetValue(cellIndex, out gameObject) && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Plant))) ? (string) UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_BUILDING_HAZARD : (string) null;

    bool IsTrueForAnyStampCell(Func<TemplateClasses.Cell, int, bool> isTrueFn)
    {
      foreach (TemplateClasses.Cell cell in _stampTemplate.cells)
      {
        int num = Grid.OffsetCell(Grid.PosToCell(stampPos), cell.location_x, cell.location_y);
        if (isTrueFn(cell, num))
          return true;
      }
      return false;
    }
  }
}
