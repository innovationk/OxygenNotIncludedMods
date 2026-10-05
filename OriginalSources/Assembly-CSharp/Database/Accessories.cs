// Decompiled with JetBrains decompiler
// Type: Database.Accessories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Database;

public class Accessories(ResourceSet parent) : ResourceSet<Accessory>(nameof (Accessories), parent)
{
  public void AddAccessories(string id, KAnimFile anim_file)
  {
    if (!((UnityEngine.Object) anim_file != (UnityEngine.Object) null))
      return;
    KAnim.Build build = anim_file.GetData().build;
    for (int index = 0; index < build.symbols.Length; ++index)
    {
      string symbol_name = HashCache.Get().Get(build.symbols[index].hash);
      AccessorySlot slot = Db.Get().AccessorySlots.Find((KAnimHashedString) symbol_name);
      if (slot != null)
      {
        Accessory accessory = new Accessory(id + symbol_name, (ResourceSet) this, slot, anim_file.batchTag, build.symbols[index], anim_file);
        slot.accessories.Add(accessory);
        HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
      }
    }
  }

  public void AddCustomAccessories(KAnimFile anim_file, ResourceSet parent, AccessorySlots slots)
  {
    if (!((UnityEngine.Object) anim_file != (UnityEngine.Object) null))
      return;
    KAnim.Build build = anim_file.GetData().build;
    for (int index = 0; index < build.symbols.Length; ++index)
    {
      string symbol_name = HashCache.Get().Get(build.symbols[index].hash);
      AccessorySlot slot1 = slots.resources.Find((Predicate<AccessorySlot>) (slot => symbol_name.IndexOf(slot.Id, 0, StringComparison.OrdinalIgnoreCase) != -1));
      if (slot1 != null)
      {
        Accessory accessory = new Accessory(symbol_name, parent, slot1, anim_file.batchTag, build.symbols[index], anim_file);
        slot1.accessories.Add(accessory);
        HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
      }
    }
  }
}
