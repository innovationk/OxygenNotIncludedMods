// Decompiled with JetBrains decompiler
// Type: BeckonFromSpaceStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
internal class BeckonFromSpaceStates : 
  GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>
{
  public BeckonFromSpaceStates.BeckoningState beckoning;
  public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.beckoning;
    this.beckoning.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Beckoning).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.ChooseSong)).DefaultState(this.beckoning.pre);
    this.beckoning.pre.PlayAnim(new Func<BeckonFromSpaceStates.Instance, string>(BeckonFromSpaceStates.GetSongAnimPre)).OnAnimQueueComplete(this.beckoning.loop);
    this.beckoning.loop.PlayAnim(new Func<BeckonFromSpaceStates.Instance, string>(BeckonFromSpaceStates.GetSongAnimLoop)).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.MooEchoFX)).OnAnimQueueComplete(this.beckoning.pst);
    this.beckoning.pst.PlayAnim(new Func<BeckonFromSpaceStates.Instance, string>(BeckonFromSpaceStates.GetSongAnimPst)).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.DoBeckon)).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.MooCheer)).BehaviourComplete(GameTags.Creatures.WantsToBeckon);
  }

  public static string GetSongAnimPre(BeckonFromSpaceStates.Instance smi)
  {
    return smi.ChosenSong.singAnimPre;
  }

  public static string GetSongAnimLoop(BeckonFromSpaceStates.Instance smi)
  {
    return smi.ChosenSong.singAnimLoop;
  }

  public static string GetSongAnimPst(BeckonFromSpaceStates.Instance smi)
  {
    return smi.ChosenSong.singAnimPst;
  }

  private static void ChooseSong(BeckonFromSpaceStates.Instance smi) => smi.ChooseSong();

  private static void MooEchoFX(BeckonFromSpaceStates.Instance smi)
  {
    KBatchedAnimController effect = FXHelpers.CreateEffect("moo_call_fx_kanim", smi.master.transform.position);
    effect.destroyOnAnimComplete = true;
    effect.Play((HashedString) "moo_call");
  }

  private static Util.IterationInstruction mooCheerVisitor(
    object obj,
    BeckonFromSpaceStates.Instance smi)
  {
    KPrefabID kprefabId = (obj as Pickupable).KPrefabID;
    if ((UnityEngine.Object) kprefabId.gameObject == (UnityEngine.Object) smi.gameObject || !kprefabId.HasTag((Tag) "Moo") || kprefabId.GetSMI<AnimInterruptMonitor.Instance>() == null)
      return Util.IterationInstruction.Continue;
    kprefabId.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(smi.def.choirAnims);
    return Util.IterationInstruction.Continue;
  }

  private static void MooCheer(BeckonFromSpaceStates.Instance smi)
  {
    Vector3 position = smi.transform.GetPosition();
    Extents extents = new Extents((int) position.x, (int) position.y, 15);
    GameScenePartitioner.Instance.VisitEntries<BeckonFromSpaceStates.Instance>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.pickupablesLayer, new Func<object, BeckonFromSpaceStates.Instance, Util.IterationInstruction>(BeckonFromSpaceStates.mooCheerVisitor), smi);
  }

  private static void DoBeckon(BeckonFromSpaceStates.Instance smi)
  {
    Db.Get().Amounts.Beckoning.Lookup(smi.gameObject).value = 0.0f;
    WorldContainer myWorld = smi.GetMyWorld();
    Vector3 position1 = smi.transform.position;
    float y = (float) (myWorld.Height + myWorld.WorldOffset.y - 1);
    float layerZ = Grid.GetLayerZ(smi.def.sceneLayer);
    float num1 = (y - position1.y) * Mathf.Tan(0.2617994f);
    double num2 = (double) position1.x + (double) UnityEngine.Random.Range(-5, 5);
    float num3 = (float) num2 - num1;
    float num4 = (float) num2 + num1;
    float x = position1.x;
    bool state = false;
    if ((double) num3 > (double) myWorld.WorldOffset.x && (double) num3 < (double) (myWorld.WorldOffset.x + myWorld.Width))
    {
      x = num3;
      state = false;
    }
    else if ((double) num3 > (double) myWorld.WorldOffset.x && (double) num3 < (double) (myWorld.WorldOffset.x + myWorld.Width))
    {
      x = num4;
      state = true;
    }
    DebugUtil.DevAssert(myWorld.ContainsPoint(new Vector2(x, y)), "Gassy Moo spawned outside world bounds");
    Vector3 position2 = new Vector3(x, y, layerZ);
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.ChosenSong.meteorID), position2, Quaternion.identity);
    GassyMooComet component = gameObject.GetComponent<GassyMooComet>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.spawnWithOffset = true;
      if ((double) x != (double) position1.x)
        component.SetCustomInitialFlip(state);
    }
    gameObject.SetActive(true);
  }

  public class Def : StateMachine.BaseDef
  {
    public Grid.SceneLayer sceneLayer;
    public HashedString[] choirAnims = new HashedString[1]
    {
      (HashedString) "reply_loop"
    };
  }

  public new class Instance : 
    GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.GameInstance
  {
    public BeckoningMonitor.SongChance ChosenSong;
    private BeckoningMonitor.Instance monitor;

    public Instance(Chore<BeckonFromSpaceStates.Instance> chore, BeckonFromSpaceStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToBeckon);
      this.monitor = this.gameObject.GetSMI<BeckoningMonitor.Instance>();
    }

    public void ChooseSong()
    {
      float num = UnityEngine.Random.value;
      BeckoningMonitor.SongChance songChance1 = (BeckoningMonitor.SongChance) null;
      foreach (BeckoningMonitor.SongChance songChance2 in this.monitor.songChances)
      {
        num -= songChance2.weight;
        if ((double) num <= 0.0)
        {
          songChance1 = songChance2;
          break;
        }
      }
      this.ChosenSong = songChance1;
    }
  }

  public class BeckoningState : 
    GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State
  {
    public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State pre;
    public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State loop;
    public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State pst;
  }
}
