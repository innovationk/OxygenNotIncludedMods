// Decompiled with JetBrains decompiler
// Type: BeckoningMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BeckoningMonitor : 
  GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventHandler(GameHashes.CaloriesConsumed, (GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.GameEvent.Callback) ((smi, data) => smi.OnCaloriesConsumed(data))).ToggleBehaviour(GameTags.Creatures.WantsToBeckon, (StateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsReadyToBeckon())).Update((System.Action<BeckoningMonitor.Instance, float>) ((smi, dt) => smi.UpdateBlockedStatusItem()), UpdateRate.SIM_1000ms);
  }

  [Serializable]
  public class SongChance
  {
    public Tag meteorID;
    public string singAnimPre;
    public string singAnimLoop;
    public string singAnimPst;
    public float weight;
  }

  public class Def : StateMachine.BaseDef
  {
    public List<BeckoningMonitor.SongChance> initialSongWeights;
    public float caloriesPerCycle;
    public string effectId = "MooWellFed";

    public override void Configure(GameObject prefab)
    {
      prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Beckoning.Id);
    }
  }

  public new class Instance : 
    GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.GameInstance
  {
    private AmountInstance beckoning;
    [Serialize]
    public List<BeckoningMonitor.SongChance> songChances;
    [MyCmpGet]
    private Effects effects;
    [MyCmpGet]
    public KSelectable kselectable;
    private Guid beckoningBlockedHandle;

    public Instance(IStateMachineTarget master, BeckoningMonitor.Def def)
      : base(master, def)
    {
      this.beckoning = Db.Get().Amounts.Beckoning.Lookup(this.gameObject);
      this.InitializSongChances();
    }

    private void InitializSongChances()
    {
      this.songChances = new List<BeckoningMonitor.SongChance>();
      if (this.def.initialSongWeights == null)
        return;
      foreach (BeckoningMonitor.SongChance initialSongWeight in this.def.initialSongWeights)
      {
        this.songChances.Add(new BeckoningMonitor.SongChance()
        {
          meteorID = initialSongWeight.meteorID,
          weight = initialSongWeight.weight,
          singAnimPre = initialSongWeight.singAnimPre,
          singAnimLoop = initialSongWeight.singAnimLoop,
          singAnimPst = initialSongWeight.singAnimPst
        });
        foreach (MooSongModifier mooSongModifier in Db.Get().MooSongModifiers.GetForTag(initialSongWeight.meteorID))
          mooSongModifier.ApplyFunction(this, initialSongWeight.meteorID);
      }
      this.NormalizeSongsChances();
    }

    public void AddSongChance(Tag type, float addedPercentChance)
    {
      foreach (BeckoningMonitor.SongChance songChance in this.songChances)
      {
        if (songChance.meteorID == type)
        {
          float num = Mathf.Min(1f - songChance.weight, Mathf.Max(0.0f - songChance.weight, addedPercentChance));
          songChance.weight += num;
        }
      }
      this.NormalizeSongsChances();
      this.master.Trigger(1105317911, (object) this.songChances);
    }

    public void NormalizeSongsChances()
    {
      float num = 0.0f;
      foreach (BeckoningMonitor.SongChance songChance in this.songChances)
        num += songChance.weight;
      foreach (BeckoningMonitor.SongChance songChance in this.songChances)
        songChance.weight /= num;
    }

    private bool IsSpaceVisible()
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      return Grid.IsValidCell(cell) && Grid.ExposedToSunlight[cell] > (byte) 0;
    }

    private bool IsBeckoningAvailable()
    {
      return (double) this.smi.beckoning.value >= (double) this.smi.beckoning.GetMax();
    }

    public bool IsReadyToBeckon() => this.IsBeckoningAvailable() && this.IsSpaceVisible();

    public void UpdateBlockedStatusItem()
    {
      bool flag = this.IsSpaceVisible();
      if (!flag && this.IsBeckoningAvailable() && this.beckoningBlockedHandle == Guid.Empty)
      {
        this.beckoningBlockedHandle = this.kselectable.AddStatusItem(Db.Get().CreatureStatusItems.BeckoningBlocked);
      }
      else
      {
        if (!flag)
          return;
        this.beckoningBlockedHandle = this.kselectable.RemoveStatusItem(this.beckoningBlockedHandle);
      }
    }

    public void OnCaloriesConsumed(object data)
    {
      CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>) data).value;
      (this.effects.Get(this.smi.def.effectId) ?? this.effects.Add(this.smi.def.effectId, true)).timeRemaining += (float) ((double) caloriesConsumedEvent.calories / (double) this.smi.def.caloriesPerCycle * 600.0);
    }
  }
}
