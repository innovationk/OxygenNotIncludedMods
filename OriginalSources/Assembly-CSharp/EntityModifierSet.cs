// Decompiled with JetBrains decompiler
// Type: EntityModifierSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

#nullable disable
public class EntityModifierSet : ModifierSet
{
  public DuplicantStatusItems DuplicantStatusItems;
  public ChoreGroups ChoreGroups;

  public override void Initialize()
  {
    base.Initialize();
    this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
    this.ChoreGroups = new ChoreGroups(this.Root);
    this.LoadTraits();
  }
}
