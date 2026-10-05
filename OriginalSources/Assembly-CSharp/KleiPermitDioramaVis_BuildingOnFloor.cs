// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_BuildingOnFloor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_BuildingOnFloor : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private KBatchedAnimController buildingKAnim;
  private Vector2 defaultScale;
  private RectTransform rectTransform;

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
    this.rectTransform = this.buildingKAnim.rectTransform();
    this.defaultScale = (Vector2) this.rectTransform.localScale;
  }

  public void ConfigureWith(PermitResource permit)
  {
    BuildingFacadeResource buildingPermit = (BuildingFacadeResource) permit;
    string place_anim = "place";
    this.buildingKAnim.SetSymbolVisiblity((KAnimHashedString) "sweep", false);
    if (buildingPermit.PrefabID == "LiquidPumpingStation")
    {
      this.rectTransform.localScale = Vector3.one * 0.7f;
      this.buildingKAnim.SetSymbolVisiblity((KAnimHashedString) "pipe2", false);
      this.buildingKAnim.SetSymbolVisiblity((KAnimHashedString) "pipe3", false);
      this.buildingKAnim.SetSymbolVisiblity((KAnimHashedString) "pipe4", false);
      place_anim = "place_alt";
    }
    else
      this.rectTransform.localScale = (Vector3) this.defaultScale;
    KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
    KleiPermitVisUtil.AnimateIn(this.buildingKAnim, place_anim: place_anim);
  }
}
