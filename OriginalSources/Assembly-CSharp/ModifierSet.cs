// Decompiled with JetBrains decompiler
// Type: ModifierSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ModifierSet : ScriptableObject
{
  public TextAsset modifiersFile;
  public ModifierSet.ModifierInfos modifierInfos;
  public ModifierSet.TraitSet traits;
  public ResourceSet<Effect> effects;
  public ModifierSet.TraitGroupSet traitGroups;
  public FertilityModifiers FertilityModifiers;
  public MooSongModifiers MooSongModifiers;
  public Database.Attributes Attributes;
  public BuildingAttributes BuildingAttributes;
  public CritterAttributes CritterAttributes;
  public PlantAttributes PlantAttributes;
  public Database.Amounts Amounts;
  public Database.AttributeConverters AttributeConverters;
  public ResourceSet Root;
  public List<Resource> ResourceTable;

  public virtual void Initialize()
  {
    this.ResourceTable = new List<Resource>();
    this.Root = (ResourceSet) new ResourceSet<Resource>("Root", (ResourceSet) null);
    this.modifierInfos = new ModifierSet.ModifierInfos();
    this.modifierInfos.Load(this.modifiersFile);
    this.Attributes = new Database.Attributes(this.Root);
    this.BuildingAttributes = new BuildingAttributes(this.Root);
    this.CritterAttributes = new CritterAttributes(this.Root);
    this.PlantAttributes = new PlantAttributes(this.Root);
    this.effects = new ResourceSet<Effect>("Effects", this.Root);
    this.traits = new ModifierSet.TraitSet();
    this.traitGroups = new ModifierSet.TraitGroupSet();
    this.FertilityModifiers = new FertilityModifiers();
    this.MooSongModifiers = new MooSongModifiers();
    this.Amounts = new Database.Amounts();
    this.Amounts.Load();
    this.AttributeConverters = new Database.AttributeConverters();
    this.LoadEffects();
    this.LoadFertilityModifiers();
    this.LoadMooSongsModifiers();
  }

  public static float ConvertValue(float value, Units units)
  {
    return Units.PerDay == units ? value * (1f / 600f) : value;
  }

  private void LoadEffects()
  {
    foreach (ModifierSet.ModifierInfo modifierInfo1 in (ResourceLoader<ModifierSet.ModifierInfo>) this.modifierInfos)
    {
      if (!this.effects.Exists(modifierInfo1.Id) && (modifierInfo1.Type == "Effect" || modifierInfo1.Type == "Base" || modifierInfo1.Type == "Need"))
      {
        string str = (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{modifierInfo1.Id.ToUpper()}.NAME");
        string description = (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{modifierInfo1.Id.ToUpper()}.TOOLTIP");
        Effect resource = new Effect(modifierInfo1.Id, str, description, modifierInfo1.Duration * 600f, modifierInfo1.ShowInUI && modifierInfo1.Type != "Need", modifierInfo1.TriggerFloatingText, modifierInfo1.IsBad, modifierInfo1.EmoteAnim, modifierInfo1.EmoteCooldown, modifierInfo1.StompGroup, modifierInfo1.CustomIcon);
        resource.stompPriority = modifierInfo1.StompPriority;
        foreach (ModifierSet.ModifierInfo modifierInfo2 in (ResourceLoader<ModifierSet.ModifierInfo>) this.modifierInfos)
        {
          if (modifierInfo2.Id == modifierInfo1.Id)
            resource.Add(new AttributeModifier(modifierInfo2.Attribute, ModifierSet.ConvertValue(modifierInfo2.Value, modifierInfo2.Units), str, modifierInfo2.Multiplier));
        }
        this.effects.Add(resource);
      }
    }
    Reactable.ReactablePrecondition precon = (Reactable.ReactablePrecondition) ((go, n) =>
    {
      int cell = Grid.PosToCell(go);
      return Grid.IsValidCell(cell) && Grid.IsGas(cell);
    });
    this.effects.Get("WetFeet").AddEmotePrecondition(precon);
    this.effects.Get("SoakingWet").AddEmotePrecondition(precon);
    Effect resource1 = new Effect("PassedOutSleep", (string) DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, (string) DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.TOOLTIP, 0.0f, true, true, true, (Emote) null, 0.0f, (string) null, true, "status_item_exhausted");
    resource1.Add(new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.6666667f, (string) DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME));
    resource1.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.0333333351f, (string) DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME));
    this.effects.Add(resource1);
    this.effects.Add(new Effect("WarmTouch", (string) DUPLICANTS.MODIFIERS.WARMTOUCH.NAME, (string) DUPLICANTS.MODIFIERS.WARMTOUCH.TOOLTIP, 120f, new string[1]
    {
      "WetFeet"
    }, true, true, false, (Emote) null, 0.0f, (string) null, false));
    this.effects.Add(new Effect("WarmTouchFood", (string) DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.NAME, (string) DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.TOOLTIP, 600f, new string[1]
    {
      "WetFeet"
    }, true, true, false, (Emote) null, 0.0f, (string) null, false));
    this.effects.Add(new Effect("RefreshingTouch", (string) DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.NAME, (string) DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.TOOLTIP, 120f, true, true, false));
    Effect resource2 = new Effect("GunkSick", (string) DUPLICANTS.MODIFIERS.GUNKSICK.NAME, (string) DUPLICANTS.MODIFIERS.GUNKSICK.TOOLTIP, 0.0f, true, true, true);
    resource2.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0333333351f, (string) DUPLICANTS.MODIFIERS.GUNKSICK.NAME));
    this.effects.Add(resource2);
    Effect resource3 = new Effect("ExpellingGunk", (string) DUPLICANTS.MODIFIERS.EXPELLINGGUNK.NAME, (string) DUPLICANTS.MODIFIERS.EXPELLINGGUNK.TOOLTIP, 0.0f, true, true, true);
    resource3.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0833333358f, (string) DUPLICANTS.MODIFIERS.GUNKSICK.NAME));
    this.effects.Add(resource3);
    Effect resource4 = new Effect("GunkHungover", (string) DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME, (string) DUPLICANTS.MODIFIERS.GUNKHUNGOVER.TOOLTIP, 600f, true, false, true);
    resource4.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0333333351f, (string) DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME));
    this.effects.Add(resource4);
    Effect resource5 = new Effect("NoLubricationMinor", (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.TOOLTIP, 0.0f, true, true, true);
    resource5.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -4f, (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME));
    resource5.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.025f, (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME));
    this.effects.Add(resource5);
    Effect resource6 = new Effect("NoLubricationMajor", (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.NAME, (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.TOOLTIP, 0.0f, true, true, true);
    resource6.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -8f, (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.NAME));
    resource6.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.05f, (string) DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME));
    this.effects.Add(resource6);
    Effect resource7 = new Effect("BionicOffline", (string) DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME, (string) DUPLICANTS.MODIFIERS.BIONICOFFLINE.TOOLTIP, 0.0f, false, true, true);
    resource7.Add(new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, 0.0f, (string) DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME));
    this.effects.Add(resource7);
    Effect resource8 = new Effect("BionicBedTimeEffect", (string) DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.NAME, (string) DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.TOOLTIP, 0.0f, false, false, false);
    resource8.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.0333333351f, (string) DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.NAME));
    this.effects.Add(resource8);
    Effect resource9 = new Effect("BionicWaterStress", (string) DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.NAME, (string) DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.TOOLTIP, 0.0f, true, true, true);
    resource9.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.333333343f, (string) DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.NAME));
    this.effects.Add(resource9);
    this.effects.Add(new Effect("RecentlySlippedTracker", (string) DUPLICANTS.MODIFIERS.SLIPPED.NAME, (string) DUPLICANTS.MODIFIERS.SLIPPED.TOOLTIP, 100f, false, false, true));
    Effect resource10 = new Effect("DuplicantDrankInk", (string) DUPLICANTS.MODIFIERS.DUPLICANTDRANKINK.NAME, (string) DUPLICANTS.MODIFIERS.DUPLICANTDRANKINK.TOOLTIP, 600f, true, true, false);
    resource10.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.0166666675f, (string) DUPLICANTS.MODIFIERS.DUPLICANTDRANKINK.NAME));
    resource10.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 1f, (string) DUPLICANTS.MODIFIERS.DUPLICANTDRANKINK.NAME));
    if (DlcManager.IsExpansion1Active())
      resource10.Add(new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, 0.2f, (string) DUPLICANTS.MODIFIERS.DUPLICANTDRANKINK.NAME));
    this.effects.Add(resource10);
    foreach (Effect resource11 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Values)
      this.effects.Add(resource11);
    this.CreateRoomEffects();
    this.CreateCritteEffects();
  }

  private void CreateRoomEffects()
  {
  }

  private void CreateMosquitoEffects()
  {
    Effect resource1 = new Effect("MosquitoFed", (string) STRINGS.CREATURES.MODIFIERS.MOSQUITO_FED.NAME, (string) STRINGS.CREATURES.MODIFIERS.MOSQUITO_FED.TOOLTIP, 600f, true, false, false);
    float num = (float) (0.89999997615814209 / 0.40000000596046448 - 1.0);
    resource1.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, num, resource1.Name, true));
    this.effects.Add(resource1);
    Effect resource2 = new Effect("DupeMosquitoBite", (string) STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME, (string) STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.TOOLTIP, 600f, true, true, true);
    resource2.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0166666675f, (string) STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME));
    resource2.Add(new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 5f, (string) STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME));
    resource2.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -1f, (string) STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE.NAME));
    this.effects.Add(resource2);
    this.effects.Add(new Effect("DupeMosquitoBiteSuppressed", (string) STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE_SUPPRESSED.NAME, (string) STRINGS.CREATURES.MODIFIERS.DUPE_MOSQUITO_BITE_SUPPRESSED.TOOLTIP, 600f, false, false, false));
    Effect resource3 = new Effect("CritterMosquitoBite", (string) STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE.NAME, (string) STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE.TOOLTIP, 300f, true, true, true);
    resource3.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, (string) STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE.NAME));
    this.effects.Add(resource3);
    this.effects.Add(new Effect("CritterMosquitoBiteSuppressed", (string) STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE_SUPPRESSED.NAME, (string) STRINGS.CREATURES.MODIFIERS.CRITTER_MOSQUITO_BITE_SUPPRESSED.TOOLTIP, 300f, false, false, false));
  }

  public void CreateCritteEffects()
  {
    Effect resource1 = new Effect("Ranched", (string) STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, (string) STRINGS.CREATURES.MODIFIERS.RANCHED.TOOLTIP, 600f, true, true, false);
    resource1.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, (string) STRINGS.CREATURES.MODIFIERS.RANCHED.NAME));
    resource1.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.09166667f, (string) STRINGS.CREATURES.MODIFIERS.RANCHED.NAME));
    this.effects.Add(resource1);
    Effect resource2 = new Effect("HadMilk", (string) STRINGS.CREATURES.MODIFIERS.HADMILK.NAME, (string) STRINGS.CREATURES.MODIFIERS.HADMILK.TOOLTIP, 600f, true, true, false);
    resource2.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, (string) STRINGS.CREATURES.MODIFIERS.HADMILK.NAME));
    this.effects.Add(resource2);
    Effect resource3 = new Effect("HadInk", (string) STRINGS.CREATURES.MODIFIERS.HADINK.NAME, (string) STRINGS.CREATURES.MODIFIERS.HADINK.TOOLTIP, 600f, true, true, false);
    resource3.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 3f, (string) STRINGS.CREATURES.MODIFIERS.HADINK.NAME));
    this.effects.Add(resource3);
    Effect resource4 = new Effect("AteWellPreparedFishFood", (string) STRINGS.CREATURES.MODIFIERS.FISHFOOD.NAME, (string) STRINGS.CREATURES.MODIFIERS.FISHFOOD.TOOLTIP, 600f, true, true, false);
    resource4.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 2f, (string) STRINGS.CREATURES.MODIFIERS.FISHFOOD.NAME));
    resource4.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.0166666675f, (string) STRINGS.CREATURES.MODIFIERS.FISHFOOD.NAME));
    this.effects.Add(resource4);
    Effect resource5 = new Effect("AteWellPreparedJawboFood", (string) STRINGS.CREATURES.MODIFIERS.JAWBOFOOD.NAME, (string) STRINGS.CREATURES.MODIFIERS.JAWBOFOOD.TOOLTIP, 600f, true, true, false);
    resource5.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 2f, (string) STRINGS.CREATURES.MODIFIERS.JAWBOFOOD.NAME));
    resource5.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.0166666675f, (string) STRINGS.CREATURES.MODIFIERS.JAWBOFOOD.NAME));
    this.effects.Add(resource5);
    Effect resource6 = new Effect("EggSong", (string) STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, (string) STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP, 600f, true, false, false);
    resource6.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 4f, (string) STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, true));
    this.effects.Add(resource6);
    Effect resource7 = new Effect("EggHug", (string) STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, (string) STRINGS.CREATURES.MODIFIERS.EGGHUG.TOOLTIP, 600f, true, true, false);
    resource7.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 1f, (string) STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, true));
    this.effects.Add(resource7);
    this.effects.Add(new Effect("HuggingFrenzy", (string) STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.NAME, (string) STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.TOOLTIP, 600f, true, false, false));
    Effect resource8 = new Effect("DivergentCropTended", (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.TOOLTIP, 600f, true, true, false);
    resource8.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, true));
    resource8.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.deltaAttribute.Id, 0.05f, (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, true));
    this.effects.Add(resource8);
    Effect resource9 = new Effect("DivergentCropTendedWorm", (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.TOOLTIP, 600f, true, true, false);
    resource9.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.5f, (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, true));
    resource9.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.deltaAttribute.Id, 0.5f, (string) STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, true));
    this.effects.Add(resource9);
    Effect resource10 = new Effect("MooWellFed", (string) STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.MOOWELLFED.TOOLTIP, 1f, true, true, false);
    resource10.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, (string) STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME));
    resource10.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, (string) STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME));
    this.effects.Add(resource10);
    Effect resource11 = new Effect("HuskyMooFed", (string) STRINGS.CREATURES.MODIFIERS.HUSKYMOOFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.HUSKYMOOFED.TOOLTIP, 1f, true, true, false);
    resource11.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, (string) STRINGS.CREATURES.MODIFIERS.HUSKYMOOFED.NAME));
    this.effects.Add(resource11);
    Effect resource12 = new Effect("HuskyMooWellFed", (string) STRINGS.CREATURES.MODIFIERS.HUSKYMOOWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.HUSKYMOOWELLFED.TOOLTIP, 1f, true, true, false);
    resource12.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, (Func<string>) (() => GameUtil.SafeStringFormat((string) STRINGS.CREATURES.STATS.MILKPRODUCTION.DISPLAYED_NAME, (object) UI.StripLinkFormatting(ElementLoader.FindElementByHash(DieselMooConfig.MILK_ELEMENT).name))), (Func<string>) (() => (string) STRINGS.CREATURES.MODIFIERS.HUSKYMOOWELLFED.NAME)));
    this.effects.Add(resource12);
    Effect resource13 = new Effect("WoodDeerWellFed", (string) STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.DEERWELLFED.TOOLTIP, 1f, true, true, false);
    resource13.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, (float) (100.0 / ((double) WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES * 600.0)), (Func<string>) (() => ModifierSet.GetScaleGrowthName("WoodDeer")), (Func<string>) (() => (string) STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME)));
    this.effects.Add(resource13);
    Effect resource14 = new Effect("SquidWellFed", (string) STRINGS.CREATURES.MODIFIERS.SQUIDWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.SQUIDWELLFED.TOOLTIP, 1f, true, true, false);
    resource14.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, SquidTuning.INK_PRODUCTION_PERCENTAGE_PER_SECOND, (string) STRINGS.CREATURES.MODIFIERS.SQUIDWELLFED.NAME));
    this.effects.Add(resource14);
    Effect resource15 = new Effect("GlassDeerWellFed", (string) STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.DEERWELLFED.TOOLTIP, 1f, true, true, false);
    resource15.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 0.027777778f, (Func<string>) (() => ModifierSet.GetScaleGrowthName("GlassDeer")), (Func<string>) (() => (string) STRINGS.CREATURES.MODIFIERS.DEERWELLFED.NAME)));
    this.effects.Add(resource15);
    Effect resource16 = new Effect("IceBellyWellFed", (string) STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.TOOLTIP, 1f, true, true, false);
    resource16.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, (float) (100.0 / ((double) IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600.0)), (Func<string>) (() => ModifierSet.GetScaleGrowthName("IceBelly")), (Func<string>) (() => (string) STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME)));
    this.effects.Add(resource16);
    Effect resource17 = new Effect("GoldBellyWellFed", (string) STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.TOOLTIP, 1f, true, true, false);
    resource17.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 0.0166666675f, (Func<string>) (() => ModifierSet.GetScaleGrowthName("GoldBelly")), (Func<string>) (() => (string) STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME)));
    this.effects.Add(resource17);
    Effect resource18 = new Effect("SeaTurtleWellFed", (string) STRINGS.CREATURES.MODIFIERS.SEATURTLEWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.SEATURTLEWELLFED.TOOLTIP, 1f, true, true, false);
    resource18.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, (float) (100.0 / ((double) SeaTurtleTuning.SCALE_GROWTH_TIME_IN_CYCLES * 600.0)), (Func<string>) (() => ModifierSet.GetScaleGrowthName("SeaTurtle")), (Func<string>) (() => (string) STRINGS.CREATURES.MODIFIERS.SEATURTLEWELLFED.NAME)));
    this.effects.Add(resource18);
    Effect resource19 = new Effect("ButterflyPollinated", (string) STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.NAME, (string) STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.TOOLTIP, 600f, true, true, false);
    resource19.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.25f, (string) STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.NAME, true));
    resource19.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.deltaAttribute.Id, 0.25f, (string) STRINGS.CREATURES.MODIFIERS.BUTTERFLYPOLLINATED.NAME, true));
    this.effects.Add(resource19);
    this.effects.Add(new Effect(PollinationMonitor.INITIALLY_POLLINATED_EFFECT, (string) STRINGS.CREATURES.MODIFIERS.INITIALLYPOLLINATED.NAME, (string) STRINGS.CREATURES.MODIFIERS.INITIALLYPOLLINATED.TOOLTIP, 600f, false, false, false));
    Effect resource20 = new Effect("RaptorWellFed", (string) STRINGS.CREATURES.MODIFIERS.RAPTORWELLFED.NAME, (string) STRINGS.CREATURES.MODIFIERS.RAPTORWELLFED.TOOLTIP, 1f, true, true, false);
    resource20.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, (float) (100.0 / ((double) RaptorConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600.0)), (Func<string>) (() => ModifierSet.GetScaleGrowthName("Raptor")), (Func<string>) (() => (string) STRINGS.CREATURES.MODIFIERS.RAPTORWELLFED.NAME)));
    this.effects.Add(resource20);
    this.effects.Add(new Effect("PredatorFailedHunt", (string) STRINGS.CREATURES.MODIFIERS.HUNT_FAILED.NAME, (string) STRINGS.CREATURES.MODIFIERS.HUNT_FAILED.TOOLTIP, 45f, true, false, true)
    {
      tag = new Tag?(GameTags.Creatures.SuppressedDiet)
    });
    this.effects.Add(new Effect("PreyEvadedHunt", (string) STRINGS.CREATURES.MODIFIERS.EVADED_HUNT.NAME, (string) STRINGS.CREATURES.MODIFIERS.EVADED_HUNT.TOOLTIP, 10f, true, false, false));
    this.CreateMosquitoEffects();
    this.effects.Add(new Effect("RecentlyProducedLubricant", (string) STRINGS.CREATURES.MODIFIERS.RECENTLYPRODUCEDLUBRICANT.NAME, (string) STRINGS.CREATURES.MODIFIERS.RECENTLYPRODUCEDLUBRICANT.TOOLTIP, 10f, false, false, false));
  }

  private static string GetScaleGrowthName(string prefabID)
  {
    LocString locString;
    return STRINGS.CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME().TryGetValue((Tag) prefabID, out locString) ? (string) locString : (string) STRINGS.CREATURES.STATS.SCALEGROWTH.SCALE;
  }

  public Trait CreateTrait(
    string id,
    string name,
    string description,
    string group_name,
    bool should_save,
    ChoreGroup[] disabled_chore_groups,
    bool positive_trait,
    bool is_valid_starter_trait)
  {
    return this.CreateTrait(id, name, description, group_name, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait, (string[]) null, (string[]) null);
  }

  public Trait CreateTrait(
    string id,
    string name,
    string description,
    string group_name,
    bool should_save,
    ChoreGroup[] disabled_chore_groups,
    bool positive_trait,
    bool is_valid_starter_trait,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
  {
    Trait trait = new Trait(id, name, description, 0.0f, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait, requiredDlcIds, forbiddenDlcIds);
    this.traits.Add(trait);
    if (group_name == "" || group_name == null)
      group_name = "Default";
    TraitGroup resource = this.traitGroups.TryGet(group_name);
    if (resource == null)
    {
      resource = new TraitGroup(group_name, group_name, group_name != "Default");
      this.traitGroups.Add(resource);
    }
    resource.Add(trait);
    return trait;
  }

  public FertilityModifier CreateFertilityModifier(
    string id,
    Tag targetTag,
    string name,
    string description,
    Func<string, string> tooltipCB,
    FertilityModifier.FertilityModFn applyFunction)
  {
    FertilityModifier resource = new FertilityModifier(id, targetTag, name, description, tooltipCB, applyFunction);
    this.FertilityModifiers.Add(resource);
    return resource;
  }

  public MooSongModifier CreateMooSongModifier(
    string id,
    Tag targetTag,
    string name,
    string description,
    Func<string, string> tooltipCB,
    MooSongModifier.MooSongModFn applyFunction)
  {
    MooSongModifier resource = new MooSongModifier(id, targetTag, name, description, tooltipCB, applyFunction);
    this.MooSongModifiers.Add(resource);
    return resource;
  }

  protected void LoadTraits()
  {
    TUNING.TRAITS.TRAIT_CREATORS.ForEach((Action<System.Action>) (action => action()));
  }

  protected void LoadFertilityModifiers()
  {
    TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.ForEach((Action<System.Action>) (action => action()));
  }

  protected void LoadMooSongsModifiers()
  {
    TUNING.CREATURES.MOO_SONG_MODIFIERS.MODIFIER_CREATORS.ForEach((Action<System.Action>) (action => action()));
  }

  public class ModifierInfo : Resource
  {
    public string Type;
    public string Attribute;
    public float Value;
    public Units Units;
    public bool Multiplier;
    public float Duration;
    public bool ShowInUI;
    public string StompGroup;
    public int StompPriority;
    public bool IsBad;
    public string CustomIcon;
    public bool TriggerFloatingText;
    public string EmoteAnim;
    public float EmoteCooldown;
  }

  [Serializable]
  public class ModifierInfos : ResourceLoader<ModifierSet.ModifierInfo>
  {
  }

  [Serializable]
  public class TraitSet : ResourceSet<Trait>
  {
  }

  [Serializable]
  public class TraitGroupSet : ResourceSet<TraitGroup>
  {
  }
}
