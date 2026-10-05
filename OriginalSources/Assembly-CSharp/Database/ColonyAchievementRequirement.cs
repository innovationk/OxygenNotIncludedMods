// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievementRequirement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public abstract class ColonyAchievementRequirement
{
  public bool shouldUpdateNameAndDescription;

  public abstract bool Success();

  public virtual bool Fail() => false;

  public virtual string GetProgress(bool complete) => "";
}
