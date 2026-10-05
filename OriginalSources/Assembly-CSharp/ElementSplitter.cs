// Decompiled with JetBrains decompiler
// Type: ElementSplitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public struct ElementSplitter
{
  public PrimaryElement primaryElement;
  public Func<Pickupable, float, Pickupable> onTakeCB;
  public Func<Pickupable, bool> canAbsorbCB;
  public KPrefabID kPrefabID;

  public ElementSplitter(GameObject go)
  {
    this.primaryElement = go.GetComponent<PrimaryElement>();
    this.kPrefabID = go.GetComponent<KPrefabID>();
    this.onTakeCB = (Func<Pickupable, float, Pickupable>) null;
    this.canAbsorbCB = (Func<Pickupable, bool>) null;
  }
}
