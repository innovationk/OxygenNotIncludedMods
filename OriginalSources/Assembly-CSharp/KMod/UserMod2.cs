// Decompiled with JetBrains decompiler
// Type: KMod.UserMod2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace KMod;

public class UserMod2
{
  public Assembly assembly { get; set; }

  public string path { get; set; }

  public Mod mod { get; set; }

  public virtual void OnLoad(Harmony harmony) => harmony.PatchAll(this.assembly);

  public virtual void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
  {
  }
}
