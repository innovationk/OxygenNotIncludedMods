// Decompiled with JetBrains decompiler
// Type: Radiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Radiator")]
public class Radiator : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public RadiationGridEmitter emitter;
  public int intensity;
  public int projectionCount;
  public int direction;
  public int angle = 360;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.emitter = new RadiationGridEmitter(Grid.PosToCell(this.gameObject), this.intensity);
    this.emitter.projectionCount = this.projectionCount;
    this.emitter.direction = this.direction;
    this.emitter.angle = this.angle;
    if ((UnityEngine.Object) this.GetComponent<Operational>() == (UnityEngine.Object) null)
      this.emitter.enabled = true;
    else
      this.Subscribe(824508782, new Action<object>(this.OnOperationalChanged));
    RadiationGridManager.emitters.Add(this.emitter);
  }

  protected override void OnCleanUp()
  {
    RadiationGridManager.emitters.Remove(this.emitter);
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object _)
  {
    this.emitter.enabled = this.GetComponent<Operational>().IsActive;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, (object) this.intensity), (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT)
    };
  }

  private void Update() => this.emitter.originCell = Grid.PosToCell(this.gameObject);
}
