// Decompiled with JetBrains decompiler
// Type: QualityOfLifeNeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class QualityOfLifeNeed : Need, ISim4000ms
{
  private AttributeInstance qolAttribute;
  public bool skipUpdate;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.expectationAttribute = this.gameObject.GetAttributes().Add(Db.Get().Attributes.QualityOfLifeExpectation);
    this.Name = (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME;
    this.ExpectationTooltip = string.Format((string) DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_TOOLTIP, (object) Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) this).GetTotalValue());
    this.stressBonus = new Need.ModifierType()
    {
      modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0f, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.GOOD_MODIFIER, is_readonly: false)
    };
    this.stressNeutral = new Need.ModifierType()
    {
      modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.008333334f, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.NEUTRAL_MODIFIER)
    };
    this.stressPenalty = new Need.ModifierType()
    {
      modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.0f, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.BAD_MODIFIER, is_readonly: false),
      statusItem = Db.Get().DuplicantStatusItems.PoorQualityOfLife
    };
    this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(this.gameObject);
  }

  public void Sim4000ms(float dt)
  {
    if (this.skipUpdate)
      return;
    float num1 = 0.004166667f;
    float b = 0.0416666679f;
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Morale);
    if (currentQualitySetting.id == "Disabled")
    {
      this.SetModifier((Need.ModifierType) null);
    }
    else
    {
      if (currentQualitySetting.id == "Easy")
      {
        num1 = 1f / 300f;
        b = 0.0166666675f;
      }
      else if (currentQualitySetting.id == "Hard")
      {
        num1 = 0.008333334f;
        b = 0.05f;
      }
      else if (currentQualitySetting.id == "VeryHard")
      {
        num1 = 0.0166666675f;
        b = 0.0833333358f;
      }
      float totalValue1 = this.qolAttribute.GetTotalValue();
      float totalValue2 = this.expectationAttribute.GetTotalValue();
      float num2 = totalValue2 - totalValue1;
      if ((double) totalValue1 < (double) totalValue2)
      {
        this.stressPenalty.modifier.SetValue(Mathf.Min(num2 * num1, b));
        this.SetModifier(this.stressPenalty);
      }
      else if ((double) totalValue1 > (double) totalValue2)
      {
        this.stressBonus.modifier.SetValue(Mathf.Max((float) (-(double) num2 * -0.01666666753590107), -0.0333333351f));
        this.SetModifier(this.stressBonus);
      }
      else
        this.SetModifier(this.stressNeutral);
    }
  }
}
