// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_BuildingHangingHook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_BuildingHangingHook : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private KBatchedAnimController buildingKAnim;
  private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
  }

  public void ConfigureWith(PermitResource permit)
  {
    KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (BuildingFacadeResource) permit);
    KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.buildingKAnimPosition, KleiPermitVisUtil.GetBuildingDef(permit), Alignment.Top());
    KleiPermitVisUtil.AnimateIn(this.buildingKAnim);
  }
}
