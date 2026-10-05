// Decompiled with JetBrains decompiler
// Type: ChoreHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class ChoreHelpers
{
  public static GameObject CreateLocator(string name, Vector3 pos)
  {
    GameObject locator = Util.KInstantiate(Assets.GetPrefab((Tag) ApproachableLocator.ID));
    locator.name = name;
    locator.transform.SetPosition(pos);
    locator.gameObject.SetActive(true);
    return locator;
  }

  public static GameObject CreateSleepLocator(Vector3 pos)
  {
    GameObject sleepLocator = Util.KInstantiate(Assets.GetPrefab((Tag) SleepLocator.ID));
    sleepLocator.name = "SLeepLocator";
    sleepLocator.transform.SetPosition(pos);
    sleepLocator.gameObject.SetActive(true);
    return sleepLocator;
  }

  public static void DestroyLocator(GameObject locator)
  {
    if (!((Object) locator != (Object) null))
      return;
    locator.gameObject.DeleteObject();
  }
}
