// Decompiled with JetBrains decompiler
// Type: FetchDrone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FetchDrone : KMonoBehaviour
{
  private static string BOTTOM = "bottom";
  private static string BOTTOM_CARRY = "bottom_carry";
  private KBatchedAnimController animController;
  private Storage pickupableStorage;
  [MyCmpAdd]
  private ChoreConsumer choreConsumer;

  protected override void OnSpawn()
  {
    ChoreGroup[] choreGroupArray = new ChoreGroup[15]
    {
      Db.Get().ChoreGroups.Build,
      Db.Get().ChoreGroups.Basekeeping,
      Db.Get().ChoreGroups.Cook,
      Db.Get().ChoreGroups.Art,
      Db.Get().ChoreGroups.Dig,
      Db.Get().ChoreGroups.Research,
      Db.Get().ChoreGroups.Farming,
      Db.Get().ChoreGroups.Ranching,
      Db.Get().ChoreGroups.MachineOperating,
      Db.Get().ChoreGroups.MedicalAid,
      Db.Get().ChoreGroups.Combat,
      Db.Get().ChoreGroups.LifeSupport,
      Db.Get().ChoreGroups.Recreation,
      Db.Get().ChoreGroups.Toggle,
      Db.Get().ChoreGroups.Rocketry
    };
    for (int index = 0; index < choreGroupArray.Length; ++index)
    {
      if (choreGroupArray[index] != null)
        this.choreConsumer.SetPermittedByUser(choreGroupArray[index], false);
    }
    foreach (Storage component in this.GetComponents<Storage>())
    {
      if (component.storageID != GameTags.ChargedPortableBattery)
      {
        this.pickupableStorage = component;
        break;
      }
    }
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.pickupableStorage.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
    this.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe(-1697596308);
    this.Unsubscribe(-1582839653);
    base.OnCleanUp();
  }

  private void OnTagsChanged(object data)
  {
    TagChangedEventData changedEventData = ((Boxed<TagChangedEventData>) data).value;
    if (!changedEventData.added || !(changedEventData.tag == GameTags.Creatures.Die))
      return;
    Brain component = this.GetComponent<Brain>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.IsRunning())
      return;
    component.Resume("death");
  }

  private void OnStorageChanged(object data)
  {
    GameObject gameObject = (GameObject) data;
    this.RemoveTracker(gameObject);
    this.ShowPickupSymbol(gameObject);
  }

  private void ShowPickupSymbol(GameObject pickupable)
  {
    bool is_visible = this.pickupableStorage.items.Contains(pickupable);
    if (is_visible)
      this.AddAnimTracker(pickupable);
    this.animController.SetSymbolVisiblity((KAnimHashedString) FetchDrone.BOTTOM, !is_visible);
    this.animController.SetSymbolVisiblity((KAnimHashedString) FetchDrone.BOTTOM_CARRY, is_visible);
  }

  private void AddAnimTracker(GameObject go)
  {
    KAnimControllerBase component1 = go.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || component1.AnimFiles == null || component1.AnimFiles.Length == 0 || !((UnityEngine.Object) component1.AnimFiles[0] != (UnityEngine.Object) null) || !component1.GetComponent<Pickupable>().trackOnPickup)
      return;
    KBatchedAnimTracker component2 = go.GetComponent<KBatchedAnimTracker>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2.controller == (UnityEngine.Object) this.animController)
      return;
    KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.useTargetPoint = false;
    kbatchedAnimTracker.fadeOut = false;
    kbatchedAnimTracker.symbol = (UnityEngine.Object) go.GetComponent<Brain>() != (UnityEngine.Object) null ? new HashedString("snapTo_pivot") : new HashedString("snapTo_thing");
    kbatchedAnimTracker.forceAlwaysVisible = true;
  }

  private void RemoveTracker(GameObject go)
  {
    KBatchedAnimTracker component = (UnityEngine.Object) go != (UnityEngine.Object) null ? go.GetComponent<KBatchedAnimTracker>() : (KBatchedAnimTracker) null;
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.controller == (UnityEngine.Object) this.animController))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }
}
