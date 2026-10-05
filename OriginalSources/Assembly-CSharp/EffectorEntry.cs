// Decompiled with JetBrains decompiler
// Type: EffectorEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
internal struct EffectorEntry(string name, float value)
{
  public string name = name;
  public int count = 1;
  public float value = value;

  public override string ToString()
  {
    string str = "";
    if (this.count > 1)
      str = string.Format((string) UI.OVERLAYS.DECOR.COUNT, (object) this.count);
    return string.Format((string) UI.OVERLAYS.DECOR.ENTRY, (object) GameUtil.GetFormattedDecor(this.value), (object) this.name, (object) str);
  }
}
