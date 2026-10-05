// Decompiled with JetBrains decompiler
// Type: Klei.AI.TraitGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Klei.AI;

public class TraitGroup : ModifierGroup<Trait>
{
  public bool IsSpawnTrait;

  public TraitGroup(string id, string name, bool is_spawn_trait)
    : base(id, name)
  {
    this.IsSpawnTrait = is_spawn_trait;
  }
}
