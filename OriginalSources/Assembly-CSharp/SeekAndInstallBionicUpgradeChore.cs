// Decompiled with JetBrains decompiler
// Type: SeekAndInstallBionicUpgradeChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class SeekAndInstallBionicUpgradeChore : Chore<SeekAndInstallBionicUpgradeChore.Instance>
{
  private Chore.Precondition CanPickupAnyAssignedUpgrade = new Chore.Precondition()
  {
    id = nameof (CanPickupAnyAssignedUpgrade),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CANPICKUPANYASSIGNEDUPGRADE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((BionicUpgradesMonitor.Instance) data).GetAnyReachableAssignedSlot() != null),
    canExecuteOnAnyThread = false
  };

  public SeekAndInstallBionicUpgradeChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.SeekAndInstallUpgrade, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new SeekAndInstallBionicUpgradeChore.Instance(this, target.gameObject);
    BionicUpgradesMonitor.Instance smi = target.gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(this.CanPickupAnyAssignedUpgrade, (object) smi);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "SeekAndInstallBionicUpgradeChore null context.consumer");
    }
    else
    {
      BionicUpgradesMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicUpgradesMonitor.Instance>();
      if (smi == null)
      {
        Debug.LogError((object) "SeekAndInstallBionicUpgradeChore null BionicUpgradesMonitor.Instance");
      }
      else
      {
        BionicUpgradesMonitor.UpgradeComponentSlot reachableAssignedSlot = smi.GetAnyReachableAssignedSlot();
        BionicUpgradeComponent upgradeComponent = reachableAssignedSlot == null ? (BionicUpgradeComponent) null : reachableAssignedSlot.assignedUpgradeComponent;
        if ((UnityEngine.Object) upgradeComponent == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "SeekAndInstallBionicUpgradeChore null upgradeComponent.gameObject");
        }
        else
        {
          this.smi.sm.initialUpgradeComponent.Set(upgradeComponent.gameObject, this.smi, false);
          this.smi.sm.dupe.Set((KMonoBehaviour) context.consumerState.consumer, this.smi);
          base.Begin(context);
        }
      }
    }
  }

  public static void SetOverrideAnimSymbol(
    SeekAndInstallBionicUpgradeChore.Instance smi,
    bool overriding)
  {
    string str = "booster";
    KBatchedAnimController component1 = smi.GetComponent<KBatchedAnimController>();
    SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
    GameObject go = smi.sm.pickedUpgrade.Get(smi);
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
    {
      go.GetComponent<KBatchedAnimTracker>().enabled = !overriding;
      Storage.MakeItemInvisible(go, overriding, false);
    }
    if (!overriding)
    {
      component2.RemoveSymbolOverride((HashedString) str);
      component1.SetSymbolVisiblity((KAnimHashedString) str, false);
    }
    else
    {
      string animStateName = BionicUpgradeComponentConfig.UpgradesData[go.PrefabID()].animStateName;
      KAnim.Build.Symbol symbol = go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) animStateName);
      component2.AddSymbolOverride((HashedString) str, symbol);
      component1.SetSymbolVisiblity((KAnimHashedString) str, true);
    }
  }

  public static bool IsBionicUpgradeAssignedTo(
    GameObject bionicUpgradeGameObject,
    GameObject ownerInQuestion)
  {
    return !((UnityEngine.Object) bionicUpgradeGameObject == (UnityEngine.Object) null) && bionicUpgradeGameObject.GetComponent<BionicUpgradeComponent>().IsAssignedTo(ownerInQuestion.GetComponent<IAssignableIdentity>());
  }

  public static void InstallUpgrade(SeekAndInstallBionicUpgradeChore.Instance smi)
  {
    Storage first1 = smi.gameObject.GetComponents<Storage>().FindFirst<Storage>((Func<Storage, bool>) (s => s.storageID == GameTags.StoragesIds.DefaultStorage));
    GameObject first2 = first1.FindFirst(GameTags.BionicUpgrade);
    if (!((UnityEngine.Object) first2 != (UnityEngine.Object) null))
      return;
    BionicUpgradeComponent component = first2.GetComponent<BionicUpgradeComponent>();
    first1.Remove(component.gameObject);
    smi.upgradeMonitor.InstallUpgrade(component);
    if (!((UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null))
      return;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, component.GetProperName(), smi.gameObject.transform, Vector3.up, track_target: true);
  }

  public class States : 
    GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore>
  {
    public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FetchSubState fetch;
    public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State install;
    public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State complete;
    public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter dupe;
    public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter initialUpgradeComponent;
    public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter pickedUpgrade;
    public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter actualunits;
    public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter amountRequested = new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter(1f);

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.fetch;
      this.Target(this.dupe);
      this.fetch.InitializeStates(this.dupe, this.initialUpgradeComponent, this.pickedUpgrade, this.amountRequested, this.actualunits, this.install).Target(this.initialUpgradeComponent).EventHandlerTransition(GameHashes.AssigneeChanged, (GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State) null, (Func<SeekAndInstallBionicUpgradeChore.Instance, object, bool>) ((smi, obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.initialUpgradeComponent.Get(smi), smi.gameObject)));
      this.install.Target(this.dupe).ToggleAnims("anim_bionic_booster_installation_kanim").PlayAnim("installation", KAnim.PlayMode.Once).Enter((StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State.Callback) (smi => SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, true))).Exit((StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State.Callback) (smi => SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, false))).OnAnimQueueComplete(this.complete).ScheduleGoTo(10f, (StateMachine.BaseState) this.complete).Target(this.pickedUpgrade).EventHandlerTransition(GameHashes.AssigneeChanged, (GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State) null, (Func<SeekAndInstallBionicUpgradeChore.Instance, object, bool>) ((smi, obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.pickedUpgrade.Get(smi), smi.gameObject)));
      this.complete.Target(this.dupe).Enter(new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State.Callback(SeekAndInstallBionicUpgradeChore.InstallUpgrade)).ReturnSuccess();
    }
  }

  public class Instance(SeekAndInstallBionicUpgradeChore master, GameObject duplicant) : 
    GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.GameInstance(master)
  {
    public BionicUpgradesMonitor.Instance upgradeMonitor
    {
      get => this.sm.dupe.Get(this).GetSMI<BionicUpgradesMonitor.Instance>();
    }
  }
}
