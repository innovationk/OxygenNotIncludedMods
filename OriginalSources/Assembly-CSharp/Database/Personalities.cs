// Decompiled with JetBrains decompiler
// Type: Database.Personalities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class Personalities : ResourceSet<Personality>
{
  public Personalities()
  {
    foreach (Personalities.PersonalityInfo entry in AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<Personalities.PersonalityLoader>.Get().entries)
    {
      if (string.IsNullOrEmpty(entry.RequiredDlcId) || DlcManager.IsContentSubscribed(entry.RequiredDlcId) || this.IsCosmeticPersonality(entry.RequiredDlcId))
      {
        Personality resource = new Personality(entry.Name.ToUpper(), (string) Strings.Get($"STRINGS.DUPLICANTS.PERSONALITIES.{entry.Name.ToUpper()}.NAME"), entry.Gender.ToUpper(), entry.PersonalityType, entry.StressTrait, entry.JoyTrait, entry.StickerType, entry.CongenitalTrait, entry.HeadShape, entry.Mouth, entry.Neck, entry.Eyes, entry.Hair, entry.Body, entry.Belt, entry.Cuff, entry.Foot, entry.Hand, entry.Pelvis, entry.Leg, entry.Leg_Skin, entry.Arm_Skin, (string) Strings.Get($"STRINGS.DUPLICANTS.PERSONALITIES.{entry.Name.ToUpper()}.DESC"), entry.ValidStarter, entry.Grave, (Tag) entry.Model, entry.SpeechMouth);
        resource.requiredDlcId = entry.RequiredDlcId;
        if (!DlcManager.IsContentSubscribed(entry.RequiredDlcId) && this.IsCosmeticPersonality(entry.RequiredDlcId))
          resource.Disabled = true;
        this.Add(resource);
      }
    }
  }

  private void AddTrait(Personality personality, string trait_name)
  {
    Trait trait = Db.Get().traits.TryGet(trait_name);
    if (trait == null)
      return;
    personality.AddTrait(trait);
  }

  private void SetAttribute(Personality personality, string attribute_name, int value)
  {
    Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(attribute_name);
    if (attribute == null)
      Debug.LogWarning((object) ("Attribute does not exist: " + attribute_name));
    else
      personality.SetAttribute(attribute, value);
  }

  public List<Personality> GetStartingPersonalities()
  {
    return this.resources.FindAll((Predicate<Personality>) (x => x.startingMinion));
  }

  public List<Personality> GetAll(bool onlyEnabledMinions, bool onlyStartingMinions)
  {
    return this.resources.FindAll((Predicate<Personality>) (personality => (!onlyStartingMinions || personality.startingMinion) && (!onlyEnabledMinions || !personality.Disabled) && (!((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null) || Game.IsDlcActiveForCurrentSave(personality.requiredDlcId))));
  }

  public Personality GetRandom(bool onlyEnabledMinions, bool onlyStartingMinions)
  {
    return this.GetAll(onlyEnabledMinions, onlyStartingMinions).GetRandom<Personality>();
  }

  public Personality GetRandom(Tag model, bool onlyEnabledMinions, bool onlyStartingMinions)
  {
    return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Predicate<Personality>) (personality => personality.model == model || model == (Tag) (string) null)).GetRandom<Personality>();
  }

  public Personality GetRandom(List<Tag> models, bool onlyEnabledMinions, bool onlyStartingMinions)
  {
    return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Predicate<Personality>) (personality => models.Contains(personality.model))).GetRandom<Personality>();
  }

  public Personality GetPersonalityFromNameStringKey(string name_string_key)
  {
    foreach (Personality resource in Db.Get().Personalities.resources)
    {
      if (resource.nameStringKey.Equals(name_string_key, StringComparison.CurrentCultureIgnoreCase))
        return resource;
    }
    return (Personality) null;
  }

  private bool IsCosmeticPersonality(string required_dlc_id)
  {
    return !string.IsNullOrEmpty(required_dlc_id) && DlcManager.CONTENT_ONLY_DLC_IDS.Contains(required_dlc_id);
  }

  public class PersonalityLoader : 
    AsyncCsvLoader<Personalities.PersonalityLoader, Personalities.PersonalityInfo>
  {
    public PersonalityLoader()
      : base(Assets.instance.personalitiesFile)
    {
    }

    public override void Run() => base.Run();
  }

  public class PersonalityInfo : Resource
  {
    public int HeadShape;
    public int Mouth;
    public int Neck;
    public int Eyes;
    public int Hair;
    public int Body;
    public int Belt;
    public int Cuff;
    public int Foot;
    public int Hand;
    public int Pelvis;
    public int Leg;
    public int Arm_Skin;
    public int Leg_Skin;
    public int SpeechMouth;
    public string Gender;
    public string PersonalityType;
    public string StressTrait;
    public string JoyTrait;
    public string StickerType;
    public string CongenitalTrait;
    public string Design;
    public bool ValidStarter;
    public string Grave;
    public string Model;
    public string RequiredDlcId;
  }
}
