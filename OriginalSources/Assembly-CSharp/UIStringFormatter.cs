// Decompiled with JetBrains decompiler
// Type: UIStringFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class UIStringFormatter
{
  private List<UIStringFormatter.Entry> entries = new List<UIStringFormatter.Entry>();

  private struct Entry
  {
    public string format;
    public string key;
    public string value;
    public string result;
  }
}
