// Decompiled with JetBrains decompiler
// Type: DevToolCommandPaletteUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public static class DevToolCommandPaletteUtil
{
  public static List<DevToolCommandPalette.Command> GenerateDefaultCommandPalette()
  {
    List<DevToolCommandPalette.Command> defaultCommandPalette = new List<DevToolCommandPalette.Command>();
    foreach (System.Type type in ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>())
    {
      System.Type devToolType = type;
      if (!devToolType.IsAbstract && ReflectionUtil.HasDefaultConstructor(devToolType))
        defaultCommandPalette.Add(new DevToolCommandPalette.Command($"Open DevTool: \"{DevToolUtil.GenerateDevToolName(devToolType)}\"", (System.Action) (() => DevToolUtil.Open((DevTool) Activator.CreateInstance(devToolType)))));
    }
    return defaultCommandPalette;
  }
}
