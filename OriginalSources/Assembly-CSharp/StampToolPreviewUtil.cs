// Decompiled with JetBrains decompiler
// Type: StampToolPreviewUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class StampToolPreviewUtil
{
  public static readonly Color COLOR_OK = Color.white;
  public static readonly Color COLOR_ERROR = Color.red;
  public const float SOLID_VIS_ALPHA = 1f;
  public const float LIQUID_VIS_ALPHA = 1f;
  public const float GAS_VIS_ALPHA = 1f;
  public const float BACKGROUND_ALPHA = 1f;

  public static Material MakeMaterial(Texture texture)
  {
    Material material = new Material(Shader.Find("Sprites/Default"));
    material.SetTexture("_MainTex", texture);
    return material;
  }

  public static void MakeQuad(
    out GameObject gameObject,
    out MeshRenderer meshRenderer,
    float mesh_size,
    Vector4? uvBox = null)
  {
    gameObject = new GameObject();
    gameObject.layer = LayerMask.NameToLayer("Place");
    float x = mesh_size / 2f;
    float y = mesh_size / 2f;
    Mesh mesh1 = new Mesh();
    mesh1.vertices = new Vector3[4]
    {
      new Vector3(-x, -y, 0.0f),
      new Vector3(x, -y, 0.0f),
      new Vector3(-x, y, 0.0f),
      new Vector3(x, y, 0.0f)
    };
    mesh1.triangles = new int[6]{ 0, 2, 1, 2, 3, 1 };
    mesh1.normals = new Vector3[4]
    {
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward
    };
    Mesh mesh2 = mesh1;
    Vector2[] vector2Array;
    if (uvBox.HasValue)
      vector2Array = new Vector2[4]
      {
        new Vector2(uvBox.Value.x, uvBox.Value.w),
        new Vector2(uvBox.Value.z, uvBox.Value.w),
        new Vector2(uvBox.Value.x, uvBox.Value.y),
        new Vector2(uvBox.Value.z, uvBox.Value.y)
      };
    else
      vector2Array = new Vector2[4]
      {
        new Vector2(0.0f, 0.0f),
        new Vector2(1f, 0.0f),
        new Vector2(0.0f, 1f),
        new Vector2(1f, 1f)
      };
    mesh2.uv = vector2Array;
    Mesh mesh3 = mesh1;
    gameObject.AddComponent<MeshFilter>().mesh = mesh3;
    meshRenderer = gameObject.AddComponent<MeshRenderer>();
  }
}
