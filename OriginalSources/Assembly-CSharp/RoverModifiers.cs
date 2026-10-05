// Decompiled with JetBrains decompiler
// Type: RoverModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class RoverModifiers : Modifiers, ISaveLoadable
{
  private static readonly EventSystem.IntraObjectHandler<RoverModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<RoverModifiers>((Action<RoverModifiers, object>) ((component, data) => component.OnBeginChore(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.attributes.Add(Db.Get().Attributes.Construction);
    this.attributes.Add(Db.Get().Attributes.Digging);
    this.attributes.Add(Db.Get().Attributes.Strength);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!((UnityEngine.Object) this.GetComponent<ChoreConsumer>() != (UnityEngine.Object) null))
      return;
    this.Subscribe<RoverModifiers>(-1988963660, RoverModifiers.OnBeginChoreDelegate);
    this.transform.SetPosition(this.transform.GetPosition() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.Move)
    });
    this.gameObject.layer = LayerMask.NameToLayer("Default");
    this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
  }

  private void SetupDependentAttribute(
    Klei.AI.Attribute targetAttribute,
    AttributeConverter attributeConverter)
  {
    Klei.AI.Attribute attribute = attributeConverter.attribute;
    AttributeInstance attributeInstance = attribute.Lookup((Component) this);
    AttributeModifier target_modifier = new AttributeModifier(targetAttribute.Id, attributeConverter.Lookup((Component) this).Evaluate(), attribute.Name, is_readonly: false);
    this.GetAttributes().Add(target_modifier);
    attributeInstance.OnDirty += (System.Action) (() => target_modifier.SetValue(attributeConverter.Lookup((Component) this).Evaluate()));
  }

  private void OnBeginChore(object data)
  {
    Storage component = this.GetComponent<Storage>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.DropAll();
  }
}
