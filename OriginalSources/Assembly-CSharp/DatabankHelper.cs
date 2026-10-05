// Decompiled with JetBrains decompiler
// Type: DatabankHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public abstract class DatabankHelper
{
  public const string CODEXID = "Databank";

  public static string ID
  {
    get => DlcManager.IsExpansion1Active() ? "OrbitalResearchDatabank" : "ResearchDatabank";
  }

  public static Tag TAG
  {
    get
    {
      return DlcManager.IsExpansion1Active() ? OrbitalResearchDatabankConfig.TAG : ResearchDatabankConfig.TAG;
    }
  }

  public static string RESEARCH_NAME
  {
    get
    {
      return DlcManager.IsExpansion1Active() ? (string) RESEARCH.TYPES.ORBITAL.NAME : (string) RESEARCH.TYPES.GAMMA.NAME;
    }
  }

  public static string RESEARCH_CODEXID
  {
    get => DlcManager.IsExpansion1Active() ? "RESEARCHDLC1" : "RESEARCH";
  }

  public static string NAME
  {
    get
    {
      return DlcManager.IsExpansion1Active() ? (string) ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME : (string) ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME;
    }
  }

  public static string NAME_PLURAL
  {
    get
    {
      return DlcManager.IsExpansion1Active() ? (string) ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME_PLURAL : (string) ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME_PLURAL;
    }
  }

  public static string DESC
  {
    get
    {
      return DlcManager.IsExpansion1Active() ? (string) ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.DESC : (string) ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.DESC;
    }
  }

  public static Sprite SPRITE => Assets.GetSprite((HashedString) "ui_databank");
}
