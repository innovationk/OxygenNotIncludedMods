// Decompiled with JetBrains decompiler
// Type: ProcGenGame.WorldgenException
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace ProcGenGame;

public class WorldgenException : Exception
{
  public readonly string userMessage;

  public WorldgenException(string message, string userMessage)
    : base(message)
  {
    this.userMessage = userMessage;
  }
}
