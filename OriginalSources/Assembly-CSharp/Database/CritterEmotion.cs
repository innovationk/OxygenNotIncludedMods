// Decompiled with JetBrains decompiler
// Type: Database.CritterEmotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Database;

public class CritterEmotion
{
  public string id;
  public bool isPositiveEmotion;
  public Sprite sprite;

  public CritterEmotion(string id, bool isPositiveEmotion, Sprite sprite)
  {
    this.id = id;
    this.isPositiveEmotion = isPositiveEmotion;
    this.sprite = sprite;
  }
}
