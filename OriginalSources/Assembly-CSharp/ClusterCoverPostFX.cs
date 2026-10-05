// Decompiled with JetBrains decompiler
// Type: ClusterCoverPostFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClusterCoverPostFX : MonoBehaviour
{
  [SerializeField]
  private Shader shader;
  private Material material;
  private Camera myCamera;

  private void Awake()
  {
    if (!((Object) this.shader != (Object) null))
      return;
    this.material = new Material(this.shader);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.SetupUVs();
    Graphics.Blit((Texture) source, destination, this.material, 0);
  }

  private void SetupUVs()
  {
    if ((Object) this.myCamera == (Object) null)
    {
      this.myCamera = this.GetComponent<Camera>();
      if ((Object) this.myCamera == (Object) null)
        return;
    }
    Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
    float distance1 = Mathf.Abs(ray.origin.z / ray.direction.z);
    Vector3 point1 = ray.GetPoint(distance1);
    ray = this.myCamera.ViewportPointToRay(Vector3.one);
    float distance2 = Mathf.Abs(ray.origin.z / ray.direction.z);
    Vector3 point2 = ray.GetPoint(distance2);
    Vector4 vector4_1;
    vector4_1.x = point1.x;
    vector4_1.y = point1.y;
    vector4_1.z = point2.x - point1.x;
    vector4_1.w = point2.y - point1.y;
    this.material.SetVector("_CameraCoords", vector4_1);
    Vector4 vector4_2;
    if ((Object) ClusterManager.Instance != (Object) null && !CameraController.Instance.ignoreClusterFX)
    {
      WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
      Vector2I worldOffset = activeWorld.WorldOffset;
      Vector2I worldSize = activeWorld.WorldSize;
      vector4_2 = new Vector4((float) worldOffset.x, (float) worldOffset.y, (float) worldSize.x, (float) worldSize.y);
      this.material.SetFloat("_HideSurface", ClusterManager.Instance.activeWorld.FullyEnclosedBorder ? 1f : 0.0f);
    }
    else
    {
      vector4_2 = new Vector4(0.0f, 0.0f, (float) Grid.WidthInCells, (float) Grid.HeightInCells);
      this.material.SetFloat("_HideSurface", 0.0f);
    }
    this.material.SetVector("_WorldCoords", vector4_2);
  }
}
