// Decompiled with JetBrains decompiler
// Type: SpriteSheetAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpriteSheetAnimator
{
  private SpriteSheet sheet;
  private Mesh mesh;
  private MaterialPropertyBlock materialProperties;
  private List<SpriteSheetAnimator.AnimInfo> anims = new List<SpriteSheetAnimator.AnimInfo>();
  private List<SpriteSheetAnimator.AnimInfo> rotatedAnims = new List<SpriteSheetAnimator.AnimInfo>();

  public SpriteSheetAnimator(SpriteSheet sheet)
  {
    this.sheet = sheet;
    this.mesh = new Mesh();
    this.mesh.name = nameof (SpriteSheetAnimator);
    this.mesh.MarkDynamic();
    this.materialProperties = new MaterialPropertyBlock();
    this.materialProperties.SetTexture("_MainTex", (Texture) sheet.texture);
  }

  public void Play(Vector3 pos, Quaternion rotation, Vector2 size, Color colour)
  {
    if (rotation == Quaternion.identity)
      this.anims.Add(new SpriteSheetAnimator.AnimInfo()
      {
        elapsedTime = 0.0f,
        pos = pos,
        rotation = rotation,
        size = size,
        colour = (Color32) colour
      });
    else
      this.rotatedAnims.Add(new SpriteSheetAnimator.AnimInfo()
      {
        elapsedTime = 0.0f,
        pos = pos,
        rotation = rotation,
        size = size,
        colour = (Color32) colour
      });
  }

  private void GetUVs(
    int frame,
    out Vector2 uv_bl,
    out Vector2 uv_br,
    out Vector2 uv_tl,
    out Vector2 uv_tr)
  {
    int num1 = frame / this.sheet.numXFrames;
    int num2;
    float x1 = (float) (num2 = frame % this.sheet.numXFrames) * this.sheet.uvFrameSize.x;
    float x2 = (float) (num2 + 1) * this.sheet.uvFrameSize.x;
    float y1 = (float) (1.0 - (double) (num1 + 1) * (double) this.sheet.uvFrameSize.y);
    float y2 = (float) (1.0 - (double) num1 * (double) this.sheet.uvFrameSize.y);
    uv_bl = new Vector2(x1, y1);
    uv_br = new Vector2(x2, y1);
    uv_tl = new Vector2(x1, y2);
    uv_tr = new Vector2(x2, y2);
  }

  public int GetFrameFromElapsedTime(float elapsed_time)
  {
    return Mathf.Min(this.sheet.numFrames, (int) ((double) elapsed_time / 0.033333335071802139));
  }

  public int GetFrameFromElapsedTimeLooping(float elapsed_time)
  {
    int elapsedTimeLooping = (int) ((double) elapsed_time / 0.033333335071802139);
    if (elapsedTimeLooping > this.sheet.numFrames)
      elapsedTimeLooping %= this.sheet.numFrames;
    return elapsedTimeLooping;
  }

  public void UpdateAnims(float dt)
  {
    this.UpdateAnims(dt, (IList<SpriteSheetAnimator.AnimInfo>) this.anims);
    this.UpdateAnims(dt, (IList<SpriteSheetAnimator.AnimInfo>) this.rotatedAnims);
  }

  private void UpdateAnims(float dt, IList<SpriteSheetAnimator.AnimInfo> anims)
  {
    int count = anims.Count;
    int index = 0;
    while (index < count)
    {
      SpriteSheetAnimator.AnimInfo anim = anims[index];
      anim.elapsedTime += dt;
      anim.frame = Mathf.Min(this.sheet.numFrames, (int) ((double) anim.elapsedTime / 0.033333335071802139));
      if (anim.frame >= this.sheet.numFrames)
      {
        --count;
        anims[index] = anims[count];
        anims.RemoveAt(count);
      }
      else
      {
        anims[index] = anim;
        ++index;
      }
    }
  }

  public void Render(List<SpriteSheetAnimator.AnimInfo> anim_infos, bool apply_rotation)
  {
    ListPool<Vector3, SpriteSheetAnimManager>.PooledList inVertices = ListPool<Vector3, SpriteSheetAnimManager>.Allocate();
    ListPool<Vector2, SpriteSheetAnimManager>.PooledList uvs = ListPool<Vector2, SpriteSheetAnimManager>.Allocate();
    ListPool<Color32, SpriteSheetAnimManager>.PooledList inColors = ListPool<Color32, SpriteSheetAnimManager>.Allocate();
    ListPool<int, SpriteSheetAnimManager>.PooledList triangles = ListPool<int, SpriteSheetAnimManager>.Allocate();
    this.mesh.Clear();
    if (apply_rotation)
    {
      int count = anim_infos.Count;
      for (int index = 0; index < count; ++index)
      {
        SpriteSheetAnimator.AnimInfo animInfo = anim_infos[index];
        Vector2 vector2 = animInfo.size * 0.5f;
        Vector3 vector3_1 = animInfo.rotation * (Vector3) -vector2;
        Vector3 vector3_2 = animInfo.rotation * (Vector3) new Vector2(vector2.x, -vector2.y);
        Vector3 vector3_3 = animInfo.rotation * (Vector3) new Vector2(-vector2.x, vector2.y);
        Vector3 vector3_4 = animInfo.rotation * (Vector3) vector2;
        inVertices.Add(animInfo.pos + vector3_1);
        inVertices.Add(animInfo.pos + vector3_2);
        inVertices.Add(animInfo.pos + vector3_4);
        inVertices.Add(animInfo.pos + vector3_3);
        Vector2 uv_bl;
        Vector2 uv_br;
        Vector2 uv_tl;
        Vector2 uv_tr;
        this.GetUVs(animInfo.frame, out uv_bl, out uv_br, out uv_tl, out uv_tr);
        uvs.Add(uv_bl);
        uvs.Add(uv_br);
        uvs.Add(uv_tr);
        uvs.Add(uv_tl);
        inColors.Add(animInfo.colour);
        inColors.Add(animInfo.colour);
        inColors.Add(animInfo.colour);
        inColors.Add(animInfo.colour);
        int num = index * 4;
        triangles.Add(num);
        triangles.Add(num + 1);
        triangles.Add(num + 2);
        triangles.Add(num);
        triangles.Add(num + 2);
        triangles.Add(num + 3);
      }
    }
    else
    {
      int count = anim_infos.Count;
      for (int index = 0; index < count; ++index)
      {
        SpriteSheetAnimator.AnimInfo animInfo = anim_infos[index];
        Vector2 vector2 = animInfo.size * 0.5f;
        Vector3 vector3_5 = (Vector3) -vector2;
        Vector3 vector3_6 = (Vector3) new Vector2(vector2.x, -vector2.y);
        Vector3 vector3_7 = (Vector3) new Vector2(-vector2.x, vector2.y);
        Vector3 vector3_8 = (Vector3) vector2;
        inVertices.Add(animInfo.pos + vector3_5);
        inVertices.Add(animInfo.pos + vector3_6);
        inVertices.Add(animInfo.pos + vector3_8);
        inVertices.Add(animInfo.pos + vector3_7);
        Vector2 uv_bl;
        Vector2 uv_br;
        Vector2 uv_tl;
        Vector2 uv_tr;
        this.GetUVs(animInfo.frame, out uv_bl, out uv_br, out uv_tl, out uv_tr);
        uvs.Add(uv_bl);
        uvs.Add(uv_br);
        uvs.Add(uv_tr);
        uvs.Add(uv_tl);
        inColors.Add(animInfo.colour);
        inColors.Add(animInfo.colour);
        inColors.Add(animInfo.colour);
        inColors.Add(animInfo.colour);
        int num = index * 4;
        triangles.Add(num);
        triangles.Add(num + 1);
        triangles.Add(num + 2);
        triangles.Add(num);
        triangles.Add(num + 2);
        triangles.Add(num + 3);
      }
    }
    this.mesh.SetVertices((List<Vector3>) inVertices);
    this.mesh.SetUVs(0, (List<Vector2>) uvs);
    this.mesh.SetColors((List<Color32>) inColors);
    this.mesh.SetTriangles((List<int>) triangles, 0);
    Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, this.sheet.material, this.sheet.renderLayer, (Camera) null, 0, this.materialProperties);
    triangles.Recycle();
    inColors.Recycle();
    uvs.Recycle();
    inVertices.Recycle();
  }

  public void Render()
  {
    this.Render(this.anims, false);
    this.Render(this.rotatedAnims, true);
  }

  public struct AnimInfo
  {
    public int frame;
    public float elapsedTime;
    public Vector3 pos;
    public Quaternion rotation;
    public Vector2 size;
    public Color32 colour;
  }
}
