// Decompiled with JetBrains decompiler
// Type: WaterCubes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/WaterCubes")]
public class WaterCubes : KMonoBehaviour
{
  public Material material;
  public Texture2D waveTexture;
  public MeshRenderer waterRenderer;
  public LiquidShaderProperties liquidShaderProperties;
  public static Color MOLTEN_METAL_COLOR = Color.white;
  private GameObject cubes;

  public static WaterCubes Instance { get; private set; }

  public static void DestroyInstance() => WaterCubes.Instance = (WaterCubes) null;

  protected override void OnPrefabInit()
  {
    WaterCubes.Instance = this;
    WaterCubes.MOLTEN_METAL_COLOR = this.material.GetColor("_MoltenMetalColor");
  }

  public void Init()
  {
    this.cubes = Util.NewGameObject(this.gameObject, nameof (WaterCubes));
    GameObject gameObject = new GameObject();
    gameObject.name = "WaterCubesMesh";
    gameObject.transform.parent = this.cubes.transform;
    this.material.renderQueue = RenderQueues.Liquid;
    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
    this.waterRenderer = gameObject.AddComponent<MeshRenderer>();
    this.waterRenderer.sharedMaterial = this.material;
    this.waterRenderer.shadowCastingMode = ShadowCastingMode.Off;
    this.waterRenderer.receiveShadows = false;
    this.waterRenderer.lightProbeUsage = LightProbeUsage.Off;
    this.waterRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    this.waterRenderer.sharedMaterial.SetTexture("_MainTex2", (Texture) this.waveTexture);
    Mesh newMesh = this.CreateNewMesh();
    meshFilter.sharedMesh = newMesh;
    this.waterRenderer.gameObject.layer = LayerMask.NameToLayer("Water");
    this.waterRenderer.gameObject.transform.parent = this.transform;
    this.waterRenderer.gameObject.transform.SetPosition(new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Liquid)));
    if (!((Object) this.liquidShaderProperties != (Object) null))
      return;
    this.liquidShaderProperties.ApplyToMaterial(this.material);
  }

  private void LateUpdate()
  {
    this.material.SetVector("_CursorWorldPosition", (Vector4) Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    if (!((Object) this.liquidShaderProperties != (Object) null))
      return;
    this.liquidShaderProperties.ApplyToMaterial(this.material);
  }

  private Mesh CreateNewMesh()
  {
    Mesh newMesh = new Mesh();
    newMesh.name = nameof (WaterCubes);
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    Vector3[] vector3Array2 = new Vector3[length];
    Vector4[] vector4Array1 = new Vector4[length];
    int[] numArray1 = new int[6];
    float layerZ = Grid.GetLayerZ(Grid.SceneLayer.Liquid);
    Vector3[] vector3Array3 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, layerZ),
      new Vector3((float) Grid.WidthInCells, 0.0f, layerZ),
      new Vector3(0.0f, Grid.HeightInMeters, layerZ),
      new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, layerZ)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    Vector3[] vector3Array4 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, -1f)
    };
    Vector4[] vector4Array2 = new Vector4[4]
    {
      new Vector4(0.0f, 1f, 0.0f, -1f),
      new Vector4(0.0f, 1f, 0.0f, -1f),
      new Vector4(0.0f, 1f, 0.0f, -1f),
      new Vector4(0.0f, 1f, 0.0f, -1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    newMesh.vertices = vector3Array3;
    newMesh.uv = vector2Array2;
    newMesh.uv2 = vector2Array2;
    newMesh.normals = vector3Array4;
    newMesh.tangents = vector4Array2;
    newMesh.triangles = numArray2;
    newMesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0.0f));
    return newMesh;
  }
}
