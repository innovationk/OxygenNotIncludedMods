// Decompiled with JetBrains decompiler
// Type: MultitoolController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MultitoolController : 
  GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase>
{
  public GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State pre;
  public GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State loop;
  public GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State pst;
  public StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.TargetParameter worker;
  private static readonly string[][][] ANIM_BASE = new string[5][][]
  {
    new string[5][]
    {
      new string[3]
      {
        "{verb}_dn_pre",
        "{verb}_dn_loop",
        "{verb}_dn_pst"
      },
      new string[3]
      {
        "ladder_{verb}_dn_pre",
        "ladder_{verb}_dn_loop",
        "ladder_{verb}_dn_pst"
      },
      new string[3]
      {
        "pole_{verb}_dn_pre",
        "pole_{verb}_dn_loop",
        "pole_{verb}_dn_pst"
      },
      new string[3]
      {
        "jetpack_{verb}_dn_pre",
        "jetpack_{verb}_dn_loop",
        "jetpack_{verb}_dn_pst"
      },
      new string[3]
      {
        "swim_{verb}_dn_pre",
        "swim_{verb}_dn_loop",
        "swim_{verb}_dn_pst"
      }
    },
    new string[5][]
    {
      new string[3]
      {
        "{verb}_diag_dn_pre",
        "{verb}_diag_dn_loop",
        "{verb}_diag_dn_pst"
      },
      new string[3]
      {
        "ladder_{verb}_diag_dn_pre",
        "ladder_{verb}_loop_diag_dn",
        "ladder_{verb}_diag_dn_pst"
      },
      new string[3]
      {
        "pole_{verb}_diag_dn_pre",
        "pole_{verb}_loop_diag_dn",
        "pole_{verb}_diag_dn_pst"
      },
      new string[3]
      {
        "jetpack_{verb}_diag_dn_pre",
        "jetpack_{verb}_diag_dn_loop",
        "jetpack_{verb}_diag_dn_pst"
      },
      new string[3]
      {
        "swim_{verb}_diag_dn_pre",
        "swim_{verb}_diag_dn_loop",
        "swim_{verb}_diag_dn_pst"
      }
    },
    new string[5][]
    {
      new string[3]
      {
        "{verb}_fwd_pre",
        "{verb}_fwd_loop",
        "{verb}_fwd_pst"
      },
      new string[3]
      {
        "ladder_{verb}_pre",
        "ladder_{verb}_loop",
        "ladder_{verb}_pst"
      },
      new string[3]
      {
        "pole_{verb}_pre",
        "pole_{verb}_loop",
        "pole_{verb}_pst"
      },
      new string[3]
      {
        "jetpack_{verb}_fwd_pre",
        "jetpack_{verb}_fwd_loop",
        "jetpack_{verb}_fwd_pst"
      },
      new string[3]
      {
        "swim_{verb}_fwd_pre",
        "swim_{verb}_fwd_loop",
        "swim_{verb}_fwd_pst"
      }
    },
    new string[5][]
    {
      new string[3]
      {
        "{verb}_diag_up_pre",
        "{verb}_diag_up_loop",
        "{verb}_diag_up_pst"
      },
      new string[3]
      {
        "ladder_{verb}_diag_up_pre",
        "ladder_{verb}_loop_diag_up",
        "ladder_{verb}_diag_up_pst"
      },
      new string[3]
      {
        "pole_{verb}_diag_up_pre",
        "pole_{verb}_loop_diag_up",
        "pole_{verb}_diag_up_pst"
      },
      new string[3]
      {
        "jetpack_{verb}_diag_up_pre",
        "jetpack_{verb}_diag_up_loop",
        "jetpack_{verb}_diag_up_pst"
      },
      new string[3]
      {
        "swim_{verb}_diag_up_pre",
        "swim_{verb}_diag_up_loop",
        "swim_{verb}_diag_up_pst"
      }
    },
    new string[5][]
    {
      new string[3]
      {
        "{verb}_up_pre",
        "{verb}_up_loop",
        "{verb}_up_pst"
      },
      new string[3]
      {
        "ladder_{verb}_up_pre",
        "ladder_{verb}_up_loop",
        "ladder_{verb}_up_pst"
      },
      new string[3]
      {
        "pole_{verb}_up_pre",
        "pole_{verb}_up_loop",
        "pole_{verb}_up_pst"
      },
      new string[3]
      {
        "jetpack_{verb}_up_pre",
        "jetpack_{verb}_up_loop",
        "jetpack_{verb}_up_pst"
      },
      new string[3]
      {
        "swim_{verb}_up_pre",
        "swim_{verb}_up_loop",
        "swim_{verb}_up_pst"
      }
    }
  };
  private static Dictionary<string, string[][][]> TOOL_ANIM_SETS = new Dictionary<string, string[][][]>();
  private static Dictionary<HashedString, string[]> drone_anims = new Dictionary<HashedString, string[]>()
  {
    {
      (HashedString) "pickup",
      new string[3]{ "pickup_pre", "pickup_loop", "pickup_pst" }
    },
    {
      (HashedString) "store",
      new string[3]{ "deposit", "deposit", "deposit" }
    },
    {
      (HashedString) "build",
      new string[3]{ "build_pre", "build_loop", "build_pst" }
    }
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre;
    this.Target(this.worker);
    this.root.ToggleSnapOn("dig");
    this.pre.Enter((StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State.Callback) (smi =>
    {
      smi.PlayPre();
      this.worker.Get<Facing>(smi).Face(smi.workable.transform.GetPosition());
    })).OnAnimQueueComplete(this.loop);
    this.loop.Enter("PlayLoop", (StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State.Callback) (smi => smi.PlayLoop())).Enter("CreateHitEffect", (StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State.Callback) (smi => smi.CreateHitEffect())).Exit("DestroyHitEffect", (StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State.Callback) (smi => smi.DestroyHitEffect())).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst, (StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.Transition.ConditionCallback) (smi => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion));
    this.pst.Enter("PlayPost", (StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State.Callback) (smi => smi.PlayPost()));
  }

  public static string[] GetAnimationStrings(
    Workable workable,
    WorkerBase worker,
    string toolString = "dig")
  {
    Debug.Assert(toolString != "build");
    string[][][] strArray1;
    if (!MultitoolController.TOOL_ANIM_SETS.TryGetValue(toolString, out strArray1))
    {
      strArray1 = new string[MultitoolController.ANIM_BASE.Length][][];
      MultitoolController.TOOL_ANIM_SETS[toolString] = strArray1;
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        string[][] strArray2 = MultitoolController.ANIM_BASE[index1];
        string[][] strArray3 = new string[strArray2.Length][];
        strArray1[index1] = strArray3;
        for (int index2 = 0; index2 < strArray3.Length; ++index2)
        {
          string[] strArray4 = strArray2[index2];
          string[] strArray5 = new string[strArray4.Length];
          strArray3[index2] = strArray5;
          for (int index3 = 0; index3 < strArray5.Length; ++index3)
            strArray5[index3] = strArray4[index3].Replace("{verb}", toolString);
        }
      }
    }
    Vector3 target = Vector3.zero;
    Vector3 source = Vector3.zero;
    MultitoolController.GetTargetPoints(workable, worker, out source, out target);
    double num1 = (double) Mathf.Lerp(0.0f, 1f, Vector2.Angle(new Vector2(0.0f, -1f), new Vector2(target.x - source.x, target.y - source.y).normalized) / 180f);
    int length = strArray1.Length;
    double num2 = (double) length;
    int index4 = Math.Min((int) (num1 * num2), length - 1);
    NavType currentNavType = worker.GetComponent<Navigator>().CurrentNavType;
    int index5 = 0;
    switch (currentNavType)
    {
      case NavType.Ladder:
        index5 = 1;
        break;
      case NavType.Hover:
        index5 = 3;
        break;
      case NavType.Swim:
        index5 = 4;
        break;
      case NavType.Pole:
        index5 = 2;
        break;
    }
    return strArray1[index4][index5];
  }

  private static string[] GetDroneAnimationStrings(HashedString context)
  {
    string[] animationStrings;
    if (MultitoolController.drone_anims.TryGetValue(context, out animationStrings))
      return animationStrings;
    Debug.LogError((object) $"Missing drone multitool anims for context {context}");
    return new string[3]{ "", "", "" };
  }

  private static void GetTargetPoints(
    Workable workable,
    WorkerBase worker,
    out Vector3 source,
    out Vector3 target)
  {
    target = workable.GetTargetPoint();
    source = worker.transform.GetPosition();
    source.y += 0.7f;
  }

  public new class Instance : 
    GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.GameInstance
  {
    public Workable workable;
    private GameObject hitEffectPrefab;
    private GameObject hitEffect;
    private string[] anims;
    private bool inPlace;

    public Instance(
      Workable workable,
      WorkerBase worker,
      HashedString context,
      GameObject hit_effect)
      : base(worker)
    {
      this.hitEffectPrefab = hit_effect;
      worker.GetComponent<AnimEventHandler>().SetContext(context);
      this.sm.worker.Set((KMonoBehaviour) worker, this.smi);
      this.workable = workable;
      if (worker.IsFetchDrone())
        this.anims = MultitoolController.GetDroneAnimationStrings(context);
      else
        this.anims = MultitoolController.GetAnimationStrings(workable, worker);
    }

    public void PlayPre()
    {
      this.sm.worker.Get<KAnimControllerBase>(this.smi).Play((HashedString) this.anims[0]);
    }

    public void PlayLoop()
    {
      if (!(this.sm.worker.Get<KAnimControllerBase>(this.smi).currentAnim != (HashedString) this.anims[1]))
        return;
      this.sm.worker.Get<KAnimControllerBase>(this.smi).Play((HashedString) this.anims[1], KAnim.PlayMode.Loop);
    }

    public void PlayPost()
    {
      if (!(this.sm.worker.Get<KAnimControllerBase>(this.smi).currentAnim != (HashedString) this.anims[2]))
        return;
      this.sm.worker.Get<KAnimControllerBase>(this.smi).Play((HashedString) this.anims[2]);
    }

    public void UpdateHitEffectTarget()
    {
      if ((UnityEngine.Object) this.hitEffect == (UnityEngine.Object) null)
        return;
      WorkerBase worker = this.sm.worker.Get<WorkerBase>(this.smi);
      AnimEventHandler component = worker.GetComponent<AnimEventHandler>();
      Vector3 targetPoint = this.workable.GetTargetPoint();
      worker.GetComponent<Facing>().Face(this.workable.transform.GetPosition());
      this.anims = MultitoolController.GetAnimationStrings(this.workable, worker);
      this.PlayLoop();
      component.SetTargetPos(targetPoint);
      component.UpdateWorkTarget(this.workable.GetTargetPoint());
      this.hitEffect.transform.SetPosition(targetPoint);
    }

    public void CreateHitEffect()
    {
      WorkerBase cmp = this.sm.worker.Get<WorkerBase>(this.smi);
      if ((UnityEngine.Object) cmp == (UnityEngine.Object) null || (UnityEngine.Object) this.workable == (UnityEngine.Object) null)
        return;
      if (Grid.PosToCell((KMonoBehaviour) this.workable) != Grid.PosToCell((KMonoBehaviour) cmp))
        cmp.Trigger(-673283254, (object) null);
      Diggable workable = this.workable as Diggable;
      if ((bool) (UnityEngine.Object) workable)
      {
        Element targetElement = workable.GetTargetElement();
        cmp.Trigger(-1762453998, (object) targetElement);
      }
      if ((UnityEngine.Object) this.hitEffectPrefab == (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) this.hitEffect != (UnityEngine.Object) null)
        this.DestroyHitEffect();
      AnimEventHandler component1 = cmp.GetComponent<AnimEventHandler>();
      Vector3 targetPoint = this.workable.GetTargetPoint();
      component1.SetTargetPos(targetPoint);
      this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, targetPoint, Grid.SceneLayer.FXFront2);
      KBatchedAnimController component2 = this.hitEffect.GetComponent<KBatchedAnimController>();
      this.hitEffect.SetActive(true);
      component2.sceneLayer = Grid.SceneLayer.FXFront2;
      component2.enabled = false;
      component2.enabled = true;
      component1.UpdateWorkTarget(this.workable.GetTargetPoint());
    }

    public void DestroyHitEffect()
    {
      WorkerBase workerBase = this.sm.worker.Get<WorkerBase>(this.smi);
      if ((UnityEngine.Object) workerBase != (UnityEngine.Object) null)
      {
        workerBase.Trigger(-1559999068, (object) null);
        workerBase.Trigger(939543986, (object) null);
      }
      if ((UnityEngine.Object) this.hitEffectPrefab == (UnityEngine.Object) null || (UnityEngine.Object) this.hitEffect == (UnityEngine.Object) null)
        return;
      this.hitEffect.DeleteObject();
    }
  }

  private enum DigDirection
  {
    dig_down,
    dig_up,
  }
}
