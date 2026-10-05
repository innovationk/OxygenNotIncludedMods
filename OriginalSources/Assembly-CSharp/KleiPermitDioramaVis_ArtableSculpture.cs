// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_ArtableSculpture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_ArtableSculpture : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private KBatchedAnimController buildingKAnim;

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
    SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
  }

  public void ConfigureWith(PermitResource permit)
  {
    KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (ArtableStage) permit);
    KleiPermitVisUtil.AnimateIn(this.buildingKAnim);
  }
}
