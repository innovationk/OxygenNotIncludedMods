// Decompiled with JetBrains decompiler
// Type: Klei.AI.PrefabAttributeModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

[AddComponentMenu("KMonoBehaviour/scripts/PrefabAttributeModifiers")]
public class PrefabAttributeModifiers : KMonoBehaviour
{
  public List<AttributeModifier> descriptors = new List<AttributeModifier>();

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public void AddAttributeDescriptor(AttributeModifier modifier) => this.descriptors.Add(modifier);

  public void RemovePrefabAttribute(AttributeModifier modifier)
  {
    this.descriptors.Remove(modifier);
  }
}
