// Decompiled with JetBrains decompiler
// Type: CircuitManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CircuitManager
{
  public const ushort INVALID_ID = 65535 /*0xFFFF*/;
  private const int SimUpdateSortKey = 1000;
  private const float MIN_POWERED_THRESHOLD = 0.01f;
  private bool dirty = true;
  private HashSet<Generator> generators = new HashSet<Generator>();
  private HashSet<IEnergyConsumer> consumers = new HashSet<IEnergyConsumer>();
  private HashSet<WireUtilityNetworkLink> bridges = new HashSet<WireUtilityNetworkLink>();
  private float elapsedTime;
  private List<CircuitManager.CircuitInfo> circuitInfo = new List<CircuitManager.CircuitInfo>();
  private List<IEnergyConsumer> consumersShadow = new List<IEnergyConsumer>();
  private List<Generator> activeGenerators = new List<Generator>();

  public void Connect(Generator generator)
  {
    if (Game.IsQuitting())
      return;
    this.generators.Add(generator);
    this.dirty = true;
  }

  public void Disconnect(Generator generator)
  {
    if (Game.IsQuitting())
      return;
    this.generators.Remove(generator);
    this.dirty = true;
  }

  public void Connect(IEnergyConsumer consumer)
  {
    if (Game.IsQuitting())
      return;
    this.consumers.Add(consumer);
    this.dirty = true;
  }

  public void Disconnect(IEnergyConsumer consumer, bool isDestroy)
  {
    if (Game.IsQuitting())
      return;
    this.consumers.Remove(consumer);
    if (!isDestroy)
      consumer.SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
    this.dirty = true;
  }

  public void Connect(WireUtilityNetworkLink bridge)
  {
    this.bridges.Add(bridge);
    this.dirty = true;
  }

  public void Disconnect(WireUtilityNetworkLink bridge)
  {
    this.bridges.Remove(bridge);
    this.dirty = true;
  }

  public float GetPowerDraw(ushort circuitID, Generator generator)
  {
    double powerDraw = 0.0;
    if ((int) circuitID >= this.circuitInfo.Count)
      return (float) powerDraw;
    CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[(int) circuitID];
    this.circuitInfo[(int) circuitID] = circuitInfo;
    this.circuitInfo[(int) circuitID] = circuitInfo;
    return (float) powerDraw;
  }

  public ushort GetCircuitID(int cell)
  {
    UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
    return networkForCell == null ? ushort.MaxValue : (ushort) networkForCell.id;
  }

  public ushort GetVirtualCircuitID(object virtualKey)
  {
    UtilityNetwork networkForVirtualKey = Game.Instance.electricalConduitSystem.GetNetworkForVirtualKey(virtualKey);
    return networkForVirtualKey == null ? ushort.MaxValue : (ushort) networkForVirtualKey.id;
  }

  public ushort GetCircuitID(ICircuitConnected ent)
  {
    return !ent.IsVirtual ? this.GetCircuitID(ent.PowerCell) : this.GetVirtualCircuitID(ent.VirtualCircuitKey);
  }

  public void Sim200msFirst(float dt) => this.Refresh(dt);

  public void RenderEveryTick(float dt) => this.Refresh(dt);

  private void Refresh(float dt)
  {
    UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem = Game.Instance.electricalConduitSystem;
    if (!electricalConduitSystem.IsDirty && !this.dirty)
      return;
    electricalConduitSystem.Update();
    IList<UtilityNetwork> networks = electricalConduitSystem.GetNetworks();
    while (this.circuitInfo.Count < networks.Count)
    {
      CircuitManager.CircuitInfo circuitInfo = new CircuitManager.CircuitInfo()
      {
        generators = new List<Generator>(),
        consumers = new List<IEnergyConsumer>(),
        batteries = new List<Battery>(),
        inputTransformers = new List<Battery>(),
        outputTransformers = new List<Generator>()
      } with
      {
        bridgeGroups = new List<WireUtilityNetworkLink>[6]
      };
      for (int index = 0; index < circuitInfo.bridgeGroups.Length; ++index)
        circuitInfo.bridgeGroups[index] = new List<WireUtilityNetworkLink>();
      this.circuitInfo.Add(circuitInfo);
    }
    this.Rebuild();
  }

  public void Rebuild()
  {
    for (int index1 = 0; index1 < this.circuitInfo.Count; ++index1)
    {
      CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[index1];
      circuitInfo.generators.Clear();
      circuitInfo.consumers.Clear();
      circuitInfo.batteries.Clear();
      circuitInfo.inputTransformers.Clear();
      circuitInfo.outputTransformers.Clear();
      circuitInfo.minBatteryPercentFull = 1f;
      for (int index2 = 0; index2 < circuitInfo.bridgeGroups.Length; ++index2)
        circuitInfo.bridgeGroups[index2].Clear();
      circuitInfo.wattsUsed = -1f;
      this.circuitInfo[index1] = circuitInfo;
    }
    this.consumersShadow.AddRange((IEnumerable<IEnergyConsumer>) this.consumers);
    List<IEnergyConsumer>.Enumerator enumerator1 = this.consumersShadow.GetEnumerator();
    while (enumerator1.MoveNext())
    {
      IEnergyConsumer current = enumerator1.Current;
      ushort circuitId = this.GetCircuitID((ICircuitConnected) current);
      if (circuitId != ushort.MaxValue)
      {
        Battery battery = current as Battery;
        if ((UnityEngine.Object) battery != (UnityEngine.Object) null)
        {
          CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[(int) circuitId];
          if ((UnityEngine.Object) battery.powerTransformer != (UnityEngine.Object) null)
          {
            circuitInfo.inputTransformers.Add(battery);
          }
          else
          {
            circuitInfo.batteries.Add(battery);
            circuitInfo.minBatteryPercentFull = Mathf.Min(this.circuitInfo[(int) circuitId].minBatteryPercentFull, battery.PercentFull);
          }
          this.circuitInfo[(int) circuitId] = circuitInfo;
        }
        else
          this.circuitInfo[(int) circuitId].consumers.Add(current);
      }
    }
    this.consumersShadow.Clear();
    for (int index = 0; index < this.circuitInfo.Count; ++index)
      this.circuitInfo[index].consumers.Sort((Comparison<IEnergyConsumer>) ((a, b) => a.WattsNeededWhenActive.CompareTo(b.WattsNeededWhenActive)));
    HashSet<Generator>.Enumerator enumerator2 = this.generators.GetEnumerator();
    while (enumerator2.MoveNext())
    {
      Generator current = enumerator2.Current;
      ushort circuitId = this.GetCircuitID((ICircuitConnected) current);
      if (circuitId != ushort.MaxValue)
      {
        if (current.GetType() == typeof (PowerTransformer))
          this.circuitInfo[(int) circuitId].outputTransformers.Add(current);
        else
          this.circuitInfo[(int) circuitId].generators.Add(current);
      }
    }
    HashSet<WireUtilityNetworkLink>.Enumerator enumerator3 = this.bridges.GetEnumerator();
    while (enumerator3.MoveNext())
    {
      WireUtilityNetworkLink current = enumerator3.Current;
      ushort circuitId = this.GetCircuitID((ICircuitConnected) current);
      if (circuitId != ushort.MaxValue)
      {
        Wire.WattageRating maxWattageRating = current.GetMaxWattageRating();
        this.circuitInfo[(int) circuitId].bridgeGroups[(int) maxWattageRating].Add(current);
      }
    }
    this.dirty = false;
  }

  private float GetBatteryJoulesAvailable(List<Battery> batteries, out int num_powered)
  {
    float batteryJoulesAvailable = 0.0f;
    num_powered = 0;
    for (int index = 0; index < batteries.Count; ++index)
    {
      if ((double) batteries[index].JoulesAvailable > 0.0)
      {
        batteryJoulesAvailable = batteries[index].JoulesAvailable;
        num_powered = batteries.Count - index;
        break;
      }
    }
    return batteryJoulesAvailable;
  }

  public void Sim200msLast(float dt)
  {
    this.elapsedTime += dt;
    if ((double) this.elapsedTime < 0.20000000298023224)
      return;
    this.elapsedTime -= 0.2f;
    for (int index1 = 0; index1 < this.circuitInfo.Count; ++index1)
    {
      CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[index1] with
      {
        wattsUsed = 0.0f
      };
      this.activeGenerators.Clear();
      List<Generator> generators = circuitInfo.generators;
      List<IEnergyConsumer> consumers = circuitInfo.consumers;
      List<Battery> batteries = circuitInfo.batteries;
      List<Generator> outputTransformers = circuitInfo.outputTransformers;
      batteries.Sort((Comparison<Battery>) ((a, b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable)));
      bool flag1 = false;
      bool flag2 = generators.Count > 0;
      for (int index2 = 0; index2 < generators.Count; ++index2)
      {
        Generator generator = generators[index2];
        if ((double) generator.JoulesAvailable > 0.0)
        {
          flag1 = true;
          this.activeGenerators.Add(generator);
        }
      }
      this.activeGenerators.Sort((Comparison<Generator>) ((a, b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable)));
      if (!flag1)
      {
        for (int index3 = 0; index3 < outputTransformers.Count; ++index3)
        {
          if ((double) outputTransformers[index3].JoulesAvailable > 0.0)
            flag1 = true;
        }
      }
      float a1 = 1f;
      for (int index4 = 0; index4 < batteries.Count; ++index4)
      {
        Battery battery = batteries[index4];
        if ((double) battery.JoulesAvailable > 0.0)
          flag1 = true;
        a1 = Mathf.Min(a1, battery.PercentFull);
      }
      for (int index5 = 0; index5 < circuitInfo.inputTransformers.Count; ++index5)
      {
        Battery inputTransformer = circuitInfo.inputTransformers[index5];
        a1 = Mathf.Min(a1, inputTransformer.PercentFull);
      }
      circuitInfo.minBatteryPercentFull = a1;
      if (flag1)
      {
        for (int index6 = 0; index6 < consumers.Count; ++index6)
        {
          IEnergyConsumer c = consumers[index6];
          float joules_needed = c.WattsUsed * 0.2f;
          if ((double) joules_needed > 0.0)
          {
            bool flag3 = false;
            for (int index7 = 0; index7 < this.activeGenerators.Count; ++index7)
            {
              Generator activeGenerator = this.activeGenerators[index7];
              joules_needed = this.PowerFromGenerator(joules_needed, activeGenerator, c);
              if ((double) joules_needed <= 0.0)
              {
                flag3 = true;
                break;
              }
            }
            if (!flag3)
            {
              for (int index8 = 0; index8 < outputTransformers.Count; ++index8)
              {
                Generator g = outputTransformers[index8];
                joules_needed = this.PowerFromGenerator(joules_needed, g, c);
                if ((double) joules_needed <= 0.0)
                {
                  flag3 = true;
                  break;
                }
              }
            }
            if (!flag3)
            {
              joules_needed = this.PowerFromBatteries(joules_needed, batteries, c);
              flag3 = (double) joules_needed <= 0.0099999997764825821;
            }
            if (flag3)
              circuitInfo.wattsUsed += c.WattsUsed;
            else
              circuitInfo.wattsUsed += c.WattsUsed - joules_needed / 0.2f;
            c.SetConnectionStatus(flag3 ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
          }
          else
            c.SetConnectionStatus(flag1 ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
        }
      }
      else if (flag2)
      {
        for (int index9 = 0; index9 < consumers.Count; ++index9)
          consumers[index9].SetConnectionStatus(CircuitManager.ConnectionStatus.Unpowered);
      }
      else
      {
        for (int index10 = 0; index10 < consumers.Count; ++index10)
          consumers[index10].SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
      }
      this.circuitInfo[index1] = circuitInfo;
    }
    for (int index11 = 0; index11 < this.circuitInfo.Count; ++index11)
    {
      CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[index11];
      circuitInfo.batteries.Sort((Comparison<Battery>) ((a, b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable)));
      circuitInfo.inputTransformers.Sort((Comparison<Battery>) ((a, b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable)));
      circuitInfo.generators.Sort((Comparison<Generator>) ((a, b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable)));
      float joules_used1 = 0.0f;
      this.ChargeTransformers<Generator>(circuitInfo.inputTransformers, circuitInfo.generators, ref joules_used1);
      this.ChargeTransformers<Generator>(circuitInfo.inputTransformers, circuitInfo.outputTransformers, ref joules_used1);
      float joules_used2 = 0.0f;
      this.ChargeBatteries(circuitInfo.batteries, circuitInfo.generators, ref joules_used2);
      this.ChargeBatteries(circuitInfo.batteries, circuitInfo.outputTransformers, ref joules_used2);
      circuitInfo.minBatteryPercentFull = 1f;
      for (int index12 = 0; index12 < circuitInfo.batteries.Count; ++index12)
      {
        float percentFull = circuitInfo.batteries[index12].PercentFull;
        if ((double) percentFull < (double) circuitInfo.minBatteryPercentFull)
          circuitInfo.minBatteryPercentFull = percentFull;
      }
      for (int index13 = 0; index13 < circuitInfo.inputTransformers.Count; ++index13)
      {
        float percentFull = circuitInfo.inputTransformers[index13].PercentFull;
        if ((double) percentFull < (double) circuitInfo.minBatteryPercentFull)
          circuitInfo.minBatteryPercentFull = percentFull;
      }
      circuitInfo.wattsUsed += joules_used1 / 0.2f;
      this.circuitInfo[index11] = circuitInfo;
    }
    for (int index = 0; index < this.circuitInfo.Count; ++index)
    {
      CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[index];
      circuitInfo.batteries.Sort((Comparison<Battery>) ((a, b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable)));
      float joules_used = 0.0f;
      this.ChargeTransformers<Battery>(circuitInfo.inputTransformers, circuitInfo.batteries, ref joules_used);
      circuitInfo.wattsUsed += joules_used / 0.2f;
      this.circuitInfo[index] = circuitInfo;
    }
    for (int index = 0; index < this.circuitInfo.Count; ++index)
    {
      CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[index];
      bool is_connected_to_something_useful1 = circuitInfo.generators.Count + circuitInfo.consumers.Count + circuitInfo.outputTransformers.Count > 0;
      this.UpdateBatteryConnectionStatus(circuitInfo.batteries, is_connected_to_something_useful1, index);
      bool is_connected_to_something_useful2 = circuitInfo.generators.Count > 0 || circuitInfo.outputTransformers.Count > 0;
      if (!is_connected_to_something_useful2)
      {
        foreach (Battery battery in circuitInfo.batteries)
        {
          if ((double) battery.JoulesAvailable > 0.0)
          {
            is_connected_to_something_useful2 = true;
            break;
          }
        }
      }
      this.UpdateBatteryConnectionStatus(circuitInfo.inputTransformers, is_connected_to_something_useful2, index);
      this.circuitInfo[index] = circuitInfo;
    }
    for (int index = 0; index < this.circuitInfo.Count; ++index)
    {
      CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[index];
      this.CheckCircuitOverloaded(0.2f, index, circuitInfo.wattsUsed);
    }
  }

  private float PowerFromBatteries(float joules_needed, List<Battery> batteries, IEnergyConsumer c)
  {
    int num_powered;
    do
    {
      float num1 = this.GetBatteryJoulesAvailable(batteries, out num_powered) * (float) num_powered;
      float num2 = (double) num1 < (double) joules_needed ? num1 : joules_needed;
      joules_needed -= num2;
      ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num2, c.Name);
      float joules = num2 / (float) num_powered;
      for (int index = batteries.Count - num_powered; index < batteries.Count; ++index)
        batteries[index].ConsumeEnergy(joules);
    }
    while ((double) joules_needed >= 0.0099999997764825821 && num_powered > 0);
    return joules_needed;
  }

  private float PowerFromGenerator(float joules_needed, Generator g, IEnergyConsumer c)
  {
    float num = Mathf.Min(g.JoulesAvailable, joules_needed);
    joules_needed -= num;
    g.ApplyDeltaJoules(-num);
    ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num, c.Name);
    return joules_needed;
  }

  private void ChargeBatteries(
    List<Battery> sink_batteries,
    List<Generator> source_generators,
    ref float joules_used)
  {
    if (sink_batteries.Count == 0)
      return;
    foreach (Generator sourceGenerator in source_generators)
    {
      bool flag = true;
      while (true)
      {
        if (flag && (double) sourceGenerator.JoulesAvailable >= 1.0)
          flag = this.ChargeBatteriesFromGenerator(sink_batteries, sourceGenerator, ref joules_used);
        else
          goto label_7;
      }
label_7:;
    }
  }

  private bool ChargeBatteriesFromGenerator(
    List<Battery> sink_batteries,
    Generator source_generator,
    ref float joules_used)
  {
    float joulesAvailable = source_generator.JoulesAvailable;
    float num = 0.0f;
    for (int index = 0; index < sink_batteries.Count; ++index)
    {
      Battery sinkBattery = sink_batteries[index];
      if ((UnityEngine.Object) sinkBattery != (UnityEngine.Object) null && (UnityEngine.Object) source_generator != (UnityEngine.Object) null && (UnityEngine.Object) sinkBattery.gameObject != (UnityEngine.Object) source_generator.gameObject)
      {
        float a = sinkBattery.Capacity - sinkBattery.JoulesAvailable;
        if ((double) a > 0.0)
        {
          float joules = Mathf.Min(a, joulesAvailable / (float) (sink_batteries.Count - index));
          sinkBattery.AddEnergy(joules);
          joulesAvailable -= joules;
          num += joules;
        }
      }
    }
    if ((double) num <= 0.0)
      return false;
    source_generator.ApplyDeltaJoules(-num);
    joules_used += num;
    return true;
  }

  private void UpdateBatteryConnectionStatus(
    List<Battery> batteries,
    bool is_connected_to_something_useful,
    int circuit_id)
  {
    foreach (Battery battery in batteries)
    {
      if (!((UnityEngine.Object) battery == (UnityEngine.Object) null))
      {
        if ((UnityEngine.Object) battery.powerTransformer == (UnityEngine.Object) null)
          battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.NotConnected);
        else if ((int) this.GetCircuitID((ICircuitConnected) battery) == circuit_id)
          battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
      }
    }
  }

  private void ChargeTransformer<T>(
    Battery sink_transformer,
    List<T> source_energy_producers,
    ref float joules_used)
    where T : IEnergyProducer
  {
    if (source_energy_producers.Count <= 0)
      return;
    float num1 = Mathf.Min(sink_transformer.Capacity - sink_transformer.JoulesAvailable, sink_transformer.ChargeCapacity);
    if ((double) num1 <= 0.0)
      return;
    float num2 = num1;
    float joules1 = 0.0f;
    for (int index = 0; index < source_energy_producers.Count; ++index)
    {
      T sourceEnergyProducer = source_energy_producers[index];
      if ((double) sourceEnergyProducer.JoulesAvailable > 0.0)
      {
        float joules2 = Mathf.Min(sourceEnergyProducer.JoulesAvailable, num2 / (float) (source_energy_producers.Count - index));
        sourceEnergyProducer.ConsumeEnergy(joules2);
        num2 -= joules2;
        joules1 += joules2;
      }
    }
    sink_transformer.AddEnergy(joules1);
    joules_used += joules1;
  }

  private void ChargeTransformers<T>(
    List<Battery> sink_transformers,
    List<T> source_energy_producers,
    ref float joules_used)
    where T : IEnergyProducer
  {
    foreach (Battery sinkTransformer in sink_transformers)
      this.ChargeTransformer<T>(sinkTransformer, source_energy_producers, ref joules_used);
  }

  private void CheckCircuitOverloaded(float dt, int id, float watts_used)
  {
    UtilityNetwork networkById = Game.Instance.electricalConduitSystem.GetNetworkByID(id);
    if (networkById == null)
      return;
    ((ElectricalUtilityNetwork) networkById)?.UpdateOverloadTime(dt, watts_used, this.circuitInfo[id].bridgeGroups);
  }

  public float GetWattsUsedByCircuit(ushort circuitID)
  {
    return circuitID == ushort.MaxValue ? -1f : this.circuitInfo[(int) circuitID].wattsUsed;
  }

  public float GetWattsNeededWhenActive(ushort originCircuitId)
  {
    if (originCircuitId == ushort.MaxValue)
      return -1f;
    HashSet<ushort> ushortSet1 = new HashSet<ushort>();
    HashSet<ushort> ushortSet2 = new HashSet<ushort>();
    HashSet<ushort> ushortSet3 = new HashSet<ushort>();
    ushortSet2.Add(originCircuitId);
    int num1 = 0;
    while (ushortSet2.Count > 0)
    {
      ++num1;
      if (num1 <= 100)
      {
        foreach (ushort index in ushortSet2)
        {
          if (index >= (ushort) 0 && (int) index < this.circuitInfo.Count)
          {
            foreach (Battery inputTransformer in this.circuitInfo[(int) index].inputTransformers)
            {
              ushort circuitId = inputTransformer.powerTransformer.CircuitID;
              if (inputTransformer.powerTransformer.CircuitID != ushort.MaxValue)
                ushortSet3.Add(circuitId);
            }
            ushortSet1.Add(index);
          }
        }
        ushortSet2.Clear();
        foreach (ushort num2 in ushortSet3)
        {
          if (!ushortSet1.Contains(num2))
            ushortSet2.Add(num2);
        }
        ushortSet3.Clear();
      }
      else
        break;
    }
    HashSet<ushort> ushortSet4 = ushortSet1;
    Dictionary<ushort, float> dictionary1 = new Dictionary<ushort, float>();
    foreach (ushort num3 in ushortSet4)
    {
      if (num3 >= (ushort) 0 && (int) num3 < this.circuitInfo.Count)
      {
        float num4 = 0.0f;
        foreach (IEnergyConsumer consumer in this.circuitInfo[(int) num3].consumers)
          num4 += consumer.WattsNeededWhenActive;
        dictionary1.Add(num3, num4);
      }
    }
    Dictionary<ushort, float> dictionary2 = new Dictionary<ushort, float>();
    foreach (Battery inputTransformer in this.circuitInfo[(int) originCircuitId].inputTransformers)
    {
      float b1;
      dictionary1.TryGetValue(inputTransformer.powerTransformer.CircuitID, out b1);
      float b2 = Mathf.Min(inputTransformer.powerTransformer.WattageRating, b1);
      float a;
      dictionary2.TryGetValue(inputTransformer.powerTransformer.CircuitID, out a);
      dictionary2[inputTransformer.powerTransformer.CircuitID] = Mathf.Max(a, b2);
    }
    float neededWhenActive;
    dictionary1.TryGetValue(originCircuitId, out neededWhenActive);
    foreach (KeyValuePair<ushort, float> keyValuePair in dictionary2)
    {
      ushort num5;
      float num6;
      keyValuePair.Deconstruct(ref num5, ref num6);
      float num7 = num6;
      neededWhenActive += num7;
    }
    return neededWhenActive;
  }

  public float GetWattsGeneratedByCircuit(ushort circuitID)
  {
    if (circuitID == ushort.MaxValue)
      return -1f;
    float generatedByCircuit = 0.0f;
    foreach (Generator generator in this.circuitInfo[(int) circuitID].generators)
    {
      if (!((UnityEngine.Object) generator == (UnityEngine.Object) null) && generator.IsProducingPower())
        generatedByCircuit += generator.WattageRating;
    }
    return generatedByCircuit;
  }

  public float GetPotentialWattsGeneratedByCircuit(ushort circuitID)
  {
    if (circuitID == ushort.MaxValue)
      return -1f;
    float generatedByCircuit = 0.0f;
    foreach (Generator generator in this.circuitInfo[(int) circuitID].generators)
      generatedByCircuit += generator.WattageRating;
    return generatedByCircuit;
  }

  public float GetJoulesAvailableOnCircuit(ushort circuitID)
  {
    int num_powered;
    return this.GetBatteryJoulesAvailable(this.GetBatteriesOnCircuit(circuitID), out num_powered) * (float) num_powered;
  }

  public List<Generator> GetGeneratorsOnCircuit(ushort circuitID)
  {
    return circuitID == ushort.MaxValue ? (List<Generator>) null : this.circuitInfo[(int) circuitID].generators;
  }

  public List<IEnergyConsumer> GetConsumersOnCircuit(ushort circuitID)
  {
    return circuitID == ushort.MaxValue ? (List<IEnergyConsumer>) null : this.circuitInfo[(int) circuitID].consumers;
  }

  public List<Battery> GetTransformersOnCircuit(ushort circuitID)
  {
    return circuitID == ushort.MaxValue ? (List<Battery>) null : this.circuitInfo[(int) circuitID].inputTransformers;
  }

  public List<Battery> GetBatteriesOnCircuit(ushort circuitID)
  {
    return circuitID == ushort.MaxValue ? (List<Battery>) null : this.circuitInfo[(int) circuitID].batteries;
  }

  public float GetMinBatteryPercentFullOnCircuit(ushort circuitID)
  {
    return circuitID == ushort.MaxValue ? 0.0f : this.circuitInfo[(int) circuitID].minBatteryPercentFull;
  }

  public bool HasBatteries(ushort circuitID)
  {
    return circuitID != ushort.MaxValue && this.circuitInfo[(int) circuitID].batteries.Count + this.circuitInfo[(int) circuitID].inputTransformers.Count > 0;
  }

  public bool HasGenerators(ushort circuitID)
  {
    return circuitID != ushort.MaxValue && this.circuitInfo[(int) circuitID].generators.Count + this.circuitInfo[(int) circuitID].outputTransformers.Count > 0;
  }

  public bool HasGenerators() => this.generators.Count > 0;

  public bool HasConsumers(ushort circuitID)
  {
    return circuitID != ushort.MaxValue && this.circuitInfo[(int) circuitID].consumers.Count > 0;
  }

  public float GetMaxSafeWattageForCircuit(ushort circuitID)
  {
    return circuitID == ushort.MaxValue || !(Game.Instance.electricalConduitSystem.GetNetworkByID((int) circuitID) is ElectricalUtilityNetwork networkById) ? 0.0f : networkById.GetMaxSafeWattage(this.circuitInfo[(int) circuitID].bridgeGroups);
  }

  private struct CircuitInfo
  {
    public List<Generator> generators;
    public List<IEnergyConsumer> consumers;
    public List<Battery> batteries;
    public List<Battery> inputTransformers;
    public List<Generator> outputTransformers;
    public List<WireUtilityNetworkLink>[] bridgeGroups;
    public float minBatteryPercentFull;
    public float wattsUsed;
  }

  public enum ConnectionStatus
  {
    NotConnected,
    Unpowered,
    Powered,
  }
}
