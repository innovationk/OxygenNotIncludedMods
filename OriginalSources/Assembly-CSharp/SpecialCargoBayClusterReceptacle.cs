// Decompiled with JetBrains decompiler
// Type: SpecialCargoBayClusterReceptacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Linq;
using UnityEngine;

#nullable disable
public class SpecialCargoBayClusterReceptacle : 
  SingleEntityReceptacle,
  IBaggedStateAnimationInstructions
{
  public const string TRAPPED_CRITTER_ANIM_NAME = "rocket_biological";
  [MyCmpReq]
  private SymbolOverrideController symbolOverrideComponent;
  [MyCmpGet]
  private KBatchedAnimController buildingAnimCtr;
  private KBatchedAnimController lootKBAC;
  public Storage sideProductStorage;
  private SpecialCargoBayCluster.Instance capsule;
  private GameObject LastCritterDead;
  [Serialize]
  private int originWorldID;
  private static Tag[] tagsForCritter = new Tag[6]
  {
    GameTags.Creatures.TrappedInCargoBay,
    GameTags.Creatures.PausedHunger,
    GameTags.Creatures.PausedReproduction,
    GameTags.Creatures.PreventGrowAnimation,
    GameTags.HideHealthBar,
    GameTags.PreventDeadAnimation
  };
  private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>((Action<SpecialCargoBayClusterReceptacle, object>) ((component, data) => component.OnRocketLanded(data)));
  private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnCargoBayRelocatedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>((Action<SpecialCargoBayClusterReceptacle, object>) ((component, data) => component.OnCargoBayRelocated(data)));

  public bool IsRocketOnGround => this.gameObject.HasTag(GameTags.RocketOnGround);

  public bool IsRocketInSpace => this.gameObject.HasTag(GameTags.RocketInSpace);

  private bool isDoorOpen => this.capsule.sm.IsDoorOpen.Get(this.capsule);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreType = Db.Get().ChoreTypes.CreatureFetch;
  }

  protected override void OnSpawn()
  {
    this.capsule = this.gameObject.GetSMI<SpecialCargoBayCluster.Instance>();
    this.SetupLootSymbolObject();
    base.OnSpawn();
    this.SetTrappedCritterAnimations(this.Occupant);
    this.Subscribe(-1697596308, new Action<object>(this.OnCritterStorageChanged));
    this.Subscribe<SpecialCargoBayClusterReceptacle>(-887025858, SpecialCargoBayClusterReceptacle.OnRocketLandedDelegate);
    this.Subscribe<SpecialCargoBayClusterReceptacle>(-1447108533, SpecialCargoBayClusterReceptacle.OnCargoBayRelocatedDelegate);
    this.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    SpecialCargoBayClusterReceptacle component = gameObject.GetComponent<SpecialCargoBayClusterReceptacle>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    Tag entityTag = (UnityEngine.Object) component.Occupant != (UnityEngine.Object) null ? component.Occupant.PrefabID() : component.requestedEntityTag;
    if ((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null && this.Occupant.PrefabID() != entityTag)
      this.ClearOccupant();
    if (entityTag != this.requestedEntityTag && this.fetchChore != null)
      this.CancelActiveRequest();
    if (!(entityTag != Tag.Invalid))
      return;
    this.CreateOrder(entityTag, component.requestedEntityAdditionalFilterTag);
  }

  public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
  {
    base.CreateOrder(entityTag, additionalFilterTag);
    if (this.fetchChore == null)
      return;
    this.fetchChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
  }

  public void SetupLootSymbolObject()
  {
    Vector3 positionForDrops = this.capsule.GetStorePositionForDrops() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse)
    };
    GameObject go = new GameObject();
    go.name = "lootSymbol";
    go.transform.SetParent(this.transform, true);
    go.SetActive(false);
    go.transform.SetPosition(positionForDrops);
    KBatchedAnimTracker kbatchedAnimTracker = go.AddOrGet<KBatchedAnimTracker>();
    kbatchedAnimTracker.symbol = (HashedString) "loot";
    kbatchedAnimTracker.forceAlwaysAlive = true;
    kbatchedAnimTracker.matchParentOffset = true;
    this.lootKBAC = go.AddComponent<KBatchedAnimController>();
    this.lootKBAC.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "mushbar_kanim")
    };
    this.lootKBAC.initialAnim = "object";
    this.buildingAnimCtr.SetSymbolVisiblity((KAnimHashedString) "loot", false);
  }

  protected override void ClearOccupant()
  {
    this.LastCritterDead = (GameObject) null;
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
      this.UnsubscribeFromOccupant();
    this.originWorldID = -1;
    this.occupyingObject = (GameObject) null;
    this.UpdateActive();
    this.UpdateStatusItem();
    if (!this.isDoorOpen)
    {
      if (this.IsRocketOnGround)
      {
        this.SetLootSymbolImage(Tag.Invalid);
        this.capsule.OpenDoor();
      }
    }
    else
      this.capsule.DropInventory();
    this.Trigger(-731304873, (object) this.occupyingObject);
  }

  private void OnCritterStorageChanged(object obj)
  {
    if (obj == null || (double) this.storage.MassStored() != 0.0 || !((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null) || !((UnityEngine.Object) this.Occupant == (UnityEngine.Object) obj))
      return;
    this.ClearOccupant();
  }

  protected override void SubscribeToOccupant()
  {
    base.SubscribeToOccupant();
    this.Subscribe(this.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
    this.Subscribe(this.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
    this.Subscribe(this.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
    this.SetupCritterTracker();
    for (int index = 0; index < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; ++index)
      this.Occupant.AddTag(SpecialCargoBayClusterReceptacle.tagsForCritter[index]);
    this.Occupant.GetComponent<Health>().UpdateHealthBar();
  }

  protected override void UnsubscribeFromOccupant()
  {
    base.UnsubscribeFromOccupant();
    this.Unsubscribe(this.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
    this.Unsubscribe(this.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
    this.Unsubscribe(this.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
    this.RemoveCritterTracker();
    if (!((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null))
      return;
    for (int index = 0; index < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; ++index)
      this.occupyingObject.RemoveTag(SpecialCargoBayClusterReceptacle.tagsForCritter[index]);
    this.occupyingObject.GetComponent<Health>().UpdateHealthBar();
  }

  public void SetLootSymbolImage(Tag productTag)
  {
    bool flag = productTag != Tag.Invalid;
    this.lootKBAC.gameObject.SetActive(flag);
    if (!flag)
      return;
    this.lootKBAC.SwapAnims(Assets.GetPrefab((Tag) productTag.ToString()).GetComponent<KBatchedAnimController>().AnimFiles);
    this.lootKBAC.Play((HashedString) "object", KAnim.PlayMode.Loop);
  }

  private void SetupCritterTracker()
  {
    if (!((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null))
      return;
    KBatchedAnimTracker kbatchedAnimTracker = this.Occupant.AddOrGet<KBatchedAnimTracker>();
    kbatchedAnimTracker.symbol = (HashedString) "critter";
    kbatchedAnimTracker.forceAlwaysAlive = true;
    kbatchedAnimTracker.matchParentOffset = true;
  }

  private void RemoveCritterTracker()
  {
    if (!((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null))
      return;
    KBatchedAnimTracker component = this.Occupant.GetComponent<KBatchedAnimTracker>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }

  protected override void ConfigureOccupyingObject(GameObject source)
  {
    this.originWorldID = source.GetMyWorldId();
    source.GetComponent<Baggable>().SetWrangled();
    this.SetTrappedCritterAnimations(source);
  }

  private void OnBabyInStorageGrows(object obj)
  {
    int originWorldId = this.originWorldID;
    this.UnsubscribeFromOccupant();
    GameObject gameObject = (GameObject) obj;
    this.storage.Store(gameObject);
    this.occupyingObject = gameObject;
    this.ConfigureOccupyingObject(gameObject);
    this.originWorldID = originWorldId;
    this.PositionOccupyingObject();
    this.SubscribeToOccupant();
    this.UpdateStatusItem();
  }

  private void OnTrappedCritterTagsChanged(object obj)
  {
    if (!((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null) || !this.Occupant.HasTag(GameTags.Creatures.Die) || !((UnityEngine.Object) this.LastCritterDead != (UnityEngine.Object) this.Occupant))
      return;
    this.capsule.PlayDeathCloud();
    this.LastCritterDead = this.Occupant;
    this.RemoveCritterTracker();
    this.Occupant.GetComponent<KBatchedAnimController>().SetVisiblity(false);
    Butcherable component = this.Occupant.GetComponent<Butcherable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.drops != null && component.drops.Count > 0)
      this.SetLootSymbolImage(component.drops.Keys.ToList<string>()[0].ToTag());
    else
      this.SetLootSymbolImage(Tag.Invalid);
    if (!this.IsRocketInSpace)
      return;
    DeathStates.Instance smi = this.Occupant.GetSMI<DeathStates.Instance>();
    smi.GoTo((StateMachine.BaseState) smi.sm.pst);
  }

  private void OnCreatureInStorageDied(object drops_obj)
  {
    if (!(drops_obj is GameObject[] gameObjectArray))
      return;
    for (int index = 0; index < gameObjectArray.Length; ++index)
      this.sideProductStorage.Store(gameObjectArray[index]);
  }

  private void SetTrappedCritterAnimations(GameObject critter)
  {
    if (!((UnityEngine.Object) critter != (UnityEngine.Object) null))
      return;
    KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
    component.FlipX = false;
    component.Play((HashedString) "rocket_biological", KAnim.PlayMode.Loop);
    component.enabled = false;
    component.enabled = true;
  }

  protected override void PositionOccupyingObject()
  {
    if (!((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null))
      return;
    this.Occupant.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
    this.SetupCritterTracker();
  }

  protected override void UpdateStatusItem(KSelectable selectable)
  {
    bool flag = (UnityEngine.Object) this.Occupant != (UnityEngine.Object) null;
    if ((UnityEngine.Object) selectable != (UnityEngine.Object) null)
    {
      if (flag)
        selectable.AddStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, (object) this);
      else
        selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored);
    }
    base.UpdateStatusItem(selectable);
  }

  private void OnCargoBayRelocated(object data)
  {
    if (!((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null))
      return;
    KBatchedAnimController component = this.Occupant.GetComponent<KBatchedAnimController>();
    component.enabled = false;
    component.enabled = true;
  }

  private void OnRocketLanded(object data)
  {
    if ((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null)
    {
      ClusterManager.Instance.MigrateCritter(this.Occupant, this.gameObject.GetMyWorldId(), this.originWorldID);
      this.originWorldID = this.Occupant.GetMyWorldId();
    }
    if (!((UnityEngine.Object) this.Occupant == (UnityEngine.Object) null) || this.isDoorOpen)
      return;
    this.SetLootSymbolImage(Tag.Invalid);
    if ((double) this.sideProductStorage.MassStored() <= 0.0)
      return;
    this.capsule.OpenDoor();
  }

  public string GetBaggedAnimationName() => "rocket_biological";
}
