// Decompiled with JetBrains decompiler
// Type: InfoDialogScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class InfoDialogScreen : KModalScreen
{
  [SerializeField]
  private InfoScreenPlainText subHeaderTemplate;
  [SerializeField]
  private InfoScreenPlainText plainTextTemplate;
  [SerializeField]
  private InfoScreenLineItem lineItemTemplate;
  [SerializeField]
  private InfoScreenSpriteItem spriteItemTemplate;
  [Space(10f)]
  [SerializeField]
  private LocText header;
  [SerializeField]
  private GameObject contentContainer;
  [SerializeField]
  private GameObject leftButtonPrefab;
  [SerializeField]
  private GameObject rightButtonPrefab;
  [SerializeField]
  private GameObject leftButtonPanel;
  [SerializeField]
  private GameObject rightButtonPanel;
  private bool escapeCloses;
  public System.Action onDeactivateFn;

  public InfoScreenPlainText GetSubHeaderPrefab() => this.subHeaderTemplate;

  public InfoScreenPlainText GetPlainTextPrefab() => this.plainTextTemplate;

  public InfoScreenLineItem GetLineItemPrefab() => this.lineItemTemplate;

  public GameObject GetPrimaryButtonPrefab() => this.leftButtonPrefab;

  public GameObject GetSecondaryButtonPrefab() => this.rightButtonPrefab;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.SetActive(false);
  }

  public override bool IsModal() => true;

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!this.escapeCloses)
      e.TryConsume(Action.Escape);
    else if (e.TryConsume(Action.Escape))
      this.Deactivate();
    else if ((UnityEngine.Object) PlayerController.Instance != (UnityEngine.Object) null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show || this.onDeactivateFn == null)
      return;
    this.onDeactivateFn();
  }

  public InfoDialogScreen AddDefaultOK(bool escapeCloses = false)
  {
    this.AddOption((string) STRINGS.UI.CONFIRMDIALOG.OK, (Action<InfoDialogScreen>) (d => d.Deactivate()), true);
    this.escapeCloses = escapeCloses;
    return this;
  }

  public InfoDialogScreen AddDefaultCancel()
  {
    this.AddOption((string) STRINGS.UI.CONFIRMDIALOG.CANCEL, (Action<InfoDialogScreen>) (d => d.Deactivate()));
    this.escapeCloses = true;
    return this;
  }

  public InfoDialogScreen AddOption(string text, Action<InfoDialogScreen> action, bool rightSide = false)
  {
    GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
    gameObject.gameObject.GetComponentInChildren<LocText>().text = text;
    gameObject.gameObject.GetComponent<KButton>().onClick += (System.Action) (() => action(this));
    return this;
  }

  public InfoDialogScreen AddOption(bool rightSide, out KButton button, out LocText buttonText)
  {
    GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
    button = gameObject.GetComponent<KButton>();
    buttonText = gameObject.GetComponentInChildren<LocText>();
    return this;
  }

  public InfoDialogScreen SetHeader(string header)
  {
    this.header.text = header;
    return this;
  }

  public InfoDialogScreen AddSprite(Sprite sprite)
  {
    Util.KInstantiateUI<InfoScreenSpriteItem>(this.spriteItemTemplate.gameObject, this.contentContainer).SetSprite(sprite);
    return this;
  }

  public InfoDialogScreen AddPlainText(string text)
  {
    Util.KInstantiateUI<InfoScreenPlainText>(this.plainTextTemplate.gameObject, this.contentContainer).SetText(text);
    return this;
  }

  public InfoDialogScreen AddLineItem(string text, string tooltip)
  {
    InfoScreenLineItem infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(this.lineItemTemplate.gameObject, this.contentContainer);
    infoScreenLineItem.SetText(text);
    infoScreenLineItem.SetTooltip(tooltip);
    return this;
  }

  public InfoDialogScreen AddSubHeader(string text)
  {
    Util.KInstantiateUI<InfoScreenPlainText>(this.subHeaderTemplate.gameObject, this.contentContainer).SetText(text);
    return this;
  }

  public InfoDialogScreen AddSpacer(float height)
  {
    GameObject gameObject = new GameObject("spacer");
    gameObject.SetActive(false);
    gameObject.transform.SetParent(this.contentContainer.transform, false);
    LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
    layoutElement.minHeight = height;
    layoutElement.preferredHeight = height;
    layoutElement.flexibleHeight = 0.0f;
    gameObject.SetActive(true);
    return this;
  }

  public InfoDialogScreen AddUI<T>(T prefab, out T spawn) where T : MonoBehaviour
  {
    spawn = Util.KInstantiateUI<T>(prefab.gameObject, this.contentContainer, true);
    return this;
  }

  public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors)
  {
    for (int index = 0; index < descriptors.Count; ++index)
      this.AddLineItem(descriptors[index].IndentedText(), descriptors[index].tooltipText);
    return this;
  }
}
