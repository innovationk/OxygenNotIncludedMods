// Decompiled with JetBrains decompiler
// Type: LocText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class LocText : TextMeshProUGUI
{
  public string key;
  public TextStyleSetting textStyleSetting;
  public bool allowOverride;
  public bool staticLayout;
  private TextLinkHandler textLinkHandler;
  private string originalString = string.Empty;
  [SerializeField]
  private bool allowLinksInternal;
  private static readonly Dictionary<string, Action> ActionLookup = ((IEnumerable<string>) Enum.GetNames(typeof (Action))).ToDictionary<string, string, Action>((Func<string, string>) (x => x), (Func<string, Action>) (x => (Action) Enum.Parse(typeof (Action), x)), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  private static readonly Dictionary<string, Pair<LocString, LocString>> ClickLookup = new Dictionary<string, Pair<LocString, LocString>>()
  {
    {
      UI.ClickType.Click.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESS, UI.CONTROLS.CLICK)
    },
    {
      UI.ClickType.Clickable.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLE, UI.CONTROLS.CLICKABLE)
    },
    {
      UI.ClickType.Clicked.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSED, UI.CONTROLS.CLICKED)
    },
    {
      UI.ClickType.Clicking.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSING, UI.CONTROLS.CLICKING)
    },
    {
      UI.ClickType.Clicks.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSES, UI.CONTROLS.CLICKS)
    },
    {
      UI.ClickType.click.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSLOWER, UI.CONTROLS.CLICKLOWER)
    },
    {
      UI.ClickType.clickable.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLELOWER, UI.CONTROLS.CLICKABLELOWER)
    },
    {
      UI.ClickType.clicked.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDLOWER, UI.CONTROLS.CLICKEDLOWER)
    },
    {
      UI.ClickType.clicking.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGLOWER, UI.CONTROLS.CLICKINGLOWER)
    },
    {
      UI.ClickType.clicks.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSESLOWER, UI.CONTROLS.CLICKSLOWER)
    },
    {
      UI.ClickType.CLICK.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSUPPER, UI.CONTROLS.CLICKUPPER)
    },
    {
      UI.ClickType.CLICKABLE.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLEUPPER, UI.CONTROLS.CLICKABLEUPPER)
    },
    {
      UI.ClickType.CLICKED.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDUPPER, UI.CONTROLS.CLICKEDUPPER)
    },
    {
      UI.ClickType.CLICKING.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGUPPER, UI.CONTROLS.CLICKINGUPPER)
    },
    {
      UI.ClickType.CLICKS.ToString(),
      new Pair<LocString, LocString>(UI.CONTROLS.PRESSESUPPER, UI.CONTROLS.CLICKSUPPER)
    }
  };
  private const string linkPrefix_open = "<link=\"";
  private const string linkSuffix = "</link>";
  private const string linkColorPrefix = "<b><style=\"KLink\">";
  private const string linkColorSuffix = "</style></b>";
  private static readonly string combinedPrefix = "<b><style=\"KLink\"><link=\"";
  private static readonly string combinedSuffix = "</style></b></link>";

  protected override void OnEnable() => base.OnEnable();

  public bool AllowLinks
  {
    get => this.allowLinksInternal;
    set
    {
      this.allowLinksInternal = value;
      this.RefreshLinkHandler();
      this.raycastTarget = this.raycastTarget || this.allowLinksInternal;
    }
  }

  [ContextMenu("Apply Settings")]
  public void ApplySettings()
  {
    if (this.key != "" && Application.isPlaying)
      this.text = (string) Strings.Get(new StringKey(this.key));
    if (!((UnityEngine.Object) this.textStyleSetting != (UnityEngine.Object) null))
      return;
    SetTextStyleSetting.ApplyStyle((TextMeshProUGUI) this, this.textStyleSetting);
  }

  private new void Awake()
  {
    base.Awake();
    if (!Application.isPlaying)
      return;
    if (this.key != "")
      this.text = Strings.Get(new StringKey(this.key)).String;
    this.text = Localization.Fixup(this.text);
    this.isRightToLeftText = Localization.IsRightToLeft;
    KInputManager.InputChange.AddListener(new UnityAction(this.RefreshText));
    SetTextStyleSetting textStyleSetting = this.gameObject.GetComponent<SetTextStyleSetting>();
    if ((UnityEngine.Object) textStyleSetting == (UnityEngine.Object) null)
      textStyleSetting = this.gameObject.AddComponent<SetTextStyleSetting>();
    if (!this.allowOverride)
      textStyleSetting.SetStyle(this.textStyleSetting);
    this.textLinkHandler = this.GetComponent<TextLinkHandler>();
  }

  private new void Start()
  {
    base.Start();
    this.RefreshLinkHandler();
  }

  private new void OnDestroy()
  {
    KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshText));
    base.OnDestroy();
  }

  public void SetLinkOverrideAction(Func<string, bool> action)
  {
    this.RefreshLinkHandler();
    if (!((UnityEngine.Object) this.textLinkHandler != (UnityEngine.Object) null))
      return;
    this.textLinkHandler.overrideLinkAction = action;
  }

  public override string text
  {
    get => base.text;
    set => base.text = this.FilterInput(value);
  }

  public override void SetText(string text)
  {
    text = this.FilterInput(text);
    base.SetText(text);
  }

  private string FilterInput(string input)
  {
    if (input != null)
    {
      string text = LocText.ParseText(input);
      this.originalString = !(text != input) ? string.Empty : input;
      input = text;
    }
    return this.AllowLinks ? LocText.ModifyLinkStrings(input) : input;
  }

  public static string ParseText(string input)
  {
    string pattern = "\\{Hotkey/(\\w+)\\}";
    return Regex.Replace(Regex.Replace(input, pattern, (MatchEvaluator) (m =>
    {
      string key = m.Groups[1].Value;
      Action action;
      return LocText.ActionLookup.TryGetValue(key, out action) ? GameUtil.GetHotkeyString(action) : m.Value;
    })), "\\(ClickType/(\\w+)\\)", (MatchEvaluator) (m =>
    {
      string key = m.Groups[1].Value;
      Pair<LocString, LocString> pair;
      if (!LocText.ClickLookup.TryGetValue(key, out pair))
        return m.Value;
      return KInputManager.currentControllerIsGamepad ? pair.first.ToString() : pair.second.ToString();
    }));
  }

  private void RefreshText()
  {
    if (!(this.originalString != string.Empty))
      return;
    this.SetText(this.originalString);
  }

  protected override void GenerateTextMesh() => base.GenerateTextMesh();

  internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
  {
    this.font = font;
    if (this.key != "")
      this.text = Strings.Get(new StringKey(this.key)).String;
    this.text = Localization.Fixup(this.text);
    this.isRightToLeftText = isRightToLeft;
  }

  private static string ModifyLinkStrings(string input)
  {
    if (input == null || input.IndexOf("<b><style=\"KLink\">") != -1)
      return input;
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    sb.Append(input);
    sb.Replace("<link=\"", LocText.combinedPrefix);
    sb.Replace("</link>", LocText.combinedSuffix);
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }

  private void RefreshLinkHandler()
  {
    if ((UnityEngine.Object) this.textLinkHandler == (UnityEngine.Object) null && this.allowLinksInternal)
    {
      this.textLinkHandler = this.GetComponent<TextLinkHandler>();
      if ((UnityEngine.Object) this.textLinkHandler == (UnityEngine.Object) null)
        this.textLinkHandler = this.gameObject.AddComponent<TextLinkHandler>();
    }
    else if (!this.allowLinksInternal && (UnityEngine.Object) this.textLinkHandler != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.textLinkHandler);
      this.textLinkHandler = (TextLinkHandler) null;
    }
    if (!((UnityEngine.Object) this.textLinkHandler != (UnityEngine.Object) null))
      return;
    this.textLinkHandler.CheckMouseOver();
  }
}
