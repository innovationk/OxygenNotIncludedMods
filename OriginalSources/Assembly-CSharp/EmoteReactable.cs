// Decompiled with JetBrains decompiler
// Type: EmoteReactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class EmoteReactable(
  GameObject gameObject,
  HashedString id,
  ChoreType chore_type,
  int range_width = 15,
  int range_height = 8,
  float globalCooldown = 0.0f,
  float localCooldown = 20f,
  float lifeSpan = float.PositiveInfinity,
  float max_initial_delay = 0.0f) : Reactable(gameObject, id, chore_type, range_width, range_height, true, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
{
  private KBatchedAnimController kbac;
  public Expression expression;
  public Thought thought;
  public Emote emote;
  private HandleVector<EmoteStep.Callbacks>.Handle[] callbackHandles;
  private KAnimFile swimOverrideAnimSet;
  private int currentStep = -1;
  private float elapsed;

  public EmoteReactable SetEmote(Emote emote)
  {
    this.emote = emote;
    return this;
  }

  public EmoteReactable RegisterEmoteStepCallbacks(
    HashedString stepName,
    Action<GameObject> startedCb,
    Action<GameObject> finishedCb)
  {
    if (this.callbackHandles == null)
      this.callbackHandles = new HandleVector<EmoteStep.Callbacks>.Handle[this.emote.StepCount];
    int stepIndex = this.emote.GetStepIndex(stepName);
    this.callbackHandles[stepIndex] = this.emote[stepIndex].RegisterCallbacks(startedCb, finishedCb);
    return this;
  }

  public EmoteReactable SetExpression(Expression expression)
  {
    this.expression = expression;
    return this;
  }

  public EmoteReactable SetThought(Thought thought)
  {
    this.thought = thought;
    return this;
  }

  public override bool InternalCanBegin(
    GameObject new_reactor,
    Navigator.ActiveTransition transition)
  {
    if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null || (UnityEngine.Object) new_reactor == (UnityEngine.Object) null)
      return false;
    Navigator component = new_reactor.GetComponent<Navigator>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.IsMoving() && (-257 & 1 << (int) (component.CurrentNavType & (NavType) 31 /*0x1F*/)) != 0 && (UnityEngine.Object) this.gameObject != (UnityEngine.Object) new_reactor;
  }

  public override void Update(float dt)
  {
    if (this.emote == null || !this.emote.IsValidStep(this.currentStep))
      return;
    if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
    {
      Facing component = this.reactor.GetComponent<Facing>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.Face(this.gameObject.transform.GetPosition());
    }
    float timeout = this.emote[this.currentStep].timeout;
    if ((double) timeout > 0.0 && (double) timeout < (double) this.elapsed)
      this.NextStep((HashedString) (string) null);
    else
      this.elapsed += dt;
  }

  protected override void InternalBegin()
  {
    this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
    Navigator component;
    this.swimOverrideAnimSet = this.reactor.TryGetComponent<Navigator>(out component) && component.CurrentNavType == NavType.Swim ? this.emote.ManifestSwimAnimSet() : (KAnimFile) null;
    this.emote.ApplyAnimOverrides(this.kbac, this.swimOverrideAnimSet);
    if (this.expression != null)
      this.reactor.GetComponent<FaceGraph>().AddExpression(this.expression);
    if (this.thought != null)
      this.reactor.GetSMI<ThoughtGraph.Instance>().AddThought(this.thought);
    this.NextStep((HashedString) (string) null);
  }

  protected override void InternalEnd()
  {
    if ((UnityEngine.Object) this.kbac != (UnityEngine.Object) null)
    {
      this.kbac.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.NextStep);
      this.emote.RemoveAnimOverrides(this.kbac, this.swimOverrideAnimSet);
      this.kbac = (KBatchedAnimController) null;
    }
    this.swimOverrideAnimSet = (KAnimFile) null;
    if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
    {
      if (this.expression != null)
        this.reactor.GetComponent<FaceGraph>().RemoveExpression(this.expression);
      if (this.thought != null)
        this.reactor.GetSMI<ThoughtGraph.Instance>().RemoveThought(this.thought);
    }
    this.currentStep = -1;
  }

  protected override void InternalCleanup()
  {
    if (this.emote == null || this.callbackHandles == null)
      return;
    for (int stepIdx = 0; this.emote.IsValidStep(stepIdx); ++stepIdx)
      this.emote[stepIdx].UnregisterCallbacks(this.callbackHandles[stepIdx]);
  }

  private void NextStep(HashedString finishedAnim)
  {
    if (this.emote.IsValidStep(this.currentStep) && (double) this.emote[this.currentStep].timeout <= 0.0)
    {
      this.kbac.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.NextStep);
      if (this.callbackHandles != null)
        this.emote[this.currentStep].OnStepFinished(this.callbackHandles[this.currentStep], this.reactor);
    }
    ++this.currentStep;
    if (!this.emote.IsValidStep(this.currentStep) || (UnityEngine.Object) this.kbac == (UnityEngine.Object) null)
    {
      this.End();
    }
    else
    {
      EmoteStep emoteStep = this.emote[this.currentStep];
      if (emoteStep.anim != HashedString.Invalid)
      {
        this.kbac.Play(emoteStep.anim, emoteStep.mode);
        if (this.kbac.IsStopped())
          emoteStep.timeout = 0.25f;
      }
      if ((double) emoteStep.timeout <= 0.0)
        this.kbac.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.NextStep);
      else
        this.elapsed = 0.0f;
      if (this.callbackHandles == null)
        return;
      emoteStep.OnStepStarted(this.callbackHandles[this.currentStep], this.reactor);
    }
  }
}
