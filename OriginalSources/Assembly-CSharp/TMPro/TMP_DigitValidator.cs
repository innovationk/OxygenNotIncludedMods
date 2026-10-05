// Decompiled with JetBrains decompiler
// Type: TMPro.TMP_DigitValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace TMPro;

[Serializable]
public class TMP_DigitValidator : TMP_InputValidator
{
  public override char Validate(ref string text, ref int pos, char ch)
  {
    if (ch < '0' || ch > '9')
      return char.MinValue;
    text += ch.ToString();
    ++pos;
    return ch;
  }
}
