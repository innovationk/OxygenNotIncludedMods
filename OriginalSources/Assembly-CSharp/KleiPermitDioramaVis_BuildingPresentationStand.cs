// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_BuildingPresentationStand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_BuildingPresentationStand : 
  KMonoBehaviour,
  IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private KBatchedAnimController buildingKAnim;
  private Alignment lastAlignment;
  private Vector2 anchorPos;
  public const float LEFT = -160f;
  public const float TOP = 156f;

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
  }

  public void ConfigureWith(PermitResource permit)
  {
    KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (BuildingFacadeResource) permit);
    KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.anchorPos, KleiPermitVisUtil.GetBuildingDef(permit), this.lastAlignment);
    KleiPermitVisUtil.AnimateIn(this.buildingKAnim);
  }

  public KleiPermitDioramaVis_BuildingPresentationStand WithAlignment(Alignment alignment)
  {
    this.lastAlignment = alignment;
    this.anchorPos = new Vector2(alignment.x.Remap((0.0f, 1f), (-160f, 160f)), alignment.y.Remap((0.0f, 1f), (-156f, 156f)));
    return this;
  }
}
