// Decompiled with JetBrains decompiler
// Type: ColdImmunityProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ColdImmunityProvider : EffectImmunityProviderStation<ColdImmunityProvider.Instance>
{
  public const string PROVIDED_IMMUNITY_EFFECT_NAME = "WarmTouch";

  public new class Def : 
    EffectImmunityProviderStation<ColdImmunityProvider.Instance>.Def,
    IGameObjectEffectDescriptor
  {
    public override string[] DefaultAnims()
    {
      return new string[3]
      {
        "warmup_pre",
        "warmup_loop",
        "warmup_pst"
      };
    }

    public override string DefaultAnimFileName() => "anim_warmup_kanim";

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      return new List<Descriptor>()
      {
        new Descriptor((string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{"WarmTouch".ToUpper()}.PROVIDERS_NAME"), (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{"WarmTouch".ToUpper()}.PROVIDERS_TOOLTIP"))
      };
    }
  }

  public new class Instance(IStateMachineTarget master, ColdImmunityProvider.Def def) : 
    EffectImmunityProviderStation<ColdImmunityProvider.Instance>.BaseInstance(master, (EffectImmunityProviderStation<ColdImmunityProvider.Instance>.Def) def)
  {
    protected override void ApplyImmunityEffect(Effects target) => target.Add("WarmTouch", true);
  }
}
