// Decompiled with JetBrains decompiler
// Type: CellSelectionInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CellSelectionInstantiator : MonoBehaviour
{
  public GameObject CellSelectionPrefab;

  private void Awake()
  {
    GameObject gameObject1 = Util.KInstantiate(this.CellSelectionPrefab, name: "WorldSelectionCollider");
    GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, name: "WorldSelectionCollider");
    CellSelectionObject component1 = gameObject1.GetComponent<CellSelectionObject>();
    CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
    component1.alternateSelectionObject = component2;
    component2.alternateSelectionObject = component1;
    CellSelectionInstantiator.CreateBackwallSelectionProxy();
  }

  private static void CreateBackwallSelectionProxy()
  {
    GameObject gameObject = new GameObject("BackwallSelectionCollider");
    gameObject.SetActive(false);
    gameObject.AddComponent<BackwallSelectionObject>();
    gameObject.AddComponent<KSelectable>().DisableSelectMarker = true;
    gameObject.AddComponent<KBoxCollider2D>();
    gameObject.SetActive(true);
  }
}
