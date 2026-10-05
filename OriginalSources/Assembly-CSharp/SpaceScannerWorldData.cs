// Decompiled with JetBrains decompiler
// Type: SpaceScannerWorldData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerWorldData
{
  [NonSerialized]
  private WorldContainer world;
  [Serialize]
  public int worldId;
  [Serialize]
  public float networkQuality01;
  [Serialize]
  public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();
  [Serialize]
  public HashSet<string> targetIdsDetected = new HashSet<string>();
  [NonSerialized]
  public SpaceScannerWorldData.Scratchpad scratchpad = new SpaceScannerWorldData.Scratchpad();

  [Serialize]
  public SpaceScannerWorldData(int worldId) => this.worldId = worldId;

  public WorldContainer GetWorld()
  {
    if ((UnityEngine.Object) this.world == (UnityEngine.Object) null)
      this.world = ClusterManager.Instance.GetWorld(this.worldId);
    return this.world;
  }

  public class Scratchpad
  {
    public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();
    public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers = new HashSet<MeteorShowerEvent.StatesInstance>();
    public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();
    public HashSet<Clustercraft> lastDetectedRocketsDLC1 = new HashSet<Clustercraft>();
  }
}
