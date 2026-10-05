// Decompiled with JetBrains decompiler
// Type: DevQuickActionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevQuickActionsScreen : MonoBehaviour
{
  public const float DEFAULT_SPACE = 60f;
  public const float ROOT_SPACE = 50f;
  public const char CATEGORY_DIVIDER = '/';
  public DevQuickActionNode originalCategoryDevNode;
  public DevQuickActionNode originalEndNode;
  public DevQuickActionTargetFollower Pointer;
  public Stack<DevQuickActionEndNode> recycledEndNodes = new Stack<DevQuickActionEndNode>();
  public Stack<DevQuickActionCategoryNode> recycledCategoriesNodes = new Stack<DevQuickActionCategoryNode>();
  private Dictionary<string, DevQuickActionCategoryNode> registeredCategoryNodes = new Dictionary<string, DevQuickActionCategoryNode>();
  private GameObject Target;
  private DevQuickActionCategoryNode RootNode;
  public static DevQuickActionsScreen Instance;

  public static void DestroyInstance()
  {
    DevQuickActionsScreen.Instance = (DevQuickActionsScreen) null;
  }

  private void Awake()
  {
    DevQuickActionsScreen.Instance = this;
    this.Pointer.SetVisibleState(false);
    this.Pointer.OnToggleChanged += new Action<bool>(this.OnPointerToggleClicked);
    this.originalEndNode.gameObject.SetActive(false);
    this.originalCategoryDevNode.gameObject.SetActive(false);
  }

  private void OnPointerToggleClicked(bool val)
  {
    if (!((UnityEngine.Object) this.Target != (UnityEngine.Object) null) || !((UnityEngine.Object) this.RootNode != (UnityEngine.Object) null))
      return;
    if (val)
      this.RootNode.Expand();
    else
      this.RootNode.Collapse();
  }

  public void Toggle(GameObject target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      this.Close();
    else if ((UnityEngine.Object) this.Target != (UnityEngine.Object) target)
      this.Open(target);
    else
      this.Close();
  }

  public void Open(GameObject target)
  {
    if ((UnityEngine.Object) this.Target != (UnityEngine.Object) null && (UnityEngine.Object) this.Target != (UnityEngine.Object) target)
      this.Close();
    this.Target = target;
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return;
    Vector3 screenPoint = CameraController.Instance.overlayCamera.WorldToScreenPoint(target.transform.position);
    this.RootNode = this.GetUnsedCategoryNode();
    this.RootNode.Setup(target.GetProperName(), (DevQuickActionNode) null);
    this.RootNode.transform.SetPosition(screenPoint);
    this.RootNode.SetChildrenSeparationSpace(50f);
    this.Target.Subscribe(1502190696, new Action<object>(this.OnTargetLost));
    List<IDevQuickAction> devQuickActionList = new List<IDevQuickAction>((IEnumerable<IDevQuickAction>) this.Target.GetComponents<IDevQuickAction>());
    devQuickActionList.AddRange((IEnumerable<IDevQuickAction>) this.Target.GetAllSMI<IDevQuickAction>());
    foreach (IDevQuickAction devQuickAction in devQuickActionList)
    {
      foreach (DevQuickActionInstruction devInstruction in devQuickAction.GetDevInstructions())
      {
        string[] strArray = devInstruction.Address.Split('/', StringSplitOptions.None);
        DevQuickActionCategoryNode parentNode = this.RootNode;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string str = strArray[index];
          if (index < strArray.Length - 1)
          {
            DevQuickActionCategoryNode node = (DevQuickActionCategoryNode) null;
            if (!this.registeredCategoryNodes.TryGetValue(str, out node))
            {
              node = this.GetUnsedCategoryNode();
              node.Setup(str, (DevQuickActionNode) parentNode);
              this.registeredCategoryNodes.Add(str, node);
              parentNode.AddChildren((DevQuickActionNode) node);
            }
            parentNode = node;
          }
          else
          {
            DevQuickActionEndNode unsedEndNode = this.GetUnsedEndNode();
            unsedEndNode.Setup(str, (DevQuickActionNode) parentNode, devInstruction.Action);
            parentNode.AddChildren((DevQuickActionNode) unsedEndNode);
            unsedEndNode.gameObject.SetActive(false);
          }
        }
      }
    }
    this.RootNode.Collapse();
    if (this.Pointer.IsToggleOn)
      this.RootNode.Expand();
    this.RootNode.gameObject.SetActive(false);
    this.Pointer.transform.position = this.RootNode.transform.position;
    this.Pointer.SetTarget(this.Target);
    this.Pointer.SetVisibleState(true);
  }

  public void Close()
  {
    if ((UnityEngine.Object) this.Target != (UnityEngine.Object) null)
      this.Target.Unsubscribe(1502190696, new Action<object>(this.OnTargetLost));
    this.Target = (GameObject) null;
    if ((UnityEngine.Object) this.RootNode != (UnityEngine.Object) null)
    {
      this.RootNode.Recycle();
      this.RootNode = (DevQuickActionCategoryNode) null;
    }
    this.registeredCategoryNodes.Clear();
    this.Pointer.SetTarget((GameObject) null);
    this.Pointer.SetVisibleState(false);
  }

  private void OnTargetLost(object o) => this.Close();

  private DevQuickActionEndNode GetUnsedEndNode()
  {
    DevQuickActionEndNode node = (DevQuickActionEndNode) null;
    if (!this.recycledEndNodes.TryPop(ref node))
      node = Util.KInstantiateUI(this.originalEndNode.gameObject, this.originalEndNode.transform.parent.gameObject).GetComponent<DevQuickActionEndNode>();
    this.SetupUnusedNodeForUse((DevQuickActionNode) node);
    return node;
  }

  private DevQuickActionCategoryNode GetUnsedCategoryNode()
  {
    DevQuickActionCategoryNode node = (DevQuickActionCategoryNode) null;
    if (!this.recycledCategoriesNodes.TryPop(ref node))
      node = Util.KInstantiateUI(this.originalCategoryDevNode.gameObject, this.originalCategoryDevNode.transform.parent.gameObject).GetComponent<DevQuickActionCategoryNode>();
    this.SetupUnusedNodeForUse((DevQuickActionNode) node);
    return node;
  }

  private void SetupUnusedNodeForUse(DevQuickActionNode node)
  {
    node.OnRecycle = new Action<DevQuickActionNode>(this.OnNodeRecycled);
    node.SetChildrenSeparationSpace(60f);
    node.gameObject.SetActive(true);
  }

  private void OnNodeRecycled(DevQuickActionNode node)
  {
    switch (node)
    {
      case DevQuickActionCategoryNode _:
        this.recycledCategoriesNodes.Push(node as DevQuickActionCategoryNode);
        break;
      case DevQuickActionEndNode _:
        this.recycledEndNodes.Push(node as DevQuickActionEndNode);
        break;
    }
  }
}
