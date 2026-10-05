// Decompiled with JetBrains decompiler
// Type: Klei.AI.Attribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class Attribute : Resource, IHasDlcRestrictions
{
  private static readonly StandardAttributeFormatter defaultFormatter = new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None);
  public string Description;
  public float BaseValue;
  public Attribute.Display ShowInUI;
  public bool IsTrainable;
  public bool IsProfession;
  public string ProfessionName;
  public List<AttributeConverter> converters = new List<AttributeConverter>();
  public string uiSprite;
  public string thoughtSprite;
  public string uiFullColourSprite;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;
  public IAttributeFormatter formatter;

  public Attribute(
    string id,
    bool is_trainable,
    Attribute.Display show_in_ui,
    bool is_profession,
    float base_value = 0.0f,
    string uiSprite = null,
    string thoughtSprite = null,
    string uiFullColourSprite = null,
    string[] overrideDLCIDs = null)
    : base(id)
  {
    string str = "STRINGS.DUPLICANTS.ATTRIBUTES." + id.ToUpper();
    this.Name = (string) Strings.Get(new StringKey(str + ".NAME"));
    this.ProfessionName = (string) Strings.Get(new StringKey(str + ".NAME"));
    this.Description = (string) Strings.Get(new StringKey(str + ".DESC"));
    this.IsTrainable = is_trainable;
    this.IsProfession = is_profession;
    this.ShowInUI = show_in_ui;
    this.BaseValue = base_value;
    this.formatter = (IAttributeFormatter) Attribute.defaultFormatter;
    this.uiSprite = uiSprite;
    this.thoughtSprite = thoughtSprite;
    this.uiFullColourSprite = uiFullColourSprite;
    this.requiredDlcIds = overrideDLCIDs;
  }

  public Attribute(
    string id,
    string name,
    string profession_name,
    string attribute_description,
    float base_value,
    Attribute.Display show_in_ui,
    bool is_trainable,
    string uiSprite = null,
    string thoughtSprite = null,
    string uiFullColourSprite = null)
    : base(id, name)
  {
    this.Description = attribute_description;
    this.ProfessionName = profession_name;
    this.BaseValue = base_value;
    this.ShowInUI = show_in_ui;
    this.IsTrainable = is_trainable;
    this.uiSprite = uiSprite;
    this.thoughtSprite = thoughtSprite;
    this.uiFullColourSprite = uiFullColourSprite;
    if (!(this.ProfessionName == ""))
      return;
    this.ProfessionName = (string) null;
  }

  public void SetFormatter(IAttributeFormatter formatter) => this.formatter = formatter;

  public AttributeInstance Lookup(Component cmp) => this.Lookup(cmp.gameObject);

  public AttributeInstance Lookup(GameObject go) => go.GetAttributes()?.Get(this);

  public string GetDescription(AttributeInstance instance) => instance.GetDescription();

  public string GetTooltip(AttributeInstance instance) => this.formatter.GetTooltip(this, instance);

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public enum Display
  {
    Normal,
    Skill,
    Expectation,
    General,
    Details,
    Never,
  }
}
