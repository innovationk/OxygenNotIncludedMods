// Decompiled with JetBrains decompiler
// Type: Database.SkillGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class SkillGroup : Resource, IListableOption
{
  public string choreGroupID;
  public List<Attribute> relevantAttributes;
  public List<string> requiredChoreGroups;
  public string choreGroupIcon;
  public string archetypeIcon;
  public bool allowAsAptitude = true;

  string IListableOption.GetProperName()
  {
    return (string) Strings.Get($"STRINGS.DUPLICANTS.SKILLGROUPS.{this.Id.ToUpper()}.NAME");
  }

  public SkillGroup(
    string id,
    string choreGroupID,
    string name,
    string icon,
    string archetype_icon)
    : base(id, name)
  {
    this.choreGroupID = choreGroupID;
    this.choreGroupIcon = icon;
    this.archetypeIcon = archetype_icon;
  }
}
