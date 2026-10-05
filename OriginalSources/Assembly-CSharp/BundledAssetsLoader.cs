// Decompiled with JetBrains decompiler
// Type: BundledAssetsLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BundledAssetsLoader : KMonoBehaviour
{
  public static BundledAssetsLoader instance;

  public BundledAssets Expansion1Assets { get; private set; }

  public List<BundledAssets> DlcAssetsList { get; private set; }

  protected override void OnPrefabInit()
  {
    BundledAssetsLoader.instance = this;
    if (DlcManager.IsExpansion1Active())
    {
      Debug.Log((object) "Loading Expansion1 assets from bundle");
      AssetBundle assetBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName("EXPANSION1_ID")));
      Debug.Assert((Object) assetBundle != (Object) null, (object) "Expansion1 is Active but its asset bundle failed to load");
      GameObject original = assetBundle.LoadAsset<GameObject>("Expansion1Assets");
      Debug.Assert((Object) original != (Object) null, (object) "Could not load the Expansion1Assets prefab");
      this.Expansion1Assets = Util.KInstantiate(original, this.gameObject).GetComponent<BundledAssets>();
    }
    this.DlcAssetsList = new List<BundledAssets>(DlcManager.DLC_PACKS.Count);
    foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
    {
      if (DlcManager.IsContentSubscribed(keyValuePair.Key))
      {
        Debug.Log((object) $"Loading DLC {keyValuePair.Key} assets from bundle");
        AssetBundle assetBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName(keyValuePair.Key)));
        Debug.Assert((Object) assetBundle != (Object) null, (object) $"DLC {keyValuePair.Key} is Active but its asset bundle failed to load");
        GameObject original = assetBundle.LoadAsset<GameObject>(keyValuePair.Value.directory + "Assets");
        Debug.Assert((Object) original != (Object) null, (object) $"Could not load the {keyValuePair.Key} prefab");
        this.DlcAssetsList.Add(Util.KInstantiate(original, this.gameObject).GetComponent<BundledAssets>());
      }
    }
  }
}
