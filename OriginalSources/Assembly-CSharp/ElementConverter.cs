// Decompiled with JetBrains decompiler
// Type: ElementConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverter : 
  StateMachineComponent<ElementConverter.StatesInstance>,
  IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpReq]
  private Storage storage;
  public bool inputIsCategory;
  public Action<float> onConvertMass;
  private float totalDiseaseWeight = float.MaxValue;
  public Operational.State OperationalRequirement = Operational.State.Active;
  private AttributeInstance machinerySpeedAttribute;
  private float workSpeedMultiplier = 1f;
  public bool spawnBubblesUnderLiquid;
  public bool showDescriptors = true;
  private const float BASE_INTERVAL = 1f;
  public ElementConverter.ConsumedElement[] consumedElements;
  public ElementConverter.OutputElement[] outputElements;
  public bool ShowInUI = true;
  private float outputMultiplier = 1f;
  private Dictionary<Tag, Guid> consumedElementStatusHandles = new Dictionary<Tag, Guid>();
  private Dictionary<SimHashes, Guid> outputElementStatusHandles = new Dictionary<SimHashes, Guid>();
  private static StatusItem ElementConverterInput;
  private static StatusItem ElementConverterOutput;

  public void SetWorkSpeedMultiplier(float speed) => this.workSpeedMultiplier = speed;

  public void SetConsumedElementActive(Tag elementId, bool active)
  {
    for (int index = 0; index < this.consumedElements.Length; ++index)
    {
      if (!(this.consumedElements[index].Tag != elementId))
      {
        this.consumedElements[index].IsActive = active;
        this.smi.UpdateStatusItems();
        break;
      }
    }
  }

  public void SetOutputElementActive(SimHashes element, bool active)
  {
    for (int index = 0; index < this.outputElements.Length; ++index)
    {
      if (this.outputElements[index].elementHash == element)
      {
        this.outputElements[index].IsActive = active;
        this.smi.UpdateStatusItems();
        break;
      }
    }
  }

  public void SetStorage(Storage storage) => this.storage = storage;

  public float OutputMultiplier
  {
    get => this.outputMultiplier;
    set => this.outputMultiplier = value;
  }

  public float AverageConvertRate
  {
    get => Game.Instance.accumulators.GetAverageRate(this.outputElements[0].accumulator);
  }

  public bool HasEnoughMass(Tag tag, bool includeInactive = false)
  {
    bool flag = false;
    List<GameObject> items = this.storage.items;
    foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
    {
      if (!(tag != consumedElement.Tag) && (includeInactive || consumedElement.IsActive))
      {
        float num = 0.0f;
        for (int index = 0; index < items.Count; ++index)
        {
          GameObject go = items[index];
          if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
            num += go.GetComponent<PrimaryElement>().Mass;
        }
        flag = (double) num >= (double) consumedElement.MassConsumptionRate;
        break;
      }
    }
    return flag;
  }

  public bool HasEnoughMassToStartConverting(bool includeInactive = false)
  {
    float num1 = 1f * this.GetSpeedMultiplier();
    bool flag1 = includeInactive || this.consumedElements.Length == 0;
    bool flag2 = true;
    List<GameObject> items = this.storage.items;
    for (int index1 = 0; index1 < this.consumedElements.Length; ++index1)
    {
      ElementConverter.ConsumedElement consumedElement = this.consumedElements[index1];
      flag1 |= consumedElement.IsActive;
      if (includeInactive || consumedElement.IsActive)
      {
        float num2 = 0.0f;
        for (int index2 = 0; index2 < items.Count; ++index2)
        {
          GameObject go = items[index2];
          if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(consumedElement.Tag))
            num2 += go.GetComponent<PrimaryElement>().Mass;
        }
        if ((double) num2 < (double) consumedElement.MassConsumptionRate * (double) num1)
        {
          flag2 = false;
          break;
        }
      }
    }
    return flag1 & flag2;
  }

  public bool CanConvertAtAll()
  {
    bool flag1 = this.consumedElements.Length == 0;
    bool flag2 = true;
    List<GameObject> items = this.storage.items;
    for (int index1 = 0; index1 < this.consumedElements.Length; ++index1)
    {
      ElementConverter.ConsumedElement consumedElement = this.consumedElements[index1];
      flag1 |= consumedElement.IsActive;
      if (consumedElement.IsActive)
      {
        bool flag3 = false;
        for (int index2 = 0; index2 < items.Count; ++index2)
        {
          GameObject go = items[index2];
          if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(consumedElement.Tag) && (double) go.GetComponent<PrimaryElement>().Mass > 0.0)
          {
            flag3 = true;
            break;
          }
        }
        if (!flag3)
        {
          flag2 = false;
          break;
        }
      }
    }
    return flag1 & flag2;
  }

  private float GetSpeedMultiplier()
  {
    return this.machinerySpeedAttribute.GetTotalValue() * this.workSpeedMultiplier;
  }

  private void ConvertMass()
  {
    float num1 = 1f * this.GetSpeedMultiplier();
    bool flag = this.consumedElements.Length == 0;
    float a1 = 1f;
    for (int index1 = 0; index1 < this.consumedElements.Length; ++index1)
    {
      ElementConverter.ConsumedElement consumedElement = this.consumedElements[index1];
      flag |= consumedElement.IsActive;
      if (consumedElement.IsActive)
      {
        float a2 = consumedElement.MassConsumptionRate * num1 * a1;
        if ((double) a2 <= 0.0)
        {
          a1 = 0.0f;
          break;
        }
        float b = 0.0f;
        for (int index2 = 0; index2 < this.storage.items.Count; ++index2)
        {
          GameObject go = this.storage.items[index2];
          if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(consumedElement.Tag))
          {
            PrimaryElement component = go.GetComponent<PrimaryElement>();
            float num2 = Mathf.Min(a2, component.Mass);
            b += num2 / a2;
          }
        }
        a1 = Mathf.Min(a1, b);
      }
    }
    if (!flag || (double) a1 <= 0.0)
    {
      this.smi.UpdateStatusItems();
    }
    else
    {
      SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid with
      {
        idx = byte.MaxValue,
        count = 0
      };
      float num3 = 0.0f;
      float num4 = 0.0f;
      float num5 = 0.0f;
      for (int index3 = 0; index3 < this.consumedElements.Length; ++index3)
      {
        ElementConverter.ConsumedElement consumedElement = this.consumedElements[index3];
        if (consumedElement.IsActive)
        {
          float num6 = consumedElement.MassConsumptionRate * num1 * a1;
          Game.Instance.accumulators.Accumulate(consumedElement.Accumulator, num6);
          for (int index4 = 0; index4 < this.storage.items.Count; ++index4)
          {
            GameObject go = this.storage.items[index4];
            if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
            {
              if (go.HasTag(consumedElement.Tag))
              {
                PrimaryElement component = go.GetComponent<PrimaryElement>();
                component.KeepZeroMassObject = true;
                float num7 = Mathf.Min(num6, component.Mass);
                int src2_count = (int) ((double) num7 / (double) component.Mass * (double) component.DiseaseCount);
                float num8 = num7 * component.Element.specificHeatCapacity;
                num5 += num8;
                num4 += num8 * component.Temperature;
                component.Mass -= num7;
                component.ModifyDiseaseCount(-src2_count, "ElementConverter.ConvertMass");
                num3 += num7;
                diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo.idx, diseaseInfo.count, component.DiseaseIdx, src2_count);
                num6 -= num7;
                if ((double) num6 <= 0.0)
                  break;
              }
              if ((double) num6 <= 0.0)
                Debug.Assert((double) num6 <= 0.0);
            }
          }
        }
      }
      float b = (double) num5 > 0.0 ? num4 / num5 : 0.0f;
      if (this.onConvertMass != null && (double) num3 > 0.0)
        this.onConvertMass(num3);
      for (int index = 0; index < this.outputElements.Length; ++index)
      {
        ElementConverter.OutputElement outputElement = this.outputElements[index];
        if (outputElement.IsActive)
        {
          SimUtil.DiseaseInfo a3 = diseaseInfo;
          if ((double) this.totalDiseaseWeight <= 0.0)
          {
            a3.idx = byte.MaxValue;
            a3.count = 0;
          }
          else
          {
            float num9 = outputElement.diseaseWeight / this.totalDiseaseWeight;
            a3.count = (int) ((double) a3.count * (double) num9);
          }
          if (outputElement.addedDiseaseIdx != byte.MaxValue)
            a3 = SimUtil.CalculateFinalDiseaseInfo(a3, new SimUtil.DiseaseInfo()
            {
              idx = outputElement.addedDiseaseIdx,
              count = outputElement.addedDiseaseCount
            });
          float num10 = outputElement.massGenerationRate * this.OutputMultiplier * num1 * a1;
          Game.Instance.accumulators.Accumulate(outputElement.accumulator, num10);
          float temperature = outputElement.useEntityTemperature || (double) b == 0.0 && (double) outputElement.minOutputTemperature == 0.0 ? this.GetComponent<PrimaryElement>().Temperature : Mathf.Max(outputElement.minOutputTemperature, b);
          Element elementByHash = ElementLoader.FindElementByHash(outputElement.elementHash);
          if (outputElement.storeOutput)
          {
            PrimaryElement primaryElement = this.storage.AddToPrimaryElement(outputElement.elementHash, num10, temperature);
            if ((UnityEngine.Object) primaryElement == (UnityEngine.Object) null)
            {
              if (elementByHash.IsGas)
                this.storage.AddGasChunk(outputElement.elementHash, num10, temperature, a3.idx, a3.count, true);
              else if (elementByHash.IsLiquid)
                this.storage.AddLiquid(outputElement.elementHash, num10, temperature, a3.idx, a3.count, true);
              else
                this.storage.Store(elementByHash.substance.SpawnResource(this.transform.GetPosition(), num10, temperature, a3.idx, a3.count, true), true);
            }
            else
              primaryElement.AddDisease(a3.idx, a3.count, "ElementConverter.ConvertMass");
          }
          else
          {
            Vector3 vector3 = new Vector3(this.transform.GetPosition().x + outputElement.outputElementOffset.x, this.transform.GetPosition().y + outputElement.outputElementOffset.y, 0.0f);
            int cell = Grid.PosToCell(vector3);
            if (elementByHash.IsLiquid)
            {
              if (this.spawnBubblesUnderLiquid && Grid.IsVisiblyInLiquid((Vector2) vector3))
                BubbleManager.instance.SpawnBubble(elementByHash.id, (Vector2) vector3, num10, temperature, new BubbleManager.Disease()
                {
                  Idx = a3.idx,
                  Count = a3.count
                });
              else
                FallingWater.instance.AddParticle(cell, elementByHash.idx, num10, temperature, a3.idx, a3.count, true);
            }
            else if (elementByHash.IsSolid)
              elementByHash.substance.SpawnResource(vector3, num10, temperature, a3.idx, a3.count);
            else if (this.spawnBubblesUnderLiquid && Grid.IsVisiblyInLiquid((Vector2) vector3))
              BubbleManager.instance.SpawnBubble(elementByHash.id, (Vector2) vector3, num10, temperature, new BubbleManager.Disease()
              {
                Idx = a3.idx,
                Count = a3.count
              });
            else
              SimMessages.AddRemoveSubstance(cell, outputElement.elementHash, CellEventLogger.Instance.OxygenModifierSimUpdate, num10, temperature, a3.idx, a3.count);
          }
          if (outputElement.elementHash == SimHashes.Oxygen || outputElement.elementHash == SimHashes.ContaminatedOxygen)
            ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num10, this.gameObject.GetProperName());
        }
      }
      this.smi.UpdateStatusItems();
      this.storage.Trigger(-1697596308, (object) this.gameObject);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.machinerySpeedAttribute = this.gameObject.GetAttributes().Add(Db.Get().Attributes.MachinerySpeed);
    if (ElementConverter.ElementConverterInput == null)
      ElementConverter.ElementConverterInput = new StatusItem("ElementConverterInput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID).SetResolveStringCallback((Func<string, object, string>) ((str, data) =>
      {
        ElementConverter.ConsumedElement consumedElement = (ElementConverter.ConsumedElement) data;
        str = str.Replace("{ElementTypes}", consumedElement.Name);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedByTag(consumedElement.Tag, consumedElement.Rate, GameUtil.TimeSlice.PerSecond));
        return str;
      }));
    if (ElementConverter.ElementConverterOutput != null)
      return;
    ElementConverter.ElementConverterOutput = new StatusItem("ElementConverterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID).SetResolveStringCallback((Func<string, object, string>) ((str, data) =>
    {
      ElementConverter.OutputElement outputElement = (ElementConverter.OutputElement) data;
      str = str.Replace("{ElementTypes}", outputElement.Name);
      str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(outputElement.Rate, GameUtil.TimeSlice.PerSecond));
      return str;
    }));
  }

  public void SetAllConsumedActive(bool active)
  {
    for (int index = 0; index < this.consumedElements.Length; ++index)
      this.consumedElements[index].IsActive = active;
    this.smi.sm.canConvert.Set(active, this.smi);
  }

  public void SetConsumedActive(Tag id, bool active)
  {
    bool flag = this.consumedElements.Length == 0;
    for (int index = 0; index < this.consumedElements.Length; ++index)
    {
      ref ElementConverter.ConsumedElement local = ref this.consumedElements[index];
      if (local.Tag == id)
      {
        local.IsActive = active;
        if (active)
        {
          flag = true;
          break;
        }
      }
      flag |= local.IsActive;
    }
    this.smi.sm.canConvert.Set(flag, this.smi);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.consumedElements.Length; ++index)
      this.consumedElements[index].Accumulator = Game.Instance.accumulators.Add("ElementsConsumed", (KMonoBehaviour) this);
    this.totalDiseaseWeight = 0.0f;
    for (int index = 0; index < this.outputElements.Length; ++index)
    {
      this.outputElements[index].accumulator = Game.Instance.accumulators.Add("OutputElements", (KMonoBehaviour) this);
      this.totalDiseaseWeight += this.outputElements[index].diseaseWeight;
    }
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    for (int index = 0; index < this.consumedElements.Length; ++index)
      Game.Instance.accumulators.Remove(this.consumedElements[index].Accumulator);
    for (int index = 0; index < this.outputElements.Length; ++index)
      Game.Instance.accumulators.Remove(this.outputElements[index].accumulator);
    base.OnCleanUp();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    if (!this.showDescriptors)
      return descriptors;
    if (this.consumedElements != null)
    {
      foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
      {
        if (consumedElement.IsActive)
        {
          Descriptor descriptor = new Descriptor();
          descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMED, (object) consumedElement.Name, (object) GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, (object) consumedElement.Name, (object) GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}")), Descriptor.DescriptorType.Requirement);
          descriptors.Add(descriptor);
        }
      }
    }
    if (this.outputElements != null)
    {
      foreach (ElementConverter.OutputElement outputElement in this.outputElements)
      {
        if (outputElement.IsActive)
        {
          LocString format1 = UI.BUILDINGEFFECTS.ELEMENTEMITTED_INPUTTEMP;
          LocString format2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_INPUTTEMP;
          if (outputElement.useEntityTemperature)
          {
            format1 = UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP;
            format2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP;
          }
          else if ((double) outputElement.minOutputTemperature > 0.0)
          {
            format1 = UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINTEMP;
            format2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINTEMP;
          }
          Descriptor descriptor = new Descriptor(string.Format((string) format1, (object) outputElement.Name, (object) GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature)), string.Format((string) format2, (object) outputElement.Name, (object) GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature)));
          descriptors.Add(descriptor);
        }
      }
    }
    return descriptors;
  }

  [DebuggerDisplay("{tag} {massConsumptionRate}")]
  [Serializable]
  public struct ConsumedElement(Tag tag, float kgPerSecond, bool isActive = true)
  {
    public Tag Tag = tag;
    public float MassConsumptionRate = kgPerSecond;
    public bool IsActive = isActive;
    public HandleVector<int>.Handle Accumulator = HandleVector<int>.InvalidHandle;

    public string Name => this.Tag.ProperName();

    public float Rate => Game.Instance.accumulators.GetAverageRate(this.Accumulator);
  }

  [Serializable]
  public struct OutputElement(
    float kgPerSecond,
    SimHashes element,
    float minOutputTemperature,
    bool useEntityTemperature = false,
    bool storeOutput = false,
    float outputElementOffsetx = 0.0f,
    float outputElementOffsety = 0.5f,
    float diseaseWeight = 1f,
    byte addedDiseaseIdx = 255 /*0xFF*/,
    int addedDiseaseCount = 0,
    bool isActive = true)
  {
    public bool IsActive = isActive;
    public SimHashes elementHash = element;
    public float minOutputTemperature = minOutputTemperature;
    public bool useEntityTemperature = useEntityTemperature;
    public float massGenerationRate = kgPerSecond;
    public bool storeOutput = storeOutput;
    public Vector2 outputElementOffset = new Vector2(outputElementOffsetx, outputElementOffsety);
    public HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
    public float diseaseWeight = diseaseWeight;
    public byte addedDiseaseIdx = addedDiseaseIdx;
    public int addedDiseaseCount = addedDiseaseCount;

    public string Name => ElementLoader.FindElementByHash(this.elementHash).tag.ProperName();

    public float Rate => Game.Instance.accumulators.GetAverageRate(this.accumulator);
  }

  public class StatesInstance : 
    GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.GameInstance
  {
    private KSelectable selectable;
    public int subscribedHandle;

    public StatesInstance(ElementConverter smi)
      : base(smi)
    {
      this.selectable = this.GetComponent<KSelectable>();
    }

    public void AddStatusItems() => this.UpdateStatusItems();

    public void RemoveStatusItems()
    {
      if (!this.master.ShowInUI)
        return;
      for (int index = 0; index < this.master.consumedElements.Length; ++index)
        this.RemoveStatusItem<Tag>(this.master.consumedElements[index].Tag, this.master.consumedElementStatusHandles);
      for (int index = 0; index < this.master.outputElements.Length; ++index)
        this.RemoveStatusItem<SimHashes>(this.master.outputElements[index].elementHash, this.master.outputElementStatusHandles);
      this.master.consumedElementStatusHandles.Clear();
      this.master.outputElementStatusHandles.Clear();
    }

    public void UpdateStatusItems()
    {
      if (!this.master.ShowInUI)
        return;
      for (int index = 0; index < this.master.consumedElements.Length; ++index)
      {
        ElementConverter.ConsumedElement consumedElement = this.master.consumedElements[index];
        if (consumedElement.IsActive)
        {
          bool flag1 = (double) consumedElement.Rate > 0.0;
          bool flag2 = this.master.consumedElementStatusHandles.ContainsKey(consumedElement.Tag);
          if (flag1 && !flag2)
            this.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, this.master.consumedElementStatusHandles);
          else if (!flag1 & flag2)
          {
            this.RemoveStatusItem<Tag>(consumedElement.Tag, this.master.consumedElementStatusHandles);
            this.master.consumedElementStatusHandles.Remove(consumedElement.Tag);
          }
        }
      }
      for (int index = 0; index < this.master.outputElements.Length; ++index)
      {
        ElementConverter.OutputElement outputElement = this.master.outputElements[index];
        if (outputElement.IsActive)
        {
          bool flag3 = (double) outputElement.Rate > 0.0;
          bool flag4 = this.master.outputElementStatusHandles.ContainsKey(outputElement.elementHash);
          if (flag3 && !flag4)
            this.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, this.master.outputElementStatusHandles);
          else if (!flag3 & flag4)
          {
            this.RemoveStatusItem<SimHashes>(outputElement.elementHash, this.master.outputElementStatusHandles);
            this.master.outputElementStatusHandles.Remove(outputElement.elementHash);
          }
        }
      }
    }

    public void AddStatusItem<ElementType, IDType>(
      ElementType element,
      IDType id,
      StatusItem status,
      Dictionary<IDType, Guid> collection)
    {
      Guid guid = this.selectable.AddStatusItem(status, (object) element);
      collection[id] = guid;
    }

    public void RemoveStatusItem<IDType>(IDType id, Dictionary<IDType, Guid> collection)
    {
      Guid guid;
      if (!collection.TryGetValue(id, out guid))
        return;
      this.selectable.RemoveStatusItem(guid);
    }

    public void OnOperationalRequirementChanged(object data)
    {
      Operational operational = data as Operational;
      this.sm.canConvert.Set((UnityEngine.Object) operational == (UnityEngine.Object) null ? ((Boxed<bool>) data).value : operational.IsActive, this);
    }
  }

  public class States : 
    GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter>
  {
    public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State disabled;
    public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State converting;
    public StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.BoolParameter canConvert;

    private bool ValidateStateTransition(ElementConverter.StatesInstance smi, bool _)
    {
      bool flag1 = smi.GetCurrentState() == smi.sm.disabled;
      if ((UnityEngine.Object) smi.master.operational == (UnityEngine.Object) null)
        return flag1;
      bool flag2 = smi.master.consumedElements.Length == 0;
      bool flag3 = this.canConvert.Get(smi);
      for (int index = 0; !flag2 && index < smi.master.consumedElements.Length; ++index)
        flag2 = smi.master.consumedElements[index].IsActive;
      if (!flag3 || flag2)
        return smi.master.operational.MeetsRequirements(smi.master.OperationalRequirement) == flag1;
      this.canConvert.Set(false, smi, true);
      return false;
    }

    private void OnEnterRoot(ElementConverter.StatesInstance smi)
    {
      int eventForState = (int) Operational.GetEventForState(smi.master.OperationalRequirement);
      smi.Unsubscribe(ref smi.subscribedHandle);
      smi.subscribedHandle = smi.Subscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
    }

    private void OnExitRoot(ElementConverter.StatesInstance smi)
    {
      smi.Unsubscribe(ref smi.subscribedHandle);
    }

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.Enter(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnEnterRoot)).Exit(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnExitRoot));
      this.disabled.ParamTransition<bool>((StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>) this.canConvert, this.converting, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition));
      this.converting.Enter("AddStatusItems", (StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback) (smi => smi.AddStatusItems())).Exit("RemoveStatusItems", (StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback) (smi => smi.RemoveStatusItems())).ParamTransition<bool>((StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>) this.canConvert, this.disabled, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition)).Update("ConvertMass", (Action<ElementConverter.StatesInstance, float>) ((smi, dt) => smi.master.ConvertMass()), UpdateRate.SIM_1000ms, true);
    }
  }
}
