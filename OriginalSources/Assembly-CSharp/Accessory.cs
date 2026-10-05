// Decompiled with JetBrains decompiler
// Type: Accessory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Accessory : Resource
{
  public KAnim.Build.Symbol symbol { get; private set; }

  public HashedString batchSource { get; private set; }

  public AccessorySlot slot { get; private set; }

  public KAnimFile animFile { get; private set; }

  public Accessory(
    string id,
    ResourceSet parent,
    AccessorySlot slot,
    HashedString batchSource,
    KAnim.Build.Symbol symbol,
    KAnimFile animFile = null,
    KAnimFile defaultAnimFile = null)
    : base(id, parent)
  {
    this.slot = slot;
    this.symbol = symbol;
    this.batchSource = batchSource;
    this.animFile = animFile;
  }

  public bool IsDefault() => (Object) this.animFile == (Object) this.slot.defaultAnimFile;
}
