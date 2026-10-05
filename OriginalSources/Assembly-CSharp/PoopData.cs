// Decompiled with JetBrains decompiler
// Type: PoopData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PoopData
{
  public bool skipSpawningPoop;
  public Storage storage;
  public string popupMessage;
  public Sprite popupIcon;

  public PoopData(bool skipSpawningPoop, Storage storage, string popupMessage = null, Sprite popupIcon = null)
  {
    this.skipSpawningPoop = skipSpawningPoop;
    this.storage = storage;
    this.popupMessage = popupMessage;
    this.popupIcon = popupIcon;
  }
}
