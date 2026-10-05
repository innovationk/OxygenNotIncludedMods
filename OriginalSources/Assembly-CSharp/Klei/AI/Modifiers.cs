// Decompiled with JetBrains decompiler
// Type: Klei.AI.Modifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
namespace Klei.AI;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Modifiers")]
public class Modifiers : KMonoBehaviour, ISaveLoadableDetails
{
  public Amounts amounts;
  public Attributes attributes;
  public Sicknesses sicknesses;
  public List<string> initialTraits = new List<string>();
  public List<string> initialAmounts = new List<string>();
  public List<string> initialAttributes = new List<string>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.amounts = new Amounts(this.gameObject);
    this.sicknesses = new Sicknesses(this.gameObject);
    this.attributes = new Attributes(this.gameObject);
    foreach (string initialAmount in this.initialAmounts)
      this.amounts.Add(new AmountInstance(Db.Get().Amounts.Get(initialAmount), this.gameObject));
    foreach (string initialAttribute in this.initialAttributes)
    {
      Attribute attribute = (Db.Get().CritterAttributes.TryGet(initialAttribute) ?? Db.Get().PlantAttributes.TryGet(initialAttribute)) ?? Db.Get().Attributes.TryGet(initialAttribute);
      DebugUtil.Assert(attribute != null, "Couldn't find an attribute for id", initialAttribute);
      this.attributes.Add(attribute);
    }
    Traits component = this.GetComponent<Traits>();
    if (this.initialTraits == null)
      return;
    foreach (string initialTrait in this.initialTraits)
    {
      Trait trait = Db.Get().traits.Get(initialTrait);
      component.Add(trait);
    }
  }

  public float GetPreModifiedAttributeValue(Attribute attribute)
  {
    return AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
  }

  public string GetPreModifiedAttributeFormattedValue(Attribute attribute)
  {
    float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
    return attribute.formatter.GetFormattedValue(totalValue, attribute.formatter.DeltaTimeSlice);
  }

  public string GetPreModifiedAttributeDescription(Attribute attribute)
  {
    float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
    return string.Format((string) DUPLICANTS.ATTRIBUTES.VALUE, (object) attribute.Name, (object) attribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None));
  }

  public string GetPreModifiedAttributeToolTip(Attribute attribute)
  {
    return attribute.formatter.GetTooltip(attribute, this.GetPreModifiers(attribute), (AttributeConverters) null);
  }

  public List<AttributeModifier> GetPreModifiers(Attribute attribute)
  {
    List<AttributeModifier> preModifiers = new List<AttributeModifier>();
    foreach (string initialTrait in this.initialTraits)
    {
      foreach (AttributeModifier selfModifier in Db.Get().traits.Get(initialTrait).SelfModifiers)
      {
        if (selfModifier.AttributeId == attribute.Id)
          preModifiers.Add(selfModifier);
      }
    }
    MutantPlant component = this.GetComponent<MutantPlant>();
    if ((Object) component != (Object) null && component.MutationIDs != null)
    {
      foreach (string mutationId in component.MutationIDs)
      {
        foreach (AttributeModifier selfModifier in Db.Get().PlantMutations.Get(mutationId).SelfModifiers)
        {
          if (selfModifier.AttributeId == attribute.Id)
            preModifiers.Add(selfModifier);
        }
      }
    }
    return preModifiers;
  }

  public void Serialize(BinaryWriter writer) => this.OnSerialize(writer);

  public void Deserialize(IReader reader) => this.OnDeserialize(reader);

  public virtual void OnSerialize(BinaryWriter writer)
  {
    this.amounts.Serialize(writer);
    this.sicknesses.Serialize(writer);
  }

  public virtual void OnDeserialize(IReader reader)
  {
    this.amounts.Deserialize(reader);
    this.sicknesses.Deserialize(reader);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.amounts == null)
      return;
    this.amounts.Cleanup();
  }
}
