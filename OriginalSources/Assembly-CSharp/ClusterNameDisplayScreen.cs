// Decompiled with JetBrains decompiler
// Type: ClusterNameDisplayScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClusterNameDisplayScreen : KScreen
{
  public static ClusterNameDisplayScreen Instance;
  public GameObject nameAndBarsPrefab;
  [SerializeField]
  private Color selectedColor;
  [SerializeField]
  private Color defaultColor;
  private List<ClusterNameDisplayScreen.Entry> m_entries = new List<ClusterNameDisplayScreen.Entry>();
  private List<KCollider2D> workingList = new List<KCollider2D>();

  public static void DestroyInstance()
  {
    ClusterNameDisplayScreen.Instance = (ClusterNameDisplayScreen) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ClusterNameDisplayScreen.Instance = this;
  }

  protected override void OnSpawn() => base.OnSpawn();

  public void AddNewEntry(ClusterGridEntity representedObject)
  {
    if (this.GetEntry(representedObject) != null)
      return;
    ClusterNameDisplayScreen.Entry entry = new ClusterNameDisplayScreen.Entry();
    entry.grid_entity = representedObject;
    GameObject gameObject = Util.KInstantiateUI(this.nameAndBarsPrefab, this.gameObject, true);
    entry.display_go = gameObject;
    gameObject.name = representedObject.name + " cluster overlay";
    entry.Name = representedObject.name;
    entry.refs = gameObject.GetComponent<HierarchyReferences>();
    entry.bars_go = entry.refs.GetReference<RectTransform>("Bars").gameObject;
    this.m_entries.Add(entry);
    if (!((UnityEngine.Object) representedObject.GetComponent<KSelectable>() != (UnityEngine.Object) null))
      return;
    this.UpdateName(representedObject);
    this.UpdateBars(representedObject);
  }

  private void LateUpdate()
  {
    if (App.isLoading || App.IsExiting)
      return;
    int count = this.m_entries.Count;
    int index = 0;
    while (index < count)
    {
      if ((UnityEngine.Object) this.m_entries[index].grid_entity != (UnityEngine.Object) null && ClusterMapScreen.GetRevealLevel(this.m_entries[index].grid_entity) == ClusterRevealLevel.Visible)
      {
        Transform entityNameTarget = ClusterMapScreen.Instance.GetGridEntityNameTarget(this.m_entries[index].grid_entity);
        if ((UnityEngine.Object) entityNameTarget != (UnityEngine.Object) null)
        {
          Vector3 position = entityNameTarget.GetPosition();
          this.m_entries[index].display_go.GetComponent<RectTransform>().SetPositionAndRotation(position, Quaternion.identity);
          this.m_entries[index].display_go.SetActive(this.m_entries[index].grid_entity.IsVisible && this.m_entries[index].grid_entity.ShowName());
        }
        else if (this.m_entries[index].display_go.activeSelf)
          this.m_entries[index].display_go.SetActive(false);
        this.UpdateBars(this.m_entries[index].grid_entity);
        if ((UnityEngine.Object) this.m_entries[index].bars_go != (UnityEngine.Object) null)
        {
          this.m_entries[index].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
          foreach (KCollider2D working in this.workingList)
            working.MarkDirty();
        }
        ++index;
      }
      else
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_entries[index].display_go);
        --count;
        this.m_entries[index] = this.m_entries[count];
      }
    }
    this.m_entries.RemoveRange(count, this.m_entries.Count - count);
  }

  public void UpdateName(ClusterGridEntity representedObject)
  {
    ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
    if (entry == null)
      return;
    KSelectable component = representedObject.GetComponent<KSelectable>();
    entry.display_go.name = component.GetProperName() + " cluster overlay";
    LocText componentInChildren = entry.display_go.GetComponentInChildren<LocText>();
    if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null))
      return;
    componentInChildren.text = component.GetProperName();
  }

  private void UpdateBars(ClusterGridEntity representedObject)
  {
    ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
    if (entry == null)
      return;
    GenericUIProgressBar componentInChildren = entry.bars_go.GetComponentInChildren<GenericUIProgressBar>(true);
    if (entry.grid_entity.ShowProgressBar())
    {
      if (!componentInChildren.gameObject.activeSelf)
        componentInChildren.gameObject.SetActive(true);
      componentInChildren.SetFillPercentage(entry.grid_entity.GetProgress());
    }
    else
    {
      if (!componentInChildren.gameObject.activeSelf)
        return;
      componentInChildren.gameObject.SetActive(false);
    }
  }

  private ClusterNameDisplayScreen.Entry GetEntry(ClusterGridEntity entity)
  {
    return this.m_entries.Find((Predicate<ClusterNameDisplayScreen.Entry>) (entry => (UnityEngine.Object) entry.grid_entity == (UnityEngine.Object) entity));
  }

  private class Entry
  {
    public string Name;
    public ClusterGridEntity grid_entity;
    public GameObject display_go;
    public GameObject bars_go;
    public HierarchyReferences refs;
  }
}
