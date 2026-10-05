// Decompiled with JetBrains decompiler
// Type: PrefabDefinedUIPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PrefabDefinedUIPosition
{
  private Option<Vector2> position;

  public void SetOn(GameObject gameObject)
  {
    if (this.position.HasValue)
      gameObject.rectTransform().anchoredPosition = this.position.Value;
    else
      this.position = (Option<Vector2>) gameObject.rectTransform().anchoredPosition;
  }

  public void SetOn(Component component)
  {
    if (this.position.HasValue)
      component.rectTransform().anchoredPosition = this.position.Value;
    else
      this.position = (Option<Vector2>) component.rectTransform().anchoredPosition;
  }
}
