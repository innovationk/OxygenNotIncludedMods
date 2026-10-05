// Decompiled with JetBrains decompiler
// Type: WeaponExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class WeaponExtensions
{
  public static Weapon AddWeapon(
    this GameObject prefab,
    float base_damage_min,
    float base_damage_max,
    AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard,
    AttackProperties.TargetType targetType = AttackProperties.TargetType.Single,
    int maxHits = 1,
    float aoeRadius = 0.0f)
  {
    Weapon weapon = prefab.AddOrGet<Weapon>();
    weapon.Configure(base_damage_min, base_damage_max, attackType, targetType, maxHits, aoeRadius);
    return weapon;
  }
}
