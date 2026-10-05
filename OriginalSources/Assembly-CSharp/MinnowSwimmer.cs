// Decompiled with JetBrains decompiler
// Type: MinnowSwimmer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class MinnowSwimmer : StateMachineComponent<MinnowSwimmer.StatesInstance>
{
  [MyCmpReq]
  private KPrefabID kPrefabID;
  [MyCmpGet]
  private Navigator navigator;
  private AttributeModifier[] attributeModifiers;

  protected override void OnSpawn()
  {
    string[] allAttributes = DUPLICANTSTATS.ALL_ATTRIBUTES;
    this.attributeModifiers = new AttributeModifier[allAttributes.Length];
    for (int index = 0; index < allAttributes.Length; ++index)
      this.attributeModifiers[index] = new AttributeModifier(allAttributes[index], 4f, (string) DUPLICANTS.CONGENITALTRAITS.MINNOW.NAME);
    this.smi.StartSM();
  }

  public void ApplyModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
      attributes.Add(this.attributeModifiers[index]);
  }

  public void RemoveModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
      attributes.Remove(this.attributeModifiers[index]);
  }

  public class StatesInstance(MinnowSwimmer master) : 
    GameStateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.GameInstance(master)
  {
    public bool IsSwimming()
    {
      return (Object) this.master.navigator != (Object) null && this.master.navigator.IsSwimming();
    }
  }

  public class States : 
    GameStateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer>
  {
    public GameStateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.State idle;
    public GameStateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.State swimming;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.TagTransition(GameTags.Dead, (GameStateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.State) null);
      this.idle.Transition(this.swimming, (StateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.Transition.ConditionCallback) (smi => smi.IsSwimming()));
      this.swimming.Enter("Swimming", (StateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.State.Callback) (smi => smi.master.ApplyModifiers())).Exit("NotSwimming", (StateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.State.Callback) (smi => smi.master.RemoveModifiers())).Transition(this.idle, (StateMachine<MinnowSwimmer.States, MinnowSwimmer.StatesInstance, MinnowSwimmer, object>.Transition.ConditionCallback) (smi => !smi.IsSwimming()));
    }
  }
}
