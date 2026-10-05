// Decompiled with JetBrains decompiler
// Type: AccessorySlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class AccessorySlot : Resource
{
  private KAnimFile file;

  public KAnimHashedString targetSymbolId { get; private set; }

  public List<Accessory> accessories { get; private set; }

  public KAnimFile AnimFile => this.file;

  public KAnimFile defaultAnimFile { get; private set; }

  public int overrideLayer { get; private set; }

  public AccessorySlot(string id, ResourceSet parent, KAnimFile swap_build, int overrideLayer = 0)
    : base(id, parent)
  {
    if ((UnityEngine.Object) swap_build == (UnityEngine.Object) null)
      Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", (object) id);
    this.targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
    this.accessories = new List<Accessory>();
    this.file = swap_build;
    this.overrideLayer = overrideLayer;
    this.defaultAnimFile = swap_build;
  }

  public AccessorySlot(
    string id,
    ResourceSet parent,
    KAnimHashedString target_symbol_id,
    KAnimFile swap_build,
    KAnimFile defaultAnimFile = null,
    int overrideLayer = 0)
    : base(id, parent)
  {
    if ((UnityEngine.Object) swap_build == (UnityEngine.Object) null)
      Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", (object) id);
    this.targetSymbolId = target_symbol_id;
    this.accessories = new List<Accessory>();
    this.file = swap_build;
    this.defaultAnimFile = (UnityEngine.Object) defaultAnimFile != (UnityEngine.Object) null ? defaultAnimFile : swap_build;
    this.overrideLayer = overrideLayer;
  }

  public void AddAccessories(KAnimFile default_build, ResourceSet parent)
  {
    KAnim.Build build = default_build.GetData().build;
    default_build.GetData().build.GetSymbol(this.targetSymbolId);
    string lower = this.Id.ToLower();
    for (int index = 0; index < build.symbols.Length; ++index)
    {
      string id = HashCache.Get().Get(build.symbols[index].hash);
      if (id.StartsWith(lower))
      {
        Accessory accessory = new Accessory(id, parent, this, this.file.batchTag, build.symbols[index], default_build);
        this.accessories.Add(accessory);
        HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
      }
    }
  }

  public Accessory Lookup(string id) => this.Lookup(new HashedString(id));

  public Accessory Lookup(HashedString full_id)
  {
    return !full_id.IsValid ? (Accessory) null : this.accessories.Find((Predicate<Accessory>) (a => a.IdHash == full_id));
  }
}
