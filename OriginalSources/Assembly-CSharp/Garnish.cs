// Decompiled with JetBrains decompiler
// Type: Garnish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Garnish
{
  public Tag itemTag;
  public string effectId;
  public float consumeRate;
  public int priority;
  public Descriptor descriptor;
  private HashedString overrideAnimName;
  private KAnimHashedString overrideSymbolName;
  private Color fxTintColor = Color.white;
  private static readonly HashedString SALT_SYMBOL = (HashedString) "saltshaker";
  private static readonly HashedString SALT_FG_SYMBOL = (HashedString) "saltshaker_fg";
  private static readonly KAnimHashedString SALT_PARTICLE_SYMBOL = new KAnimHashedString("salt_particle");
  public static readonly List<Garnish> All = new List<Garnish>()
  {
    new Garnish()
    {
      itemTag = TableSaltConfig.TAG,
      effectId = "MessTableSalt",
      consumeRate = TableSaltTuning.CONSUMABLE_RATE,
      priority = 0,
      descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.MESS_TABLE_SALT, (object) TableSaltTuning.MORALE_MODIFIER), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, (object) TableSaltTuning.MORALE_MODIFIER))
    },
    new Garnish()
    {
      itemTag = CaviarConfig.TAG,
      effectId = "MessCaviar",
      consumeRate = CaviarTuning.CONSUMABLE_RATE,
      priority = 10,
      descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.MESS_CAVIAR, (object) CaviarTuning.MORALE_MODIFIER, (object) CaviarTuning.STRESS_MODIFIER), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.MESS_CAVIAR, (object) CaviarTuning.MORALE_MODIFIER, (object) CaviarTuning.STRESS_MODIFIER)),
      overrideAnimName = (HashedString) "caviarshaker_kanim",
      overrideSymbolName = (KAnimHashedString) "object",
      fxTintColor = Color.black
    }
  };

  public KAnim.Build.Symbol GetOverrideSymbol()
  {
    if (!this.overrideAnimName.IsValid)
      return (KAnim.Build.Symbol) null;
    KAnimFile anim = Assets.GetAnim(this.overrideAnimName);
    return (Object) anim == (Object) null ? (KAnim.Build.Symbol) null : anim.GetData().build.GetSymbol(this.overrideSymbolName);
  }

  public EffectInstance Activate(Storage storage, GameObject diner)
  {
    storage.ConsumeIgnoringDisease(this.itemTag, this.consumeRate);
    EffectInstance effectInstance = diner.GetComponent<Effects>().Add(this.effectId, true);
    KAnim.Build.Symbol overrideSymbol = this.GetOverrideSymbol();
    SymbolOverrideController component1;
    if (overrideSymbol != null && diner.TryGetComponent<SymbolOverrideController>(out component1))
    {
      component1.AddSymbolOverride(Garnish.SALT_SYMBOL, overrideSymbol);
      component1.AddSymbolOverride(Garnish.SALT_FG_SYMBOL, overrideSymbol);
    }
    KBatchedAnimController component2;
    if (!diner.TryGetComponent<KBatchedAnimController>(out component2))
      return effectInstance;
    component2.SetSymbolTint(Garnish.SALT_PARTICLE_SYMBOL, this.fxTintColor);
    return effectInstance;
  }

  public static void Deactivate(GameObject diner)
  {
    SymbolOverrideController component1;
    if (diner.TryGetComponent<SymbolOverrideController>(out component1))
    {
      component1.RemoveSymbolOverride(Garnish.SALT_SYMBOL);
      component1.RemoveSymbolOverride(Garnish.SALT_FG_SYMBOL);
    }
    KBatchedAnimController component2;
    if (!diner.TryGetComponent<KBatchedAnimController>(out component2))
      return;
    component2.SetSymbolTint(Garnish.SALT_PARTICLE_SYMBOL, Color.white);
  }

  public static void SetDinerVisibility(KAnimControllerBase controller, bool visible)
  {
    controller.SetSymbolVisiblity((KAnimHashedString) Garnish.SALT_SYMBOL, visible);
    controller.SetSymbolVisiblity((KAnimHashedString) Garnish.SALT_FG_SYMBOL, visible);
  }

  public static Garnish GetActive(Storage storage)
  {
    if ((Object) storage == (Object) null)
      return (Garnish) null;
    Garnish active = (Garnish) null;
    foreach (Garnish garnish in Garnish.All)
    {
      if ((double) storage.GetMassAvailable(garnish.itemTag) >= (double) garnish.consumeRate && (active == null || garnish.priority > active.priority))
        active = garnish;
    }
    return active;
  }

  public static bool HasAny(Storage storage) => Garnish.GetActive(storage) != null;
}
