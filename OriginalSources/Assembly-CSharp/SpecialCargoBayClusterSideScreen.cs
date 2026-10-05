// Decompiled with JetBrains decompiler
// Type: SpecialCargoBayClusterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SpecialCargoBayClusterSideScreen : ReceptacleSideScreen
{
  public LayoutElement descriptionContent;
  public float descriptionLayoutDefaultSize = -1f;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public override bool IsValidForTarget(GameObject target)
  {
    return (Object) target.GetComponent<SpecialCargoBayClusterReceptacle>() != (Object) null;
  }

  protected override bool RequiresAvailableAmountToDeposit() => false;

  protected override void UpdateState(object data)
  {
    base.UpdateState(data);
    this.SetDescriptionSidescreenFoldState((Object) this.targetReceptacle != (Object) null && (Object) this.targetReceptacle.Occupant == (Object) null);
  }

  protected override void SetResultDescriptions(GameObject go)
  {
    base.SetResultDescriptions(go);
    if ((Object) this.targetReceptacle != (Object) null && (Object) this.targetReceptacle.Occupant != (Object) null)
    {
      this.descriptionLabel.SetText("");
      this.SetDescriptionSidescreenFoldState(false);
    }
    else
      this.SetDescriptionSidescreenFoldState(true);
  }

  public void SetDescriptionSidescreenFoldState(bool visible)
  {
    this.descriptionContent.minHeight = visible ? this.descriptionLayoutDefaultSize : 0.0f;
  }
}
