// Decompiled with JetBrains decompiler
// Type: ValveBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ValveBase")]
public class ValveBase : KMonoBehaviour, ISaveLoadable
{
  [SerializeField]
  public ConduitType conduitType;
  [SerializeField]
  public float maxFlow = 0.5f;
  [Serialize]
  private float currentFlow;
  [MyCmpGet]
  protected KBatchedAnimController controller;
  protected HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;
  private int curFlowIdx = -1;
  private int inputCell;
  private int outputCell;
  [SerializeField]
  public ValveBase.AnimRangeInfo[] animFlowRanges;
  private SimHashes lastElementTransfered = SimHashes.Vacuum;

  public float CurrentFlow
  {
    set => this.currentFlow = value;
    get => this.currentFlow;
  }

  public HandleVector<int>.Handle AccumulatorHandle => this.flowAccumulator;

  public float MaxFlow => this.maxFlow;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.flowAccumulator = Game.Instance.accumulators.Add("Flow", (KMonoBehaviour) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.inputCell = component.GetUtilityInputCell();
    this.outputCell = component.GetUtilityOutputCell();
    Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    this.UpdateAnim();
    this.OnCmpEnable();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.flowAccumulator);
    Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
    base.OnCleanUp();
  }

  private void ConduitUpdate(float dt)
  {
    ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
    ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);
    if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
    {
      this.OnMassTransfer(0.0f);
      this.UpdateAnim();
    }
    else
    {
      ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
      float mass = Mathf.Min(contents.mass, this.currentFlow * dt);
      float num = 0.0f;
      if ((double) mass > 0.0)
      {
        int disease_count = (int) ((double) mass / (double) contents.mass * (double) contents.diseaseCount);
        num = flowManager.AddElement(this.outputCell, contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count);
        Game.Instance.accumulators.Accumulate(this.flowAccumulator, num);
        if ((double) num > 0.0)
        {
          if (this.lastElementTransfered != contents.element && contents.element != SimHashes.Vacuum && this.conduitType == ConduitType.Liquid)
          {
            Element elementByHash = ElementLoader.FindElementByHash(contents.element);
            if (elementByHash != null)
              GameUtil.TintLiquidSymbolOnBuilding("water_color", this.controller, elementByHash);
          }
          this.lastElementTransfered = contents.element;
          flowManager.RemoveElement(this.inputCell, num);
        }
      }
      this.OnMassTransfer(num);
      this.UpdateAnim();
    }
  }

  protected virtual void OnMassTransfer(float amount)
  {
  }

  public virtual void UpdateAnim()
  {
    float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
    if ((double) averageRate > 0.0)
    {
      for (int index = 0; index < this.animFlowRanges.Length; ++index)
      {
        if ((double) averageRate <= (double) this.animFlowRanges[index].minFlow)
        {
          if (this.curFlowIdx == index)
            break;
          this.curFlowIdx = index;
          this.controller.Play((HashedString) this.animFlowRanges[index].animName, (double) averageRate <= 0.0 ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop);
          break;
        }
      }
    }
    else
      this.controller.Play((HashedString) "off");
  }

  [Serializable]
  public struct AnimRangeInfo(float min_flow, string anim_name)
  {
    public float minFlow = min_flow;
    public string animName = anim_name;
  }
}
