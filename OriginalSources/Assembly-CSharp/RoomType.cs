// Decompiled with JetBrains decompiler
// Type: RoomType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RoomType : Resource
{
  public string tooltip { get; private set; }

  public string description { get; set; }

  public string effect { get; private set; }

  public RoomConstraints.Constraint primary_constraint { get; private set; }

  public RoomConstraints.Constraint[] additional_constraints { get; private set; }

  public int priority { get; private set; }

  public bool single_assignee { get; private set; }

  public RoomDetails.Detail[] display_details { get; private set; }

  public bool priority_building_use { get; private set; }

  public RoomTypeCategory category { get; private set; }

  public RoomType[] upgrade_paths { get; private set; }

  public string[] effects { get; private set; }

  public int sortKey { get; private set; }

  public RoomType(
    string id,
    string name,
    string description,
    string tooltip,
    string effect,
    RoomTypeCategory category,
    RoomConstraints.Constraint primary_constraint,
    RoomConstraints.Constraint[] additional_constraints,
    RoomDetails.Detail[] display_details,
    int priority = 0,
    RoomType[] upgrade_paths = null,
    bool single_assignee = false,
    bool priority_building_use = false,
    string[] effects = null,
    int sortKey = 0)
    : base(id, name)
  {
    this.tooltip = tooltip;
    this.description = description;
    this.effect = effect;
    this.category = category;
    this.primary_constraint = primary_constraint;
    this.additional_constraints = additional_constraints;
    this.display_details = display_details;
    this.priority = priority;
    this.upgrade_paths = upgrade_paths;
    this.single_assignee = single_assignee;
    this.priority_building_use = priority_building_use;
    this.effects = effects;
    this.sortKey = sortKey;
    if (this.upgrade_paths == null)
      return;
    foreach (RoomType upgradePath in this.upgrade_paths)
      Debug.Assert(upgradePath != null, (object) (name + " has a null upgrade path. Maybe it wasn't initialized yet."));
  }

  public RoomType.RoomIdentificationResult isSatisfactory(Room candidate_room)
  {
    if (this.primary_constraint != null && !this.primary_constraint.isSatisfied(candidate_room))
      return RoomType.RoomIdentificationResult.primary_unsatisfied;
    if (this.additional_constraints != null)
    {
      foreach (RoomConstraints.Constraint additionalConstraint in this.additional_constraints)
      {
        if (!additionalConstraint.isSatisfied(candidate_room))
          return RoomType.RoomIdentificationResult.primary_satisfied;
      }
    }
    return RoomType.RoomIdentificationResult.all_satisfied;
  }

  public string GetCriteriaString()
  {
    string str = $"<b>{this.Name}</b>\n{this.tooltip}\n\n{(string) ROOMS.CRITERIA.HEADER}";
    if (this == Db.Get().RoomTypes.Neutral)
      str = $"{str}\n    • {(string) ROOMS.CRITERIA.NEUTRAL_TYPE}";
    string criteriaString = str + (this.primary_constraint == null ? "" : "\n    • " + this.primary_constraint.name);
    if (this.additional_constraints != null)
    {
      foreach (RoomConstraints.Constraint additionalConstraint in this.additional_constraints)
        criteriaString = $"{criteriaString}\n    • {additionalConstraint.name}";
    }
    return criteriaString;
  }

  public string GetRoomEffectsString()
  {
    if (this.effects == null || this.effects.Length == 0)
      return (string) null;
    string header = (string) ROOMS.EFFECTS.HEADER;
    foreach (string effect1 in this.effects)
    {
      Effect effect2 = Db.Get().effects.Get(effect1);
      header += Effect.CreateTooltip(effect2, false, showHeader: false);
    }
    return header;
  }

  public void TriggerRoomEffects(
    KPrefabID triggerer,
    Effects target,
    out List<EffectInstance> result)
  {
    result = (List<EffectInstance>) null;
    if (this.primary_constraint == null || (Object) triggerer == (Object) null || this.effects == null || !this.primary_constraint.building_criteria(triggerer))
      return;
    result = new List<EffectInstance>();
    foreach (string effect in this.effects)
      result.Add(target.Add(effect, true));
  }

  public void TriggerRoomEffects(KPrefabID triggerer, Effects target)
  {
    this.TriggerRoomEffects(triggerer, target, out List<EffectInstance> _);
  }

  public enum RoomIdentificationResult
  {
    all_satisfied,
    primary_satisfied,
    primary_unsatisfied,
  }
}
