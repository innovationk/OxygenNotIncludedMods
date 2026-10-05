// Decompiled with JetBrains decompiler
// Type: Health
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Health")]
public class Health : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  public bool canBeIncapacitated;
  public HealthBar healthBar;
  public bool isCritter;
  private bool isCritterPrev;
  private Effects effects;
  private AmountInstance amountInstance;

  [Serialize]
  public Health.HealthState State { get; private set; }

  [Serialize]
  public Tag CauseOfIncapacitation { get; private set; }

  public AmountInstance GetAmountInstance => this.amountInstance;

  public float hitPoints
  {
    get => this.amountInstance.value;
    set => this.amountInstance.value = value;
  }

  public float maxHitPoints => this.amountInstance.GetMax();

  public float percent() => this.hitPoints / this.maxHitPoints;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Health.Add(this);
    this.amountInstance = Db.Get().Amounts.HitPoints.Lookup(this.gameObject);
    this.amountInstance.value = this.amountInstance.GetMax();
    this.amountInstance.OnDelta += new Action<float>(this.OnHealthChanged);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.State == Health.HealthState.Incapacitated || (double) this.hitPoints == 0.0)
    {
      if (this.canBeIncapacitated)
        this.Incapacitate(GameTags.HitPointsDepleted);
      else
        this.Kill();
    }
    if (this.State != Health.HealthState.Incapacitated && this.State != Health.HealthState.Dead)
      this.UpdateStatus();
    this.effects = this.GetComponent<Effects>();
    this.UpdateHealthBar();
    this.UpdateWoundEffects();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Health.Remove(this);
  }

  public void UpdateHealthBar()
  {
    if ((UnityEngine.Object) NameDisplayScreen.Instance == (UnityEngine.Object) null)
      return;
    bool flag = this.State == Health.HealthState.Dead || this.State == Health.HealthState.Incapacitated || (double) this.hitPoints >= (double) this.maxHitPoints || this.gameObject.HasTag((Tag) "HideHealthBar");
    NameDisplayScreen.Instance.SetHealthDisplay(this.gameObject, new Func<float>(this.percent), !flag);
  }

  private void OnRecover() => this.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);

  public void OnHealthChanged(float delta)
  {
    this.BoxingTrigger<float>(-1664904872, delta);
    if (this.State != Health.HealthState.Invincible)
    {
      if ((double) this.hitPoints == 0.0 && !this.IsDefeated())
      {
        if (this.canBeIncapacitated)
          this.Incapacitate(GameTags.HitPointsDepleted);
        else
          this.Kill();
      }
      else
        this.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
    }
    this.UpdateStatus();
    this.UpdateWoundEffects();
    this.UpdateHealthBar();
  }

  [ContextMenu("DoDamage")]
  public void DoDamage() => this.Damage(1f);

  public void Damage(float amount)
  {
    if (this.State != Health.HealthState.Invincible)
      this.hitPoints = Mathf.Max(0.0f, this.hitPoints - amount);
    this.OnHealthChanged(-amount);
  }

  private void UpdateWoundEffects()
  {
    if (!(bool) (UnityEngine.Object) this.effects)
      return;
    if (this.isCritter != this.isCritterPrev)
    {
      if (this.isCritterPrev)
      {
        this.effects.Remove("LightWoundsCritter");
        this.effects.Remove("ModerateWoundsCritter");
        this.effects.Remove("SevereWoundsCritter");
      }
      else
      {
        this.effects.Remove("LightWounds");
        this.effects.Remove("ModerateWounds");
        this.effects.Remove("SevereWounds");
      }
      this.isCritterPrev = this.isCritter;
    }
    string effect_id1;
    string effect_id2;
    string effect_id3;
    if (this.isCritter)
    {
      effect_id1 = "LightWoundsCritter";
      effect_id2 = "ModerateWoundsCritter";
      effect_id3 = "SevereWoundsCritter";
    }
    else
    {
      effect_id1 = "LightWounds";
      effect_id2 = "ModerateWounds";
      effect_id3 = "SevereWounds";
    }
    switch (this.State)
    {
      case Health.HealthState.Perfect:
      case Health.HealthState.Alright:
      case Health.HealthState.Incapacitated:
      case Health.HealthState.Dead:
        this.effects.Remove(effect_id1);
        this.effects.Remove(effect_id2);
        this.effects.Remove(effect_id3);
        break;
      case Health.HealthState.Scuffed:
        if (!this.effects.HasEffect(effect_id1))
          this.effects.Add(effect_id1, true);
        this.effects.Remove(effect_id2);
        this.effects.Remove(effect_id3);
        break;
      case Health.HealthState.Injured:
        this.effects.Remove(effect_id1);
        if (!this.effects.HasEffect(effect_id2))
          this.effects.Add(effect_id2, true);
        this.effects.Remove(effect_id3);
        break;
      case Health.HealthState.Critical:
        this.effects.Remove(effect_id1);
        this.effects.Remove(effect_id2);
        if (this.effects.HasEffect(effect_id3))
          break;
        this.effects.Add(effect_id3, true);
        break;
    }
  }

  private void UpdateStatus()
  {
    float num = this.hitPoints / this.maxHitPoints;
    Health.HealthState healthState = this.State != Health.HealthState.Invincible ? ((double) num < 1.0 ? ((double) num < 0.85000002384185791 ? ((double) num < 0.6600000262260437 ? ((double) num < 0.33 ? ((double) num <= 0.0 ? ((double) num != 0.0 ? Health.HealthState.Dead : Health.HealthState.Incapacitated) : Health.HealthState.Critical) : Health.HealthState.Injured) : Health.HealthState.Scuffed) : Health.HealthState.Alright) : Health.HealthState.Perfect) : Health.HealthState.Invincible;
    if (this.State == healthState)
      return;
    if (this.State == Health.HealthState.Incapacitated && healthState != Health.HealthState.Dead)
      this.OnRecover();
    if (healthState == Health.HealthState.Perfect)
      this.Trigger(-1491582671, (object) this);
    this.State = healthState;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.State != Health.HealthState.Dead && this.State != Health.HealthState.Perfect && this.State != Health.HealthState.Alright && !this.isCritter)
      component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, (object) this.State);
    else
      component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, (StatusItem) null);
  }

  public bool IsIncapacitated() => this.State == Health.HealthState.Incapacitated;

  public bool IsDefeated()
  {
    return this.State == Health.HealthState.Incapacitated || this.State == Health.HealthState.Dead;
  }

  public void Incapacitate(Tag cause)
  {
    this.CauseOfIncapacitation = cause;
    this.State = Health.HealthState.Incapacitated;
    this.Damage(this.hitPoints);
    this.gameObject.Trigger(-1506500077);
  }

  private void Kill()
  {
    if (this.gameObject.GetSMI<DeathMonitor.Instance>() == null)
      return;
    this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
  }

  public enum HealthState
  {
    Perfect,
    Alright,
    Scuffed,
    Injured,
    Critical,
    Incapacitated,
    Dead,
    Invincible,
  }
}
