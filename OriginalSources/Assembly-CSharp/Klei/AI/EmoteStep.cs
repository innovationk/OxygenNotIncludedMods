// Decompiled with JetBrains decompiler
// Type: Klei.AI.EmoteStep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class EmoteStep
{
  public HashedString anim = HashedString.Invalid;
  public KAnim.PlayMode mode = KAnim.PlayMode.Once;
  public float timeout = -1f;
  private HandleVector<EmoteStep.Callbacks> callbacks = new HandleVector<EmoteStep.Callbacks>(64 /*0x40*/);

  public int Id => this.anim.HashValue;

  public HandleVector<EmoteStep.Callbacks>.Handle RegisterCallbacks(
    Action<GameObject> startedCb,
    Action<GameObject> finishedCb)
  {
    if (startedCb == null && finishedCb == null)
      return HandleVector<EmoteStep.Callbacks>.InvalidHandle;
    return this.callbacks.Add(new EmoteStep.Callbacks()
    {
      StartedCb = startedCb,
      FinishedCb = finishedCb
    });
  }

  public void UnregisterCallbacks(
    HandleVector<EmoteStep.Callbacks>.Handle callbackHandle)
  {
    this.callbacks.Release(callbackHandle);
  }

  public void UnregisterAllCallbacks()
  {
    this.callbacks = new HandleVector<EmoteStep.Callbacks>(64 /*0x40*/);
  }

  public void OnStepStarted(
    HandleVector<EmoteStep.Callbacks>.Handle callbackHandle,
    GameObject parameter)
  {
    if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
      return;
    EmoteStep.Callbacks callbacks = this.callbacks.GetItem(callbackHandle);
    if (callbacks.StartedCb == null)
      return;
    callbacks.StartedCb(parameter);
  }

  public void OnStepFinished(
    HandleVector<EmoteStep.Callbacks>.Handle callbackHandle,
    GameObject parameter)
  {
    if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
      return;
    EmoteStep.Callbacks callbacks = this.callbacks.GetItem(callbackHandle);
    if (callbacks.FinishedCb == null)
      return;
    callbacks.FinishedCb(parameter);
  }

  public struct Callbacks
  {
    public Action<GameObject> StartedCb;
    public Action<GameObject> FinishedCb;
  }
}
