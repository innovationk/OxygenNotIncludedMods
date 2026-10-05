// Decompiled with JetBrains decompiler
// Type: KleiItemsStatusRefresher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class KleiItemsStatusRefresher
{
  public static HashSet<KleiItemsStatusRefresher.UIListener> listeners = new HashSet<KleiItemsStatusRefresher.UIListener>();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  private static void Initialize()
  {
    KleiItems.AddInventoryRefreshCallback(new KleiItems.InventoryRefreshCallback(KleiItemsStatusRefresher.OnRefreshResponseFromServer));
  }

  private static void OnRefreshResponseFromServer()
  {
    foreach (KleiItemsStatusRefresher.UIListener listener in KleiItemsStatusRefresher.listeners)
      listener.Internal_RefreshUI();
  }

  public static void Refresh()
  {
    foreach (KleiItemsStatusRefresher.UIListener listener in KleiItemsStatusRefresher.listeners)
      listener.Internal_RefreshUI();
  }

  public static KleiItemsStatusRefresher.UIListener AddOrGetListener(Component component)
  {
    return KleiItemsStatusRefresher.AddOrGetListener(component.gameObject);
  }

  public static KleiItemsStatusRefresher.UIListener AddOrGetListener(GameObject onGameObject)
  {
    return onGameObject.AddOrGet<KleiItemsStatusRefresher.UIListener>();
  }

  public class UIListener : MonoBehaviour
  {
    private System.Action refreshUIFn;

    public void Internal_RefreshUI()
    {
      if (this.refreshUIFn == null)
        return;
      this.refreshUIFn();
    }

    public void OnRefreshUI(System.Action fn) => this.refreshUIFn = fn;

    private void OnEnable() => KleiItemsStatusRefresher.listeners.Add(this);

    private void OnDisable() => KleiItemsStatusRefresher.listeners.Remove(this);
  }
}
