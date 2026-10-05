// Decompiled with JetBrains decompiler
// Type: LargeImpactorNotificationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LargeImpactorNotificationMonitor : 
  GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>
{
  public const string NOTIFICATION_PREFAB_ID = "LargeImpactNotification";
  public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State undiscovered;
  public LargeImpactorNotificationMonitor.DiscoveredStates discovered;
  public StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.BoolParameter HasBeenDiscovered;
  public StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.BoolParameter SequenceCompleted;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.undiscovered;
    this.undiscovered.ParamTransition<bool>((StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.Parameter<bool>) this.HasBeenDiscovered, (GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State) this.discovered, GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.IsTrue).EventHandler(GameHashes.DiscoveredSpace, (Func<LargeImpactorNotificationMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), new GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.GameEvent.Callback(LargeImpactorNotificationMonitor.OnDuplicantReachedSpace)).EventHandler(GameHashes.DLCPOICompleted, (Func<LargeImpactorNotificationMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), new GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.GameEvent.Callback(LargeImpactorNotificationMonitor.OnPOIActivated));
    this.discovered.DefaultState(this.discovered.sequence);
    this.discovered.sequence.ParamTransition<bool>((StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.Parameter<bool>) this.SequenceCompleted, (GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State) this.discovered.notification, GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.IsTrue).Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.RevealSurface)).Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.PlaySequence)).EventHandler(GameHashes.SequenceCompleted, new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.CompleteSequence));
    this.discovered.notification.DefaultState(this.discovered.notification.delayEntry);
    this.discovered.notification.delayEntry.ScheduleGoTo(3f, (StateMachine.BaseState) this.discovered.notification.running);
    this.discovered.notification.running.Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.PlayNotificationEnterSound)).Enter(new StateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State.Callback(LargeImpactorNotificationMonitor.SetLandingZoneVisualizationToActive)).ScheduleAction("Toggle off the visualization after a delay", 2f, new System.Action<LargeImpactorNotificationMonitor.Instance>(LargeImpactorNotificationMonitor.FoldTheVisualization)).ToggleNotification((Func<LargeImpactorNotificationMonitor.Instance, Notification>) (smi => smi.notification));
  }

  public static void CompleteSequence(LargeImpactorNotificationMonitor.Instance smi)
  {
    smi.sm.SequenceCompleted.Set(true, smi);
  }

  public static void Discover(LargeImpactorNotificationMonitor.Instance smi)
  {
    smi.sm.HasBeenDiscovered.Set(true, smi);
  }

  public static void RevealSurface(LargeImpactorNotificationMonitor.Instance smi)
  {
    smi.RevealSurface();
  }

  public static void PlayNotificationEnterSound(LargeImpactorNotificationMonitor.Instance smi)
  {
    KFMOD.PlayUISound(GlobalAssets.GetSound("Notification_Imperative"));
  }

  public static void SetLandingZoneVisualizationToActive(
    LargeImpactorNotificationMonitor.Instance smi)
  {
    smi.GetComponent<LargeImpactorVisualizer>().Active = true;
  }

  public static void FoldTheVisualization(LargeImpactorNotificationMonitor.Instance smi)
  {
    LargeImpactorVisualizer component = smi.GetComponent<LargeImpactorVisualizer>();
    if (component.Folded)
      return;
    component.SetFoldedState(true);
  }

  public static void OnPOIActivated(LargeImpactorNotificationMonitor.Instance smi, object obj)
  {
    if (!(((GameObject) obj).PrefabID() == (Tag) "POIDlc4TechUnlock"))
      return;
    LargeImpactorNotificationMonitor.Discover(smi);
  }

  public static void OnDuplicantReachedSpace(
    LargeImpactorNotificationMonitor.Instance smi,
    object obj)
  {
    if (((GameObject) obj).GetMyWorldId() != smi.gameObject.GetMyWorldId())
      return;
    LargeImpactorNotificationMonitor.Discover(smi);
  }

  public static void PlaySequence(LargeImpactorNotificationMonitor.Instance smi)
  {
    smi.PlaySequence();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class NotificationStates : 
    GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State
  {
    public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State delayEntry;
    public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State running;
  }

  public class DiscoveredStates : 
    GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State
  {
    public GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.State sequence;
    public LargeImpactorNotificationMonitor.NotificationStates notification;
  }

  public new class Instance : 
    GameStateMachine<LargeImpactorNotificationMonitor, LargeImpactorNotificationMonitor.Instance, IStateMachineTarget, LargeImpactorNotificationMonitor.Def>.GameInstance
  {
    private Notifier notifier;
    private Coroutine sequenceCoroutine;
    private LargeImpactorSequenceUIReticle sequenceReticle;

    public bool HasRevealSequencePlayed => this.sm.SequenceCompleted.Get(this);

    public Notification notification { private set; get; }

    public Instance(IStateMachineTarget master, LargeImpactorNotificationMonitor.Def def)
      : base(master, def)
    {
      this.notifier = this.gameObject.AddOrGet<Notifier>();
      LargeImpactorStatus.Instance smi = this.smi.GetSMI<LargeImpactorStatus.Instance>();
      string name = (string) MISC.NOTIFICATIONS.INCOMINGPREHISTORICASTEROIDNOTIFICATION.NAME;
      object obj = (object) smi;
      Func<List<Notification>, object, string> tooltip = new Func<List<Notification>, object, string>(this.ResolveNotificationTooltip);
      object tooltip_data = obj;
      this.notification = new Notification(name, NotificationType.Custom, tooltip, tooltip_data, false);
      this.notification.customNotificationID = "LargeImpactNotification";
    }

    private string ResolveNotificationTooltip(List<Notification> not, object data)
    {
      LargeImpactorStatus.Instance instance = (LargeImpactorStatus.Instance) data;
      return GameUtil.SafeStringFormat((string) MISC.NOTIFICATIONS.INCOMINGPREHISTORICASTEROIDNOTIFICATION.TOOLTIP, (object) GameUtil.GetFormattedInt((float) instance.Health), (object) GameUtil.GetFormattedInt((float) instance.def.MAX_HEALTH), (object) GameUtil.GetFormattedCycles(instance.TimeRemainingBeforeCollision));
    }

    public void RevealSurface()
    {
      GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id);
      if (gameplayEventInstance == null)
        return;
      WorldContainer world = ClusterManager.Instance.GetWorld(gameplayEventInstance.worldId);
      if (!((UnityEngine.Object) world != (UnityEngine.Object) null) || world.IsSurfaceRevealed)
        return;
      world.RevealSurface();
    }

    public void SetNotificationVisibility(bool visible)
    {
      if (visible)
        this.notifier.Add(this.notification);
      else
        this.notifier.Remove(this.notification);
    }

    public void PlaySequence()
    {
      this.AbortSequenceCoroutine();
      this.CreateReticleForSequence();
      GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id);
      if (gameplayEventInstance == null)
        return;
      this.sequenceCoroutine = LargeImpactorRevealSequence.Start((KMonoBehaviour) this.notifier, this.sequenceReticle, ClusterManager.Instance.GetWorld(gameplayEventInstance.worldId));
    }

    private void CreateReticleForSequence()
    {
      this.DeleteReticleObject();
      this.sequenceReticle = Util.KInstantiateUI<LargeImpactorSequenceUIReticle>(ScreenPrefabs.Instance.largeImpactorSequenceReticlePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
      this.sequenceReticle.SetTarget(this.gameObject.GetSMI<LargeImpactorStatus.Instance>());
    }

    private void DeleteReticleObject()
    {
      if (!((UnityEngine.Object) this.sequenceReticle != (UnityEngine.Object) null))
        return;
      this.sequenceReticle.gameObject.DeleteObject();
    }

    private void AbortSequenceCoroutine()
    {
      if (this.sequenceCoroutine == null)
        return;
      this.notifier.StopCoroutine(this.sequenceCoroutine);
      this.sequenceCoroutine = (Coroutine) null;
    }

    protected override void OnCleanUp()
    {
      this.AbortSequenceCoroutine();
      this.DeleteReticleObject();
      base.OnCleanUp();
    }
  }
}
