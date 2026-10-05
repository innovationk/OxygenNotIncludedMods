// Decompiled with JetBrains decompiler
// Type: TeleporterWorkableUse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class TeleporterWorkableUse : Workable
{
  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(5f);
    this.resetProgressOnStop = true;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    Teleporter component = this.GetComponent<Teleporter>();
    Teleporter teleportTarget = component.FindTeleportTarget();
    component.SetTeleportTarget(teleportTarget);
    TeleportalPad.StatesInstance smi = teleportTarget.GetSMI<TeleportalPad.StatesInstance>();
    smi.sm.targetTeleporter.Trigger(smi);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    TeleportalPad.StatesInstance smi = this.GetSMI<TeleportalPad.StatesInstance>();
    smi.sm.doTeleport.Trigger(smi);
  }
}
