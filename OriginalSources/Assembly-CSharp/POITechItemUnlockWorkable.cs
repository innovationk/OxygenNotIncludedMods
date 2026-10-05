// Decompiled with JetBrains decompiler
// Type: POITechItemUnlockWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class POITechItemUnlockWorkable : Workable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.ResearchingFromPOI;
    this.alwaysShowProgressBar = true;
    this.resetProgressOnStop = false;
    this.synchronizeAnims = true;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    POITechItemUnlocks.Instance smi = this.GetSMI<POITechItemUnlocks.Instance>();
    smi.UnlockTechItems();
    smi.sm.pendingChore.Set(false, smi);
    this.gameObject.Trigger(1980521255);
    Prioritizable.RemoveRef(this.gameObject);
  }
}
