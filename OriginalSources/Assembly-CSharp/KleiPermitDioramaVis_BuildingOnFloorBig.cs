// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_BuildingOnFloorBig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_BuildingOnFloorBig : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private KBatchedAnimController buildingKAnim;
  private Vector2 defaultAnchoredPosition;

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
    this.defaultAnchoredPosition = this.buildingKAnim.rectTransform().anchoredPosition;
  }

  public void ConfigureWith(PermitResource permit)
  {
    BuildingFacadeResource buildingPermit = (BuildingFacadeResource) permit;
    this.buildingKAnim.SetSymbolVisiblity((KAnimHashedString) "booster", false);
    this.buildingKAnim.SetSymbolVisiblity((KAnimHashedString) "blue_light_bloom", false);
    this.buildingKAnim.rectTransform().anchoredPosition = this.defaultAnchoredPosition;
    this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.825f;
    string place_anim = "place";
    if (buildingPermit.PrefabID == "SteamTurbine2")
    {
      this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0.0f, 140f);
      place_anim = "place_alt";
    }
    KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
    KleiPermitVisUtil.AnimateIn(this.buildingKAnim, place_anim: place_anim);
  }
}
