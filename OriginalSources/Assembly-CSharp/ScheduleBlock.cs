// Decompiled with JetBrains decompiler
// Type: ScheduleBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ScheduleBlock
{
  [Serialize]
  public string name;
  [Serialize]
  private string _groupId;

  public List<ScheduleBlockType> allowed_types
  {
    get
    {
      Debug.Assert(!string.IsNullOrEmpty(this._groupId));
      return Db.Get().ScheduleGroups.Get(this._groupId).allowedTypes;
    }
  }

  public string GroupId
  {
    set => this._groupId = value;
    get => this._groupId;
  }

  public ScheduleBlock(string name, string groupId)
  {
    this.name = name;
    this._groupId = groupId;
  }

  public bool IsAllowed(ScheduleBlockType type)
  {
    if (this.allowed_types != null)
    {
      foreach (ScheduleBlockType allowedType in this.allowed_types)
      {
        if (type.IdHash == allowedType.IdHash)
          return true;
      }
    }
    return false;
  }
}
