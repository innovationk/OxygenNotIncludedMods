// Decompiled with JetBrains decompiler
// Type: KIconToggleMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KIconToggleMenu : KScreen
{
  [SerializeField]
  private Transform toggleParent;
  [SerializeField]
  private KToggle prefab;
  [SerializeField]
  private ToggleGroup group;
  [SerializeField]
  private Sprite[] icons;
  [SerializeField]
  public TextStyleSetting ToggleToolTipTextStyleSetting;
  [SerializeField]
  public TextStyleSetting ToggleToolTipHeaderTextStyleSetting;
  [SerializeField]
  protected bool repeatKeyDownToggles = true;
  protected KToggle currentlySelectedToggle;
  protected IList<KIconToggleMenu.ToggleInfo> toggleInfo;
  protected List<KToggle> toggles = new List<KToggle>();
  private List<KToggle> dontDestroyToggles = new List<KToggle>();
  protected int selected = -1;

  public event KIconToggleMenu.OnSelect onSelect;

  public void Setup(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
  {
    this.toggleInfo = toggleInfo;
    this.RefreshButtons();
  }

  protected void Setup() => this.RefreshButtons();

  protected virtual void RefreshButtons()
  {
    foreach (KToggle toggle in this.toggles)
    {
      if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
      {
        if (!this.dontDestroyToggles.Contains(toggle))
          UnityEngine.Object.Destroy((UnityEngine.Object) toggle.gameObject);
        else
          toggle.ClearOnClick();
      }
    }
    this.toggles.Clear();
    this.dontDestroyToggles.Clear();
    if (this.toggleInfo == null)
      return;
    Transform transform = (UnityEngine.Object) this.toggleParent != (UnityEngine.Object) null ? this.toggleParent : this.transform;
    for (int index = 0; index < this.toggleInfo.Count; ++index)
    {
      int idx = index;
      KIconToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[index];
      KToggle toggle;
      if ((UnityEngine.Object) toggleInfo.instanceOverride != (UnityEngine.Object) null)
      {
        toggle = toggleInfo.instanceOverride;
        this.dontDestroyToggles.Add(toggle);
      }
      else
        toggle = !(bool) (UnityEngine.Object) toggleInfo.prefabOverride ? Util.KInstantiateUI<KToggle>(this.prefab.gameObject, transform.gameObject, true) : Util.KInstantiateUI<KToggle>(toggleInfo.prefabOverride.gameObject, transform.gameObject, true);
      toggle.Deselect();
      toggle.gameObject.name = "Toggle:" + toggleInfo.text;
      toggle.group = this.group;
      toggle.onClick += (System.Action) (() => this.OnClick(idx));
      LocText componentInChildren = toggle.transform.GetComponentInChildren<LocText>();
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        componentInChildren.SetText(toggleInfo.text);
      if (toggleInfo.getSpriteCB != null)
        toggle.fgImage.sprite = toggleInfo.getSpriteCB();
      else if (toggleInfo.icon != null)
        toggle.fgImage.sprite = Assets.GetSprite((HashedString) toggleInfo.icon);
      toggleInfo.SetToggle(toggle);
      this.toggles.Add(toggle);
    }
  }

  public Sprite GetIcon(string name)
  {
    foreach (Sprite icon in this.icons)
    {
      if (icon.name == name)
        return icon;
    }
    return (Sprite) null;
  }

  public virtual void ClearSelection()
  {
    if (this.toggles == null)
      return;
    foreach (KToggle toggle in this.toggles)
    {
      toggle.Deselect();
      toggle.ClearAnimState();
    }
    this.selected = -1;
  }

  private void OnClick(int i)
  {
    if (this.onSelect == null)
      return;
    this.selected = i;
    this.onSelect(this.toggleInfo[i]);
    if (!this.toggles[i].isOn)
      this.selected = -1;
    for (int index = 0; index < this.toggles.Count; ++index)
    {
      if (index != this.selected)
        this.toggles[index].isOn = false;
    }
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.toggles == null || this.toggleInfo == null)
      return;
    for (int index = 0; index < this.toggleInfo.Count; ++index)
    {
      if (this.toggles[index].isActiveAndEnabled)
      {
        Action hotKey = this.toggleInfo[index].hotKey;
        if (hotKey != Action.NumActions && e.TryConsume(hotKey))
        {
          if (this.selected == index && !this.repeatKeyDownToggles)
            break;
          this.toggles[index].Click();
          if (this.selected == index)
            this.toggles[index].Deselect();
          this.selected = index;
          break;
        }
      }
    }
  }

  public virtual void Close()
  {
    this.ClearSelection();
    this.Show(false);
  }

  public delegate void OnSelect(KIconToggleMenu.ToggleInfo toggleInfo);

  public class ToggleInfo
  {
    public string text;
    public object userData;
    public string icon;
    public string tooltip;
    public string tooltipHeader;
    public KToggle toggle;
    public Action hotKey;
    public ToolTip.ComplexTooltipDelegate getTooltipText;
    public Func<Sprite> getSpriteCB;
    public KToggle prefabOverride;
    public KToggle instanceOverride;

    public ToggleInfo(
      string text,
      string icon,
      object user_data = null,
      Action hotkey = Action.NumActions,
      string tooltip = "",
      string tooltip_header = "")
    {
      this.text = text;
      this.userData = user_data;
      this.icon = icon;
      this.hotKey = hotkey;
      this.tooltip = tooltip;
      this.tooltipHeader = tooltip_header;
      this.getTooltipText = new ToolTip.ComplexTooltipDelegate(this.DefaultGetTooltipText);
    }

    public ToggleInfo(string text, object user_data, Action hotkey, Func<Sprite> get_sprite_cb)
    {
      this.text = text;
      this.userData = user_data;
      this.hotKey = hotkey;
      this.getSpriteCB = get_sprite_cb;
    }

    public virtual void SetToggle(KToggle toggle)
    {
      this.toggle = toggle;
      toggle.GetComponent<ToolTip>().OnComplexToolTip = this.getTooltipText;
    }

    protected virtual List<Tuple<string, TextStyleSetting>> DefaultGetTooltipText()
    {
      List<Tuple<string, TextStyleSetting>> tooltipText = new List<Tuple<string, TextStyleSetting>>();
      if (this.tooltipHeader != null)
        tooltipText.Add(new Tuple<string, TextStyleSetting>(this.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
      tooltipText.Add(new Tuple<string, TextStyleSetting>(this.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
      return tooltipText;
    }
  }
}
