// Decompiled with JetBrains decompiler
// Type: BuildWatermark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BuildWatermark : KScreen
{
  public bool interactable = true;
  public LocText textDisplay;
  public ToolTip toolTip;
  public KButton button;
  public List<GameObject> archiveIcons;
  public static BuildWatermark Instance;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    BuildWatermark.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RefreshText();
  }

  public static string GetBuildText()
  {
    string str1 = DistributionPlatform.Initialized ? LaunchInitializer.BuildPrefix() + "-" : "??-";
    string str2 = !Application.isEditor ? str1 + 744825U.ToString() : str1 + "<EDITOR>";
    string buildText = !DistributionPlatform.Initialized ? str2 + "-?" : $"{str2}-{DlcManager.GetSubscribedContentLetters()}";
    if (DebugHandler.enabled)
      buildText += "D";
    if (!"".IsNullOrWhiteSpace())
      buildText += "-<Patch:>";
    return buildText;
  }

  public void RefreshText()
  {
    bool flag1 = true;
    bool flag2 = DistributionPlatform.Initialized && DistributionPlatform.Inst.IsArchiveBranch;
    string buildText = BuildWatermark.GetBuildText();
    this.button.ClearOnClick();
    if (flag1)
    {
      this.textDisplay.SetText(string.Format((string) UI.DEVELOPMENTBUILDS.WATERMARK, (object) buildText));
      this.toolTip.ClearMultiStringTooltip();
    }
    else
    {
      this.textDisplay.SetText(string.Format((string) UI.DEVELOPMENTBUILDS.TESTING_WATERMARK, (object) buildText));
      this.toolTip.SetSimpleTooltip((string) UI.DEVELOPMENTBUILDS.TESTING_TOOLTIP);
      if (this.interactable)
        this.button.onClick += new System.Action(this.ShowTestingMessage);
    }
    foreach (GameObject archiveIcon in this.archiveIcons)
      archiveIcon.SetActive(flag1 & flag2);
  }

  private void ShowTestingMessage()
  {
    Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, Global.Instance.globalCanvas, true).PopupConfirmDialog((string) UI.DEVELOPMENTBUILDS.TESTING_MESSAGE, (System.Action) (() => App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/")), (System.Action) (() => { }), title_text: (string) UI.DEVELOPMENTBUILDS.TESTING_MESSAGE_TITLE, confirm_text: (string) UI.DEVELOPMENTBUILDS.TESTING_MORE_INFO);
  }
}
