// Decompiled with JetBrains decompiler
// Type: GravitasBathroomStall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GravitasBathroomStall : 
  GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>
{
  public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State start;
  public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State branch;
  public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State blinking;
  public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State activated;
  public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State complete;
  public StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.BoolParameter hasBeenActivated;
  public StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.BoolParameter hasShownPopup;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.start;
    this.root.DefaultState(this.start);
    this.start.PlayAnim("idle").Update((System.Action<GravitasBathroomStall.Instance, float>) ((smi, dt) =>
    {
      if (!((UnityEngine.Object) HijackedHeadquarters.Instance.PrinterceptorInstance != (UnityEngine.Object) null) || !HijackedHeadquarters.IsOperational(HijackedHeadquarters.Instance.PrinterceptorInstance.GetSMI<HijackedHeadquarters.Instance>()))
        return;
      smi.sm.hasBeenActivated.Set(smi.master.GetComponent<Activatable>().IsActivated, smi);
      smi.GoTo((StateMachine.BaseState) this.branch);
    }));
    this.branch.ParamTransition<bool>((StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.Parameter<bool>) this.hasBeenActivated, this.blinking, GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.IsFalse).ParamTransition<bool>((StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.Parameter<bool>) this.hasBeenActivated, this.activated, GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.IsTrue);
    this.blinking.PlayAnim("code_ready", KAnim.PlayMode.Loop).EventHandlerTransition(GameHashes.BuildingActivated, this.activated, (Func<GravitasBathroomStall.Instance, object, bool>) ((smi, data) => ((Boxed<bool>) data).value)).Enter((StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State.Callback) (smi => smi.SubscribeToPrinterceptorOperational())).Exit((StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State.Callback) (smi => smi.UnsubscribeFromPrinterceptorOperational()));
    this.activated.Enter((StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State.Callback) (smi =>
    {
      if (!smi.sm.hasShownPopup.Get(smi))
        smi.ShowLoreUnlockedPopup();
      else
        smi.GoTo((StateMachine.BaseState) this.complete);
      smi.sm.hasBeenActivated.Set(true, smi);
    })).PlayAnim("activated");
    this.complete.PlayAnim("idle");
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, GravitasBathroomStall.Def def) : 
    GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.GameInstance(master, def)
  {
    private StoryInstance storyInstance;
    private Notification completeNotification;
    private int onBuildingSelectHandle = -1;
    private int printerceptorOperationalEventHandle = -1;

    public override void StartSM()
    {
      base.StartSM();
      this.GetComponent<Activatable>().activationCondition = (Func<bool>) (() => (UnityEngine.Object) HijackedHeadquarters.Instance.PrinterceptorInstance != (UnityEngine.Object) null && HijackedHeadquarters.IsOperational(HijackedHeadquarters.Instance.PrinterceptorInstance.GetSMI<HijackedHeadquarters.Instance>()));
      this.storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.HijackedHeadquarters.HashId);
      this.onBuildingSelectHandle = this.Subscribe(-1503271301, new System.Action<object>(this.OnBuildingSelect));
    }

    public override void StopSM(string reason)
    {
      if (this.onBuildingSelectHandle != -1)
        this.Unsubscribe(ref this.onBuildingSelectHandle);
      base.StopSM(reason);
    }

    private void OnBuildingSelect(object obj)
    {
      if (!((Boxed<bool>) obj).value || this.completeNotification == null)
        return;
      this.completeNotification.customClickCallback(this.completeNotification.customClickData);
    }

    public void ShowLoreUnlockedPopup()
    {
      this.completeNotification = EventInfoScreen.CreateNotification(EventInfoDataHelper.GenerateStoryTraitData((string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.UNLOCK_POPUP.NAME, (string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.UNLOCK_POPUP.DESCRIPTION, (string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.UNLOCK_POPUP.BUTTON, "printerceptorcoderevealed_kanim", EventInfoDataHelper.PopupType.NORMAL, callback: (System.Action) (() =>
      {
        this.smi.sm.hasShownPopup.Set(true, this.smi);
        this.smi.master.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(GravitasBathroomStall.Instance.Sequence(this.smi));
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.complete);
      })));
      this.gameObject.AddOrGet<Notifier>().Add(this.completeNotification);
      this.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, (object) this.smi);
    }

    private static IEnumerator Sequence(GravitasBathroomStall.Instance smi)
    {
      StoryManager.Instance.GetStoryInstance(Db.Get().Stories.HijackedHeadquarters.HashId);
      smi.ClearEndNotification();
      if (!HijackedHeadquarters.Instance.PrinterceptorInstance.IsNullOrDestroyed())
      {
        smi.RevealPrinterceptor();
        CameraController.Instance.FadeOut();
        yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
        Vector3 vector3 = new Vector3(2f, 3f, 0.0f);
        GameUtil.FocusCamera(HijackedHeadquarters.Instance.PrinterceptorInstance.transform.position + vector3, 10f, false);
        yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
        if (SpeedControlScreen.Instance.IsPaused)
          SpeedControlScreen.Instance.Unpause(false);
        CameraController.Instance.FadeIn();
        yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
        HijackedHeadquarters.Instance.PrinterceptorInstance.GetSMI<HijackedHeadquarters.Instance>().UnlockPrinterceptor();
      }
    }

    private void RevealPrinterceptor()
    {
      List<WorldGenSpawner.Spawnable> spawnableList = new List<WorldGenSpawner.Spawnable>();
      foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
        spawnableList.AddRange((IEnumerable<WorldGenSpawner.Spawnable>) SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag((Tag) "HijackedHeadquarters", worldContainer.id));
      foreach (WorldGenSpawner.Spawnable spawnable in spawnableList)
      {
        int x;
        int y;
        Grid.CellToXY(spawnable.cell, out x, out y);
        GridVisibility.Reveal(x, y, 10, 10f);
      }
    }

    public void ClearEndNotification()
    {
      this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
      if (this.completeNotification != null)
        this.gameObject.AddOrGet<Notifier>().Remove(this.completeNotification);
      this.completeNotification = (Notification) null;
    }

    public void SubscribeToPrinterceptorOperational()
    {
      this.UnsubscribeFromPrinterceptorOperational();
      if (!((UnityEngine.Object) HijackedHeadquarters.Instance.PrinterceptorInstance != (UnityEngine.Object) null))
        return;
      this.printerceptorOperationalEventHandle = HijackedHeadquarters.Instance.PrinterceptorInstance.Subscribe(-592767678, (System.Action<object>) (data =>
      {
        this.smi.master.GetComponent<Activatable>().CancelChore();
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.start);
      }));
    }

    public void UnsubscribeFromPrinterceptorOperational()
    {
      if (this.printerceptorOperationalEventHandle != -1 && (UnityEngine.Object) HijackedHeadquarters.Instance.PrinterceptorInstance != (UnityEngine.Object) null)
        HijackedHeadquarters.Instance.PrinterceptorInstance.Unsubscribe(this.printerceptorOperationalEventHandle);
      this.printerceptorOperationalEventHandle = -1;
    }
  }
}
