// Decompiled with JetBrains decompiler
// Type: GameTagExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class GameTagExtensions
{
  public static GameObject Prefab(this Tag tag) => Assets.GetPrefab(tag);

  public static string ProperName(this Tag tag) => TagManager.GetProperName(tag);

  public static string ProperNameStripLink(this Tag tag) => TagManager.GetProperName(tag, true);

  public static Tag Create(SimHashes id) => TagManager.Create(id.ToString());

  public static Tag CreateTag(this SimHashes id) => TagManager.Create(id.ToString());
}
