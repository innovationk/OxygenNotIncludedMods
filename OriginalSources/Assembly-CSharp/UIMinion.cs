// Decompiled with JetBrains decompiler
// Type: UIMinion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIMinion : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
  public const float ANIM_SCALE = 0.38f;
  private KBatchedAnimController animController;
  private GameObject spawn;
  private UIMinionOrMannequinReactSource lastReactSource;

  public GameObject SpawnedAvatar
  {
    get
    {
      if ((Object) this.spawn == (Object) null)
        this.TrySpawn();
      return this.spawn;
    }
  }

  public Option<global::Personality> Personality { get; private set; }

  protected override void OnSpawn() => this.TrySpawn();

  public void TrySpawn()
  {
    if (!((Object) this.animController == (Object) null))
      return;
    this.animController = Util.KInstantiateUI(Assets.GetPrefab((Tag) MinionUIPortrait.ID), this.gameObject).GetComponent<KBatchedAnimController>();
    this.animController.gameObject.SetActive(true);
    this.animController.animScale = 0.38f;
    this.animController.Play((HashedString) "idle_default", KAnim.PlayMode.Loop);
    BaseMinionConfig.ConfigureSymbols(this.animController.gameObject);
    this.spawn = this.animController.gameObject;
  }

  public void SetMinion(global::Personality personality)
  {
    this.SpawnedAvatar.GetComponent<Accessorizer>().ApplyMinionPersonality(personality);
    this.Personality = (Option<global::Personality>) personality;
    this.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = (Option<MinionVoice>) MinionVoice.ByPersonality(personality);
  }

  public void SetOutfit(
    ClothingOutfitUtility.OutfitType outfitType,
    IEnumerable<ClothingItemResource> outfit)
  {
    outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
    WearableAccessorizer component = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
    component.ClearClothingItems();
    component.ApplyClothingItems(outfitType, outfit);
  }

  public MinionVoice GetMinionVoice()
  {
    return MinionVoice.ByObject((Object) this.SpawnedAvatar).UnwrapOr(MinionVoice.Random());
  }

  public void React(UIMinionOrMannequinReactSource source)
  {
    if (source != UIMinionOrMannequinReactSource.OnPersonalityChanged && this.lastReactSource == source)
    {
      KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
      if (currentAnim != null && currentAnim.name != "idle_default")
        return;
    }
    switch (source)
    {
      case UIMinionOrMannequinReactSource.OnPersonalityChanged:
        this.animController.Play((HashedString) "react");
        break;
      case UIMinionOrMannequinReactSource.OnWholeOutfitChanged:
      case UIMinionOrMannequinReactSource.OnBottomChanged:
        this.animController.Play((HashedString) "react_bottoms");
        break;
      case UIMinionOrMannequinReactSource.OnHatChanged:
        this.animController.Play((HashedString) "react_glasses");
        break;
      case UIMinionOrMannequinReactSource.OnTopChanged:
        this.animController.Play((HashedString) "react_tops");
        break;
      case UIMinionOrMannequinReactSource.OnGlovesChanged:
        this.animController.Play((HashedString) "react_gloves");
        break;
      case UIMinionOrMannequinReactSource.OnShoesChanged:
        this.animController.Play((HashedString) "react_shoes");
        break;
      default:
        this.animController.Play((HashedString) "cheer_pre");
        this.animController.Queue((HashedString) "cheer_loop");
        this.animController.Queue((HashedString) "cheer_pst");
        break;
    }
    this.animController.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop);
    this.lastReactSource = source;
  }
}
