// Decompiled with JetBrains decompiler
// Type: RelatedEntitiesSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RelatedEntitiesSideScreen : SideScreenContent, ISim1000ms
{
  private GameObject target;
  private IRelatedEntities targetRelatedEntitiesComponent;
  public GameObject rowPrefab;
  public RectTransform rowContainer;
  public Dictionary<KSelectable, GameObject> rows = new Dictionary<KSelectable, GameObject>();
  private int uiRefreshSubHandle = -1;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.rowPrefab.SetActive(false);
    if (!show)
      return;
    this.RefreshOptions();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IRelatedEntities>() != null;
  }

  public override void SetTarget(GameObject target)
  {
    this.target = target;
    this.targetRelatedEntitiesComponent = target.GetComponent<IRelatedEntities>();
    this.RefreshOptions();
    this.uiRefreshSubHandle = Game.Instance.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
  }

  public override void ClearTarget()
  {
    if (this.uiRefreshSubHandle == -1 || this.targetRelatedEntitiesComponent == null)
      return;
    Game.Instance.Unsubscribe(this.uiRefreshSubHandle);
    this.uiRefreshSubHandle = -1;
  }

  private void RefreshOptions(object data = null)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.ClearRows();
    foreach (KSelectable relatedEntity in this.targetRelatedEntitiesComponent.GetRelatedEntities())
      this.AddRow(relatedEntity);
  }

  private void ClearRows()
  {
    for (int index = this.rowContainer.childCount - 1; index >= 0; --index)
      Util.KDestroyGameObject((Component) this.rowContainer.GetChild(index));
    this.rows.Clear();
  }

  private void AddRow(KSelectable entity)
  {
    GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
    gameObject.GetComponent<KButton>().onClick += (System.Action) (() => SelectTool.Instance.SelectAndFocus(entity.transform.position, entity));
    HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
    component.GetReference<LocText>("label").SetText((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) entity ? $"<b>{entity.GetProperName()}</b>" : entity.GetProperName());
    component.GetReference<Image>("icon").sprite = Def.GetUISprite((object) entity.gameObject).first;
    this.rows.Add(entity, gameObject);
    this.RefreshMainStatus(entity);
  }

  private void RefreshMainStatus(KSelectable entity)
  {
    if (entity.IsNullOrDestroyed() || !this.rows.ContainsKey(entity))
      return;
    HierarchyReferences component = this.rows[entity].GetComponent<HierarchyReferences>();
    StatusItemGroup.Entry statusItem = entity.GetStatusItem(Db.Get().StatusItemCategories.Main);
    LocText reference = component.GetReference<LocText>("status");
    if (statusItem.data != null)
    {
      reference.gameObject.SetActive(true);
      reference.SetText(statusItem.item.GetName(statusItem.data));
    }
    else
    {
      reference.gameObject.SetActive(false);
      reference.SetText("");
    }
  }

  public void Sim1000ms(float dt)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    foreach (KeyValuePair<KSelectable, GameObject> row in this.rows)
      this.RefreshMainStatus(row.Key);
  }
}
