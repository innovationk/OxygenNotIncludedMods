// Decompiled with JetBrains decompiler
// Type: MinionStartingStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MinionStartingStats : ITelepadDeliverable
{
  public string Name;
  public string NameStringKey;
  public string GenderStringKey;
  public List<Trait> Traits = new List<Trait>();
  public int rarityBalance;
  public Trait stressTrait;
  public Trait joyTrait;
  public Trait congenitaltrait;
  public string stickerType;
  public int voiceIdx;
  public Dictionary<string, int> StartingLevels = new Dictionary<string, int>();
  public Personality personality;
  public List<Accessory> accessories = new List<Accessory>();
  public bool IsValid;
  public string overrideOutfitID;
  public Dictionary<SkillGroup, float> skillAptitudes = new Dictionary<SkillGroup, float>();

  public MinionStartingStats(
    Personality personality,
    string guaranteedAptitudeID = null,
    string guaranteedTraitID = null,
    bool isDebugMinion = false)
  {
    this.personality = personality;
    this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion);
  }

  public MinionStartingStats(
    bool is_starter_minion,
    string guaranteedAptitudeID = null,
    string guaranteedTraitID = null,
    bool isDebugMinion = false)
  {
    this.personality = Db.Get().Personalities.GetRandom(true, is_starter_minion);
    this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
  }

  public MinionStartingStats(
    Tag model,
    bool is_starter_minion,
    string guaranteedAptitudeID = null,
    string guaranteedTraitID = null,
    bool isDebugMinion = false)
  {
    this.personality = Db.Get().Personalities.GetRandom(model, true, is_starter_minion);
    this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
  }

  public MinionStartingStats(
    List<Tag> models,
    bool is_starter_minion,
    string guaranteedAptitudeID = null,
    string guaranteedTraitID = null,
    bool isDebugMinion = false)
  {
    this.personality = Db.Get().Personalities.GetRandom(models, true, is_starter_minion);
    this.GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
  }

  private void GenerateStats(
    string guaranteedAptitudeID = null,
    string guaranteedTraitID = null,
    bool isDebugMinion = false,
    bool is_starter_minion = false)
  {
    this.voiceIdx = UnityEngine.Random.Range(0, 4);
    this.Name = this.personality.Name;
    this.NameStringKey = this.personality.nameStringKey;
    this.GenderStringKey = this.personality.genderStringKey;
    this.Traits.Add(Db.Get().traits.Get(BaseMinionConfig.GetMinionBaseTraitIDForModel(this.personality.model)));
    List<ChoreGroup> disabled_chore_groups = new List<ChoreGroup>();
    this.GenerateAptitudes(guaranteedAptitudeID);
    this.GenerateAttributes(this.GenerateTraits(is_starter_minion, disabled_chore_groups, guaranteedAptitudeID, guaranteedTraitID, isDebugMinion), disabled_chore_groups);
    KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(this.personality);
    foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
    {
      if (resource.accessories.Count != 0)
      {
        Accessory accessory = (Accessory) null;
        if (resource == Db.Get().AccessorySlots.HeadShape)
        {
          accessory = resource.Lookup(bodyData.headShape);
          if (accessory == null)
            this.personality.headShape = 0;
        }
        else if (resource == Db.Get().AccessorySlots.Mouth)
        {
          accessory = resource.Lookup(bodyData.mouth);
          if (accessory == null)
            this.personality.mouth = 0;
        }
        else if (resource == Db.Get().AccessorySlots.Eyes)
        {
          accessory = resource.Lookup(bodyData.eyes);
          if (accessory == null)
            this.personality.eyes = 0;
        }
        else if (resource == Db.Get().AccessorySlots.Hair)
        {
          accessory = resource.Lookup(bodyData.hair);
          if (accessory == null)
            this.personality.hair = 0;
        }
        else if (resource == Db.Get().AccessorySlots.HatHair)
          accessory = resource.accessories[0];
        else if (resource == Db.Get().AccessorySlots.Body)
        {
          accessory = resource.Lookup(bodyData.body);
          if (accessory == null)
            this.personality.body = 0;
        }
        else if (resource == Db.Get().AccessorySlots.Arm)
          accessory = resource.Lookup(bodyData.arms);
        else if (resource == Db.Get().AccessorySlots.ArmLower)
          accessory = resource.Lookup(bodyData.armslower);
        else if (resource == Db.Get().AccessorySlots.ArmLowerSkin)
          accessory = resource.Lookup(bodyData.armLowerSkin);
        else if (resource == Db.Get().AccessorySlots.ArmUpperSkin)
          accessory = resource.Lookup(bodyData.armUpperSkin);
        else if (resource == Db.Get().AccessorySlots.LegSkin)
          accessory = resource.Lookup(bodyData.legSkin);
        else if (resource == Db.Get().AccessorySlots.Leg)
          accessory = resource.Lookup(bodyData.legs);
        else if (resource == Db.Get().AccessorySlots.Belt)
          accessory = resource.Lookup(bodyData.belt) ?? resource.accessories[0];
        else if (resource == Db.Get().AccessorySlots.Neck)
          accessory = resource.Lookup(bodyData.neck);
        else if (resource == Db.Get().AccessorySlots.Pelvis)
          accessory = resource.Lookup(bodyData.pelvis);
        else if (resource == Db.Get().AccessorySlots.Foot)
          accessory = resource.Lookup(bodyData.foot) ?? resource.accessories[0];
        else if (resource == Db.Get().AccessorySlots.Skirt)
          accessory = resource.Lookup(bodyData.skirt);
        else if (resource == Db.Get().AccessorySlots.Necklace)
          accessory = resource.Lookup(bodyData.necklace);
        else if (resource == Db.Get().AccessorySlots.Cuff)
          accessory = resource.Lookup(bodyData.cuff) ?? resource.accessories[0];
        else if (resource == Db.Get().AccessorySlots.Hand)
          accessory = resource.Lookup(bodyData.hand) ?? resource.accessories[0];
        this.accessories.Add(accessory);
      }
    }
  }

  private int GenerateTraits(
    bool is_starter_minion,
    List<ChoreGroup> disabled_chore_groups,
    string guaranteedAptitudeID = null,
    string guaranteedTraitID = null,
    bool isDebugMinion = false)
  {
    int statDelta = 0;
    List<string> selectedTraits = new List<string>();
    KRandom randSeed = new KRandom();
    this.stressTrait = Db.Get().traits.Get(this.personality.stresstrait);
    this.joyTrait = Db.Get().traits.Get(this.personality.joyTrait);
    this.stickerType = this.personality.stickerType;
    Trait trait1 = Db.Get().traits.TryGet(this.personality.congenitaltrait);
    this.congenitaltrait = trait1 == null || trait1.Name == "None" ? (Trait) null : trait1;
    if (this.personality.model == GameTags.Minions.Models.Bionic)
    {
      DUPLICANTSTATS.TraitVal random = DUPLICANTSTATS.BIONICBUGTRAITS.GetRandom<DUPLICANTSTATS.TraitVal>();
      SelectTrait(random, Db.Get().traits.Get(random.id), false);
      DUPLICANTSTATS.TraitVal traitVal = guaranteedAptitudeID == null ? DUPLICANTSTATS.BIONICUPGRADETRAITS.GetRandom<DUPLICANTSTATS.TraitVal>() : this.GetBionicTraitsCompatibleWithArchetype(guaranteedAptitudeID).GetRandom<DUPLICANTSTATS.TraitVal>();
      SelectTrait(traitVal, Db.Get().traits.Get(traitVal.id), true);
      this.IsValid = true;
      return statDelta;
    }
    Func<List<DUPLICANTSTATS.TraitVal>, bool, bool> func = (Func<List<DUPLICANTSTATS.TraitVal>, bool, bool>) ((traitPossibilities, positiveTrait) =>
    {
      if (this.Traits.Count > DUPLICANTSTATS.MAX_TRAITS)
        return false;
      double num1 = (double) Mathf.Abs(Util.GaussianRandom());
      int count = traitPossibilities.Count;
      int num2;
      if (!positiveTrait)
      {
        if (DUPLICANTSTATS.rarityDeckActive.Count < 1)
          DUPLICANTSTATS.rarityDeckActive.AddRange((IEnumerable<int>) DUPLICANTSTATS.RARITY_DECK);
        if (DUPLICANTSTATS.rarityDeckActive.Count == DUPLICANTSTATS.RARITY_DECK.Count)
          DUPLICANTSTATS.rarityDeckActive.ShuffleSeeded<int>(randSeed);
        num2 = DUPLICANTSTATS.rarityDeckActive[DUPLICANTSTATS.rarityDeckActive.Count - 1];
        DUPLICANTSTATS.rarityDeckActive.RemoveAt(DUPLICANTSTATS.rarityDeckActive.Count - 1);
      }
      else
      {
        List<int> list = new List<int>();
        if (is_starter_minion)
        {
          list.Add(this.rarityBalance - 1);
          list.Add(this.rarityBalance);
          list.Add(this.rarityBalance);
          list.Add(this.rarityBalance + 1);
        }
        else
        {
          list.Add(this.rarityBalance - 2);
          list.Add(this.rarityBalance - 1);
          list.Add(this.rarityBalance);
          list.Add(this.rarityBalance + 1);
          list.Add(this.rarityBalance + 2);
        }
        list.ShuffleSeeded<int>(randSeed);
        int b1 = list[0];
        int b2 = Mathf.Max(DUPLICANTSTATS.RARITY_COMMON, b1);
        num2 = Mathf.Min(DUPLICANTSTATS.RARITY_LEGENDARY, b2);
      }
      List<DUPLICANTSTATS.TraitVal> list1 = new List<DUPLICANTSTATS.TraitVal>((IEnumerable<DUPLICANTSTATS.TraitVal>) traitPossibilities);
      for (int index = list1.Count - 1; index > -1; --index)
      {
        if (list1[index].rarity != num2)
        {
          list1.RemoveAt(index);
          --count;
        }
      }
      list1.ShuffleSeeded<DUPLICANTSTATS.TraitVal>(randSeed);
      foreach (DUPLICANTSTATS.TraitVal traitVal in list1)
      {
        Debug.Assert((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null, (object) "IsDLCActiveForCurrentSave should not be called from the front end");
        if (!Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) traitVal))
          --count;
        else if (selectedTraits.Contains(traitVal.id))
        {
          --count;
        }
        else
        {
          Trait trait2 = Db.Get().traits.TryGet(traitVal.id);
          if (trait2 == null)
          {
            Debug.LogWarning((object) ("Trying to add nonexistent trait: " + traitVal.id));
            --count;
          }
          else if (!isDebugMinion || trait2.disabledChoreGroups == null || trait2.disabledChoreGroups.Length == 0)
          {
            if (is_starter_minion && !trait2.ValidStarterTrait)
              --count;
            else if (traitVal.doNotGenerateTrait)
              --count;
            else if (this.AreTraitAndAptitudesExclusive(traitVal, this.skillAptitudes))
              --count;
            else if (is_starter_minion && guaranteedAptitudeID != null && this.AreTraitAndArchetypeExclusive(traitVal, guaranteedAptitudeID))
              --count;
            else if (this.AreTraitsMutuallyExclusive(traitVal, selectedTraits))
            {
              --count;
            }
            else
            {
              SelectTrait(traitVal, trait2, positiveTrait);
              return true;
            }
          }
        }
      }
      return false;
    });
    int num3;
    int num4;
    if (is_starter_minion)
    {
      num3 = 1;
      num4 = 1;
    }
    else
    {
      if (DUPLICANTSTATS.podTraitConfigurationsActive.Count < 1)
        DUPLICANTSTATS.podTraitConfigurationsActive.AddRange((IEnumerable<Tuple<int, int>>) DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);
      if (DUPLICANTSTATS.podTraitConfigurationsActive.Count == DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK.Count)
        DUPLICANTSTATS.podTraitConfigurationsActive.ShuffleSeeded<Tuple<int, int>>(randSeed);
      num3 = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].first;
      num4 = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].second;
      DUPLICANTSTATS.podTraitConfigurationsActive.RemoveAt(DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1);
    }
    bool flag = false;
    int num5 = 0;
    int num6 = 0;
    int num7 = (num4 + num3) * 4;
    if (!string.IsNullOrEmpty(guaranteedTraitID))
    {
      DUPLICANTSTATS.TraitVal traitVal = DUPLICANTSTATS.GetTraitVal(guaranteedTraitID);
      if (traitVal.id == guaranteedTraitID)
      {
        Trait trait3 = Db.Get().traits.TryGet(traitVal.id);
        bool positiveTrait = trait3.PositiveTrait;
        selectedTraits.Add(traitVal.id);
        statDelta += traitVal.statBonus;
        this.rarityBalance += positiveTrait ? -traitVal.rarity : traitVal.rarity;
        this.Traits.Add(trait3);
        if (trait3.disabledChoreGroups != null)
        {
          for (int index = 0; index < trait3.disabledChoreGroups.Length; ++index)
            disabled_chore_groups.Add(trait3.disabledChoreGroups[index]);
        }
        if (positiveTrait)
          ++num5;
        else
          ++num6;
      }
    }
    if (!flag)
    {
      if (this.congenitaltrait != null)
      {
        DUPLICANTSTATS.TraitVal traitVal;
        if (this.congenitaltrait.PositiveTrait)
        {
          ++num5;
          traitVal = DUPLICANTSTATS.GOODTRAITS.Find((Predicate<DUPLICANTSTATS.TraitVal>) (match => match.id == this.congenitaltrait.Id));
        }
        else
        {
          ++num6;
          traitVal = DUPLICANTSTATS.BADTRAITS.Find((Predicate<DUPLICANTSTATS.TraitVal>) (match => match.id == this.congenitaltrait.Id));
        }
        SelectTrait(traitVal, this.congenitaltrait, this.congenitaltrait.PositiveTrait);
      }
    }
    for (; num7 > 0 && (num6 < num4 || num5 < num3); --num7)
    {
      if (num6 < num4 && func(DUPLICANTSTATS.BADTRAITS, false))
        ++num6;
      if (num5 < num3 && func(DUPLICANTSTATS.GOODTRAITS, true))
        ++num5;
    }
    if (num7 > 0)
      this.IsValid = true;
    return statDelta;

    void SelectTrait(DUPLICANTSTATS.TraitVal traitVal, Trait trait, bool isPositiveTrait)
    {
      selectedTraits.Add(traitVal.id);
      statDelta += traitVal.statBonus;
      this.rarityBalance += isPositiveTrait ? -traitVal.rarity : traitVal.rarity;
      this.Traits.Add(trait);
      if (trait.disabledChoreGroups == null)
        return;
      for (int index = 0; index < trait.disabledChoreGroups.Length; ++index)
        disabled_chore_groups.Add(trait.disabledChoreGroups[index]);
    }
  }

  private void GenerateAptitudes(string guaranteedAptitudeID = null)
  {
    if (this.personality.model == BionicMinionConfig.MODEL)
      return;
    int num = UnityEngine.Random.Range(1, 4);
    List<SkillGroup> list = new List<SkillGroup>((IEnumerable<SkillGroup>) Db.Get().SkillGroups.resources);
    list.RemoveAll((Predicate<SkillGroup>) (match => !match.allowAsAptitude));
    list.Shuffle<SkillGroup>();
    if (guaranteedAptitudeID != null)
    {
      this.skillAptitudes.Add(Db.Get().SkillGroups.Get(guaranteedAptitudeID), (float) DUPLICANTSTATS.APTITUDE_BONUS);
      list.Remove(Db.Get().SkillGroups.Get(guaranteedAptitudeID));
      --num;
    }
    for (int index = 0; index < num; ++index)
      this.skillAptitudes.Add(list[index], (float) DUPLICANTSTATS.APTITUDE_BONUS);
  }

  private void GenerateAttributes(int pointsDelta, List<ChoreGroup> disabled_chore_groups)
  {
    List<string> stringList = new List<string>((IEnumerable<string>) DUPLICANTSTATS.ALL_ATTRIBUTES);
    for (int index = 0; index < stringList.Count; ++index)
    {
      if (!this.StartingLevels.ContainsKey(stringList[index]))
        this.StartingLevels[stringList[index]] = 0;
    }
    foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.skillAptitudes)
    {
      if (skillAptitude.Key.relevantAttributes.Count > 0)
      {
        for (int index = 0; index < skillAptitude.Key.relevantAttributes.Count; ++index)
        {
          if (!this.StartingLevels.ContainsKey(skillAptitude.Key.relevantAttributes[index].Id))
            Debug.LogError((object) $"Need to add {skillAptitude.Key.relevantAttributes[index].Id} to TUNING.DUPLICANTSTATS.ALL_ATTRIBUTES");
          this.StartingLevels[skillAptitude.Key.relevantAttributes[index].Id] += DUPLICANTSTATS.APTITUDE_ATTRIBUTE_BONUSES[this.skillAptitudes.Count - 1];
        }
      }
    }
    int num1 = pointsDelta;
    List<SkillGroup> list = new List<SkillGroup>((IEnumerable<SkillGroup>) this.skillAptitudes.Keys);
    if (pointsDelta > 0)
    {
      for (int index1 = num1; index1 > 0; --index1)
      {
        list.Shuffle<SkillGroup>();
        for (int index2 = 0; index2 < list[0].relevantAttributes.Count; ++index2)
          ++this.StartingLevels[list[0].relevantAttributes[index2].Id];
      }
    }
    if (disabled_chore_groups.Count <= 0)
      return;
    int num2 = 0;
    int num3 = 0;
    foreach (KeyValuePair<string, int> startingLevel in this.StartingLevels)
    {
      if (startingLevel.Value > num2)
        num2 = startingLevel.Value;
      if (startingLevel.Key == disabled_chore_groups[0].attribute.Id)
        num3 = startingLevel.Value;
    }
    if (num2 != num3)
      return;
    foreach (string key in stringList)
    {
      if (key != disabled_chore_groups[0].attribute.Id)
      {
        int num4 = 0;
        this.StartingLevels.TryGetValue(key, out num4);
        int num5 = 0;
        if (num4 > 0)
          num5 = 1;
        this.StartingLevels[disabled_chore_groups[0].attribute.Id] = num4 - num5;
        this.StartingLevels[key] = num2 + num5;
        break;
      }
    }
  }

  public Option<ClothingOutfitTarget> GetSelectedOutfitOption()
  {
    Option<ClothingOutfitTarget> option = new Option<ClothingOutfitTarget>();
    return this.overrideOutfitID == null ? ClothingOutfitTarget.TryFromTemplateId(this.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.Clothing)) : ClothingOutfitTarget.TryFromTemplateId(this.overrideOutfitID);
  }

  public void Apply(GameObject go)
  {
    MinionIdentity component = go.GetComponent<MinionIdentity>();
    component.SetName(this.Name);
    component.nameStringKey = this.NameStringKey;
    component.genderStringKey = this.GenderStringKey;
    component.personalityResourceId = this.personality.IdHash;
    component.model = this.personality.model;
    this.ApplyTraits(go);
    this.ApplyRace(go);
    this.ApplyAptitudes(go);
    this.ApplyAccessories(go);
    this.ApplyExperience(go);
    this.ApplyOutfit(this.personality, go, this.GetSelectedOutfitOption());
    this.ApplyJoyResponseOutfit(this.personality, go);
  }

  public void ApplyExperience(GameObject go)
  {
    foreach (KeyValuePair<string, int> startingLevel in this.StartingLevels)
      go.GetComponent<AttributeLevels>().SetLevel(startingLevel.Key, startingLevel.Value);
  }

  public void ApplyAccessories(GameObject go)
  {
    Accessorizer component = go.GetComponent<Accessorizer>();
    component.ApplyMinionPersonality(this.personality);
    component.UpdateHairBasedOnHat();
  }

  public void ApplyOutfit(
    Personality personality,
    GameObject go,
    Option<ClothingOutfitTarget> overrideOutfit)
  {
    WearableAccessorizer component = go.GetComponent<WearableAccessorizer>();
    Option<ClothingOutfitTarget> option = overrideOutfit;
    if (option.IsSome())
      component.ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, option.Unwrap().ReadItemValues());
    else
      component.ClearClothingItems();
  }

  public void ApplyJoyResponseOutfit(Personality personality, GameObject go)
  {
    JoyResponseOutfitTarget responseOutfitTarget = JoyResponseOutfitTarget.FromPersonality(personality);
    JoyResponseOutfitTarget.FromMinion(go).WriteFacadeId(responseOutfitTarget.ReadFacadeId());
  }

  public void ApplyRace(GameObject go)
  {
    go.GetComponent<MinionIdentity>().voiceIdx = this.voiceIdx;
  }

  public static KCompBuilder.BodyData CreateBodyData(Personality p)
  {
    KCompBuilder.BodyData bodyData = new KCompBuilder.BodyData()
    {
      eyes = HashCache.Get().Add($"eyes_{p.eyes:000}"),
      hair = HashCache.Get().Add($"hair_{p.hair:000}"),
      headShape = HashCache.Get().Add($"headshape_{p.headShape:000}"),
      mouth = HashCache.Get().Add($"mouth_{p.mouth:000}"),
      neck = HashCache.Get().Add("neck"),
      arms = HashCache.Get().Add($"arm_sleeve_{p.body:000}"),
      armslower = HashCache.Get().Add($"arm_lower_sleeve_{p.body:000}"),
      body = HashCache.Get().Add($"torso_{p.body:000}"),
      hat = HashedString.Invalid,
      faceFX = HashedString.Invalid,
      armLowerSkin = HashCache.Get().Add($"arm_lower_{(p.arm_skin != 0 ? p.arm_skin : p.headShape):000}"),
      armUpperSkin = HashCache.Get().Add($"arm_upper_{(p.arm_skin != 0 ? p.arm_skin : p.headShape):000}"),
      legSkin = HashCache.Get().Add($"leg_skin_{(p.leg_skin != 0 ? p.leg_skin : p.headShape):000}")
    };
    bodyData.neck = HashCache.Get().Add(p.neck != 0 ? $"neck_{p.neck:000}" : "neck");
    bodyData.legs = HashCache.Get().Add(p.leg != 0 ? $"leg_{p.leg:000}" : "leg");
    bodyData.belt = HashCache.Get().Add(p.belt != 0 ? $"belt_{p.belt:000}" : "belt");
    bodyData.pelvis = HashCache.Get().Add(p.pelvis != 0 ? $"pelvis_{p.pelvis:000}" : "pelvis");
    bodyData.foot = HashCache.Get().Add(p.foot != 0 ? $"foot_{p.foot:000}" : "foot");
    bodyData.hand = HashCache.Get().Add(p.hand != 0 ? $"hand_paint_{p.hand:000}" : "hand_paint");
    bodyData.cuff = HashCache.Get().Add(p.cuff != 0 ? $"cuff_{p.cuff:000}" : "cuff");
    return bodyData;
  }

  public void ApplyAptitudes(GameObject go)
  {
    MinionResume component = go.GetComponent<MinionResume>();
    foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.skillAptitudes)
      component.SetAptitude((HashedString) skillAptitude.Key.Id, skillAptitude.Value);
  }

  public void ApplyTraits(GameObject go)
  {
    Klei.AI.Traits component1 = go.GetComponent<Klei.AI.Traits>();
    component1.Clear();
    foreach (Trait trait in this.Traits)
      component1.Add(trait);
    component1.Add(this.stressTrait);
    component1.Add(this.joyTrait);
    go.GetComponent<MinionIdentity>().SetStickerType(this.stickerType);
    MinionIdentity component2 = go.GetComponent<MinionIdentity>();
    component2.SetName(this.Name);
    component2.nameStringKey = this.NameStringKey;
    go.GetComponent<MinionIdentity>().SetGender(this.GenderStringKey);
  }

  public GameObject Deliver(Vector3 location)
  {
    GameObject prefab = Assets.GetPrefab(this.personality.model);
    GameObject gameObject = Util.KInstantiate(prefab);
    gameObject.name = prefab.name;
    gameObject.SetActive(true);
    gameObject.transform.SetLocalPosition(location);
    this.Apply(gameObject);
    Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
    EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", Telepad.PortalBirthAnim);
    return gameObject;
  }

  private List<DUPLICANTSTATS.TraitVal> GetBionicTraitsCompatibleWithArchetype(
    string guaranteedAptitudeID)
  {
    if (!DUPLICANTSTATS.ARCHETYPE_BIONIC_TRAIT_COMPATIBILITY.ContainsKey(guaranteedAptitudeID))
      Debug.LogError((object) $"Need to add attribute {guaranteedAptitudeID} to ARCHETYPE_BIONIC_TRAIT_COMPATIBILITY");
    List<DUPLICANTSTATS.TraitVal> compatibleWithArchetype = DUPLICANTSTATS.BIONICUPGRADETRAITS.FindAll((Predicate<DUPLICANTSTATS.TraitVal>) (t => DUPLICANTSTATS.ARCHETYPE_BIONIC_TRAIT_COMPATIBILITY[guaranteedAptitudeID].Contains(t.id) || DUPLICANTSTATS.ARCHETYPE_BIONIC_TRAIT_COMPATIBILITY[guaranteedAptitudeID].Contains(t.id.Replace("StartWith", ""))));
    if (compatibleWithArchetype.Count <= 0)
      compatibleWithArchetype = DUPLICANTSTATS.BIONICUPGRADETRAITS;
    return compatibleWithArchetype;
  }

  private bool AreTraitAndAptitudesExclusive(
    DUPLICANTSTATS.TraitVal traitVal,
    Dictionary<SkillGroup, float> aptitudes)
  {
    if (traitVal.mutuallyExclusiveAptitudes == null)
      return false;
    foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.skillAptitudes)
    {
      foreach (HashedString exclusiveAptitude in traitVal.mutuallyExclusiveAptitudes)
      {
        if (exclusiveAptitude == skillAptitude.Key.IdHash && (double) skillAptitude.Value > 0.0)
          return true;
      }
    }
    return false;
  }

  private bool AreTraitAndArchetypeExclusive(
    DUPLICANTSTATS.TraitVal traitVal,
    string guaranteedAptitudeID)
  {
    if (!DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS.ContainsKey(guaranteedAptitudeID))
      Debug.LogError((object) $"Need to add attribute {guaranteedAptitudeID} to ARCHETYPE_TRAIT_EXCLUSIONS");
    foreach (string str in DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS[guaranteedAptitudeID])
    {
      if (str == traitVal.id)
        return true;
    }
    return false;
  }

  private bool AreTraitsMutuallyExclusive(
    DUPLICANTSTATS.TraitVal traitVal,
    List<string> selectedTraits)
  {
    foreach (string selectedTrait in selectedTraits)
    {
      foreach (DUPLICANTSTATS.TraitVal traitVal1 in DUPLICANTSTATS.GOODTRAITS)
      {
        if (selectedTrait == traitVal1.id && traitVal1.mutuallyExclusiveTraits != null && traitVal1.mutuallyExclusiveTraits.Contains(traitVal.id))
          return true;
      }
      foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.BADTRAITS)
      {
        if (selectedTrait == traitVal2.id && traitVal2.mutuallyExclusiveTraits != null && traitVal2.mutuallyExclusiveTraits.Contains(traitVal.id))
          return true;
      }
      foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.CONGENITALTRAITS)
      {
        if (selectedTrait == traitVal3.id && traitVal3.mutuallyExclusiveTraits != null && traitVal3.mutuallyExclusiveTraits.Contains(traitVal.id))
          return true;
      }
      foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.SPECIALTRAITS)
      {
        if (selectedTrait == traitVal4.id && traitVal4.mutuallyExclusiveTraits != null && traitVal4.mutuallyExclusiveTraits.Contains(traitVal.id))
          return true;
      }
      if (traitVal.mutuallyExclusiveTraits != null && traitVal.mutuallyExclusiveTraits.Contains(selectedTrait))
        return true;
    }
    return false;
  }
}
