// Decompiled with JetBrains decompiler
// Type: ConduitFlowVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ConduitFlowVisualizer
{
  private ConduitFlow flowManager;
  private EventReference overlaySound;
  private bool showContents;
  private double animTime;
  private int layer;
  private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);
  private List<ConduitFlowVisualizer.AudioInfo> audioInfo;
  private HashSet<int> insulatedCells = new HashSet<int>();
  private HashSet<int> radiantCells = new HashSet<int>();
  private Game.ConduitVisInfo visInfo;
  private ConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;
  private ConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;
  private int highlightedCell = -1;
  private Color32 highlightColour = (Color32) new Color(0.2f, 0.2f, 0.2f, 0.2f);
  private ConduitFlowVisualizer.Tuning tuning;

  public ConduitFlowVisualizer(
    ConduitFlow flow_manager,
    Game.ConduitVisInfo vis_info,
    EventReference overlay_sound,
    ConduitFlowVisualizer.Tuning tuning)
  {
    this.flowManager = flow_manager;
    this.visInfo = vis_info;
    this.overlaySound = overlay_sound;
    this.tuning = tuning;
    this.movingBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
    this.staticBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
    ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.InitializeResources();
  }

  public void FreeResources()
  {
    this.movingBallMesh.Cleanup();
    this.staticBallMesh.Cleanup();
  }

  private float CalculateMassScale(float mass)
  {
    return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, (float) (((double) mass - (double) this.visInfo.overlayMassScaleRange.x) / ((double) this.visInfo.overlayMassScaleRange.y - (double) this.visInfo.overlayMassScaleRange.x)));
  }

  private Color32 GetContentsColor(Element element, Color32 default_color)
  {
    if (element == null)
      return default_color;
    return (Color32) ((element.IsMoltenMetal ? Color.Lerp((Color) element.substance.conduitColour, WaterCubes.MOLTEN_METAL_COLOR, 0.8f) : (Color) element.substance.conduitColour) with
    {
      a = (element.substance.Glows ? 1f : 0.5f)
    });
  }

  private Color32 GetTintColour()
  {
    return !this.showContents ? this.visInfo.tint : GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName);
  }

  private Color32 GetInsulatedTintColour()
  {
    return !this.showContents ? this.visInfo.insulatedTint : GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName);
  }

  private Color32 GetRadiantTintColour()
  {
    return !this.showContents ? this.visInfo.radiantTint : GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayRadiantTintName);
  }

  private Color32 GetCellTintColour(int cell)
  {
    return !this.insulatedCells.Contains(cell) ? (!this.radiantCells.Contains(cell) ? this.GetTintColour() : this.GetRadiantTintColour()) : this.GetInsulatedTintColour();
  }

  public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
  {
    this.animTime += (double) Time.deltaTime;
    if (trigger_audio)
    {
      if (this.audioInfo == null)
        this.audioInfo = new List<ConduitFlowVisualizer.AudioInfo>();
      for (int index = 0; index < this.audioInfo.Count; ++index)
      {
        ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[index] with
        {
          distance = float.PositiveInfinity,
          position = Vector3.zero
        };
        audioInfo.blobCount = (audioInfo.blobCount + 1) % 10;
        this.audioInfo[index] = audioInfo;
      }
    }
    if (this.tuning.renderMesh)
      this.RenderMesh(z, render_layer, lerp_percent, trigger_audio);
    if (!trigger_audio)
      return;
    this.TriggerAudio();
  }

  private void RenderMesh(float z, int render_layer, float lerp_percent, bool trigger_audio)
  {
    GridArea visibleArea = GridVisibleArea.GetVisibleArea();
    Vector2I min = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
    Vector2I max = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
    ConduitFlowVisualizer.RenderMeshContext context = new ConduitFlowVisualizer.RenderMeshContext(this, lerp_percent, min, max);
    if (context.visible_conduits.Count == 0)
    {
      context.Finish();
    }
    else
    {
      ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Reset(context);
      GlobalJobManager.Run((IWorkItemCollection) ConduitFlowVisualizer.RenderMeshBatchJob.Instance);
      float z1 = 0.0f;
      if (this.showContents)
        z1 = 1f;
      float w = (float) ((int) (this.animTime / (1.0 / (double) this.tuning.framesPerSecond)) % (int) this.tuning.spriteCount) * (1f / this.tuning.spriteCount);
      this.movingBallMesh.Begin();
      this.movingBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
      this.movingBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
      this.movingBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z1, w));
      this.movingBallMesh.SetVector("_Highlight", new Vector4((float) this.highlightColour.r / (float) byte.MaxValue, (float) this.highlightColour.g / (float) byte.MaxValue, (float) this.highlightColour.b / (float) byte.MaxValue, 0.0f));
      this.staticBallMesh.Begin();
      this.staticBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
      this.staticBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
      this.staticBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z1, 0.0f));
      this.staticBallMesh.SetVector("_Highlight", new Vector4((float) this.highlightColour.r / (float) byte.MaxValue, (float) this.highlightColour.g / (float) byte.MaxValue, (float) this.highlightColour.b / (float) byte.MaxValue, 0.0f));
      ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Finish(this.movingBallMesh, this.staticBallMesh, CameraController.Instance.transform.GetPosition(), trigger_audio ? this : (ConduitFlowVisualizer) null);
      this.movingBallMesh.End(z, this.layer);
      this.staticBallMesh.End(z, this.layer);
      ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Reset(ConduitFlowVisualizer.RenderMeshContext.EmptyContext);
    }
  }

  public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
  {
    this.showContents = show_contents;
    this.layer = show_contents & move_to_overlay_layer ? LayerMask.NameToLayer("MaskedOverlay") : 0;
  }

  private void AddAudioSource(ConduitFlow.Conduit conduit, Vector3 camera_pos)
  {
    UtilityNetwork network = this.flowManager.GetNetwork(conduit);
    if (network == null)
      return;
    Vector3 posCcc = Grid.CellToPosCCC(conduit.GetCell(this.flowManager), Grid.SceneLayer.Building);
    float num = Vector3.SqrMagnitude(posCcc - camera_pos);
    bool flag = false;
    for (int index = 0; index < this.audioInfo.Count; ++index)
    {
      ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[index];
      if (audioInfo.networkID == network.id)
      {
        if ((double) num < (double) audioInfo.distance)
        {
          audioInfo.distance = num;
          audioInfo.position = posCcc;
          this.audioInfo[index] = audioInfo;
        }
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.audioInfo.Add(new ConduitFlowVisualizer.AudioInfo()
    {
      networkID = network.id,
      position = posCcc,
      distance = num,
      blobCount = 0
    });
  }

  private void TriggerAudio()
  {
    if (SpeedControlScreen.Instance.IsPaused)
      return;
    CameraController instance1 = CameraController.Instance;
    int num1 = 0;
    List<ConduitFlowVisualizer.AudioInfo> audioInfoList = new List<ConduitFlowVisualizer.AudioInfo>();
    for (int index = 0; index < this.audioInfo.Count; ++index)
    {
      if (instance1.IsVisiblePos(this.audioInfo[index].position))
      {
        audioInfoList.Add(this.audioInfo[index]);
        ++num1;
      }
    }
    for (int index = 0; index < audioInfoList.Count; ++index)
    {
      ConduitFlowVisualizer.AudioInfo audioInfo = audioInfoList[index];
      if ((double) audioInfo.distance != double.PositiveInfinity)
      {
        EventInstance instance2 = SoundEvent.BeginOneShot(this.overlaySound, audioInfo.position with
        {
          z = 0.0f
        });
        int num2 = (int) instance2.setParameterByName("blobCount", (float) audioInfo.blobCount);
        int num3 = (int) instance2.setParameterByName("networkCount", (float) num1);
        SoundEvent.EndOneShot(instance2);
      }
    }
  }

  public void AddThermalConductivity(int cell, float conductivity)
  {
    if ((double) conductivity < 1.0)
    {
      this.insulatedCells.Add(cell);
    }
    else
    {
      if ((double) conductivity <= 1.0)
        return;
      this.radiantCells.Add(cell);
    }
  }

  public void RemoveThermalConductivity(int cell, float conductivity)
  {
    if ((double) conductivity < 1.0)
    {
      this.insulatedCells.Remove(cell);
    }
    else
    {
      if ((double) conductivity <= 1.0)
        return;
      this.radiantCells.Remove(cell);
    }
  }

  public void SetHighlightedCell(int cell) => this.highlightedCell = cell;

  [Serializable]
  public class Tuning
  {
    public bool renderMesh;
    public float size;
    public float spriteCount;
    public float framesPerSecond;
    public Texture2D backgroundTexture;
    public Texture2D foregroundTexture;
  }

  private class ConduitFlowMesh
  {
    private Mesh mesh;
    private Material material;
    private List<Vector3> positions = new List<Vector3>();
    private List<Vector4> uvs = new List<Vector4>();
    private List<int> triangles = new List<int>();
    private List<Color32> colors = new List<Color32>();
    private int quadIndex;

    public ConduitFlowMesh()
    {
      this.mesh = new Mesh();
      this.mesh.name = "ConduitMesh";
      this.material = new Material(Shader.Find("Klei/ConduitBall"));
    }

    public void AddQuad(
      Vector2 pos,
      Color32 color,
      float size,
      float is_foreground,
      float highlight,
      Vector2I uvbl,
      Vector2I uvtl,
      Vector2I uvbr,
      Vector2I uvtr)
    {
      float num = size * 0.5f;
      this.positions.Add(new Vector3(pos.x - num, pos.y - num, 0.0f));
      this.positions.Add(new Vector3(pos.x - num, pos.y + num, 0.0f));
      this.positions.Add(new Vector3(pos.x + num, pos.y - num, 0.0f));
      this.positions.Add(new Vector3(pos.x + num, pos.y + num, 0.0f));
      this.uvs.Add(new Vector4((float) uvbl.x, (float) uvbl.y, is_foreground, highlight));
      this.uvs.Add(new Vector4((float) uvtl.x, (float) uvtl.y, is_foreground, highlight));
      this.uvs.Add(new Vector4((float) uvbr.x, (float) uvbr.y, is_foreground, highlight));
      this.uvs.Add(new Vector4((float) uvtr.x, (float) uvtr.y, is_foreground, highlight));
      this.colors.Add(color);
      this.colors.Add(color);
      this.colors.Add(color);
      this.colors.Add(color);
      this.triangles.Add(this.quadIndex * 4);
      this.triangles.Add(this.quadIndex * 4 + 1);
      this.triangles.Add(this.quadIndex * 4 + 2);
      this.triangles.Add(this.quadIndex * 4 + 2);
      this.triangles.Add(this.quadIndex * 4 + 1);
      this.triangles.Add(this.quadIndex * 4 + 3);
      ++this.quadIndex;
    }

    public void SetTexture(string id, Texture2D texture)
    {
      this.material.SetTexture(id, (Texture) texture);
    }

    public void SetVector(string id, Vector4 data) => this.material.SetVector(id, data);

    public void Begin()
    {
      this.positions.Clear();
      this.uvs.Clear();
      this.triangles.Clear();
      this.colors.Clear();
      this.quadIndex = 0;
    }

    public void End(float z, int layer)
    {
      this.mesh.Clear();
      this.mesh.SetVertices(this.positions);
      this.mesh.SetUVs(0, this.uvs);
      this.mesh.SetColors(this.colors);
      this.mesh.SetTriangles(this.triangles, 0, false);
      Graphics.DrawMesh(this.mesh, new Vector3(ConduitFlowVisualizer.GRID_OFFSET.x, ConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
    }

    public void Cleanup()
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mesh);
      this.mesh = (Mesh) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.material);
      this.material = (Material) null;
    }
  }

  private struct AudioInfo
  {
    public int networkID;
    public int blobCount;
    public float distance;
    public Vector3 position;
  }

  private struct RenderMeshContext
  {
    public static ConduitFlowVisualizer.RenderMeshContext EmptyContext;
    public ListPool<int, ConduitFlowVisualizer>.PooledList visible_conduits;
    public ConduitFlowVisualizer outer;
    public float lerp_percent;

    public RenderMeshContext(
      ConduitFlowVisualizer outer,
      float lerp_percent,
      Vector2I min,
      Vector2I max)
    {
      this.outer = outer;
      this.lerp_percent = lerp_percent;
      this.visible_conduits = ListPool<int, ConduitFlowVisualizer>.Allocate();
      this.visible_conduits.Capacity = Math.Max(outer.flowManager.soaInfo.NumEntries, this.visible_conduits.Capacity);
      for (int idx = 0; idx != outer.flowManager.soaInfo.NumEntries; ++idx)
      {
        Vector2I xy = Grid.CellToXY(outer.flowManager.soaInfo.GetCell(idx));
        if (min <= xy && xy <= max)
          this.visible_conduits.Add(idx);
      }
    }

    public void Finish() => this.visible_conduits.Recycle();
  }

  private class RenderMeshPerThreadData
  {
    public List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball> moving_balls = new List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball>();
    public List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball> static_balls = new List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball>();
    public List<ConduitFlow.Conduit> moving_conduits = new List<ConduitFlow.Conduit>();

    public void Finish(
      ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh,
      ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh,
      Vector3 camera_pos,
      ConduitFlowVisualizer visualizer)
    {
      for (int index = 0; index != this.moving_balls.Count; ++index)
        this.moving_balls[index].Consume(moving_ball_mesh);
      this.moving_balls.Clear();
      for (int index = 0; index != this.static_balls.Count; ++index)
        this.static_balls[index].Consume(static_ball_mesh);
      this.static_balls.Clear();
      if (visualizer != null)
      {
        foreach (ConduitFlow.Conduit movingConduit in this.moving_conduits)
          visualizer.AddAudioSource(movingConduit, camera_pos);
      }
      this.moving_conduits.Clear();
    }

    public struct Ball(
      ConduitFlow.FlowDirections direction,
      Vector2 pos,
      Color32 color,
      float size,
      bool foreground,
      bool highlight)
    {
      private Vector2 pos = pos;
      private float size = size;
      private Color32 color = color;
      private ConduitFlow.FlowDirections direction = direction;
      private bool foreground = foreground;
      private bool highlight = highlight;
      private static Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack> uv_packs = new Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack>();

      public static void InitializeResources()
      {
        ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.None] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack()
        {
          bl = new Vector2I(0, 0),
          tl = new Vector2I(0, 1),
          br = new Vector2I(1, 0),
          tr = new Vector2I(1, 1)
        };
        ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Left] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack()
        {
          bl = new Vector2I(0, 0),
          tl = new Vector2I(0, 1),
          br = new Vector2I(1, 0),
          tr = new Vector2I(1, 1)
        };
        ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Right] = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Left];
        ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Up] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack()
        {
          bl = new Vector2I(1, 0),
          tl = new Vector2I(0, 0),
          br = new Vector2I(1, 1),
          tr = new Vector2I(0, 1)
        };
        ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Down] = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Up];
      }

      private static ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack GetUVPack(
        ConduitFlow.FlowDirections direction)
      {
        return ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[direction];
      }

      public void Consume(ConduitFlowVisualizer.ConduitFlowMesh mesh)
      {
        ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack uvPack = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.GetUVPack(this.direction);
        mesh.AddQuad(this.pos, this.color, this.size, this.foreground ? 1f : 0.0f, this.highlight ? 1f : 0.0f, uvPack.bl, uvPack.tl, uvPack.br, uvPack.tr);
      }

      private class UVPack
      {
        public Vector2I bl;
        public Vector2I tl;
        public Vector2I br;
        public Vector2I tr;
      }
    }
  }

  private class RenderMeshBatchJob : 
    WorkItemCollectionWithThreadContex<ConduitFlowVisualizer.RenderMeshContext, ConduitFlowVisualizer.RenderMeshPerThreadData>
  {
    private const int kBatchSize = 32 /*0x20*/;
    private static ConduitFlowVisualizer.RenderMeshBatchJob instance;

    public static ConduitFlowVisualizer.RenderMeshBatchJob Instance
    {
      get
      {
        if (ConduitFlowVisualizer.RenderMeshBatchJob.instance == null || ConduitFlowVisualizer.RenderMeshBatchJob.instance.threadContexts.Count != GlobalJobManager.ThreadCount)
          ConduitFlowVisualizer.RenderMeshBatchJob.instance = new ConduitFlowVisualizer.RenderMeshBatchJob();
        return ConduitFlowVisualizer.RenderMeshBatchJob.instance;
      }
    }

    public RenderMeshBatchJob()
    {
      this.threadContexts = new List<ConduitFlowVisualizer.RenderMeshPerThreadData>();
      for (int index = 0; index < GlobalJobManager.ThreadCount; ++index)
        this.threadContexts.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData());
    }

    public void Reset(ConduitFlowVisualizer.RenderMeshContext context)
    {
      this.sharedData = context;
      if (context.visible_conduits == null)
        this.count = 0;
      else
        this.count = (context.visible_conduits.Count + 32 /*0x20*/ - 1) / 32 /*0x20*/;
    }

    public override void RunItem(
      int item,
      ref ConduitFlowVisualizer.RenderMeshContext shared_data,
      ConduitFlowVisualizer.RenderMeshPerThreadData thread_context,
      int threadIndex)
    {
      Element element = (Element) null;
      int num1 = item * 32 /*0x20*/;
      int num2 = Math.Min(shared_data.visible_conduits.Count, num1 + 32 /*0x20*/);
      for (int index = num1; index < num2; ++index)
      {
        ConduitFlow.Conduit conduit = shared_data.outer.flowManager.soaInfo.GetConduit(shared_data.visible_conduits[index]);
        ConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(shared_data.outer.flowManager);
        ConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(shared_data.outer.flowManager);
        if ((double) lastFlowInfo.contents.mass > 0.0)
        {
          int cell = conduit.GetCell(shared_data.outer.flowManager);
          int cellFromDirection = ConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
          Vector2I xy1 = Grid.CellToXY(cell);
          Vector2I xy2 = Grid.CellToXY(cellFromDirection);
          Vector2 pos = cell == -1 ? (Vector2) xy1 : Vector2.Lerp(new Vector2((float) xy1.x, (float) xy1.y), new Vector2((float) xy2.x, (float) xy2.y), shared_data.lerp_percent);
          Color32 color32 = Color32.Lerp(shared_data.outer.GetCellTintColour(cell), shared_data.outer.GetCellTintColour(cellFromDirection), shared_data.lerp_percent);
          bool highlight = false;
          if (shared_data.outer.showContents)
          {
            if ((double) lastFlowInfo.contents.mass >= (double) initialContents.mass)
              thread_context.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(lastFlowInfo.direction, pos, color32, shared_data.outer.tuning.size, false, false));
            if (element == null || lastFlowInfo.contents.element != element.id)
              element = ElementLoader.FindElementByHash(lastFlowInfo.contents.element);
          }
          else
          {
            element = (Element) null;
            highlight = Grid.PosToCell(new Vector3(pos.x + ConduitFlowVisualizer.GRID_OFFSET.x, pos.y + ConduitFlowVisualizer.GRID_OFFSET.y, 0.0f)) == shared_data.outer.highlightedCell;
          }
          Color32 contentsColor = shared_data.outer.GetContentsColor(element, color32);
          float num3 = 1f;
          if (shared_data.outer.showContents || (double) lastFlowInfo.contents.mass < (double) initialContents.mass)
            num3 = shared_data.outer.CalculateMassScale(lastFlowInfo.contents.mass);
          thread_context.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(lastFlowInfo.direction, pos, contentsColor, shared_data.outer.tuning.size * num3, true, highlight));
          thread_context.moving_conduits.Add(conduit);
        }
        if ((double) initialContents.mass > (double) lastFlowInfo.contents.mass && (double) initialContents.mass > 0.0)
        {
          int cell = conduit.GetCell(shared_data.outer.flowManager);
          Vector2 xy = (Vector2) Grid.CellToXY(cell);
          float mass = initialContents.mass - lastFlowInfo.contents.mass;
          bool highlight = false;
          Color32 cellTintColour = shared_data.outer.GetCellTintColour(cell);
          float massScale = shared_data.outer.CalculateMassScale(mass);
          if (shared_data.outer.showContents)
          {
            thread_context.static_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(ConduitFlow.FlowDirections.None, xy, cellTintColour, shared_data.outer.tuning.size * massScale, false, false));
            if (element == null || initialContents.element != element.id)
              element = ElementLoader.FindElementByHash(initialContents.element);
          }
          else
          {
            element = (Element) null;
            highlight = cell == shared_data.outer.highlightedCell;
          }
          Color32 contentsColor = shared_data.outer.GetContentsColor(element, cellTintColour);
          thread_context.static_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(ConduitFlow.FlowDirections.None, xy, contentsColor, shared_data.outer.tuning.size * massScale, true, highlight));
        }
      }
    }

    public void Finish(
      ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh,
      ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh,
      Vector3 camera_pos,
      ConduitFlowVisualizer visualizer)
    {
      foreach (ConduitFlowVisualizer.RenderMeshPerThreadData threadContext in this.threadContexts)
        threadContext.Finish(moving_ball_mesh, static_ball_mesh, camera_pos, visualizer);
      this.sharedData.Finish();
    }
  }
}
