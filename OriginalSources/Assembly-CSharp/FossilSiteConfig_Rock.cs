// Decompiled with JetBrains decompiler
// Type: FossilSiteConfig_Rock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FossilSiteConfig_Rock : IEntityConfig
{
  public static readonly HashedString FossilQuestCriteriaID = (HashedString) "LostRockFossil";
  public const string ID = "FossilRock";

  public GameObject CreatePrefab()
  {
    string name = (string) CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.NAME;
    string desc = (string) CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.DESC;
    EffectorValues tieR4 = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    KAnimFile anim = Assets.GetAnim((HashedString) "fossil_rock_kanim");
    EffectorValues decor = tieR4;
    EffectorValues noise = tieR3;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("FossilRock", name, desc, 4000f, anim, "object", Grid.SceneLayer.BuildingBack, 2, 2, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Fossil);
    component.Temperature = 315f;
    placedEntity.AddOrGet<Operational>();
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<Demolishable>().allowDemolition = false;
    placedEntity.AddOrGetDef<MinorFossilDigSite.Def>().fossilQuestCriteriaID = FossilSiteConfig_Rock.FossilQuestCriteriaID;
    placedEntity.AddOrGetDef<FossilHuntInitializer.Def>();
    placedEntity.AddOrGet<MinorDigSiteWorkable>();
    placedEntity.AddOrGet<Prioritizable>();
    Prioritizable.AddRef(placedEntity);
    placedEntity.AddOrGet<LoopingSounds>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<EntombVulnerable>().SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
