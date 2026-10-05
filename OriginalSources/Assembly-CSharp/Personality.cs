// Decompiled with JetBrains decompiler
// Type: Personality
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Personality : Resource
{
  public List<Personality.StartingAttribute> attributes = new List<Personality.StartingAttribute>();
  public List<Trait> traits = new List<Trait>();
  public int headShape;
  public int mouth;
  public int neck;
  public int eyes;
  public int hair;
  public int body;
  public int belt;
  public int cuff;
  public int foot;
  public int hand;
  public int pelvis;
  public int leg;
  public int leg_skin;
  public int arm_skin;
  public int speech_mouth;
  public string nameStringKey;
  public string genderStringKey;
  public string personalityType;
  public Tag model;
  public string stresstrait;
  public string joyTrait;
  public string stickerType;
  public string congenitaltrait;
  public string unformattedDescription;
  public string graveStone;
  public bool startingMinion;
  public string requiredDlcId;

  public string description => this.GetDescription();

  [Obsolete("Modders: Use constructor with isStartingMinion parameter")]
  public Personality(
    string name_string_key,
    string name,
    string Gender,
    string PersonalityType,
    string StressTrait,
    string JoyTrait,
    string StickerType,
    string CongenitalTrait,
    int headShape,
    int mouth,
    int neck,
    int eyes,
    int hair,
    int body,
    string description)
    : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, true, "", GameTags.Minions.Models.Standard, 0)
  {
  }

  [Obsolete("Modders: Added additional body part customization to duplicant personalities")]
  public Personality(
    string name_string_key,
    string name,
    string Gender,
    string PersonalityType,
    string StressTrait,
    string JoyTrait,
    string StickerType,
    string CongenitalTrait,
    int headShape,
    int mouth,
    int neck,
    int eyes,
    int hair,
    int body,
    string description,
    bool isStartingMinion)
    : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, true, "", GameTags.Minions.Models.Standard, 0)
  {
  }

  [Obsolete("Modders: Added a custom gravestone image to duplicant personalities")]
  public Personality(
    string name_string_key,
    string name,
    string Gender,
    string PersonalityType,
    string StressTrait,
    string JoyTrait,
    string StickerType,
    string CongenitalTrait,
    int headShape,
    int mouth,
    int neck,
    int eyes,
    int hair,
    int body,
    int belt,
    int cuff,
    int foot,
    int hand,
    int pelvis,
    int leg,
    string description,
    bool isStartingMinion)
    : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, isStartingMinion, "", GameTags.Minions.Models.Standard, 0)
  {
  }

  [Obsolete("Modders: Added 'model', 'arm_skin' and 'leg skin' to duplicant personalities")]
  public Personality(
    string name_string_key,
    string name,
    string Gender,
    string PersonalityType,
    string StressTrait,
    string JoyTrait,
    string StickerType,
    string CongenitalTrait,
    int headShape,
    int mouth,
    int neck,
    int eyes,
    int hair,
    int body,
    int belt,
    int cuff,
    int foot,
    int hand,
    int pelvis,
    int leg,
    string description,
    bool isStartingMinion,
    string graveStone)
    : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, isStartingMinion, "", GameTags.Minions.Models.Standard, 0)
  {
  }

  [Obsolete("Modders: Added override_speech_mouth to duplicant personalities")]
  public Personality(
    string name_string_key,
    string name,
    string Gender,
    string PersonalityType,
    string StressTrait,
    string JoyTrait,
    string StickerType,
    string CongenitalTrait,
    int headShape,
    int mouth,
    int neck,
    int eyes,
    int hair,
    int body,
    int belt,
    int cuff,
    int foot,
    int hand,
    int pelvis,
    int leg,
    int arm_skin,
    int leg_skin,
    string description,
    bool isStartingMinion,
    string graveStone,
    Tag model)
    : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, belt, cuff, foot, hand, pelvis, leg, arm_skin, leg_skin, description, isStartingMinion, graveStone, model, 0)
  {
  }

  public Personality(
    string name_string_key,
    string name,
    string Gender,
    string PersonalityType,
    string StressTrait,
    string JoyTrait,
    string StickerType,
    string CongenitalTrait,
    int headShape,
    int mouth,
    int neck,
    int eyes,
    int hair,
    int body,
    int belt,
    int cuff,
    int foot,
    int hand,
    int pelvis,
    int leg,
    int arm_skin,
    int leg_skin,
    string description,
    bool isStartingMinion,
    string graveStone,
    Tag model,
    int SpeechMouth)
    : base(name_string_key, name)
  {
    this.nameStringKey = name_string_key;
    this.genderStringKey = Gender;
    this.personalityType = PersonalityType;
    this.stresstrait = StressTrait;
    this.joyTrait = JoyTrait;
    this.stickerType = StickerType;
    this.congenitaltrait = CongenitalTrait;
    this.unformattedDescription = description;
    this.headShape = headShape;
    this.mouth = mouth;
    this.neck = neck;
    this.eyes = eyes;
    this.hair = hair;
    this.body = body;
    this.belt = belt;
    this.cuff = cuff;
    this.foot = foot;
    this.hand = hand;
    this.pelvis = pelvis;
    this.leg = leg;
    this.arm_skin = arm_skin;
    this.leg_skin = leg_skin;
    this.startingMinion = isStartingMinion;
    this.graveStone = graveStone;
    this.model = model;
    this.speech_mouth = SpeechMouth;
  }

  public string GetDescription()
  {
    this.unformattedDescription = this.unformattedDescription.Replace("{0}", this.Name);
    return this.unformattedDescription;
  }

  public void SetAttribute(Klei.AI.Attribute attribute, int value)
  {
    this.attributes.Add(new Personality.StartingAttribute(attribute, value));
  }

  public void AddTrait(Trait trait) => this.traits.Add(trait);

  public void SetSelectedTemplateOutfitId(
    ClothingOutfitUtility.OutfitType outfitType,
    Option<string> outfit)
  {
    CustomClothingOutfits.Instance.Internal_SetDuplicantPersonalityOutfit(outfitType, this.Id, outfit);
  }

  public string GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType)
  {
    string outfitId;
    return CustomClothingOutfits.Instance.Internal_TryGetDuplicantPersonalityOutfit(outfitType, this.Id, out outfitId) ? outfitId : (string) null;
  }

  public Sprite GetMiniIcon()
  {
    return string.IsNullOrWhiteSpace(this.nameStringKey) ? Assets.GetSprite((HashedString) "unknown") : Assets.GetSprite((HashedString) ("dreamIcon_" + (!(this.nameStringKey == "MIMA") ? this.nameStringKey[0].ToString() + this.nameStringKey.Substring(1).ToLower() : "Mi-Ma")));
  }

  public class StartingAttribute
  {
    public Klei.AI.Attribute attribute;
    public int value;

    public StartingAttribute(Klei.AI.Attribute attribute, int value)
    {
      this.attribute = attribute;
      this.value = value;
    }
  }
}
