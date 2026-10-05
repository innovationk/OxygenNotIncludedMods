// Decompiled with JetBrains decompiler
// Type: RemoteWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public abstract class RemoteWorkable : Workable, IRemoteDockWorkTarget
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.RemoteDockWorkTargets.Add(this.gameObject.GetMyWorldId(), (IRemoteDockWorkTarget) this);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.RemoteDockWorkTargets.Remove(this.gameObject.GetMyWorldId(), (IRemoteDockWorkTarget) this);
  }

  public abstract Chore RemoteDockChore { get; }

  public virtual IApproachable Approachable => (IApproachable) this;
}
