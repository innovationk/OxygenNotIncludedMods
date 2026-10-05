// Decompiled with JetBrains decompiler
// Type: CellSelectionObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CellSelectionObject")]
public class CellSelectionObject : KMonoBehaviour, ICellSelectionProxy
{
  private static CellSelectionObject selectionObjectA;
  private static CellSelectionObject selectionObjectB;
  [HideInInspector]
  public CellSelectionObject alternateSelectionObject;
  private float zDepth = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection) - 0.6f;
  private float zDepthSelected = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection);
  private KBoxCollider2D mCollider;
  private KSelectable mSelectable;
  private Vector3 offset = new Vector3(0.5f, 0.5f, 0.0f);
  public GameObject SelectedDisplaySprite;
  public Sprite Sprite_Selected;
  public Sprite Sprite_Hover;
  public int mouseCell;
  private int selectedCell;
  public string ElementName;
  public Element element;
  public Element.State state;
  public float Mass;
  public float temperature;
  public Tag tags;
  public byte diseaseIdx;
  public int diseaseCount;
  public List<BubbleManager.CellBubbleInfo> bubbleInfos = new List<BubbleManager.CellBubbleInfo>();
  private Guid bubbleStatusHandle = Guid.Empty;
  private float updateTimer;
  private Dictionary<HashedString, Func<bool>> overlayFilterMap = new Dictionary<HashedString, Func<bool>>();
  private bool isAppFocused = true;

  public KSelectable Selectable => this.mSelectable;

  Element ICellSelectionProxy.Element => this.element;

  public int SelectedCell => this.selectedCell;

  public float FlowRate => Grid.AccumulatedFlow[this.selectedCell] / 3f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.mCollider = this.GetComponent<KBoxCollider2D>();
    this.mCollider.size = new Vector2(1.1f, 1.1f);
    this.mSelectable = this.GetComponent<KSelectable>();
    this.SelectedDisplaySprite.transform.localScale = Vector3.one * (25f / 64f);
    this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
    this.Subscribe(Game.Instance.gameObject, 493375141, new Action<object>(this.ForceRefreshUserMenu));
    this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, (Func<bool>) (() => Grid.Element[this.mouseCell].IsGas));
    this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, (Func<bool>) (() => Grid.Element[this.mouseCell].IsGas));
    this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, (Func<bool>) (() => Grid.Element[this.mouseCell].IsLiquid));
    if ((UnityEngine.Object) CellSelectionObject.selectionObjectA == (UnityEngine.Object) null)
      CellSelectionObject.selectionObjectA = this;
    else if ((UnityEngine.Object) CellSelectionObject.selectionObjectB == (UnityEngine.Object) null)
      CellSelectionObject.selectionObjectB = this;
    else
      Debug.LogError((object) "CellSelectionObjects not properly cleaned up.");
  }

  protected override void OnCleanUp()
  {
    CellSelectionObject.selectionObjectA = (CellSelectionObject) null;
    CellSelectionObject.selectionObjectB = (CellSelectionObject) null;
    base.OnCleanUp();
  }

  public static bool IsSelectionObject(GameObject testObject)
  {
    return (UnityEngine.Object) testObject == (UnityEngine.Object) CellSelectionObject.selectionObjectA.gameObject || (UnityEngine.Object) testObject == (UnityEngine.Object) CellSelectionObject.selectionObjectB.gameObject;
  }

  private void OnApplicationFocus(bool focusStatus) => this.isAppFocused = focusStatus;

  private void Update()
  {
    if (!this.isAppFocused || (UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null || (UnityEngine.Object) Game.Instance == (UnityEngine.Object) null || !Game.Instance.GameStarted())
      return;
    this.SelectedDisplaySprite.SetActive(PlayerController.Instance.IsUsingDefaultTool() && !DebugHandler.HideUI);
    if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) this.mSelectable)
    {
      this.mouseCell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
      this.mCollider.enabled = !((UnityEngine.Object) this.alternateSelectionObject != (UnityEngine.Object) null) || !((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.alternateSelectionObject.Selectable) || this.mouseCell != this.alternateSelectionObject.SelectedCell;
      if (Grid.IsValidCell(this.mouseCell) && Grid.IsVisible(this.mouseCell))
      {
        bool flag = true;
        foreach (KeyValuePair<HashedString, Func<bool>> overlayFilter in this.overlayFilterMap)
        {
          if (overlayFilter.Value == null)
            Debug.LogWarning((object) "Filter value is null");
          else if ((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null)
            Debug.LogWarning((object) "Overlay screen Instance is null");
          else if (OverlayScreen.Instance.GetMode() == overlayFilter.Key)
          {
            flag = false;
            if (this.gameObject.layer != LayerMask.NameToLayer("MaskedOverlay"))
              this.gameObject.layer = LayerMask.NameToLayer("MaskedOverlay");
            if (!overlayFilter.Value())
            {
              this.SelectedDisplaySprite.SetActive(false);
              return;
            }
            break;
          }
        }
        if (flag && this.gameObject.layer != LayerMask.NameToLayer("Default"))
          this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.transform.SetPosition((Grid.CellToPos(this.mouseCell, 0.0f, 0.0f, 0.0f) + this.offset) with
        {
          z = this.zDepth
        });
        this.mSelectable.SetName(Grid.Element[this.mouseCell].name);
      }
      if ((UnityEngine.Object) SelectTool.Instance.hover != (UnityEngine.Object) this.mSelectable)
        this.SelectedDisplaySprite.SetActive(false);
    }
    this.updateTimer += Time.deltaTime;
    if ((double) this.updateTimer < 0.5)
      return;
    this.updateTimer = 0.0f;
    if (!((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.mSelectable))
      return;
    this.UpdateValues();
  }

  public void UpdateValues()
  {
    if (!Grid.IsValidCell(this.selectedCell))
      return;
    this.Mass = Grid.Mass[this.selectedCell];
    this.element = Grid.Element[this.selectedCell];
    this.ElementName = this.element.name;
    this.state = this.element.state;
    this.tags = this.element.GetMaterialCategoryTag();
    this.temperature = Grid.Temperature[this.selectedCell];
    this.diseaseIdx = Grid.DiseaseIdx[this.selectedCell];
    this.diseaseCount = Grid.DiseaseCount[this.selectedCell];
    this.mSelectable.SetName(Grid.Element[this.selectedCell].name);
    DetailsScreen.Instance.Trigger(-1514841199, (object) null);
    this.UpdateStatusItem();
    int cell = Grid.CellAbove(this.selectedCell);
    bool flag = this.element.IsLiquid && Grid.IsValidCell(cell) && (Grid.Element[cell].IsGas || Grid.Element[cell].IsVacuum);
    if (this.element.sublimateId != (SimHashes) 0 && this.element.IsSolid | flag)
    {
      this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, (object) this);
      bool all_not_gaseous;
      bool all_over_pressure;
      GameUtil.IsEmissionBlocked(this.selectedCell, out all_not_gaseous, out all_over_pressure);
      if (all_not_gaseous)
      {
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, (object) this);
        this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure);
      }
      else if (all_over_pressure)
      {
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, (object) this);
        this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked);
      }
      else
      {
        this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure);
        this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked);
      }
    }
    else
    {
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationEmitting);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure);
    }
    if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(this.selectedCell))
      this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BuriedItem, (object) this);
    else
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.BuriedItem, true);
    bool space = CellSelectionObject.IsExposedToSpace(this.selectedCell);
    this.mSelectable.ToggleStatusItem(Db.Get().MiscStatusItems.Space, space);
    this.UpdateBubbleStatusItem();
  }

  private void UpdateBubbleStatusItem()
  {
    this.bubbleInfos.Clear();
    if ((UnityEngine.Object) BubbleManager.instance != (UnityEngine.Object) null)
      BubbleManager.instance.GetBubblesInCell(this.selectedCell, this.bubbleInfos);
    if (this.bubbleInfos.Count == 0)
    {
      if (!(this.bubbleStatusHandle != Guid.Empty))
        return;
      this.mSelectable.RemoveStatusItem(this.bubbleStatusHandle);
      this.bubbleStatusHandle = Guid.Empty;
    }
    else if (this.bubbleStatusHandle != Guid.Empty)
    {
      this.mSelectable.RemoveStatusItem(this.bubbleStatusHandle, true);
      this.bubbleStatusHandle = this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BubbleContents, (object) this);
    }
    else
      this.bubbleStatusHandle = this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BubbleContents, (object) this);
  }

  public static bool IsExposedToSpace(int cell)
  {
    return Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space && !BackwallManager.HasBackwall(cell) && (UnityEngine.Object) Grid.Objects[cell, 2] == (UnityEngine.Object) null && (UnityEngine.Object) Grid.Objects[cell, 9] == (UnityEngine.Object) null && !Grid.HasDoor[cell];
  }

  private void UpdateStatusItem()
  {
    if (this.element.id == SimHashes.Vacuum || this.element.id == SimHashes.Void)
    {
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalCategory, true);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, true);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalMass, true);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalDisease, true);
    }
    else
    {
      if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalCategory))
      {
        Func<Element> data = (Func<Element>) (() => this.element);
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, (object) data);
      }
      if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalTemperature))
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, (object) this);
      if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalMass))
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalMass, (object) this);
      if (this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalDisease))
        return;
      this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalDisease, (object) this);
    }
  }

  public void OnObjectSelected(object o)
  {
    this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
    this.UpdateStatusItem();
    if (!((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.mSelectable))
      return;
    this.selectedCell = Grid.PosToCell(this.gameObject);
    this.UpdateValues();
    this.transform.SetPosition((Grid.CellToPos(this.selectedCell, 0.0f, 0.0f, 0.0f) + this.offset) with
    {
      z = this.zDepth
    });
    this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Selected;
  }

  public string MassString() => $"{this.Mass:0.00}";

  private void ForceRefreshUserMenu(object data) => Game.Instance.userMenu.Refresh(this.gameObject);
}
