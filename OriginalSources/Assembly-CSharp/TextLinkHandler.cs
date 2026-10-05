// Decompiled with JetBrains decompiler
// Type: TextLinkHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class TextLinkHandler : 
  MonoBehaviour,
  IPointerClickHandler,
  IEventSystemHandler,
  IPointerEnterHandler,
  IPointerExitHandler
{
  private static TextLinkHandler hoveredText;
  [MyCmpGet]
  private LocText text;
  private bool hoverLink;
  public Func<string, bool> overrideLinkAction;

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left || !this.text.AllowLinks)
      return;
    int intersectingLink = TMP_TextUtilities.FindIntersectingLink((TMP_Text) this.text, KInputManager.GetMousePos(), (Camera) null);
    if (intersectingLink == -1)
      return;
    string str = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[intersectingLink].GetLinkID());
    if (this.overrideLinkAction != null && !this.overrideLinkAction(str))
      return;
    if (!CodexCache.entries.ContainsKey(str))
    {
      SubEntry subEntry = CodexCache.FindSubEntry(str);
      if (subEntry == null || subEntry.disabled)
        str = "PAGENOTFOUND";
    }
    else if (CodexCache.entries[str].disabled)
      str = "PAGENOTFOUND";
    if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
      ManagementMenu.Instance.ToggleCodex();
    ManagementMenu.Instance.codexScreen.ChangeArticle(str, true);
  }

  private void Update()
  {
    this.CheckMouseOver();
    if (!((UnityEngine.Object) TextLinkHandler.hoveredText == (UnityEngine.Object) this) || !this.text.AllowLinks)
      return;
    PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
  }

  private void OnEnable() => this.CheckMouseOver();

  private void OnDisable() => this.ClearState();

  private void Awake()
  {
    this.text = this.GetComponent<LocText>();
    if (!this.text.AllowLinks || this.text.raycastTarget)
      return;
    this.text.raycastTarget = true;
  }

  public void OnPointerEnter(PointerEventData eventData) => this.SetMouseOver();

  public void OnPointerExit(PointerEventData eventData) => this.ClearState();

  private void ClearState()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || this.Equals((object) null) || !((UnityEngine.Object) TextLinkHandler.hoveredText == (UnityEngine.Object) this))
      return;
    if (this.hoverLink && (UnityEngine.Object) PlayerController.Instance != (UnityEngine.Object) null && (UnityEngine.Object) PlayerController.Instance.ActiveTool != (UnityEngine.Object) null)
      PlayerController.Instance.ActiveTool.SetLinkCursor(false);
    TextLinkHandler.hoveredText = (TextLinkHandler) null;
    this.hoverLink = false;
  }

  public void CheckMouseOver()
  {
    if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
      return;
    if (TMP_TextUtilities.FindIntersectingLink((TMP_Text) this.text, KInputManager.GetMousePos(), (Camera) null) != -1)
    {
      this.SetMouseOver();
      this.hoverLink = true;
    }
    else
    {
      if (!((UnityEngine.Object) TextLinkHandler.hoveredText == (UnityEngine.Object) this))
        return;
      this.hoverLink = false;
    }
  }

  private void SetMouseOver()
  {
    if ((UnityEngine.Object) TextLinkHandler.hoveredText != (UnityEngine.Object) null && (UnityEngine.Object) TextLinkHandler.hoveredText != (UnityEngine.Object) this)
      TextLinkHandler.hoveredText.hoverLink = false;
    TextLinkHandler.hoveredText = this;
  }
}
