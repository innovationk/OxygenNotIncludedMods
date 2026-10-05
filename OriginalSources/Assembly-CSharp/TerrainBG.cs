// Decompiled with JetBrains decompiler
// Type: TerrainBG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/TerrainBG")]
public class TerrainBG : KMonoBehaviour
{
  public Material northernLightMaterial_ceres;
  public Material largeImpactorFragmentsMaterial;
  public Material starsMaterial_surface;
  public Material starsMaterial_orbit;
  public Material starsMaterial_space;
  public Material backgroundMaterial;
  public Material gasMaterial;
  public bool doDraw = true;
  private const string Sound_Destroyed_Victory_End_Sequence = "Asteroid_destroyed_end";
  [SerializeField]
  private Texture3D noiseVolume;
  private Mesh starsPlane;
  private Mesh northernLightsPlane;
  private Mesh largeImpactorDefeatedPlane;
  private Mesh worldPlane;
  private Mesh gasPlane;
  private int layer;
  private float northernLightSkySize = 2f;
  public static bool preventLargeImpactorFragmentsFromProgressing;
  public const float LargeImpactorFragmentsEntryEffectDuration = 2.5f;
  private float LargeImpactorEntryProgress = -1f;
  private MaterialPropertyBlock[] propertyBlocks;

  public bool LargeImpactorFragmentsVisible
  {
    get
    {
      return (Object) ClusterManager.Instance.activeWorld != (Object) null && ClusterManager.Instance.activeWorld.largeImpactorFragments == FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.ALLOWED && SaveGame.Instance.ColonyAchievementTracker.largeImpactorState == ColonyAchievementTracker.LargeImpactorState.Defeated;
    }
  }

  public float LargeImpactorBackgroundScale
  {
    get => SaveGame.Instance.ColonyAchievementTracker.LargeImpactorBackgroundScale;
  }

  protected override void OnPrefabInit()
  {
    TerrainBG.preventLargeImpactorFragmentsFromProgressing = false;
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    this.layer = LayerMask.NameToLayer("Default");
    this.noiseVolume = this.CreateTexture3D(32 /*0x20*/);
    this.starsPlane = this.CreateStarsPlane("StarsPlane");
    this.northernLightsPlane = this.CreateNorthernLightsPlane("NorthernLightsPlane");
    this.largeImpactorDefeatedPlane = this.CreateGridSizePlane("LargeImpactorDefeatedPlane");
    this.worldPlane = this.CreateWorldPlane("WorldPlane");
    this.gasPlane = this.CreateGasPlane("GasPlane");
    this.propertyBlocks = new MaterialPropertyBlock[Lighting.Instance.Settings.BackgroundLayers];
    for (int index = 0; index < this.propertyBlocks.Length; ++index)
      this.propertyBlocks[index] = new MaterialPropertyBlock();
    this.LargeImpactorEntryProgress = this.LargeImpactorFragmentsVisible ? 1f : -1f;
    this.largeImpactorFragmentsMaterial.SetFloat("_EntryProgress", this.LargeImpactorEntryProgress);
    this.largeImpactorFragmentsMaterial.SetFloat("_LargeImpactorScale", this.LargeImpactorBackgroundScale);
  }

  private Texture3D CreateTexture3D(int size)
  {
    Color32[] colors = new Color32[size * size * size];
    Texture3D texture3D = new Texture3D(size, size, size, TextureFormat.RGBA32, true);
    for (int index1 = 0; index1 < size; ++index1)
    {
      for (int index2 = 0; index2 < size; ++index2)
      {
        for (int index3 = 0; index3 < size; ++index3)
        {
          Color32 color32 = new Color32((byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue));
          colors[index1 + index2 * size + index3 * size * size] = color32;
        }
      }
    }
    texture3D.SetPixels32(colors);
    texture3D.Apply();
    return texture3D;
  }

  public Mesh CreateGasPlane(string name)
  {
    Mesh gasPlane = new Mesh();
    gasPlane.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3((float) Grid.WidthInCells, 0.0f, 0.0f),
      new Vector3(0.0f, Grid.HeightInMeters, 0.0f),
      new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    gasPlane.vertices = vector3Array2;
    gasPlane.uv = vector2Array2;
    gasPlane.triangles = numArray2;
    gasPlane.bounds = new Bounds(new Vector3((float) Grid.WidthInCells * 0.5f, (float) Grid.HeightInCells * 0.5f, 0.0f), new Vector3((float) Grid.WidthInCells, (float) Grid.HeightInCells, 0.0f));
    return gasPlane;
  }

  public Mesh CreateWorldPlane(string name)
  {
    Mesh worldPlane = new Mesh();
    worldPlane.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3((float) -Grid.WidthInCells, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) Grid.WidthInCells * 2f, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) -Grid.WidthInCells, Grid.HeightInMeters * 2f, 0.0f),
      new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    worldPlane.vertices = vector3Array2;
    worldPlane.uv = vector2Array2;
    worldPlane.triangles = numArray2;
    worldPlane.bounds = new Bounds(new Vector3((float) Grid.WidthInCells * 0.5f, (float) Grid.HeightInCells * 0.5f, 0.0f), new Vector3((float) Grid.WidthInCells, (float) Grid.HeightInCells, 0.0f));
    return worldPlane;
  }

  public Mesh CreateStarsPlane(string name)
  {
    Mesh starsPlane = new Mesh();
    starsPlane.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3((float) -Grid.WidthInCells, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) Grid.WidthInCells * 2f, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) -Grid.WidthInCells, Grid.HeightInMeters * 2f, 0.0f),
      new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    starsPlane.vertices = vector3Array2;
    starsPlane.uv = vector2Array2;
    starsPlane.triangles = numArray2;
    Vector2 vector2 = new Vector2((float) Grid.WidthInCells, 2f * (float) Grid.HeightInCells);
    starsPlane.bounds = new Bounds(new Vector3(0.5f * vector2.x, 0.5f * vector2.y, 0.0f), new Vector3(vector2.x, vector2.y, 0.0f));
    return starsPlane;
  }

  public Mesh CreateNorthernLightsPlane(string name)
  {
    Mesh northernLightsPlane = new Mesh();
    northernLightsPlane.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    float x = 1f;
    float y = this.northernLightSkySize * 0.5f;
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3(-x, -y, 0.0f),
      new Vector3(x, -y, 0.0f),
      new Vector3(-x, y, 0.0f),
      new Vector3(x, y, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    northernLightsPlane.vertices = vector3Array2;
    northernLightsPlane.uv = vector2Array2;
    northernLightsPlane.triangles = numArray2;
    return northernLightsPlane;
  }

  public Mesh CreateGridSizePlane(string name)
  {
    Mesh gridSizePlane = new Mesh();
    gridSizePlane.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    float num = (float) Mathf.Max(Grid.WidthInCells, Grid.HeightInCells) / 2f;
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3(-num, -num, 0.0f),
      new Vector3(num, -num, 0.0f),
      new Vector3(-num, num, 0.0f),
      new Vector3(num, num, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    gridSizePlane.vertices = vector3Array2;
    gridSizePlane.uv = vector2Array2;
    gridSizePlane.triangles = numArray2;
    return gridSizePlane;
  }

  private void LateUpdate()
  {
    if (!this.doDraw)
      return;
    Material material = this.starsMaterial_surface;
    if (ClusterManager.Instance.activeWorld.IsModuleInterior)
    {
      Clustercraft component = ClusterManager.Instance.activeWorld.GetComponent<Clustercraft>();
      material = component.Status == Clustercraft.CraftStatus.InFlight ? (!((Object) ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(component.Location, EntityLayer.Asteroid) != (Object) null) ? this.starsMaterial_space : this.starsMaterial_space) : this.starsMaterial_surface;
      bool flag = component.IsFlightInProgress() && component.HasResourcesToMove();
      material.SetFloat("_IsInFlight", flag ? 1f : 0.0f);
      material.SetFloat("_AccelerationTimeStamp", component.LastTimeFlightBegan);
      material.SetFloat("_DecelerationTimeStamp", component.LastTimeFlightStopped);
    }
    material.renderQueue = RenderQueues.Stars;
    material.SetTexture("_NoiseVolume", (Texture) this.noiseVolume);
    Graphics.DrawMesh(this.starsPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Background) + 1f), Quaternion.identity, material, this.layer);
    if (this.LargeImpactorFragmentsVisible)
    {
      Vector3 position = new Vector3(CameraController.Instance.transform.position.x, CameraController.Instance.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.85f);
      if (!TerrainBG.preventLargeImpactorFragmentsFromProgressing && (double) this.LargeImpactorEntryProgress < 1.0)
      {
        if ((double) this.LargeImpactorEntryProgress < 0.0)
        {
          this.LargeImpactorEntryProgress = 0.0f;
          this.largeImpactorFragmentsMaterial.SetFloat("_LargeImpactorScale", this.LargeImpactorBackgroundScale);
        }
        if (!SpeedControlScreen.Instance.IsPaused)
        {
          if ((double) this.LargeImpactorEntryProgress == 0.0)
            KFMOD.PlayUISound(GlobalAssets.GetSound("Asteroid_destroyed_end"));
          this.LargeImpactorEntryProgress += Time.unscaledDeltaTime / 2.5f;
        }
        this.LargeImpactorEntryProgress = Mathf.Clamp01(this.LargeImpactorEntryProgress);
        this.largeImpactorFragmentsMaterial.SetFloat("_EntryProgress", this.LargeImpactorEntryProgress);
      }
      this.largeImpactorFragmentsMaterial.SetFloat("_UnscaledTime", Time.timeSinceLevelLoad);
      Graphics.DrawMesh(this.largeImpactorDefeatedPlane, position, Quaternion.identity, this.largeImpactorFragmentsMaterial, this.layer);
    }
    if ((Object) ClusterManager.Instance.activeWorld != (Object) null && ClusterManager.Instance.activeWorld.northernlights > 0)
      Graphics.DrawMesh(this.northernLightsPlane, new Vector3(CameraController.Instance.transform.position.x, CameraController.Instance.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.8f), Quaternion.identity, this.northernLightMaterial_ceres, this.layer);
    this.backgroundMaterial.renderQueue = RenderQueues.Backwall;
    for (int index = 0; index < Lighting.Instance.Settings.BackgroundLayers; ++index)
    {
      if (index >= Lighting.Instance.Settings.BackgroundLayers - 1)
      {
        float t = (float) index / (float) (Lighting.Instance.Settings.BackgroundLayers - 1);
        float x = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundDarkening, t);
        float z = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundUVScale, t);
        float w = 1f;
        if (index == Lighting.Instance.Settings.BackgroundLayers - 1)
          w = 0.0f;
        MaterialPropertyBlock propertyBlock = this.propertyBlocks[index];
        propertyBlock.SetVector("_BackWallParameters", new Vector4(x, Lighting.Instance.Settings.BackgroundClip, z, w));
        propertyBlock.SetVectorArray("_ZoneTextureParallaxData", SubworldZoneRenderData.zoneTextureParallaxData);
        int num = (Object) ClusterManager.Instance.activeWorld != (Object) null ? 1 : 0;
        Graphics.DrawMesh(this.worldPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Background)), Quaternion.identity, this.backgroundMaterial, this.layer, (Camera) null, 0, propertyBlock);
      }
    }
    this.gasMaterial.renderQueue = RenderQueues.Gas;
    Graphics.DrawMesh(this.gasPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Gas)), Quaternion.identity, this.gasMaterial, this.layer);
    Graphics.DrawMesh(this.gasPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.GasFront)), Quaternion.identity, this.gasMaterial, this.layer);
  }
}
