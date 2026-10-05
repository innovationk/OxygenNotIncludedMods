// Decompiled with JetBrains decompiler
// Type: SpriteListDialogScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SpriteListDialogScreen : KModalScreen
{
  public System.Action onDeactivateCB;
  [SerializeField]
  private GameObject buttonPrefab;
  [SerializeField]
  private GameObject buttonPanel;
  [SerializeField]
  private LocText titleText;
  [SerializeField]
  private LocText popupMessage;
  [SerializeField]
  private GameObject listPanel;
  [SerializeField]
  private GameObject listPrefab;
  private List<SpriteListDialogScreen.Button> buttons;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.SetActive(false);
    this.buttons = new List<SpriteListDialogScreen.Button>();
  }

  public override bool IsModal() => true;

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  public void AddOption(string text, System.Action action)
  {
    GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
    this.buttons.Add(new SpriteListDialogScreen.Button()
    {
      label = text,
      action = action,
      gameObject = gameObject
    });
  }

  public void AddListRow(Sprite sprite, string text, float width = -1f, float height = -1f)
  {
    GameObject gameObject = Util.KInstantiateUI(this.listPrefab, this.listPanel, true);
    gameObject.GetComponentInChildren<LocText>().text = text;
    Image componentInChildren = gameObject.GetComponentInChildren<Image>();
    componentInChildren.sprite = sprite;
    if ((UnityEngine.Object) sprite == (UnityEngine.Object) null)
    {
      Color color = componentInChildren.color with
      {
        a = 0.0f
      };
      componentInChildren.color = color;
    }
    if ((double) width >= 0.0 || (double) height >= 0.0)
    {
      componentInChildren.GetComponent<AspectRatioFitter>().enabled = false;
      LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
      component.minWidth = width;
      component.preferredWidth = width;
      component.minHeight = height;
      component.preferredHeight = height;
    }
    else
      componentInChildren.GetComponent<AspectRatioFitter>().aspectRatio = (UnityEngine.Object) sprite == (UnityEngine.Object) null ? 1f : sprite.rect.width / sprite.rect.height;
  }

  public void PopupConfirmDialog(string text, string title_text = null)
  {
    foreach (SpriteListDialogScreen.Button button in this.buttons)
    {
      button.gameObject.GetComponentInChildren<LocText>().text = button.label;
      button.gameObject.GetComponent<KButton>().onClick += button.action;
    }
    if (title_text != null)
      this.titleText.text = title_text;
    this.popupMessage.text = text;
  }

  protected override void OnDeactivate()
  {
    if (this.onDeactivateCB != null)
      this.onDeactivateCB();
    base.OnDeactivate();
  }

  private struct Button
  {
    public System.Action action;
    public GameObject gameObject;
    public string label;
  }
}
