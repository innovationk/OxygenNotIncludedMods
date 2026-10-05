// Decompiled with JetBrains decompiler
// Type: Klei.AI.AmountInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace Klei.AI;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{amount.Name} {value} ({deltaAttribute.value}/{minAttribute.value}/{maxAttribute.value})")]
public class AmountInstance : ModifierInstance<Amount>, ISaveLoadable, ISim200ms
{
  [Serialize]
  public float value;
  public AttributeInstance minAttribute;
  public AttributeInstance maxAttribute;
  public AttributeInstance deltaAttribute;
  public Action<float> OnDelta;
  public System.Action OnMaxValueReached;
  public bool hide;
  private bool _paused;

  public Amount amount => this.modifier;

  public bool paused
  {
    get => this._paused;
    set
    {
      this._paused = this.paused;
      if (this._paused)
        this.Deactivate();
      else
        this.Activate();
    }
  }

  public float GetMin() => this.minAttribute.GetTotalValue();

  public float GetMax() => this.maxAttribute.GetTotalValue();

  public float GetDelta() => this.deltaAttribute.GetTotalValue();

  public AmountInstance(Amount amount, GameObject game_object)
    : base(game_object, amount)
  {
    Attributes attributes = game_object.GetAttributes();
    this.minAttribute = attributes.Add(amount.minAttribute);
    this.maxAttribute = attributes.Add(amount.maxAttribute);
    this.deltaAttribute = attributes.Add(amount.deltaAttribute);
  }

  public float SetValue(float value)
  {
    this.value = Mathf.Min(Mathf.Max(value, this.GetMin()), this.GetMax());
    return this.value;
  }

  public void Publish(float delta, float previous_value)
  {
    if (this.OnDelta != null)
      this.OnDelta(delta);
    if (this.OnMaxValueReached == null || (double) previous_value >= (double) this.GetMax() || (double) this.value < (double) this.GetMax())
      return;
    this.OnMaxValueReached();
  }

  public float ApplyDelta(float delta)
  {
    float previous_value = this.value;
    double num = (double) this.SetValue(this.value + delta);
    this.Publish(delta, previous_value);
    return this.value;
  }

  public string GetValueString() => this.amount.GetValueString(this);

  public string GetDescription() => this.amount.GetDescription(this);

  public string GetTooltip() => this.amount.GetTooltip(this);

  public Sprite GetSprite() => this.amount.GetSprite(this);

  public void Activate() => SimAndRenderScheduler.instance.Add((object) this);

  public void Sim200ms(float dt)
  {
  }

  public static void BatchUpdate(
    List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances,
    float time_delta)
  {
    if ((double) time_delta == 0.0)
      return;
    AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Reset(new AmountInstance.BatchUpdateContext(amount_instances, time_delta));
    GlobalJobManager.Run((IWorkItemCollection) AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance);
    AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Finish();
    AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Reset(AmountInstance.BatchUpdateContext.EmptyContext);
  }

  public void Deactivate() => SimAndRenderScheduler.instance.Remove((object) this);

  private struct BatchUpdateContext(
    List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances,
    float time_delta)
  {
    public List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances = amount_instances;
    public float time_delta = time_delta;
    public static AmountInstance.BatchUpdateContext EmptyContext = new AmountInstance.BatchUpdateContext((List<UpdateBucketWithUpdater<ISim200ms>.Entry>) null, 0.0f);

    public struct Result
    {
      public AmountInstance amount_instance;
      public float previous;
      public float delta;
    }
  }

  private class AmmountInstanceBatchUpdateDispatcher : 
    WorkItemCollectionWithThreadContex<AmountInstance.BatchUpdateContext, List<AmountInstance.BatchUpdateContext.Result>>
  {
    private const int kBatchSize = 512 /*0x0200*/;
    private static AmountInstance.AmmountInstanceBatchUpdateDispatcher instance;

    public static AmountInstance.AmmountInstanceBatchUpdateDispatcher Instance
    {
      get
      {
        if (AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance == null || AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance.threadContexts.Count != GlobalJobManager.ThreadCount)
          AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance = new AmountInstance.AmmountInstanceBatchUpdateDispatcher();
        return AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance;
      }
    }

    public AmmountInstanceBatchUpdateDispatcher()
    {
      this.threadContexts = new List<List<AmountInstance.BatchUpdateContext.Result>>(GlobalJobManager.ThreadCount);
      for (int index = 0; index < GlobalJobManager.ThreadCount; ++index)
        this.threadContexts.Add(new List<AmountInstance.BatchUpdateContext.Result>());
    }

    public void Reset(AmountInstance.BatchUpdateContext context)
    {
      this.sharedData = context;
      if (context.amount_instances == null)
        this.count = 0;
      else
        this.count = (context.amount_instances.Count + 512 /*0x0200*/ - 1) / 512 /*0x0200*/;
    }

    public override void RunItem(
      int item,
      ref AmountInstance.BatchUpdateContext shared_data,
      List<AmountInstance.BatchUpdateContext.Result> thread_context,
      int threadIndex)
    {
      int num1 = item * 512 /*0x0200*/;
      int num2 = Mathf.Min(num1 + 512 /*0x0200*/, shared_data.amount_instances.Count);
      for (int index = num1; index < num2; ++index)
      {
        AmountInstance data = (AmountInstance) shared_data.amount_instances[index].data;
        float num3 = data.GetDelta() * shared_data.time_delta;
        if ((double) num3 != 0.0)
        {
          thread_context.Add(new AmountInstance.BatchUpdateContext.Result()
          {
            amount_instance = data,
            previous = data.value,
            delta = num3
          });
          double num4 = (double) data.SetValue(data.value + num3);
        }
      }
    }

    public void Finish()
    {
      foreach (List<AmountInstance.BatchUpdateContext.Result> threadContext in this.threadContexts)
      {
        foreach (AmountInstance.BatchUpdateContext.Result result in threadContext)
          result.amount_instance.Publish(result.delta, result.previous);
        threadContext.Clear();
      }
    }
  }
}
