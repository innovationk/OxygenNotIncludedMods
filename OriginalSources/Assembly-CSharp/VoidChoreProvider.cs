// Decompiled with JetBrains decompiler
// Type: VoidChoreProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class VoidChoreProvider : ChoreProvider
{
  public static VoidChoreProvider Instance;

  public static void DestroyInstance() => VoidChoreProvider.Instance = (VoidChoreProvider) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    VoidChoreProvider.Instance = this;
  }

  public override void AddChore(Chore chore)
  {
  }

  public override void RemoveChore(Chore chore)
  {
  }

  public override void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded,
    List<Chore.Precondition.Context> failed_contexts)
  {
  }
}
