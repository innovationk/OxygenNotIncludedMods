// Decompiled with JetBrains decompiler
// Type: TransitionDriver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
public class TransitionDriver
{
  private static Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();
  public static ObjectPool<Navigator.ActiveTransition> TransitionPool = new ObjectPool<Navigator.ActiveTransition>((Func<Navigator.ActiveTransition>) (() => new Navigator.ActiveTransition()), collectionCheck: false, defaultCapacity: 128 /*0x80*/);
  private Stack<TransitionDriver.InterruptOverrideLayer> interruptOverrideStack = new Stack<TransitionDriver.InterruptOverrideLayer>(8);
  private Navigator.ActiveTransition transition;
  private Navigator navigator;
  private Vector3 targetPos;
  private bool isComplete;
  private Brain brain;
  public List<TransitionDriver.OverrideLayer> overrideLayers = new List<TransitionDriver.OverrideLayer>();
  private LoggerFS log;
  private Action<object> onAnimComplete_;
  private int onAnimCompleteHandle = -1;

  private Action<object> onAnimCompleteBinding
  {
    get
    {
      if (this.onAnimComplete_ == null)
        this.onAnimComplete_ = new Action<object>(this.OnAnimComplete);
      return this.onAnimComplete_;
    }
  }

  public Navigator.ActiveTransition GetTransition => this.transition;

  public TransitionDriver(Navigator navigator)
  {
    this.log = new LoggerFS(nameof (TransitionDriver));
  }

  public void BeginTransition(
    Navigator navigator,
    NavGrid.Transition transition,
    float defaultSpeed)
  {
    Navigator.ActiveTransition transition1 = TransitionDriver.TransitionPool.Get();
    transition1.Init(transition, defaultSpeed);
    this.BeginTransition(navigator, transition1);
  }

  private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    bool flag1 = this.interruptOverrideStack.Count != 0;
    foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
    {
      if (!flag1 || !(overrideLayer is TransitionDriver.InterruptOverrideLayer))
        overrideLayer.BeginTransition(navigator, transition);
    }
    this.navigator = navigator;
    this.transition = transition;
    this.isComplete = false;
    Grid.SceneLayer layer = navigator.sceneLayer;
    if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
      layer = Grid.SceneLayer.BuildingUse;
    else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
    {
      layer = Grid.SceneLayer.FXFront;
      navigator.animController.SetSceneLayer(layer);
    }
    else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
      navigator.animController.SetSceneLayer(layer);
    int target_cell = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) navigator), transition.x, transition.y);
    this.targetPos = this.GetTargetPosition(transition.navGridTransition, target_cell, layer);
    KAnimControllerBase animController = (KAnimControllerBase) navigator.animController;
    animController.PlaySpeedMultiplier = transition.animSpeed;
    if (transition.isLooping)
    {
      bool flag2 = transition.preAnim != (HashedString) "";
      bool flag3 = animController.CurrentAnim != null && (HashedString) animController.CurrentAnim.name == transition.anim;
      if ((!flag2 || animController.CurrentAnim == null ? 0 : ((HashedString) animController.CurrentAnim.name == transition.preAnim ? 1 : 0)) != 0)
      {
        animController.ClearQueue();
        animController.Queue(transition.anim, KAnim.PlayMode.Loop);
      }
      else if (flag3)
      {
        if (animController.PlayMode != KAnim.PlayMode.Loop)
        {
          animController.ClearQueue();
          animController.Queue(transition.anim, KAnim.PlayMode.Loop);
        }
      }
      else if (flag2)
      {
        animController.Play(transition.preAnim);
        animController.Queue(transition.anim, KAnim.PlayMode.Loop);
      }
      else
        animController.Play(transition.anim, KAnim.PlayMode.Loop);
    }
    else if (transition.anim != (HashedString) (string) null)
    {
      if (transition.preAnim != (HashedString) (string) null)
      {
        animController.Play(transition.preAnim);
        animController.Queue(transition.anim);
      }
      else
        animController.Play(transition.anim);
      navigator.Unsubscribe(this.onAnimCompleteHandle);
      this.onAnimCompleteHandle = navigator.Subscribe(-1061186183, this.onAnimCompleteBinding);
    }
    if (transition.navGridTransition.y != 0)
    {
      if (transition.navGridTransition.start == NavType.RightWall)
        navigator.facing.SetFacing(transition.navGridTransition.y < 0);
      else if (transition.navGridTransition.start == NavType.LeftWall)
        navigator.facing.SetFacing(transition.navGridTransition.y > 0);
    }
    if (transition.navGridTransition.x != 0)
    {
      if (transition.navGridTransition.start == NavType.Ceiling)
        navigator.facing.SetFacing(transition.navGridTransition.x > 0);
      else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
        navigator.facing.SetFacing(transition.navGridTransition.x < 0);
    }
    this.brain = navigator.GetComponent<Brain>();
  }

  private Vector3 GetTargetPosition(
    NavGrid.Transition trans,
    int target_cell,
    Grid.SceneLayer layer)
  {
    if (trans.useXOffset)
    {
      if (trans.x < 0)
        return Grid.CellToPosRBC(target_cell, layer);
      if (trans.x > 0)
        return Grid.CellToPosLBC(target_cell, layer);
    }
    return Grid.CellToPosCBC(target_cell, layer);
  }

  public void UpdateTransition(float dt)
  {
    if ((UnityEngine.Object) this.navigator == (UnityEngine.Object) null)
      return;
    foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
    {
      if (!(this.interruptOverrideStack.Count != 0 & overrideLayer is TransitionDriver.InterruptOverrideLayer) || this.interruptOverrideStack.Peek() == overrideLayer)
        overrideLayer.UpdateTransition(this.navigator, this.transition);
    }
    if ((UnityEngine.Object) this.brain != (UnityEngine.Object) null)
    {
      int num = this.isComplete ? 1 : 0;
    }
    if (this.transition.isLooping)
    {
      float speed = this.transition.speed;
      Vector3 position = this.navigator.transform.GetPosition();
      int cell1 = Grid.PosToCell(position);
      if (this.transition.x > 0)
      {
        position.x += dt * speed;
        if ((double) position.x > (double) this.targetPos.x)
          this.isComplete = true;
      }
      else if (this.transition.x < 0)
      {
        position.x -= dt * speed;
        if ((double) position.x < (double) this.targetPos.x)
          this.isComplete = true;
      }
      else
        position.x = this.targetPos.x;
      if (this.transition.y > 0)
      {
        position.y += dt * speed;
        if ((double) position.y > (double) this.targetPos.y)
          this.isComplete = true;
      }
      else if (this.transition.y < 0)
      {
        position.y -= dt * speed;
        if ((double) position.y < (double) this.targetPos.y)
          this.isComplete = true;
      }
      else
        position.y = this.targetPos.y;
      this.navigator.transform.SetPosition(position);
      int cell2 = Grid.PosToCell(position);
      if (cell2 != cell1)
        this.navigator.BoxingTrigger<int>(915392638, cell2);
    }
    if (!this.isComplete)
      return;
    this.isComplete = false;
    Navigator navigator = this.navigator;
    navigator.SetCurrentNavType(this.transition.end);
    navigator.transform.SetPosition(this.targetPos);
    this.EndTransition();
    navigator.AdvancePath();
  }

  public void EndTransition()
  {
    if (!((UnityEngine.Object) this.navigator != (UnityEngine.Object) null))
      return;
    this.interruptOverrideStack.Clear();
    foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
      overrideLayer.EndTransition(this.navigator, this.transition);
    this.navigator.animController.PlaySpeedMultiplier = 1f;
    this.navigator.Unsubscribe(ref this.onAnimCompleteHandle);
    if ((UnityEngine.Object) this.brain != (UnityEngine.Object) null)
      this.brain.Resume("move_handler");
    TransitionDriver.TransitionPool.Release(this.transition);
    this.transition = (Navigator.ActiveTransition) null;
    this.navigator = (Navigator) null;
    this.brain = (Brain) null;
  }

  private void OnAnimComplete(object data)
  {
    if ((UnityEngine.Object) this.navigator != (UnityEngine.Object) null)
      this.navigator.Unsubscribe(ref this.onAnimCompleteHandle);
    this.isComplete = true;
  }

  public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src)
  {
    Navigator.ActiveTransition activeTransition = TransitionDriver.TransitionPool.Get();
    activeTransition.Copy(src);
    src.Copy(TransitionDriver.emptyTransition);
    return activeTransition;
  }

  public class OverrideLayer
  {
    public OverrideLayer(Navigator navigator)
    {
    }

    public virtual void Destroy()
    {
    }

    public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
    }

    public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
    }

    public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
    }
  }

  public class InterruptOverrideLayer : TransitionDriver.OverrideLayer
  {
    protected Navigator.ActiveTransition originalTransition;
    protected TransitionDriver driver;

    protected bool InterruptInProgress => this.originalTransition != null;

    public InterruptOverrideLayer(Navigator navigator)
      : base(navigator)
    {
      this.driver = navigator.transitionDriver;
    }

    public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
      this.driver.interruptOverrideStack.Push(this);
      this.originalTransition = TransitionDriver.SwapTransitionWithEmpty(transition);
    }

    public override void UpdateTransition(
      Navigator navigator,
      Navigator.ActiveTransition transition)
    {
      if (!this.IsOverrideComplete())
        return;
      this.driver.interruptOverrideStack.Pop();
      transition.Copy(this.originalTransition);
      TransitionDriver.TransitionPool.Release(this.originalTransition);
      this.originalTransition = (Navigator.ActiveTransition) null;
      this.EndTransition(navigator, transition);
      this.driver.BeginTransition(navigator, transition);
    }

    public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
    {
      base.EndTransition(navigator, transition);
      if (this.originalTransition == null)
        return;
      TransitionDriver.TransitionPool.Release(this.originalTransition);
      this.originalTransition = (Navigator.ActiveTransition) null;
    }

    protected virtual bool IsOverrideComplete()
    {
      return this.originalTransition != null && this.driver.interruptOverrideStack.Count != 0 && this.driver.interruptOverrideStack.Peek() == this;
    }
  }
}
