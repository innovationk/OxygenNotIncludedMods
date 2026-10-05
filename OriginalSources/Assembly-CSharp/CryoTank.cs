// Decompiled with JetBrains decompiler
// Type: CryoTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class CryoTank : StateMachineComponent<CryoTank.StatesInstance>, ISidescreenButtonControl
{
  public string[][] possible_contents_ids;
  public string machineSound;
  public string overrideAnim;
  public CellOffset dropOffset = CellOffset.none;
  private GameObject opener;
  private Chore chore;

  public string SidescreenButtonText => (string) BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTON;

  public string SidescreenButtonTooltip => (string) BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTONTOOLTIP;

  public bool SidescreenEnabled() => true;

  public void OnSidescreenButtonPressed() => this.OnClickOpen();

  public bool SidescreenButtonInteractable() => this.HasDefrostedFriend();

  public int ButtonSideScreenSortOrder() => 20;

  public void SetButtonTextOverride(ButtonMenuTextOverride text)
  {
    throw new NotImplementedException();
  }

  public int HorizontalGroupID() => -1;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    Demolishable component = this.GetComponent<Demolishable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.allowDemolition = !this.HasDefrostedFriend();
  }

  public bool HasDefrostedFriend()
  {
    return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.closed) && this.chore == null;
  }

  public void DropContents()
  {
    MinionStartingStats minionStartingStats = new MinionStartingStats(GameTags.Minions.Models.Standard, false, guaranteedTraitID: "AncientKnowledge");
    GameObject prefab = Assets.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
    GameObject gameObject = Util.KInstantiate(prefab);
    gameObject.name = prefab.name;
    Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
    Vector3 posCbc = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this.transform.position), this.dropOffset), Grid.SceneLayer.Move);
    gameObject.transform.SetLocalPosition(posCbc);
    gameObject.SetActive(true);
    minionStartingStats.Apply(gameObject);
    gameObject.GetComponent<MinionIdentity>().arrivalTime = (float) UnityEngine.Random.Range(-2000, -1000);
    MinionResume component1 = gameObject.GetComponent<MinionResume>();
    int num = 3;
    for (int index = 0; index < num; ++index)
      component1.ForceAddSkillPoint();
    this.smi.sm.defrostedDuplicant.Set(gameObject, this.smi, false);
    gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
    ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      this.smi.defrostAnimChore = (Chore) new EmoteChore((IStateMachineTarget) component2, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_cryo_chamber_kanim", new HashedString[2]
      {
        (HashedString) "defrost",
        (HashedString) "defrost_exit"
      }, KAnim.PlayMode.Once);
      Vector3 position = gameObject.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Gas)
      };
      gameObject.transform.SetPosition(position);
      gameObject.GetMyWorld().SetDupeVisited();
    }
    SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant = true;
  }

  public void ShowEventPopup()
  {
    GameObject go = this.smi.sm.defrostedDuplicant.Get(this.smi);
    if (!((UnityEngine.Object) this.opener != (UnityEngine.Object) null) || !((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    SimpleEvent.StatesInstance smi = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.CryoFriend).smi as SimpleEvent.StatesInstance;
    smi.minions = new GameObject[2]{ go, this.opener };
    smi.SetTextParameter("dupe", this.opener.GetProperName());
    smi.SetTextParameter("friend", go.GetProperName());
    smi.ShowEventPopup();
  }

  public void Cheer()
  {
    GameObject gameObject = this.smi.sm.defrostedDuplicant.Get(this.smi);
    if (!((UnityEngine.Object) this.opener != (UnityEngine.Object) null) || !((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    Db db = Db.Get();
    this.opener.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
    EmoteChore emoteChore1 = new EmoteChore((IStateMachineTarget) this.opener.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer);
    gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
    EmoteChore emoteChore2 = new EmoteChore((IStateMachineTarget) gameObject.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer);
  }

  private void OnClickOpen() => this.ActivateChore();

  private void OnClickCancel() => this.CancelActivateChore();

  public void ActivateChore(object param = null)
  {
    if (this.chore != null)
      return;
    this.GetComponent<Workable>().SetWorkTime(1.5f);
    this.chore = (Chore) new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, on_complete: (Action<Chore>) (o => this.CompleteActivateChore()), override_anims: Assets.GetAnim((HashedString) this.overrideAnim), priority_class: PriorityScreen.PriorityClass.high);
  }

  public void CancelActivateChore(object param = null)
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
  }

  private void CompleteActivateChore()
  {
    this.opener = this.chore.driver.gameObject;
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.open);
    this.chore = (Chore) null;
    Demolishable component = this.smi.GetComponent<Demolishable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.allowDemolition = true;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  public class StatesInstance(CryoTank master) : 
    GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.GameInstance(master)
  {
    public Chore defrostAnimChore;
  }

  public class States : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank>
  {
    public StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.TargetParameter defrostedDuplicant;
    public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State closed;
    public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State open;
    public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrost;
    public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrostExit;
    public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State off;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.closed;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.closed.PlayAnim("on").Enter((StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State.Callback) (smi =>
      {
        if (smi.master.machineSound == null)
          return;
        LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.StartSound(GlobalAssets.GetSound(smi.master.machineSound));
      }));
      this.open.GoTo(this.defrost).Exit((StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State.Callback) (smi => smi.master.DropContents()));
      this.defrost.PlayAnim("defrost").OnAnimQueueComplete(this.defrostExit).Update((Action<CryoTank.StatesInstance, float>) ((smi, dt) => smi.sm.defrostedDuplicant.Get(smi).GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse))).Exit((StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State.Callback) (smi => smi.master.ShowEventPopup()));
      this.defrostExit.PlayAnim("defrost_exit").Update((Action<CryoTank.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.defrostAnimChore != null && !smi.defrostAnimChore.isComplete)
          return;
        smi.GoTo((StateMachine.BaseState) this.off);
      })).Exit((StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State.Callback) (smi =>
      {
        GameObject gameObject = smi.sm.defrostedDuplicant.Get(smi);
        if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
          return;
        gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Move);
        smi.master.Cheer();
      }));
      this.off.PlayAnim("off").Enter((StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State.Callback) (smi =>
      {
        if (smi.master.machineSound == null)
          return;
        LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.StopSound(GlobalAssets.GetSound(smi.master.machineSound));
      }));
    }
  }
}
