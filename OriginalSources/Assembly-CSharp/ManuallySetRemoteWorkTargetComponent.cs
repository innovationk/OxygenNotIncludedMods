// Decompiled with JetBrains decompiler
// Type: ManuallySetRemoteWorkTargetComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ManuallySetRemoteWorkTargetComponent : RemoteDockWorkTargetComponent
{
  private Chore chore;

  public override Chore RemoteDockChore => this.chore;

  public void SetChore(Chore chore_) => this.chore = chore_;
}
