// Decompiled with JetBrains decompiler
// Type: KMod.Content
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace KMod;

[Flags]
public enum Content : byte
{
  LayerableFiles = 1,
  Strings = 2,
  DLL = 4,
  Translation = 8,
  Animation = 16, // 0x10
}
