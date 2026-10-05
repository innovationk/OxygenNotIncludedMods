// Decompiled with JetBrains decompiler
// Type: ExecutableSpecificString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ExecutableSpecificString
{
  private string baseString;
  private string soString;

  public ExecutableSpecificString(string baseStr, string soStr)
  {
    this.baseString = baseStr;
    this.soString = soStr;
  }

  public static implicit operator string(ExecutableSpecificString dualString)
  {
    return !DlcManager.IsExpansion1Active() ? dualString.baseString : dualString.soString;
  }

  public static implicit operator LocString(ExecutableSpecificString dualString)
  {
    return new LocString((string) dualString);
  }
}
