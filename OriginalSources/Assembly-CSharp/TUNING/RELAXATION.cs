// Decompiled with JetBrains decompiler
// Type: TUNING.RELAXATION
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace TUNING;

public class RELAXATION
{
  public const float MASSAGE_TABLE = -30f;

  public abstract class PRIORITY
  {
    public static int TIER0 = 1;
    public static int RECENTLY_USED = 5;
    public static int TIER1 = 10;
    public static int TIER2 = 20;
    public static int TIER3 = 30;
    public static int TIER4 = 40;
    public static int TIER5 = 50;
    public static int SPECIAL_EVENT = 100;
  }
}
