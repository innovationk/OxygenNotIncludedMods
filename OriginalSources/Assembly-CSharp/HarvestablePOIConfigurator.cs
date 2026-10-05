// Decompiled with JetBrains decompiler
// Type: HarvestablePOIConfigurator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIConfigurator")]
public class HarvestablePOIConfigurator : KMonoBehaviour
{
  private static List<HarvestablePOIConfigurator.HarvestablePOIType> _poiTypes;
  public HashedString presetType;
  public float presetMin;
  public float presetMax = 1f;

  public static HarvestablePOIConfigurator.HarvestablePOIType FindType(HashedString typeId)
  {
    HarvestablePOIConfigurator.HarvestablePOIType type = (HarvestablePOIConfigurator.HarvestablePOIType) null;
    if (typeId != HashedString.Invalid)
      type = HarvestablePOIConfigurator._poiTypes.Find((Predicate<HarvestablePOIConfigurator.HarvestablePOIType>) (t => (HashedString) t.id == typeId));
    if (type == null)
      Debug.LogError((object) $"Tried finding a harvestable poi with id {typeId.ToString()} but it doesn't exist!");
    return type;
  }

  public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration MakeConfiguration()
  {
    return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
  }

  private HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration CreateRandomInstance(
    HashedString typeId,
    float min,
    float max)
  {
    int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
    ClusterGridEntity component = this.GetComponent<ClusterGridEntity>();
    Vector3 position = ClusterGrid.Instance.GetPosition(component);
    int x = (int) position.x;
    KRandom randomSource = new KRandom(globalWorldSeed + x + (int) position.y);
    return new HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration()
    {
      typeId = typeId,
      capacityRoll = this.Roll(randomSource, min, max),
      rechargeRoll = this.Roll(randomSource, min, max)
    };
  }

  private float Roll(KRandom randomSource, float min, float max)
  {
    return (float) (randomSource.NextDouble() * ((double) max - (double) min)) + min;
  }

  public class HarvestablePOIType : IHasDlcRestrictions
  {
    public string id;
    public HashedString idHash;
    public Dictionary<SimHashes, float> harvestableElements;
    public float poiCapacityMin;
    public float poiCapacityMax;
    public float poiRechargeMin;
    public float poiRechargeMax;
    public int initialDataBanks;
    public Dictionary<SimHashes, float> initialLiberatedResources;
    public bool canProvideArtifacts;
    [Obsolete]
    public string dlcID;
    public string[] requiredDlcIds;
    public string[] forbiddenDlcIds;
    public List<string> orbitalObject;
    public int maxNumOrbitingObjects;

    public string[] GetRequiredDlcIds() => this.requiredDlcIds;

    public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

    public HarvestablePOIType(
      string id,
      Dictionary<SimHashes, float> harvestableElements,
      float poiCapacityMin = 54000f,
      float poiCapacityMax = 81000f,
      float poiRechargeMin = 30000f,
      float poiRechargeMax = 60000f,
      bool canProvideArtifacts = true,
      List<string> orbitalObject = null,
      int maxNumOrbitingObjects = 20,
      string[] requiredDlcIds = null,
      string[] forbiddenDlcIds = null)
      : this(id, harvestableElements, 0, poiCapacityMin: poiCapacityMin, poiCapacityMax: poiCapacityMax, poiRechargeMin: poiRechargeMin, poiRechargeMax: poiRechargeMax, canProvideArtifacts: canProvideArtifacts, orbitalObject: orbitalObject, maxNumOrbitingObjects: maxNumOrbitingObjects, requiredDlcIds: requiredDlcIds, forbiddenDlcIds: forbiddenDlcIds)
    {
    }

    public HarvestablePOIType(
      string id,
      Dictionary<SimHashes, float> harvestableElements,
      int initialDatabanks,
      Dictionary<SimHashes, float> initialLiberatedResources = null,
      float poiCapacityMin = 54000f,
      float poiCapacityMax = 81000f,
      float poiRechargeMin = 30000f,
      float poiRechargeMax = 60000f,
      bool canProvideArtifacts = true,
      List<string> orbitalObject = null,
      int maxNumOrbitingObjects = 20,
      string[] requiredDlcIds = null,
      string[] forbiddenDlcIds = null)
    {
      this.id = id;
      this.idHash = (HashedString) id;
      this.harvestableElements = harvestableElements;
      this.initialDataBanks = initialDatabanks;
      this.initialLiberatedResources = initialLiberatedResources;
      this.poiCapacityMin = poiCapacityMin;
      this.poiCapacityMax = poiCapacityMax;
      this.poiRechargeMin = poiRechargeMin;
      this.poiRechargeMax = poiRechargeMax;
      this.canProvideArtifacts = canProvideArtifacts;
      this.orbitalObject = orbitalObject;
      this.maxNumOrbitingObjects = maxNumOrbitingObjects;
      this.requiredDlcIds = requiredDlcIds;
      this.forbiddenDlcIds = forbiddenDlcIds;
      if (HarvestablePOIConfigurator._poiTypes == null)
        HarvestablePOIConfigurator._poiTypes = new List<HarvestablePOIConfigurator.HarvestablePOIType>();
      HarvestablePOIConfigurator._poiTypes.Add(this);
    }

    [Obsolete]
    public HarvestablePOIType(
      string id,
      Dictionary<SimHashes, float> harvestableElements,
      float poiCapacityMin = 54000f,
      float poiCapacityMax = 81000f,
      float poiRechargeMin = 30000f,
      float poiRechargeMax = 60000f,
      bool canProvideArtifacts = true,
      List<string> orbitalObject = null,
      int maxNumOrbitingObjects = 20,
      string dlcID = "EXPANSION1_ID")
      : this(id, harvestableElements, poiCapacityMin, poiCapacityMax, poiRechargeMin, poiRechargeMax, canProvideArtifacts, orbitalObject, maxNumOrbitingObjects, (string[]) null, (string[]) null)
    {
      this.requiredDlcIds = DlcManager.EXPANSION1;
    }
  }

  [Serializable]
  public class HarvestablePOIInstanceConfiguration
  {
    public HashedString typeId;
    private bool didInit;
    public float capacityRoll;
    public float rechargeRoll;
    private float poiTotalCapacity;
    private float poiRecharge;

    private void Init()
    {
      if (this.didInit)
        return;
      this.didInit = true;
      this.poiTotalCapacity = MathUtil.ReRange(this.capacityRoll, 0.0f, 1f, this.poiType.poiCapacityMin, this.poiType.poiCapacityMax);
      this.poiRecharge = MathUtil.ReRange(this.rechargeRoll, 0.0f, 1f, this.poiType.poiRechargeMin, this.poiType.poiRechargeMax);
    }

    public HarvestablePOIConfigurator.HarvestablePOIType poiType
    {
      get => HarvestablePOIConfigurator.FindType(this.typeId);
    }

    public Dictionary<SimHashes, float> GetElementsWithWeights()
    {
      this.Init();
      return this.poiType.harvestableElements;
    }

    public bool CanProvideArtifacts()
    {
      this.Init();
      return this.poiType.canProvideArtifacts;
    }

    public float GetMaxCapacity()
    {
      this.Init();
      return this.poiTotalCapacity;
    }

    public float GetRechargeTime()
    {
      this.Init();
      return this.poiRecharge;
    }
  }
}
