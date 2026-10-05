// Decompiled with JetBrains decompiler
// Type: QuestCriteria_LessThan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class QuestCriteria_LessThan(
  Tag id,
  float[] targetValues,
  int requiredCount = 1,
  HashSet<Tag> acceptedTags = null,
  QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : QuestCriteria(id, targetValues, requiredCount, acceptedTags, flags)
{
  protected override bool ValueSatisfies_Internal(float current, float target)
  {
    return (double) current < (double) target;
  }
}
