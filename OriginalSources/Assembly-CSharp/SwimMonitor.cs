// Decompiled with JetBrains decompiler
// Type: SwimMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SwimMonitor : GameStateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget>
{
  public static StatusItem HasSuitSwimPenalty = new StatusItem(nameof (HasSuitSwimPenalty), (string) DUPLICANTS.STATUSITEMS.HASSUITSWIMPENALTY.NAME, (string) DUPLICANTS.STATUSITEMS.HASSUITSWIMPENALTY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  public static Dictionary<HashedString, HashedString> SurfaceSwimOverride = new Dictionary<HashedString, HashedString>()
  {
    {
      (HashedString) "swim_swim_1_0_loop",
      (HashedString) "shallow_swim_1_0_loop"
    }
  };
  public static Dictionary<HashedString, Dictionary<HashedString, HashedString>> transitionAnims = new Dictionary<HashedString, Dictionary<HashedString, HashedString>>()
  {
    {
      (HashedString) "treading_loop",
      new Dictionary<HashedString, HashedString>()
      {
        {
          (HashedString) "shallow_swim_1_0_loop",
          (HashedString) "treading_trans_shallow_swim_1_0"
        },
        {
          (HashedString) "swim_swim_1_0_loop",
          (HashedString) "treading_trans_swim_1_0"
        },
        {
          (HashedString) "swim_swim_1_-1_loop",
          (HashedString) "treading_trans_swim_1_-1"
        },
        {
          (HashedString) "swim_swim_0_-1_loop",
          (HashedString) "treading_trans_swim_0_-1"
        }
      }
    },
    {
      (HashedString) "shallow_swim_1_0_loop",
      new Dictionary<HashedString, HashedString>()
      {
        {
          (HashedString) "swim_swim_1_0_loop",
          (HashedString) "shallow_trans_swim_1_0"
        },
        {
          (HashedString) "swim_swim_1_-1_loop",
          (HashedString) "shallow_trans_swim_1_-1"
        },
        {
          (HashedString) "swim_swim_0_-1_loop",
          (HashedString) "shallow_trans_swim_0_-1"
        }
      }
    },
    {
      (HashedString) "swim_swim_1_0_loop",
      new Dictionary<HashedString, HashedString>()
      {
        {
          (HashedString) "shallow_swim_1_0_loop",
          (HashedString) "horizontal_trans_shallow_swim_1_0"
        },
        {
          (HashedString) "swim_swim_1_-1_loop",
          (HashedString) "horizontal_trans_swim_1_-1"
        },
        {
          (HashedString) "swim_swim_0_-1_loop",
          (HashedString) "horizontal_trans_swim_0_-1"
        },
        {
          (HashedString) "swim_swim_0_1_loop",
          (HashedString) "horizontal_trans_swim_0_1"
        },
        {
          (HashedString) "swim_swim_1_1_loop",
          (HashedString) "horizontal_trans_swim_1_1"
        }
      }
    },
    {
      (HashedString) "swim_swim_0_1_loop",
      new Dictionary<HashedString, HashedString>()
      {
        {
          (HashedString) "swim_swim_1_-1_loop",
          (HashedString) "up_trans_swim_1_-1"
        },
        {
          (HashedString) "swim_swim_0_-1_loop",
          (HashedString) "up_trans_swim_0_-1"
        },
        {
          (HashedString) "swim_swim_0_1_loop",
          (HashedString) "up_trans_swim_1_0"
        },
        {
          (HashedString) "swim_swim_1_1_loop",
          (HashedString) "up_trans_swim_1_1"
        }
      }
    },
    {
      (HashedString) "swim_swim_0_-1_loop",
      new Dictionary<HashedString, HashedString>()
      {
        {
          (HashedString) "swim_swim_1_0_loop",
          (HashedString) "down_trans_swim_1_0"
        },
        {
          (HashedString) "swim_swim_0_1_loop",
          (HashedString) "down_trans_swim_0_1"
        },
        {
          (HashedString) "swim_swim_1_-1_loop",
          (HashedString) "down_trans_swim_1_-1"
        },
        {
          (HashedString) "swim_swim_1_1_loop",
          (HashedString) "down_trans_swim_1_1"
        }
      }
    },
    {
      (HashedString) "swim_swim_1_1_loop",
      new Dictionary<HashedString, HashedString>()
      {
        {
          (HashedString) "swim_swim_1_0_loop",
          (HashedString) "up_diagonal_trans_swim_1_0"
        },
        {
          (HashedString) "swim_swim_0_1_loop",
          (HashedString) "up_diagonal_trans_swim_0_1"
        },
        {
          (HashedString) "swim_swim_1_-1_loop",
          (HashedString) "up_diagonal_trans_swim_1_-1"
        },
        {
          (HashedString) "swim_swim_0_-1_loop",
          (HashedString) "up_diagonal_trans_swim_0_-1"
        },
        {
          (HashedString) "shallow_swim_1_0_loop",
          (HashedString) "up_diagonal_trans_shallow_swim_1_0"
        }
      }
    },
    {
      (HashedString) "swim_swim_1_-1_loop",
      new Dictionary<HashedString, HashedString>()
      {
        {
          (HashedString) "swim_swim_0_1_loop",
          (HashedString) "down_diagonal_trans_swim_0_1"
        },
        {
          (HashedString) "swim_swim_1_0_loop",
          (HashedString) "down_diagonal_trans_swim_1_0"
        },
        {
          (HashedString) "swim_swim_1_1_loop",
          (HashedString) "down_diagonal_trans_swim_1_1"
        },
        {
          (HashedString) "swim_swim_0_-1_loop",
          (HashedString) "down_diagonal_trans_swim_0_-1"
        }
      }
    }
  };
  public static float OffsetEpsilon = 0.0001f;
  private static AttributeModifier swimmingStaminaModifier = new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.06666667f, (string) DUPLICANTS.MODIFIERS.SWIMMINGSTAMINA.NAME);
  private static AttributeModifier swimmingAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, 3f, (string) DUPLICANTS.MODIFIERS.SWIMMINGATHLETICS.NAME);
  private static AttributeModifier inLiquidStaminaModifier = new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, -0.06666667f, (string) DUPLICANTS.MODIFIERS.INLIQUIDSTAMINA.NAME);
  public GameStateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.State cannotSwim;
  public GameStateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.State canSwim;
  public StateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.BoolParameter hasSwimSkill;
  public StateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.BoolParameter hasSwimSkill2;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.cannotSwim;
    this.root.EventHandler(GameHashes.RolesUpdated, (Func<SwimMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), new StateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.State.Callback(SwimMonitor.CheckSwimSkill)).EventHandler(GameHashes.PathAdvanced, (GameStateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, data) => this.OnPathAdvanced(smi, data)));
    this.cannotSwim.Enter((StateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (smi.navigator.CurrentNavType != NavType.Swim)
        return;
      smi.navigator.SetCurrentNavType(NavType.Floor);
      smi.navigator.Stop();
    })).ParamTransition<bool>((StateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.hasSwimSkill, this.canSwim, GameStateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.IsTrue);
    this.canSwim.ParamTransition<bool>((StateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.hasSwimSkill, this.cannotSwim, GameStateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.IsFalse).Enter((StateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      bool add = smi.navigator.CurrentNavType == NavType.Swim;
      if (smi.resume.HasPerk(smi.swimStaminaPerk))
        smi.ApplyAttributeModifier(SwimMonitor.swimmingStaminaModifier, add);
      if (smi.resume.HasPerk(smi.swimAthleticPerk))
        smi.ApplyAttributeModifier(SwimMonitor.swimmingAthleticsModifier, add);
      smi.previousNavType = smi.navigator.CurrentNavType;
    })).ToggleAnims("anim_loco_swim_kanim").Update((System.Action<SwimMonitor.Instance, float>) ((smi, dt) => SwimMonitor.UpdateSwimOffset(smi)));
  }

  private static void UpdateSwimOffset(SwimMonitor.Instance smi)
  {
    if (smi.navigator.IsMoving() || smi.navigator.CurrentNavType != NavType.Swim)
      return;
    SwimMonitor.SetSwimOffset(smi);
  }

  private static void SetSwimOffset(SwimMonitor.Instance smi)
  {
    Vector3 offset = smi.animController.Offset with
    {
      y = SwimMonitor.ComputeSwimOffsetY(smi.navigator.cachedCell)
    };
    if ((double) MathF.Abs(offset.y - smi.animController.Offset.y) <= (double) SwimMonitor.OffsetEpsilon)
      return;
    smi.animController.Offset = offset;
  }

  public static void CheckSwimSkill(SwimMonitor.Instance smi)
  {
    smi.sm.hasSwimSkill.Set(smi.resume.HasPerk(Db.Get().SkillPerks.CanSwim), smi);
  }

  public static float ComputeSwimOffsetY(int cell)
  {
    return Mathf.Clamp((float) (((double) Grid.Mass[cell] / 1000.0 - 1.0) * 0.5), -0.6f, 0.0f);
  }

  public void OnPathAdvanced(SwimMonitor.Instance smi, object data)
  {
    int num = smi.sm.hasSwimSkill.Get(smi) ? 1 : 0;
    bool add1 = smi.navigator.CurrentNavType == NavType.Swim;
    if (num != 0)
    {
      bool on = false;
      if (add1)
        on = (smi.navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) != 0 || (smi.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) != 0 || (smi.navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) != 0;
      bool flag = smi.previousNavType == NavType.Swim;
      if (add1 != flag)
      {
        if (smi.resume.HasPerk(smi.swimStaminaPerk))
          smi.ApplyAttributeModifier(SwimMonitor.swimmingStaminaModifier, add1);
        if (smi.resume.HasPerk(smi.swimAthleticPerk))
          smi.ApplyAttributeModifier(SwimMonitor.swimmingAthleticsModifier, add1);
      }
      smi.previousNavType = smi.navigator.CurrentNavType;
      smi.selectable.ToggleStatusItem(SwimMonitor.HasSuitSwimPenalty, on);
    }
    bool add2 = add1 || Grid.IsSubstantialLiquid(smi.navigator.cachedCell);
    if (add2 == smi.wasInLiquid)
      return;
    smi.ApplyAttributeModifier(SwimMonitor.inLiquidStaminaModifier, add2);
    smi.wasInLiquid = add2;
  }

  public new class Instance : 
    GameStateMachine<SwimMonitor, SwimMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    [MyCmpReq]
    public MinionResume resume;
    public Navigator navigator;
    public KBatchedAnimController animController;
    public KSelectable selectable;
    public SkillPerk swimStaminaPerk;
    public SkillPerk swimAthleticPerk;
    public NavType previousNavType;
    public bool wasInLiquid;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.navigator = this.GetComponent<Navigator>();
      this.animController = this.GetComponent<KBatchedAnimController>();
      this.selectable = this.GetComponent<KSelectable>();
      this.previousNavType = this.navigator.CurrentNavType;
      this.swimStaminaPerk = Db.Get().SkillPerks.IncreaseSwimmerStaminaInLiquid;
      this.swimAthleticPerk = Db.Get().SkillPerks.IncreaseSwimmerAthleticsInLiquid;
      this.wasInLiquid = Grid.IsSubstantialLiquid(this.navigator.cachedCell);
      if (this.wasInLiquid)
        this.ApplyAttributeModifier(SwimMonitor.inLiquidStaminaModifier, true);
      SwimMonitor.CheckSwimSkill(this);
    }

    public bool CanSwim() => this.sm.hasSwimSkill.Get(this);

    public void ApplyAttributeModifier(AttributeModifier modifier, bool add)
    {
      Klei.AI.Attributes attributes = this.gameObject.GetAttributes();
      if (add)
        attributes.Add(modifier);
      else
        attributes.Remove(modifier);
    }
  }
}
