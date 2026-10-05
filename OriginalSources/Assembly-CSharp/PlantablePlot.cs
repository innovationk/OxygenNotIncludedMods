// Decompiled with JetBrains decompiler
// Type: PlantablePlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantablePlot : SingleEntityReceptacle, ISaveLoadable
{
  private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>((Action<PlantablePlot, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>((Action<PlantablePlot, object>) ((component, data) =>
  {
    if (!((UnityEngine.Object) component.plantRef.Get() != (UnityEngine.Object) null))
      return;
    component.plantRef.Get().Trigger(144050788, data);
  }));
  public bool IsOffGround;
  public Tag tagOnPlanted = Tag.Invalid;
  public Vector3 occupyingObjectVisualOffset = Vector3.zero;
  public Grid.SceneLayer plantLayer = Grid.SceneLayer.BuildingBack;
  [SerializeField]
  public bool has_liquid_pipe_input;
  [SerializeField]
  private bool accepts_fertilizer;
  [SerializeField]
  private bool accepts_irrigation = true;
  [Serialize]
  private Ref<KPrefabID> plantRef;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  [MyCmpGet]
  private RequireInputs requireInputs;
  private EntityPreview plantPreview;
  private static Action<object, object> OnOccupantUprootedDispatcher = (Action<object, object>) ((context, data) => Unsafe.As<PlantablePlot>(context).OnOccupantUprooted(data));
  private int onOccupantUprootedHandle = -1;

  public KPrefabID plant
  {
    get => this.plantRef.Get();
    set => this.plantRef.Set(value);
  }

  public bool ValidPlant => (UnityEngine.Object) this.plantPreview == (UnityEngine.Object) null || this.plantPreview.Valid;

  public bool AcceptsFertilizer => this.accepts_fertilizer;

  public bool AcceptsIrrigation => this.accepts_irrigation;

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (!DlcManager.FeaturePlantMutationsEnabled())
    {
      this.requestedEntityAdditionalFilterTag = Tag.Invalid;
    }
    else
    {
      if (!this.requestedEntityTag.IsValid || !this.requestedEntityAdditionalFilterTag.IsValid || PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag))
        return;
      this.requestedEntityAdditionalFilterTag = Tag.Invalid;
    }
  }

  protected override void OnPrefabInit()
  {
    this.gameObject.RemoveTag(GameTags.Decoration);
    base.OnPrefabInit();
    this.choreType = Db.Get().ChoreTypes.FarmFetch;
    this.statusItemNeed = Db.Get().BuildingStatusItems.NeedSeed;
    this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableSeed;
    this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingSeedDelivery;
    this.plantRef = new Ref<KPrefabID>();
    this.Subscribe<PlantablePlot>(-905833192, PlantablePlot.OnCopySettingsDelegate);
    this.Subscribe<PlantablePlot>(144050788, PlantablePlot.OnUpdateRoomDelegate);
    if (!this.HasTag(GameTags.FarmTiles))
      return;
    this.storage.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
    DropAllWorkable component1 = this.GetComponent<DropAllWorkable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
    Toggleable component2 = this.GetComponent<Toggleable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component2.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
  }

  protected override void OnSpawn()
  {
    if ((UnityEngine.Object) this.plant != (UnityEngine.Object) null)
      this.RegisterWithPlant(this.plant.gameObject);
    else
      this.ClearRequireInputs();
    base.OnSpawn();
    this.autoReplaceEntity = false;
    Components.PlantablePlots.Add(this.gameObject.GetMyWorldId(), this);
    this.GetComponent<Prioritizable>().onPriorityChanged += new Action<PrioritySetting>(this.SyncPriority);
  }

  private void OnCopySettings(object data)
  {
    PlantablePlot component1 = ((GameObject) data).GetComponent<PlantablePlot>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.occupyingObject == (UnityEngine.Object) null && (this.requestedEntityTag != component1.requestedEntityTag || this.requestedEntityAdditionalFilterTag != component1.requestedEntityAdditionalFilterTag || (UnityEngine.Object) component1.occupyingObject != (UnityEngine.Object) null))
    {
      Tag requestedEntityTag = component1.requestedEntityTag;
      Tag additionalFilterTag = component1.requestedEntityAdditionalFilterTag;
      if ((UnityEngine.Object) component1.occupyingObject != (UnityEngine.Object) null)
      {
        SeedProducer component2 = component1.occupyingObject.GetComponent<SeedProducer>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          requestedEntityTag = TagManager.Create(component2.seedInfo.seedId);
          MutantPlant component3 = component1.occupyingObject.GetComponent<MutantPlant>();
          additionalFilterTag = (bool) (UnityEngine.Object) component3 ? component3.SubSpeciesID : Tag.Invalid;
        }
      }
      GameObject prefab = Assets.GetPrefab(requestedEntityTag);
      IReceptacleDirection component4 = (UnityEngine.Object) prefab == (UnityEngine.Object) null ? (IReceptacleDirection) null : prefab.GetComponent<IReceptacleDirection>();
      if (component4 == null || component4.Direction == this.Direction)
      {
        this.CancelActiveRequest();
        this.CreateOrder(requestedEntityTag, additionalFilterTag);
      }
    }
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    Prioritizable component5 = this.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component5 != (UnityEngine.Object) null))
      return;
    Prioritizable component6 = this.occupyingObject.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component6 != (UnityEngine.Object) null))
      return;
    component6.SetMasterPriority(component5.GetMasterPriority());
  }

  public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
  {
    this.SetPreview(entityTag, false);
    if (this.ValidPlant)
      base.CreateOrder(entityTag, additionalFilterTag);
    else
      this.SetPreview(Tag.Invalid, false);
  }

  private void SyncPriority(PrioritySetting priority)
  {
    Prioritizable component1 = this.GetComponent<Prioritizable>();
    if (!object.Equals((object) component1.GetMasterPriority(), (object) priority))
      component1.SetMasterPriority(priority);
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    Prioritizable component2 = this.occupyingObject.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || object.Equals((object) component2.GetMasterPriority(), (object) priority))
      return;
    component2.SetMasterPriority(component1.GetMasterPriority());
  }

  public void SetFertilizationFlags(bool fertilizer, bool liquid_piping)
  {
    this.accepts_fertilizer = fertilizer;
    this.has_liquid_pipe_input = liquid_piping;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if ((UnityEngine.Object) this.plantPreview != (UnityEngine.Object) null)
      Util.KDestroyGameObject(this.plantPreview.gameObject);
    if ((bool) (UnityEngine.Object) this.occupyingObject)
      this.occupyingObject.Trigger(-216549700);
    Components.PlantablePlots.Remove(this.gameObject.GetMyWorldId(), this);
  }

  protected override GameObject SpawnOccupyingObject(GameObject depositedEntity)
  {
    PlantableSeed component1 = depositedEntity.GetComponent<PlantableSeed>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell((KMonoBehaviour) this), this.plantLayer);
      GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(component1.PlantID), posCbc, this.plantLayer);
      MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component1.GetComponent<MutantPlant>().CopyMutationsTo(component2);
      gameObject.SetActive(true);
      this.destroyEntityOnDeposit = true;
      return gameObject;
    }
    this.destroyEntityOnDeposit = false;
    return depositedEntity;
  }

  protected override void ConfigureOccupyingObject(GameObject newPlant)
  {
    this.plantRef.Set(newPlant.GetComponent<KPrefabID>());
    this.RegisterWithPlant(newPlant);
    UprootedMonitor component1 = newPlant.GetComponent<UprootedMonitor>();
    if ((bool) (UnityEngine.Object) component1)
      component1.canBeUprooted = false;
    this.autoReplaceEntity = false;
    Prioritizable component2 = this.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    Prioritizable component3 = newPlant.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null))
      return;
    component3.SetMasterPriority(component2.GetMasterPriority());
    component3.onPriorityChanged += new Action<PrioritySetting>(this.SyncPriority);
  }

  public void ReplacePlant(GameObject plant, bool keepStorage)
  {
    if (keepStorage)
    {
      this.UnsubscribeFromOccupant();
      this.occupyingObject = (GameObject) null;
    }
    this.ForceDeposit(plant);
  }

  protected override void PositionOccupyingObject()
  {
    base.PositionOccupyingObject();
    KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
    component.SetSceneLayer(this.plantLayer);
    this.OffsetAnim(component, this.occupyingObjectVisualOffset);
  }

  private void RegisterWithPlant(GameObject plant)
  {
    this.occupyingObject = plant;
    IrrigationMonitor.Def def = plant.GetComponent<StateMachineController>().GetDef<IrrigationMonitor.Def>();
    this.ApplyRequireInputs(def != null && def.consumedElements != null && def.consumedElements.Length != 0);
    ReceptacleMonitor component = plant.GetComponent<ReceptacleMonitor>();
    if ((bool) (UnityEngine.Object) component)
    {
      if (this.tagOnPlanted != Tag.Invalid)
        component.AddTag(this.tagOnPlanted);
      component.SetReceptacle(this);
    }
    plant.Trigger(1309017699, (object) this.storage);
  }

  protected override void SubscribeToOccupant()
  {
    base.SubscribeToOccupant();
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.onOccupantUprootedHandle = this.Subscribe(this.occupyingObject, -216549700, PlantablePlot.OnOccupantUprootedDispatcher, (object) this);
  }

  protected override void UnsubscribeFromOccupant()
  {
    base.UnsubscribeFromOccupant();
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.Unsubscribe(this.occupyingObject, ref this.onOccupantUprootedHandle);
  }

  private void ClearRequireInputs()
  {
    if (!((UnityEngine.Object) this.requireInputs != (UnityEngine.Object) null))
      return;
    this.requireInputs.RequiresInputConduit = false;
  }

  private void ApplyRequireInputs(bool irrigate)
  {
    if (!((UnityEngine.Object) this.requireInputs != (UnityEngine.Object) null))
      return;
    this.requireInputs.RequiresInputConduit = irrigate && this.AcceptsIrrigation;
  }

  private void OnOccupantUprooted(object data)
  {
    this.autoReplaceEntity = false;
    this.requestedEntityTag = Tag.Invalid;
    this.requestedEntityAdditionalFilterTag = Tag.Invalid;
    this.ClearRequireInputs();
  }

  public override void OrderRemoveOccupant()
  {
    if ((UnityEngine.Object) this.Occupant == (UnityEngine.Object) null)
      return;
    Uprootable component = this.Occupant.GetComponent<Uprootable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.MarkForUproot();
  }

  public override void SetPreview(Tag entityTag, bool solid = false)
  {
    PlantableSeed plantableSeed = (PlantableSeed) null;
    if (entityTag.IsValid)
    {
      GameObject prefab = Assets.GetPrefab(entityTag);
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        DebugUtil.LogWarningArgs((UnityEngine.Object) this.gameObject, (object) "Planter tried previewing a tag with no asset! If this was the 'Empty' tag, ignore it, that will go away in new save games. Otherwise... Eh? Tag was: ", (object) entityTag);
        return;
      }
      plantableSeed = prefab.GetComponent<PlantableSeed>();
    }
    if ((UnityEngine.Object) this.plantPreview != (UnityEngine.Object) null)
    {
      KPrefabID component = this.plantPreview.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) plantableSeed != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.PrefabTag == plantableSeed.PreviewID)
        return;
      this.plantPreview.gameObject.Unsubscribe(-1820564715, new Action<object>(this.OnValidChanged));
      Util.KDestroyGameObject(this.plantPreview.gameObject);
    }
    if (!((UnityEngine.Object) plantableSeed != (UnityEngine.Object) null))
      return;
    GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(plantableSeed.PreviewID), Grid.SceneLayer.Front);
    this.plantPreview = go.GetComponent<EntityPreview>();
    go.transform.SetPosition(Vector3.zero);
    go.transform.SetParent(this.gameObject.transform, false);
    go.transform.SetLocalPosition(Vector3.zero);
    Vector3 relativePosition = this.GetOccupyingObjectRelativePosition();
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
    {
      if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
        go.transform.SetLocalPosition(relativePosition);
      else if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
        go.transform.SetLocalPosition(Rotatable.GetRotatedOffset(relativePosition, Orientation.R90));
      else
        go.transform.SetLocalPosition(Rotatable.GetRotatedOffset(relativePosition, Orientation.R180));
    }
    else
      go.transform.SetLocalPosition(relativePosition);
    this.OffsetAnim(go.GetComponent<KBatchedAnimController>(), this.occupyingObjectVisualOffset);
    go.SetActive(true);
    go.Subscribe(-1820564715, new Action<object>(this.OnValidChanged));
    if (solid)
      this.plantPreview.SetSolid();
    this.plantPreview.UpdateValidity();
  }

  private void OffsetAnim(KBatchedAnimController kanim, Vector3 offset)
  {
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
      offset = this.rotatable.GetRotatedOffset(offset);
    kanim.Offset = offset;
  }

  private void OnValidChanged(object obj)
  {
    this.Trigger(-1820564715, obj);
    if (this.plantPreview.Valid || this.GetActiveRequest == null)
      return;
    this.CancelActiveRequest();
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.ENABLESDOMESTICGROWTH, (string) UI.BUILDINGEFFECTS.TOOLTIPS.ENABLESDOMESTICGROWTH);
    descriptors.Add(descriptor);
    return descriptors;
  }
}
