// Decompiled with JetBrains decompiler
// Type: PartyPointWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class PartyPointWorkable : Workable, IWorkerPrioritizable
{
  private GameObject lastTalker;
  public int basePriority;
  public string specificEffect;
  public KAnimFile[][] workerOverrideAnims;
  private PartyPointWorkable.ActivityType activity;

  private PartyPointWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_generic_convo_kanim")
    };
    this.workAnimPlayMode = KAnim.PlayMode.Loop;
    this.faceTargetWhenWorking = true;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
    this.synchronizeAnims = false;
    this.showProgressBar = false;
    this.resetProgressOnStop = true;
    this.lightEfficiencyBonus = false;
    this.activity = (double) UnityEngine.Random.Range(0.0f, 100f) <= 80.0 ? PartyPointWorkable.ActivityType.Talk : PartyPointWorkable.ActivityType.Dance;
    switch (this.activity)
    {
      case PartyPointWorkable.ActivityType.Talk:
        this.workAnims = new HashedString[1]
        {
          (HashedString) "idle"
        };
        this.workerOverrideAnims = new KAnimFile[1][]
        {
          new KAnimFile[1]
          {
            Assets.GetAnim((HashedString) "anim_generic_convo_kanim")
          }
        };
        break;
      case PartyPointWorkable.ActivityType.Dance:
        this.workAnims = new HashedString[1]
        {
          (HashedString) "working_loop"
        };
        this.workerOverrideAnims = new KAnimFile[3][]
        {
          new KAnimFile[1]
          {
            Assets.GetAnim((HashedString) "anim_interacts_phonobox_danceone_kanim")
          },
          new KAnimFile[1]
          {
            Assets.GetAnim((HashedString) "anim_interacts_phonobox_dancetwo_kanim")
          },
          new KAnimFile[1]
          {
            Assets.GetAnim((HashedString) "anim_interacts_phonobox_dancethree_kanim")
          }
        };
        break;
    }
  }

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    this.overrideAnims = this.workerOverrideAnims[UnityEngine.Random.Range(0, this.workerOverrideAnims.Length)];
    return base.GetAnim(worker);
  }

  public override Vector3 GetFacingTarget()
  {
    return (UnityEngine.Object) this.lastTalker != (UnityEngine.Object) null ? this.lastTalker.transform.GetPosition() : base.GetFacingTarget();
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt) => false;

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse);
    worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
    worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
    worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
    worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (string.IsNullOrEmpty(this.specificEffect))
      return;
    component.Add(this.specificEffect, true);
  }

  private void OnStartedTalking(object data)
  {
    if (!(data is ConversationManager.StartedTalkingEvent startedTalkingEvent))
      return;
    GameObject talker = startedTalkingEvent.talker;
    if ((UnityEngine.Object) talker == (UnityEngine.Object) this.worker.gameObject)
    {
      if (this.activity != PartyPointWorkable.ActivityType.Talk)
        return;
      KBatchedAnimController component = this.worker.GetComponent<KBatchedAnimController>();
      component.Play((HashedString) (startedTalkingEvent.anim + UnityEngine.Random.Range(1, 9).ToString()));
      component.Queue((HashedString) "idle", KAnim.PlayMode.Loop);
    }
    else
    {
      if (this.activity == PartyPointWorkable.ActivityType.Talk)
        this.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
      this.lastTalker = talker;
    }
  }

  private void OnStoppedTalking(object data)
  {
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }

  private enum ActivityType
  {
    Talk,
    Dance,
    LENGTH,
  }
}
