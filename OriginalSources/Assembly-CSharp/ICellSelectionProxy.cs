// Decompiled with JetBrains decompiler
// Type: ICellSelectionProxy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface ICellSelectionProxy
{
  const float CELL_SELECTION_Z_OFFSET = -0.6f;
  const float BACKWALL_SELECTION_Z_OFFSET = -0.5f;

  Element Element { get; }

  void OnObjectSelected(object o);

  static bool IsSelectionProxy(GameObject go)
  {
    return CellSelectionObject.IsSelectionObject(go) || BackwallSelectionObject.IsBackwallSelectionObject(go);
  }
}
