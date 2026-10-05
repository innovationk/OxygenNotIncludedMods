// Decompiled with JetBrains decompiler
// Type: OutfitDesignerScreenConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public readonly struct OutfitDesignerScreenConfig
{
  public readonly ClothingOutfitTarget sourceTarget;
  public readonly Option<ClothingOutfitTarget> outfitTemplate;
  public readonly Option<Personality> minionPersonality;
  public readonly Option<GameObject> targetMinionInstance;
  public readonly Action<ClothingOutfitTarget> onWriteToOutfitTargetFn;
  public readonly bool isValid;

  public OutfitDesignerScreenConfig(
    ClothingOutfitTarget sourceTarget,
    Option<Personality> minionPersonality,
    Option<GameObject> targetMinionInstance,
    Action<ClothingOutfitTarget> onWriteToOutfitTargetFn = null)
  {
    this.sourceTarget = sourceTarget;
    this.outfitTemplate = sourceTarget.IsTemplateOutfit() ? Option.Some<ClothingOutfitTarget>(sourceTarget) : (Option<ClothingOutfitTarget>) Option.None;
    this.minionPersonality = minionPersonality;
    this.targetMinionInstance = targetMinionInstance;
    this.onWriteToOutfitTargetFn = onWriteToOutfitTargetFn;
    this.isValid = true;
    ClothingOutfitTarget.MinionInstance minionInstance;
    if (!sourceTarget.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance))
      return;
    Debug.Assert(targetMinionInstance.HasValue && targetMinionInstance == minionInstance.minionInstance);
  }

  public OutfitDesignerScreenConfig WithOutfit(ClothingOutfitTarget sourceTarget)
  {
    return new OutfitDesignerScreenConfig(sourceTarget, this.minionPersonality, this.targetMinionInstance, this.onWriteToOutfitTargetFn);
  }

  public OutfitDesignerScreenConfig OnWriteToOutfitTarget(
    Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
  {
    return new OutfitDesignerScreenConfig(this.sourceTarget, this.minionPersonality, this.targetMinionInstance, onWriteToOutfitTargetFn);
  }

  public static OutfitDesignerScreenConfig Mannequin(ClothingOutfitTarget outfit)
  {
    return new OutfitDesignerScreenConfig(outfit, (Option<Personality>) Option.None, (Option<GameObject>) Option.None);
  }

  public static OutfitDesignerScreenConfig Minion(
    ClothingOutfitTarget outfit,
    Personality personality)
  {
    return new OutfitDesignerScreenConfig(outfit, (Option<Personality>) personality, (Option<GameObject>) Option.None);
  }

  public static OutfitDesignerScreenConfig Minion(
    ClothingOutfitTarget outfit,
    GameObject targetMinionInstance)
  {
    Personality minionPersonality = Db.Get().Personalities.Get(targetMinionInstance.GetComponent<MinionIdentity>().personalityResourceId);
    ClothingOutfitTarget.MinionInstance minionInstance;
    Debug.Assert(outfit.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance));
    Debug.Assert((UnityEngine.Object) minionInstance.minionInstance == (UnityEngine.Object) targetMinionInstance);
    return new OutfitDesignerScreenConfig(outfit, (Option<Personality>) minionPersonality, (Option<GameObject>) targetMinionInstance);
  }

  public static OutfitDesignerScreenConfig Minion(
    ClothingOutfitTarget outfit,
    MinionBrowserScreen.GridItem item)
  {
    switch (item)
    {
      case MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget:
        return OutfitDesignerScreenConfig.Minion(outfit, personalityTarget.personality);
      case MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget:
        return OutfitDesignerScreenConfig.Minion(outfit, minionInstanceTarget.minionInstance);
      default:
        throw new NotImplementedException();
    }
  }

  public void ApplyAndOpenScreen()
  {
    LockerNavigator.Instance.outfitDesignerScreen.GetComponent<OutfitDesignerScreen>().Configure(this);
    LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitDesignerScreen);
  }
}
