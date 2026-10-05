// Decompiled with JetBrains decompiler
// Type: GroundRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/GroundRenderer")]
public class GroundRenderer : KMonoBehaviour
{
  [SerializeField]
  private GroundMasks masks;
  private GroundMasks.BiomeMaskData[] biomeMasks;
  private Dictionary<SimHashes, GroundRenderer.Materials> elementMaterials = new Dictionary<SimHashes, GroundRenderer.Materials>();
  private bool[,] dirtyChunks;
  private GroundRenderer.WorldChunk[,] worldChunks;
  private const int ChunkEdgeSize = 16 /*0x10*/;
  private Vector2I size;
  private static int SHINE_COLOR = Shader.PropertyToID("_ShineColour");

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
    this.OnShadersReloaded();
    this.masks.Initialize();
    SubWorld.ZoneType[] values = (SubWorld.ZoneType[]) Enum.GetValues(typeof (SubWorld.ZoneType));
    this.biomeMasks = new GroundMasks.BiomeMaskData[values.Length];
    for (int index = 0; index < values.Length; ++index)
    {
      SubWorld.ZoneType zone_type = values[index];
      this.biomeMasks[index] = this.GetBiomeMask(zone_type);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.size = new Vector2I((Grid.WidthInCells + 16 /*0x10*/ - 1) / 16 /*0x10*/, (Grid.HeightInCells + 16 /*0x10*/ - 1) / 16 /*0x10*/);
    this.dirtyChunks = new bool[this.size.x, this.size.y];
    this.worldChunks = new GroundRenderer.WorldChunk[this.size.x, this.size.y];
    for (int y = 0; y < this.size.y; ++y)
    {
      for (int x = 0; x < this.size.x; ++x)
      {
        this.worldChunks[x, y] = new GroundRenderer.WorldChunk(x, y);
        this.dirtyChunks[x, y] = true;
      }
    }
  }

  public void Render(Vector2I vis_min, Vector2I vis_max, bool forceVisibleRebuild = false)
  {
    if (!this.enabled)
      return;
    int layer = LayerMask.NameToLayer("World");
    Vector2I vector2I1 = new Vector2I(vis_min.x / 16 /*0x10*/, vis_min.y / 16 /*0x10*/);
    Vector2I vector2I2 = new Vector2I((vis_max.x + 16 /*0x10*/ - 1) / 16 /*0x10*/, (vis_max.y + 16 /*0x10*/ - 1) / 16 /*0x10*/);
    for (int y = vector2I1.y; y < vector2I2.y; ++y)
    {
      for (int x = vector2I1.x; x < vector2I2.x; ++x)
      {
        GroundRenderer.WorldChunk worldChunk = this.worldChunks[x, y];
        if (this.dirtyChunks[x, y] | forceVisibleRebuild)
        {
          this.dirtyChunks[x, y] = false;
          worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
        }
        worldChunk.Render(layer);
      }
    }
    this.RebuildDirtyChunks();
  }

  public void RenderAll()
  {
    this.Render(new Vector2I(0, 0), new Vector2I(this.worldChunks.GetLength(0) * 16 /*0x10*/, this.worldChunks.GetLength(1) * 16 /*0x10*/), true);
  }

  private void RebuildDirtyChunks()
  {
    for (int index1 = 0; index1 < this.dirtyChunks.GetLength(1); ++index1)
    {
      for (int index2 = 0; index2 < this.dirtyChunks.GetLength(0); ++index2)
      {
        if (this.dirtyChunks[index2, index1])
        {
          this.dirtyChunks[index2, index1] = false;
          this.worldChunks[index2, index1].Rebuild(this.biomeMasks, this.elementMaterials);
        }
      }
    }
  }

  public void MarkDirty(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    Vector2I vector2I = new Vector2I(xy.x / 16 /*0x10*/, xy.y / 16 /*0x10*/);
    this.dirtyChunks[vector2I.x, vector2I.y] = true;
    int num = xy.x % 16 /*0x10*/ != 0 ? 0 : (vector2I.x > 0 ? 1 : 0);
    bool flag1 = xy.x % 16 /*0x10*/ == 15 && vector2I.x < this.size.x - 1;
    bool flag2 = xy.y % 16 /*0x10*/ == 0 && vector2I.y > 0;
    bool flag3 = xy.y % 16 /*0x10*/ == 15 && vector2I.y < this.size.y - 1;
    if (num != 0)
    {
      this.dirtyChunks[vector2I.x - 1, vector2I.y] = true;
      if (flag2)
        this.dirtyChunks[vector2I.x - 1, vector2I.y - 1] = true;
      if (flag3)
        this.dirtyChunks[vector2I.x - 1, vector2I.y + 1] = true;
    }
    if (flag2)
      this.dirtyChunks[vector2I.x, vector2I.y - 1] = true;
    if (flag3)
      this.dirtyChunks[vector2I.x, vector2I.y + 1] = true;
    if (!flag1)
      return;
    this.dirtyChunks[vector2I.x + 1, vector2I.y] = true;
    if (flag2)
      this.dirtyChunks[vector2I.x + 1, vector2I.y - 1] = true;
    if (!flag3)
      return;
    this.dirtyChunks[vector2I.x + 1, vector2I.y + 1] = true;
  }

  private Vector2I GetChunkIdx(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    return new Vector2I(xy.x / 16 /*0x10*/, xy.y / 16 /*0x10*/);
  }

  private GroundMasks.BiomeMaskData GetBiomeMask(SubWorld.ZoneType zone_type)
  {
    GroundMasks.BiomeMaskData biomeMask = (GroundMasks.BiomeMaskData) null;
    this.masks.biomeMasks.TryGetValue(zone_type.ToString().ToLower(), out biomeMask);
    return biomeMask;
  }

  private void InitOpaqueMaterial(Material material, Element element)
  {
    material.name = element.id.ToString() + "_opaque";
    material.renderQueue = RenderQueues.WorldOpaque;
    material.EnableKeyword("OPAQUE");
    material.DisableKeyword("ALPHA");
    this.ConfigureMaterialShine(material);
    material.SetTexture("_AlphaTestMap", (Texture) Texture2D.whiteTexture);
    material.SetInt("_IsBackwallEdge", 0);
    material.SetInt("_SrcAlpha", 1);
    material.SetInt("_DstAlpha", 0);
    material.SetInt("_ZWrite", 1);
  }

  private void InitAlphaMaterial(Material material, Element element)
  {
    material.name = element.id.ToString() + "_alpha";
    material.renderQueue = RenderQueues.WorldTransparent;
    material.EnableKeyword("ALPHA");
    material.DisableKeyword("OPAQUE");
    this.ConfigureMaterialShine(material);
    material.SetTexture("_AlphaTestMap", (Texture) this.masks.maskAtlas.texture);
    material.SetInt("_IsBackwallEdge", 0);
    material.SetInt("_SrcAlpha", 5);
    material.SetInt("_DstAlpha", 10);
    material.SetInt("_ZWrite", 0);
  }

  private void InitBackwallMaterial(Material material, Element element)
  {
    material.name = element.id.ToString() + "_backwall";
    material.renderQueue = RenderQueues.NaturalBackwall;
    material.EnableKeyword("OPAQUE");
    material.DisableKeyword("ALPHA");
    this.ConfigureMaterialShine(material);
    material.SetTexture("_AlphaTestMap", (Texture) Texture2D.grayTexture);
    material.SetInt("_IsBackwallEdge", 0);
    material.SetInt("_StencilPass", 0);
    material.SetInt("_SrcAlpha", 5);
    material.SetInt("_DstAlpha", 10);
    material.SetInt("_ZWrite", 0);
  }

  private void InitAlphaBackwallMaterial(Material material, Element element)
  {
    material.name = element.id.ToString() + "_backwall_alpha";
    material.renderQueue = RenderQueues.BackwallTransparent;
    material.EnableKeyword("ALPHA");
    material.DisableKeyword("OPAQUE");
    this.ConfigureMaterialShine(material);
    material.SetTexture("_AlphaTestMap", (Texture) this.masks.maskAtlas.texture);
    material.SetInt("_IsBackwallEdge", 1);
    material.SetInt("_StencilPass", 0);
    material.SetInt("_SrcAlpha", 5);
    material.SetInt("_DstAlpha", 10);
    material.SetInt("_ZWrite", 0);
  }

  private void ConfigureMaterialShine(Material material)
  {
    if ((UnityEngine.Object) material.GetTexture("_ShineMask") != (UnityEngine.Object) null)
    {
      material.DisableKeyword("MATTE");
      material.EnableKeyword("SHINY");
    }
    else
    {
      material.EnableKeyword("MATTE");
      material.DisableKeyword("SHINY");
    }
  }

  [ContextMenu("Reload Shaders")]
  public void OnShadersReloaded()
  {
    this.FreeMaterials();
    foreach (Element element in ElementLoader.elements)
    {
      if (element.IsSolid)
      {
        if ((UnityEngine.Object) element.substance.material == (UnityEngine.Object) null)
          DebugUtil.LogErrorArgs((object) element.name, (object) "must have material associated with it in the substance table");
        Material material1 = new Material(element.substance.material);
        this.InitOpaqueMaterial(material1, element);
        Material material2 = new Material(material1);
        this.InitAlphaMaterial(material2, element);
        Material material3 = new Material(material1);
        this.InitBackwallMaterial(material3, element);
        Material material4 = new Material(material1);
        this.InitAlphaBackwallMaterial(material4, element);
        GroundRenderer.Materials materials = new GroundRenderer.Materials(material1, material2, material3, material4);
        this.elementMaterials[element.id] = materials;
      }
    }
    if (this.worldChunks == null)
      return;
    for (int index1 = 0; index1 < this.dirtyChunks.GetLength(1); ++index1)
    {
      for (int index2 = 0; index2 < this.dirtyChunks.GetLength(0); ++index2)
        this.dirtyChunks[index2, index1] = true;
    }
    GroundRenderer.WorldChunk[,] worldChunks = this.worldChunks;
    int upperBound1 = worldChunks.GetUpperBound(0);
    int upperBound2 = worldChunks.GetUpperBound(1);
    for (int lowerBound1 = worldChunks.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
    {
      for (int lowerBound2 = worldChunks.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
      {
        GroundRenderer.WorldChunk worldChunk = worldChunks[lowerBound1, lowerBound2];
        worldChunk.Clear();
        worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
      }
    }
  }

  public void SetShineColors(SimHashes element, Color centerColor, Color edgeColor)
  {
    GroundRenderer.Materials materials;
    if (!this.elementMaterials.TryGetValue(element, out materials))
      return;
    materials.alpha.SetColor(GroundRenderer.SHINE_COLOR, edgeColor);
    materials.opaque.SetColor(GroundRenderer.SHINE_COLOR, centerColor);
  }

  public void FreeResources()
  {
    this.FreeMaterials();
    this.elementMaterials.Clear();
    if (this.worldChunks == null)
      return;
    GroundRenderer.WorldChunk[,] worldChunks = this.worldChunks;
    int upperBound1 = worldChunks.GetUpperBound(0);
    int upperBound2 = worldChunks.GetUpperBound(1);
    for (int lowerBound1 = worldChunks.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
    {
      for (int lowerBound2 = worldChunks.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
        worldChunks[lowerBound1, lowerBound2].FreeResources();
    }
    this.worldChunks = (GroundRenderer.WorldChunk[,]) null;
  }

  private void FreeMaterials()
  {
    foreach (GroundRenderer.Materials materials in this.elementMaterials.Values)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) materials.opaque);
      UnityEngine.Object.Destroy((UnityEngine.Object) materials.alpha);
      UnityEngine.Object.Destroy((UnityEngine.Object) materials.backwall);
      UnityEngine.Object.Destroy((UnityEngine.Object) materials.backwallAlpha);
    }
    this.elementMaterials.Clear();
  }

  [Serializable]
  private struct Materials(
    Material opaque,
    Material alpha,
    Material backwall,
    Material backwallAlpha)
  {
    public Material opaque = opaque;
    public Material alpha = alpha;
    public Material backwall = backwall;
    public Material backwallAlpha = backwallAlpha;
  }

  private class ElementChunk
  {
    public SimHashes element;
    private GroundRenderer.ElementChunk.RenderData alpha;
    private GroundRenderer.ElementChunk.RenderData opaque;
    private GroundRenderer.ElementChunk.RenderData backwall;
    private GroundRenderer.ElementChunk.RenderData backwallAlpha;
    public int tileCount;

    public ElementChunk(
      SimHashes element,
      Dictionary<SimHashes, GroundRenderer.Materials> materials)
    {
      this.element = element;
      GroundRenderer.Materials material = materials[element];
      this.alpha = new GroundRenderer.ElementChunk.RenderData(material.alpha);
      this.opaque = new GroundRenderer.ElementChunk.RenderData(material.opaque);
      this.backwall = new GroundRenderer.ElementChunk.RenderData(material.backwall);
      this.backwallAlpha = new GroundRenderer.ElementChunk.RenderData(material.backwallAlpha);
      this.Clear();
    }

    public void Clear()
    {
      this.opaque.Clear();
      this.alpha.Clear();
      this.backwall.Clear();
      this.backwallAlpha.Clear();
      this.tileCount = 0;
    }

    public void AddOpaqueQuad(int x, int y, GroundMasks.UVData uvs)
    {
      this.opaque.AddQuad(x, y, uvs);
      ++this.tileCount;
    }

    public void AddAlphaQuad(int x, int y, GroundMasks.UVData uvs)
    {
      this.alpha.AddQuad(x, y, uvs);
      ++this.tileCount;
    }

    public void AddBackwallQuad(int x, int y, GroundMasks.UVData uvs)
    {
      this.backwall.AddQuad(x, y, uvs);
      ++this.tileCount;
    }

    public void AddBackwallAlphaQuad(int x, int y, GroundMasks.UVData uvs)
    {
      this.backwallAlpha.AddQuad(x, y, uvs);
      ++this.tileCount;
    }

    public void Build()
    {
      this.backwall.Build();
      this.opaque.Build();
      this.alpha.Build();
      this.backwallAlpha.Build();
    }

    public void Render(int layer, int element_idx)
    {
      float z1 = Grid.GetLayerZ(Grid.SceneLayer.Ground) - 0.0001f * (float) element_idx;
      float z2 = Grid.GetLayerZ(Grid.SceneLayer.Backwall) - 0.0001f * (float) element_idx;
      this.backwall.Render(new Vector3(0.0f, 0.0f, z2), layer);
      this.backwallAlpha.Render(new Vector3(0.0f, 0.0f, z2), layer);
      this.opaque.Render(new Vector3(0.0f, 0.0f, z1), layer);
      this.alpha.Render(new Vector3(0.0f, 0.0f, z1), layer);
    }

    public void FreeResources()
    {
      this.alpha.FreeResources();
      this.opaque.FreeResources();
      this.backwall.FreeResources();
      this.backwallAlpha.FreeResources();
      this.alpha = (GroundRenderer.ElementChunk.RenderData) null;
      this.opaque = (GroundRenderer.ElementChunk.RenderData) null;
      this.backwall = (GroundRenderer.ElementChunk.RenderData) null;
      this.backwallAlpha = (GroundRenderer.ElementChunk.RenderData) null;
    }

    private class RenderData
    {
      public Material material;
      public Mesh mesh;
      public List<Vector3> pos;
      public List<Vector2> uv;
      public List<int> indices;

      public RenderData(Material material)
      {
        this.material = material;
        this.mesh = new Mesh();
        this.mesh.MarkDynamic();
        this.mesh.name = nameof (ElementChunk);
        this.pos = new List<Vector3>();
        this.uv = new List<Vector2>();
        this.indices = new List<int>();
      }

      public void ClearMesh()
      {
        if (!((UnityEngine.Object) this.mesh != (UnityEngine.Object) null))
          return;
        this.mesh.Clear();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mesh);
        this.mesh = (Mesh) null;
      }

      public void Clear()
      {
        if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) null)
          this.mesh.Clear();
        if (this.pos != null)
          this.pos.Clear();
        if (this.uv != null)
          this.uv.Clear();
        if (this.indices == null)
          return;
        this.indices.Clear();
      }

      public void FreeResources()
      {
        this.ClearMesh();
        this.Clear();
        this.pos = (List<Vector3>) null;
        this.uv = (List<Vector2>) null;
        this.indices = (List<int>) null;
        this.material = (Material) null;
      }

      public void Build()
      {
        this.mesh.Clear();
        if (this.pos.Count == 0)
          return;
        this.mesh.SetVertices(this.pos);
        this.mesh.SetUVs(0, this.uv);
        this.mesh.SetTriangles(this.indices, 0);
      }

      public void AddQuad(int x, int y, GroundMasks.UVData uvs)
      {
        int count = this.pos.Count;
        this.indices.Add(count);
        this.indices.Add(count + 1);
        this.indices.Add(count + 3);
        this.indices.Add(count);
        this.indices.Add(count + 3);
        this.indices.Add(count + 2);
        this.pos.Add(new Vector3((float) x - 0.5f, (float) y - 0.5f, 0.0f));
        this.pos.Add(new Vector3((float) ((double) x + 1.0 - 0.5), (float) y - 0.5f, 0.0f));
        this.pos.Add(new Vector3((float) x - 0.5f, (float) ((double) y + 1.0 - 0.5), 0.0f));
        this.pos.Add(new Vector3((float) ((double) x + 1.0 - 0.5), (float) ((double) y + 1.0 - 0.5), 0.0f));
        this.uv.Add(uvs.bl);
        this.uv.Add(uvs.br);
        this.uv.Add(uvs.tl);
        this.uv.Add(uvs.tr);
      }

      public void Render(Vector3 position, int layer)
      {
        if (this.pos.Count == 0)
          return;
        Graphics.DrawMesh(this.mesh, position, Quaternion.identity, this.material, layer, (Camera) null, 0, (MaterialPropertyBlock) null, ShadowCastingMode.Off, false, (Transform) null, false);
      }
    }
  }

  private struct WorldChunk(int x, int y)
  {
    public readonly int chunkX = x;
    public readonly int chunkY = y;
    private List<GroundRenderer.ElementChunk> elementChunks = new List<GroundRenderer.ElementChunk>();
    private static Element[] elements = new Element[4];
    private static Element[] uniqueElements = new Element[4];
    private static int[] substances = new int[4];
    private static GroundRenderer.WorldChunk.BiomeMaskCheck[] biomeChecks = new GroundRenderer.WorldChunk.BiomeMaskCheck[2]
    {
      new GroundRenderer.WorldChunk.BiomeMaskCheck((Func<int, Element>) (cell => Grid.Element[cell]), (Func<Element, int, bool>) ((e, cell) => (cell == Grid.InvalidCell || Grid.RenderedByWorld[cell]) && e.IsSolid), (Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData>) ((ec, a, b, c) => ec.AddOpaqueQuad(a, b, c)), (Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData>) ((ec, a, b, c) => ec.AddAlphaQuad(a, b, c))),
      new GroundRenderer.WorldChunk.BiomeMaskCheck((Func<int, Element>) (cell => BackwallManager.At(cell).Element ?? ElementLoader.FindElementByHash(SimHashes.Vacuum)), (Func<Element, int, bool>) ((e, cell) => e.id != SimHashes.Vacuum), (Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData>) ((ec, a, b, c) => ec.AddBackwallQuad(a, b, c)), (Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData>) ((ec, a, b, c) => ec.AddBackwallAlphaQuad(a, b, c)))
    };
    private static Vector2 NoiseScale = (Vector2) new Vector3(1f, 1f);

    public void Clear() => this.elementChunks.Clear();

    private static void InsertSorted(Element element, Element[] array, int size)
    {
      int id = (int) element.id;
      for (int index = 0; index < size; ++index)
      {
        Element element1 = array[index];
        if (element1.id > (SimHashes) id)
        {
          array[index] = element;
          element = element1;
          id = (int) element1.id;
        }
      }
      array[size] = element;
    }

    public void Rebuild(
      GroundMasks.BiomeMaskData[] biomeMasks,
      Dictionary<SimHashes, GroundRenderer.Materials> materials)
    {
      foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
        elementChunk.Clear();
      Vector2I vector2I1 = new Vector2I(this.chunkX * 16 /*0x10*/, this.chunkY * 16 /*0x10*/);
      Vector2I vector2I2 = new Vector2I(Math.Min(Grid.WidthInCells, (this.chunkX + 1) * 16 /*0x10*/), Math.Min(Grid.HeightInCells, (this.chunkY + 1) * 16 /*0x10*/));
      for (int y = vector2I1.y; y < vector2I2.y; ++y)
      {
        int num1 = Math.Max(0, y - 1);
        int num2 = y;
        for (int x = vector2I1.x; x < vector2I2.x; ++x)
        {
          int num3 = Math.Max(0, x - 1);
          int num4 = x;
          int num5 = num1 * Grid.WidthInCells + num3;
          int num6 = num1 * Grid.WidthInCells + num4;
          int num7 = num2 * Grid.WidthInCells + num3;
          int num8 = num2 * Grid.WidthInCells + num4;
          for (int index1 = 0; index1 < GroundRenderer.WorldChunk.biomeChecks.Length; ++index1)
          {
            GroundRenderer.WorldChunk.BiomeMaskCheck biomeCheck = GroundRenderer.WorldChunk.biomeChecks[index1];
            GroundRenderer.WorldChunk.elements[0] = biomeCheck.elementGet(num5);
            GroundRenderer.WorldChunk.elements[1] = biomeCheck.elementGet(num6);
            GroundRenderer.WorldChunk.elements[2] = biomeCheck.elementGet(num7);
            GroundRenderer.WorldChunk.elements[3] = biomeCheck.elementGet(num8);
            GroundRenderer.WorldChunk.substances[0] = biomeCheck.elementCheck(GroundRenderer.WorldChunk.elements[0], num5) ? GroundRenderer.WorldChunk.elements[0].substance.idx : -1;
            GroundRenderer.WorldChunk.substances[1] = biomeCheck.elementCheck(GroundRenderer.WorldChunk.elements[1], num6) ? GroundRenderer.WorldChunk.elements[1].substance.idx : -1;
            GroundRenderer.WorldChunk.substances[2] = biomeCheck.elementCheck(GroundRenderer.WorldChunk.elements[2], num7) ? GroundRenderer.WorldChunk.elements[2].substance.idx : -1;
            GroundRenderer.WorldChunk.substances[3] = biomeCheck.elementCheck(GroundRenderer.WorldChunk.elements[3], num8) ? GroundRenderer.WorldChunk.elements[3].substance.idx : -1;
            GroundRenderer.WorldChunk.uniqueElements[0] = GroundRenderer.WorldChunk.elements[0];
            GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[1], GroundRenderer.WorldChunk.uniqueElements, 1);
            GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[2], GroundRenderer.WorldChunk.uniqueElements, 2);
            GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[3], GroundRenderer.WorldChunk.uniqueElements, 3);
            int num9 = -1;
            int biomeIdx = GroundRenderer.WorldChunk.GetBiomeIdx(y * Grid.WidthInCells + x);
            GroundMasks.BiomeMaskData biomeMaskData = biomeMasks[biomeIdx] ?? biomeMasks[3];
            for (int index2 = 0; index2 < GroundRenderer.WorldChunk.uniqueElements.Length; ++index2)
            {
              Element uniqueElement = GroundRenderer.WorldChunk.uniqueElements[index2];
              if (biomeCheck.elementCheck(uniqueElement, Grid.InvalidCell))
              {
                int idx = uniqueElement.substance.idx;
                if (idx != num9)
                {
                  num9 = idx;
                  int index3 = (GroundRenderer.WorldChunk.substances[2] >= idx ? 1 : 0) << 3 | (GroundRenderer.WorldChunk.substances[3] >= idx ? 1 : 0) << 2 | (GroundRenderer.WorldChunk.substances[0] >= idx ? 1 : 0) << 1 | (GroundRenderer.WorldChunk.substances[1] >= idx ? 1 : 0);
                  if (index3 > 0)
                  {
                    GroundMasks.UVData[] variationUvs = biomeMaskData.tiles[index3].variationUVs;
                    float staticRandom = GroundRenderer.WorldChunk.GetStaticRandom(x, y);
                    int num10 = Mathf.Min(variationUvs.Length - 1, (int) ((double) variationUvs.Length * (double) staticRandom));
                    GroundMasks.UVData uvData = variationUvs[num10 % variationUvs.Length];
                    GroundRenderer.ElementChunk elementChunk = this.GetElementChunk(uniqueElement.id, materials);
                    if (index3 == 15)
                      biomeCheck.opaqueAction(elementChunk, x, y, uvData);
                    else
                      biomeCheck.alphaAction(elementChunk, x, y, uvData);
                  }
                }
              }
            }
          }
        }
      }
      foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
        elementChunk.Build();
      for (int index4 = this.elementChunks.Count - 1; index4 >= 0; --index4)
      {
        if (this.elementChunks[index4].tileCount == 0)
        {
          int index5 = this.elementChunks.Count - 1;
          this.elementChunks[index4] = this.elementChunks[index5];
          this.elementChunks.RemoveAt(index5);
        }
      }
    }

    private GroundRenderer.ElementChunk GetElementChunk(
      SimHashes elementID,
      Dictionary<SimHashes, GroundRenderer.Materials> materials)
    {
      GroundRenderer.ElementChunk elementChunk = (GroundRenderer.ElementChunk) null;
      for (int index = 0; index < this.elementChunks.Count; ++index)
      {
        if (this.elementChunks[index].element == elementID)
        {
          elementChunk = this.elementChunks[index];
          break;
        }
      }
      if (elementChunk == null)
      {
        elementChunk = new GroundRenderer.ElementChunk(elementID, materials);
        this.elementChunks.Add(elementChunk);
      }
      return elementChunk;
    }

    private static int GetBiomeIdx(int cell)
    {
      if (!Grid.IsValidCell(cell))
        return 0;
      SubWorld.ZoneType biomeIdx = SubWorld.ZoneType.Sandstone;
      if ((UnityEngine.Object) World.Instance != (UnityEngine.Object) null && (UnityEngine.Object) World.Instance.zoneRenderData != (UnityEngine.Object) null)
        biomeIdx = World.Instance.zoneRenderData.GetSubWorldZoneType(cell);
      return (int) biomeIdx;
    }

    private static float GetStaticRandom(int x, int y)
    {
      return PerlinSimplexNoise.noise((float) x * GroundRenderer.WorldChunk.NoiseScale.x, (float) y * GroundRenderer.WorldChunk.NoiseScale.y);
    }

    public void Render(int layer)
    {
      for (int index = 0; index < this.elementChunks.Count; ++index)
      {
        GroundRenderer.ElementChunk elementChunk = this.elementChunks[index];
        elementChunk.Render(layer, ElementLoader.FindElementByHash(elementChunk.element).substance.idx);
      }
    }

    public void FreeResources()
    {
      foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
        elementChunk.FreeResources();
      this.elementChunks.Clear();
      this.elementChunks = (List<GroundRenderer.ElementChunk>) null;
    }

    public class BiomeMaskCheck
    {
      public Func<int, Element> elementGet;
      public Func<Element, int, bool> elementCheck;
      public Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData> opaqueAction;
      public Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData> alphaAction;

      public BiomeMaskCheck(
        Func<int, Element> elementGet,
        Func<Element, int, bool> checkFn,
        Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData> opaqueAction,
        Action<GroundRenderer.ElementChunk, int, int, GroundMasks.UVData> alphaAction)
      {
        this.elementGet = elementGet;
        this.elementCheck = checkFn;
        this.opaqueAction = opaqueAction;
        this.alphaAction = alphaAction;
      }
    }
  }
}
