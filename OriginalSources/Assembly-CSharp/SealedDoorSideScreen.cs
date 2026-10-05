// Decompiled with JetBrains decompiler
// Type: SealedDoorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SealedDoorSideScreen : SideScreenContent
{
  [SerializeField]
  private LocText label;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private Door target;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.button.onClick += (System.Action) (() => this.target.OrderUnseal());
    this.Refresh();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<Door>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    Door component = target.GetComponent<Door>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Target doesn't have a Door associated with it.");
    }
    else
    {
      this.target = component;
      this.Refresh();
    }
  }

  private void Refresh()
  {
    if (!this.target.isSealed)
      this.ContentContainer.SetActive(false);
    else
      this.ContentContainer.SetActive(true);
  }
}
