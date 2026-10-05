// Decompiled with JetBrains decompiler
// Type: Klei.Actions.ActionAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Klei.Actions;

[AttributeUsage(AttributeTargets.Class)]
public class ActionAttribute : Attribute
{
  public readonly string ActionName;

  public ActionAttribute(string actionName) => this.ActionName = actionName;
}
