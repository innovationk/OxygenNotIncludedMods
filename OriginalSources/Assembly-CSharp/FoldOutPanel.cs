// Decompiled with JetBrains decompiler
// Type: FoldOutPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FoldOutPanel : KMonoBehaviour
{
  private bool panelOpen = true;
  public GameObject container;
  public bool startOpen = true;

  protected override void OnSpawn()
  {
    this.GetComponentInChildren<MultiToggle>().onClick += new System.Action(this.OnClick);
    this.ToggleOpen(this.startOpen);
  }

  private void OnClick() => this.ToggleOpen(!this.panelOpen);

  private void ToggleOpen(bool open)
  {
    this.panelOpen = open;
    this.container.SetActive(this.panelOpen);
    this.GetComponentInChildren<MultiToggle>().ChangeState(this.panelOpen ? 1 : 0);
  }
}
