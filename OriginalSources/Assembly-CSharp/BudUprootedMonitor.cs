// Decompiled with JetBrains decompiler
// Type: BudUprootedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/BudUprootedMonitor")]
public class BudUprootedMonitor : KMonoBehaviour
{
  [Serialize]
  public bool canBeUprooted = true;
  [Serialize]
  private bool uprooted;
  public bool destroyOnParentLost;
  public Ref<KPrefabID> parentObject = new Ref<KPrefabID>();
  private HandleVector<int>.Handle partitionerEntry;
  private static readonly EventSystem.IntraObjectHandler<BudUprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BudUprootedMonitor>((Action<BudUprootedMonitor, object>) ((component, data) =>
  {
    if (component.uprooted)
      return;
    component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted);
    component.uprooted = true;
    component.Trigger(-216549700, (object) null);
  }));

  public bool IsUprooted
  {
    get => this.uprooted || this.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<BudUprootedMonitor>(-216549700, BudUprootedMonitor.OnUprootedDelegate);
  }

  public void SetParentObject(KPrefabID id)
  {
    this.parentObject = new Ref<KPrefabID>(id);
    this.Subscribe(id.gameObject, 1969584890, new Action<object>(this.OnLoseParent));
  }

  private void OnLoseParent(object obj)
  {
    if (this.uprooted || this.isNull)
      return;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted);
    this.uprooted = true;
    this.Trigger(-216549700, (object) null);
    if (!this.destroyOnParentLost)
      return;
    Util.KDestroyGameObject(this.gameObject);
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public static bool IsObjectUprooted(GameObject plant)
  {
    BudUprootedMonitor component = plant.GetComponent<BudUprootedMonitor>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.IsUprooted;
  }
}
