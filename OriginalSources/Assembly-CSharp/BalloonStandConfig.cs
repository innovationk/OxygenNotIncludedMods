// Decompiled with JetBrains decompiler
// Type: BalloonStandConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class BalloonStandConfig : IEntityConfig
{
  public static readonly string ID = "BalloonStand";
  private static Chore.Precondition HasNoBalloon = new Chore.Precondition()
  {
    id = nameof (HasNoBalloon),
    description = "__ Duplicant doesn't have a balloon already",
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect("HasBalloon"))
  };

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(BalloonStandConfig.ID, BalloonStandConfig.ID, false);
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_balloon_receiver_kanim")
    };
    GetBalloonWorkable getBalloonWorkable = entity.AddOrGet<GetBalloonWorkable>();
    getBalloonWorkable.workTime = 2f;
    getBalloonWorkable.workLayer = Grid.SceneLayer.BuildingFront;
    getBalloonWorkable.overrideAnims = kanimFileArray;
    getBalloonWorkable.synchronizeAnims = false;
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    GetBalloonWorkable component = inst.GetComponent<GetBalloonWorkable>();
    WorkChore<GetBalloonWorkable> data = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, (IStateMachineTarget) component, on_complete: new Action<Chore>(this.MakeNewBalloonChore), schedule_block: Db.Get().ScheduleBlockTypes.Recreation, priority_class: PriorityScreen.PriorityClass.high, ignore_building_assignment: true);
    data.AddPrecondition(BalloonStandConfig.HasNoBalloon, (object) data);
    data.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
    component.GetBalloonArtist().NextBalloonOverride();
  }

  private void MakeNewBalloonChore(Chore chore)
  {
    GetBalloonWorkable component = chore.target.GetComponent<GetBalloonWorkable>();
    WorkChore<GetBalloonWorkable> data = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, (IStateMachineTarget) component, on_complete: new Action<Chore>(this.MakeNewBalloonChore), schedule_block: Db.Get().ScheduleBlockTypes.Recreation, priority_class: PriorityScreen.PriorityClass.high, ignore_building_assignment: true);
    data.AddPrecondition(BalloonStandConfig.HasNoBalloon, (object) data);
    data.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
    component.GetBalloonArtist().NextBalloonOverride();
  }
}
