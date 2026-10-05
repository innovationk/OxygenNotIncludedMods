// Decompiled with JetBrains decompiler
// Type: NavigationTactics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public static class NavigationTactics
{
  public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, pathCostPenalty: 4);
  public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12);
  public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999);
  public static NavTactic FetchDronePickup = new NavTactic(1, 0, 0, 0, 1, 0, 1, 1);
}
