// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_ArtablePainting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_ArtablePainting : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private KBatchedAnimController buildingKAnim;
  private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
    SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
  }

  public void ConfigureWith(PermitResource permit)
  {
    KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (ArtableStage) permit);
    BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
    this.buildingKAnimPosition.SetOn((Component) this.buildingKAnim);
    this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0.0f, (float) (-176.0 * (double) buildingDef.HeightInCells / 2.0 + 176.0));
    this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.9f;
    KleiPermitVisUtil.AnimateIn(this.buildingKAnim);
  }
}
