// Decompiled with JetBrains decompiler
// Type: OutfitBrowserScreenConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public readonly struct OutfitBrowserScreenConfig
{
  public readonly Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType;
  public readonly Option<ClothingOutfitTarget> selectedTarget;
  public readonly Option<Personality> minionPersonality;
  public readonly Option<GameObject> targetMinionInstance;
  public readonly bool isValid;
  public readonly bool isPickingOutfitForDupe;

  public OutfitBrowserScreenConfig(
    Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType,
    Option<ClothingOutfitTarget> selectedTarget,
    Option<Personality> minionPersonality,
    Option<GameObject> minionInstance)
  {
    this.onlyShowOutfitType = onlyShowOutfitType;
    this.selectedTarget = selectedTarget;
    this.minionPersonality = minionPersonality;
    this.isPickingOutfitForDupe = minionPersonality.HasValue || minionInstance.HasValue;
    this.targetMinionInstance = minionInstance;
    this.isValid = true;
    if (!minionPersonality.IsSome() && !this.targetMinionInstance.IsSome())
      return;
    Debug.Assert(onlyShowOutfitType.IsSome(), (object) "If viewing outfits for a specific duplicant personality or instance, an onlyShowOutfitType must also be given.");
  }

  public OutfitBrowserScreenConfig WithOutfitType(
    Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType)
  {
    return new OutfitBrowserScreenConfig(onlyShowOutfitType, this.selectedTarget, this.minionPersonality, this.targetMinionInstance);
  }

  public OutfitBrowserScreenConfig WithOutfit(Option<ClothingOutfitTarget> sourceTarget)
  {
    return new OutfitBrowserScreenConfig(this.onlyShowOutfitType, sourceTarget, this.minionPersonality, this.targetMinionInstance);
  }

  public string GetMinionName()
  {
    if (this.targetMinionInstance.HasValue)
      return this.targetMinionInstance.Value.GetProperName();
    return this.minionPersonality.HasValue ? this.minionPersonality.Value.Name : "-";
  }

  public static OutfitBrowserScreenConfig Mannequin()
  {
    return new OutfitBrowserScreenConfig((Option<ClothingOutfitUtility.OutfitType>) Option.None, (Option<ClothingOutfitTarget>) Option.None, (Option<Personality>) Option.None, (Option<GameObject>) Option.None);
  }

  public static OutfitBrowserScreenConfig Minion(
    ClothingOutfitUtility.OutfitType onlyShowOutfitType,
    Personality personality)
  {
    return new OutfitBrowserScreenConfig((Option<ClothingOutfitUtility.OutfitType>) onlyShowOutfitType, (Option<ClothingOutfitTarget>) Option.None, (Option<Personality>) personality, (Option<GameObject>) Option.None);
  }

  public static OutfitBrowserScreenConfig Minion(
    ClothingOutfitUtility.OutfitType onlyShowOutfitType,
    GameObject minionInstance)
  {
    Personality minionPersonality = Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
    return new OutfitBrowserScreenConfig((Option<ClothingOutfitUtility.OutfitType>) onlyShowOutfitType, (Option<ClothingOutfitTarget>) ClothingOutfitTarget.FromMinion(onlyShowOutfitType, minionInstance), (Option<Personality>) minionPersonality, (Option<GameObject>) minionInstance);
  }

  public static OutfitBrowserScreenConfig Minion(
    ClothingOutfitUtility.OutfitType onlyShowOutfitType,
    MinionBrowserScreen.GridItem item)
  {
    switch (item)
    {
      case MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget:
        return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, personalityTarget.personality);
      case MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget:
        return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, minionInstanceTarget.minionInstance);
      default:
        throw new NotImplementedException();
    }
  }

  public void ApplyAndOpenScreen()
  {
    LockerNavigator.Instance.outfitBrowserScreen.GetComponent<OutfitBrowserScreen>().Configure(this);
    LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitBrowserScreen);
  }
}
