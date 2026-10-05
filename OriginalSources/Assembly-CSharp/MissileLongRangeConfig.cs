// Decompiled with JetBrains decompiler
// Type: MissileLongRangeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MissileLongRangeConfig : IEntityConfig
{
  public const string ID = "MissileLongRange";
  public const float MASS_PER_MISSILE = 200f;
  public const int DAMAGE_PER_MISSILE = 10;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("MissileLongRange", (string) ITEMS.MISSILE_LONGRANGE.NAME, (string) ITEMS.MISSILE_LONGRANGE.DESC, 200f, true, Assets.GetAnim((HashedString) "longrange_missile_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, isPickupable: true, element: SimHashes.Iron, additionalTags: new List<Tag>());
    looseEntity.AddTag(GameTags.LongRangeMissile);
    looseEntity.AddTag(GameTags.IndustrialProduct);
    looseEntity.AddTag(GameTags.PedestalDisplayable);
    MissileLongRangeProjectile.Def def = looseEntity.AddOrGetDef<MissileLongRangeProjectile.Def>();
    def.starmapOverrideSymbol = "payload";
    def.missileName = "STRINGS.ITEMS.MISSILE_LONGRANGE.NAME";
    def.missileDesc = "STRINGS.ITEMS.MISSILE_LONGRANGE.DESC";
    looseEntity.AddOrGet<EntitySplitter>().maxStackSize = 200f;
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public class DamageEventPayload
  {
    public int damage;
    public static MissileLongRangeConfig.DamageEventPayload sharedInstance = new MissileLongRangeConfig.DamageEventPayload();

    public DamageEventPayload(int damage = 10) => this.damage = damage;
  }
}
