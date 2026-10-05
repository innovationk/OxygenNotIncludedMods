// Decompiled with JetBrains decompiler
// Type: DehydratedFoodPackage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FoodRehydrator;
using KSerialization;
using System;
using System.Linq;
using UnityEngine;

#nullable disable
public class DehydratedFoodPackage : Workable, IApproachable
{
  [Serialize]
  public Tag FoodTag;
  [MyCmpReq]
  private Storage storage;

  public GameObject Rehydrator
  {
    get
    {
      Storage storage = this.gameObject.GetComponent<Pickupable>().storage;
      return (UnityEngine.Object) storage != (UnityEngine.Object) null ? storage.gameObject : (GameObject) null;
    }
    private set
    {
    }
  }

  public override BuildingFacade GetBuildingFacade()
  {
    return !((UnityEngine.Object) this.Rehydrator != (UnityEngine.Object) null) ? (BuildingFacade) null : this.Rehydrator.GetComponent<BuildingFacade>();
  }

  public override KAnimControllerBase GetAnimController()
  {
    return !((UnityEngine.Object) this.Rehydrator != (UnityEngine.Object) null) ? (KAnimControllerBase) null : this.Rehydrator.GetComponent<KAnimControllerBase>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    CellOffset[] offsets = new CellOffset[2];
    offsets[1] = new CellOffset(0, -1);
    this.SetOffsets(offsets);
    if (this.storage.items.Count < 1)
    {
      this.storage.ConsumeAllIgnoringDisease(this.FoodTag);
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(this.FoodTag), Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures);
      go.SetActive(true);
      go.GetComponent<Edible>().Calories = 1000000f;
      this.storage.Store(go);
    }
    this.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
    this.DehydrateItem(this.storage.items.ElementAtOrDefault<GameObject>(0));
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    if (!((UnityEngine.Object) this.Rehydrator != (UnityEngine.Object) null))
      return;
    DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetFabricatedFoodSymbol(this.FoodTag);
    this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable((Workable) this);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    if (this.storage.items.Count != 1)
    {
      DebugUtil.DevAssert(false, "OnCompleteWork invalid contents of package");
    }
    else
    {
      GameObject gameObject1 = this.storage.items[0];
      this.storage.Transfer(worker.GetComponent<Storage>());
      DebugUtil.DevAssert((UnityEngine.Object) this.Rehydrator != (UnityEngine.Object) null, "OnCompleteWork but no rehydrator");
      DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
      this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable((Workable) null);
      GameObject gameObject2 = this.gameObject;
      GameObject food = gameObject1;
      component.ConsumeResourcesForRehydration(gameObject2, food);
      DehydratedFoodPackage.RehydrateStartWorkItem startWorkInfo = (DehydratedFoodPackage.RehydrateStartWorkItem) worker.GetStartWorkInfo();
      if (startWorkInfo == null || startWorkInfo.setResultCb == null || !((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null))
        return;
      startWorkInfo.setResultCb(gameObject1);
    }
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    if (!((UnityEngine.Object) this.Rehydrator != (UnityEngine.Object) null))
      return;
    this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable((Workable) null);
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  private void StorageChangeHandler(object obj)
  {
    GameObject gameObject = (GameObject) obj;
    DebugUtil.DevAssert(!this.storage.items.Contains(gameObject), "Attempting to add item to a dehydrated food package which is not allowed");
    this.RehydrateItem(gameObject);
  }

  public void DehydrateItem(GameObject item)
  {
    DebugUtil.DevAssert((UnityEngine.Object) item != (UnityEngine.Object) null, "Attempting to dehydrate contents of an empty packet");
    if (this.storage.items.Count != 1 || (UnityEngine.Object) item == (UnityEngine.Object) null)
      DebugUtil.DevAssert(false, "DehydrateItem called, incorrect content");
    else
      item.AddTag(GameTags.Dehydrated);
  }

  public void RehydrateItem(GameObject item)
  {
    if (this.storage.items.Count != 0)
    {
      DebugUtil.DevAssert(false, "RehydrateItem called, incorrect storage content");
    }
    else
    {
      item.RemoveTag(GameTags.Dehydrated);
      item.AddTag(GameTags.Rehydrated);
      item.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood);
    }
  }

  private void Swap<Type>(ref Type a, ref Type b)
  {
    Type ype = a;
    a = b;
    b = ype;
  }

  public class RehydrateStartWorkItem : WorkerBase.StartWorkInfo
  {
    public DehydratedFoodPackage package;
    public Action<GameObject> setResultCb;

    public RehydrateStartWorkItem(DehydratedFoodPackage pkg, Action<GameObject> setResultCB)
      : base((Workable) pkg)
    {
      this.package = pkg;
      this.setResultCb = setResultCB;
    }
  }
}
