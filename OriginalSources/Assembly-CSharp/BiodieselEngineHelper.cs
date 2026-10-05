// Decompiled with JetBrains decompiler
// Type: BiodieselEngineHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
internal static class BiodieselEngineHelper
{
  public static string ID
  {
    get => DlcManager.IsExpansion1Active() ? "BiodieselEngineCluster" : "BiodieselEngine";
  }

  public static string CODEXID => BiodieselEngineHelper.ID.ToUpperInvariant();

  public static string NAME
  {
    get
    {
      return DlcManager.IsExpansion1Active() ? (string) BUILDINGS.PREFABS.BIODIESELENGINECLUSTER.NAME : (string) BUILDINGS.PREFABS.BIODIESELENGINE.NAME;
    }
  }
}
