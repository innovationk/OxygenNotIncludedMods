// Decompiled with JetBrains decompiler
// Type: WarpPortalSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WarpPortalSideScreen : SideScreenContent
{
  [SerializeField]
  private LocText label;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private LocText buttonLabel;
  [SerializeField]
  private KButton cancelButton;
  [SerializeField]
  private LocText cancelButtonLabel;
  [SerializeField]
  private WarpPortal target;
  [SerializeField]
  private GameObject contents;
  [SerializeField]
  private GameObject progressBar;
  [SerializeField]
  private LocText progressLabel;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.buttonLabel.SetText((string) STRINGS.UI.UISIDESCREENS.WARPPORTALSIDESCREEN.BUTTON);
    this.cancelButtonLabel.SetText((string) STRINGS.UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CANCELBUTTON);
    this.button.onClick += new System.Action(this.OnButtonClick);
    this.cancelButton.onClick += new System.Action(this.OnCancelClick);
    this.Refresh();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<WarpPortal>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    WarpPortal component = target.GetComponent<WarpPortal>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Target doesn't have a WarpPortal associated with it.");
    }
    else
    {
      this.target = component;
      target.GetComponent<Assignable>().OnAssign += new Action<IAssignableIdentity>(this.Refresh);
      this.Refresh();
    }
  }

  private void Update()
  {
    if (!this.progressBar.activeSelf)
      return;
    RectTransform rectTransform = this.progressBar.GetComponentsInChildren<Image>()[1].rectTransform;
    float num = this.target.rechargeProgress / 3000f;
    rectTransform.sizeDelta = new Vector2(rectTransform.transform.parent.GetComponent<LayoutElement>().minWidth * num, 24f);
    this.progressLabel.text = GameUtil.GetFormattedPercent(num * 100f);
  }

  private void OnButtonClick()
  {
    if (!this.target.ReadyToWarp)
      return;
    this.target.StartWarpSequence();
    this.Refresh();
  }

  private void OnCancelClick()
  {
    this.target.CancelAssignment();
    this.Refresh();
  }

  private void Refresh(object data = null)
  {
    this.progressBar.SetActive(false);
    this.cancelButton.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      if (this.target.ReadyToWarp)
      {
        this.label.text = (string) STRINGS.UI.UISIDESCREENS.WARPPORTALSIDESCREEN.WAITING;
        this.button.gameObject.SetActive(true);
        this.cancelButton.gameObject.SetActive(true);
      }
      else if (this.target.IsConsumed)
      {
        this.button.gameObject.SetActive(false);
        this.progressBar.SetActive(true);
        this.label.text = (string) STRINGS.UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CONSUMED;
      }
      else if (this.target.IsWorking)
      {
        this.label.text = (string) STRINGS.UI.UISIDESCREENS.WARPPORTALSIDESCREEN.UNDERWAY;
        this.button.gameObject.SetActive(false);
        this.cancelButton.gameObject.SetActive(true);
      }
      else
      {
        this.label.text = (string) STRINGS.UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
        this.button.gameObject.SetActive(false);
      }
    }
    else
    {
      this.label.text = (string) STRINGS.UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
      this.button.gameObject.SetActive(false);
    }
  }
}
