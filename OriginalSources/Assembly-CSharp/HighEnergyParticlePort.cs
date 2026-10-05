// Decompiled with JetBrains decompiler
// Type: HighEnergyParticlePort
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HighEnergyParticlePort : KMonoBehaviour, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Building m_building;
  public HighEnergyParticlePort.OnParticleCapture onParticleCapture;
  public HighEnergyParticlePort.OnParticleCaptureAllowed onParticleCaptureAllowed;
  public HighEnergyParticlePort.OnParticleCapture onParticleUncapture;
  public HighEnergyParticle currentParticle;
  public bool requireOperational = true;
  public bool particleInputEnabled;
  public bool particleOutputEnabled;
  public CellOffset particleInputOffset;
  public CellOffset particleOutputOffset;

  public int GetHighEnergyParticleInputPortPosition()
  {
    return this.m_building.GetHighEnergyParticleInputCell();
  }

  public int GetHighEnergyParticleOutputPortPosition()
  {
    return this.m_building.GetHighEnergyParticleOutputCell();
  }

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.HighEnergyParticlePorts.Add(this);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.HighEnergyParticlePorts.Remove(this);
  }

  public bool InputActive()
  {
    Operational component = this.GetComponent<Operational>();
    if (!this.particleInputEnabled || !((Object) component != (Object) null) || !component.IsFunctional)
      return false;
    return !this.requireOperational || component.IsOperational;
  }

  public bool AllowCapture(HighEnergyParticle particle)
  {
    return this.onParticleCaptureAllowed == null || this.onParticleCaptureAllowed(particle);
  }

  public void Capture(HighEnergyParticle particle)
  {
    this.currentParticle = particle;
    if (this.onParticleCapture == null)
      return;
    this.onParticleCapture(particle);
  }

  public void Uncapture(HighEnergyParticle particle)
  {
    if (this.onParticleUncapture != null)
      this.onParticleUncapture(particle);
    this.currentParticle = (HighEnergyParticle) null;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    if (this.particleInputEnabled)
      descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.PARTICLE_PORT_INPUT, (string) UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_INPUT, Descriptor.DescriptorType.Requirement));
    if (this.particleOutputEnabled)
      descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.PARTICLE_PORT_OUTPUT, (string) UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_OUTPUT));
    return descriptors;
  }

  public delegate void OnParticleCapture(HighEnergyParticle particle);

  public delegate bool OnParticleCaptureAllowed(HighEnergyParticle particle);
}
