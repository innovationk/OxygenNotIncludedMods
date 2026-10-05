// Decompiled with JetBrains decompiler
// Type: UseSolidLubricantChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
public class UseSolidLubricantChore : Chore<UseSolidLubricantChore.Instance>
{
  public const float LOOP_LENGTH = 6.666f;
  public static readonly Chore.Precondition SolidLubricantIsNotNull = new Chore.Precondition()
  {
    id = "SolidLubricantIsNotNull ",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (Object) null != (Object) context.consumerState.consumer.GetSMI<BionicOilMonitor.Instance>().GetClosestSolidLubricant())
  };

  public UseSolidLubricantChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.SolidOilChange, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new UseSolidLubricantChore.Instance(this, target.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(UseSolidLubricantChore.SolidLubricantIsNotNull, (object) null);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((Object) context.consumerState.consumer == (Object) null)
    {
      Debug.LogError((object) "ReloadElectrobankChore null context.consumer");
    }
    else
    {
      BionicOilMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicOilMonitor.Instance>();
      if (smi == null)
      {
        Debug.LogError((object) "ReloadElectrobankChore null RationMonitor.Instance");
      }
      else
      {
        Pickupable closestSolidLubricant = smi.GetClosestSolidLubricant();
        if ((Object) closestSolidLubricant == (Object) null)
        {
          Debug.LogError((object) "ReloadElectrobankChore null electrobank.gameObject");
        }
        else
        {
          this.smi.sm.solidLubricantSource.Set(closestSolidLubricant.gameObject, this.smi, false);
          this.smi.sm.dupe.Set((KMonoBehaviour) context.consumerState.consumer, this.smi);
          base.Begin(context);
        }
      }
    }
  }

  public static void ConsumeLubricant(UseSolidLubricantChore.Instance smi)
  {
    PrimaryElement component = smi.sm.pickedUpSolidLubricant.Get(smi).GetComponent<PrimaryElement>();
    float amount = Mathf.Min(component.Mass, 200f - smi.oilMonitor.oilAmount.value);
    smi.oilMonitor.RefillOil(amount);
    if ((double) amount >= (double) component.Mass)
    {
      Util.KDestroyGameObject(component.gameObject);
      smi.sm.pickedUpSolidLubricant.Set((KMonoBehaviour) null, smi);
    }
    else
      component.Mass -= amount;
    BionicOilMonitor.ApplyLubricationEffects(smi.master.GetComponent<Effects>(), component.GetComponent<PrimaryElement>().ElementID);
  }

  public static void SetOverrideAnimSymbol(UseSolidLubricantChore.Instance smi, bool overriding)
  {
    string str = "lubricant";
    KBatchedAnimController component1 = smi.GetComponent<KBatchedAnimController>();
    SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
    GameObject go = smi.sm.pickedUpSolidLubricant.Get(smi);
    if ((Object) go != (Object) null)
    {
      KBatchedAnimTracker component3 = go.GetComponent<KBatchedAnimTracker>();
      if ((Object) component3 != (Object) null)
        component3.enabled = !overriding;
      Storage.MakeItemInvisible(go, overriding, false);
    }
    if (!overriding)
    {
      component2.RemoveSymbolOverride((HashedString) str);
      component1.SetSymbolVisiblity((KAnimHashedString) str, false);
    }
    else
    {
      KAnim.Build.Symbol symbolByIndex = go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
      component2.AddSymbolOverride((HashedString) str, symbolByIndex);
      component1.SetSymbolVisiblity((KAnimHashedString) str, true);
    }
  }

  public class States : 
    GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore>
  {
    public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FetchSubState fetch;
    public UseSolidLubricantChore.States.InstallState consume;
    public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State complete;
    public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State lubricantLost;
    public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter dupe;
    public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter solidLubricantSource;
    public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter pickedUpSolidLubricant;
    public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter messstation;
    public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter actualunits;
    public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter amountRequested = new StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter(LubricationStickConfig.MASS_PER_RECIPE);

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.fetch;
      this.Target(this.dupe);
      this.fetch.InitializeStates(this.dupe, this.solidLubricantSource, this.pickedUpSolidLubricant, this.amountRequested, this.actualunits, (GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State) this.consume).OnTargetLost(this.solidLubricantSource, this.lubricantLost);
      this.consume.DefaultState(this.consume.pre).ToggleAnims("anim_bionic_kanim").Enter("Add Symbol Override", (StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State.Callback) (smi => UseSolidLubricantChore.SetOverrideAnimSymbol(smi, true))).Exit("Revert Symbol Override", (StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State.Callback) (smi => UseSolidLubricantChore.SetOverrideAnimSymbol(smi, false)));
      this.consume.pre.PlayAnim("lubricate_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.consume.loop).ScheduleGoTo(4.7f, (StateMachine.BaseState) this.consume.loop);
      this.consume.loop.PlayAnim("lubricate_loop", KAnim.PlayMode.Loop).ScheduleGoTo(6.666f, (StateMachine.BaseState) this.consume.pst);
      this.consume.pst.PlayAnim("lubricate_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3.5f, (StateMachine.BaseState) this.complete);
      this.complete.Enter(new StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State.Callback(UseSolidLubricantChore.ConsumeLubricant)).ReturnSuccess();
      this.lubricantLost.Target(this.dupe).ReturnFailure();
    }

    public class InstallState : 
      GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State
    {
      public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State pre;
      public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State loop;
      public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State pst;
    }
  }

  public class Instance(UseSolidLubricantChore master, GameObject duplicant) : 
    GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.GameInstance(master)
  {
    public BionicOilMonitor.Instance oilMonitor
    {
      get => this.sm.dupe.Get(this).GetSMI<BionicOilMonitor.Instance>();
    }
  }
}
