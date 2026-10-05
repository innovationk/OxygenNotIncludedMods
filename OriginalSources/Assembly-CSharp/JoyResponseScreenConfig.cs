// Decompiled with JetBrains decompiler
// Type: JoyResponseScreenConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using UnityEngine;

#nullable disable
public readonly struct JoyResponseScreenConfig
{
  public readonly JoyResponseOutfitTarget target;
  public readonly Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem;
  public readonly bool isValid;

  private JoyResponseScreenConfig(
    JoyResponseOutfitTarget target,
    Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem)
  {
    this.target = target;
    this.initalSelectedItem = initalSelectedItem;
    this.isValid = true;
  }

  public JoyResponseScreenConfig WithInitialSelection(
    Option<BalloonArtistFacadeResource> initialSelectedItem)
  {
    return new JoyResponseScreenConfig(this.target, (Option<JoyResponseDesignerScreen.GalleryItem>) (JoyResponseDesignerScreen.GalleryItem) JoyResponseDesignerScreen.GalleryItem.Of(initialSelectedItem));
  }

  public static JoyResponseScreenConfig Minion(GameObject minionInstance)
  {
    return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromMinion(minionInstance), (Option<JoyResponseDesignerScreen.GalleryItem>) Option.None);
  }

  public static JoyResponseScreenConfig Personality(global::Personality personality)
  {
    return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromPersonality(personality), (Option<JoyResponseDesignerScreen.GalleryItem>) Option.None);
  }

  public static JoyResponseScreenConfig From(MinionBrowserScreen.GridItem item)
  {
    switch (item)
    {
      case MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget:
        return JoyResponseScreenConfig.Personality(personalityTarget.personality);
      case MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget:
        return JoyResponseScreenConfig.Minion(minionInstanceTarget.minionInstance);
      default:
        throw new NotImplementedException();
    }
  }

  public void ApplyAndOpenScreen()
  {
    LockerNavigator.Instance.joyResponseDesignerScreen.GetComponent<JoyResponseDesignerScreen>().Configure(this);
    LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.joyResponseDesignerScreen);
  }
}
