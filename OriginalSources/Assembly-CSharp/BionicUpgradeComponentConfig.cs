// Decompiled with JetBrains decompiler
// Type: BionicUpgradeComponentConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

#nullable disable
public class BionicUpgradeComponentConfig : IMultiEntityConfig
{
  public const string DEFAULT_ANIM_FILE_NAME = "upgrade_disc_kanim";
  public const string STARTING_TRAIT_PREFIX = "StartWith";
  public const string OVERCLOCKED_BOOSTER_TECH_ID = "OverclockedBoosters";
  public const string Booster_Dig1 = "Booster_Dig1";
  public const string Booster_Construct1 = "Booster_Construct1";
  public const string Booster_Dig2 = "Booster_Dig2";
  public const string Booster_Farm1 = "Booster_Farm1";
  public const string Booster_Ranch1 = "Booster_Ranch1";
  public const string Booster_Cook1 = "Booster_Cook1";
  public const string Booster_Art1 = "Booster_Art1";
  public const string Booster_Research1 = "Booster_Research1";
  public const string Booster_Research2 = "Booster_Research2";
  public const string Booster_Research3 = "Booster_Research3";
  public const string Booster_Pilot1 = "Booster_Pilot1";
  public const string Booster_PilotVanilla1 = "Booster_PilotVanilla1";
  public const string Booster_Suits1 = "Booster_Suits1";
  public const string Booster_Carry1 = "Booster_Carry1";
  public const string Booster_Op1 = "Booster_Op1";
  public const string Booster_Op2 = "Booster_Op2";
  public const string Booster_Medicine1 = "Booster_Medicine1";
  public const string Booster_Tidy1 = "Booster_Tidy1";
  public const string Booster_Mod_Swim = "Booster_Mod_Swim";
  public static List<string> BASIC_BOOSTERS = new List<string>()
  {
    nameof (Booster_Dig1),
    nameof (Booster_Construct1),
    nameof (Booster_Carry1),
    nameof (Booster_Research1),
    nameof (Booster_Medicine1)
  };
  public static Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData> UpgradesData = new Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>();

  public static string GenerateTooltipForBooster(BionicUpgradeComponent booster)
  {
    string str = $"<b>{booster.GetProperName()}</b>";
    InfoDescription component = booster.gameObject.GetComponent<InfoDescription>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      str = $"{str}\n{component.description}";
    return $"{str}\n\n{BionicUpgradeComponentConfig.UpgradesData[booster.PrefabID()].stateMachineDescription}";
  }

  public static Tag[] GetBoostersWithSkillPerk(string perkID)
  {
    return BionicUpgradeComponentConfig.UpgradesData.Where<KeyValuePair<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>>((Func<KeyValuePair<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>, bool>) (data => ((IEnumerable<string>) data.Value.skillPerks).Contains<string>(perkID))).Select<KeyValuePair<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>, Tag>((Func<KeyValuePair<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>, Tag>) (kvp => kvp.Key)).ToArray<Tag>();
  }

  public AttributeModifier[] CreateBoosterModifiers(
    string name,
    Dictionary<string, float> attributes)
  {
    AttributeModifier[] boosterModifiers = new AttributeModifier[attributes.Count];
    string description = (string) Strings.Get($"STRINGS.ITEMS.BIONIC_BOOSTERS.{name.ToUpper()}.NAME");
    int index = 0;
    foreach (KeyValuePair<string, float> attribute1 in attributes)
    {
      Klei.AI.Attribute attribute2 = Db.Get().Attributes.Get(attribute1.Key);
      boosterModifiers[index] = new AttributeModifier(attribute2.Id, attribute1.Value, description);
      ++index;
    }
    return boosterModifiers;
  }

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> prefabs = new List<GameObject>();
    if (!DlcManager.IsContentSubscribed("DLC3_ID"))
      return prefabs;
    string str1 = "Booster_Dig1";
    AttributeModifier[] boosterModifiers1 = this.CreateBoosterModifiers(str1, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Digging.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks1 = new SkillPerk[1]
    {
      Db.Get().SkillPerks.CanDigVeryFirm
    };
    string upgradeID1 = str1;
    AttributeModifier[] attributeModifierArray1 = boosterModifiers1;
    string id1 = Db.Get().Attributes.Digging.Id;
    AttributeModifier[] modifiers1 = attributeModifierArray1;
    SkillPerk[] skillPerks2 = skillPerks1;
    string[] hats1 = new string[2]
    {
      "hat_role_mining1",
      "hat_role_mining2"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def1 = new BionicUpgrade_SkilledWorker.Def(upgradeID1, id1, modifiers1, skillPerks2, hats1);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str1, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def1)), sm_description: $"{skill_worker_def1.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "basic_excavation_0", isStartingBooster: true, isCarePackage: true, skillPerks: skillPerks1));
    string str2 = "Booster_Construct1";
    AttributeModifier[] boosterModifiers2 = this.CreateBoosterModifiers(str2, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Construction.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks3 = new SkillPerk[1]
    {
      Db.Get().SkillPerks.CanDemolish
    };
    string upgradeID2 = str2;
    AttributeModifier[] attributeModifierArray2 = boosterModifiers2;
    string id2 = Db.Get().Attributes.Construction.Id;
    AttributeModifier[] modifiers2 = attributeModifierArray2;
    SkillPerk[] skillPerks4 = skillPerks3;
    string[] hats2 = new string[3]
    {
      "hat_role_building1",
      "hat_role_building2",
      "hat_role_building3"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def2 = new BionicUpgrade_SkilledWorker.Def(upgradeID2, id2, modifiers2, skillPerks4, hats2);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str2, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def2)), sm_description: $"{skill_worker_def2.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "basic_construction_0", isStartingBooster: true, isCarePackage: true, skillPerks: skillPerks3));
    string str3 = "Booster_Carry1";
    AttributeModifier[] boosterModifiers3 = this.CreateBoosterModifiers(str3, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Strength.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks5 = new SkillPerk[1]
    {
      Db.Get().SkillPerks.IncreasedCarryBionics
    };
    string upgradeID3 = str3;
    AttributeModifier[] attributeModifierArray3 = boosterModifiers3;
    string id3 = Db.Get().Attributes.Athletics.Id;
    AttributeModifier[] modifiers3 = attributeModifierArray3;
    SkillPerk[] skillPerks6 = skillPerks5;
    string[] hats3 = new string[2]
    {
      "hat_role_hauling1",
      "hat_role_hauling2"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def3 = new BionicUpgrade_SkilledWorker.Def(upgradeID3, id3, modifiers3, skillPerks6, hats3);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str3, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def3)), sm_description: $"{skill_worker_def3.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "basic_strength_0", isCarePackage: true, skillPerks: skillPerks5));
    string str4 = "Booster_Research1";
    AttributeModifier[] boosterModifiers4 = this.CreateBoosterModifiers(str4, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Learning.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks7 = new SkillPerk[4]
    {
      Db.Get().SkillPerks.AllowAdvancedResearch,
      Db.Get().SkillPerks.CanStudyWorldObjects,
      Db.Get().SkillPerks.AllowGeyserTuning,
      Db.Get().SkillPerks.AllowChemistry
    };
    string upgradeID4 = str4;
    AttributeModifier[] attributeModifierArray4 = boosterModifiers4;
    string id4 = Db.Get().Attributes.Learning.Id;
    AttributeModifier[] modifiers4 = attributeModifierArray4;
    SkillPerk[] skillPerks8 = skillPerks7;
    string[] hats4 = new string[2]
    {
      "hat_role_research1",
      "hat_role_research2"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def4 = new BionicUpgrade_SkilledWorker.Def(upgradeID4, id4, modifiers4, skillPerks8, hats4);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str4, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def4)), sm_description: $"{skill_worker_def4.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "science_4", isCarePackage: true, skillPerks: skillPerks7));
    string str5 = "Booster_Medicine1";
    AttributeModifier[] boosterModifiers5 = this.CreateBoosterModifiers(str5, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Caring.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks9 = new SkillPerk[3]
    {
      Db.Get().SkillPerks.CanCompound,
      Db.Get().SkillPerks.CanDoctor,
      Db.Get().SkillPerks.CanAdvancedMedicine
    };
    string upgradeID5 = str5;
    AttributeModifier[] attributeModifierArray5 = boosterModifiers5;
    string id5 = Db.Get().Attributes.DoctoredLevel.Id;
    AttributeModifier[] modifiers5 = attributeModifierArray5;
    SkillPerk[] skillPerks10 = skillPerks9;
    string[] hats5 = new string[3]
    {
      "hat_role_medicalaid1",
      "hat_role_medicalaid2",
      "hat_role_medicalaid3"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def5 = new BionicUpgrade_SkilledWorker.Def(upgradeID5, id5, modifiers5, skillPerks10, hats5);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str5, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def5)), sm_description: $"{skill_worker_def5.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "medicine_0", isStartingBooster: true, isCarePackage: true, skillPerks: skillPerks9));
    string str6 = "Booster_Dig2";
    SkillPerk[] skillPerkArray;
    if (!DlcManager.IsExpansion1Active())
      skillPerkArray = new SkillPerk[2]
      {
        Db.Get().SkillPerks.CanDigNearlyImpenetrable,
        Db.Get().SkillPerks.CanDigSuperDuperHard
      };
    else
      skillPerkArray = new SkillPerk[3]
      {
        Db.Get().SkillPerks.CanDigNearlyImpenetrable,
        Db.Get().SkillPerks.CanDigSuperDuperHard,
        Db.Get().SkillPerks.CanDigRadioactiveMaterials
      };
    SkillPerk[] skillPerks11 = skillPerkArray;
    AttributeModifier[] boosterModifiers6 = this.CreateBoosterModifiers(str6, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Digging.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    string[] strArray1;
    if (!DlcManager.IsExpansion1Active())
      strArray1 = new string[1]{ "hat_role_mining3" };
    else
      strArray1 = new string[2]
      {
        "hat_role_mining3",
        "hat_role_mining4"
      };
    string[] strArray2 = strArray1;
    string upgradeID6 = str6;
    AttributeModifier[] attributeModifierArray6 = boosterModifiers6;
    string id6 = Db.Get().Attributes.Digging.Id;
    AttributeModifier[] modifiers6 = attributeModifierArray6;
    SkillPerk[] skillPerks12 = skillPerks11;
    string[] hats6 = strArray2;
    BionicUpgrade_SkilledWorker.Def skill_worker_def6 = new BionicUpgrade_SkilledWorker.Def(upgradeID6, id6, modifiers6, skillPerks12, hats6);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str6, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def6)), sm_description: $"{skill_worker_def6.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "excavation_1", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, isStartingBooster: true, isCarePackage: true, skillPerks: skillPerks11));
    string str7 = "Booster_Farm1";
    List<SkillPerk> skillPerkList1 = new List<SkillPerk>()
    {
      Db.Get().SkillPerks.CanFarmTinker,
      Db.Get().SkillPerks.CanFarmStation,
      Db.Get().SkillPerks.CanSalvagePlantFiber
    };
    if (DlcManager.IsContentSubscribed("DLC5_ID"))
      skillPerkList1.Add(Db.Get().SkillPerks.CanFarmClams);
    if (DlcManager.IsExpansion1Active())
      skillPerkList1.Add(Db.Get().SkillPerks.CanIdentifyMutantSeeds);
    AttributeModifier[] boosterModifiers7 = this.CreateBoosterModifiers(str7, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Botanist.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    string upgradeID7 = str7;
    AttributeModifier[] attributeModifierArray7 = boosterModifiers7;
    string id7 = Db.Get().Attributes.Botanist.Id;
    AttributeModifier[] modifiers7 = attributeModifierArray7;
    SkillPerk[] array1 = skillPerkList1.ToArray();
    string[] hats7 = new string[3]
    {
      "hat_role_farming1",
      "hat_role_farming2",
      "hat_role_farming3"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def7 = new BionicUpgrade_SkilledWorker.Def(upgradeID7, id7, modifiers7, array1, hats7);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str7, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def7)), sm_description: $"{skill_worker_def7.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "agriculture_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, isStartingBooster: true, skillPerks: skillPerkList1.ToArray()));
    string str8 = "Booster_Ranch1";
    AttributeModifier[] boosterModifiers8 = this.CreateBoosterModifiers(str8, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Ranching.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks13 = new SkillPerk[3]
    {
      Db.Get().SkillPerks.CanWrangleCreatures,
      Db.Get().SkillPerks.CanUseRanchStation,
      Db.Get().SkillPerks.CanUseMilkingStation
    };
    string upgradeID8 = str8;
    AttributeModifier[] attributeModifierArray8 = boosterModifiers8;
    string id8 = Db.Get().Attributes.Ranching.Id;
    AttributeModifier[] modifiers8 = attributeModifierArray8;
    SkillPerk[] skillPerks14 = skillPerks13;
    string[] hats8 = new string[2]
    {
      "hat_role_rancher1",
      "hat_role_rancher2"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def8 = new BionicUpgrade_SkilledWorker.Def(upgradeID8, id8, modifiers8, skillPerks14, hats8);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str8, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def8)), sm_description: $"{skill_worker_def8.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "ranching_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, isStartingBooster: true, skillPerks: skillPerks13));
    string str9 = "Booster_Cook1";
    AttributeModifier[] boosterModifiers9 = this.CreateBoosterModifiers(str9, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Cooking.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks15 = new SkillPerk[5]
    {
      Db.Get().SkillPerks.CanElectricGrill,
      Db.Get().SkillPerks.CanDeepFry,
      Db.Get().SkillPerks.CanGasRange,
      Db.Get().SkillPerks.CanSushiBar,
      Db.Get().SkillPerks.CanSpiceGrinder
    };
    string upgradeID9 = str9;
    AttributeModifier[] attributeModifierArray9 = boosterModifiers9;
    string id9 = Db.Get().Attributes.Cooking.Id;
    AttributeModifier[] modifiers9 = attributeModifierArray9;
    SkillPerk[] skillPerks16 = skillPerks15;
    string[] hats9 = new string[2]
    {
      "hat_role_cooking1",
      "hat_role_cooking2"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def9 = new BionicUpgrade_SkilledWorker.Def(upgradeID9, id9, modifiers9, skillPerks16, hats9);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str9, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def9)), sm_description: $"{skill_worker_def9.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "cooking_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, isStartingBooster: true, isCarePackage: true, skillPerks: skillPerks15));
    string str10 = "Booster_Art1";
    List<SkillPerk> skillPerkList2 = new List<SkillPerk>()
    {
      Db.Get().SkillPerks.CanArt,
      Db.Get().SkillPerks.CanClothingAlteration,
      Db.Get().SkillPerks.CanArtGreat
    };
    if (DlcManager.FeatureClusterSpaceEnabled())
      skillPerkList2.Add(Db.Get().SkillPerks.CanStudyArtifact);
    AttributeModifier[] boosterModifiers10 = this.CreateBoosterModifiers(str10, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Art.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    string upgradeID10 = str10;
    AttributeModifier[] attributeModifierArray10 = boosterModifiers10;
    string id10 = Db.Get().Attributes.Art.Id;
    AttributeModifier[] modifiers10 = attributeModifierArray10;
    SkillPerk[] array2 = skillPerkList2.ToArray();
    string[] hats10 = new string[3]
    {
      "hat_role_art1",
      "hat_role_art2",
      "hat_role_art3"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def10 = new BionicUpgrade_SkilledWorker.Def(upgradeID10, id10, modifiers10, array2, hats10);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str10, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def10)), sm_description: $"{skill_worker_def10.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "creativity_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, isStartingBooster: true, skillPerks: skillPerkList2.ToArray()));
    string str11 = "Booster_Research2";
    List<SkillPerk> skillPerkList3 = new List<SkillPerk>()
    {
      Db.Get().SkillPerks.CanMissionControl
    };
    if (DlcManager.FeatureClusterSpaceEnabled())
    {
      skillPerkList3.Add(Db.Get().SkillPerks.CanUseClusterTelescope);
      skillPerkList3.Add(Db.Get().SkillPerks.AllowOrbitalResearch);
    }
    else
      skillPerkList3.Add(Db.Get().SkillPerks.AllowInterstellarResearch);
    string[] strArray3;
    if (!DlcManager.IsExpansion1Active())
      strArray3 = new string[1]{ "hat_role_research3" };
    else
      strArray3 = new string[2]
      {
        "hat_role_research3",
        "hat_role_research4"
      };
    string[] strArray4 = strArray3;
    AttributeModifier[] boosterModifiers11 = this.CreateBoosterModifiers(str11, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Learning.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    string upgradeID11 = str11;
    AttributeModifier[] attributeModifierArray11 = boosterModifiers11;
    string id11 = Db.Get().Attributes.Learning.Id;
    AttributeModifier[] modifiers11 = attributeModifierArray11;
    SkillPerk[] array3 = skillPerkList3.ToArray();
    string[] hats11 = strArray4;
    BionicUpgrade_SkilledWorker.Def skill_worker_def11 = new BionicUpgrade_SkilledWorker.Def(upgradeID11, id11, modifiers11, array3, hats11);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str11, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def11)), sm_description: $"{skill_worker_def11.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "science_2", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, skillPerks: skillPerkList3.ToArray()));
    if (DlcManager.IsExpansion1Active())
    {
      string str12 = "Booster_Research3";
      AttributeModifier[] boosterModifiers12 = this.CreateBoosterModifiers(str12, new Dictionary<string, float>()
      {
        {
          Db.Get().Attributes.Learning.Id,
          5f
        },
        {
          Db.Get().Attributes.Athletics.Id,
          2f
        }
      });
      SkillPerk[] skillPerks17 = new SkillPerk[1]
      {
        Db.Get().SkillPerks.AllowNuclearResearch
      };
      string upgradeID12 = str12;
      AttributeModifier[] attributeModifierArray12 = boosterModifiers12;
      string id12 = Db.Get().Attributes.Learning.Id;
      AttributeModifier[] modifiers12 = attributeModifierArray12;
      SkillPerk[] skillPerks18 = skillPerks17;
      string[] hats12 = new string[1]
      {
        "hat_role_research5"
      };
      BionicUpgrade_SkilledWorker.Def skill_worker_def12 = new BionicUpgrade_SkilledWorker.Def(upgradeID12, id12, modifiers12, skillPerks18, hats12);
      prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str12, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def12)), sm_description: $"{skill_worker_def12.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "science_3", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, skillPerks: skillPerks17));
    }
    if (DlcManager.IsExpansion1Active())
    {
      string str13 = "Booster_Pilot1";
      AttributeModifier[] boosterModifiers13 = this.CreateBoosterModifiers(str13, new Dictionary<string, float>()
      {
        {
          Db.Get().Attributes.SpaceNavigation.Id,
          5f
        },
        {
          Db.Get().Attributes.Athletics.Id,
          2f
        }
      });
      SkillPerk[] skillPerks19 = new SkillPerk[1]
      {
        Db.Get().SkillPerks.CanUseRocketControlStation
      };
      string upgradeID13 = str13;
      AttributeModifier[] attributeModifierArray13 = boosterModifiers13;
      string id13 = Db.Get().Attributes.SpaceNavigation.Id;
      AttributeModifier[] modifiers13 = attributeModifierArray13;
      SkillPerk[] skillPerks20 = skillPerks19;
      string[] hats13 = new string[2]
      {
        "hat_role_astronaut1",
        "hat_role_astronaut2"
      };
      BionicUpgrade_SkilledWorker.Def skill_worker_def13 = new BionicUpgrade_SkilledWorker.Def(upgradeID13, id13, modifiers13, skillPerks20, hats13);
      prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str13, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def13)), sm_description: $"{skill_worker_def13.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "piloting_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, skillPerks: skillPerks19));
    }
    if (DlcManager.IsPureVanilla())
    {
      string str14 = "Booster_PilotVanilla1";
      AttributeModifier[] boosterModifiers14 = this.CreateBoosterModifiers(str14, new Dictionary<string, float>()
      {
        {
          Db.Get().Attributes.Athletics.Id,
          3f
        }
      });
      SkillPerk[] skillPerks21 = new SkillPerk[1]
      {
        Db.Get().SkillPerks.CanUseRockets
      };
      BionicUpgrade_SkilledWorker.Def skill_worker_def14 = new BionicUpgrade_SkilledWorker.Def(str14, (string) null, boosterModifiers14, skillPerks21, new string[2]
      {
        "hat_role_astronaut1",
        "hat_role_astronaut2"
      });
      prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str14, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def14)), sm_description: $"{skill_worker_def14.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "piloting_vanilla_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, skillPerks: skillPerks21));
    }
    string str15 = "Booster_Suits1";
    AttributeModifier[] boosterModifiers15 = this.CreateBoosterModifiers(str15, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Athletics.Id,
        5f
      }
    });
    SkillPerk[] skillPerks22 = new SkillPerk[2]
    {
      Db.Get().SkillPerks.ExosuitDurability,
      Db.Get().SkillPerks.ExosuitExpertise
    };
    string upgradeID14 = str15;
    AttributeModifier[] attributeModifierArray14 = boosterModifiers15;
    string id14 = Db.Get().Attributes.Athletics.Id;
    AttributeModifier[] modifiers14 = attributeModifierArray14;
    SkillPerk[] skillPerks23 = skillPerks22;
    string[] hats14 = new string[2]
    {
      "hat_role_suits1",
      "hat_role_suits2"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def15 = new BionicUpgrade_SkilledWorker.Def(upgradeID14, id14, modifiers14, skillPerks23, hats14);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str15, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def15)), sm_description: $"{skill_worker_def15.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "suits_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, isStartingBooster: true, skillPerks: skillPerks22));
    string str16 = "Booster_Tidy1";
    AttributeModifier[] boosterModifiers16 = this.CreateBoosterModifiers(str16, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Strength.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks24 = new SkillPerk[2]
    {
      Db.Get().SkillPerks.CanDoPlumbing,
      Db.Get().SkillPerks.CanMakeMissiles
    };
    string upgradeID15 = str16;
    AttributeModifier[] attributeModifierArray15 = boosterModifiers16;
    string id15 = Db.Get().Attributes.Strength.Id;
    AttributeModifier[] modifiers15 = attributeModifierArray15;
    SkillPerk[] skillPerks25 = skillPerks24;
    string[] hats15 = new string[3]
    {
      "hat_role_basekeeping1",
      "hat_role_basekeeping2",
      "hat_role_pyrotechnics"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def16 = new BionicUpgrade_SkilledWorker.Def(upgradeID15, id15, modifiers15, skillPerks25, hats15);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str16, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def16)), sm_description: $"{skill_worker_def16.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "tidy_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, skillPerks: skillPerks24));
    string str17 = "Booster_Op1";
    AttributeModifier[] boosterModifiers17 = this.CreateBoosterModifiers(str17, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Machinery.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks26 = new SkillPerk[2]
    {
      Db.Get().SkillPerks.CanPowerTinker,
      Db.Get().SkillPerks.CanCraftElectronics
    };
    string upgradeID16 = str17;
    AttributeModifier[] attributeModifierArray16 = boosterModifiers17;
    string id16 = Db.Get().Attributes.Machinery.Id;
    AttributeModifier[] modifiers16 = attributeModifierArray16;
    SkillPerk[] skillPerks27 = skillPerks26;
    string[] hats16 = new string[2]
    {
      "hat_role_technicals1",
      "hat_role_technicals2"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def17 = new BionicUpgrade_SkilledWorker.Def(upgradeID16, id16, modifiers16, skillPerks27, hats16);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str17, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def17)), sm_description: $"{skill_worker_def17.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "machinery_0", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, isStartingBooster: true, skillPerks: skillPerks26));
    string str18 = "Booster_Op2";
    AttributeModifier[] boosterModifiers18 = this.CreateBoosterModifiers(str18, new Dictionary<string, float>()
    {
      {
        Db.Get().Attributes.Machinery.Id,
        5f
      },
      {
        Db.Get().Attributes.Athletics.Id,
        2f
      }
    });
    SkillPerk[] skillPerks28 = new SkillPerk[1]
    {
      Db.Get().SkillPerks.ConveyorBuild
    };
    string upgradeID17 = str18;
    AttributeModifier[] attributeModifierArray17 = boosterModifiers18;
    string id17 = Db.Get().Attributes.Machinery.Id;
    AttributeModifier[] modifiers17 = attributeModifierArray17;
    SkillPerk[] skillPerks29 = skillPerks28;
    string[] hats17 = new string[1]
    {
      "hat_role_engineering1"
    };
    BionicUpgrade_SkilledWorker.Def skill_worker_def18 = new BionicUpgrade_SkilledWorker.Def(upgradeID17, id17, modifiers17, skillPerks29, hats17);
    prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str18, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def18)), sm_description: $"{skill_worker_def18.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: DlcManager.DLC3, animStateName: "machinery_1", booster: BionicUpgradeComponentConfig.BoosterType.Intermediate, skillPerks: skillPerks28));
    if (DlcManager.IsContentSubscribed("DLC5_ID"))
    {
      string str19 = "Booster_Mod_Swim";
      AttributeModifier[] boosterModifiers19 = this.CreateBoosterModifiers(str19, new Dictionary<string, float>()
      {
        {
          Db.Get().Attributes.Athletics.Id,
          5f
        },
        {
          Db.Get().Amounts.Stress.deltaAttribute.Id,
          0.0166666675f
        }
      });
      SkillPerk[] skillPerks30 = new SkillPerk[5]
      {
        Db.Get().SkillPerks.CanSwim,
        Db.Get().SkillPerks.IncreasedCarryBionicsMinor,
        Db.Get().SkillPerks.ExosuitDurability,
        Db.Get().SkillPerks.ExosuitExpertise,
        Db.Get().SkillPerks.ImprovedLiquidTemperatureTolerance
      };
      string upgradeID18 = str19;
      AttributeModifier[] attributeModifierArray18 = boosterModifiers19;
      string id18 = Db.Get().Attributes.Athletics.Id;
      AttributeModifier[] modifiers18 = attributeModifierArray18;
      SkillPerk[] skillPerks31 = skillPerks30;
      string[] hats18 = new string[2]
      {
        "hat_role_swimming",
        "hat_role_swimming2"
      };
      BionicUpgrade_SkilledWorker.Def skill_worker_def19 = new BionicUpgrade_SkilledWorker.Def(upgradeID18, id18, modifiers18, skillPerks31, hats18);
      prefabs.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(str19, stateMachine: (Func<StateMachine.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def19)), sm_description: $"{skill_worker_def19.GetDescription()}\n\n{string.Format((string) STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, (object) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME)}", dlcIDs: new string[2]
      {
        "DLC3_ID",
        "DLC5_ID"
      }, animStateName: "mod_swim", craftTechUnlockID: "ArtificialFriends", booster: BionicUpgradeComponentConfig.BoosterType.Overclocked, skillPerks: skillPerks30, recipeInputOverride: new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement((Tag) "Booster_Suits1", 1f),
        new ComplexRecipe.RecipeElement((Tag) "PowerStationTools", 4f)
      }));
    }
    prefabs.RemoveAll((Predicate<GameObject>) (t => (UnityEngine.Object) t == (UnityEngine.Object) null));
    return prefabs;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public static Tag GetBionicUpgradePrefabIDWithTraitID(string traitID)
  {
    foreach (Tag key in BionicUpgradeComponentConfig.UpgradesData.Keys)
    {
      BionicUpgradeComponentConfig.BionicUpgradeData bionicUpgradeData = BionicUpgradeComponentConfig.UpgradesData[key];
      if (bionicUpgradeData.relatedTrait != null && bionicUpgradeData.relatedTrait == traitID)
        return key;
    }
    return Tag.Invalid;
  }

  public static GameObject CreateNewUpgradeComponent(
    string id,
    string name = null,
    string desc = null,
    float wattageCost = 0.0f,
    Func<StateMachine.Instance, StateMachine.Instance> stateMachine = null,
    string sm_description = "",
    string[] dlcIDs = null,
    string animFile = "upgrade_disc_kanim",
    string animStateName = "object",
    SimHashes element = SimHashes.Creature,
    string craftTechUnlockID = null,
    BionicUpgradeComponentConfig.BoosterType booster = BionicUpgradeComponentConfig.BoosterType.Basic,
    bool isStartingBooster = false,
    bool isCarePackage = false,
    SkillPerk[] skillPerks = null,
    ComplexRecipe.RecipeElement[] recipeInputOverride = null,
    bool addTechItem = true)
  {
    if (!DlcManager.IsAllContentSubscribed(dlcIDs))
      return (GameObject) null;
    if (name == null)
      name = (string) Strings.Get($"STRINGS.ITEMS.BIONIC_BOOSTERS.{id.ToUpper()}.NAME");
    if (desc == null)
      desc = (string) Strings.Get($"STRINGS.ITEMS.BIONIC_BOOSTERS.{id.ToUpper()}.DESC");
    string ID = id;
    TechItem techItem = new TechItem(ID, (ResourceSet) Db.Get().TechItems, (string) Strings.Get($"STRINGS.RESEARCH.OTHER_TECH_ITEMS.{id.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.RESEARCH.OTHER_TECH_ITEMS.{id.ToUpper()}.DESC"), (Func<string, bool, Sprite>) ((a, b) => Def.GetUISprite((object) Assets.GetPrefab((Tag) ID)).first), craftTechUnlockID, dlcIDs, (string[]) null, false);
    if (addTechItem && !craftTechUnlockID.IsNullOrWhiteSpace())
      Db.Get().Techs.Get(craftTechUnlockID).AddUnlockedItemIDs(techItem.Id);
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(ID, name, desc, 25f, true, Assets.GetAnim((HashedString) animFile), animStateName, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.45f, true, SORTORDER.ARTIFACTS, element, new List<Tag>()
    {
      GameTags.BionicUpgrade,
      GameTags.MiscPickupable,
      GameTags.NotRoomAssignable
    });
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    DecorProvider decorProvider = looseEntity.AddOrGet<DecorProvider>();
    decorProvider.SetValues(TUNING.DECOR.NONE);
    decorProvider.overrideName = looseEntity.GetProperName();
    looseEntity.AddOrGet<BionicUpgradeComponent>().slotID = Db.Get().AssignableSlots.BionicUpgrade.Id;
    looseEntity.AddOrGet<KSelectable>();
    KPrefabID component = looseEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.PedestalDisplayable);
    component.requiredDlcIds = dlcIDs;
    component.SetUnityEditorConfigOverride(nameof (BionicUpgradeComponentConfig));
    string id1 = (string) null;
    if (isStartingBooster)
    {
      id1 = "StartWith" + id;
      DUPLICANTSTATS.BIONICUPGRADETRAITS.Add(new DUPLICANTSTATS.TraitVal()
      {
        id = id1,
        requiredDlcIds = DlcManager.DLC3
      });
      TraitUtil.CreateBionicUpgradeTrait(id1, sm_description)();
    }
    Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData> upgradesData = BionicUpgradeComponentConfig.UpgradesData;
    Tag prefabTag = component.PrefabTag;
    double cost = (double) wattageCost;
    Func<StateMachine.Instance, StateMachine.Instance> func = stateMachine;
    string str = sm_description;
    string animStateName1 = animStateName;
    BionicUpgradeComponentConfig.BoosterType boosterType = booster;
    string relatedTrait = id1;
    int booster1 = (int) boosterType;
    Func<StateMachine.Instance, StateMachine.Instance> smi = func;
    string stateMachineDescription = str;
    int num = isCarePackage ? 1 : 0;
    string[] array = ((IEnumerable<SkillPerk>) skillPerks).Select<SkillPerk, string>((Func<SkillPerk, string>) (perk => perk.Id)).ToArray<string>();
    BionicUpgradeComponentConfig.BionicUpgradeData bionicUpgradeData = new BionicUpgradeComponentConfig.BionicUpgradeData((float) cost, animStateName1, relatedTrait, (BionicUpgradeComponentConfig.BoosterType) booster1, smi, stateMachineDescription, num != 0, array);
    upgradesData.Add(prefabTag, bionicUpgradeData);
    if (!BionicUpgradeComponentConfig.BASIC_BOOSTERS.Contains(ID))
    {
      ComplexRecipe.RecipeElement[] recipeElementArray1;
      if (recipeInputOverride == null)
        recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
        {
          new ComplexRecipe.RecipeElement((Tag) "PowerStationTools", 8f)
        };
      else
        recipeElementArray1 = recipeInputOverride;
      ComplexRecipe.RecipeElement[] recipeElementArray2 = recipeElementArray1;
      LocString format = booster != BionicUpgradeComponentConfig.BoosterType.Overclocked ? STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.BIONIC_COMPONENT_RECIPE_DESC : STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.BIONIC_OVERCLOCK_RECIPE_DESC;
      ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(ID.ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
      };
      ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2, (IList<ComplexRecipe.RecipeElement>) recipeElementArray3), recipeElementArray2, recipeElementArray3)
      {
        time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME,
        description = $"{string.Format((string) format, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, (object) name)}\n\n{BionicUpgradeComponentConfig.UpgradesData[(Tag) ID].stateMachineDescription}",
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "AdvancedCraftingTable"
        },
        requiredTech = craftTechUnlockID,
        sortOrder = 3,
        runTimeDescription = (Func<string>) (() => BionicUpgradeComponentConfig.GetColonyBoosterAssignmentString(ID))
      };
    }
    else
    {
      ComplexRecipe.RecipeElement[] recipeElementArray4;
      if (recipeInputOverride == null)
        recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
        {
          new ComplexRecipe.RecipeElement((Tag) "PowerStationTools", booster == BionicUpgradeComponentConfig.BoosterType.Basic ? 2f : 4f, true)
        };
      else
        recipeElementArray4 = recipeInputOverride;
      ComplexRecipe.RecipeElement[] recipeElementArray5 = recipeElementArray4;
      ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement((Tag) ID, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
      };
      ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6, dlcIDs)
      {
        time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 2f,
        description = $"{string.Format((string) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.BIONIC_COMPONENT_RECIPE_DESC, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, (object) name)}\n\n{BionicUpgradeComponentConfig.UpgradesData[(Tag) ID].stateMachineDescription}",
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "CraftingTable"
        },
        sortOrder = 1,
        runTimeDescription = (Func<string>) (() => BionicUpgradeComponentConfig.GetColonyBoosterAssignmentString(ID))
      };
    }
    return looseEntity;
  }

  public static string GetColonyBoosterAssignmentString(string boosterID)
  {
    int num = 0;
    foreach (MinionIdentity worldItem in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId))
    {
      if (worldItem.HasTag(GameTags.Minions.Models.Bionic))
      {
        BionicUpgradesMonitor.Instance smi = worldItem.GetSMI<BionicUpgradesMonitor.Instance>();
        if (smi != null && smi.upgradeComponentSlots != null)
        {
          foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
          {
            if (upgradeComponentSlot.HasUpgradeComponentAssigned && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == (Tag) boosterID)
            {
              ++num;
              break;
            }
          }
        }
      }
    }
    return num == 0 ? string.Format((string) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.COLONY_HAS_BOOSTER_ASSIGNED_NONE) : string.Format((string) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.COLONY_HAS_BOOSTER_ASSIGNED_COUNT, (object) num);
  }

  public enum BoosterType
  {
    Basic,
    Intermediate,
    Overclocked,
    Sleep,
    Space,
    Special,
  }

  public class BionicUpgradeData
  {
    private const string DEFAULT_ANIM_STATE_NAME = "object";
    public string stateMachineDescription;
    public string animStateName = "object";
    public string[] skillPerks = new string[0];

    public float WattageCost { private set; get; }

    public Func<StateMachine.Instance, StateMachine.Instance> stateMachine { private set; get; }

    public string uiAnimName
    {
      get => !(this.animStateName == "object") ? "ui_" + this.animStateName : "ui";
    }

    public string relatedTrait { private set; get; }

    public BionicUpgradeComponentConfig.BoosterType Booster { private set; get; }

    public bool isCarePackage { private set; get; }

    public BionicUpgradeData(
      float cost,
      string animStateName,
      string relatedTrait,
      BionicUpgradeComponentConfig.BoosterType booster,
      Func<StateMachine.Instance, StateMachine.Instance> smi,
      string stateMachineDescription,
      bool isCarePackage,
      string[] skillPerkIds = null)
    {
      this.WattageCost = cost;
      this.stateMachine = smi;
      this.stateMachineDescription = stateMachineDescription;
      this.animStateName = animStateName;
      this.relatedTrait = relatedTrait;
      this.Booster = booster;
      this.isCarePackage = isCarePackage;
      this.skillPerks = skillPerkIds;
    }
  }
}
