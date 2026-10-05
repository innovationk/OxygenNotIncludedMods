// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeModifierSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class AttributeModifierSickness : Sickness.SicknessComponent
{
  private Dictionary<Tag, AttributeModifier[]> GetAttributeModifierForMinionModel = new Dictionary<Tag, AttributeModifier[]>();
  private AttributeModifier[] attributeModifiers;

  public AttributeModifierSickness(Tag minionModel, AttributeModifier[] attribute_modifiers)
  {
    this.GetAttributeModifierForMinionModel[minionModel] = attribute_modifiers;
    this.attributeModifiers = new AttributeModifier[0];
  }

  public AttributeModifierSickness(AttributeModifier[] attribute_modifiers)
  {
    this.attributeModifiers = attribute_modifiers;
  }

  public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
  {
    Attributes attributes = go.GetAttributes();
    Tag key = go.PrefabID();
    if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
    {
      for (int index = 0; index < this.GetAttributeModifierForMinionModel[key].Length; ++index)
      {
        AttributeModifier modifier = this.GetAttributeModifierForMinionModel[key][index];
        attributes.Add(modifier);
      }
    }
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Add(attributeModifier);
    }
    return (object) null;
  }

  public override void OnCure(GameObject go, object instance_data)
  {
    Attributes attributes = go.GetAttributes();
    Tag key = go.PrefabID();
    if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
    {
      for (int index = 0; index < this.GetAttributeModifierForMinionModel[key].Length; ++index)
      {
        AttributeModifier modifier = this.GetAttributeModifierForMinionModel[key][index];
        attributes.Remove(modifier);
      }
    }
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Remove(attributeModifier);
    }
  }

  public AttributeModifier[] Modifers => this.attributeModifiers;

  public override List<Descriptor> GetSymptoms(GameObject victim)
  {
    if ((Object) victim == (Object) null)
      return this.GetSymptoms();
    List<Descriptor> symptoms = new List<Descriptor>();
    Tag key = victim.PrefabID();
    if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
    {
      foreach (AttributeModifier attributeModifier in this.GetAttributeModifierForMinionModel[key])
      {
        Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
        symptoms.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom));
      }
    }
    foreach (AttributeModifier attributeModifier in this.attributeModifiers)
    {
      Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
      symptoms.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom));
    }
    return symptoms;
  }

  public override List<Descriptor> GetSymptoms()
  {
    List<Descriptor> symptoms = new List<Descriptor>();
    foreach (Tag key in this.GetAttributeModifierForMinionModel.Keys)
    {
      string properName = Assets.GetPrefab(key).GetProperName();
      foreach (AttributeModifier attributeModifier in this.GetAttributeModifierForMinionModel[key])
      {
        Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
        symptoms.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_BY_MODEL_MODIFIER_SYMPTOMS, (object) properName, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom));
      }
    }
    foreach (AttributeModifier attributeModifier in this.attributeModifiers)
    {
      Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
      symptoms.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, (object) attribute.Name, (object) attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom));
    }
    return symptoms;
  }
}
