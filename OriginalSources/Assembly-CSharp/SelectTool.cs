// Decompiled with JetBrains decompiler
// Type: SelectTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using UnityEngine;

#nullable disable
public class SelectTool : InterfaceTool
{
  public KSelectable selected;
  protected int cell_new;
  private int selectedCell;
  protected int defaultLayerMask;
  public static SelectTool Instance;
  private KSelectable delayedNextSelection;
  private bool delayedSkipSound;
  private KSelectable previousSelection;

  public static void DestroyInstance() => SelectTool.Instance = (SelectTool) null;

  protected override void OnPrefabInit()
  {
    this.defaultLayerMask = 1 | LayerMask.GetMask("World", "Pickupable", "Place", "PlaceWithDepth", "BlockSelection", "Construction", "Selection");
    this.layerMask = this.defaultLayerMask;
    this.selectMarker = Util.KInstantiateUI<SelectMarker>(EntityPrefabs.Instance.SelectMarker, GameScreenManager.Instance.worldSpaceCanvas);
    this.selectMarker.gameObject.SetActive(false);
    this.populateHitsList = true;
    SelectTool.Instance = this;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    ToolMenu.Instance.PriorityScreen.ResetPriority();
    this.Select((KSelectable) null);
  }

  public void SetLayerMask(int mask)
  {
    this.layerMask = mask;
    this.ClearHover();
    this.LateUpdate();
  }

  public void ClearLayerMask() => this.layerMask = this.defaultLayerMask;

  public int GetDefaultLayerMask() => this.defaultLayerMask;

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.ClearHover();
    this.Select((KSelectable) null);
  }

  public void Focus(Vector3 pos, KSelectable selectable, Vector3 offset)
  {
    if ((UnityEngine.Object) selectable != (UnityEngine.Object) null)
      pos = selectable.transform.GetPosition();
    pos.z = -40f;
    pos += offset;
    WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(pos);
    if ((UnityEngine.Object) worldFromPosition != (UnityEngine.Object) null)
      GameUtil.FocusCameraOnWorld(worldFromPosition.id, pos);
    else
      DebugUtil.DevLogError("DevError: specified camera focus position has null world - possible out of bounds location");
  }

  public void SelectAndFocus(Vector3 pos, KSelectable selectable, Vector3 offset)
  {
    this.Focus(pos, selectable, offset);
    this.Select(selectable);
  }

  public void SelectAndFocus(Vector3 pos, KSelectable selectable)
  {
    this.SelectAndFocus(pos, selectable, Vector3.zero);
  }

  public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
  {
    this.delayedNextSelection = new_selected;
    this.delayedSkipSound = skipSound;
    UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame));
  }

  private void DoSelectNextFrame(object data)
  {
    this.Select(this.delayedNextSelection, this.delayedSkipSound);
    this.delayedNextSelection = (KSelectable) null;
  }

  public void Select(KSelectable new_selected, bool skipSound = false)
  {
    if ((UnityEngine.Object) new_selected == (UnityEngine.Object) this.previousSelection)
      return;
    this.previousSelection = new_selected;
    if ((UnityEngine.Object) this.selected != (UnityEngine.Object) null)
      this.selected.Unselect();
    GameObject data = (GameObject) null;
    if ((UnityEngine.Object) new_selected != (UnityEngine.Object) null && new_selected.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
    {
      SelectToolHoverTextCard component = this.GetComponent<SelectToolHoverTextCard>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        int selectedSelectableIndex = component.currentSelectedSelectableIndex;
        int displayedSelectables = component.recentNumberOfDisplayedSelectables;
        if (displayedSelectables != 0)
        {
          int num1 = (selectedSelectableIndex + 1) % displayedSelectables;
          if (!skipSound)
          {
            if (displayedSelectables == 1)
            {
              KFMOD.PlayUISound(GlobalAssets.GetSound("Select_empty"));
            }
            else
            {
              EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Select_full"), Vector3.zero);
              int num2 = (int) instance.setParameterByName("selection", (float) num1);
              SoundEvent.EndOneShot(instance);
            }
            this.playedSoundThisFrame = true;
          }
        }
      }
      if ((UnityEngine.Object) new_selected == (UnityEngine.Object) this.hover)
        this.ClearHover();
      new_selected.Select();
      data = new_selected.gameObject;
      this.selectMarker.SetTargetTransform(data.transform);
      this.selectMarker.gameObject.SetActive(!new_selected.DisableSelectMarker);
    }
    else if ((UnityEngine.Object) this.selectMarker != (UnityEngine.Object) null)
      this.selectMarker.gameObject.SetActive(false);
    this.selected = (UnityEngine.Object) data == (UnityEngine.Object) null ? (KSelectable) null : new_selected;
    Game.Instance.Trigger(-1503271301, (object) data);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    KSelectable objectUnderCursor = this.GetObjectUnderCursor<KSelectable>(true, (Func<KSelectable, bool>) (s => s.GetComponent<KSelectable>().IsSelectable), (Component) this.selected);
    this.selectedCell = Grid.PosToCell(cursor_pos);
    this.Select(objectUnderCursor);
    if (DevToolSimDebug.Instance != null)
      DevToolSimDebug.Instance.SetCell(this.selectedCell);
    if (DevToolNavGrid.Instance == null)
      return;
    DevToolNavGrid.Instance.SetCell(this.selectedCell);
  }

  public int GetSelectedCell() => this.selectedCell;
}
