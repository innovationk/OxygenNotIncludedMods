// Decompiled with JetBrains decompiler
// Type: Database.Stories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class Stories : ResourceSet<Story>
{
  public Story MegaBrainTank;
  public Story CreatureManipulator;
  public Story LonelyMinion;
  public Story FossilHunt;
  public Story MorbRoverMaker;
  public Story HijackedHeadquarters;

  public Stories(ResourceSet parent)
    : base(nameof (Stories), parent)
  {
    this.MegaBrainTank = this.Add(new Story(nameof (MegaBrainTank), "storytraits/MegaBrainTank", 0, 1, 43, "storytraits/mega_brain_tank").SetKeepsake("keepsake_megabrain"));
    this.CreatureManipulator = this.Add(new Story(nameof (CreatureManipulator), "storytraits/CritterManipulator", 1, 2, 43, "storytraits/creature_manipulator_retrofit").SetKeepsake("keepsake_crittermanipulator"));
    this.LonelyMinion = this.Add(new Story(nameof (LonelyMinion), "storytraits/LonelyMinion", 2, 3, 44, "storytraits/lonelyminion_retrofit").SetKeepsake("keepsake_lonelyminion"));
    this.FossilHunt = this.Add(new Story(nameof (FossilHunt), "storytraits/FossilHunt", 3, 4, 44, "storytraits/fossil_hunt_retrofit").SetKeepsake("keepsake_fossilhunt"));
    this.MorbRoverMaker = this.Add(new Story(nameof (MorbRoverMaker), "storytraits/MorbRoverMaker", 4, 5, 50, "storytraits/morb_rover_maker_retrofit").SetKeepsake("keepsake_morbrovermaker"));
    this.HijackedHeadquarters = this.Add(new Story("HijackHeadquarters", "storytraits/HijackHeadquarters", 5, 6, 57, "storytraits/hijack_headquarters_retrofit").SetKeepsake("keepsake_hijackheadquarters"));
    this.resources.Sort();
  }

  public void AddStoryMod(Story mod)
  {
    mod.kleiUseOnlyCoordinateOrder = -1;
    this.Add(mod);
    this.resources.Sort();
  }

  public int GetHighestCoordinate()
  {
    int a = 0;
    foreach (Story resource in this.resources)
      a = Mathf.Max(a, resource.kleiUseOnlyCoordinateOrder);
    return a;
  }

  public WorldTrait GetStoryTrait(string id, bool assertMissingTrait = false)
  {
    Story story = this.resources.Find((Predicate<Story>) (x => x.Id == id));
    return story != null ? SettingsCache.GetCachedStoryTrait(story.worldgenStoryTraitKey, assertMissingTrait) : (WorldTrait) null;
  }

  public Story GetStoryFromStoryTrait(string storyTraitTemplate)
  {
    return this.resources.Find((Predicate<Story>) (x => x.worldgenStoryTraitKey == storyTraitTemplate));
  }

  public List<Story> GetStoriesSortedByCoordinateOrder()
  {
    List<Story> byCoordinateOrder = new List<Story>((IEnumerable<Story>) this.resources);
    byCoordinateOrder.Sort((Comparison<Story>) ((s1, s2) => s1.kleiUseOnlyCoordinateOrder.CompareTo(s2.kleiUseOnlyCoordinateOrder)));
    return byCoordinateOrder;
  }
}
