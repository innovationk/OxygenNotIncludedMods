// Decompiled with JetBrains decompiler
// Type: Database.PermitRarityExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public static class PermitRarityExtensions
{
  public static string GetLocStringName(this PermitRarity rarity)
  {
    switch (rarity)
    {
      case PermitRarity.Unknown:
        return (string) UI.PERMIT_RARITY.UNKNOWN;
      case PermitRarity.Universal:
        return (string) UI.PERMIT_RARITY.UNIVERSAL;
      case PermitRarity.Loyalty:
        return (string) UI.PERMIT_RARITY.LOYALTY;
      case PermitRarity.Common:
        return (string) UI.PERMIT_RARITY.COMMON;
      case PermitRarity.Decent:
        return (string) UI.PERMIT_RARITY.DECENT;
      case PermitRarity.Nifty:
        return (string) UI.PERMIT_RARITY.NIFTY;
      case PermitRarity.Splendid:
        return (string) UI.PERMIT_RARITY.SPLENDID;
      default:
        DebugUtil.DevAssert(false, $"Couldn't get name for rarity {rarity}");
        return "-";
    }
  }
}
