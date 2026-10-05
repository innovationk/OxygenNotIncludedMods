// Decompiled with JetBrains decompiler
// Type: BionicMinionStorageExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BionicMinionStorageExtension : 
  KMonoBehaviour,
  StoredMinionIdentity.IStoredMinionExtension
{
  private static readonly List<Tag> StoragesTypesToTransfer = new List<Tag>()
  {
    GameTags.StoragesIds.BionicBatteryStorage,
    GameTags.StoragesIds.BionicUpgradeStorage,
    GameTags.StoragesIds.BionicOxygenTankStorage
  };

  public void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject)
  {
    Storage[] components = storedMinionGameObject.GetComponents<Storage>();
    foreach (Tag tag in BionicMinionStorageExtension.StoragesTypesToTransfer)
    {
      Tag inventoryType = tag;
      if (components == null || !((UnityEngine.Object) components.FindFirst<Storage>((Func<Storage, bool>) (s => s.storageID == inventoryType)) != (UnityEngine.Object) null))
      {
        Storage storage = storedMinionGameObject.AddComponent<Storage>();
        storage.allowItemRemoval = false;
        storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
        storage.storageID = inventoryType;
      }
    }
  }

  void StoredMinionIdentity.IStoredMinionExtension.PullFrom(StoredMinionIdentity source)
  {
    Storage[] components1 = source.GetComponents<Storage>();
    Storage[] components2 = this.GetComponents<Storage>();
    foreach (Storage storage in components1)
    {
      bool test = false;
      foreach (Storage target in components2)
      {
        if (target.storageID == storage.storageID)
        {
          storage.Transfer(target, hide_popups: true);
          test = true;
          break;
        }
      }
      DebugUtil.DevAssert(test, "Missmatched storages on BionicMinionStorageExtension");
    }
  }

  void StoredMinionIdentity.IStoredMinionExtension.PushTo(StoredMinionIdentity destination)
  {
    GameObject gameObject = destination.gameObject;
    this.AddStoredMinionGameObjectRequirements(gameObject);
    Storage[] components1 = this.GetComponents<Storage>();
    Storage[] components2 = gameObject.GetComponents<Storage>();
    foreach (Tag tag in BionicMinionStorageExtension.StoragesTypesToTransfer)
    {
      Storage storage1 = (Storage) null;
      Storage target = (Storage) null;
      foreach (Storage storage2 in components1)
      {
        if (storage2.storageID == tag)
        {
          storage1 = storage2;
          break;
        }
      }
      foreach (Storage storage3 in components2)
      {
        if (storage3.storageID == tag)
        {
          target = storage3;
          break;
        }
      }
      storage1.Transfer(target, true, true);
    }
  }
}
