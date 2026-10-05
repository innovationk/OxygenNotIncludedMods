// Decompiled with JetBrains decompiler
// Type: ResearchTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ResearchTypes
{
  public List<ResearchType> Types = new List<ResearchType>();

  public ResearchTypes()
  {
    this.Types.Add(new ResearchType("basic", (string) RESEARCH.TYPES.ALPHA.NAME, (string) RESEARCH.TYPES.ALPHA.DESC, Assets.GetSprite((HashedString) "research_type_alpha_icon"), new Color(0.596078455f, 0.6666667f, 0.9137255f), new Recipe.Ingredient[1]
    {
      new Recipe.Ingredient("Dirt".ToTag(), 100f)
    }, 600f, (HashedString) "research_center_kanim", new string[1]
    {
      "ResearchCenter"
    }, (string) RESEARCH.TYPES.ALPHA.RECIPEDESC));
    this.Types.Add(new ResearchType("advanced", (string) RESEARCH.TYPES.BETA.NAME, (string) RESEARCH.TYPES.BETA.DESC, Assets.GetSprite((HashedString) "research_type_beta_icon"), new Color(0.6f, 0.384313732f, 0.5686275f), new Recipe.Ingredient[1]
    {
      new Recipe.Ingredient("Water".ToTag(), 25f)
    }, 1200f, (HashedString) "research_center_kanim", new string[1]
    {
      "AdvancedResearchCenter"
    }, (string) RESEARCH.TYPES.BETA.RECIPEDESC));
    this.Types.Add(new ResearchType("space", (string) RESEARCH.TYPES.GAMMA.NAME, (string) RESEARCH.TYPES.GAMMA.DESC, Assets.GetSprite((HashedString) "research_type_gamma_icon"), (Color) new Color32((byte) 240 /*0xF0*/, (byte) 141, (byte) 44, byte.MaxValue), (Recipe.Ingredient[]) null, 2400f, (HashedString) "research_center_kanim", new string[1]
    {
      "CosmicResearchCenter"
    }, (string) RESEARCH.TYPES.GAMMA.RECIPEDESC));
    this.Types.Add(new ResearchType("nuclear", (string) RESEARCH.TYPES.DELTA.NAME, (string) RESEARCH.TYPES.DELTA.DESC, Assets.GetSprite((HashedString) "research_type_delta_icon"), (Color) new Color32((byte) 231, (byte) 210, (byte) 17, byte.MaxValue), (Recipe.Ingredient[]) null, 2400f, (HashedString) "research_center_kanim", new string[1]
    {
      "NuclearResearchCenter"
    }, (string) RESEARCH.TYPES.DELTA.RECIPEDESC));
    this.Types.Add(new ResearchType("orbital", (string) RESEARCH.TYPES.ORBITAL.NAME, (string) RESEARCH.TYPES.ORBITAL.DESC, Assets.GetSprite((HashedString) "research_type_orbital_icon"), (Color) new Color32((byte) 240 /*0xF0*/, (byte) 141, (byte) 44, byte.MaxValue), (Recipe.Ingredient[]) null, 2400f, (HashedString) "research_center_kanim", new string[2]
    {
      "OrbitalResearchCenter",
      "DLC1CosmicResearchCenter"
    }, (string) RESEARCH.TYPES.ORBITAL.RECIPEDESC));
  }

  public ResearchType GetResearchType(string id)
  {
    foreach (ResearchType type in this.Types)
    {
      if (id == type.id)
        return type;
    }
    Debug.LogWarning((object) $"No research with type id {id} found");
    return (ResearchType) null;
  }

  public class ID
  {
    public const string BASIC = "basic";
    public const string ADVANCED = "advanced";
    public const string SPACE = "space";
    public const string NUCLEAR = "nuclear";
    public const string ORBITAL = "orbital";
  }
}
