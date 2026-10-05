// Decompiled with JetBrains decompiler
// Type: DevQuickActionInstruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public struct DevQuickActionInstruction(string address, System.Action action)
{
  public string Address = address;
  public System.Action Action = action;

  public DevQuickActionInstruction(
    IDevQuickAction.CommonMenusNames category,
    string name,
    System.Action action)
    : this($"{category.ToString()}/{name}", action)
  {
  }
}
