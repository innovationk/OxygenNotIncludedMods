// Decompiled with JetBrains decompiler
// Type: Hit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class Hit
{
  private AttackProperties properties;
  private GameObject target;

  public Hit(AttackProperties properties, GameObject target)
  {
    this.properties = properties;
    this.target = target;
    this.DeliverHit();
  }

  private float rollDamage()
  {
    return (float) Mathf.RoundToInt(Random.Range(this.properties.base_damage_min, this.properties.base_damage_max));
  }

  private void DeliverHit()
  {
    Health component1 = this.target.GetComponent<Health>();
    if (!(bool) (Object) component1)
      return;
    this.target.Trigger(-787691065, (object) this.properties.attacker.GetComponent<FactionAlignment>());
    float amount = this.rollDamage() * (1f + this.target.GetComponent<AttackableBase>().GetDamageMultiplier());
    component1.Damage(amount);
    if (this.properties.effects == null)
      return;
    Effects component2 = this.target.GetComponent<Effects>();
    if (!(bool) (Object) component2)
      return;
    foreach (AttackEffect effect in this.properties.effects)
    {
      if ((double) Random.Range(0.0f, 100f) < (double) effect.effectProbability * 100.0)
        component2.Add(effect.effectID, true);
    }
  }
}
