// Decompiled with JetBrains decompiler
// Type: Compostable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Compostable")]
public class Compostable : KMonoBehaviour
{
  [SerializeField]
  public bool isMarkedForCompost;
  public GameObject originalPrefab;
  public GameObject compostPrefab;
  public Action<KMonoBehaviour> OnDeserializeCb;
  private static readonly EventSystem.IntraObjectHandler<Compostable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Compostable>((Action<Compostable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Compostable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Compostable>((Action<Compostable, object>) ((component, data) => component.OnStore(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.isMarkedForCompost = this.GetComponent<KPrefabID>().HasTag(GameTags.Compostable);
    if (this.isMarkedForCompost)
      this.MarkForCompost();
    this.Subscribe<Compostable>(493375141, Compostable.OnRefreshUserMenuDelegate);
    this.Subscribe<Compostable>(856640610, Compostable.OnStoreDelegate);
  }

  [OnDeserialized]
  internal void OnDeserializedMethod()
  {
    if (this.OnDeserializeCb == null)
      return;
    this.OnDeserializeCb((KMonoBehaviour) this);
  }

  private void MarkForCompost(bool force = false) => this.RefreshStatusItem();

  private void OnToggleCompost()
  {
    if (!this.isMarkedForCompost)
    {
      Pickupable component = this.GetComponent<Pickupable>();
      if ((UnityEngine.Object) component.storage != (UnityEngine.Object) null)
        component.storage.Drop(this.gameObject, true);
      Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.compostPrefab);
      if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
        return;
      SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
    }
    else
    {
      Pickupable component = this.GetComponent<Pickupable>();
      Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.originalPrefab);
      SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
    }
  }

  private void RefreshStatusItem()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompost);
    component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage);
    if (!this.isMarkedForCompost)
      return;
    if ((UnityEngine.Object) this.GetComponent<Pickupable>() != (UnityEngine.Object) null && (UnityEngine.Object) this.GetComponent<Pickupable>().storage == (UnityEngine.Object) null)
      component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompost);
    else
      component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage);
  }

  private void OnStore(object _) => this.RefreshStatusItem();

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, this.isMarkedForCompost ? new KIconButtonMenu.ButtonInfo("action_compost", (string) UI.USERMENUACTIONS.COMPOST.NAME_OFF, new System.Action(this.OnToggleCompost), tooltipText: (string) UI.USERMENUACTIONS.COMPOST.TOOLTIP_OFF) : new KIconButtonMenu.ButtonInfo("action_compost", (string) UI.USERMENUACTIONS.COMPOST.NAME, new System.Action(this.OnToggleCompost), tooltipText: (string) UI.USERMENUACTIONS.COMPOST.TOOLTIP));
  }
}
