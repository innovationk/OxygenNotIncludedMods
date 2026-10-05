// Decompiled with JetBrains decompiler
// Type: DebugBaseTemplateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DebugBaseTemplateButton : KScreen
{
  private bool SaveAllBuildings;
  private bool SaveAllPickups;
  public KButton saveBaseButton;
  public KButton clearButton;
  private TemplateContainer pasteAndSelectAsset;
  public KButton AddSelectionButton;
  public KButton RemoveSelectionButton;
  public KButton clearSelectionButton;
  public KButton DestroyButton;
  public KButton DeconstructButton;
  public KButton MoveButton;
  public TemplateContainer moveAsset;
  public KInputTextField nameField;
  private string SaveName = "enter_template_name";
  public GameObject Placer;
  public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;
  public List<int> SelectedCells = new List<int>();

  public static DebugBaseTemplateButton Instance { get; private set; }

  public static void DestroyInstance()
  {
    DebugBaseTemplateButton.Instance = (DebugBaseTemplateButton) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DebugBaseTemplateButton.Instance = this;
    this.gameObject.SetActive(false);
    this.SetupLocText();
    this.ConsumeMouseScroll = true;
    KInputTextField nameField = this.nameField;
    nameField.onFocus = nameField.onFocus + (System.Action) (() => this.isEditing = true);
    this.nameField.onEndEdit.AddListener((UnityAction<string>) (_param1 => this.isEditing = false));
    this.nameField.onValueChanged.AddListener((UnityAction<string>) (_param1 => Util.ScrubInputField(this.nameField, true)));
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.ConsumeMouseScroll = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.saveBaseButton != (UnityEngine.Object) null)
    {
      this.saveBaseButton.onClick -= new System.Action(this.OnClickSaveBase);
      this.saveBaseButton.onClick += new System.Action(this.OnClickSaveBase);
    }
    if ((UnityEngine.Object) this.clearButton != (UnityEngine.Object) null)
    {
      this.clearButton.onClick -= new System.Action(this.OnClickClear);
      this.clearButton.onClick += new System.Action(this.OnClickClear);
    }
    if ((UnityEngine.Object) this.AddSelectionButton != (UnityEngine.Object) null)
    {
      this.AddSelectionButton.onClick -= new System.Action(this.OnClickAddSelection);
      this.AddSelectionButton.onClick += new System.Action(this.OnClickAddSelection);
    }
    if ((UnityEngine.Object) this.RemoveSelectionButton != (UnityEngine.Object) null)
    {
      this.RemoveSelectionButton.onClick -= new System.Action(this.OnClickRemoveSelection);
      this.RemoveSelectionButton.onClick += new System.Action(this.OnClickRemoveSelection);
    }
    if ((UnityEngine.Object) this.clearSelectionButton != (UnityEngine.Object) null)
    {
      this.clearSelectionButton.onClick -= new System.Action(this.OnClickClearSelection);
      this.clearSelectionButton.onClick += new System.Action(this.OnClickClearSelection);
    }
    if ((UnityEngine.Object) this.MoveButton != (UnityEngine.Object) null)
    {
      this.MoveButton.onClick -= new System.Action(this.OnClickMove);
      this.MoveButton.onClick += new System.Action(this.OnClickMove);
    }
    if ((UnityEngine.Object) this.DestroyButton != (UnityEngine.Object) null)
    {
      this.DestroyButton.onClick -= new System.Action(this.OnClickDestroySelection);
      this.DestroyButton.onClick += new System.Action(this.OnClickDestroySelection);
    }
    if (!((UnityEngine.Object) this.DeconstructButton != (UnityEngine.Object) null))
      return;
    this.DeconstructButton.onClick -= new System.Action(this.OnClickDeconstructSelection);
    this.DeconstructButton.onClick += new System.Action(this.OnClickDeconstructSelection);
  }

  private void SetupLocText()
  {
  }

  private void OnClickDestroySelection() => DebugTool.Instance.Activate(DebugTool.Type.Destroy);

  private void OnClickDeconstructSelection()
  {
    DebugTool.Instance.Activate(DebugTool.Type.Deconstruct);
  }

  private void OnClickMove()
  {
    DebugTool.Instance.DeactivateTool();
    this.moveAsset = this.GetSelectionAsAsset();
    StampTool.Instance.Activate(this.moveAsset);
  }

  private void OnClickAddSelection() => DebugTool.Instance.Activate(DebugTool.Type.AddSelection);

  private void OnClickRemoveSelection()
  {
    DebugTool.Instance.Activate(DebugTool.Type.RemoveSelection);
  }

  private void OnClickClearSelection()
  {
    this.ClearSelection();
    this.nameField.text = "";
  }

  private void OnClickClear() => DebugTool.Instance.Activate(DebugTool.Type.Clear);

  protected override void OnDeactivate()
  {
    if ((UnityEngine.Object) DebugTool.Instance != (UnityEngine.Object) null)
      DebugTool.Instance.DeactivateTool();
    base.OnDeactivate();
  }

  protected override void OnDisable()
  {
    if (!((UnityEngine.Object) DebugTool.Instance != (UnityEngine.Object) null))
      return;
    DebugTool.Instance.DeactivateTool();
  }

  private TemplateContainer GetSelectionAsAsset()
  {
    List<TemplateClasses.Cell> _cells = new List<TemplateClasses.Cell>();
    List<Prefab> _buildings = new List<Prefab>();
    List<Prefab> _pickupables = new List<Prefab>();
    List<Prefab> _primaryElementOres = new List<Prefab>();
    List<Prefab> _otherEntities = new List<Prefab>();
    HashSet<GameObject> _excludeEntities = new HashSet<GameObject>();
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (int selectedCell in this.SelectedCells)
    {
      num1 += (float) Grid.CellToXY(selectedCell).x;
      num2 += (float) Grid.CellToXY(selectedCell).y;
    }
    float num3;
    int rootX;
    int rootY;
    Grid.CellToXY(Grid.PosToCell(new Vector3(num1 / (float) this.SelectedCells.Count, num3 = num2 / (float) this.SelectedCells.Count, 0.0f)), out rootX, out rootY);
    for (int index = 0; index < this.SelectedCells.Count; ++index)
    {
      int selectedCell = this.SelectedCells[index];
      int x;
      int y;
      Grid.CellToXY(this.SelectedCells[index], out x, out y);
      _cells.Add(new TemplateClasses.Cell(x - rootX, y - rootY, selectedCell));
    }
    for (int idx = 0; idx < Components.BuildingCompletes.Count; ++idx)
    {
      BuildingComplete buildingComplete = Components.BuildingCompletes[idx];
      if (!_excludeEntities.Contains(buildingComplete.gameObject))
      {
        int cell = Grid.PosToCell((KMonoBehaviour) buildingComplete);
        int x;
        int y;
        Grid.CellToXY(cell, out x, out y);
        if (this.SaveAllBuildings || this.SelectedCells.Contains(cell))
        {
          foreach (int placementCell in buildingComplete.PlacementCells)
          {
            int xplace;
            int yplace;
            Grid.CellToXY(placementCell, out xplace, out yplace);
            string id = Grid.DiseaseIdx[placementCell] != byte.MaxValue ? Db.Get().Diseases[(int) Grid.DiseaseIdx[placementCell]].Id : (string) null;
            if (_cells.Find((Predicate<TemplateClasses.Cell>) (c => c.location_x == xplace - rootX && c.location_y == yplace - rootY)) == null)
              _cells.Add(new TemplateClasses.Cell(xplace - rootX, yplace - rootY, Grid.Element[placementCell].id, Grid.Temperature[placementCell], Grid.Mass[placementCell], id, Grid.DiseaseCount[placementCell]));
          }
          Orientation _rotation = Orientation.Neutral;
          Rotatable component1 = buildingComplete.gameObject.GetComponent<Rotatable>();
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
            _rotation = component1.GetOrientation();
          SimHashes _element1 = SimHashes.Void;
          float num4 = 280f;
          string _disease1 = (string) null;
          int _disease_count1 = 0;
          PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
          buildingComplete.GetComponent<KPrefabID>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            _element1 = component2.ElementID;
            num4 = component2.Temperature;
            _disease1 = component2.DiseaseIdx != byte.MaxValue ? Db.Get().Diseases[(int) component2.DiseaseIdx].Id : (string) null;
            _disease_count1 = component2.DiseaseCount;
          }
          List<Prefab.template_amount_value> templateAmountValueList1 = new List<Prefab.template_amount_value>();
          List<Prefab.template_amount_value> templateAmountValueList2 = new List<Prefab.template_amount_value>();
          foreach (AmountInstance modifier in buildingComplete.gameObject.GetAmounts().ModifierList)
            templateAmountValueList1.Add(new Prefab.template_amount_value(modifier.amount.Id, modifier.value));
          Battery component3 = buildingComplete.GetComponent<Battery>();
          if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          {
            float joulesAvailable = component3.JoulesAvailable;
            templateAmountValueList2.Add(new Prefab.template_amount_value("joulesAvailable", joulesAvailable));
          }
          Unsealable component4 = buildingComplete.GetComponent<Unsealable>();
          if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
          {
            float num5 = component4.facingRight ? 1f : 0.0f;
            templateAmountValueList2.Add(new Prefab.template_amount_value("sealedDoorDirection", num5));
          }
          LogicSwitch component5 = buildingComplete.GetComponent<LogicSwitch>();
          if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
          {
            float num6 = component5.IsSwitchedOn ? 1f : 0.0f;
            templateAmountValueList2.Add(new Prefab.template_amount_value("switchSetting", num6));
          }
          int _connections = 0;
          IHaveUtilityNetworkMgr component6 = buildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
          if (component6 != null)
            _connections = (int) component6.GetNetworkManager().GetConnections(cell, true);
          string facadeIdId = (string) null;
          BuildingFacade component7 = buildingComplete.GetComponent<BuildingFacade>();
          if ((UnityEngine.Object) component7 != (UnityEngine.Object) null)
            facadeIdId = component7.CurrentFacade;
          x -= rootX;
          y -= rootY;
          float _temperature = Mathf.Clamp(num4, 1f, 99999f);
          Prefab prefab = new Prefab(buildingComplete.PrefabID().Name, Prefab.Type.Building, x, y, _element1, _temperature, 0.0f, _disease1, _disease_count1, _rotation, templateAmountValueList1.ToArray(), templateAmountValueList2.ToArray(), _connections, facadeIdId);
          LoreBearer component8 = buildingComplete.GetComponent<LoreBearer>();
          if ((UnityEngine.Object) component8 != (UnityEngine.Object) null && !string.IsNullOrEmpty(component8.poiOverrideLoreUnlockId))
          {
            prefab.loreUnlockId = component8.poiOverrideLoreUnlockId;
            prefab.loreDisplayText = component8.poiOverrideLoreDisplayText;
            prefab.loreNextCollectionId = component8.poiOverrideNextCollectionId;
          }
          Storage component9 = buildingComplete.gameObject.GetComponent<Storage>();
          if ((UnityEngine.Object) component9 != (UnityEngine.Object) null)
          {
            foreach (GameObject go in component9.items)
            {
              float _units = 0.0f;
              SimHashes _element2 = SimHashes.Vacuum;
              float _temp = 280f;
              string _disease2 = (string) null;
              int _disease_count2 = 0;
              bool _isOre = false;
              PrimaryElement component10 = go.GetComponent<PrimaryElement>();
              if ((UnityEngine.Object) component10 != (UnityEngine.Object) null)
              {
                _units = component10.Units;
                _element2 = component10.ElementID;
                _temp = component10.Temperature;
                _disease2 = component10.DiseaseIdx != byte.MaxValue ? Db.Get().Diseases[(int) component10.DiseaseIdx].Id : (string) null;
                _disease_count2 = component10.DiseaseCount;
              }
              Rottable.Instance smi = go.gameObject.GetSMI<Rottable.Instance>();
              if ((UnityEngine.Object) go.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
                _isOre = true;
              StorageItem _storage = new StorageItem(go.PrefabID().Name, _units, _temp, _element2, _disease2, _disease_count2, _isOre);
              if (smi != null)
                _storage.rottable.rotAmount = smi.RotValue;
              prefab.AssignStorage(_storage);
              _excludeEntities.Add(go);
            }
          }
          _buildings.Add(prefab);
          _excludeEntities.Add(buildingComplete.gameObject);
        }
      }
    }
    for (int idx = 0; idx < Components.Pickupables.Count; ++idx)
    {
      if (Components.Pickupables[idx].gameObject.activeSelf)
      {
        Pickupable pickupable = Components.Pickupables[idx];
        if (!_excludeEntities.Contains(pickupable.gameObject))
        {
          int cell = Grid.PosToCell((KMonoBehaviour) pickupable);
          if ((this.SaveAllPickups || this.SelectedCells.Contains(cell)) && !(bool) (UnityEngine.Object) Components.Pickupables[idx].gameObject.GetComponent<MinionBrain>())
          {
            int x;
            int y;
            Grid.CellToXY(cell, out x, out y);
            x -= rootX;
            y -= rootY;
            SimHashes _element = SimHashes.Void;
            float _temperature = 280f;
            float _units = 1f;
            string _disease = (string) null;
            int _disease_count = 0;
            float num7 = 0.0f;
            Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
            if (smi != null)
              num7 = smi.RotValue;
            PrimaryElement component = pickupable.gameObject.GetComponent<PrimaryElement>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              _element = component.ElementID;
              _units = component.Units;
              _temperature = component.Temperature;
              _disease = component.DiseaseIdx != byte.MaxValue ? Db.Get().Diseases[(int) component.DiseaseIdx].Id : (string) null;
              _disease_count = component.DiseaseCount;
            }
            Tag tag;
            if ((UnityEngine.Object) pickupable.gameObject.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
            {
              tag = pickupable.PrefabID();
              Prefab prefab = new Prefab(tag.Name, Prefab.Type.Ore, x, y, _element, _temperature, _units, _disease, _disease_count);
              _primaryElementOres.Add(prefab);
            }
            else
            {
              tag = pickupable.PrefabID();
              Prefab prefab = new Prefab(tag.Name, Prefab.Type.Pickupable, x, y, _element, _temperature, _units, _disease, _disease_count)
              {
                rottable = new TemplateClasses.Rottable()
              };
              prefab.rottable.rotAmount = num7;
              _pickupables.Add(prefab);
            }
            _excludeEntities.Add(pickupable.gameObject);
          }
        }
      }
    }
    this.GetEntities<Crop>((IEnumerable<Crop>) Components.Crops.Items, rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Health>((IEnumerable<Health>) Components.Health.Items, rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Harvestable>((IEnumerable<Harvestable>) Components.Harvestables.Items, rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Edible>((IEnumerable<Edible>) Components.Edibles.Items, rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Geyser>(rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<OccupyArea>(rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<FogOfWarMask>(rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    _otherEntities.RemoveAll((Predicate<Prefab>) (x => Assets.GetPrefab((Tag) x.id).HasTag(GameTags.ExcludeFromTemplate)));
    TemplateContainer selectionAsAsset = new TemplateContainer();
    selectionAsAsset.Init(_cells, _buildings, _pickupables, _primaryElementOres, _otherEntities);
    return selectionAsAsset;
  }

  private void GetEntities<T>(
    int rootX,
    int rootY,
    ref List<Prefab> _primaryElementOres,
    ref List<Prefab> _otherEntities,
    ref HashSet<GameObject> _excludeEntities)
  {
    this.GetEntities<object>((IEnumerable<object>) UnityEngine.Object.FindObjectsByType(typeof (T), FindObjectsSortMode.InstanceID), rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
  }

  private void GetEntities<T>(
    IEnumerable<T> component_collection,
    int rootX,
    int rootY,
    ref List<Prefab> _primaryElementOres,
    ref List<Prefab> _otherEntities,
    ref HashSet<GameObject> _excludeEntities)
  {
    foreach (T component1 in component_collection)
    {
      if (!_excludeEntities.Contains(((object) component1 as KMonoBehaviour).gameObject) && ((object) component1 as KMonoBehaviour).gameObject.activeSelf)
      {
        int cell = Grid.PosToCell((object) component1 as KMonoBehaviour);
        if (this.SelectedCells.Contains(cell) && !(bool) (UnityEngine.Object) ((object) component1 as KMonoBehaviour).gameObject.GetComponent<MinionBrain>())
        {
          Orientation orientation = Orientation.Neutral;
          Rotatable component2 = ((object) component1 as KMonoBehaviour).GetComponent<Rotatable>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            orientation = component2.Orientation;
          int x;
          int y;
          Grid.CellToXY(cell, out x, out y);
          x -= rootX;
          y -= rootY;
          SimHashes simHashes = SimHashes.Void;
          float num1 = 280f;
          float num2 = 1f;
          string str = (string) null;
          int num3 = 0;
          PrimaryElement component3 = ((object) component1 as KMonoBehaviour).gameObject.GetComponent<PrimaryElement>();
          if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          {
            simHashes = component3.ElementID;
            num2 = component3.Units;
            num1 = component3.Temperature;
            str = component3.DiseaseIdx != byte.MaxValue ? Db.Get().Diseases[(int) component3.DiseaseIdx].Id : (string) null;
            num3 = component3.DiseaseCount;
          }
          List<Prefab.template_amount_value> templateAmountValueList = new List<Prefab.template_amount_value>();
          if (((object) component1 as KMonoBehaviour).gameObject.GetAmounts() != null)
          {
            foreach (AmountInstance modifier in ((object) component1 as KMonoBehaviour).gameObject.GetAmounts().ModifierList)
              templateAmountValueList.Add(new Prefab.template_amount_value(modifier.amount.Id, modifier.value));
          }
          if ((UnityEngine.Object) ((object) component1 as KMonoBehaviour).gameObject.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
          {
            string name = ((object) component1 as KMonoBehaviour).PrefabID().Name;
            int loc_x = x;
            int loc_y = y;
            int _element = (int) simHashes;
            double _temperature = (double) num1;
            double _units = (double) num2;
            string _disease = str;
            int _disease_count = num3;
            Prefab.template_amount_value[] array = templateAmountValueList.ToArray();
            int _rotation = (int) orientation;
            Prefab.template_amount_value[] _amount_values = array;
            Prefab prefab = new Prefab(name, Prefab.Type.Ore, loc_x, loc_y, (SimHashes) _element, (float) _temperature, (float) _units, _disease, _disease_count, (Orientation) _rotation, _amount_values);
            _primaryElementOres.Add(prefab);
            _excludeEntities.Add(((object) component1 as KMonoBehaviour).gameObject);
          }
          else
          {
            string name = ((object) component1 as KMonoBehaviour).PrefabID().Name;
            int loc_x = x;
            int loc_y = y;
            int _element = (int) simHashes;
            double _temperature = (double) num1;
            double _units = (double) num2;
            string _disease = str;
            int _disease_count = num3;
            Prefab.template_amount_value[] array = templateAmountValueList.ToArray();
            int _rotation = (int) orientation;
            Prefab.template_amount_value[] _amount_values = array;
            Prefab prefab = new Prefab(name, Prefab.Type.Other, loc_x, loc_y, (SimHashes) _element, (float) _temperature, (float) _units, _disease, _disease_count, (Orientation) _rotation, _amount_values);
            LoreBearer component4 = ((object) component1 as KMonoBehaviour).gameObject.GetComponent<LoreBearer>();
            if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && !string.IsNullOrEmpty(component4.poiOverrideLoreUnlockId))
            {
              prefab.loreUnlockId = component4.poiOverrideLoreUnlockId;
              prefab.loreDisplayText = component4.poiOverrideLoreDisplayText;
              prefab.loreNextCollectionId = component4.poiOverrideNextCollectionId;
            }
            _otherEntities.Add(prefab);
            _excludeEntities.Add(((object) component1 as KMonoBehaviour).gameObject);
          }
        }
      }
    }
  }

  private static int CountLoreOverrides(TemplateContainer template)
  {
    int num = 0;
    if (template.buildings != null)
    {
      foreach (Prefab building in template.buildings)
      {
        if (!string.IsNullOrEmpty(building.loreUnlockId))
          ++num;
      }
    }
    if (template.otherEntities != null)
    {
      foreach (Prefab otherEntity in template.otherEntities)
      {
        if (!string.IsNullOrEmpty(otherEntity.loreUnlockId))
          ++num;
      }
    }
    return num;
  }

  private void OnClickSaveBase()
  {
    TemplateContainer asset = this.GetSelectionAsAsset();
    if (this.SelectedCells.Count <= 0)
    {
      Debug.LogWarning((object) "No cells selected. Use buttons above to select the area you want to save.");
    }
    else
    {
      this.SaveName = this.nameField.text;
      if (this.SaveName == null || this.SaveName == "")
      {
        Debug.LogWarning((object) "Invalid save name. Please enter a name in the input field.");
      }
      else
      {
        if (TemplateCache.TemplateExists(this.SaveName))
        {
          int num1 = DebugBaseTemplateButton.CountLoreOverrides(TemplateCache.GetTemplate(this.SaveName));
          int num2 = DebugBaseTemplateButton.CountLoreOverrides(asset);
          if (num2 < num1)
          {
            Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true).PopupConfirmDialog(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.LORE_OVERRIDE_WARNING.Replace("{newCount}", num2.ToString()).Replace("{oldCount}", num1.ToString()), (System.Action) (() => this.DoSaveTemplate(asset)), (System.Action) (() => { }));
            return;
          }
        }
        this.DoSaveTemplate(asset);
      }
    }
  }

  private void DoSaveTemplate(TemplateContainer asset)
  {
    asset.SaveToYaml(this.SaveName);
    TemplateCache.Clear();
    TemplateCache.Init();
    PasteBaseTemplateScreen.Instance.RefreshStampButtons();
  }

  public void ClearSelection()
  {
    for (int index = this.SelectedCells.Count - 1; index >= 0; --index)
      this.RemoveFromSelection(this.SelectedCells[index]);
  }

  public void DestroySelection()
  {
  }

  public void DeconstructSelection()
  {
  }

  public void AddToSelection(int cell)
  {
    if (this.SelectedCells.Contains(cell))
      return;
    GameObject gameObject = Util.KInstantiate(this.Placer);
    Grid.Objects[cell, 7] = gameObject;
    Vector3 posCbc = Grid.CellToPosCBC(cell, this.visualizerLayer);
    float num = -0.15f;
    posCbc.z += num;
    gameObject.transform.SetPosition(posCbc);
    this.SelectedCells.Add(cell);
  }

  public void RemoveFromSelection(int cell)
  {
    if (!this.SelectedCells.Contains(cell))
      return;
    GameObject go = Grid.Objects[cell, 7];
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      go.DeleteObject();
    this.SelectedCells.Remove(cell);
  }
}
