// Decompiled with JetBrains decompiler
// Type: QuestCriteria
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class QuestCriteria
{
  public const int MAX_VALUES = 32 /*0x20*/;
  public const int INVALID_VALUE = -1;
  public readonly Tag CriteriaId;
  public readonly QuestCriteria.BehaviorFlags EvaluationBehaviors;
  public readonly float[] TargetValues;
  public readonly int RequiredCount = 1;
  public readonly HashSet<Tag> AcceptedTags;

  public string Text { get; private set; }

  public string Tooltip { get; private set; }

  public QuestCriteria(
    Tag id,
    float[] targetValues = null,
    int requiredCount = 1,
    HashSet<Tag> acceptedTags = null,
    QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.None)
  {
    Debug.Assert(targetValues == null || targetValues.Length != 0 && targetValues.Length <= 32 /*0x20*/);
    this.CriteriaId = id;
    this.EvaluationBehaviors = flags;
    this.TargetValues = targetValues;
    this.AcceptedTags = acceptedTags;
    this.RequiredCount = requiredCount;
  }

  public bool ValueSatisfies(float value, int valueHandle)
  {
    if (float.IsNaN(value))
      return false;
    float target = this.TargetValues == null ? 0.0f : this.TargetValues[valueHandle];
    return this.ValueSatisfies_Internal(value, target);
  }

  protected virtual bool ValueSatisfies_Internal(float current, float target) => true;

  public bool IsSatisfied(uint satisfactionState, uint satisfactionMask)
  {
    return ((int) satisfactionState & (int) satisfactionMask) == (int) satisfactionMask;
  }

  public void PopulateStrings(string prefix)
  {
    string upperInvariant = this.CriteriaId.Name.ToUpperInvariant();
    StringEntry result;
    if (Strings.TryGet($"{prefix}CRITERIA.{upperInvariant}.NAME", out result))
      this.Text = result.String;
    if (!Strings.TryGet($"{prefix}CRITERIA.{upperInvariant}.TOOLTIP", out result))
      return;
    this.Tooltip = result.String;
  }

  public uint GetSatisfactionMask()
  {
    return this.TargetValues == null ? 1U : (uint) Mathf.Pow(2f, (float) (this.TargetValues.Length - 1));
  }

  public uint GetValueMask(int valueHandle)
  {
    if (this.TargetValues == null)
      return 1;
    if (!QuestCriteria.HasBehavior(this.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
      valueHandle %= this.TargetValues.Length;
    return (uint) (1 << valueHandle);
  }

  public static bool HasBehavior(
    QuestCriteria.BehaviorFlags flags,
    QuestCriteria.BehaviorFlags behavior)
  {
    return (flags & behavior) == behavior;
  }

  public enum BehaviorFlags
  {
    None = 0,
    TrackArea = 1,
    AllowsRegression = 2,
    TrackValues = 4,
    TrackItems = 8,
    UniqueItems = 24, // 0x00000018
  }
}
