// Decompiled with JetBrains decompiler
// Type: BalloonFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
  public StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.TargetParameter fx;
  public KAnimFile defaultAnim = Assets.GetAnim((HashedString) "balloon_anim_kanim");
  private KAnimFile defaultBalloon = Assets.GetAnim((HashedString) "balloon_basic_red_kanim");
  private const string defaultAnimName = "balloon_anim_kanim";
  private const string balloonAnimName = "balloon_basic_red_kanim";
  private const string TARGET_SYMBOL_TO_OVERRIDE = "body";
  private const int TARGET_OVERRIDE_PRIORITY = 0;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.Target(this.fx);
    this.root.Exit("DestroyFX", (StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DestroyFX()));
  }

  public new class Instance : 
    GameStateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.GameInstance
  {
    private KBatchedAnimController balloonAnimController;
    private Option<BalloonOverrideSymbol> currentBodyOverrideSymbol;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.balloonAnimController = FXHelpers.CreateEffectOverride(new string[2]
      {
        "balloon_anim_kanim",
        "balloon_basic_red_kanim"
      }, master.gameObject.transform.GetPosition() + new Vector3(0.0f, 0.3f, 1f), master.transform, true, Grid.SceneLayer.Creatures);
      this.sm.fx.Set(this.balloonAnimController.gameObject, this.smi, false);
      this.balloonAnimController.defaultAnim = "idle_default";
      master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add((KAnimControllerBase) this.balloonAnimController.GetComponent<KBatchedAnimController>());
    }

    public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverride)
    {
      KAnimFile kanimFile = balloonOverride.animFile.IsSome() ? balloonOverride.animFile.Unwrap() : this.smi.sm.defaultBalloon;
      this.balloonAnimController.SwapAnims(new KAnimFile[2]
      {
        this.smi.sm.defaultAnim,
        kanimFile
      });
      SymbolOverrideController component = this.balloonAnimController.GetComponent<SymbolOverrideController>();
      if (this.currentBodyOverrideSymbol.IsSome())
        component.RemoveSymbolOverride((HashedString) "body");
      if (balloonOverride.symbol.IsNone())
      {
        if (this.currentBodyOverrideSymbol.IsSome())
          component.AddSymbolOverride((HashedString) "body", this.smi.sm.defaultAnim.GetData().build.GetSymbol((KAnimHashedString) "body"));
        this.balloonAnimController.SetBatchGroupOverride(HashedString.Invalid);
      }
      else
      {
        component.AddSymbolOverride((HashedString) "body", balloonOverride.symbol.Unwrap());
        this.balloonAnimController.SetBatchGroupOverride(kanimFile.batchTag);
      }
      this.currentBodyOverrideSymbol = (Option<BalloonOverrideSymbol>) balloonOverride;
    }

    public void DestroyFX() => Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
  }
}
