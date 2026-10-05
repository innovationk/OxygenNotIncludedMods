// Decompiled with JetBrains decompiler
// Type: WorkerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

#nullable disable
public abstract class WorkerBase : KMonoBehaviour
{
  public abstract bool UsesMultiTool();

  public abstract bool IsFetchDrone();

  public abstract KBatchedAnimController GetAnimController();

  public abstract WorkerBase.State GetState();

  public abstract WorkerBase.StartWorkInfo GetStartWorkInfo();

  public abstract Workable GetWorkable();

  public abstract Attributes GetAttributes();

  public abstract AttributeConverterInstance GetAttributeConverter(string id);

  public abstract Guid OfferStatusItem(StatusItem item, object data = null);

  public abstract void RevokeStatusItem(Guid id);

  public abstract void StartWork(WorkerBase.StartWorkInfo start_work_info);

  public abstract void StopWork();

  public abstract bool InstantlyFinish();

  public abstract WorkerBase.WorkResult Work(float dt);

  public abstract void SetWorkCompleteData(object data);

  public class StartWorkInfo
  {
    public Workable workable { get; set; }

    public StartWorkInfo(Workable workable) => this.workable = workable;
  }

  public enum State
  {
    Idle,
    Working,
    PendingCompletion,
    Completing,
  }

  public enum WorkResult
  {
    Success,
    InProgress,
    Failed,
  }
}
