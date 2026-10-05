// Decompiled with JetBrains decompiler
// Type: Trappable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Trappable")]
public class Trappable : KMonoBehaviour, IGameObjectEffectDescriptor
{
  private bool registered;
  private ulong cellChangedHandlerID;
  private static readonly Action<object> OnCellChangedDispatcher = (Action<object>) (obj => Unsafe.As<Trappable>(obj).OnCellChange());
  private static readonly EventSystem.IntraObjectHandler<Trappable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Trappable>((Action<Trappable, object>) ((component, data) => component.OnStore(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Register();
    this.OnCellChange();
  }

  protected override void OnCleanUp()
  {
    this.Unregister();
    base.OnCleanUp();
  }

  private void OnCellChange()
  {
    GameScenePartitioner.Instance.TriggerEvent(Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.trapsLayer, (object) this);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.Register();
  }

  protected override void OnCmpDisable()
  {
    this.Unregister();
    base.OnCmpDisable();
  }

  private void Register()
  {
    if (this.registered)
      return;
    this.Subscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
    this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, Trappable.OnCellChangedDispatcher, (object) this, "Trappable.Register");
    this.registered = true;
  }

  private void Unregister()
  {
    if (!this.registered)
      return;
    this.Unsubscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
    this.registered = false;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_LAND_TRAP, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_TRAP)
    };
  }

  public void OnStore(object data)
  {
    Storage cmp = data as Storage;
    if (((bool) (UnityEngine.Object) cmp ? ((UnityEngine.Object) cmp.GetComponent<Trap>() != (UnityEngine.Object) null ? 1 : (cmp.GetSMI<ReusableTrap.Instance>() != null ? 1 : 0)) : 0) != 0)
    {
      this.gameObject.AddTag(GameTags.Trapped);
      Navigator component1 = this.gameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.Stop();
      Brain component2 = this.gameObject.GetComponent<Brain>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      Game.BrainScheduler.PrioritizeBrain(component2);
    }
    else
      this.gameObject.RemoveTag(GameTags.Trapped);
  }
}
