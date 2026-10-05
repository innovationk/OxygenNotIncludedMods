// Decompiled with JetBrains decompiler
// Type: BubbleSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/BubbleSpawner")]
public class BubbleSpawner : KMonoBehaviour
{
  public float emitMass;
  public float emitVariance;
  public Vector3 emitOffset = Vector3.zero;
  [MyCmpGet]
  private Storage storage;
  public SimHashes element;
  private static readonly EventSystem.IntraObjectHandler<BubbleSpawner> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<BubbleSpawner>((Action<BubbleSpawner, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnSpawn()
  {
    this.emitMass += (UnityEngine.Random.value - 0.5f) * this.emitVariance * this.emitMass;
    base.OnSpawn();
    this.Subscribe<BubbleSpawner>(-1697596308, BubbleSpawner.OnStorageChangedDelegate);
  }

  private void OnStorageChanged(object data)
  {
    GameObject first = this.storage.FindFirst(ElementLoader.FindElementByHash(this.element).tag);
    if ((UnityEngine.Object) first == (UnityEngine.Object) null)
      return;
    PrimaryElement component = first.GetComponent<PrimaryElement>();
    if ((double) component.Mass < (double) this.emitMass)
      return;
    first.GetComponent<PrimaryElement>().Mass -= this.emitMass;
    BubbleManager.instance.SpawnBubble(this.element, (Vector2) this.transform.GetPosition(), this.emitMass, component.Temperature, BubbleManager.Disease.None);
  }
}
