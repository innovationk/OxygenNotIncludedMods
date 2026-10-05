// Decompiled with JetBrains decompiler
// Type: EquipmentDef
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;

#nullable disable
public class EquipmentDef : Def
{
  public string Id;
  public string Slot;
  public string FabricatorId;
  public float FabricationTime;
  public string RecipeTechUnlock;
  public SimHashes OutputElement;
  public Dictionary<string, float> InputElementMassMap;
  public float Mass;
  public KAnimFile Anim;
  public string SnapOn;
  public string SnapOn1;
  public KAnimFile BuildOverride;
  public int BuildOverridePriority;
  public bool IsBody;
  public List<AttributeModifier> AttributeModifiers;
  public string RecipeDescription;
  public List<Klei.AI.Effect> EffectImmunites = new List<Klei.AI.Effect>();
  public Action<Equippable> OnEquipCallBack;
  public Action<Equippable> OnUnequipCallBack;
  public EntityTemplates.CollisionShape CollisionShape;
  public float width;
  public float height = 0.325f;
  public Tag[] AdditionalTags;
  public string wornID;
  public List<Descriptor> additionalDescriptors = new List<Descriptor>();

  public override string Name
  {
    get => (string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{this.Id.ToUpper()}.NAME");
  }

  public string Desc => (string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{this.Id.ToUpper()}.DESC");

  public string Effect
  {
    get => (string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{this.Id.ToUpper()}.EFFECT");
  }

  public string GenericName
  {
    get => (string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{this.Id.ToUpper()}.GENERICNAME");
  }

  public string WornName
  {
    get => (string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{this.Id.ToUpper()}.WORN_NAME");
  }

  public string WornDesc
  {
    get => (string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{this.Id.ToUpper()}.WORN_DESC");
  }
}
