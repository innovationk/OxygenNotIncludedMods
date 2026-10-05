// Decompiled with JetBrains decompiler
// Type: PioneerLanderConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PioneerLanderConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "PioneerLander";
  public const string PREVIEW_ID = "PioneerLander_Preview";
  public const float MASS = 400f;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "rocket_pioneer_cargo_lander_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PioneerLander", name, desc, 400f, anim, "grounded", Grid.SceneLayer.Building, 3, 3, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.RoomProberBuilding
    });
    placedEntity.AddOrGetDef<CargoLander.Def>().previewTag = "PioneerLander_Preview".ToTag();
    CargoDropperMinion.Def def = placedEntity.AddOrGetDef<CargoDropperMinion.Def>();
    def.kAnimName = "anim_interacts_pioneer_cargo_lander_kanim";
    def.animName = "enter";
    placedEntity.AddOrGet<MinionStorage>();
    placedEntity.AddOrGet<Prioritizable>();
    Prioritizable.AddRef(placedEntity);
    placedEntity.AddOrGet<Operational>();
    placedEntity.AddOrGet<Deconstructable>().audioSize = "large";
    placedEntity.AddOrGet<Storable>();
    Placeable placeable = placedEntity.AddOrGet<Placeable>();
    placeable.kAnimName = "rocket_pioneer_cargo_lander_kanim";
    placeable.animName = "place";
    placeable.placementRules = new List<Placeable.PlacementRules>()
    {
      Placeable.PlacementRules.OnFoundation,
      Placeable.PlacementRules.VisibleToSpace,
      Placeable.PlacementRules.RestrictToWorld
    };
    placeable.checkRootCellOnly = true;
    EntityTemplates.CreateAndRegisterPreview("PioneerLander_Preview", Assets.GetAnim((HashedString) "rocket_pioneer_cargo_lander_kanim"), "place", ObjectLayer.Building, 3, 3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    OccupyArea component = inst.GetComponent<OccupyArea>();
    component.ApplyToCells = false;
    component.objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
