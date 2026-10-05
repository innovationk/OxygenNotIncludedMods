// Decompiled with JetBrains decompiler
// Type: CheckboxListGroupSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

#nullable disable
public class CheckboxListGroupSideScreen : SideScreenContent
{
  public const int DefaultCheckboxListSideScreenSortOrder = 20;
  private ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer> checkboxContainerPool;
  private GameObjectPool checkboxPool;
  [SerializeField]
  private GameObject checkboxGroupPrefab;
  [SerializeField]
  private GameObject checkboxPrefab;
  [SerializeField]
  private RectTransform groupParent;
  [SerializeField]
  private RectTransform checkboxParent;
  [SerializeField]
  private LocText descriptionLabel;
  private List<ICheckboxListGroupControl> targets;
  private GameObject currentBuildTarget;
  private int uiRefreshSubHandle = -1;
  private List<CheckboxListGroupSideScreen.CheckboxContainer> activeChecklistGroups = new List<CheckboxListGroupSideScreen.CheckboxContainer>();

  private CheckboxListGroupSideScreen.CheckboxContainer InstantiateCheckboxContainer()
  {
    return new CheckboxListGroupSideScreen.CheckboxContainer(Util.KInstantiateUI(this.checkboxGroupPrefab, this.groupParent.gameObject, true).GetComponent<HierarchyReferences>());
  }

  private GameObject InstantiateCheckbox()
  {
    return Util.KInstantiateUI(this.checkboxPrefab, this.checkboxParent.gameObject);
  }

  protected override void OnSpawn()
  {
    this.checkboxPrefab.SetActive(false);
    this.checkboxGroupPrefab.SetActive(false);
    base.OnSpawn();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    ICheckboxListGroupControl[] components = target.GetComponents<ICheckboxListGroupControl>();
    if (components != null)
    {
      foreach (ICheckboxListGroupControl listGroupControl in components)
      {
        if (listGroupControl.SidescreenEnabled())
          return true;
      }
    }
    foreach (ICheckboxListGroupControl listGroupControl in target.GetAllSMI<ICheckboxListGroupControl>())
    {
      if (listGroupControl.SidescreenEnabled())
        return true;
    }
    return false;
  }

  public override int GetSideScreenSortOrder()
  {
    return this.targets == null ? 20 : this.targets[0].CheckboxSideScreenSortOrder();
  }

  public override void SetTarget(GameObject target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.targets = target.GetAllSMI<ICheckboxListGroupControl>();
      this.targets.AddRange((IEnumerable<ICheckboxListGroupControl>) target.GetComponents<ICheckboxListGroupControl>());
      this.Rebuild(target);
      this.uiRefreshSubHandle = this.currentBuildTarget.Subscribe(1980521255, new Action<object>(this.Refresh));
    }
  }

  public override void ClearTarget()
  {
    if (this.uiRefreshSubHandle != -1 && (UnityEngine.Object) this.currentBuildTarget != (UnityEngine.Object) null)
    {
      this.currentBuildTarget.Unsubscribe(this.uiRefreshSubHandle);
      this.uiRefreshSubHandle = -1;
    }
    this.ReleaseContainers(this.activeChecklistGroups.Count);
  }

  public override string GetTitle()
  {
    return this.targets != null && this.targets.Count > 0 && this.targets[0] != null ? this.targets[0].Title : base.GetTitle();
  }

  private void Rebuild(GameObject buildTarget)
  {
    if (this.checkboxContainerPool == null)
    {
      this.checkboxContainerPool = new ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer>(new Func<CheckboxListGroupSideScreen.CheckboxContainer>(this.InstantiateCheckboxContainer), collectionCheck: false);
      this.checkboxPool = new GameObjectPool(new Func<GameObject>(this.InstantiateCheckbox), (Action<GameObject>) (_ => { }));
    }
    this.descriptionLabel.enabled = !this.targets[0].Description.IsNullOrWhiteSpace();
    if (!this.targets[0].Description.IsNullOrWhiteSpace())
      this.descriptionLabel.SetText(this.targets[0].Description);
    if ((UnityEngine.Object) buildTarget == (UnityEngine.Object) this.currentBuildTarget)
    {
      this.Refresh();
    }
    else
    {
      this.currentBuildTarget = buildTarget;
      foreach (ICheckboxListGroupControl target in this.targets)
      {
        foreach (ICheckboxListGroupControl.ListGroup group in target.GetData())
        {
          CheckboxListGroupSideScreen.CheckboxContainer groupUI = this.checkboxContainerPool.Get();
          this.InitContainer(target, group, groupUI);
        }
      }
    }
  }

  [ContextMenu("Force refresh")]
  private void Test() => this.Refresh();

  private void Refresh(object data = null)
  {
    int num = 0;
    foreach (ICheckboxListGroupControl target in this.targets)
    {
      foreach (ICheckboxListGroupControl.ListGroup group in target.GetData())
      {
        if (++num > this.activeChecklistGroups.Count)
          this.InitContainer(target, group, this.checkboxContainerPool.Get());
        CheckboxListGroupSideScreen.CheckboxContainer activeChecklistGroup = this.activeChecklistGroups[num - 1];
        if (group.resolveTitleCallback != null)
          activeChecklistGroup.container.GetReference<LocText>("Text").SetText(group.resolveTitleCallback(group.title));
        for (int index = 0; index < group.checkboxItems.Length; ++index)
        {
          ICheckboxListGroupControl.CheckboxItem checkboxItem = group.checkboxItems[index];
          if (activeChecklistGroup.checkboxUIItems.Count <= index)
            this.CreateSingleCheckBoxForGroupUI(activeChecklistGroup);
          this.SetCheckboxData(activeChecklistGroup.checkboxUIItems[index], checkboxItem, target);
        }
        while (activeChecklistGroup.checkboxUIItems.Count > group.checkboxItems.Length)
          this.RemoveSingleCheckboxFromContainer(activeChecklistGroup.checkboxUIItems[activeChecklistGroup.checkboxUIItems.Count - 1], activeChecklistGroup);
      }
    }
    this.ReleaseContainers(this.activeChecklistGroups.Count - num);
  }

  private void ReleaseContainers(int count)
  {
    int count1 = this.activeChecklistGroups.Count;
    for (int index1 = 1; index1 <= count; ++index1)
    {
      int index2 = count1 - index1;
      CheckboxListGroupSideScreen.CheckboxContainer activeChecklistGroup = this.activeChecklistGroups[index2];
      this.activeChecklistGroups.RemoveAt(index2);
      for (int index3 = activeChecklistGroup.checkboxUIItems.Count - 1; index3 >= 0; --index3)
        this.RemoveSingleCheckboxFromContainer(activeChecklistGroup.checkboxUIItems[index3], activeChecklistGroup);
      activeChecklistGroup.container.gameObject.SetActive(false);
      this.checkboxContainerPool.Release(activeChecklistGroup);
    }
  }

  private void InitContainer(
    ICheckboxListGroupControl target,
    ICheckboxListGroupControl.ListGroup group,
    CheckboxListGroupSideScreen.CheckboxContainer groupUI)
  {
    this.activeChecklistGroups.Add(groupUI);
    groupUI.container.gameObject.SetActive(true);
    string sourceText = group.title;
    if (group.resolveTitleCallback != null)
      sourceText = group.resolveTitleCallback(sourceText);
    groupUI.container.GetReference<LocText>("Text").SetText(sourceText);
    foreach (ICheckboxListGroupControl.CheckboxItem checkboxItem in group.checkboxItems)
      this.CreateSingleCheckBoxForGroupUI(checkboxItem, target, groupUI);
  }

  public void RemoveSingleCheckboxFromContainer(
    HierarchyReferences checkbox,
    CheckboxListGroupSideScreen.CheckboxContainer container)
  {
    container.checkboxUIItems.Remove(checkbox);
    checkbox.gameObject.SetActive(false);
    checkbox.transform.SetParent((Transform) this.checkboxParent);
    this.checkboxPool.ReleaseInstance(checkbox.gameObject);
  }

  public HierarchyReferences CreateSingleCheckBoxForGroupUI(
    CheckboxListGroupSideScreen.CheckboxContainer container)
  {
    HierarchyReferences component = this.checkboxPool.GetInstance().GetComponent<HierarchyReferences>();
    component.gameObject.SetActive(true);
    container.checkboxUIItems.Add(component);
    component.transform.SetParent(container.container.transform);
    return component;
  }

  public HierarchyReferences CreateSingleCheckBoxForGroupUI(
    ICheckboxListGroupControl.CheckboxItem data,
    ICheckboxListGroupControl target,
    CheckboxListGroupSideScreen.CheckboxContainer container)
  {
    HierarchyReferences checkBoxForGroupUi = this.CreateSingleCheckBoxForGroupUI(container);
    this.SetCheckboxData(checkBoxForGroupUi, data, target);
    return checkBoxForGroupUi;
  }

  public void SetCheckboxData(
    HierarchyReferences checkboxUI,
    ICheckboxListGroupControl.CheckboxItem data,
    ICheckboxListGroupControl target)
  {
    LocText reference1 = checkboxUI.GetReference<LocText>("Text");
    reference1.SetText(data.text);
    reference1.SetLinkOverrideAction(data.overrideLinkActions);
    checkboxUI.GetReference<Image>("Check").enabled = data.isOn;
    ToolTip reference2 = checkboxUI.GetReference<ToolTip>("Tooltip");
    reference2.SetSimpleTooltip(data.tooltip);
    reference2.refreshWhileHovering = data.resolveTooltipCallback != null;
    reference2.OnToolTip = (Func<string>) (() => data.resolveTooltipCallback == null ? data.tooltip : data.resolveTooltipCallback(data.tooltip, (object) target));
  }

  public class CheckboxContainer
  {
    public HierarchyReferences container;
    public List<HierarchyReferences> checkboxUIItems = new List<HierarchyReferences>();

    public CheckboxContainer(HierarchyReferences container) => this.container = container;
  }
}
