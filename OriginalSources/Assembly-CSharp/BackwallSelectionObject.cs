// Decompiled with JetBrains decompiler
// Type: BackwallSelectionObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class BackwallSelectionObject : KMonoBehaviour, ICellSelectionProxy
{
  private static BackwallSelectionObject instance;
  private KSelectable mSelectable;
  private KBoxCollider2D mCollider;
  private GameObject hoverCursor;
  private int selectedCell;
  private float updateTimer;
  public Element element;
  public float Mass;
  public float temperature;
  private static readonly Vector3 offset = new Vector3(0.5f, 0.5f, 0.0f);
  private float zDepth = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection) - 0.5f;
  private bool isAppFocused = true;

  public static BackwallSelectionObject Instance => BackwallSelectionObject.instance;

  public int SelectedCell => this.selectedCell;

  Element ICellSelectionProxy.Element => this.element;

  protected override void OnPrefabInit() => BackwallSelectionObject.instance = this;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.mSelectable = this.GetComponent<KSelectable>();
    this.mSelectable.IsSelectable = true;
    this.mCollider = this.GetComponent<KBoxCollider2D>();
    this.mCollider.size = new Vector2(1.1f, 1.1f);
    this.hoverCursor = BackwallSelectionObject.CreateHoverCursor(this.transform);
    this.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnObjectSelected));
  }

  private static GameObject CreateHoverCursor(Transform parent)
  {
    GameObject hoverCursor = new GameObject("Backwall Selection Hover Cursor");
    hoverCursor.transform.SetParent(parent, false);
    hoverCursor.transform.localPosition = new Vector3(0.0f, 0.0f, -10f);
    hoverCursor.transform.localScale = new Vector3(0.39f, 0.39f, 1f);
    SpriteRenderer spriteRenderer = hoverCursor.AddComponent<SpriteRenderer>();
    spriteRenderer.sprite = Assets.GetSprite((HashedString) "cursorIcon");
    spriteRenderer.sortingOrder = 1;
    return hoverCursor;
  }

  protected override void OnCleanUp()
  {
    BackwallSelectionObject.instance = (BackwallSelectionObject) null;
    base.OnCleanUp();
  }

  private void OnApplicationFocus(bool focusStatus) => this.isAppFocused = focusStatus;

  private void Update()
  {
    if (!this.isAppFocused || (UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null || (UnityEngine.Object) Game.Instance == (UnityEngine.Object) null || !Game.Instance.GameStarted() || !PlayerController.Instance.IsUsingDefaultTool())
      return;
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.mSelectable)
    {
      this.hoverCursor.SetActive(false);
      this.updateTimer += Time.deltaTime;
      if ((double) this.updateTimer < 0.5)
        return;
      this.updateTimer = 0.0f;
      this.UpdateValues();
    }
    else
    {
      int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
      bool flag1 = Grid.IsValidCell(cell) && Grid.IsVisible(cell) && BackwallManager.HasBackwall(cell);
      this.mCollider.enabled = flag1;
      bool flag2 = (UnityEngine.Object) SelectTool.Instance.hover == (UnityEngine.Object) this.mSelectable;
      this.hoverCursor.SetActive(flag1 & flag2);
      if (!flag1)
        return;
      this.transform.SetPosition((Grid.CellToPos(cell, 0.0f, 0.0f, 0.0f) + BackwallSelectionObject.offset) with
      {
        z = this.zDepth
      });
      this.mSelectable.SetName($"{BackwallManager.At(cell).Element.nameUpperCase} {(string) UI.TOOLS.GENERIC.NATURAL_BACKWALL_LABEL}");
    }
  }

  public void OnObjectSelected(object o)
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) this.mSelectable)
      return;
    this.selectedCell = Grid.PosToCell(this.gameObject);
    this.updateTimer = 0.0f;
    this.UpdateValues();
  }

  public void UpdateValues()
  {
    GameObject gameObject = Grid.Objects[this.selectedCell, 2];
    if (BackwallManager.HasBackwall(this.selectedCell))
    {
      BackwallManager.BackwallIndexer backwallIndexer = BackwallManager.At(this.SelectedCell);
      this.element = backwallIndexer.Element;
      backwallIndexer = BackwallManager.At(this.SelectedCell);
      this.Mass = backwallIndexer.Mass;
      backwallIndexer = BackwallManager.At(this.SelectedCell);
      this.temperature = backwallIndexer.Temperature;
    }
    else
    {
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return;
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      this.element = component.Element;
      this.Mass = component.Mass;
      this.temperature = component.Temperature;
    }
    this.mSelectable.SetName($"{this.element.name} {(string) UI.TOOLS.GENERIC.NATURAL_BACKWALL_LABEL_TITLECASE}");
    if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.BackwallMass))
      this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BackwallMass, (object) this);
    if (this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.BackwallTemperature))
      return;
    this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BackwallTemperature, (object) this);
  }

  public static bool IsBackwallSelectionObject(GameObject go)
  {
    return (UnityEngine.Object) BackwallSelectionObject.instance != (UnityEngine.Object) null && (UnityEngine.Object) go == (UnityEngine.Object) BackwallSelectionObject.instance.gameObject;
  }
}
