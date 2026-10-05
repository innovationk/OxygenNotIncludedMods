// Decompiled with JetBrains decompiler
// Type: LoreBearerSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LoreBearerSideScreen : SideScreenContent
{
  public const int DefaultButtonMenuSideScreenSortOrder = 20;
  public KButton button;
  private LoreBearer target;

  public override bool IsValidForTarget(GameObject target)
  {
    LoreBearer component = target.GetComponent<LoreBearer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.hideLore)
      return false;
    return component.useDefaultLore || !component.poiOverrideLoreUnlockId.IsNullOrWhiteSpace();
  }

  public override int GetSideScreenSortOrder() => this.target.GetSideScreenSortOrder();

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<LoreBearer>();
      this.Refresh();
    }
  }

  private void Refresh()
  {
    this.button.isInteractable = this.target.SidescreenButtonInteractable();
    this.button.ClearOnClick();
    this.button.onClick += new System.Action(this.target.OnSidescreenButtonPressed);
    this.button.onClick += new System.Action(this.Refresh);
    this.button.GetComponentInChildren<LocText>().SetText(this.target.SidescreenButtonText);
    this.button.GetComponent<ToolTip>().SetSimpleTooltip(this.target.SidescreenButtonTooltip);
  }
}
