// Decompiled with JetBrains decompiler
// Type: InvalidPortReporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class InvalidPortReporter : KMonoBehaviour
{
  public static readonly Operational.Flag portsNotOverlapping = new Operational.Flag("ports_not_overlapping", Operational.Flag.Type.Functional);
  private static readonly EventSystem.IntraObjectHandler<InvalidPortReporter> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<InvalidPortReporter>((Action<InvalidPortReporter, object>) ((component, data) => component.OnTagsChanged(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnTagsChanged((object) null);
    this.Subscribe<InvalidPortReporter>(-1582839653, InvalidPortReporter.OnTagsChangedDelegate);
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  private void OnTagsChanged(object _)
  {
    bool on = this.gameObject.HasTag(GameTags.HasInvalidPorts);
    Operational component1 = this.GetComponent<Operational>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetFlag(InvalidPortReporter.portsNotOverlapping, !on);
    KSelectable component2 = this.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component2.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidPortOverlap, on, (object) this.gameObject);
  }
}
