// Decompiled with JetBrains decompiler
// Type: AmbienceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AmbienceManager")]
public class AmbienceManager : KMonoBehaviour
{
  public Material LiquidMaterial;
  private float emitterZPosition;
  public AmbienceManager.QuadrantDef[] quadrantDefs;
  public AmbienceManager.Quadrant[] quadrants = new AmbienceManager.Quadrant[4];

  public static float BoilingTreshold { private set; get; } = 1f;

  protected override void OnSpawn()
  {
    if (!RuntimeManager.IsInitialized)
    {
      this.enabled = false;
    }
    else
    {
      AmbienceManager.BoilingTreshold = this.LiquidMaterial.GetFloat("_BoilingTreshold");
      for (int index = 0; index < this.quadrants.Length; ++index)
        this.quadrants[index] = new AmbienceManager.Quadrant(this.quadrantDefs[index]);
    }
  }

  protected override void OnForcedCleanUp()
  {
    foreach (AmbienceManager.Quadrant quadrant in this.quadrants)
    {
      foreach (AmbienceManager.Layer allLayer in quadrant.GetAllLayers())
        allLayer.Stop();
    }
  }

  private void LateUpdate()
  {
    GridArea visibleArea = GridVisibleArea.GetVisibleArea();
    Vector2I min = visibleArea.Min;
    Vector2I max = visibleArea.Max;
    Vector2I vector2I = min + (max - min) / 2;
    Vector3 worldPoint1 = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
    Vector3 worldPoint2 = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.GetPosition().z));
    Vector3 vector3_1 = worldPoint2 + (worldPoint1 - worldPoint2) / 2f;
    Vector3 vector3_2 = worldPoint1 - worldPoint2;
    if ((double) vector3_2.x > (double) vector3_2.y)
      vector3_2.y = vector3_2.x;
    else
      vector3_2.x = vector3_2.y;
    Vector3 vector3_3 = vector3_1 + vector3_2 / 2f;
    Vector3 vector3_4 = vector3_1 - vector3_2 / 2f;
    Vector3 vector3_5 = vector3_2 / 2f / 2f;
    this.quadrants[0].Update(new Vector2I(min.x, min.y), new Vector2I(vector2I.x, vector2I.y), new Vector3(vector3_4.x + vector3_5.x, vector3_4.y + vector3_5.y, this.emitterZPosition));
    this.quadrants[1].Update(new Vector2I(vector2I.x, min.y), new Vector2I(max.x, vector2I.y), new Vector3(vector3_1.x + vector3_5.x, vector3_4.y + vector3_5.y, this.emitterZPosition));
    this.quadrants[2].Update(new Vector2I(min.x, vector2I.y), new Vector2I(vector2I.x, max.y), new Vector3(vector3_4.x + vector3_5.x, vector3_1.y + vector3_5.y, this.emitterZPosition));
    this.quadrants[3].Update(new Vector2I(vector2I.x, vector2I.y), new Vector2I(max.x, max.y), new Vector3(vector3_1.x + vector3_5.x, vector3_1.y + vector3_5.y, this.emitterZPosition));
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    for (int index = 0; index < this.quadrants.Length; ++index)
    {
      num1 += (float) this.quadrants[index].spaceLayer.tileCount;
      num2 += (float) this.quadrants[index].facilityLayer.tileCount;
      num3 += (float) this.quadrants[index].totalTileCount;
    }
    AudioMixer.instance.UpdateSpaceVisibleSnapshot(num1 / num3);
    AudioMixer.instance.UpdateFacilityVisibleSnapshot(num2 / num3);
  }

  public class Tuning : TuningData<AmbienceManager.Tuning>
  {
    public int backwallTileValue = 1;
    public int foundationTileValue = 2;
    public int buildingTileValue = 3;
  }

  public class LiquidLayer(EventReference sound, EventReference one_shot_sound = default (EventReference)) : 
    AmbienceManager.Layer(sound, one_shot_sound)
  {
    private const string BOILING_INTENSITY_ID = "Boiling_Intensity";
    private const string BOILING_TILE_PERCENTAGE_ID = "Boiling_Tile_Percentage";
    public int boilingTileCount;
    public float boilTilePercentage;
    public float averageBoilIntensity;

    public override void Reset()
    {
      base.Reset();
      this.boilingTileCount = 0;
      this.averageBoilIntensity = 0.0f;
    }

    public override void UpdatePercentage(int cell_count)
    {
      base.UpdatePercentage(cell_count);
      this.boilTilePercentage = (float) this.boilingTileCount / (float) cell_count;
    }

    public override void UpdateParameters(Vector3 emitter_position)
    {
      base.UpdateParameters(emitter_position);
      int num = (int) this.soundEvent.setParameterByName("Boiling_Tile_Percentage", this.boilTilePercentage);
    }

    public override void UpdateAverageTemperature()
    {
      base.UpdateAverageTemperature();
      this.UpdateAverageBoilIntensity();
    }

    public void UpdateAverageBoilIntensity()
    {
      this.averageBoilIntensity = this.tileCount > 0 ? this.averageBoilIntensity / (float) this.tileCount : 0.0f;
      int num = (int) this.soundEvent.setParameterByName("Boiling_Intensity", this.averageBoilIntensity);
    }
  }

  public class Layer : IComparable<AmbienceManager.Layer>
  {
    private const string TILE_PERCENTAGE_ID = "tilePercentage";
    private const string AVERAGE_TEMPERATURE_ID = "averageTemperature";
    private const string AVERAGE_RADIATION_ID = "averageRadiation";
    public EventReference sound;
    public EventReference oneShotSound;
    public int tileCount;
    public float tilePercentage;
    public float volume;
    public bool isRunning;
    protected EventInstance soundEvent;
    public float averageTemperature;
    public float averageRadiation;
    public static Comparison<AmbienceManager.Layer> DefaultComparitor = (Comparison<AmbienceManager.Layer>) ((a, b) => a.CompareTo(b));

    public Layer(EventReference sound, EventReference one_shot_sound = default (EventReference))
    {
      this.sound = sound;
      this.oneShotSound = one_shot_sound;
    }

    public virtual void Reset()
    {
      this.tileCount = 0;
      this.averageTemperature = 0.0f;
      this.averageRadiation = 0.0f;
    }

    public virtual void UpdatePercentage(int cell_count)
    {
      this.tilePercentage = (float) this.tileCount / (float) cell_count;
    }

    public virtual void UpdateAverageTemperature()
    {
      this.averageTemperature /= (float) this.tileCount;
      int num = (int) this.soundEvent.setParameterByName("averageTemperature", this.averageTemperature);
    }

    public void UpdateAverageRadiation()
    {
      this.averageRadiation = this.tileCount > 0 ? this.averageRadiation / (float) this.tileCount : 0.0f;
      int num = (int) this.soundEvent.setParameterByName("averageRadiation", this.averageRadiation);
    }

    public virtual void UpdateParameters(Vector3 emitter_position)
    {
      if (!this.soundEvent.isValid())
        return;
      int num1 = (int) this.soundEvent.set3DAttributes(new Vector3(emitter_position.x, emitter_position.y, 0.0f).To3DAttributes());
      int num2 = (int) this.soundEvent.setParameterByName("tilePercentage", this.tilePercentage);
    }

    public void SetCustomParameter(string parameterName, float value)
    {
      int num = (int) this.soundEvent.setParameterByName(parameterName, value);
    }

    public int CompareTo(AmbienceManager.Layer layer) => layer.tileCount - this.tileCount;

    public void SetVolume(float volume)
    {
      if ((double) this.volume == (double) volume)
        return;
      this.volume = volume;
      if (!this.soundEvent.isValid())
        return;
      int num = (int) this.soundEvent.setVolume(volume);
    }

    public void Stop()
    {
      if (this.soundEvent.isValid())
      {
        int num1 = (int) this.soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        int num2 = (int) this.soundEvent.release();
      }
      this.isRunning = false;
    }

    public void Start(Vector3 emitter_position)
    {
      if (this.isRunning)
        return;
      if (!this.oneShotSound.IsNull)
      {
        EventInstance instance = KFMOD.CreateInstance(this.oneShotSound);
        if (!instance.isValid())
        {
          Debug.LogWarning((object) ("Could not find event: " + this.oneShotSound.ToString()));
        }
        else
        {
          ATTRIBUTES_3D attributes = new Vector3(emitter_position.x, emitter_position.y, 0.0f).To3DAttributes();
          int num1 = (int) instance.set3DAttributes(attributes);
          int num2 = (int) instance.setVolume(this.tilePercentage * 2f);
          int num3 = (int) instance.start();
          int num4 = (int) instance.release();
        }
      }
      else
      {
        this.soundEvent = KFMOD.CreateInstance(this.sound);
        if (this.soundEvent.isValid())
        {
          int num = (int) this.soundEvent.start();
        }
        this.isRunning = true;
      }
    }
  }

  [Serializable]
  public class QuadrantDef
  {
    public string name;
    public EventReference[] liquidSounds;
    public EventReference[] gasSounds;
    public EventReference[] solidSounds;
    public EventReference fogSound;
    public EventReference spaceSound;
    public EventReference rocketInteriorSound;
    public EventReference facilitySound;
    public EventReference radiationSound;
  }

  public class Quadrant
  {
    public string name;
    public Vector3 emitterPosition;
    public AmbienceManager.Layer[] gasLayers = new AmbienceManager.Layer[6];
    public AmbienceManager.LiquidLayer[] liquidLayers = new AmbienceManager.LiquidLayer[6];
    public AmbienceManager.Layer fogLayer;
    public AmbienceManager.Layer spaceLayer;
    public AmbienceManager.Layer rocketInteriorLayer;
    public AmbienceManager.Layer facilityLayer;
    public AmbienceManager.Layer radiationLayer;
    public AmbienceManager.Layer[] solidLayers = new AmbienceManager.Layer[21];
    private List<AmbienceManager.Layer> allLayers = new List<AmbienceManager.Layer>();
    private List<AmbienceManager.Layer> loopingLayers = new List<AmbienceManager.Layer>();
    private List<AmbienceManager.Layer> oneShotLayers = new List<AmbienceManager.Layer>();
    private List<AmbienceManager.Layer> topLayers = new List<AmbienceManager.Layer>();
    public static int activeSolidLayerCount = 2;
    public int totalTileCount;
    private bool m_isRadiationEnabled;
    private bool m_isClusterSpaceEnabled;
    private const string ROCKET_STATE_FOR_AMBIENCE = "RocketState";
    private AmbienceManager.Quadrant.SolidTimer[] solidTimers;

    public Quadrant(AmbienceManager.QuadrantDef def)
    {
      this.name = def.name;
      this.fogLayer = new AmbienceManager.Layer(def.fogSound);
      this.allLayers.Add(this.fogLayer);
      this.loopingLayers.Add(this.fogLayer);
      this.spaceLayer = new AmbienceManager.Layer(def.spaceSound);
      this.allLayers.Add(this.spaceLayer);
      this.loopingLayers.Add(this.spaceLayer);
      this.m_isClusterSpaceEnabled = DlcManager.FeatureClusterSpaceEnabled();
      if (this.m_isClusterSpaceEnabled)
      {
        this.rocketInteriorLayer = new AmbienceManager.Layer(def.rocketInteriorSound);
        this.allLayers.Add(this.rocketInteriorLayer);
      }
      this.facilityLayer = new AmbienceManager.Layer(def.facilitySound);
      this.allLayers.Add(this.facilityLayer);
      this.loopingLayers.Add(this.facilityLayer);
      this.m_isRadiationEnabled = Sim.IsRadiationEnabled();
      if (this.m_isRadiationEnabled)
      {
        this.radiationLayer = new AmbienceManager.Layer(def.radiationSound);
        this.allLayers.Add(this.radiationLayer);
      }
      for (int index = 0; index < this.gasLayers.Length; ++index)
      {
        this.gasLayers[index] = new AmbienceManager.Layer(def.gasSounds[index]);
        this.liquidLayers[index] = new AmbienceManager.LiquidLayer(def.liquidSounds[index]);
        this.allLayers.Add(this.gasLayers[index]);
        this.allLayers.Add((AmbienceManager.Layer) this.liquidLayers[index]);
        this.loopingLayers.Add(this.gasLayers[index]);
        this.loopingLayers.Add((AmbienceManager.Layer) this.liquidLayers[index]);
      }
      for (int index = 0; index < this.solidLayers.Length; ++index)
      {
        if (index >= def.solidSounds.Length)
          Debug.LogError((object) ("Missing solid layer: " + ((SolidAmbienceType) index).ToString()));
        this.solidLayers[index] = new AmbienceManager.Layer(new EventReference(), def.solidSounds[index]);
        this.allLayers.Add(this.solidLayers[index]);
        this.oneShotLayers.Add(this.solidLayers[index]);
      }
      this.solidTimers = new AmbienceManager.Quadrant.SolidTimer[AmbienceManager.Quadrant.activeSolidLayerCount];
      for (int index = 0; index < AmbienceManager.Quadrant.activeSolidLayerCount; ++index)
        this.solidTimers[index] = new AmbienceManager.Quadrant.SolidTimer();
    }

    public void Update(Vector2I min, Vector2I max, Vector3 emitter_position)
    {
      this.emitterPosition = emitter_position;
      this.totalTileCount = 0;
      for (int index = 0; index < this.allLayers.Count; ++index)
        this.allLayers[index].Reset();
      float num1 = 1f - AmbienceManager.BoilingTreshold;
      for (int y = min.y; y < max.y; ++y)
      {
        if (y % 2 != 1)
        {
          for (int x = min.x; x < max.x; ++x)
          {
            if (x % 2 != 0)
            {
              int cell = Grid.XYToCell(x, y);
              if (Grid.IsValidCell(cell))
              {
                ++this.totalTileCount;
                if (Grid.IsVisible(cell))
                {
                  if (Grid.GravitasFacility[cell])
                  {
                    this.facilityLayer.tileCount += 8;
                  }
                  else
                  {
                    Element element = Grid.Element[cell];
                    if (element != null)
                    {
                      if (element.IsLiquid && Grid.IsSubstantialLiquid(cell))
                      {
                        AmbienceType ambience = element.substance.GetAmbience();
                        if (ambience != AmbienceType.None)
                        {
                          ++this.liquidLayers[(int) ambience].tileCount;
                          this.liquidLayers[(int) ambience].averageTemperature += Grid.Temperature[cell];
                          float num2 = Mathf.Clamp01(element.GetRelativeHeatLevel(Grid.Temperature[cell]) - AmbienceManager.BoilingTreshold) / num1;
                          this.liquidLayers[(int) ambience].boilingTileCount += (double) num2 > 0.0 ? 1 : 0;
                          this.liquidLayers[(int) ambience].averageBoilIntensity += num2;
                        }
                      }
                      else if (element.IsGas)
                      {
                        AmbienceType ambience = element.substance.GetAmbience();
                        if (ambience != AmbienceType.None)
                        {
                          ++this.gasLayers[(int) ambience].tileCount;
                          this.gasLayers[(int) ambience].averageTemperature += Grid.Temperature[cell];
                        }
                      }
                      else if (element.IsSolid)
                      {
                        SolidAmbienceType solidAmbience = element.substance.GetSolidAmbience();
                        if (Grid.Foundation[cell])
                        {
                          this.solidLayers[3].tileCount += TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
                          this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
                        }
                        else if ((UnityEngine.Object) Grid.Objects[cell, 2] != (UnityEngine.Object) null)
                        {
                          this.solidLayers[3].tileCount += TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
                          this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
                        }
                        else if (solidAmbience != SolidAmbienceType.None)
                          ++this.solidLayers[(int) solidAmbience].tileCount;
                        else if (element.id == SimHashes.Regolith || element.id == SimHashes.MaficRock)
                          ++this.spaceLayer.tileCount;
                      }
                      else if (element.id == SimHashes.Vacuum && CellSelectionObject.IsExposedToSpace(cell))
                      {
                        if ((UnityEngine.Object) Grid.Objects[cell, 1] != (UnityEngine.Object) null)
                          this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().buildingTileValue;
                        ++this.spaceLayer.tileCount;
                      }
                    }
                  }
                  if ((double) Grid.Radiation[cell] > 0.0)
                  {
                    this.radiationLayer.averageRadiation += Grid.Radiation[cell];
                    ++this.radiationLayer.tileCount;
                  }
                }
                else
                  ++this.fogLayer.tileCount;
              }
            }
          }
        }
      }
      Vector2I vector2I = max - min;
      int cell_count = vector2I.x * vector2I.y;
      for (int index = 0; index < this.allLayers.Count; ++index)
        this.allLayers[index].UpdatePercentage(cell_count);
      this.loopingLayers.Sort(AmbienceManager.Layer.DefaultComparitor);
      this.topLayers.Clear();
      for (int index = 0; index < this.loopingLayers.Count; ++index)
      {
        AmbienceManager.Layer loopingLayer = this.loopingLayers[index];
        if (index < 3 && (double) loopingLayer.tilePercentage > 0.0)
        {
          loopingLayer.Start(emitter_position);
          loopingLayer.UpdateAverageTemperature();
          loopingLayer.UpdateParameters(emitter_position);
          this.topLayers.Add(loopingLayer);
        }
        else
          loopingLayer.Stop();
      }
      if (this.m_isClusterSpaceEnabled)
      {
        float volume = 0.0f;
        if ((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) ClusterManager.Instance.activeWorld != (UnityEngine.Object) null && ClusterManager.Instance.activeWorld.IsModuleInterior)
          volume = 1f;
        this.rocketInteriorLayer.Start(emitter_position);
        this.rocketInteriorLayer.SetCustomParameter("RocketState", (float) ClusterManager.RocketInteriorState);
        this.rocketInteriorLayer.SetVolume(volume);
      }
      if (this.m_isRadiationEnabled)
      {
        this.radiationLayer.Start(emitter_position);
        this.radiationLayer.UpdateAverageRadiation();
        this.radiationLayer.UpdateParameters(emitter_position);
      }
      this.oneShotLayers.Sort(AmbienceManager.Layer.DefaultComparitor);
      for (int index = 0; index < AmbienceManager.Quadrant.activeSolidLayerCount; ++index)
      {
        if (this.solidTimers[index].ShouldPlay() && (double) this.oneShotLayers[index].tilePercentage > 0.0)
          this.oneShotLayers[index].Start(emitter_position);
      }
    }

    public List<AmbienceManager.Layer> GetAllLayers() => this.allLayers;

    public class SolidTimer
    {
      public static float solidMinTime = 9f;
      public static float solidMaxTime = 15f;
      public float solidTargetTime;

      public SolidTimer()
      {
        this.solidTargetTime = Time.unscaledTime + UnityEngine.Random.value * AmbienceManager.Quadrant.SolidTimer.solidMinTime;
      }

      public bool ShouldPlay()
      {
        if ((double) Time.unscaledTime <= (double) this.solidTargetTime)
          return false;
        this.solidTargetTime = (float) ((double) Time.unscaledTime + (double) AmbienceManager.Quadrant.SolidTimer.solidMinTime + (double) UnityEngine.Random.value * ((double) AmbienceManager.Quadrant.SolidTimer.solidMaxTime - (double) AmbienceManager.Quadrant.SolidTimer.solidMinTime));
        return true;
      }
    }
  }
}
