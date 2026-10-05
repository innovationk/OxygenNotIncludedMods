// Decompiled with JetBrains decompiler
// Type: Meteorphile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class Meteorphile : StateMachineComponent<Meteorphile.StatesInstance>
{
  [MyCmpReq]
  private KPrefabID kPrefabID;
  private AttributeModifier[] attributeModifiers;

  protected override void OnSpawn()
  {
    this.attributeModifiers = new AttributeModifier[11]
    {
      new AttributeModifier("Construction", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Digging", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Machinery", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Athletics", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Learning", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Cooking", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Art", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Strength", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Caring", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Botanist", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME),
      new AttributeModifier("Ranching", TUNING.TRAITS.METEORPHILE_MODIFIER, (string) DUPLICANTS.TRAITS.METEORPHILE.NAME)
    };
    this.smi.StartSM();
  }

  public void ApplyModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Add(attributeModifier);
    }
  }

  public void RemoveModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Remove(attributeModifier);
    }
  }

  public class StatesInstance(Meteorphile master) : 
    GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.GameInstance(master)
  {
    public bool IsMeteors()
    {
      if ((Object) GameplayEventManager.Instance == (Object) null || this.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
        return false;
      int myWorldId = this.GetMyWorldId();
      List<GameplayEventInstance> results = new List<GameplayEventInstance>();
      GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(myWorldId, ref results);
      for (int index = 0; index < results.Count; ++index)
      {
        if (results[index].smi is MeteorShowerEvent.StatesInstance smi && smi.IsInsideState((StateMachine.BaseState) smi.sm.running.bombarding))
          return true;
      }
      return false;
    }
  }

  public class States : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile>
  {
    public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State idle;
    public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State early;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.TagTransition(GameTags.Dead, (GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State) null);
      this.idle.Transition(this.early, (StateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.Transition.ConditionCallback) (smi => smi.IsMeteors()));
      this.early.Enter("Meteors", (StateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State.Callback) (smi => smi.master.ApplyModifiers())).Exit("NotMeteors", (StateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State.Callback) (smi => smi.master.RemoveModifiers())).ToggleStatusItem(Db.Get().DuplicantStatusItems.Meteorphile).ToggleExpression(Db.Get().Expressions.Happy).Transition(this.idle, (StateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.Transition.ConditionCallback) (smi => !smi.IsMeteors()));
    }
  }
}
