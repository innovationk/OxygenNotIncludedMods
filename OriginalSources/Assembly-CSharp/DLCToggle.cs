// Decompiled with JetBrains decompiler
// Type: DLCToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class DLCToggle : KMonoBehaviour
{
  private bool expansion1Active;

  protected override void OnPrefabInit() => this.expansion1Active = DlcManager.IsExpansion1Active();

  public void ToggleExpansion1Cicked()
  {
    Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.GetComponentInParent<Canvas>().gameObject, true).AddDefaultCancel().SetHeader((string) (this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1)).AddSprite(this.expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall).AddPlainText((string) (this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_DESC : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_DESC)).AddOption((string) UI.CONFIRMDIALOG.OK, (Action<InfoDialogScreen>) (screen => DlcManager.ToggleDLC("EXPANSION1_ID")), true);
  }
}
