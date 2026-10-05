// Decompiled with JetBrains decompiler
// Type: Staterpillar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Staterpillar : KMonoBehaviour
{
  public ObjectLayer conduitLayer;
  public string connectorDefId;
  private IList<Tag> dummyElement;
  private BuildingDef connectorDef;
  [Serialize]
  private Ref<KPrefabID> connectorRef = new Ref<KPrefabID>();
  private AttributeModifier wildMod = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -75f, (string) BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.WILD);
  private ConduitDispenser cachedConduitDispenser;
  private StaterpillarGenerator cachedGenerator;

  protected override void OnPrefabInit()
  {
    this.dummyElement = (IList<Tag>) new List<Tag>()
    {
      SimHashes.Unobtanium.CreateTag()
    };
    this.connectorDef = Assets.GetBuildingDef(this.connectorDefId);
  }

  public void SpawnConnectorBuilding(int targetCell)
  {
    if (this.conduitLayer == ObjectLayer.Wire)
      this.SpawnGenerator(targetCell);
    else
      this.SpawnConduitConnector(targetCell);
  }

  public void DestroyOrphanedConnectorBuilding()
  {
    KPrefabID building = this.GetConnectorBuilding();
    if (!((UnityEngine.Object) building != (UnityEngine.Object) null))
      return;
    this.connectorRef.Set((KPrefabID) null);
    this.cachedGenerator = (StaterpillarGenerator) null;
    this.cachedConduitDispenser = (ConduitDispenser) null;
    GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Connector building", (Action<object>) (o =>
    {
      if (!((UnityEngine.Object) building != (UnityEngine.Object) null))
        return;
      Util.KDestroyGameObject(building.gameObject);
    }));
  }

  public void EnableConnector()
  {
    if (this.conduitLayer == ObjectLayer.Wire)
      this.EnableGenerator();
    else
      this.EnableConduitConnector();
  }

  public bool IsConnectorBuildingSpawned() => (UnityEngine.Object) this.GetConnectorBuilding() != (UnityEngine.Object) null;

  public bool IsConnected()
  {
    if (this.conduitLayer != ObjectLayer.Wire)
      return this.GetConduitDispenser().IsConnected;
    return this.GetGenerator().CircuitID != ushort.MaxValue;
  }

  public KPrefabID GetConnectorBuilding() => this.connectorRef.Get();

  private void SpawnConduitConnector(int targetCell)
  {
    if (!((UnityEngine.Object) this.GetConduitDispenser() == (UnityEngine.Object) null))
      return;
    GameObject gameObject = this.connectorDef.Build(targetCell, Orientation.R180, (Storage) null, this.dummyElement, this.gameObject.GetComponent<PrimaryElement>().Temperature);
    this.connectorRef = new Ref<KPrefabID>(gameObject.GetComponent<KPrefabID>());
    gameObject.SetActive(true);
    gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
  }

  private void EnableConduitConnector()
  {
    ConduitDispenser conduitDispenser = this.GetConduitDispenser();
    conduitDispenser.GetComponent<BuildingCellVisualizer>().enabled = true;
    conduitDispenser.storage = this.GetComponent<Storage>();
    conduitDispenser.SetOnState(true);
  }

  public ConduitDispenser GetConduitDispenser()
  {
    if ((UnityEngine.Object) this.cachedConduitDispenser == (UnityEngine.Object) null)
    {
      KPrefabID kprefabId = this.connectorRef.Get();
      if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null)
        this.cachedConduitDispenser = kprefabId.GetComponent<ConduitDispenser>();
    }
    return this.cachedConduitDispenser;
  }

  private void DestroyOrphanedConduitDispenserBuilding()
  {
    ConduitDispenser dispenser = this.GetConduitDispenser();
    if (!((UnityEngine.Object) dispenser != (UnityEngine.Object) null))
      return;
    this.connectorRef.Set((KPrefabID) null);
    GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Dispenser", (Action<object>) (o =>
    {
      if (!((UnityEngine.Object) dispenser != (UnityEngine.Object) null))
        return;
      Util.KDestroyGameObject(dispenser.gameObject);
    }));
  }

  private void SpawnGenerator(int targetCell)
  {
    StaterpillarGenerator generator = this.GetGenerator();
    GameObject gameObject = (GameObject) null;
    if ((UnityEngine.Object) generator != (UnityEngine.Object) null)
      gameObject = generator.gameObject;
    if (!(bool) (UnityEngine.Object) gameObject)
    {
      gameObject = this.connectorDef.Build(targetCell, Orientation.R180, (Storage) null, this.dummyElement, this.gameObject.GetComponent<PrimaryElement>().Temperature);
      StaterpillarGenerator component = gameObject.GetComponent<StaterpillarGenerator>();
      component.parent = new Ref<Staterpillar>(this);
      this.connectorRef = new Ref<KPrefabID>(component.GetComponent<KPrefabID>());
      gameObject.SetActive(true);
      gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
      component.enabled = false;
    }
    Attributes attributes = gameObject.gameObject.GetAttributes();
    bool flag1 = (double) this.gameObject.GetSMI<WildnessMonitor.Instance>().wildness.value > 0.0;
    if (flag1)
      attributes.Add(this.wildMod);
    bool flag2 = this.gameObject.GetComponent<Effects>().HasEffect("Unhappy");
    CreatureCalorieMonitor.Instance smi = this.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
    if (!(smi.IsHungry() | flag2))
      return;
    float calories0to1 = smi.GetCalories0to1();
    float num1 = 1f;
    if ((double) calories0to1 <= 0.0)
      num1 = flag1 ? 0.1f : 0.025f;
    else if ((double) calories0to1 <= 0.30000001192092896)
      num1 = 0.5f;
    else if ((double) calories0to1 <= 0.5)
      num1 = 0.75f;
    if ((double) num1 >= 1.0)
      return;
    float num2 = !flag1 ? (float) ((1.0 - (double) num1) * 100.0) : Mathf.Lerp(0.0f, 25f, 1f - num1);
    AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -num2, (string) BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY);
    attributes.Add(modifier);
  }

  private void EnableGenerator()
  {
    StaterpillarGenerator generator = this.GetGenerator();
    generator.enabled = true;
    generator.GetComponent<BuildingCellVisualizer>().enabled = true;
  }

  public StaterpillarGenerator GetGenerator()
  {
    if ((UnityEngine.Object) this.cachedGenerator == (UnityEngine.Object) null)
    {
      KPrefabID kprefabId = this.connectorRef.Get();
      if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null)
        this.cachedGenerator = kprefabId.GetComponent<StaterpillarGenerator>();
    }
    return this.cachedGenerator;
  }
}
