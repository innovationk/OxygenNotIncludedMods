// Decompiled with JetBrains decompiler
// Type: BrainScheduler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/BrainScheduler")]
public class BrainScheduler : KMonoBehaviour, IRenderEveryTick
{
  public const float millisecondsPerFrame = 33.33333f;
  public const float secondsPerFrame = 0.0333333276f;
  public const float framesPerSecond = 30.0000057f;
  private List<BrainScheduler.BrainGroup> brainGroups = new List<BrainScheduler.BrainGroup>();

  public List<BrainScheduler.BrainGroup> debugGetBrainGroups() => this.brainGroups;

  protected override void OnPrefabInit()
  {
    this.brainGroups.Add((BrainScheduler.BrainGroup) new BrainScheduler.DupeBrainGroup());
    this.brainGroups.Add((BrainScheduler.BrainGroup) new BrainScheduler.CreatureBrainGroup());
    Components.Brains.Register(new Action<Brain>(this.OnAddBrain), new Action<Brain>(this.OnRemoveBrain));
  }

  private void OnAddBrain(Brain brain)
  {
    bool test = false;
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
    {
      if (brain.HasTag(brainGroup.tag))
      {
        brainGroup.AddBrain(brain);
        test = true;
      }
    }
    DebugUtil.Assert(test);
  }

  private void OnRemoveBrain(Brain brain)
  {
    bool test = false;
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
    {
      if (brain.HasTag(brainGroup.tag))
      {
        test = true;
        brainGroup.RemoveBrain(brain);
      }
      Navigator component = brain.GetComponent<Navigator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.executePathProbeTaskAsync = false;
    }
    DebugUtil.Assert(test);
  }

  public void PrioritizeBrain(Brain brain)
  {
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
    {
      if (brain.HasTag(brainGroup.tag))
        brainGroup.PrioritizeBrain(brain);
    }
  }

  public void RenderEveryTick(float dt)
  {
    if (Game.IsQuitting() || KMonoBehaviour.isLoadingScene)
      return;
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
    {
      brainGroup.RenderEveryTick(dt);
      brainGroup.PostRenderEveryTick(dt);
    }
  }

  protected override void OnForcedCleanUp() => base.OnForcedCleanUp();

  public abstract class BrainGroup
  {
    protected List<Brain> brains = new List<Brain>();
    protected Queue<Brain> priorityBrains = new Queue<Brain>();
    private string increaseLoadLabel;
    private string decreaseLoadLabel;
    public bool debugFreezeLoadAdustment;
    public int debugMaxPriorityBrainCountSeen;
    protected int nextUpdateBrain;

    public Tag tag { get; private set; }

    protected BrainGroup(Tag tag)
    {
      this.tag = tag;
      string str = tag.ToString();
      this.increaseLoadLabel = "IncLoad" + str;
      this.decreaseLoadLabel = "DecLoad" + str;
    }

    public void AddBrain(Brain brain) => this.brains.Add(brain);

    public void RemoveBrain(Brain brain)
    {
      int num = this.brains.IndexOf(brain);
      if (num != -1)
      {
        this.brains.RemoveAt(num);
        this.OnRemoveBrain(num, ref this.nextUpdateBrain);
      }
      if (!this.priorityBrains.Contains(brain))
        return;
      List<Brain> collection = new List<Brain>((IEnumerable<Brain>) this.priorityBrains);
      collection.Remove(brain);
      this.priorityBrains = new Queue<Brain>((IEnumerable<Brain>) collection);
    }

    public int BrainCount => this.brains.Count;

    public void PrioritizeBrain(Brain brain)
    {
      if (this.priorityBrains.Contains(brain))
        return;
      this.priorityBrains.Enqueue(brain);
    }

    private void IncrementBrainIndex(ref int brainIndex)
    {
      ++brainIndex;
      if (brainIndex != this.brains.Count)
        return;
      brainIndex = 0;
    }

    private void ClampBrainIndex(ref int brainIndex)
    {
      brainIndex = MathUtil.Clamp(0, this.brains.Count - 1, brainIndex);
    }

    private void OnRemoveBrain(int removedIndex, ref int brainIndex)
    {
      if (removedIndex < brainIndex)
      {
        --brainIndex;
      }
      else
      {
        if (brainIndex != this.brains.Count)
          return;
        brainIndex = 0;
      }
    }

    public void RenderEveryTick(float dt)
    {
      this.BeginBrainGroupUpdate();
      int num = this.InitialProbeCount();
      for (int index = 0; index != this.brains.Count && num != 0; ++index)
      {
        this.ClampBrainIndex(ref this.nextUpdateBrain);
        this.debugMaxPriorityBrainCountSeen = Mathf.Max(this.debugMaxPriorityBrainCountSeen, this.priorityBrains.Count);
        Brain brain;
        if (this.AllowPriorityBrains() && this.priorityBrains.Count > 0)
        {
          brain = this.priorityBrains.Dequeue();
        }
        else
        {
          brain = this.brains[this.nextUpdateBrain];
          this.IncrementBrainIndex(ref this.nextUpdateBrain);
        }
        if (brain.IsRunning())
        {
          brain.UpdateBrain();
          --num;
        }
      }
      this.EndBrainGroupUpdate();
    }

    public virtual void PostRenderEveryTick(float dt)
    {
    }

    protected abstract int InitialProbeCount();

    protected abstract int InitialProbeSize();

    protected abstract int MinProbeSize();

    protected abstract int IdealProbeSize();

    protected abstract int ProbeSizeStep();

    public abstract float GetEstimatedFrameTime();

    public abstract float LoadBalanceThreshold();

    public abstract bool AllowPriorityBrains();

    public virtual void BeginBrainGroupUpdate()
    {
    }

    public virtual void EndBrainGroupUpdate()
    {
    }
  }

  private class DupeBrainGroup : BrainScheduler.BrainGroup
  {
    private bool usePriorityBrain = true;

    public DupeBrainGroup()
      : base(GameTags.DupeBrain)
    {
    }

    protected override int InitialProbeCount()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeCount;
    }

    protected override int InitialProbeSize()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeSize;
    }

    protected override int MinProbeSize()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().minProbeSize;
    }

    protected override int IdealProbeSize()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().idealProbeSize;
    }

    protected override int ProbeSizeStep()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().probeSizeStep;
    }

    public override float GetEstimatedFrameTime()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().estimatedFrameTime;
    }

    public override float LoadBalanceThreshold()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().loadBalanceThreshold;
    }

    public override bool AllowPriorityBrains() => this.usePriorityBrain;

    public override void BeginBrainGroupUpdate()
    {
      base.BeginBrainGroupUpdate();
      this.usePriorityBrain = !this.usePriorityBrain;
    }

    public class Tuning : TuningData<BrainScheduler.DupeBrainGroup.Tuning>
    {
      public int initialProbeCount = 1;
      public int initialProbeSize = 1000;
      public int minProbeSize = 100;
      public int idealProbeSize = 1000;
      public int probeSizeStep = 100;
      public float estimatedFrameTime = 2f;
      public float loadBalanceThreshold = 0.1f;
    }
  }

  private class CreatureBrainGroup : BrainScheduler.BrainGroup
  {
    public CreatureBrainGroup()
      : base(GameTags.CreatureBrain)
    {
    }

    protected override int InitialProbeCount()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeCount;
    }

    protected override int InitialProbeSize()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeSize;
    }

    protected override int MinProbeSize()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().minProbeSize;
    }

    protected override int IdealProbeSize()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().idealProbeSize;
    }

    protected override int ProbeSizeStep()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().probeSizeStep;
    }

    public override float GetEstimatedFrameTime()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().estimatedFrameTime;
    }

    public override float LoadBalanceThreshold()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().loadBalanceThreshold;
    }

    public override void PostRenderEveryTick(float dt)
    {
      int num = this.InitialProbeCount();
      for (int index1 = 0; index1 < this.brains.Count && index1 < num; ++index1)
      {
        int index2 = (this.nextUpdateBrain + index1) % this.brains.Count;
        if (this.brains[index2].IsRunning())
        {
          Navigator component = this.brains[index2].GetComponent<Navigator>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.NavGrid != null)
            component.NavGrid.AddDirtyCell(component.cachedCell);
        }
      }
    }

    public override bool AllowPriorityBrains() => true;

    public class Tuning : TuningData<BrainScheduler.CreatureBrainGroup.Tuning>
    {
      public int initialProbeCount = 5;
      public int initialProbeSize = 1000;
      public int minProbeSize = 100;
      public int idealProbeSize = 300;
      public int probeSizeStep = 100;
      public float estimatedFrameTime = 1f;
      public float loadBalanceThreshold = 0.1f;
    }
  }
}
