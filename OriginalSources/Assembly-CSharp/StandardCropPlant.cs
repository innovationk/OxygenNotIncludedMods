// Decompiled with JetBrains decompiler
// Type: StandardCropPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class StandardCropPlant : StateMachineComponent<StandardCropPlant.StatesInstance>
{
  private const int WILT_LEVELS = 3;
  [MyCmpReq]
  private Crop crop;
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private ReceptacleMonitor rm;
  [MyCmpReq]
  private Growing growing;
  [MyCmpReq]
  private KAnimControllerBase animController;
  [MyCmpGet]
  private Harvestable harvestable;
  public bool wiltsOnReadyToHarvest;
  public bool preventGrowPositionUpdate;
  public static StandardCropPlant.AnimSet defaultAnimSet = new StandardCropPlant.AnimSet()
  {
    pre_grow = (string) null,
    grow = "grow",
    grow_pst = "grow_pst",
    idle_full = "idle_full",
    wilt_base = "wilt",
    wilt_recover_base = (string) null,
    harvest = "harvest",
    waning = "waning"
  };
  public StandardCropPlant.AnimSet anims = StandardCropPlant.defaultAnimSet;

  public static string GetWiltAnimFromAnimSet(
    StandardCropPlant.AnimSet set,
    float growingPercentage)
  {
    return StandardCropPlant.GetGenericWiltAnimFromAnimSet(set, (Func<int, string>) (stg => set.GetWiltLevel(stg)), growingPercentage);
  }

  public static string GetWiltRecoverAnimFromAnimSet(
    StandardCropPlant.AnimSet set,
    float growingPercentage)
  {
    return StandardCropPlant.GetGenericWiltAnimFromAnimSet(set, (Func<int, string>) (stg => set.GetWiltRecoverLevel(stg)), growingPercentage);
  }

  private static string GetGenericWiltAnimFromAnimSet(
    StandardCropPlant.AnimSet set,
    Func<int, string> wiltLevelFn,
    float growingPercentage)
  {
    int num = (double) growingPercentage >= 0.75 ? ((double) growingPercentage >= 1.0 ? 3 : 2) : 1;
    return wiltLevelFn(num);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  public Notification CreateDeathNotification()
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + this.gameObject.GetProperName()));
  }

  public void RefreshPositionPercent()
  {
    this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
  }

  private static string ToolTipResolver(List<Notification> notificationList, object data)
  {
    string str = "";
    for (int index = 0; index < notificationList.Count; ++index)
    {
      Notification notification = notificationList[index];
      str += (string) notification.tooltipData;
      if (index < notificationList.Count - 1)
        str += "\n";
    }
    return string.Format((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, (object) str);
  }

  public class AnimSet
  {
    public string pre_grow;
    public string grow;
    public string grow_pst;
    public string idle_full;
    public string wilt_base;
    public string wilt_recover_base;
    public string harvest;
    public string waning;
    public KAnim.PlayMode grow_playmode = KAnim.PlayMode.Paused;
    private Dictionary<string, string[]> m_wilt = new Dictionary<string, string[]>();

    public void ClearWiltLevelCache() => this.m_wilt.Clear();

    public string GetWiltLevel(int level) => this.GetWiltAnimLevel(this.wilt_base, level);

    public string GetWiltRecoverLevel(int level)
    {
      return this.GetWiltAnimLevel(this.wilt_recover_base, level);
    }

    private string GetWiltAnimLevel(string baseSTR, int level)
    {
      if (baseSTR == null)
        return (string) null;
      if (!this.m_wilt.ContainsKey(baseSTR))
      {
        this.m_wilt[baseSTR] = new string[3];
        for (int index = 0; index < 3; ++index)
          this.m_wilt[baseSTR][index] = baseSTR + (index + 1).ToString();
      }
      return this.m_wilt[baseSTR][level - 1];
    }

    public AnimSet()
    {
    }

    public AnimSet(StandardCropPlant.AnimSet template)
    {
      this.pre_grow = template.pre_grow;
      this.grow = template.grow;
      this.grow_pst = template.grow_pst;
      this.idle_full = template.idle_full;
      this.wilt_base = template.wilt_base;
      this.wilt_recover_base = template.wilt_recover_base;
      this.harvest = template.harvest;
      this.waning = template.waning;
      this.grow_playmode = template.grow_playmode;
    }
  }

  public class States : 
    GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant>
  {
    public StandardCropPlant.States.AliveStates alive;
    public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead;
    public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState blighted;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      default_state = (StateMachine.BaseState) this.alive;
      this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
          smi.master.gameObject.AddOrGet<Notifier>().Add(smi.master.CreateDeathNotification());
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        Harvestable component = smi.master.GetComponent<Harvestable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.CanBeHarvested && (UnityEngine.Object) GameScheduler.Instance != (UnityEngine.Object) null)
          GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), (object) null, (SchedulerGroup) null);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blighted.InitializeStates(this.masterTarget, this.dead).PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.waning)).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted).TagTransition(GameTags.Blighted, (GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State) this.alive, true);
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.pre_idle).ToggleComponent<Growing>().TagTransition(GameTags.Blighted, (GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State) this.blighted);
      this.alive.pre_idle.EnterTransition(this.alive.idle, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => smi.master.anims.pre_grow == null)).PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.pre_grow)).OnAnimQueueComplete(this.alive.idle).ScheduleGoTo(8f, (StateMachine.BaseState) this.alive.idle);
      this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => smi.master.growing.ReachedNextHarvest())).PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.grow), (Func<StandardCropPlant.StatesInstance, KAnim.PlayMode>) (smi => smi.master.anims.grow_playmode)).Enter(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent)).Update(new Action<StandardCropPlant.StatesInstance, float>(StandardCropPlant.States.RefreshPositionPercent), UpdateRate.SIM_4000ms).EventHandler(GameHashes.ConsumePlant, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent));
      this.alive.pre_fruiting.PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.grow_pst)).TriggerOnEnter(GameHashes.BurstEmitDisease).EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting).EventTransition(GameHashes.Wilt, this.alive.wilting).ScheduleGoTo(8f, (StateMachine.BaseState) this.alive.fruiting);
      this.alive.fruiting_lost.Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(false);
      })).GoTo(this.alive.idle);
      this.alive.wilting.PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim)).EventTransition(GameHashes.WiltRecover, this.alive.wiltRecover, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting())).EventTransition(GameHashes.Harvest, this.alive.harvest);
      this.alive.wiltRecover.EnterTransition(this.alive.idle, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(StandardCropPlant.States.DoesNotHaveWiltRecoverAnim)).PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltRecoverAnim)).OnAnimQueueComplete(this.alive.idle);
      this.alive.fruiting.PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.idle_full), KAnim.PlayMode.Loop).ToggleTag(GameTags.FullyGrown).Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(true);
      })).EventHandlerTransition(GameHashes.Wilt, this.alive.wilting, (Func<StandardCropPlant.StatesInstance, object, bool>) ((smi, obj) => smi.master.wiltsOnReadyToHarvest)).EventTransition(GameHashes.Harvest, this.alive.harvest).EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => !smi.master.growing.ReachedNextHarvest()));
      this.alive.harvest.PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.harvest)).Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if ((UnityEngine.Object) smi.master != (UnityEngine.Object) null)
          smi.master.crop.SpawnConfiguredFruit((object) null);
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(false);
      })).Exit((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi => smi.Trigger(113170146))).OnAnimQueueComplete(this.alive.idle);
    }

    private static string GetWiltAnim(StandardCropPlant.StatesInstance smi)
    {
      float growingPercentage = smi.master.growing.PercentOfCurrentHarvest();
      return StandardCropPlant.GetWiltAnimFromAnimSet(smi.master.anims, growingPercentage);
    }

    private static bool DoesNotHaveWiltRecoverAnim(StandardCropPlant.StatesInstance smi)
    {
      return StandardCropPlant.States.GetWiltRecoverAnim(smi) == null;
    }

    private static string GetWiltRecoverAnim(StandardCropPlant.StatesInstance smi)
    {
      float growingPercentage = smi.master.growing.PercentOfCurrentHarvest();
      return StandardCropPlant.GetWiltRecoverAnimFromAnimSet(smi.master.anims, growingPercentage);
    }

    private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi, float dt)
    {
      StandardCropPlant.States.RefreshPositionPercent(smi);
    }

    private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi)
    {
      if (smi.master.preventGrowPositionUpdate)
        return;
      smi.master.RefreshPositionPercent();
    }

    public class AliveStates : 
      GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState
    {
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_idle;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State idle;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_fruiting;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_lost;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State barren;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wiltRecover;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State destroy;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State harvest;
    }
  }

  public class StatesInstance(StandardCropPlant master) : 
    GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.GameInstance(master)
  {
  }
}
