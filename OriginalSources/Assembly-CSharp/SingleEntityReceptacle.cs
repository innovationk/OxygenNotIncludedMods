// Decompiled with JetBrains decompiler
// Type: SingleEntityReceptacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/SingleEntityReceptacle")]
public class SingleEntityReceptacle : Workable, IRender1000ms
{
  [MyCmpGet]
  public Operational operational;
  [MyCmpReq]
  protected Storage storage;
  [MyCmpGet]
  public Rotatable rotatable;
  protected FetchChore fetchChore;
  public ChoreType choreType = Db.Get().ChoreTypes.Fetch;
  [Serialize]
  public bool autoReplaceEntity;
  [Serialize]
  public Tag requestedEntityTag;
  [Serialize]
  public Tag requestedEntityAdditionalFilterTag;
  [Serialize]
  protected Ref<KSelectable> occupyObjectRef = new Ref<KSelectable>();
  [SerializeField]
  private List<Tag> possibleDepositTagsList = new List<Tag>();
  [SerializeField]
  private List<Func<GameObject, bool>> additionalCriteria = new List<Func<GameObject, bool>>();
  [SerializeField]
  protected bool destroyEntityOnDeposit;
  [SerializeField]
  protected SingleEntityReceptacle.ReceptacleDirection direction;
  public static Dictionary<Tag, List<SingleEntityReceptacle.CustomPositionData>> CustomOccupyingObjectRelativePosition = new Dictionary<Tag, List<SingleEntityReceptacle.CustomPositionData>>();
  public Vector3 occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 3f);
  private Tag selfPrefabID;
  protected StatusItem statusItemAwaitingDelivery;
  protected StatusItem statusItemNeed;
  protected StatusItem statusItemNoneAvailable;
  private static readonly EventSystem.IntraObjectHandler<SingleEntityReceptacle> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SingleEntityReceptacle>((Action<SingleEntityReceptacle, object>) ((component, data) => component.OnOperationalChanged(data)));

  public FetchChore GetActiveRequest => this.fetchChore;

  protected GameObject occupyingObject
  {
    get
    {
      return (UnityEngine.Object) this.occupyObjectRef.Get() != (UnityEngine.Object) null ? this.occupyObjectRef.Get().gameObject : (GameObject) null;
    }
    set
    {
      if ((UnityEngine.Object) value == (UnityEngine.Object) null)
        this.occupyObjectRef.Set((KSelectable) null);
      else
        this.occupyObjectRef.Set(value.GetComponent<KSelectable>());
    }
  }

  public GameObject Occupant => this.occupyingObject;

  public IReadOnlyList<Tag> possibleDepositObjectTags
  {
    get => (IReadOnlyList<Tag>) this.possibleDepositTagsList;
  }

  public bool HasDepositTag(Tag tag) => this.possibleDepositTagsList.Contains(tag);

  public bool IsValidEntity(GameObject candidate)
  {
    if (!Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) candidate.GetComponent<KPrefabID>()))
      return false;
    IReceptacleDirection component = candidate.GetComponent<IReceptacleDirection>();
    bool flag = this.Direction == SingleEntityReceptacle.ReceptacleDirection.Any || (UnityEngine.Object) this.rotatable != (UnityEngine.Object) null || component == null || component.Direction == this.Direction;
    for (int index = 0; flag && index < this.additionalCriteria.Count; ++index)
      flag = this.additionalCriteria[index](candidate);
    return flag;
  }

  public SingleEntityReceptacle.ReceptacleDirection Direction => this.direction;

  protected Vector3 GetOccupyingObjectRelativePosition()
  {
    if ((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null && SingleEntityReceptacle.CustomOccupyingObjectRelativePosition.ContainsKey(this.selfPrefabID))
    {
      KPrefabID component = this.Occupant.GetComponent<KPrefabID>();
      foreach (SingleEntityReceptacle.CustomPositionData customPositionData in SingleEntityReceptacle.CustomOccupyingObjectRelativePosition[this.selfPrefabID])
      {
        if (component.HasTag(customPositionData.tag))
          return customPositionData.pos;
      }
    }
    return this.occupyingObjectRelativePosition;
  }

  protected override void OnPrefabInit()
  {
    this.selfPrefabID = this.gameObject.PrefabID();
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
    {
      this.PositionOccupyingObject();
      this.SubscribeToOccupant();
    }
    this.UpdateStatusItem();
    if ((UnityEngine.Object) this.occupyingObject == (UnityEngine.Object) null && !this.requestedEntityTag.IsValid)
      this.requestedEntityAdditionalFilterTag = (Tag) (string) null;
    if ((UnityEngine.Object) this.occupyingObject == (UnityEngine.Object) null && this.requestedEntityTag.IsValid)
      this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
    this.Subscribe<SingleEntityReceptacle>(-592767678, SingleEntityReceptacle.OnOperationalChangedDelegate);
    this.TriggerReceptacleOperationalSignal();
  }

  public void AddDepositTag(Tag t) => this.possibleDepositTagsList.Add(t);

  public void AddAdditionalCriteria(Func<GameObject, bool> criteria)
  {
    this.additionalCriteria.Add(criteria);
  }

  public void SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection d)
  {
    this.direction = d;
  }

  public virtual void SetPreview(Tag entityTag, bool solid = false)
  {
  }

  public virtual void CreateOrder(Tag entityTag, Tag additionalFilterTag)
  {
    this.requestedEntityTag = entityTag;
    this.requestedEntityAdditionalFilterTag = additionalFilterTag;
    this.CreateFetchChore(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
    this.SetPreview(entityTag, true);
    this.UpdateStatusItem();
  }

  public virtual void Render1000ms(float dt) => this.UpdateStatusItem();

  protected void UpdateStatusItem() => this.UpdateStatusItem(this.GetComponent<KSelectable>());

  protected virtual void UpdateStatusItem(KSelectable selectable)
  {
    if ((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null)
      selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, (StatusItem) null);
    else if (this.fetchChore != null)
    {
      bool flag = (UnityEngine.Object) this.fetchChore.fetcher != (UnityEngine.Object) null;
      WorldContainer myWorld = this.GetMyWorld();
      if (!flag && (UnityEngine.Object) myWorld != (UnityEngine.Object) null)
      {
        foreach (Tag tag in this.fetchChore.tags)
        {
          if ((double) myWorld.worldInventory.GetTotalAmount(tag, true) > 0.0)
          {
            if ((double) myWorld.worldInventory.GetTotalAmount(this.requestedEntityAdditionalFilterTag, true) <= 0.0)
            {
              if (!(this.requestedEntityAdditionalFilterTag == Tag.Invalid))
                break;
            }
            flag = true;
            break;
          }
        }
      }
      if (flag)
        selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemAwaitingDelivery);
      else
        selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNoneAvailable);
    }
    else
      selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNeed);
  }

  protected void CreateFetchChore(Tag entityTag, Tag additionalRequiredTag)
  {
    if (this.fetchChore != null || !entityTag.IsValid || !(entityTag != GameTags.Empty))
      return;
    ChoreType choreType = this.choreType;
    Storage storage = this.storage;
    double prefabFetchMass = (double) this.GetPrefabFetchMass(entityTag);
    HashSet<Tag> tags = new HashSet<Tag>();
    tags.Add(entityTag);
    Tag required_tag = !additionalRequiredTag.IsValid || !(additionalRequiredTag != GameTags.Empty) ? Tag.Invalid : additionalRequiredTag;
    Action<Chore> on_complete = new Action<Chore>(this.OnFetchComplete);
    Action<Chore> on_begin = (Action<Chore>) (chore => this.UpdateStatusItem());
    Action<Chore> on_end = (Action<Chore>) (chore => this.UpdateStatusItem());
    this.fetchChore = new FetchChore(choreType, storage, (float) prefabFetchMass, tags, FetchChore.MatchCriteria.MatchID, required_tag, on_complete: on_complete, on_begin: on_begin, on_end: on_end, operational_requirement: Operational.State.Functional);
    MaterialNeeds.UpdateNeed(this.requestedEntityTag, 1f, this.gameObject.GetMyWorldId());
    this.UpdateStatusItem();
  }

  private float GetPrefabFetchMass(Tag entityTag)
  {
    GameObject prefab = Assets.GetPrefab(entityTag);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      PrimaryElement component = prefab.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component.MassPerUnit;
    }
    KCrashReporter.ReportDevNotification($"SingleEntityReceptacle {this.name} is requesting {entityTag.Name} which is not an entity", Environment.StackTrace);
    return 1f;
  }

  public virtual void OrderRemoveOccupant() => this.ClearOccupant();

  protected virtual void ClearOccupant()
  {
    if ((bool) (UnityEngine.Object) this.occupyingObject)
    {
      this.UnsubscribeFromOccupant();
      this.storage.DropAll();
    }
    this.occupyingObject = (GameObject) null;
    this.UpdateActive();
    this.UpdateStatusItem();
    this.Trigger(-731304873, (object) this.occupyingObject);
  }

  public void CancelActiveRequest()
  {
    if (this.fetchChore != null)
    {
      MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, this.gameObject.GetMyWorldId());
      this.fetchChore.Cancel("User canceled");
      this.fetchChore = (FetchChore) null;
    }
    this.requestedEntityTag = Tag.Invalid;
    this.requestedEntityAdditionalFilterTag = Tag.Invalid;
    this.UpdateStatusItem();
    this.SetPreview(Tag.Invalid);
  }

  private void OnOccupantDestroyed(object data)
  {
    this.occupyingObject = (GameObject) null;
    this.ClearOccupant();
    if (!this.autoReplaceEntity || !this.requestedEntityTag.IsValid || !(this.requestedEntityTag != GameTags.Empty))
      return;
    this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
  }

  protected virtual void SubscribeToOccupant()
  {
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.Subscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
  }

  protected virtual void UnsubscribeFromOccupant()
  {
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.Unsubscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
  }

  private void OnFetchComplete(Chore chore)
  {
    if (this.fetchChore == null)
      Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} OnFetchComplete fetchChore null", (object) this.gameObject);
    else if ((UnityEngine.Object) this.fetchChore.fetchTarget == (UnityEngine.Object) null)
      Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} OnFetchComplete fetchChore.fetchTarget null", (object) this.gameObject);
    else
      this.OnDepositObject(this.fetchChore.fetchTarget.gameObject);
  }

  public void ForceDeposit(GameObject depositedObject)
  {
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
      this.ClearOccupant();
    this.OnDepositObject(depositedObject);
  }

  protected virtual void OnDepositObject(GameObject depositedObject)
  {
    this.SetPreview(Tag.Invalid);
    MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, this.gameObject.GetMyWorldId());
    KBatchedAnimController component = depositedObject.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.GetBatchInstanceData().ClearOverrideTransformMatrix();
    this.occupyingObject = this.SpawnOccupyingObject(depositedObject);
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
    {
      this.ConfigureOccupyingObject(this.occupyingObject);
      this.occupyingObject.SetActive(true);
      this.PositionOccupyingObject();
      this.SubscribeToOccupant();
    }
    else
      Debug.LogWarning((object) (this.gameObject.name + " EntityReceptacle did not spawn occupying entity."));
    if (this.fetchChore != null)
    {
      this.fetchChore.Cancel("receptacle filled");
      this.fetchChore = (FetchChore) null;
    }
    if (!this.autoReplaceEntity)
    {
      this.requestedEntityTag = Tag.Invalid;
      this.requestedEntityAdditionalFilterTag = Tag.Invalid;
    }
    this.UpdateActive();
    this.UpdateStatusItem();
    if (this.destroyEntityOnDeposit)
      Util.KDestroyGameObject(depositedObject);
    this.Trigger(-731304873, (object) this.occupyingObject);
  }

  protected virtual GameObject SpawnOccupyingObject(GameObject depositedEntity) => depositedEntity;

  protected virtual void ConfigureOccupyingObject(GameObject source)
  {
  }

  protected virtual void PositionOccupyingObject()
  {
    Vector3 relativePosition = this.GetOccupyingObjectRelativePosition();
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
      this.occupyingObject.transform.SetPosition(this.gameObject.transform.GetPosition() + this.rotatable.GetRotatedOffset(relativePosition));
    else
      this.occupyingObject.transform.SetPosition(this.gameObject.transform.GetPosition() + relativePosition);
    KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
    component.enabled = false;
    component.enabled = true;
  }

  protected void UpdateActive()
  {
    if (this.Equals((object) null) || (UnityEngine.Object) this == (UnityEngine.Object) null || this.gameObject.Equals((object) null) || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !((UnityEngine.Object) this.operational != (UnityEngine.Object) null))
      return;
    this.operational.SetActive(this.operational.IsOperational && (UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null);
  }

  protected override void OnCleanUp()
  {
    this.CancelActiveRequest();
    this.UnsubscribeFromOccupant();
    base.OnCleanUp();
  }

  protected virtual void OnOperationalChanged(object data)
  {
    this.UpdateActive();
    this.TriggerReceptacleOperationalSignal();
  }

  private void TriggerReceptacleOperationalSignal()
  {
    if ((UnityEngine.Object) this.operational == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) this.occupyingObject)
      return;
    this.occupyingObject.Trigger(this.operational.IsOperational ? 1628751838 : 960378201);
  }

  public struct CustomPositionData(Tag t, Vector3 p)
  {
    public Tag tag = t;
    public Vector3 pos = p;
  }

  public enum ReceptacleDirection
  {
    Top,
    Side,
    Bottom,
    Any,
  }
}
