// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Klei.AI;

[DebuggerDisplay("{AttributeId}")]
public class AttributeModifier
{
  public Func<string> NameCB;
  public string Description;
  public Func<string> DescriptionCB;

  public string AttributeId { get; private set; }

  public float Value { get; private set; }

  public bool IsMultiplier { get; private set; }

  public GameUtil.TimeSlice? OverrideTimeSlice { get; set; }

  public bool UIOnly { get; private set; }

  public bool IsReadonly { get; private set; }

  public AttributeModifier(
    string attribute_id,
    float value,
    string description = null,
    bool is_multiplier = false,
    bool uiOnly = false,
    bool is_readonly = true)
  {
    this.AttributeId = attribute_id;
    this.Value = value;
    this.Description = description == null ? attribute_id : description;
    this.DescriptionCB = (Func<string>) null;
    this.IsMultiplier = is_multiplier;
    this.UIOnly = uiOnly;
    this.IsReadonly = is_readonly;
    this.OverrideTimeSlice = new GameUtil.TimeSlice?();
  }

  public AttributeModifier(
    string attribute_id,
    float value,
    Func<string> description_cb,
    bool is_multiplier = false,
    bool uiOnly = false)
    : this(attribute_id, value, (Func<string>) null, description_cb, is_multiplier, uiOnly)
  {
  }

  public AttributeModifier(
    string attribute_id,
    float value,
    Func<string> name_cb,
    Func<string> description_cb,
    bool is_multiplier = false,
    bool uiOnly = false)
  {
    this.AttributeId = attribute_id;
    this.Value = value;
    this.NameCB = name_cb;
    this.DescriptionCB = description_cb;
    this.Description = (string) null;
    this.IsMultiplier = is_multiplier;
    this.UIOnly = uiOnly;
    this.OverrideTimeSlice = new GameUtil.TimeSlice?();
    if (description_cb != null)
      return;
    Debug.LogWarning((object) ("AttributeModifier being constructed without a description callback: " + attribute_id));
  }

  public void Reconstruct(
    string attribute_id,
    float value,
    string description = null,
    bool is_multiplier = false,
    bool uiOnly = false,
    bool is_readonly = true)
  {
    this.AttributeId = attribute_id;
    this.Value = value;
    this.Description = description == null ? attribute_id : description;
    this.DescriptionCB = (Func<string>) null;
    this.NameCB = (Func<string>) null;
    this.IsMultiplier = is_multiplier;
    this.UIOnly = uiOnly;
    this.IsReadonly = is_readonly;
    this.OverrideTimeSlice = new GameUtil.TimeSlice?();
  }

  public void SetValue(float value) => this.Value = value;

  public static Attribute FetchAttribute(string attributeId)
  {
    return Db.Get().Attributes.TryGet(attributeId) ?? Db.Get().BuildingAttributes.TryGet(attributeId) ?? Db.Get().PlantAttributes.TryGet(attributeId) ?? Db.Get().CritterAttributes.TryGet(attributeId) ?? (Attribute) null;
  }

  private Attribute FetchAttribute() => AttributeModifier.FetchAttribute(this.AttributeId);

  public string GetName()
  {
    Attribute attribute = this.FetchAttribute();
    if (attribute == null || attribute.ShowInUI == Attribute.Display.Never)
      return "";
    return this.NameCB != null ? this.NameCB() : attribute.Name;
  }

  public string GetDescription()
  {
    return this.DescriptionCB == null ? this.Description : this.DescriptionCB();
  }

  public string GetFormattedString()
  {
    Attribute attribute = this.FetchAttribute();
    IAttributeFormatter formatter = this.IsMultiplier || attribute == null ? (IAttributeFormatter) null : attribute.formatter;
    string str = "";
    string text = formatter == null ? (!this.IsMultiplier ? str + GameUtil.GetFormattedSimple(this.Value) : str + GameUtil.GetFormattedPercent(this.Value * 100f)) : formatter.GetFormattedModifier(this);
    if (text != null && text.Length > 0 && text[0] != '-')
    {
      GameUtil.TimeSlice? overrideTimeSlice = this.OverrideTimeSlice;
      GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None;
      if (!(overrideTimeSlice.GetValueOrDefault() == timeSlice & overrideTimeSlice.HasValue))
        text = GameUtil.AddPositiveSign(text, (double) this.Value > 0.0);
    }
    return text;
  }

  public AttributeModifier Clone()
  {
    return new AttributeModifier(this.AttributeId, this.Value, this.Description);
  }
}
