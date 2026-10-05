// Decompiled with JetBrains decompiler
// Type: KeroseneEngineHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
internal static class KeroseneEngineHelper
{
  public static string ID
  {
    get => DlcManager.IsExpansion1Active() ? "KeroseneEngineCluster" : "KeroseneEngine";
  }

  public static string CODEXID => KeroseneEngineHelper.ID.ToUpperInvariant();

  public static string NAME
  {
    get
    {
      return DlcManager.IsExpansion1Active() ? (string) BUILDINGS.PREFABS.KEROSENEENGINECLUSTER.NAME : (string) BUILDINGS.PREFABS.KEROSENEENGINE.NAME;
    }
  }
}
