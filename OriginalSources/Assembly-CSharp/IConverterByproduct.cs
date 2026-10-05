// Decompiled with JetBrains decompiler
// Type: IConverterByproduct
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public interface IConverterByproduct
{
  Tag ByproductAssociatedInputTag { get; }

  Tag ByproductTag { get; }

  float ByproductRate { get; }

  bool ByproductIsContinuous { get; }

  void GetByproductDescriptors(GameObject go, List<Descriptor> descriptors);
}
