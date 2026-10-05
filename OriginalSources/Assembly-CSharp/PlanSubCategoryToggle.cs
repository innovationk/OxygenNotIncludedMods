// Decompiled with JetBrains decompiler
// Type: PlanSubCategoryToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlanSubCategoryToggle : KMonoBehaviour
{
  [SerializeField]
  private MultiToggle toggle;
  [SerializeField]
  private GameObject gridContainer;
  private bool open = true;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.toggle.onClick += (System.Action) (() =>
    {
      this.open = !this.open;
      this.gridContainer.SetActive(this.open);
      this.toggle.ChangeState(this.open ? 0 : 1);
    });
  }
}
