// Decompiled with JetBrains decompiler
// Type: UIMinionOrMannequin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIMinionOrMannequin : KMonoBehaviour
{
  public UIMinion minion;
  public UIMannequin mannequin;

  public UIMinionOrMannequin.ITarget current { get; private set; }

  protected override void OnSpawn() => this.TrySpawn();

  public bool TrySpawn()
  {
    bool flag = false;
    if (this.mannequin.IsNullOrDestroyed())
    {
      GameObject go = new GameObject("UIMannequin");
      go.AddOrGet<RectTransform>().Fill(Padding.All(10f));
      go.transform.SetParent(this.transform, false);
      AspectRatioFitter aspectRatioFitter = go.AddOrGet<AspectRatioFitter>();
      aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
      aspectRatioFitter.aspectRatio = 1f;
      this.mannequin = go.AddOrGet<UIMannequin>();
      this.mannequin.TrySpawn();
      go.SetActive(false);
      flag = true;
    }
    if (this.minion.IsNullOrDestroyed())
    {
      GameObject go = new GameObject("UIMinion");
      go.AddOrGet<RectTransform>().Fill(Padding.All(10f));
      go.transform.SetParent(this.transform, false);
      AspectRatioFitter aspectRatioFitter = go.AddOrGet<AspectRatioFitter>();
      aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
      aspectRatioFitter.aspectRatio = 1f;
      this.minion = go.AddOrGet<UIMinion>();
      this.minion.TrySpawn();
      go.SetActive(false);
      flag = true;
    }
    if (flag)
      this.SetAsMannequin();
    return flag;
  }

  public UIMinionOrMannequin.ITarget SetFrom(Option<Personality> personality)
  {
    return personality.IsSome() ? (UIMinionOrMannequin.ITarget) this.SetAsMinion(personality.Unwrap()) : (UIMinionOrMannequin.ITarget) this.SetAsMannequin();
  }

  public UIMinion SetAsMinion(Personality personality)
  {
    this.mannequin.gameObject.SetActive(false);
    this.minion.gameObject.SetActive(true);
    this.minion.SetMinion(personality);
    this.current = (UIMinionOrMannequin.ITarget) this.minion;
    return this.minion;
  }

  public UIMannequin SetAsMannequin()
  {
    this.minion.gameObject.SetActive(false);
    this.mannequin.gameObject.SetActive(true);
    this.current = (UIMinionOrMannequin.ITarget) this.mannequin;
    return this.mannequin;
  }

  public MinionVoice GetMinionVoice()
  {
    return MinionVoice.ByObject((Object) this.current.SpawnedAvatar).UnwrapOr(MinionVoice.Random());
  }

  public interface ITarget
  {
    GameObject SpawnedAvatar { get; }

    Option<Personality> Personality { get; }

    void SetOutfit(
      ClothingOutfitUtility.OutfitType outfitType,
      IEnumerable<ClothingItemResource> clothingItems);

    void React(UIMinionOrMannequinReactSource source);
  }
}
