// Decompiled with JetBrains decompiler
// Type: DevToolEntityDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using ImGuiNET;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevToolEntityDebug : DevTool
{
  private bool lockSelection;
  private GameObject lockedObject;
  private Dictionary<string, bool> expandedAttributes = new Dictionary<string, bool>();

  public DevToolEntityDebug() => this.RequiresGameRunning = true;

  protected override void RenderTo(DevPanel panel)
  {
    if ((UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null)
    {
      ImGui.Text("SelectTool not available.");
    }
    else
    {
      ImGui.Checkbox("Lock Selection", ref this.lockSelection);
      GameObject gameObject = (GameObject) null;
      if (this.lockSelection && (UnityEngine.Object) this.lockedObject != (UnityEngine.Object) null)
      {
        gameObject = this.lockedObject;
      }
      else
      {
        KSelectable selected = SelectTool.Instance.selected;
        if (!selected.IsNullOrDestroyed())
        {
          gameObject = selected.gameObject;
          if (this.lockSelection)
            this.lockedObject = gameObject;
        }
      }
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        ImGui.Text("Nothing selected.");
      }
      else
      {
        Modifiers component = gameObject.GetComponent<Modifiers>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        {
          ImGui.Text("Selected object has no Modifiers component.");
        }
        else
        {
          this.Name = "Entity Debug: " + gameObject.name;
          if ((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null)
            ImGui.Text($"GameTime: {GameClock.Instance.GetTime() / 600f:F2} cycles");
          ImGui.Separator();
          this.DrawAmounts(component);
          this.DrawEffects(component);
          this.DrawAttributes(component);
          this.DrawAttributeLevels(component);
          this.DrawTraits(component);
          this.DrawDeaths(component);
          this.DrawUrges(component);
          this.DrawDiseases(component);
          this.DrawSicknesses(component);
          this.DrawResume(component);
          this.DrawStomach(component);
        }
      }
    }
  }

  private void DrawAmounts(Modifiers entity)
  {
    if (entity.GetAmounts() == null || !ImGui.CollapsingHeader("Amounts (Min/Max/Delta)"))
      return;
    List<AmountInstance> amountInstanceList = new List<AmountInstance>((IEnumerable<AmountInstance>) entity.GetAmounts().ModifierList);
    amountInstanceList.Sort((Comparison<AmountInstance>) ((x, y) => x.amount.Id.CompareTo(y.amount.Id)));
    foreach (AmountInstance instance in amountInstanceList)
    {
      string label = $"{instance.amount.Id} ({instance.GetMin()}/{instance.GetMax()}/{instance.GetDelta():F2})";
      float num = instance.value;
      ref float local = ref num;
      double min = (double) instance.GetMin();
      double max = (double) instance.GetMax();
      if (ImGui.DragFloat(label, ref local, 0.1f, (float) min, (float) max))
        instance.amount.DebugSetValue(instance, num);
    }
  }

  private void DrawEffects(Modifiers entity)
  {
    Effects component = entity.GetComponent<Effects>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !ImGui.CollapsingHeader("Effects"))
      return;
    List<Effect> effectList = new List<Effect>((IEnumerable<Effect>) Db.Get().effects.resources);
    effectList.Sort((Comparison<Effect>) ((x, y) => x.Name.CompareTo(y.Name)));
    foreach (Effect effect in effectList)
    {
      if (effect != null)
      {
        bool v = component.HasEffect(effect);
        if (ImGui.Checkbox(effect.Name, ref v))
        {
          if (v)
            component.Add(effect, false);
          else
            component.Remove(effect);
        }
        if (v)
        {
          ImGui.SameLine();
          EffectInstance effectInstance = component.Get(effect);
          float timeRemaining = effectInstance.timeRemaining;
          ImGui.SetNextItemWidth(100f);
          if (ImGui.DragFloat("##time_" + effect.Id, ref timeRemaining, 0.1f, 0.0f, float.MaxValue, "%.1f"))
            effectInstance.timeRemaining = timeRemaining;
        }
      }
    }
  }

  private void DrawAttributes(Modifiers entity)
  {
    if (entity.GetAttributes() == null || !ImGui.CollapsingHeader("Attributes"))
      return;
    foreach (AttributeInstance attribute in entity.GetAttributes())
    {
      float totalValue = attribute.GetTotalValue();
      string fmt = $"{attribute.Attribute.Id}: {totalValue} ({(ValueType) (float) ((double) totalValue * 600.0)}/cycle)";
      if (attribute.Modifiers.Count > 0)
      {
        bool flag = false;
        this.expandedAttributes.TryGetValue(attribute.Attribute.Id, out flag);
        if (ImGui.TreeNode(attribute.Attribute.Id, fmt))
        {
          this.expandedAttributes[attribute.Attribute.Id] = true;
          for (int i = 0; i < attribute.Modifiers.Count; ++i)
          {
            AttributeModifier modifier = attribute.Modifiers[i];
            string str = modifier.IsMultiplier ? " x " : "";
            ImGui.Text($"  {modifier.GetDescription()}: {str}{modifier.Value} ({(ValueType) (float) ((double) modifier.Value * 600.0)}/cycle)");
          }
          ImGui.TreePop();
        }
        else
          this.expandedAttributes[attribute.Attribute.Id] = false;
      }
      else
        ImGui.Text(fmt);
    }
  }

  private void DrawAttributeLevels(Modifiers entity)
  {
    AttributeLevels component = entity.GetComponent<AttributeLevels>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !ImGui.CollapsingHeader("Attribute Levels"))
      return;
    foreach (AttributeLevel attributeLevel in component)
    {
      ImGui.Text($"{$"{attributeLevel.attribute.Attribute.Id} Lv{attributeLevel.GetLevel()}"}: {$"{attributeLevel.experience:F0}/{attributeLevel.GetExperienceForNextLevel():F0} ({attributeLevel.GetPercentComplete():F3})"}");
      ImGui.SameLine();
      if (ImGui.SmallButton("+##" + attributeLevel.attribute.Attribute.Id))
        attributeLevel.LevelUp(component);
    }
  }

  private void DrawTraits(Modifiers entity)
  {
    Traits component = entity.GetComponent<Traits>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !ImGui.CollapsingHeader("Traits"))
      return;
    List<Trait> traitList = new List<Trait>((IEnumerable<Trait>) Db.Get().traits.resources);
    traitList.Sort((Comparison<Trait>) ((a, b) => UI.StripLinkFormatting(a.Name).CompareTo(UI.StripLinkFormatting(b.Name))));
    foreach (Trait trait in traitList)
    {
      bool v = component.HasTrait(trait);
      if (ImGui.Checkbox(UI.StripLinkFormatting(trait.Name), ref v))
      {
        if (v)
          component.Add(trait);
        else
          component.Remove(trait);
      }
    }
  }

  private void DrawDeaths(Modifiers entity)
  {
    DeathMonitor.Instance smi = entity.GetSMI<DeathMonitor.Instance>();
    if (smi == null || !ImGui.CollapsingHeader("Deaths"))
      return;
    foreach (Death resource in Db.Get().Deaths.resources)
    {
      if (ImGui.Button(resource.Id))
        smi.Kill(resource);
    }
  }

  private void DrawUrges(Modifiers entity)
  {
    ChoreConsumer component = entity.GetComponent<ChoreConsumer>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !ImGui.CollapsingHeader("Urges"))
      return;
    foreach (Urge resource in Db.Get().Urges.resources)
    {
      bool v = component.HasUrge(resource);
      if (ImGui.Checkbox(resource.Name, ref v))
      {
        if (v)
          component.AddUrge(resource);
        else
          component.RemoveUrge(resource);
      }
    }
  }

  private void DrawDiseases(Modifiers entity)
  {
    if (!ImGui.CollapsingHeader("Diseases"))
      return;
    Diseases diseases = Db.Get().Diseases;
    PrimaryElement component = entity.gameObject.GetComponent<PrimaryElement>();
    for (int index = 0; index < diseases.Count; ++index)
    {
      Klei.AI.Disease disease = diseases[index];
      int diseaseCount = (int) component.DiseaseIdx == index ? component.DiseaseCount : 0;
      ImGui.Text($"{Util.StripTextFormatting(disease.Name)}: {diseaseCount}");
      ImGui.SameLine();
      if (ImGui.SmallButton("Add 100##" + disease.Id))
        component.AddDisease((byte) index, 100, "debug");
    }
  }

  private void DrawSicknesses(Modifiers entity)
  {
    MinionModifiers component = entity.GetComponent<MinionModifiers>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !ImGui.CollapsingHeader("Sicknesses"))
      return;
    Database.Sicknesses sicknesses1 = Db.Get().Sicknesses;
    Klei.AI.Sicknesses sicknesses2 = component.sicknesses;
    for (int idx = 0; idx < sicknesses1.Count; ++idx)
    {
      Sickness modifier = sicknesses1[idx];
      string str = Util.StripTextFormatting(modifier.Name);
      SicknessInstance sicknessInstance = sicknesses2.Get(modifier);
      bool v = sicknessInstance != null;
      if (ImGui.Checkbox(str + "##sick", ref v))
      {
        if (v)
          sicknesses2.Infect(new SicknessExposureInfo(modifier.Id, "debug menu"));
        else
          sicknessInstance.Cure();
      }
      if (v && sicknessInstance != null)
      {
        ImGui.SameLine();
        float percentCured = sicknessInstance.GetPercentCured();
        ImGui.SetNextItemWidth(100f);
        if (ImGui.DragFloat("##cure_" + modifier.Id, ref percentCured, 0.01f, 0.0f, 1f, "%.2f"))
          sicknessInstance.SetPercentCured(percentCured);
      }
    }
  }

  private void DrawResume(Modifiers entity)
  {
    MinionResume component = entity.GetComponent<MinionResume>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !ImGui.CollapsingHeader("Resume"))
      return;
    float experienceGained = component.TotalExperienceGained;
    if (ImGui.DragFloat("Total Experience", ref experienceGained, 1f))
      component.AddExperience(experienceGained - component.TotalExperienceGained);
    ImGui.Text($"Next Level: {MinionResume.CalculateNextExperienceBar(component.TotalSkillPointsGained)}");
    ImGui.Text($"Total Skill Points: {component.TotalSkillPointsGained}");
    ImGui.SameLine();
    if (ImGui.SmallButton("+##skillpoint"))
      component.ForceAddSkillPoint();
    ImGui.Separator();
    foreach (Skill skill in new List<Skill>((IEnumerable<Skill>) Db.Get().Skills.resources))
    {
      bool v = component.MasteryBySkillID.ContainsKey(skill.Id) && component.MasteryBySkillID[skill.Id];
      if (ImGui.Checkbox($"{UI.StripLinkFormatting(skill.Name)}##skill_{skill.Id}", ref v))
      {
        if (v)
          component.MasterSkill(skill.Id);
        else
          component.UnmasterSkill(skill.Id);
      }
    }
  }

  private void DrawStomach(Modifiers entity)
  {
    CreatureCalorieMonitor.Instance smi = entity.GetSMI<CreatureCalorieMonitor.Instance>();
    if (smi == null || !ImGui.CollapsingHeader("Stomach"))
      return;
    CreatureCalorieMonitor.Stomach stomach = smi.stomach;
    ImGui.Text($"Fullness: {stomach.GetFullness()}");
    ImGui.Text($"Hunger {(ValueType) (float) ((double) smi.calories.GetMax() * (double) smi.HungryRatio)}: {(ValueType) (float) (((double) smi.calories.GetMax() - (double) smi.calories.value) / ((double) smi.calories.GetMax() * (1.0 - (double) smi.HungryRatio)))}");
    List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> calorieEntries = stomach.GetCalorieEntries();
    for (int index = 0; index < calorieEntries.Count; ++index)
    {
      CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = calorieEntries[index];
      float calories = caloriesConsumedEntry.calories;
      if (ImGui.DragFloat(caloriesConsumedEntry.tag.Name, ref calories, 0.1f))
      {
        caloriesConsumedEntry.calories = calories;
        calorieEntries[index] = caloriesConsumedEntry;
      }
    }
  }
}
