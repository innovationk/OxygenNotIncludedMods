// Decompiled with JetBrains decompiler
// Type: MessStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/MessStation")]
public class MessStation : Workable, IDiningSeat
{
  [MyCmpGet]
  private Ownable ownable;
  private MessStation.MessStationSM.Instance smi;
  public static readonly HashedString eatAnim = (HashedString) "anim_eat_table_kanim";
  public static readonly HashedString reloadElectrobankAnim = (HashedString) "anim_bionic_eat_table_kanim";

  protected override void OnPrefabInit()
  {
    this.ownable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.HasCaloriesOwnablePrecondition));
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim(MessStation.eatAnim)
    };
  }

  public static bool CanBeAssignedTo(IAssignableIdentity assignee)
  {
    MinionAssignablesProxy assignablesProxy = assignee as MinionAssignablesProxy;
    if ((UnityEngine.Object) assignablesProxy == (UnityEngine.Object) null)
      return false;
    MinionIdentity target = assignablesProxy.target as MinionIdentity;
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return false;
    if (Db.Get().Amounts.Calories.Lookup((Component) target) != null)
      return true;
    return Game.IsDlcActiveForCurrentSave("DLC3_ID") && target.model == BionicMinionConfig.MODEL;
  }

  private bool HasCaloriesOwnablePrecondition(MinionAssignablesProxy worker)
  {
    return MessStation.CanBeAssignedTo((IAssignableIdentity) worker);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    worker.GetWorkable().GetComponent<Edible>().CompleteWork(worker);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new MessStation.MessStationSM.Instance(this);
    this.smi.StartSM();
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    Storage component = go.GetComponent<Storage>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      foreach (Garnish garnish in Garnish.All)
      {
        if (component.Has(garnish.itemTag))
          descriptors.Add(garnish.descriptor);
      }
    }
    return descriptors;
  }

  public bool HasGarnish => this.smi.HasGarnish;

  public HashedString EatAnim => MessStation.eatAnim;

  public HashedString ReloadElectrobankAnim => MessStation.reloadElectrobankAnim;

  public Storage FindStorage() => this.GetComponent<Storage>();

  public Operational FindOperational() => this.GetComponent<Operational>();

  public KPrefabID Diner { get; set; }

  public class MessStationSM : 
    GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
  {
    public MessStation.MessStationSM.SaltState salt;
    public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.salt.none;
      this.salt.none.Transition(this.salt.salty, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => smi.HasGarnish)).PlayAnim("off");
      this.salt.salty.Transition(this.salt.none, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => !smi.HasGarnish)).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating);
      this.eating.Transition(this.salt.salty, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => smi.HasGarnish && !smi.IsEating())).Transition(this.salt.none, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => !smi.HasGarnish && !smi.IsEating())).PlayAnim("off");
    }

    public class SaltState : 
      GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
    {
      public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;
      public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
    }

    public new class Instance : 
      GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
    {
      private Storage garnishStorage;
      private Reservable reservable;
      private SymbolOverrideController symbolOverrideController;
      private static readonly HashedString SALT_SYMBOL = (HashedString) "saltshaker";

      public Instance(MessStation master)
        : base(master)
      {
        this.garnishStorage = master.GetComponent<Storage>();
        this.reservable = master.GetComponent<Reservable>();
        this.symbolOverrideController = master.GetComponent<SymbolOverrideController>();
        this.garnishStorage.Subscribe(-1697596308, (Action<object>) (_ => this.UpdateGarnishOverride()));
        this.UpdateGarnishOverride();
      }

      public bool HasGarnish => Garnish.HasAny(this.garnishStorage);

      public void UpdateGarnishOverride()
      {
        if ((UnityEngine.Object) this.symbolOverrideController == (UnityEngine.Object) null)
          return;
        KAnim.Build.Symbol overrideSymbol = Garnish.GetActive(this.garnishStorage)?.GetOverrideSymbol();
        if (overrideSymbol != null)
          this.symbolOverrideController.AddSymbolOverride(MessStation.MessStationSM.Instance.SALT_SYMBOL, overrideSymbol);
        else
          this.symbolOverrideController.RemoveSymbolOverride(MessStation.MessStationSM.Instance.SALT_SYMBOL);
      }

      public bool IsEating()
      {
        ChoreDriver component;
        if ((UnityEngine.Object) this.reservable == (UnityEngine.Object) null || (UnityEngine.Object) this.reservable.ReservedBy == (UnityEngine.Object) null || !this.reservable.ReservedBy.TryGetComponent<ChoreDriver>(out component) || !component.HasChore())
          return false;
        return component.GetCurrentChore() is ReloadElectrobankChore currentChore ? currentChore.IsInstallingAtMessStation() : component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
      }
    }
  }
}
