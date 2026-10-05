// Decompiled with JetBrains decompiler
// Type: ArtifactPOIConfigurator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIConfigurator")]
public class ArtifactPOIConfigurator : KMonoBehaviour
{
  private static List<ArtifactPOIConfigurator.ArtifactPOIType> _poiTypes;
  public static ArtifactPOIConfigurator.ArtifactPOIType defaultArtifactPoiType = new ArtifactPOIConfigurator.ArtifactPOIType("HarvestablePOIArtifacts", requiredDlcIds: DlcManager.EXPANSION1);
  public HashedString presetType;
  public float presetMin;
  public float presetMax = 1f;

  public static ArtifactPOIConfigurator.ArtifactPOIType FindType(HashedString typeId)
  {
    ArtifactPOIConfigurator.ArtifactPOIType type = (ArtifactPOIConfigurator.ArtifactPOIType) null;
    if (typeId != HashedString.Invalid)
      type = ArtifactPOIConfigurator._poiTypes.Find((Predicate<ArtifactPOIConfigurator.ArtifactPOIType>) (t => (HashedString) t.id == typeId));
    if (type == null)
      Debug.LogError((object) $"Tried finding a harvestable poi with id {typeId.ToString()} but it doesn't exist!");
    return type;
  }

  public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration MakeConfiguration()
  {
    return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
  }

  private ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration CreateRandomInstance(
    HashedString typeId,
    float min,
    float max)
  {
    int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
    ClusterGridEntity component = this.GetComponent<ClusterGridEntity>();
    Vector3 position = ClusterGrid.Instance.GetPosition(component);
    int x = (int) position.x;
    KRandom randomSource = new KRandom(globalWorldSeed + x + (int) position.y);
    return new ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration()
    {
      typeId = typeId,
      rechargeRoll = this.Roll(randomSource, min, max)
    };
  }

  private float Roll(KRandom randomSource, float min, float max)
  {
    return (float) (randomSource.NextDouble() * ((double) max - (double) min)) + min;
  }

  public class ArtifactPOIType : IHasDlcRestrictions
  {
    public string id;
    public HashedString idHash;
    public string harvestableArtifactID;
    public bool destroyOnHarvest;
    public float poiRechargeTimeMin;
    public float poiRechargeTimeMax;
    [Obsolete]
    public string dlcID;
    public int initialDatabankCount;
    public string[] requiredDlcIds;
    public string[] forbiddenDlcIds;
    public List<string> orbitalObject = new List<string>()
    {
      Db.Get().OrbitalTypeCategories.gravitas.Id
    };

    public string[] GetRequiredDlcIds() => this.requiredDlcIds;

    public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

    public ArtifactPOIType(
      string id,
      string harvestableArtifactID = null,
      bool destroyOnHarvest = false,
      float poiRechargeTimeMin = 30000f,
      float poiRechargeTimeMax = 60000f,
      string[] requiredDlcIds = null,
      string[] forbiddenDlcIds = null)
      : this(id, 0, harvestableArtifactID, destroyOnHarvest, poiRechargeTimeMin, poiRechargeTimeMax, requiredDlcIds, forbiddenDlcIds)
    {
    }

    public ArtifactPOIType(
      string id,
      int databankCount,
      string harvestableArtifactID = null,
      bool destroyOnHarvest = false,
      float poiRechargeTimeMin = 30000f,
      float poiRechargeTimeMax = 60000f,
      string[] requiredDlcIds = null,
      string[] forbiddenDlcIds = null)
    {
      this.id = id;
      this.idHash = (HashedString) id;
      this.initialDatabankCount = databankCount;
      this.harvestableArtifactID = harvestableArtifactID;
      this.destroyOnHarvest = destroyOnHarvest;
      this.poiRechargeTimeMin = poiRechargeTimeMin;
      this.poiRechargeTimeMax = poiRechargeTimeMax;
      this.requiredDlcIds = requiredDlcIds;
      this.forbiddenDlcIds = forbiddenDlcIds;
      if (ArtifactPOIConfigurator._poiTypes == null)
        ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
      ArtifactPOIConfigurator._poiTypes.Add(this);
    }

    [Obsolete]
    public ArtifactPOIType(
      string id,
      string harvestableArtifactID = null,
      bool destroyOnHarvest = false,
      float poiRechargeTimeMin = 30000f,
      float poiRechargeTimeMax = 60000f,
      string dlcID = "EXPANSION1_ID")
    {
      this.id = id;
      this.idHash = (HashedString) id;
      this.harvestableArtifactID = harvestableArtifactID;
      this.destroyOnHarvest = destroyOnHarvest;
      this.poiRechargeTimeMin = poiRechargeTimeMin;
      this.poiRechargeTimeMax = poiRechargeTimeMax;
      this.dlcID = dlcID;
      if (ArtifactPOIConfigurator._poiTypes == null)
        ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
      ArtifactPOIConfigurator._poiTypes.Add(this);
    }
  }

  [Serializable]
  public class ArtifactPOIInstanceConfiguration
  {
    public HashedString typeId;
    private bool didInit;
    public float rechargeRoll;
    private float poiRechargeTime;

    private void Init()
    {
      if (this.didInit)
        return;
      this.didInit = true;
      this.poiRechargeTime = MathUtil.ReRange(this.rechargeRoll, 0.0f, 1f, this.poiType.poiRechargeTimeMin, this.poiType.poiRechargeTimeMax);
    }

    public ArtifactPOIConfigurator.ArtifactPOIType poiType
    {
      get => ArtifactPOIConfigurator.FindType(this.typeId);
    }

    public bool DestroyOnHarvest()
    {
      this.Init();
      return this.poiType.destroyOnHarvest;
    }

    public string GetArtifactID()
    {
      this.Init();
      return this.poiType.harvestableArtifactID;
    }

    public float GetRechargeTime()
    {
      this.Init();
      return this.poiRechargeTime;
    }
  }
}
