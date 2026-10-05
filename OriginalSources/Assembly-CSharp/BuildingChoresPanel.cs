// Decompiled with JetBrains decompiler
// Type: BuildingChoresPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BuildingChoresPanel : TargetPanel
{
  public GameObject choreGroupPrefab;
  public GameObject chorePrefab;
  public BuildingChoresPanelDupeRow dupePrefab;
  private GameObject detailsPanel;
  private DetailsPanelDrawer drawer;
  private HierarchyReferences choreGroup;
  private List<HierarchyReferences> choreEntries = new List<HierarchyReferences>();
  private int activeChoreEntries;
  private List<BuildingChoresPanelDupeRow> dupeEntries = new List<BuildingChoresPanelDupeRow>();
  private int activeDupeEntries;
  private List<BuildingChoresPanel.DupeEntryData> DupeEntryDatas = new List<BuildingChoresPanel.DupeEntryData>();

  public override bool IsValidForTarget(GameObject target)
  {
    KPrefabID component = target.GetComponent<KPrefabID>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.HasTag(GameTags.HasChores) && !component.HasTag(GameTags.BaseMinion);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreGroup = Util.KInstantiateUI<HierarchyReferences>(this.choreGroupPrefab, this.gameObject);
    this.choreGroup.gameObject.SetActive(true);
  }

  private void Update() => this.Refresh();

  protected override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    this.Refresh();
  }

  public override void OnDeselectTarget(GameObject target) => base.OnDeselectTarget(target);

  private void Refresh() => this.RefreshDetails();

  private void RefreshDetails()
  {
    int myParentWorldId = this.selectedTarget.GetMyParentWorldId();
    List<Chore> choreList = (List<Chore>) null;
    GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(myParentWorldId, out choreList);
    for (int index = 0; choreList != null && index < choreList.Count; ++index)
    {
      Chore chore = choreList[index];
      if (!chore.isNull && (UnityEngine.Object) chore.gameObject == (UnityEngine.Object) this.selectedTarget)
        this.AddChoreEntry(chore);
    }
    List<FetchChore> fetchChoreList = (List<FetchChore>) null;
    GlobalChoreProvider.Instance.fetchMap.TryGetValue(myParentWorldId, out fetchChoreList);
    for (int index = 0; fetchChoreList != null && index < fetchChoreList.Count; ++index)
    {
      FetchChore fetchChore = fetchChoreList[index];
      if (!fetchChore.isNull && (UnityEngine.Object) fetchChore.gameObject == (UnityEngine.Object) this.selectedTarget)
        this.AddChoreEntry((Chore) fetchChore);
    }
    for (int activeDupeEntries = this.activeDupeEntries; activeDupeEntries < this.dupeEntries.Count; ++activeDupeEntries)
      this.dupeEntries[activeDupeEntries].gameObject.SetActive(false);
    this.activeDupeEntries = 0;
    for (int activeChoreEntries = this.activeChoreEntries; activeChoreEntries < this.choreEntries.Count; ++activeChoreEntries)
      this.choreEntries[activeChoreEntries].gameObject.SetActive(false);
    this.activeChoreEntries = 0;
  }

  private void AddChoreEntry(Chore chore)
  {
    HierarchyReferences choreEntry = this.GetChoreEntry(GameUtil.GetChoreName(chore, (object) null), chore.choreType, this.choreGroup.GetReference<RectTransform>("EntriesContainer"));
    FetchChore fetch = chore as FetchChore;
    ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      gameObjectList.Add(minionIdentity.gameObject);
    foreach (RobotAi.Instance instance in Components.LiveRobotsIdentities.Items)
      gameObjectList.Add(instance.gameObject);
    foreach (GameObject gameObject in gameObjectList)
    {
      pooledList.Clear();
      ChoreConsumer component = gameObject.GetComponent<ChoreConsumer>();
      Chore.Precondition.Context context = new Chore.Precondition.Context();
      ChoreConsumer.PreconditionSnapshot preconditionSnapshot = component.GetLastPreconditionSnapshot();
      if (preconditionSnapshot.doFailedContextsNeedSorting)
      {
        preconditionSnapshot.failedContexts.Sort();
        preconditionSnapshot.doFailedContextsNeedSorting = false;
      }
      pooledList.AddRange((IEnumerable<Chore.Precondition.Context>) preconditionSnapshot.failedContexts);
      pooledList.AddRange((IEnumerable<Chore.Precondition.Context>) preconditionSnapshot.succeededContexts);
      int num1 = -1;
      int num2 = 0;
      for (int index = pooledList.Count - 1; index >= 0; --index)
      {
        if (!((UnityEngine.Object) pooledList[index].chore.driver != (UnityEngine.Object) null) || !((UnityEngine.Object) pooledList[index].chore.driver != (UnityEngine.Object) component.choreDriver))
        {
          bool flag = pooledList[index].IsPotentialSuccess();
          if (flag)
            ++num2;
          FetchAreaChore chore1 = pooledList[index].chore as FetchAreaChore;
          if (pooledList[index].chore == chore || fetch != null && chore1 != null && chore1.smi.SameDestination(fetch))
          {
            num1 = flag ? num2 : int.MaxValue;
            context = pooledList[index];
            break;
          }
        }
      }
      if (num1 >= 0)
        this.DupeEntryDatas.Add(new BuildingChoresPanel.DupeEntryData()
        {
          consumer = component,
          context = context,
          personalPriority = component.GetPersonalPriority(chore.choreType),
          rank = num1
        });
    }
    pooledList.Recycle();
    this.DupeEntryDatas.Sort();
    foreach (BuildingChoresPanel.DupeEntryData dupeEntryData in this.DupeEntryDatas)
      this.GetDupeEntry(dupeEntryData, choreEntry.GetReference<RectTransform>("DupeContainer"));
    this.DupeEntryDatas.Clear();
  }

  private HierarchyReferences GetChoreEntry(
    string label,
    ChoreType choreType,
    RectTransform parent)
  {
    HierarchyReferences choreEntry;
    if (this.activeChoreEntries >= this.choreEntries.Count)
    {
      choreEntry = Util.KInstantiateUI<HierarchyReferences>(this.chorePrefab, parent.gameObject);
      this.choreEntries.Add(choreEntry);
    }
    else
    {
      choreEntry = this.choreEntries[this.activeChoreEntries];
      choreEntry.transform.SetParent((Transform) parent);
      choreEntry.transform.SetAsLastSibling();
    }
    ++this.activeChoreEntries;
    choreEntry.GetReference<LocText>("ChoreLabel").text = label;
    choreEntry.GetReference<LocText>("ChoreSubLabel").text = GameUtil.ChoreGroupsForChoreType(choreType);
    Image reference1 = choreEntry.GetReference<Image>("Icon");
    if (choreType.groups.Length != 0)
    {
      Sprite sprite = Assets.GetSprite((HashedString) choreType.groups[0].sprite);
      reference1.sprite = sprite;
      reference1.gameObject.SetActive(true);
      reference1.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, (object) choreType.groups[0].Name);
    }
    else
      reference1.gameObject.SetActive(false);
    Image reference2 = choreEntry.GetReference<Image>("Icon2");
    if (choreType.groups.Length > 1)
    {
      Sprite sprite = Assets.GetSprite((HashedString) choreType.groups[1].sprite);
      reference2.sprite = sprite;
      reference2.gameObject.SetActive(true);
      reference2.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, (object) choreType.groups[1].Name);
    }
    else
      reference2.gameObject.SetActive(false);
    choreEntry.gameObject.SetActive(true);
    return choreEntry;
  }

  private BuildingChoresPanelDupeRow GetDupeEntry(
    BuildingChoresPanel.DupeEntryData data,
    RectTransform parent)
  {
    BuildingChoresPanelDupeRow dupeEntry;
    if (this.activeDupeEntries >= this.dupeEntries.Count)
    {
      dupeEntry = Util.KInstantiateUI<BuildingChoresPanelDupeRow>(this.dupePrefab.gameObject, parent.gameObject);
      this.dupeEntries.Add(dupeEntry);
    }
    else
    {
      dupeEntry = this.dupeEntries[this.activeDupeEntries];
      dupeEntry.transform.SetParent((Transform) parent);
      dupeEntry.transform.SetAsLastSibling();
    }
    ++this.activeDupeEntries;
    dupeEntry.Init(data);
    dupeEntry.gameObject.SetActive(true);
    return dupeEntry;
  }

  public class DupeEntryData : IComparable<BuildingChoresPanel.DupeEntryData>
  {
    public ChoreConsumer consumer;
    public Chore.Precondition.Context context;
    public int personalPriority;
    public int rank;

    public int CompareTo(BuildingChoresPanel.DupeEntryData other)
    {
      if (this.personalPriority != other.personalPriority)
        return other.personalPriority.CompareTo(this.personalPriority);
      if (this.rank != other.rank)
        return this.rank.CompareTo(other.rank);
      return this.consumer.GetProperName() != other.consumer.GetProperName() ? this.consumer.GetProperName().CompareTo(other.consumer.GetProperName()) : this.consumer.GetInstanceID().CompareTo(other.consumer.GetInstanceID());
    }
  }
}
