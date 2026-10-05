// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_BuildingOnBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class KleiPermitDioramaVis_BuildingOnBackground : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private KBatchedAnimController buildingKAnimPrefab;
  private KBatchedAnimController[] buildingKAnimArray;

  public void ConfigureSetup()
  {
    this.buildingKAnimPrefab.gameObject.SetActive(false);
    this.buildingKAnimArray = new KBatchedAnimController[9];
    for (int index = 0; index < this.buildingKAnimArray.Length; ++index)
      this.buildingKAnimArray[index] = (KBatchedAnimController) Object.Instantiate((Object) this.buildingKAnimPrefab, this.buildingKAnimPrefab.transform.parent, false);
    Vector2 anchoredPosition = this.buildingKAnimPrefab.rectTransform().anchoredPosition;
    Vector2 vector2_1 = 175f * Vector2.one;
    Vector2 vector2_2 = vector2_1 * new Vector2(-1f, 0.0f);
    Vector2 vector2_3 = anchoredPosition + vector2_2;
    int index1 = 0;
    for (int x = 0; x < 3; ++x)
    {
      int y = 0;
      while (y < 3)
      {
        this.buildingKAnimArray[index1].rectTransform().anchoredPosition = vector2_3 + vector2_1 * new Vector2((float) x, (float) y);
        this.buildingKAnimArray[index1].gameObject.SetActive(true);
        ++y;
        ++index1;
      }
    }
  }

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureWith(PermitResource permit)
  {
    BuildingFacadeResource buildingPermit = (BuildingFacadeResource) permit;
    BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
    DebugUtil.DevAssert(buildingDef.WidthInCells == 1, "assert failed");
    DebugUtil.DevAssert(buildingDef.HeightInCells == 1, "assert failed");
    foreach (KBatchedAnimController buildingKanim in this.buildingKAnimArray)
      KleiPermitVisUtil.ConfigureToRenderBuilding(buildingKanim, buildingPermit);
  }
}
