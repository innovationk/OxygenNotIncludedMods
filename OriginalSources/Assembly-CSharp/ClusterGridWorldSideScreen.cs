// Decompiled with JetBrains decompiler
// Type: ClusterGridWorldSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ClusterGridWorldSideScreen : SideScreenContent
{
  public Image icon;
  public KButton viewButton;
  private AsteroidGridEntity targetEntity;

  protected override void OnSpawn() => this.viewButton.onClick += new System.Action(this.OnClickView);

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<AsteroidGridEntity>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetEntity = target.GetComponent<AsteroidGridEntity>();
    this.icon.sprite = Def.GetUISprite((object) this.targetEntity).first;
    WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
    bool flag = (UnityEngine.Object) component != (UnityEngine.Object) null && component.IsDiscovered;
    this.viewButton.isInteractable = flag;
    if (!flag)
      this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_DISABLE_TOOLTIP);
    else
      this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_TOOLTIP);
  }

  private void OnClickView()
  {
    WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
    if (!component.IsDupeVisited)
      component.LookAtSurface();
    ClusterManager.Instance.SetActiveWorld(component.id);
    ManagementMenu.Instance.CloseAll();
  }
}
