// Decompiled with JetBrains decompiler
// Type: BubbleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/BubbleManager")]
public class BubbleManager : KMonoBehaviour, ISim33ms, IRenderEveryTick
{
  public static BubbleManager instance;
  [Serialize]
  private readonly Dictionary<BubbleManager.WorldArchetype, BubbleManager.InstanceData> bubbles = new Dictionary<BubbleManager.WorldArchetype, BubbleManager.InstanceData>();
  [Serialize]
  private readonly Dictionary<BubbleManager.Archetype.Id, BubbleManager.Archetype> archetypes = new Dictionary<BubbleManager.Archetype.Id, BubbleManager.Archetype>();
  private Mesh mesh;
  private MaterialPropertyBlock propertyBlock;
  [SerializeField]
  private Texture2D texture;
  [SerializeField]
  private int numFrames;
  [SerializeField]
  private Material material;
  [SerializeField]
  private Grid.SceneLayer sceneLayer;
  [SerializeField]
  private Vector2 particleSize;
  private bool isInfraredON;
  private static Vector2 DEFAULT_VELOCITY = new Vector2(0.0f, 1f);

  public static void DestroyInstance() => BubbleManager.instance = (BubbleManager) null;

  protected override void OnPrefabInit()
  {
    BubbleManager.instance = this;
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    this.mesh = new Mesh();
    this.mesh.MarkDynamic();
    this.mesh.name = "BubbleManager Mesh";
    this.propertyBlock = new MaterialPropertyBlock();
    this.propertyBlock.SetTexture("_MainTex", (Texture) this.texture);
    Game.Instance.Subscribe(-880408538, new Action<object>(this.OnTemperatureOverlayInfraredUpdate));
    Game.Instance.Subscribe(972756592, new Action<object>(this.OnTemperatureOverlayInfraredClear));
  }

  private BubbleManager.Archetype.Id RegisterArchetype(BubbleManager.Archetype archetype)
  {
    BubbleManager.Archetype.Id id = archetype.GetId();
    this.archetypes.TryAdd(id, archetype);
    return id;
  }

  private void OnTemperatureOverlayInfraredClear(object obj) => this.isInfraredON = false;

  private void OnTemperatureOverlayInfraredUpdate(object obj) => this.isInfraredON = true;

  [System.Runtime.Serialization.OnDeserialized]
  public void OnDeserialized()
  {
    ListPool<BubbleManager.Archetype.Id, BubbleManager>.PooledList pooledList1 = ListPool<BubbleManager.Archetype.Id, BubbleManager>.Allocate();
    foreach (BubbleManager.Archetype.Id key in this.archetypes.Keys)
    {
      int num = 0;
      foreach (KeyValuePair<BubbleManager.WorldArchetype, BubbleManager.InstanceData> bubble in this.bubbles)
      {
        BubbleManager.WorldArchetype worldArchetype1;
        BubbleManager.InstanceData instanceData1;
        bubble.Deconstruct(ref worldArchetype1, ref instanceData1);
        BubbleManager.WorldArchetype worldArchetype2 = worldArchetype1;
        BubbleManager.InstanceData instanceData2 = instanceData1;
        if (worldArchetype2.archetype.hashCode == key.hashCode)
          num += instanceData2.Count;
      }
      if (num == 0)
        pooledList1.Add(key);
    }
    foreach (BubbleManager.Archetype.Id key in (List<BubbleManager.Archetype.Id>) pooledList1)
      this.archetypes.Remove(key);
    pooledList1.Recycle();
    ListPool<BubbleManager.WorldArchetype, BubbleManager>.PooledList pooledList2 = ListPool<BubbleManager.WorldArchetype, BubbleManager>.Allocate();
    foreach (BubbleManager.WorldArchetype key in this.bubbles.Keys)
    {
      if (!this.archetypes.ContainsKey(key.archetype))
        pooledList2.Add(key);
    }
    if (pooledList2.Count != 0)
      DebugUtil.LogWarningArgs((object) "BubbleManager.OnDeserialized is deleting bubbles");
    foreach (BubbleManager.WorldArchetype key in (List<BubbleManager.WorldArchetype>) pooledList2)
      this.bubbles.Remove(key);
    pooledList2.Recycle();
  }

  public void SpawnBubble(
    SimHashes element,
    Vector2 position,
    float mass,
    float temperature,
    BubbleManager.Disease disease,
    Vector2? velocity = null)
  {
    if ((double) mass < 9.9999997171806854E-10)
    {
      Debug.LogFormat("BubbleManager.SpawnBubble: Attempted to spawn a bubble with mass {0} which is below the sim's minimum mass threshold of {1}. Bubble will not be spawned.", (object) mass, (object) 1E-09f);
    }
    else
    {
      int frame = UnityEngine.Random.Range(0, this.numFrames);
      int cell = Grid.PosToCell(position);
      byte num = Grid.WorldIdx[cell];
      this.ManifestBucket(new BubbleManager.WorldArchetype()
      {
        worldIdx = (int) num,
        archetype = this.RegisterArchetype(new BubbleManager.Archetype(velocity ?? BubbleManager.DEFAULT_VELOCITY, element))
      }).Add(position, mass, temperature, frame, disease);
    }
  }

  private BubbleManager.InstanceData ManifestBucket(BubbleManager.WorldArchetype worldArchetype)
  {
    BubbleManager.InstanceData instanceData;
    if (!this.bubbles.TryGetValue(worldArchetype, out instanceData))
    {
      instanceData = new BubbleManager.InstanceData();
      this.bubbles[worldArchetype] = instanceData;
    }
    return instanceData;
  }

  private static bool ShouldPop(Vector2 position, SimHashes bubbleElement, out int cell)
  {
    cell = Grid.PosToCell(position);
    return Grid.Element[cell].id == bubbleElement || !UnderwaterSoundEvent.IsVisiblyInLiquid((Vector3) position);
  }

  public void Sim33ms(float dt)
  {
    ListPool<int, BubbleManager>.PooledList indices = ListPool<int, BubbleManager>.Allocate();
    ListPool<BubbleManager.WorldArchetype, BubbleManager>.PooledList pooledList = ListPool<BubbleManager.WorldArchetype, BubbleManager>.Allocate();
    foreach (KeyValuePair<BubbleManager.WorldArchetype, BubbleManager.InstanceData> bubble in this.bubbles)
    {
      BubbleManager.WorldArchetype worldArchetype1;
      BubbleManager.InstanceData instanceData1;
      bubble.Deconstruct(ref worldArchetype1, ref instanceData1);
      BubbleManager.WorldArchetype worldArchetype2 = worldArchetype1;
      BubbleManager.InstanceData instanceData2 = instanceData1;
      BubbleManager.Archetype archetype;
      if (!this.archetypes.TryGetValue(worldArchetype2.archetype, out archetype))
      {
        DebugUtil.LogWarningArgs((object) "BubbleManager.Sim33ms: Unknown archetype id. Skipping this bubble type for this world");
        pooledList.Add(worldArchetype2);
      }
      else
      {
        Vector2 velocity = archetype.velocity;
        velocity.Normalize();
        velocity *= Grid.HalfCellSizeInMeters;
        foreach (BubbleManager.InstanceData.Subscript subscript in instanceData2)
        {
          subscript.Position += archetype.velocity * dt;
          Vector2 position = subscript.Position + velocity;
          int cell1;
          bool flag = BubbleManager.ShouldPop(position, archetype.element, out cell1);
          if (!subscript.Visible | flag)
          {
            if (Grid.Solid[cell1] && Grid.Element[cell1].IsSolid)
              cell1 = Grid.PosToCell(subscript.Position);
            else if (Grid.Element[cell1].IsLiquid && Grid.Element[cell1].id != archetype.element)
            {
              int cell2 = Grid.CellAbove(cell1);
              if ((!Grid.IsValidCell(cell2) ? 0 : (Grid.IsGas(cell2) ? 1 : (Grid.Element[cell2].IsVacuum ? 1 : 0))) != 0)
                cell1 = cell2;
            }
            SimMessages.AddRemoveSubstance(cell1, archetype.element, CellEventLogger.Instance.FallingWaterAddToSim, subscript.Mass, subscript.Temperature, subscript.Disease.Idx, subscript.Disease.Count);
            indices.Add(subscript.Index);
          }
          if (!subscript.FadingOut)
          {
            if (BubbleManager.ShouldPop(position + velocity * 2f, archetype.element, out int _))
              subscript.FadingOut = true;
          }
          else
            subscript.Alpha = Mathf.Max(0.0f, subscript.Alpha - archetype.alphaFadeSpeed * dt);
          subscript.ElapsedTime += dt;
        }
        instanceData2.Destroy((List<int>) indices);
        indices.Clear();
      }
    }
    indices.Recycle();
    foreach (BubbleManager.WorldArchetype key in (List<BubbleManager.WorldArchetype>) pooledList)
      this.bubbles.Remove(key);
    pooledList.Recycle();
  }

  public void GetBubblesInCell(int cell, List<BubbleManager.CellBubbleInfo> results)
  {
    results.Clear();
    int num1 = (int) Grid.WorldIdx[cell];
    foreach (KeyValuePair<BubbleManager.WorldArchetype, BubbleManager.InstanceData> bubble in this.bubbles)
    {
      BubbleManager.WorldArchetype worldArchetype1;
      BubbleManager.InstanceData instanceData1;
      bubble.Deconstruct(ref worldArchetype1, ref instanceData1);
      BubbleManager.WorldArchetype worldArchetype2 = worldArchetype1;
      BubbleManager.InstanceData instanceData2 = instanceData1;
      BubbleManager.Archetype archetype;
      if (worldArchetype2.worldIdx == num1 && this.archetypes.TryGetValue(worldArchetype2.archetype, out archetype))
      {
        float num2 = 0.0f;
        float num3 = 0.0f;
        foreach (BubbleManager.InstanceData.Subscript subscript in instanceData2)
        {
          if (Grid.PosToCell(subscript.Position) == cell)
          {
            num2 += subscript.Mass;
            num3 += subscript.Mass * subscript.Temperature;
          }
        }
        if ((double) num2 > 0.0)
        {
          bool flag = false;
          for (int index = 0; index < results.Count; ++index)
          {
            if (results[index].element == archetype.element)
            {
              BubbleManager.CellBubbleInfo result = results[index];
              float num4 = result.totalMass + num2;
              result.averageTemperature = (result.averageTemperature * result.totalMass + num3) / num4;
              result.totalMass = num4;
              results[index] = result;
              flag = true;
              break;
            }
          }
          if (!flag)
            results.Add(new BubbleManager.CellBubbleInfo()
            {
              element = archetype.element,
              totalMass = num2,
              averageTemperature = num3 / num2
            });
        }
      }
    }
  }

  public void RenderEveryTick(float dt)
  {
    List<Vector3> vertices = MeshUtil.vertices;
    List<Color32> colours32 = MeshUtil.colours32;
    List<Vector2> uvs = MeshUtil.uvs;
    List<Vector2> uv2s = MeshUtil.uv2s;
    List<Vector4> uv4s = MeshUtil.uv4s;
    List<int> indices = MeshUtil.indices;
    float x = this.particleSize.x * 0.5f;
    float y = this.particleSize.y * 0.5f;
    Vector2 vector2_1 = new Vector2(-x, -y);
    Vector2 vector2_2 = new Vector2(x, -y);
    Vector2 vector2_3 = new Vector2(x, y);
    Vector2 vector2_4 = new Vector2(-x, y);
    uvs.Clear();
    uv2s.Clear();
    uv4s.Clear();
    vertices.Clear();
    indices.Clear();
    colours32.Clear();
    int num1 = 0;
    foreach (KeyValuePair<BubbleManager.WorldArchetype, BubbleManager.InstanceData> bubble in this.bubbles)
    {
      BubbleManager.WorldArchetype worldArchetype1;
      BubbleManager.InstanceData instanceData1;
      bubble.Deconstruct(ref worldArchetype1, ref instanceData1);
      BubbleManager.WorldArchetype worldArchetype2 = worldArchetype1;
      BubbleManager.InstanceData instanceData2 = instanceData1;
      if (worldArchetype2.worldIdx == ClusterManager.Instance.activeWorldId)
      {
        BubbleManager.Archetype archetype;
        if (!this.archetypes.TryGetValue(worldArchetype2.archetype, out archetype))
        {
          DebugUtil.LogWarningArgs((object) "BubbleManager.RenderEveryTick: Unknown archetype id, likely dynamically registered");
        }
        else
        {
          int a = instanceData2.CountVisible();
          if (a != 0)
          {
            int b = 16249 - num1;
            if (b <= 0)
            {
              DebugUtil.LogWarningArgs((object) "BubbleManager.RenderEveryTick: Particle capacity reached, skipping remaining archetypes");
              break;
            }
            int num2 = Mathf.Min(a, b);
            bool flag = num2 == a;
            if (!flag)
              DebugUtil.LogWarningArgs((object) "Too many bubbles to render. Wanted", (object) a, (object) "but truncating to", (object) num2);
            int num3 = 0;
            foreach (BubbleManager.InstanceData.Subscript subscript in instanceData2)
            {
              if (subscript.Visible)
              {
                vertices.Add((Vector3) (subscript.Position + vector2_1));
                vertices.Add((Vector3) (subscript.Position + vector2_2));
                vertices.Add((Vector3) (subscript.Position + vector2_3));
                vertices.Add((Vector3) (subscript.Position + vector2_4));
                uvs.Add(new Vector2(0.0f, 0.0f));
                uvs.Add(new Vector2(1f, 0.0f));
                uvs.Add(new Vector2(1f, 1f));
                uvs.Add(new Vector2(0.0f, 1f));
                Color32 colour = archetype.Colour;
                colour.a = (byte) ((double) colour.a * (double) subscript.Alpha);
                Vector2 vector2_5 = new Vector2((float) subscript.SizeLevel, (float) subscript.Frame);
                uv2s.Add(vector2_5);
                uv2s.Add(vector2_5);
                uv2s.Add(vector2_5);
                uv2s.Add(vector2_5);
                Vector4 vector4 = this.isInfraredON ? (Vector4) SimDebugView.Instance.NormalizedTemperature(subscript.Temperature) : Vector4.zero;
                uv4s.Add(vector4);
                uv4s.Add(vector4);
                uv4s.Add(vector4);
                uv4s.Add(vector4);
                colours32.Add(colour);
                colours32.Add(colour);
                colours32.Add(colour);
                colours32.Add(colour);
                int num4 = (num1 + num3) * 4;
                indices.Add(num4);
                indices.Add(num4 + 1);
                indices.Add(num4 + 2);
                indices.Add(num4);
                indices.Add(num4 + 2);
                indices.Add(num4 + 3);
                ++num3;
                if (!flag)
                {
                  if (num3 == num2)
                    break;
                }
              }
            }
            DebugUtil.DevAssert(num3 == num2, "Rendered bubble count does not match expected");
            num1 += num3;
          }
        }
      }
    }
    if (num1 <= 0)
      return;
    this.mesh.Clear();
    this.mesh.SetVertices(vertices);
    this.mesh.SetUVs(0, uvs);
    this.mesh.SetUVs(1, uv2s);
    this.mesh.SetUVs(2, uv4s);
    this.mesh.SetColors(colours32);
    this.mesh.SetTriangles(indices, 0);
    int layer = LayerMask.NameToLayer("Default");
    this.material.SetVector("_ClusterWorldSizeInfo", PropertyTextures.CalculateClusterWorldSize());
    Graphics.DrawMesh(this.mesh, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(this.sceneLayer)), Quaternion.identity, this.material, layer, (Camera) null, 0, this.propertyBlock);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.Unsubscribe(-880408538, new Action<object>(this.OnTemperatureOverlayInfraredUpdate));
    Game.Instance.Unsubscribe(972756592, new Action<object>(this.OnTemperatureOverlayInfraredClear));
    base.OnCleanUp();
  }

  [Serializable]
  public struct Disease
  {
    public static readonly BubbleManager.Disease None = new BubbleManager.Disease()
    {
      Idx = byte.MaxValue,
      Count = 0
    };
    public byte Idx;
    public int Count;
  }

  [Serializable]
  private readonly struct Archetype
  {
    public readonly Vector2 velocity;
    public readonly SimHashes element;
    public readonly float alphaFadeSpeed;

    public Color32 Colour
    {
      get
      {
        DebugUtil.DevAssert(ElementLoader.elementTable != null, "Elements are not loaded yet");
        ushort elementIndex = ElementLoader.GetElementIndex(this.element);
        DebugUtil.DevAssert(ElementLoader.elements != null, "Elements are not loaded yet");
        Element element = ElementLoader.elements[(int) elementIndex];
        return (Color32) ((element.IsMoltenMetal ? WaterCubes.MOLTEN_METAL_COLOR : (Color) element.substance.colour) with
        {
          a = (float) byte.MaxValue
        });
      }
    }

    public Archetype(Vector2 velocity, SimHashes elementId)
    {
      this.velocity = velocity;
      this.element = elementId;
      this.alphaFadeSpeed = velocity.magnitude;
    }

    public BubbleManager.Archetype.Id GetId()
    {
      return new BubbleManager.Archetype.Id()
      {
        hashCode = this.GetHashCode()
      };
    }

    [Serializable]
    public struct Id
    {
      public int hashCode;
    }
  }

  [Serializable]
  private struct WorldArchetype
  {
    public int worldIdx;
    public BubbleManager.Archetype.Id archetype;
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  private class InstanceData : IEnumerable<BubbleManager.InstanceData.Subscript>, IEnumerable
  {
    private const int INVALID_ENTRY = -1;
    private const float FULLY_OPAQUE = -1f;
    public static readonly float[] MassTresholds = new float[2]
    {
      0.1f,
      0.3f
    };
    [Serialize]
    private readonly List<Vector2> position = new List<Vector2>();
    [Serialize]
    private readonly List<float> elapsedTime = new List<float>();
    [Serialize]
    private readonly List<int> frame = new List<int>();
    [Serialize]
    private readonly List<float> temperature = new List<float>();
    [Serialize]
    private readonly List<float> mass = new List<float>();
    [Serialize]
    private readonly List<byte> sizeLevel = new List<byte>();
    [Serialize]
    private readonly List<float> alpha = new List<float>();
    [Serialize]
    private readonly List<BubbleManager.Disease> disease = new List<BubbleManager.Disease>();
    [Serialize]
    private readonly List<int> freeList = new List<int>();

    public int Add(
      Vector2 position,
      float mass,
      float temperature,
      int frame,
      BubbleManager.Disease disease)
    {
      int index = this.ManifestIndex();
      BubbleManager.InstanceData.Subscript subscript = this[index] with
      {
        Position = position,
        ElapsedTime = 0.0f,
        Frame = frame,
        Mass = mass,
        SizeLevel = this.CalculateAndGetSizeLevel(mass),
        Temperature = temperature,
        Alpha = -1f,
        Disease = disease
      };
      return index;
    }

    private byte CalculateAndGetSizeLevel(float mass)
    {
      DebugUtil.DevAssert(BubbleManager.InstanceData.MassTresholds != null, "MassTresholds should be statically initialized");
      DebugUtil.DevAssert(BubbleManager.InstanceData.MassTresholds.Length != 0, "MassTresholds should be statically initialized");
      for (int andGetSizeLevel = 0; andGetSizeLevel < BubbleManager.InstanceData.MassTresholds.Length; ++andGetSizeLevel)
      {
        if ((double) BubbleManager.InstanceData.MassTresholds[andGetSizeLevel] - (double) mass >= 0.0)
          return (byte) andGetSizeLevel;
      }
      return (byte) BubbleManager.InstanceData.MassTresholds.Length;
    }

    public void Destroy(int index)
    {
      this.freeList.Add(index);
      this.frame[index] = -1;
    }

    public void Destroy(List<int> indices)
    {
      this.freeList.AddRange((IEnumerable<int>) indices);
      foreach (int index in indices)
        this.frame[index] = -1;
    }

    private int ManifestIndex()
    {
      if (this.freeList.Count > 0)
      {
        List<int> freeList = this.freeList;
        int num = freeList[freeList.Count - 1];
        this.freeList.RemoveAt(this.freeList.Count - 1);
        return num;
      }
      int count = this.position.Count;
      this.position.Add(new Vector2());
      this.elapsedTime.Add(0.0f);
      this.frame.Add(0);
      this.temperature.Add(0.0f);
      this.mass.Add(0.0f);
      this.sizeLevel.Add((byte) 0);
      this.alpha.Add(-1f);
      this.disease.Add(BubbleManager.Disease.None);
      return count;
    }

    public int Begin => this.Next(-1);

    public int End => this.position.Count;

    public int Next(int index)
    {
      if (index == this.End)
        return this.End;
      do
      {
        ++index;
        if (index == this.End)
          return this.End;
      }
      while (this.frame[index] == -1);
      return index;
    }

    public int Count => this.position.Count - this.freeList.Count;

    [System.Runtime.Serialization.OnDeserialized]
    public void OnDeserialized()
    {
      if (this.disease.Count < this.position.Count)
      {
        this.disease.Capacity = Math.Max(this.disease.Capacity, this.position.Count);
        for (int count = this.disease.Count; count != this.position.Count; ++count)
          this.disease.Add(BubbleManager.Disease.None);
      }
      if (this.alpha.Count >= this.position.Count)
        return;
      this.alpha.Capacity = Math.Max(this.alpha.Capacity, this.position.Count);
      for (int count = this.alpha.Count; count != this.position.Count; ++count)
        this.alpha.Add(-1f);
    }

    public int CountVisible()
    {
      int num = 0;
      foreach (BubbleManager.InstanceData.Subscript subscript in this)
      {
        if (subscript.Visible)
          ++num;
      }
      return num;
    }

    public BubbleManager.InstanceData.Subscript this[int index]
    {
      get => new BubbleManager.InstanceData.Subscript(this, index);
    }

    public IEnumerator<BubbleManager.InstanceData.Subscript> GetEnumerator()
    {
      return (IEnumerator<BubbleManager.InstanceData.Subscript>) new BubbleManager.InstanceData.Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public readonly struct Subscript(BubbleManager.InstanceData data, int index)
    {
      private readonly BubbleManager.InstanceData data = data;
      private readonly int index = index;

      public int Index => this.index;

      public Vector2 Position
      {
        get => this.data.position[this.index];
        set => this.data.position[this.index] = value;
      }

      public float ElapsedTime
      {
        get => this.data.elapsedTime[this.index];
        set => this.data.elapsedTime[this.index] = value;
      }

      public int Frame
      {
        get => this.data.frame[this.index];
        set => this.data.frame[this.index] = value;
      }

      public float Temperature
      {
        get => this.data.temperature[this.index];
        set => this.data.temperature[this.index] = value;
      }

      public float Mass
      {
        get => this.data.mass[this.index];
        set => this.data.mass[this.index] = value;
      }

      public byte SizeLevel
      {
        get => this.data.sizeLevel[this.index];
        set => this.data.sizeLevel[this.index] = value;
      }

      public bool Visible => (double) this.data.alpha[this.index] != 0.0;

      public bool FadingOut
      {
        get => (double) this.data.alpha[this.index] != -1.0;
        set
        {
          DebugUtil.DevAssert(value, "Cannot set FadingOut to false. Once the fade out is begun, it cannot be stopped");
          if (!value || (double) this.data.alpha[this.index] != -1.0)
            return;
          this.data.alpha[this.index] = 1f;
        }
      }

      public float Alpha
      {
        get
        {
          float num = this.data.alpha[this.index];
          return (double) num != -1.0 ? num : 1f;
        }
        set => this.data.alpha[this.index] = value;
      }

      public BubbleManager.Disease Disease
      {
        get => this.data.disease[this.index];
        set => this.data.disease[this.index] = value;
      }
    }

    public struct Enumerator(BubbleManager.InstanceData outer) : 
      IEnumerator<BubbleManager.InstanceData.Subscript>,
      IEnumerator,
      IDisposable
    {
      private int index = -1;
      private readonly BubbleManager.InstanceData outer = outer;

      public bool MoveNext()
      {
        this.index = this.index == -1 ? this.outer.Begin : this.outer.Next(this.index);
        return this.index != this.outer.End;
      }

      public void Reset() => this.index = this.outer.Begin;

      readonly void IDisposable.Dispose()
      {
      }

      public readonly BubbleManager.InstanceData.Subscript Current
      {
        get => new BubbleManager.InstanceData.Subscript(this.outer, this.index);
      }

      readonly object IEnumerator.Current => (object) this.Current;
    }
  }

  public struct CellBubbleInfo
  {
    public SimHashes element;
    public float totalMass;
    public float averageTemperature;
  }
}
