// Decompiled with JetBrains decompiler
// Type: ElementSplitterComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ElementSplitterComponents : KGameObjectComponentManager<ElementSplitter>
{
  private const float MAX_STACK_SIZE = 25000f;

  public HandleVector<int>.Handle Add(GameObject go) => this.Add(go, new ElementSplitter(go));

  protected override void OnPrefabInit(HandleVector<int>.Handle handle)
  {
    ElementSplitter data = this.GetData(handle);
    Pickupable component = data.primaryElement.GetComponent<Pickupable>();
    Func<Pickupable, float, Pickupable> func1 = (Func<Pickupable, float, Pickupable>) ((obj, amount) => ElementSplitterComponents.OnTake(obj, handle, amount));
    component.OnTake += func1;
    Func<Pickupable, bool> func2 = (Func<Pickupable, bool>) (other => ElementSplitterComponents.CanFirstAbsorbSecond(handle, this.GetHandle(other.gameObject)));
    component.CanAbsorb += func2;
    component.absorbable = true;
    data.onTakeCB = func1;
    data.canAbsorbCB = func2;
    this.SetData(handle, data);
  }

  protected override void OnSpawn(HandleVector<int>.Handle handle)
  {
  }

  protected override void OnCleanUp(HandleVector<int>.Handle handle)
  {
    ElementSplitter data = this.GetData(handle);
    if (!((UnityEngine.Object) data.primaryElement != (UnityEngine.Object) null))
      return;
    Pickupable component = data.primaryElement.GetComponent<Pickupable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.OnTake -= data.onTakeCB;
    component.CanAbsorb -= data.canAbsorbCB;
  }

  private static bool CanFirstAbsorbSecond(
    HandleVector<int>.Handle first,
    HandleVector<int>.Handle second)
  {
    if (first == HandleVector<int>.InvalidHandle || second == HandleVector<int>.InvalidHandle)
      return false;
    ElementSplitter data1 = GameComps.ElementSplitters.GetData(first);
    ElementSplitter data2 = GameComps.ElementSplitters.GetData(second);
    return data1.primaryElement.ElementID == data2.primaryElement.ElementID && (double) data1.primaryElement.Units + (double) data2.primaryElement.Units < 25000.0 && !data1.kPrefabID.HasTag(GameTags.MarkedForMove) && !data2.kPrefabID.HasTag(GameTags.MarkedForMove);
  }

  private static Pickupable OnTake(
    Pickupable pickupable,
    HandleVector<int>.Handle handle,
    float amount)
  {
    ElementSplitter data = GameComps.ElementSplitters.GetData(handle);
    Storage storage = pickupable.storage;
    PrimaryElement primaryElement = pickupable.PrimaryElement;
    Pickupable component = primaryElement.Element.substance.SpawnResource(pickupable.transform.GetPosition(), amount, primaryElement.Temperature, byte.MaxValue, 0, true).GetComponent<Pickupable>();
    pickupable.TotalAmount -= amount;
    component.Trigger(1335436905, (object) pickupable);
    ElementSplitterComponents.CopyRenderSettings(pickupable.GetComponent<KBatchedAnimController>(), component.GetComponent<KBatchedAnimController>());
    if ((UnityEngine.Object) storage != (UnityEngine.Object) null)
    {
      storage.Trigger(-1697596308, (object) data.primaryElement.gameObject);
      storage.Trigger(-778359855, (object) storage);
    }
    return component;
  }

  private static void CopyRenderSettings(KBatchedAnimController src, KBatchedAnimController dest)
  {
    if (!((UnityEngine.Object) src != (UnityEngine.Object) null) || !((UnityEngine.Object) dest != (UnityEngine.Object) null))
      return;
    dest.OverlayColour = src.OverlayColour;
  }
}
