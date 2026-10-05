// Decompiled with JetBrains decompiler
// Type: KAnimLayering
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KAnimLayering
{
  public static readonly KAnimHashedString UI = new KAnimHashedString("ui");
  private static Dictionary<KAnim.SymbolFlags, KAnimBatchGroup.MaterialType> layerSettings = new Dictionary<KAnim.SymbolFlags, KAnimBatchGroup.MaterialType>()
  {
    {
      KAnim.SymbolFlags.FG,
      KAnimBatchGroup.MaterialType.Default
    },
    {
      KAnim.SymbolFlags.SH,
      KAnimBatchGroup.MaterialType.Shine
    }
  };
  private bool isLayer;
  private KAnimControllerBase controller;
  private Dictionary<KAnim.SymbolFlags, KAnimControllerBase> layerControllers;
  private Dictionary<KAnim.SymbolFlags, KAnimLink> links;
  private Grid.SceneLayer layer = Grid.SceneLayer.BuildingFront;

  public KAnimLayering(KAnimControllerBase controller, Grid.SceneLayer layer)
  {
    this.controller = controller;
    this.layer = layer;
  }

  public void SetLayer(Grid.SceneLayer layer)
  {
    this.layer = layer;
    if (this.layerControllers == null)
      return;
    foreach (Component component in this.layerControllers.Values)
      component.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, (float) ((double) Grid.GetLayerZ(layer) - (double) this.controller.gameObject.transform.GetPosition().z - 0.10000000149011612)));
  }

  public void SetIsLayer(bool is_layer) => this.isLayer = is_layer;

  public bool GetIsLayer() => this.isLayer;

  public void SetSyncLayeringTint(bool sync)
  {
    if (this.links == null)
      return;
    foreach (KeyValuePair<KAnim.SymbolFlags, KAnimLink> link in this.links)
      link.Value.syncTint = sync;
  }

  private static bool IsAnimLayered(KAnimFile[] anims, KAnim.SymbolFlags layer_flag)
  {
    for (int index = 0; index < anims.Length; ++index)
    {
      if (KAnimLayering.IsAnimFileLayered(anims[index], layer_flag))
        return true;
    }
    return false;
  }

  private static bool IsAnimFileLayered(KAnimFile anim_file, KAnim.SymbolFlags layer_flag)
  {
    if ((Object) anim_file == (Object) null)
      return false;
    KAnimFileData data = anim_file.GetData();
    if (data.build == null)
      return false;
    foreach (KAnim.Build.Symbol symbol in data.build.symbols)
    {
      if (((KAnim.SymbolFlags) symbol.flags & layer_flag) != (KAnim.SymbolFlags) 0)
        return true;
    }
    return false;
  }

  private static bool IsOverrideAnimLayered(
    IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> override_anims,
    KAnim.SymbolFlags layer_flag)
  {
    foreach (KAnimControllerBase.OverrideAnimFileData overrideAnim in (IEnumerable<KAnimControllerBase.OverrideAnimFileData>) override_anims)
    {
      if (KAnimLayering.IsAnimFileLayered(overrideAnim.file, layer_flag))
        return true;
    }
    return false;
  }

  private void HideSymbolsInternal(KAnim.SymbolFlags symbol_flag_to_hide)
  {
    foreach (KAnimFile animFile in this.controller.AnimFiles)
      this.SetAnimVisibility(animFile, symbol_flag_to_hide);
    IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = this.controller.OverrideAnimFiles;
    for (int index = 0; index < overrideAnimFiles.Count; ++index)
      this.SetAnimVisibility(overrideAnimFiles[index].file, symbol_flag_to_hide);
  }

  private void SetAnimVisibility(KAnimFile anim_file, KAnim.SymbolFlags symbol_flag)
  {
    if ((Object) anim_file == (Object) null)
      return;
    KAnimFileData data = anim_file.GetData();
    if (data.build == null)
      return;
    KAnim.Build.Symbol[] symbols = data.build.symbols;
    for (int index = 0; index < symbols.Length; ++index)
    {
      if (((KAnim.SymbolFlags) symbols[index].flags & symbol_flag) != 0 != this.isLayer && !(symbols[index].hash == KAnimLayering.UI))
        this.controller.SetSymbolVisiblity(symbols[index].hash, false);
    }
  }

  public void HideSymbols()
  {
    if ((Object) EntityPrefabs.Instance == (Object) null || this.isLayer)
      return;
    foreach (KAnim.SymbolFlags key in KAnimLayering.layerSettings.Keys)
    {
      bool flag1 = KAnimLayering.IsAnimLayered(this.controller.AnimFiles, key);
      bool flag2 = KAnimLayering.IsOverrideAnimLayered(this.controller.OverrideAnimFiles, key);
      bool flag3 = flag1 | flag2;
      if (flag3 && this.layer != Grid.SceneLayer.NoLayer)
      {
        int num = this.layerControllers == null ? 1 : (!this.layerControllers.ContainsKey(key) ? 1 : 0);
        if (num != 0)
        {
          if (this.layerControllers == null)
            this.layerControllers = new Dictionary<KAnim.SymbolFlags, KAnimControllerBase>();
          if (this.links == null)
            this.links = new Dictionary<KAnim.SymbolFlags, KAnimLink>();
          GameObject prefab = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, this.controller.gameObject);
          prefab.name = $"{this.controller.name}_{key.ToString().ToLower()}";
          KAnimControllerBase component = prefab.GetComponent<KAnimControllerBase>();
          if (flag2)
            SymbolOverrideControllerUtil.AddToPrefab(prefab).applySymbolOverridesEveryFrame = true;
          this.layerControllers.Add(key, component);
          this.links.Add(key, new KAnimLink(this.controller, component));
          component.materialType = KAnimLayering.layerSettings[key];
        }
        KAnimControllerBase layerController = this.layerControllers[key];
        layerController.AnimFiles = this.controller.AnimFiles;
        layerController.GetLayering().SetIsLayer(true);
        layerController.initialAnim = this.controller.initialAnim;
        this.Dirty();
        KAnimSynchronizer synchronizer = this.controller.GetSynchronizer();
        if (num != 0)
          synchronizer.Add(layerController);
        else
          this.RefreshForegroundBatchGroup();
        synchronizer.Sync(layerController);
        Vector3 position = new Vector3(0.0f, 0.0f, (float) ((double) Grid.GetLayerZ(this.layer) - (double) this.controller.gameObject.transform.GetPosition().z - 0.10000000149011612));
        layerController.gameObject.transform.SetLocalPosition(position);
        layerController.gameObject.SetActive(true);
        if (flag2)
        {
          foreach (KAnimControllerBase.OverrideAnimFileData overrideAnimFile in (IEnumerable<KAnimControllerBase.OverrideAnimFileData>) this.controller.OverrideAnimFiles)
            layerController.AddAnimOverrides(overrideAnimFile.file, overrideAnimFile.priority);
        }
      }
      else
      {
        KAnimControllerBase controller;
        if (!flag3 && this.layerControllers != null && this.layerControllers.Count != 0 && this.layerControllers.TryGetValue(key, out controller))
        {
          this.controller.GetSynchronizer().Remove(controller);
          controller.gameObject.DeleteObject();
          this.layerControllers.Remove(key);
          if (this.links != null)
          {
            this.links[key].Unregister();
            this.links.Remove(key);
          }
        }
      }
    }
    if (this.layerControllers == null)
      return;
    foreach (KeyValuePair<KAnim.SymbolFlags, KAnimControllerBase> layerController in this.layerControllers)
    {
      this.HideSymbolsInternal(layerController.Key);
      layerController.Value.GetLayering()?.HideSymbolsInternal(layerController.Key);
    }
  }

  private void RefreshForegroundBatchGroup()
  {
    if (this.layerControllers == null)
      return;
    foreach (KeyValuePair<KAnim.SymbolFlags, KAnimControllerBase> layerController in this.layerControllers)
    {
      foreach (KAnimControllerBase.OverrideAnimFileData overrideAnimFileData in new List<KAnimControllerBase.OverrideAnimFileData>((IEnumerable<KAnimControllerBase.OverrideAnimFileData>) layerController.Value.OverrideAnimFiles))
        layerController.Value.RemoveAnimOverrides(overrideAnimFileData.file);
      layerController.Value.GetComponent<KBatchedAnimController>().SwapAnims(layerController.Value.AnimFiles);
    }
  }

  public void Dirty()
  {
    if (this.layerControllers == null)
      return;
    foreach (KeyValuePair<KAnim.SymbolFlags, KAnimControllerBase> layerController in this.layerControllers)
    {
      layerController.Value.Offset = this.controller.Offset;
      layerController.Value.Pivot = this.controller.Pivot;
      layerController.Value.Rotation = this.controller.Rotation;
      layerController.Value.FlipX = this.controller.FlipX;
      layerController.Value.FlipY = this.controller.FlipY;
    }
  }
}
