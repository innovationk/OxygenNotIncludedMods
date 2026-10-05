// Decompiled with JetBrains decompiler
// Type: Storable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class Storable : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Storable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Storable>((Action<Storable, object>) ((component, data) => component.OnStore(data)));
  private static readonly EventSystem.IntraObjectHandler<Storable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Storable>((Action<Storable, object>) ((component, data) => component.RefreshStorageTags(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Storable>(856640610, Storable.OnStoreDelegate);
    this.Subscribe<Storable>(-778359855, Storable.RefreshStorageTagsDelegate);
  }

  public void OnStore(object data) => this.RefreshStorageTags(data);

  private void RefreshStorageTags(object data = null)
  {
    bool flag = data is Storage || data != null && ((Boxed<bool>) data).value;
    Storage storage = (Storage) data;
    if ((UnityEngine.Object) storage != (UnityEngine.Object) null && (UnityEngine.Object) storage.gameObject == (UnityEngine.Object) this.gameObject)
      return;
    KPrefabID component1 = this.GetComponent<KPrefabID>();
    SaveLoadRoot component2 = this.GetComponent<SaveLoadRoot>();
    KSelectable component3 = this.GetComponent<KSelectable>();
    if ((bool) (UnityEngine.Object) component3)
      component3.IsSelectable = !flag;
    if (flag)
    {
      component1.AddTag(GameTags.Stored);
      if (((UnityEngine.Object) storage == (UnityEngine.Object) null ? 1 : (!storage.allowItemRemoval ? 1 : 0)) != 0)
        component1.AddTag(GameTags.StoredPrivate);
      else
        component1.RemoveTag(GameTags.StoredPrivate);
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      component2.SetRegistered(false);
    }
    else
    {
      component1.RemoveTag(GameTags.Stored);
      component1.RemoveTag(GameTags.StoredPrivate);
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      component2.SetRegistered(true);
    }
  }
}
