// Decompiled with JetBrains decompiler
// Type: MinionPersonalityPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MinionPersonalityPanel : DetailScreenTab
{
  private CollapsibleDetailContentPanel bioPanel;
  private CollapsibleDetailContentPanel traitsPanel;
  private CollapsibleDetailContentPanel resumePanel;
  private CollapsibleDetailContentPanel attributesPanel;
  private CollapsibleDetailContentPanel equipmentPanel;
  private CollapsibleDetailContentPanel amenitiesPanel;
  private SchedulerHandle updateHandle;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<MinionIdentity>() != (UnityEngine.Object) null;
  }

  protected override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    this.Refresh();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.bioPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.PERSONALITY.GROUPNAME_BIO);
    this.traitsPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.STATS.GROUPNAME_TRAITS);
    this.attributesPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.STATS.GROUPNAME_ATTRIBUTES);
    this.resumePanel = this.CreateCollapsableSection((string) UI.DETAILTABS.PERSONALITY.GROUPNAME_RESUME);
    this.amenitiesPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_ROOMS);
    this.equipmentPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_OWNABLE);
  }

  protected override void OnCleanUp()
  {
    this.updateHandle.ClearScheduler();
    base.OnCleanUp();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Refresh();
    this.ScheduleUpdate();
  }

  private void ScheduleUpdate()
  {
    this.updateHandle = UIScheduler.Instance.Schedule("RefreshMinionPersonalityPanel", 1f, (Action<object>) (o =>
    {
      this.Refresh();
      this.ScheduleUpdate();
    }), (object) null, (SchedulerGroup) null);
  }

  private void Refresh()
  {
    if (!this.gameObject.activeSelf || (UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null || (UnityEngine.Object) this.selectedTarget.GetComponent<MinionIdentity>() == (UnityEngine.Object) null)
      return;
    MinionPersonalityPanel.RefreshBioPanel(this.bioPanel, this.selectedTarget);
    MinionPersonalityPanel.RefreshTraitsPanel(this.traitsPanel, this.selectedTarget);
    MinionPersonalityPanel.RefreshAmenitiesPanel(this.amenitiesPanel, this.selectedTarget);
    MinionPersonalityPanel.RefreshEquipmentPanel(this.equipmentPanel, this.selectedTarget);
    MinionPersonalityPanel.RefreshResumePanel(this.resumePanel, this.selectedTarget);
    MinionPersonalityPanel.RefreshAttributesPanel(this.attributesPanel, this.selectedTarget);
  }

  private static void RefreshBioPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    MinionIdentity component1 = targetEntity.GetComponent<MinionIdentity>();
    if (!(bool) (UnityEngine.Object) component1)
    {
      targetPanel.SetActive(false);
    }
    else
    {
      targetPanel.SetActive(true);
      targetPanel.SetLabel("name", (string) DUPLICANTS.NAMETITLE + component1.name, "");
      targetPanel.SetLabel("model", (string) DUPLICANTS.MODELTITLE + component1.model.ProperName(), GameTags.Minions.Models.GetModelTooltipForTag(component1.model));
      targetPanel.SetLabel("age", (string) DUPLICANTS.ARRIVALTIME + GameUtil.GetFormattedCycles((float) (((double) GameClock.Instance.GetCycle() - (double) component1.arrivalTime) * 600.0), "F0", true), string.Format((string) DUPLICANTS.ARRIVALTIME_TOOLTIP, (object) (float) ((double) component1.arrivalTime + 1.0), (object) component1.name));
      targetPanel.SetLabel("gender", (string) DUPLICANTS.GENDERTITLE + string.Format((string) Strings.Get($"STRINGS.DUPLICANTS.GENDER.{component1.genderStringKey.ToUpper()}.NAME"), (object) component1.gender), "");
      targetPanel.SetLabel("personality", string.Format((string) Strings.Get($"STRINGS.DUPLICANTS.PERSONALITIES.{component1.nameStringKey.ToUpper()}.DESC"), (object) component1.name), string.Format((string) Strings.Get(string.Format("STRINGS.DUPLICANTS.DESC_TOOLTIP", (object) component1.nameStringKey.ToUpper())), (object) component1.name));
      MinionResume component2 = targetEntity.GetComponent<MinionResume>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AptitudeBySkillGroup.Count > 0)
      {
        targetPanel.SetLabel("interestHeader", (string) UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.NAME + "\n", string.Format((string) UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.TOOLTIP, (object) targetEntity.name));
        foreach (KeyValuePair<HashedString, float> keyValuePair in component2.AptitudeBySkillGroup)
        {
          if ((double) keyValuePair.Value != 0.0)
          {
            SkillGroup skillGroup = Db.Get().SkillGroups.TryGet(keyValuePair.Key);
            if (skillGroup != null)
              targetPanel.SetLabel(skillGroup.Name, "  • " + skillGroup.Name, string.Format((string) DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, (object) skillGroup.Name, (object) keyValuePair.Value));
          }
        }
      }
      targetPanel.Commit();
    }
  }

  private static void RefreshTraitsPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    if (!(bool) (UnityEngine.Object) targetEntity.GetComponent<MinionIdentity>())
    {
      targetPanel.SetActive(false);
    }
    else
    {
      targetPanel.SetActive(true);
      foreach (Trait trait in targetEntity.GetComponent<Traits>().TraitList)
      {
        if (!string.IsNullOrEmpty(trait.Name))
          targetPanel.SetLabel(trait.Id, trait.Name, trait.GetTooltip());
      }
      targetPanel.Commit();
    }
  }

  private static void RefreshEquipmentPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    Equipment equipment = targetEntity.GetComponent<MinionIdentity>().GetEquipment();
    bool flag = false;
    foreach (AssignableSlotInstance slot in equipment.Slots)
    {
      if (slot.slot.showInUI && slot.IsAssigned())
      {
        flag = true;
        string name = slot.assignable.GetComponent<KSelectable>().GetName();
        string str = "";
        List<Descriptor> descriptorList = new List<Descriptor>((IEnumerable<Descriptor>) GameUtil.GetGameObjectEffects(slot.assignable.gameObject));
        if (descriptorList.Count > 0)
        {
          str += "\n";
          foreach (Descriptor descriptor in descriptorList)
            str = $"{str}  • {descriptor.IndentedText()}\n";
        }
        targetPanel.SetLabel(slot.slot.Name, $"{slot.slot.Name}: {name}", string.Format((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.ASSIGNED_TOOLTIP, (object) name, (object) str, (object) targetEntity.GetProperName()));
      }
    }
    if (!flag)
      targetPanel.SetLabel("NoSuitAssigned", (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT, string.Format((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT_TOOLTIP, (object) targetEntity.GetProperName()));
    targetPanel.Commit();
  }

  private static void RefreshAmenitiesPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    Ownables soleOwner = targetEntity.GetComponent<MinionIdentity>().GetSoleOwner();
    bool flag = false;
    foreach (AssignableSlotInstance slot in soleOwner.Slots)
    {
      if (slot.slot.showInUI && slot.IsAssigned())
      {
        flag = true;
        string name = slot.assignable.GetComponent<KSelectable>().GetName();
        string str = "";
        List<Descriptor> descriptorList = new List<Descriptor>((IEnumerable<Descriptor>) GameUtil.GetGameObjectEffects(slot.assignable.gameObject));
        if (descriptorList.Count > 0)
        {
          str += "\n";
          foreach (Descriptor descriptor in descriptorList)
            str = $"{str}  • {descriptor.IndentedText()}\n";
        }
        targetPanel.SetLabel(slot.slot.Name, $"{slot.slot.Name}: {name}", string.Format((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.ASSIGNED_TOOLTIP, (object) name, (object) str, (object) targetEntity.GetProperName()));
      }
    }
    if (!flag)
      targetPanel.SetLabel("NothingAssigned", (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES, string.Format((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES_TOOLTIP, (object) targetEntity.GetProperName()));
    targetPanel.Commit();
  }

  private static void RefreshAttributesPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    if (!(bool) (UnityEngine.Object) targetEntity.GetComponent<MinionIdentity>())
    {
      targetPanel.SetActive(false);
    }
    else
    {
      List<AttributeInstance> all = new List<AttributeInstance>((IEnumerable<AttributeInstance>) targetEntity.GetAttributes().AttributeTable).FindAll((Predicate<AttributeInstance>) (a => a.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill));
      if (all.Count > 0)
      {
        foreach (AttributeInstance attributeInstance in all)
          targetPanel.SetLabel(attributeInstance.Id, $"{attributeInstance.Name}: {attributeInstance.GetFormattedValue()}", attributeInstance.GetAttributeValueTooltip());
      }
      targetPanel.Commit();
    }
  }

  private static void RefreshResumePanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    MinionResume component = targetEntity.GetComponent<MinionResume>();
    targetPanel.SetTitle(string.Format((string) UI.DETAILTABS.PERSONALITY.GROUPNAME_RESUME, (object) targetEntity.name.ToUpper()));
    List<Skill> skillList = new List<Skill>();
    foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
    {
      if (keyValuePair.Value)
      {
        Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
        skillList.Add(skill);
      }
    }
    targetPanel.SetLabel("mastered_skills_header", (string) UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS, (string) UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS_TOOLTIP);
    if (skillList.Count == 0)
    {
      targetPanel.SetLabel("no_skills", "    • " + (string) UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.NAME, string.Format((string) UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.TOOLTIP, (object) targetEntity.name));
    }
    else
    {
      foreach (Skill restrictions in skillList)
      {
        if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
        {
          string str = "";
          foreach (SkillPerk perk in restrictions.perks)
          {
            if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) perk))
              str = $"{str}    • {perk.Name}\n";
          }
          targetPanel.SetLabel(restrictions.Id, "    • " + restrictions.Name, $"{restrictions.description}\n{str}");
        }
      }
    }
    targetPanel.Commit();
  }
}
