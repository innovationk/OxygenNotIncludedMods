// Decompiled with JetBrains decompiler
// Type: JoyResponseOutfitTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public readonly struct JoyResponseOutfitTarget(JoyResponseOutfitTarget.Implementation impl)
{
  private readonly JoyResponseOutfitTarget.Implementation impl = impl;

  public Option<string> ReadFacadeId() => this.impl.ReadFacadeId();

  public void WriteFacadeId(Option<string> facadeId) => this.impl.WriteFacadeId(facadeId);

  public string GetMinionName() => this.impl.GetMinionName();

  public Personality GetPersonality() => this.impl.GetPersonality();

  public static JoyResponseOutfitTarget FromMinion(GameObject minionInstance)
  {
    return new JoyResponseOutfitTarget((JoyResponseOutfitTarget.Implementation) new JoyResponseOutfitTarget.MinionInstanceTarget(minionInstance));
  }

  public static JoyResponseOutfitTarget FromPersonality(Personality personality)
  {
    return new JoyResponseOutfitTarget((JoyResponseOutfitTarget.Implementation) new JoyResponseOutfitTarget.PersonalityTarget(personality));
  }

  public interface Implementation
  {
    Option<string> ReadFacadeId();

    void WriteFacadeId(Option<string> permitId);

    string GetMinionName();

    Personality GetPersonality();
  }

  public readonly struct MinionInstanceTarget : JoyResponseOutfitTarget.Implementation
  {
    public readonly GameObject minionInstance;
    public readonly WearableAccessorizer wearableAccessorizer;

    public MinionInstanceTarget(GameObject minionInstance)
    {
      this.minionInstance = minionInstance;
      this.wearableAccessorizer = minionInstance.GetComponent<WearableAccessorizer>();
    }

    public string GetMinionName() => this.minionInstance.GetProperName();

    public Personality GetPersonality()
    {
      return Db.Get().Personalities.Get(this.minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
    }

    public Option<string> ReadFacadeId() => this.wearableAccessorizer.GetJoyResponseId();

    public void WriteFacadeId(Option<string> permitId)
    {
      this.wearableAccessorizer.SetJoyResponseId(permitId);
    }
  }

  public readonly struct PersonalityTarget(Personality personality) : 
    JoyResponseOutfitTarget.Implementation
  {
    public readonly Personality personality = personality;

    public string GetMinionName() => this.personality.Name;

    public Personality GetPersonality() => this.personality;

    public Option<string> ReadFacadeId()
    {
      return (Option<string>) this.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse);
    }

    public void WriteFacadeId(Option<string> facadeId)
    {
      this.personality.SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse, facadeId);
    }
  }
}
