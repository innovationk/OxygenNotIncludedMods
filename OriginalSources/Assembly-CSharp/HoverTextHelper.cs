// Decompiled with JetBrains decompiler
// Type: HoverTextHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class HoverTextHelper
{
  private static readonly string[] massStrings = new string[4];
  private static readonly string[] invalidCellMassStrings = new string[4]
  {
    "",
    "",
    "",
    ""
  };
  private static float cachedMass = -1f;
  private static Element cachedElement;

  public static void DestroyStatics()
  {
    HoverTextHelper.cachedElement = (Element) null;
    HoverTextHelper.cachedMass = -1f;
  }

  public static string[] MassStringsReadOnly(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return HoverTextHelper.invalidCellMassStrings;
    Element element = Grid.Element[cell];
    float num1 = Grid.Mass[cell];
    if (element == HoverTextHelper.cachedElement && (double) num1 == (double) HoverTextHelper.cachedMass)
      return HoverTextHelper.massStrings;
    HoverTextHelper.cachedElement = element;
    HoverTextHelper.cachedMass = num1;
    HoverTextHelper.massStrings[3] = " " + GameUtil.GetBreathableString(element, num1);
    if (element.id == SimHashes.Vacuum)
    {
      HoverTextHelper.massStrings[0] = (string) UI.NA;
      HoverTextHelper.massStrings[1] = "";
      HoverTextHelper.massStrings[2] = "";
    }
    else if (element.id == SimHashes.Unobtanium)
    {
      HoverTextHelper.massStrings[0] = (string) UI.NEUTRONIUMMASS;
      HoverTextHelper.massStrings[1] = "";
      HoverTextHelper.massStrings[2] = "";
    }
    else
    {
      HoverTextHelper.massStrings[2] = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
      if ((double) num1 < 5.0)
      {
        num1 *= 1000f;
        HoverTextHelper.massStrings[2] = (string) UI.UNITSUFFIXES.MASS.GRAM;
      }
      if ((double) num1 < 5.0)
      {
        num1 *= 1000f;
        HoverTextHelper.massStrings[2] = (string) UI.UNITSUFFIXES.MASS.MILLIGRAM;
      }
      if ((double) num1 < 5.0)
      {
        float f = num1 * 1000f;
        HoverTextHelper.massStrings[2] = (string) UI.UNITSUFFIXES.MASS.MICROGRAM;
        num1 = Mathf.Floor(f);
      }
      int num2 = Mathf.FloorToInt(num1);
      int num3 = Mathf.RoundToInt((float) (10.0 * ((double) num1 - (double) num2)));
      if (num3 == 10)
      {
        ++num2;
        num3 = 0;
      }
      HoverTextHelper.massStrings[0] = num2.ToString();
      HoverTextHelper.massStrings[1] = "." + num3.ToString();
    }
    return HoverTextHelper.massStrings;
  }
}
