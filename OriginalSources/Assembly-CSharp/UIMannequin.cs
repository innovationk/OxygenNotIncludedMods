// Decompiled with JetBrains decompiler
// Type: UIMannequin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class UIMannequin : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
  public const float ANIM_SCALE = 0.38f;
  private KBatchedAnimController animController;
  private GameObject spawn;
  public bool shouldShowOutfitWithDefaultItems = true;
  public Option<global::Personality> personalityToUseForDefaultClothing;

  public GameObject SpawnedAvatar
  {
    get
    {
      if ((UnityEngine.Object) this.spawn == (UnityEngine.Object) null)
        this.TrySpawn();
      return this.spawn;
    }
  }

  public Option<global::Personality> Personality => new Option<global::Personality>();

  protected override void OnSpawn() => this.TrySpawn();

  public void TrySpawn()
  {
    if (!((UnityEngine.Object) this.animController == (UnityEngine.Object) null))
      return;
    this.animController = Util.KInstantiateUI(Assets.GetPrefab((Tag) MannequinUIPortrait.ID), this.gameObject).GetComponent<KBatchedAnimController>();
    this.animController.LoadAnims();
    this.animController.gameObject.SetActive(true);
    this.animController.animScale = 0.38f;
    this.animController.Play((HashedString) "idle", KAnim.PlayMode.Paused);
    this.spawn = this.animController.gameObject;
    BaseMinionConfig.ConfigureSymbols(this.spawn, false);
    this.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = (Option<MinionVoice>) Option.None;
  }

  public void SetOutfit(
    ClothingOutfitUtility.OutfitType outfitType,
    IEnumerable<ClothingItemResource> outfit)
  {
    bool flag = outfit.Count<ClothingItemResource>() == 0;
    if (this.shouldShowOutfitWithDefaultItems)
      outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
    this.SpawnedAvatar.GetComponent<SymbolOverrideController>().RemoveAllSymbolOverrides();
    BaseMinionConfig.ConfigureSymbols(this.SpawnedAvatar, false);
    Accessorizer component1 = this.SpawnedAvatar.GetComponent<Accessorizer>();
    WearableAccessorizer component2 = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
    global::Personality personality = this.personalityToUseForDefaultClothing.UnwrapOr(Db.Get().Personalities.Get("ABE"));
    component1.ApplyMinionPersonality(personality);
    component2.ClearClothingItems();
    component2.ApplyClothingItems(outfitType, outfit);
    List<KAnimHashedString> second = new List<KAnimHashedString>(32 /*0x20*/);
    if (this.shouldShowOutfitWithDefaultItems && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
    {
      second.Add((KAnimHashedString) "foot");
      second.Add((KAnimHashedString) "hand_paint");
      if (flag)
        second.Add((KAnimHashedString) "belt");
      if (!outfit.Select<ClothingItemResource, PermitCategory>((Func<ClothingItemResource, PermitCategory>) (item => item.Category)).Contains<PermitCategory>(PermitCategory.DupeTops))
      {
        second.Add((KAnimHashedString) "torso");
        second.Add((KAnimHashedString) "neck");
        second.Add((KAnimHashedString) "arm_lower");
        second.Add((KAnimHashedString) "arm_lower_sleeve");
        second.Add((KAnimHashedString) "arm_sleeve");
        second.Add((KAnimHashedString) "cuff");
      }
      if (!outfit.Select<ClothingItemResource, PermitCategory>((Func<ClothingItemResource, PermitCategory>) (item => item.Category)).Contains<PermitCategory>(PermitCategory.DupeGloves))
      {
        second.Add((KAnimHashedString) "arm_lower_sleeve");
        second.Add((KAnimHashedString) "cuff");
      }
      if (!outfit.Select<ClothingItemResource, PermitCategory>((Func<ClothingItemResource, PermitCategory>) (item => item.Category)).Contains<PermitCategory>(PermitCategory.DupeBottoms))
      {
        second.Add((KAnimHashedString) "leg");
        second.Add((KAnimHashedString) "pelvis");
      }
    }
    KAnimHashedString[] array = outfit.SelectMany<ClothingItemResource, KAnimHashedString>((Func<ClothingItemResource, IEnumerable<KAnimHashedString>>) (item => ((IEnumerable<KAnim.Build.Symbol>) item.AnimFile.GetData().build.symbols).Select<KAnim.Build.Symbol, KAnimHashedString>((Func<KAnim.Build.Symbol, KAnimHashedString>) (s => s.hash)))).Concat<KAnimHashedString>((IEnumerable<KAnimHashedString>) second).ToArray<KAnimHashedString>();
    foreach (KAnim.Build.Symbol symbol in this.animController.AnimFiles[0].GetData().build.symbols)
    {
      if (symbol.hash == (KAnimHashedString) "mannequin_arm" || symbol.hash == (KAnimHashedString) "mannequin_body" || symbol.hash == (KAnimHashedString) "mannequin_headshape" || symbol.hash == (KAnimHashedString) "mannequin_leg")
        this.animController.SetSymbolVisiblity(symbol.hash, true);
      else
        this.animController.SetSymbolVisiblity(symbol.hash, ((IEnumerable<KAnimHashedString>) array).Contains<KAnimHashedString>(symbol.hash));
    }
  }

  private static ClothingItemResource GetItemForCategory(
    PermitCategory category,
    IEnumerable<ClothingItemResource> outfit)
  {
    foreach (ClothingItemResource itemForCategory in outfit)
    {
      if (itemForCategory.Category == category)
        return itemForCategory;
    }
    return (ClothingItemResource) null;
  }

  public void React(UIMinionOrMannequinReactSource source)
  {
    this.animController.Play((HashedString) "idle");
  }
}
