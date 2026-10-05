// Decompiled with JetBrains decompiler
// Type: BabyMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
public class BabyMonitor : 
  GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>
{
  public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State baby;
  public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State spawnadult;
  public Effect babyEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.baby;
    this.root.Enter(new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State.Callback(BabyMonitor.AddBabyEffect));
    this.baby.Transition(this.spawnadult, new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.Transition.ConditionCallback(BabyMonitor.IsReadyToSpawnAdult), UpdateRate.SIM_4000ms);
    this.spawnadult.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, (StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.Transition.ConditionCallback) (smi => true));
    this.babyEffect = new Effect("IsABaby", (string) CREATURES.MODIFIERS.BABY.NAME, (string) CREATURES.MODIFIERS.BABY.TOOLTIP, 0.0f, true, false, false);
    this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.9f, (string) CREATURES.MODIFIERS.BABY.NAME, true));
    this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, (string) CREATURES.MODIFIERS.BABY.NAME));
  }

  private static void AddBabyEffect(BabyMonitor.Instance smi)
  {
    smi.Get<Effects>().Add(smi.sm.babyEffect, false);
  }

  private static bool IsReadyToSpawnAdult(BabyMonitor.Instance smi)
  {
    AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(smi.gameObject);
    float num = smi.def.adultThreshold;
    if (GenericGameSettings.instance.acceleratedLifecycle)
      num = 0.005f;
    return (double) amountInstance.value > (double) num;
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag adultPrefab;
    public string onGrowDropID;
    public float onGrowDropUnits = 1f;
    public bool forceAdultNavType;
    public float adultThreshold = 5f;
    public System.Action<GameObject> configureAdultOnMaturation;
  }

  public new class Instance(IStateMachineTarget master, BabyMonitor.Def def) : 
    GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.GameInstance(master, def)
  {
    public void SpawnAdult()
    {
      Vector3 position = this.smi.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Creatures)
      };
      GameObject gameObject1 = Util.KInstantiate(Assets.GetPrefab(this.smi.def.adultPrefab), position);
      gameObject1.SetActive(true);
      if (!this.smi.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
        gameObject1.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "growup_pst");
      if (this.smi.def.onGrowDropID != null)
      {
        GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab((Tag) this.smi.def.onGrowDropID), position);
        gameObject2.GetComponent<PrimaryElement>().Mass *= this.smi.def.onGrowDropUnits;
        gameObject2.SetActive(true);
      }
      foreach (AmountInstance modifier in this.gameObject.GetAmounts().ModifierList)
      {
        AmountInstance amountInstance = modifier.amount.Lookup(gameObject1);
        if (amountInstance != null)
        {
          float num = modifier.value / modifier.GetMax();
          amountInstance.value = num * amountInstance.GetMax();
        }
      }
      EffectInstance effectInstance = this.gameObject.GetComponent<Effects>().Get("AteFromFeeder");
      if (effectInstance != null)
        gameObject1.GetComponent<Effects>().Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
      if (!this.smi.def.forceAdultNavType)
      {
        Navigator component = this.smi.GetComponent<Navigator>();
        gameObject1.GetComponent<Navigator>().SetCurrentNavType(component.CurrentNavType);
      }
      gameObject1.Trigger(-2027483228, (object) this.gameObject);
      KSelectable component1 = this.gameObject.GetComponent<KSelectable>();
      if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component1)
        SelectTool.Instance.Select(gameObject1.GetComponent<KSelectable>());
      this.smi.gameObject.Trigger(663420073, (object) gameObject1);
      this.smi.gameObject.DeleteObject();
      if (this.def.configureAdultOnMaturation == null)
        return;
      this.def.configureAdultOnMaturation(gameObject1);
    }
  }
}
