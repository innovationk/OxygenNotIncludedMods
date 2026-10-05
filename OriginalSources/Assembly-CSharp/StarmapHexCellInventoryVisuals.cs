// Decompiled with JetBrains decompiler
// Type: StarmapHexCellInventoryVisuals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StarmapHexCellInventoryVisuals : ClusterGridEntity
{
  public const int MAX_VISUAL_ITEMS = 6;
  public const string ANIM_FILE_NAME = "harvestable_elements_kanim";
  public const string DEFAULT_ANIM_STATE_NAME = "idle_6";
  public const string ANIM_STATE_NAME_PREFIX = "idle_";
  public const string SYMBOL_SWAP_NAME_PREFIX = "swap0";
  private static readonly HashedString GLOW_SYMBOL = (HashedString) "glow";
  public StarmapHexCellInventory inventory;
  private KBatchedAnimController animController;
  private KBatchedAnimController[] symbolAnimControllers;

  public override string Name => (string) UI.CLUSTERMAP.HEXCELL_INVENTORY.NAME;

  public override EntityLayer Layer => EntityLayer.Debri;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      return new List<ClusterGridEntity.AnimConfig>()
      {
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) "harvestable_elements_kanim"),
          initialAnim = "idle_6",
          playMode = KAnim.PlayMode.Loop,
          additionalInfo = (object) this
        }
      };
    }
  }

  public override bool IsVisible => true;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Hidden;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.inventory = this.GetComponent<StarmapHexCellInventory>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.inventory.RegisterInventory(this.Location))
    {
      StarmapHexCellInventory.AllInventories[this.Location].TransferAllItemsFromExternalInventory(this.inventory);
      this.gameObject.DeleteObject();
    }
    else
    {
      this.Subscribe(-1697596308, new Action<object>(this.RefreshVisuals));
      this.Subscribe(-1503271301, new Action<object>(this.OnSelectObject));
      this.RefreshVisuals((object) null);
    }
  }

  private void OnSelectObject(object data) => this.ToggleSelectionGlow(((Boxed<bool>) data).value);

  public void RefreshVisuals(object o) => this.RefreshVisuals();

  public void RefreshVisuals()
  {
    if ((UnityEngine.Object) ClusterMapScreen.Instance == (UnityEngine.Object) null || !ClusterMapScreen.Instance.isActiveAndEnabled)
      return;
    bool flag = this.inventory.ItemCount > 0;
    if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null)
    {
      int num = Mathf.Min(6, this.inventory.ItemCount);
      this.animController.Play((HashedString) ("idle_" + num.ToString()), KAnim.PlayMode.Loop);
      for (int index = 0; index < this.symbolAnimControllers.Length; ++index)
      {
        KBatchedAnimController symbolAnimController = this.symbolAnimControllers[index];
        KBatchedAnimTracker component1 = symbolAnimController.GetComponent<KBatchedAnimTracker>();
        if (index < num)
        {
          GameObject prefab = Assets.GetPrefab(this.inventory.Items[index].ID);
          Element element = ElementLoader.GetElement(prefab.PrefabID());
          KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
          string str = (element == null ? 0 : (element.IsLiquid ? 1 : 0)) != 0 ? "idle2" : (string.IsNullOrEmpty(component2.initialAnim) ? "object" : component2.initialAnim);
          KAnimFile fromPrefabWithTag = Def.GetAnimFileFromPrefabWithTag(prefab, str, out string _);
          symbolAnimController.SwapAnims(new KAnimFile[1]
          {
            fromPrefabWithTag
          });
          symbolAnimController.Play((HashedString) str);
          if (element != null)
          {
            Color colour = (Color) element.substance.colour with
            {
              a = 1f
            };
            if (!element.IsSolid)
              symbolAnimController.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
            if (element.IsGas)
              symbolAnimController.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
          }
          symbolAnimController.gameObject.SetActive(true);
          component1.forceAlwaysVisible = true;
        }
        else
        {
          component1.forceAlwaysVisible = false;
          symbolAnimController.gameObject.SetActive(false);
        }
      }
    }
    if (flag == this.m_selectable.IsSelectable)
      return;
    this.m_selectable.IsSelectable = flag;
  }

  public override void onClustermapVisualizerAnimCreated(
    KBatchedAnimController controller,
    ClusterGridEntity.AnimConfig config)
  {
    if (config.additionalInfo != this)
      return;
    this.animController = controller;
    this.SetupAnimControllerAndSymbols();
    this.RefreshVisuals((object) null);
  }

  private void ToggleSelectionGlow(bool glow)
  {
    this.animController.SetSymbolVisiblity((KAnimHashedString) StarmapHexCellInventoryVisuals.GLOW_SYMBOL, glow);
  }

  private void SetupAnimControllerAndSymbols()
  {
    this.DeleteSymbolControllers();
    if (!((UnityEngine.Object) this.animController != (UnityEngine.Object) null))
      return;
    this.animController.SetSymbolVisiblity((KAnimHashedString) StarmapHexCellInventoryVisuals.GLOW_SYMBOL, false);
    this.symbolAnimControllers = new KBatchedAnimController[6];
    for (int index = 0; index < this.symbolAnimControllers.Length; ++index)
    {
      KBatchedAnimController symbolController = this.CreateSymbolController("swap0" + (index + 1).ToString());
      this.symbolAnimControllers[index] = symbolController;
    }
  }

  private KBatchedAnimController CreateSymbolController(string symbolName)
  {
    KBatchedAnimController emptyKanimController = this.CreateEmptyKAnimController(symbolName);
    Matrix4x4 symbolTransform = this.animController.GetSymbolTransform((HashedString) symbolName, out bool _);
    Matrix2x3 symbolLocalTransform = this.animController.GetSymbolLocalTransform((HashedString) symbolName, out bool _);
    Vector3 column = (Vector3) symbolTransform.GetColumn(3);
    Vector3 vector3 = Vector3.one * symbolLocalTransform.m00;
    emptyKanimController.transform.SetParent(this.animController.transform, false);
    emptyKanimController.transform.SetPosition(column);
    emptyKanimController.transform.localPosition = emptyKanimController.transform.localPosition with
    {
      z = -1f / 400f
    };
    emptyKanimController.transform.localScale = vector3;
    KBatchedAnimTracker kbatchedAnimTracker = emptyKanimController.gameObject.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.controller = this.animController;
    kbatchedAnimTracker.symbol = new HashedString(symbolName);
    kbatchedAnimTracker.forceAlwaysVisible = false;
    emptyKanimController.gameObject.SetActive(false);
    this.animController.SetSymbolVisiblity((KAnimHashedString) symbolName, false);
    return emptyKanimController;
  }

  private KBatchedAnimController CreateEmptyKAnimController(string name)
  {
    GameObject gameObject = new GameObject($"{this.gameObject.name}-{name}");
    gameObject.SetActive(false);
    KBatchedAnimController emptyKanimController = gameObject.AddComponent<KBatchedAnimController>();
    emptyKanimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "harvestable_elements_kanim")
    };
    emptyKanimController.materialType = KAnimBatchGroup.MaterialType.UI;
    emptyKanimController.animScale = (UnityEngine.Object) this.animController == (UnityEngine.Object) null ? 0.08f : this.animController.animScale;
    emptyKanimController.fgLayer = Grid.SceneLayer.NoLayer;
    emptyKanimController.sceneLayer = Grid.SceneLayer.NoLayer;
    return emptyKanimController;
  }

  private void DeleteSymbolControllers()
  {
    if (this.symbolAnimControllers == null)
      return;
    for (int index = 0; index < this.symbolAnimControllers.Length; ++index)
    {
      KBatchedAnimController symbolAnimController = this.symbolAnimControllers[index];
      if ((UnityEngine.Object) symbolAnimController != (UnityEngine.Object) null)
        symbolAnimController.gameObject.DeleteObject();
    }
    this.symbolAnimControllers = (KBatchedAnimController[]) null;
  }
}
