// Decompiled with JetBrains decompiler
// Type: ClothingOutfitNameProposal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public readonly struct ClothingOutfitNameProposal
{
  public readonly string candidateName;
  public readonly ClothingOutfitNameProposal.Result result;

  private ClothingOutfitNameProposal(string candidateName, ClothingOutfitNameProposal.Result result)
  {
    this.candidateName = candidateName;
    this.result = result;
  }

  public static ClothingOutfitNameProposal ForNewOutfit(string candidateName)
  {
    if (string.IsNullOrEmpty(candidateName))
      return Make(ClothingOutfitNameProposal.Result.Error_NoInputName);
    return ClothingOutfitTarget.DoesTemplateExist(candidateName) ? Make(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists) : Make(ClothingOutfitNameProposal.Result.NewOutfit);

    ClothingOutfitNameProposal Make(ClothingOutfitNameProposal.Result result)
    {
      return new ClothingOutfitNameProposal(candidateName, result);
    }
  }

  public static ClothingOutfitNameProposal FromExistingOutfit(
    string candidateName,
    ClothingOutfitTarget existingOutfit,
    bool isSameNameAllowed)
  {
    if (string.IsNullOrEmpty(candidateName))
      return Make(ClothingOutfitNameProposal.Result.Error_NoInputName);
    if (!ClothingOutfitTarget.DoesTemplateExist(candidateName))
      return Make(ClothingOutfitNameProposal.Result.NewOutfit);
    if (!isSameNameAllowed || !(candidateName == existingOutfit.ReadName()))
      return Make(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists);
    return existingOutfit.CanWriteName ? Make(ClothingOutfitNameProposal.Result.SameOutfit) : Make(ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly);

    ClothingOutfitNameProposal Make(ClothingOutfitNameProposal.Result result)
    {
      return new ClothingOutfitNameProposal(candidateName, result);
    }
  }

  public enum Result
  {
    None,
    NewOutfit,
    SameOutfit,
    Error_NoInputName,
    Error_NameAlreadyExists,
    Error_SameOutfitReadonly,
  }
}
