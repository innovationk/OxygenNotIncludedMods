// Decompiled with JetBrains decompiler
// Type: AttackProperties
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class AttackProperties
{
  public Weapon attacker;
  public AttackProperties.DamageType damageType;
  public AttackProperties.TargetType targetType;
  public float base_damage_min;
  public float base_damage_max;
  public int maxHits;
  public float aoe_radius = 2f;
  public List<AttackEffect> effects;

  public enum DamageType
  {
    Standard,
  }

  public enum TargetType
  {
    Single,
    AreaOfEffect,
  }
}
