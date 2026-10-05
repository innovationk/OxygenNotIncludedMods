// Decompiled with JetBrains decompiler
// Type: MoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MoleConfig : IEntityConfig
{
  public const string ID = "Mole";
  public const string BASE_TRAIT_ID = "MoleBaseTrait";
  public const string EGG_ID = "MoleEgg";
  private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;
  private static float CALORIES_PER_KG_OF_DIRT = 1000f;
  public static int EGG_SORT_ORDER = 800;

  public static GameObject CreateMole(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby = false)
  {
    GameObject mole = BaseMoleConfig.BaseMole(id, name, (string) STRINGS.CREATURES.SPECIES.MOLE.DESC, "MoleBaseTrait", anim_file, is_baby, 173.15f, 673.15f, 73.1499939f, 773.15f);
    mole.AddTag(GameTags.Creatures.Digger);
    EntityTemplates.ExtendEntityToWildCreature(mole, MoleTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("MoleBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>()
    {
      SimHashes.Regolith.CreateTag(),
      SimHashes.Dirt.CreateTag(),
      SimHashes.IronOre.CreateTag()
    }, MoleConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
    CreatureCalorieMonitor.Def def = mole.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minConsumedCaloriesBeforePooping = MoleConfig.MIN_POOP_SIZE_IN_CALORIES;
    mole.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    mole.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
    mole.AddOrGet<LoopingSounds>();
    foreach (HashedString symbol in MoleTuning.GINGER_SYMBOL_NAMES)
      mole.GetComponent<KAnimControllerBase>().SetSymbolVisiblity((KAnimHashedString) symbol, false);
    mole.AddTag(GameTags.OriginalCreature);
    return mole;
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(MoleConfig.CreateMole("Mole", (string) STRINGS.CREATURES.SPECIES.MOLE.NAME, (string) STRINGS.CREATURES.SPECIES.MOLE.DESC, "driller_kanim"), this as IHasDlcRestrictions, "MoleEgg", (string) STRINGS.CREATURES.SPECIES.MOLE.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.MOLE.DESC, "egg_driller_kanim", MoleTuning.EGG_MASS, "MoleBaby", 60.0000038f, 20f, MoleTuning.EGG_CHANCES_BASE, MoleConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => MoleConfig.SetSpawnNavType(inst);

  public static void SetSpawnNavType(GameObject inst)
  {
    int cell = Grid.PosToCell(inst);
    Navigator component1 = inst.GetComponent<Navigator>();
    Pickupable component2 = inst.GetComponent<Pickupable>();
    if (!((Object) component1 != (Object) null) || !((Object) component2 == (Object) null) && !((Object) component2.storage == (Object) null))
      return;
    if (Grid.IsSolidCell(cell))
    {
      component1.SetCurrentNavType(NavType.Solid);
      inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
    }
    else
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
  }
}
