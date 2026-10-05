// Decompiled with JetBrains decompiler
// Type: Element
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ElementData;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

#nullable disable
[DebuggerDisplay("{name}")]
[Serializable]
public class Element : IComparable<Element>
{
  public const int INVALID_ID = 0;
  public SimHashes id;
  public Tag tag;
  public ushort idx;
  public float specificHeatCapacity;
  public float thermalConductivity = 1f;
  public float molarMass = 1f;
  public float strength;
  public float flow;
  public float maxCompression;
  public float viscosity;
  public float minHorizontalFlow = float.PositiveInfinity;
  public float minVerticalFlow = float.PositiveInfinity;
  public float maxMass = 10000f;
  public float solidSurfaceAreaMultiplier;
  public float liquidSurfaceAreaMultiplier;
  public float gasSurfaceAreaMultiplier;
  public Element.State state;
  public byte hardness;
  public float lowTemp;
  public SimHashes lowTempTransitionTarget;
  public Element lowTempTransition;
  public float highTemp;
  public SimHashes highTempTransitionTarget;
  public Element highTempTransition;
  public SimHashes highTempTransitionOreID = SimHashes.Vacuum;
  public float highTempTransitionOreMassConversion;
  public SimHashes lowTempTransitionOreID = SimHashes.Vacuum;
  public float lowTempTransitionOreMassConversion;
  public SimHashes sublimateId;
  public SimHashes convertId;
  public SpawnFXHashes sublimateFX;
  public float sublimateRate;
  public float sublimateEfficiency;
  public float sublimateProbability;
  public float offGasPercentage;
  public float lightAbsorptionFactor;
  public float radiationAbsorptionFactor;
  public float radiationPer1000Mass;
  public Sim.PhysicsData defaultValues;
  public SimHashes refinedMetalTarget;
  public float toxicity;
  public Substance substance;
  public Tag materialCategory;
  public int buildMenuSort;
  public ElementComposition[] elementComposition;
  public Tag[] oreTags = new Tag[0];
  public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
  public bool disabled;
  public string dlcId;
  private const float MOLTEN_LIQUID_MIN_TEMP = 373.15f;
  public const byte StateMask = 3;

  public bool IsMoltenMetal
  {
    get
    {
      return this.IsLiquid && this.substance.Metalic && (double) this.lowTemp > 373.14999389648438 && this.HasTag(GameTags.Metal);
    }
  }

  public float GetRelativeHeatLevel(float currentTemperature)
  {
    float num1 = this.lowTemp - 3f;
    float num2 = this.highTemp + 3f;
    return Mathf.Clamp01((float) (((double) currentTemperature - (double) num1) / ((double) num2 - (double) num1)));
  }

  public float PressureToMass(float pressure) => pressure / this.defaultValues.pressure;

  public bool IsSlippery => this.HasTag(GameTags.Slippery);

  public bool IsUnstable => this.HasTag(GameTags.Unstable);

  public bool IsLiquid => (this.state & Element.State.Solid) == Element.State.Liquid;

  public bool IsGas => (this.state & Element.State.Solid) == Element.State.Gas;

  public bool IsSolid => (this.state & Element.State.Solid) == Element.State.Solid;

  public bool IsVacuum => (this.state & Element.State.Solid) == Element.State.Vacuum;

  public bool IsTemperatureInsulated => (this.state & Element.State.TemperatureInsulated) != 0;

  public bool IsState(Element.State expected_state)
  {
    return (this.state & Element.State.Solid) == expected_state;
  }

  public bool HasTransitionUp
  {
    get
    {
      return this.highTempTransitionTarget != (SimHashes) 0 && this.highTempTransitionTarget != SimHashes.Unobtanium && this.highTempTransition != null && this.highTempTransition != this;
    }
  }

  public string name { get; set; }

  public string nameUpperCase { get; set; }

  public string description { get; set; }

  public string GetStateString() => Element.GetStateString(this.state);

  public static string GetStateString(Element.State state)
  {
    if ((state & Element.State.Solid) == Element.State.Solid)
      return (string) ELEMENTS.STATE.SOLID;
    if ((state & Element.State.Solid) == Element.State.Liquid)
      return (string) ELEMENTS.STATE.LIQUID;
    return (state & Element.State.Solid) == Element.State.Gas ? (string) ELEMENTS.STATE.GAS : (string) ELEMENTS.STATE.VACUUM;
  }

  public string FullDescription(bool addHardnessColor = true)
  {
    StringBuilder sb1 = GlobalStringBuilderPool.Alloc();
    sb1.Clear();
    sb1.Append(this.Description());
    if (this.IsSolid)
    {
      sb1.Append("\n\n");
      sb1.AppendFormat((string) ELEMENTS.ELEMENTDESCSOLID, (object) this.GetMaterialCategoryTag().ProperName(), (object) GameUtil.GetFormattedTemperature(this.highTemp), (object) GameUtil.GetHardnessString(this, addHardnessColor));
    }
    else if (this.IsLiquid)
    {
      sb1.Append("\n\n");
      sb1.AppendFormat((string) ELEMENTS.ELEMENTDESCLIQUID, (object) this.GetMaterialCategoryTag().ProperName(), (object) GameUtil.GetFormattedTemperature(this.lowTemp), (object) GameUtil.GetFormattedTemperature(this.highTemp));
    }
    else if (!this.IsVacuum)
    {
      sb1.Append("\n\n");
      sb1.AppendFormat((string) ELEMENTS.ELEMENTDESCGAS, (object) this.GetMaterialCategoryTag().ProperName(), (object) GameUtil.GetFormattedTemperature(this.lowTemp));
    }
    StringBuilder sb2 = GlobalStringBuilderPool.Alloc();
    sb2.Append((string) ELEMENTS.THERMALPROPERTIES);
    sb2.Replace("{SPECIFIC_HEAT_CAPACITY}", GameUtil.GetFormattedSHC(this.specificHeatCapacity));
    sb2.Replace("{THERMAL_CONDUCTIVITY}", GameUtil.GetFormattedThermalConductivity(this.thermalConductivity));
    sb1.Append("\n");
    sb1.Append(sb2.ToString());
    GlobalStringBuilderPool.Free(sb2);
    if (DlcManager.FeatureRadiationEnabled())
    {
      sb1.Append("\n");
      sb1.AppendFormat((string) ELEMENTS.RADIATIONPROPERTIES, (object) this.radiationAbsorptionFactor, (object) GameUtil.GetFormattedRads((float) ((double) this.radiationPer1000Mass * 1.1000000238418579 / 600.0), GameUtil.TimeSlice.PerCycle));
    }
    if (this.oreTags.Length != 0 && !this.IsVacuum)
    {
      sb1.Append("\n\n");
      StringBuilder sb3 = GlobalStringBuilderPool.Alloc();
      for (int index = 0; index < this.oreTags.Length; ++index)
      {
        Tag tag = new Tag(this.oreTags[index]);
        if (!GameTags.HiddenElementTags.Contains(tag))
        {
          sb3.Append(tag.ProperName());
          if (index < this.oreTags.Length - 1)
            sb3.Append(", ");
        }
      }
      sb1.AppendFormat((string) ELEMENTS.ELEMENTPROPERTIES, (object) GlobalStringBuilderPool.ReturnAndFree(sb3));
    }
    if (this.attributeModifiers.Count > 0)
    {
      foreach (AttributeModifier attributeModifier in this.attributeModifiers)
      {
        sb1.AppendLine();
        sb1.AppendFormat((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name, (object) attributeModifier.GetFormattedString());
      }
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb1);
  }

  public string Description() => this.description;

  public bool HasTag(Tag search_tag)
  {
    return this.tag == search_tag || Array.IndexOf<Tag>(this.oreTags, search_tag) != -1;
  }

  public Tag GetMaterialCategoryTag() => this.materialCategory;

  public int CompareTo(Element other) => this.id - other.id;

  [Serializable]
  public enum State : byte
  {
    Vacuum = 0,
    Gas = 1,
    Liquid = 2,
    Solid = 3,
    Unbreakable = 4,
    Unstable = 8,
    TemperatureInsulated = 16, // 0x10
  }
}
