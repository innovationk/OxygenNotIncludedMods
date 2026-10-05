// Decompiled with JetBrains decompiler
// Type: TintedSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
  [ReadOnly]
  public string name;
  public Sprite sprite;
  public Color color;

  public void OnAfterDeserialize()
  {
  }

  public void OnBeforeSerialize()
  {
    if (!((UnityEngine.Object) this.sprite != (UnityEngine.Object) null))
      return;
    this.name = this.sprite.name;
  }
}
