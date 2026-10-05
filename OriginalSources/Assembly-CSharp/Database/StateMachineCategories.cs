// Decompiled with JetBrains decompiler
// Type: Database.StateMachineCategories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class StateMachineCategories : ResourceSet<StateMachine.Category>
{
  public StateMachine.Category Ai;
  public StateMachine.Category Monitor;
  public StateMachine.Category Chore;
  public StateMachine.Category Misc;

  public StateMachineCategories()
  {
    this.Ai = this.Add(new StateMachine.Category(nameof (Ai)));
    this.Monitor = this.Add(new StateMachine.Category(nameof (Monitor)));
    this.Chore = this.Add(new StateMachine.Category(nameof (Chore)));
    this.Misc = this.Add(new StateMachine.Category(nameof (Misc)));
  }
}
