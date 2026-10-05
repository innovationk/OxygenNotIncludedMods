// Decompiled with JetBrains decompiler
// Type: DevQuickActionEndNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine.Events;

#nullable disable
public class DevQuickActionEndNode : DevQuickActionNode
{
  private UnityEngine.UI.Button button;

  protected void Awake()
  {
    this.button = this.GetComponent<UnityEngine.UI.Button>();
    this.button.onClick.AddListener(new UnityAction(this.ButtonClicked));
  }

  private void ButtonClicked()
  {
    System.Action nodeInteractedWith = this.OnNodeInteractedWith;
    if (nodeInteractedWith == null)
      return;
    nodeInteractedWith();
  }

  public void Setup(string name, DevQuickActionNode parentNode, System.Action clickCB)
  {
    this.label.SetText(name);
    this.parentNode = parentNode;
    this.OnNodeInteractedWith = clickCB;
  }
}
