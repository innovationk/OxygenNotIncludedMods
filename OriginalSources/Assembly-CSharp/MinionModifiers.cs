// Decompiled with JetBrains decompiler
// Type: MinionModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.IO;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class MinionModifiers : Modifiers, ISaveLoadable
{
  public bool addBaseTraits = true;
  private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDeathDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>((Action<MinionModifiers, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnAttachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>((Action<MinionModifiers, object>) ((component, data) => component.OnAttachFollowCam(data)));
  private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDetachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>((Action<MinionModifiers, object>) ((component, data) => component.OnDetachFollowCam(data)));
  private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>((Action<MinionModifiers, object>) ((component, data) => component.OnBeginChore(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (!this.addBaseTraits)
      return;
    foreach (Klei.AI.Attribute resource in Db.Get().Attributes.resources)
    {
      if (this.attributes.Get(resource) == null)
        this.attributes.Add(resource);
    }
    foreach (Klei.AI.Disease resource in Db.Get().Diseases.resources)
    {
      AmountInstance amountInstance = this.AddAmount(resource.amount);
      this.attributes.Add(resource.cureSpeedBase);
      double num = (double) amountInstance.SetValue(0.0f);
    }
    ChoreConsumer component = this.GetComponent<ChoreConsumer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.AddProvider((ChoreProvider) GlobalChoreProvider.Instance);
    this.gameObject.AddComponent<QualityOfLifeNeed>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!((UnityEngine.Object) this.GetComponent<ChoreConsumer>() != (UnityEngine.Object) null))
      return;
    this.Subscribe<MinionModifiers>(1623392196, MinionModifiers.OnDeathDelegate);
    this.Subscribe<MinionModifiers>(-1506069671, MinionModifiers.OnAttachFollowCamDelegate);
    this.Subscribe<MinionModifiers>(-485480405, MinionModifiers.OnDetachFollowCamDelegate);
    this.Subscribe<MinionModifiers>(-1988963660, MinionModifiers.OnBeginChoreDelegate);
    AmountInstance amountInstance = this.GetAmounts().Get("Calories");
    if (amountInstance != null)
      amountInstance.OnMaxValueReached += new System.Action(this.OnMaxCaloriesReached);
    this.transform.SetPosition(this.transform.GetPosition() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.Move)
    });
    this.gameObject.layer = LayerMask.NameToLayer("Default");
    this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
  }

  private AmountInstance AddAmount(Amount amount)
  {
    return this.amounts.Add(new AmountInstance(amount, this.gameObject));
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

  private void OnDeath(object data)
  {
    Debug.LogFormat("OnDeath {0} -- {1} has died!", data, (object) this.name);
    MinionIdentity component = this.GetComponent<MinionIdentity>();
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) component)
        minionIdentity.GetComponent<Effects>().Add("Mourning", true);
    }
  }

  private void OnMaxCaloriesReached() => this.GetComponent<Effects>().Add("WellFed", true);

  private void OnBeginChore(object data)
  {
    Storage component = this.GetComponent<Storage>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.DropAll();
  }

  public override void OnSerialize(BinaryWriter writer) => base.OnSerialize(writer);

  public override void OnDeserialize(IReader reader) => base.OnDeserialize(reader);

  private void OnAttachFollowCam(object data)
  {
    this.GetComponent<Effects>().Add("CenterOfAttention", false);
  }

  private void OnDetachFollowCam(object data)
  {
    this.GetComponent<Effects>().Remove("CenterOfAttention");
  }
}
