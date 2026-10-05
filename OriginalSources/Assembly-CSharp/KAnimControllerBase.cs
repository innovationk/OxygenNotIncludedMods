// Decompiled with JetBrains decompiler
// Type: KAnimControllerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class KAnimControllerBase : MonoBehaviour, ISerializationCallbackReceiver
{
  [NonSerialized]
  public GameObject showWhenMissing;
  [SerializeField]
  public KAnimBatchGroup.MaterialType materialType;
  [SerializeField]
  public string initialAnim;
  [SerializeField]
  public KAnim.PlayMode initialMode = KAnim.PlayMode.Once;
  [SerializeField]
  protected KAnimFile[] animFiles = new KAnimFile[0];
  [SerializeField]
  protected Vector3 offset;
  [SerializeField]
  protected Vector3 pivot;
  [SerializeField]
  protected float rotation;
  [SerializeField]
  public bool destroyOnAnimComplete;
  [SerializeField]
  public bool inactiveDisable;
  [SerializeField]
  protected bool flipX;
  [SerializeField]
  protected bool flipY;
  public int initialBlendParameters = -1;
  [SerializeField]
  public bool forceUseGameTime;
  public string defaultAnim;
  protected KAnim.Anim curAnim;
  protected int curAnimFrameIdx = -1;
  protected int prevAnimFrame = -1;
  public bool usingNewSymbolOverrideSystem;
  protected HandleVector<int>.Handle eventManagerHandle = HandleVector<int>.InvalidHandle;
  protected List<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = new List<KAnimControllerBase.OverrideAnimFileData>();
  public bool randomiseLoopedOffset;
  protected float elapsedTime;
  protected float playSpeed = 1f;
  protected KAnim.PlayMode mode = KAnim.PlayMode.Once;
  protected bool stopped = true;
  public float animHeight = 1f;
  public float animWidth = 1f;
  protected bool isVisible;
  protected Bounds bounds;
  public Action<Bounds> OnUpdateBounds;
  public Action<Color> OnTintChanged;
  public Action<Color> OnHighlightChanged;
  protected KAnimSynchronizer synchronizer;
  protected KAnimLayering layering;
  [SerializeField]
  protected bool _enabled = true;
  protected bool hasEnableRun;
  protected bool hasAwakeRun;
  protected KBatchedAnimInstanceData batchInstanceData;
  public KAnimControllerBase.VisibilityType visibilityType;
  public Action<GameObject> onDestroySelf;
  [SerializeField]
  protected List<KAnimHashedString> hiddenSymbols = new List<KAnimHashedString>();
  [SerializeField]
  protected HashSet<KAnimHashedString> hiddenSymbolsSet = new HashSet<KAnimHashedString>();
  protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> anims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();
  protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> overrideAnims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();
  protected System.Collections.Generic.Queue<KAnimControllerBase.AnimData> animQueue = new System.Collections.Generic.Queue<KAnimControllerBase.AnimData>();
  protected int maxSymbols;
  public Grid.SceneLayer fgLayer = Grid.SceneLayer.NoLayer;
  protected AnimEventManager aem;
  private static HashedString snaptoPivot = new HashedString("snapTo_pivot");

  protected KAnimControllerBase()
  {
    this.previousFrame = -1;
    this.currentFrame = -1;
    this.PlaySpeedMultiplier = 1f;
    this.synchronizer = new KAnimSynchronizer(this);
    this.layering = new KAnimLayering(this, this.fgLayer);
    this.isVisible = true;
  }

  public abstract KAnim.Anim GetAnim(int index);

  public KAnim.Build curBuild { get; protected set; }

  public event Action<Color32> OnOverlayColourChanged;

  public new bool enabled
  {
    get => this._enabled;
    set
    {
      this._enabled = value;
      if (!this.hasAwakeRun)
        return;
      if (this._enabled)
        this.Enable();
      else
        this.Disable();
    }
  }

  public bool HasBatchInstanceData => this.batchInstanceData != null;

  public SymbolInstanceGpuData symbolInstanceGpuData { get; protected set; }

  public SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; protected set; }

  public uint BlendPackedValues => this.batchInstanceData.GetAllBlendPackedValues();

  public Color32 TintColour
  {
    get => (Color32) this.batchInstanceData.GetTintColour();
    set
    {
      if (this.batchInstanceData == null || !this.batchInstanceData.SetTintColour((Color) value))
        return;
      this.SetDirty();
      this.SuspendUpdates(false);
      if (this.OnTintChanged == null)
        return;
      this.OnTintChanged((Color) value);
    }
  }

  public Color32 HighlightColour
  {
    get => (Color32) this.batchInstanceData.GetHighlightcolour();
    set
    {
      if (!this.batchInstanceData.SetHighlightColour((Color) value))
        return;
      this.SetDirty();
      this.SuspendUpdates(false);
      if (this.OnHighlightChanged == null)
        return;
      this.OnHighlightChanged((Color) value);
    }
  }

  public Color OverlayColour
  {
    get => this.batchInstanceData.GetOverlayColour();
    set
    {
      if (!this.batchInstanceData.SetOverlayColour(value))
        return;
      this.SetDirty();
      this.SuspendUpdates(false);
      if (this.OnOverlayColourChanged == null)
        return;
      this.OnOverlayColourChanged((Color32) value);
    }
  }

  public event KAnimControllerBase.KAnimEvent onAnimEnter;

  public event KAnimControllerBase.KAnimEvent onAnimComplete;

  public event Action<int> onLayerChanged;

  public int previousFrame { get; protected set; }

  public int currentFrame { get; protected set; }

  public HashedString currentAnim => this.curAnim == null ? new HashedString() : this.curAnim.hash;

  public float PlaySpeedMultiplier { set; get; }

  public void SetFGLayer(Grid.SceneLayer layer)
  {
    this.fgLayer = layer;
    this.GetLayering();
    if (this.layering == null)
      return;
    this.layering.SetLayer(this.fgLayer);
  }

  public KAnim.PlayMode PlayMode
  {
    get => this.mode;
    set => this.mode = value;
  }

  public bool FlipX
  {
    get => this.flipX;
    set
    {
      this.flipX = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.SetDirty();
    }
  }

  public bool FlipY
  {
    get => this.flipY;
    set
    {
      this.flipY = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.SetDirty();
    }
  }

  public Vector3 Offset
  {
    get => this.offset;
    set
    {
      this.offset = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.DeRegister();
      this.Register();
      this.RefreshVisibilityListener();
      this.SetDirty();
    }
  }

  public float Rotation
  {
    get => this.rotation;
    set
    {
      this.rotation = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.SetDirty();
    }
  }

  public Vector3 Pivot
  {
    get => this.pivot;
    set
    {
      this.pivot = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.SetDirty();
    }
  }

  public Vector3 PositionIncludingOffset => this.transform.GetPosition() + this.Offset;

  public KAnimBatchGroup.MaterialType GetMaterialType() => this.materialType;

  public Vector3 GetWorldPivot()
  {
    Vector3 position = this.transform.GetPosition();
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      position.x += component.offset.x;
      position.y += component.offset.y - component.size.y / 2f;
    }
    return position;
  }

  public KAnim.Anim GetCurrentAnim() => this.curAnim;

  public KAnimHashedString GetBuildHash()
  {
    return this.curBuild == null ? (KAnimHashedString) KAnimBatchManager.NO_BATCH : this.curBuild.fileHash;
  }

  protected float GetDuration()
  {
    return this.curAnim != null ? (float) this.curAnim.numFrames / this.curAnim.frameRate : 0.0f;
  }

  protected int GetFrameIdxFromOffset(int offset)
  {
    int frameIdxFromOffset = -1;
    if (this.curAnim != null)
      frameIdxFromOffset = offset + this.curAnim.firstFrameIdx;
    return frameIdxFromOffset;
  }

  public int GetFrameIdx(float time, bool absolute)
  {
    int frameIdx = -1;
    if (this.curAnim != null)
      frameIdx = this.curAnim.GetFrameIdx(this.mode, time) + (absolute ? this.curAnim.firstFrameIdx : 0);
    return frameIdx;
  }

  public bool IsStopped() => this.stopped;

  public KAnim.Anim CurrentAnim => this.curAnim;

  public KAnimSynchronizer GetSynchronizer() => this.synchronizer;

  public KAnimLayering GetLayering()
  {
    if (this.layering == null && this.fgLayer != Grid.SceneLayer.NoLayer)
      this.layering = new KAnimLayering(this, this.fgLayer);
    return this.layering;
  }

  public KAnim.PlayMode GetMode() => this.mode;

  public static string GetModeString(KAnim.PlayMode mode)
  {
    switch (mode)
    {
      case KAnim.PlayMode.Loop:
        return "Loop";
      case KAnim.PlayMode.Once:
        return "Once";
      case KAnim.PlayMode.Paused:
        return "Paused";
      default:
        return "Unknown";
    }
  }

  public float GetPlaySpeed() => this.playSpeed;

  public void SetElapsedTime(float value) => this.elapsedTime = value;

  public float GetElapsedTime() => this.elapsedTime;

  protected abstract void SuspendUpdates(bool suspend);

  protected abstract void OnStartQueuedAnim();

  public abstract void SetDirty();

  protected abstract void RefreshVisibilityListener();

  protected abstract void DeRegister();

  protected abstract void Register();

  protected abstract void OnAwake();

  protected abstract void OnStart();

  protected abstract void OnStop();

  protected abstract void Enable();

  protected abstract void Disable();

  protected abstract void UpdateFrame(float t);

  public abstract Matrix2x3 GetTransformMatrix();

  public abstract Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible);

  public abstract void UpdateAllHiddenSymbols();

  public abstract void UpdateHiddenSymbol(KAnimHashedString specificSymbol);

  public abstract void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> specificSymbols);

  public abstract void TriggerStop();

  public virtual void SetLayer(int layer)
  {
    if (this.onLayerChanged == null)
      return;
    this.onLayerChanged(layer);
  }

  public Vector3 GetPivotSymbolPosition()
  {
    bool symbolVisible = false;
    Matrix4x4 symbolTransform = this.GetSymbolTransform(KAnimControllerBase.snaptoPivot, out symbolVisible);
    Vector3 pivotSymbolPosition = this.transform.GetPosition();
    if (symbolVisible)
      pivotSymbolPosition = new Vector3(symbolTransform[0, 3], symbolTransform[1, 3], symbolTransform[2, 3]);
    return pivotSymbolPosition;
  }

  public virtual Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
  {
    symbolVisible = false;
    return Matrix4x4.identity;
  }

  private void Awake()
  {
    this.aem = Singleton<AnimEventManager>.Instance;
    this.SetFGLayer(this.fgLayer);
    this.OnAwake();
    if (!string.IsNullOrEmpty(this.initialAnim))
    {
      this.SetDirty();
      this.Play((HashedString) this.initialAnim, this.initialMode);
    }
    this.hasAwakeRun = true;
  }

  private void Start() => this.OnStart();

  protected virtual void OnDestroy()
  {
    this.animFiles = (KAnimFile[]) null;
    this.curAnim = (KAnim.Anim) null;
    this.curBuild = (KAnim.Build) null;
    this.synchronizer = (KAnimSynchronizer) null;
    this.layering = (KAnimLayering) null;
    this.animQueue = (System.Collections.Generic.Queue<KAnimControllerBase.AnimData>) null;
    this.overrideAnims = (Dictionary<HashedString, KAnimControllerBase.AnimLookupData>) null;
    this.anims = (Dictionary<HashedString, KAnimControllerBase.AnimLookupData>) null;
    this.synchronizer = (KAnimSynchronizer) null;
    this.layering = (KAnimLayering) null;
    this.overrideAnimFiles = (List<KAnimControllerBase.OverrideAnimFileData>) null;
  }

  protected void AnimEnter(HashedString hashed_name)
  {
    if (this.onAnimEnter == null)
      return;
    this.onAnimEnter(hashed_name);
  }

  public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0.0f)
  {
    if (!this.stopped)
      this.Stop();
    this.Queue(anim_name, mode, speed, time_offset);
  }

  public void Play(HashedString[] anim_names, KAnim.PlayMode mode = KAnim.PlayMode.Once)
  {
    if (!this.stopped)
      this.Stop();
    for (int index = 0; index < anim_names.Length - 1; ++index)
      this.Queue(anim_names[index]);
    Debug.Assert(anim_names.Length != 0, (object) "Play was called with an empty anim array");
    this.Queue(anim_names[anim_names.Length - 1], mode);
  }

  public void Queue(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0.0f)
  {
    this.animQueue.Enqueue(new KAnimControllerBase.AnimData()
    {
      anim = anim_name,
      mode = mode,
      speed = speed,
      timeOffset = time_offset
    });
    this.mode = mode == KAnim.PlayMode.Paused ? KAnim.PlayMode.Paused : KAnim.PlayMode.Once;
    if (this.aem != null)
      this.aem.SetMode(this.eventManagerHandle, this.mode);
    if (this.animQueue.Count != 1 || !this.stopped)
      return;
    this.StartQueuedAnim();
  }

  public void QueueAndSyncTransition(
    HashedString anim_name,
    KAnim.PlayMode mode = KAnim.PlayMode.Once,
    float speed = 1f,
    float time_offset = 0.0f)
  {
    this.SyncTransition();
    this.Queue(anim_name, mode, speed, time_offset);
  }

  public void SyncTransition() => this.elapsedTime %= Mathf.Max(float.Epsilon, this.GetDuration());

  public void ClearQueue() => this.animQueue.Clear();

  private void Restart(
    HashedString anim_name,
    KAnim.PlayMode mode = KAnim.PlayMode.Once,
    float speed = 1f,
    float time_offset = 0.0f)
  {
    if (this.curBuild == null)
    {
      Debug.LogWarning((object) $"[{this.gameObject.name}] Missing build while trying to play anim [{anim_name.ToString()}]", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      System.Collections.Generic.Queue<KAnimControllerBase.AnimData> animDataQueue = new System.Collections.Generic.Queue<KAnimControllerBase.AnimData>();
      animDataQueue.Enqueue(new KAnimControllerBase.AnimData()
      {
        anim = anim_name,
        mode = mode,
        speed = speed,
        timeOffset = time_offset
      });
      while (this.animQueue.Count > 0)
        animDataQueue.Enqueue(this.animQueue.Dequeue());
      this.animQueue = animDataQueue;
      if (this.animQueue.Count != 1 || !this.stopped)
        return;
      this.StartQueuedAnim();
    }
  }

  protected void StartQueuedAnim()
  {
    this.StopAnimEventSequence();
    this.previousFrame = -1;
    this.currentFrame = -1;
    this.SuspendUpdates(false);
    this.stopped = false;
    this.OnStartQueuedAnim();
    KAnimControllerBase.AnimData animData = this.animQueue.Dequeue();
    while (animData.mode == KAnim.PlayMode.Loop && this.animQueue.Count > 0)
      animData = this.animQueue.Dequeue();
    KAnimControllerBase.AnimLookupData animLookupData;
    if (this.overrideAnims == null || !this.overrideAnims.TryGetValue(animData.anim, out animLookupData))
    {
      if (!this.anims.TryGetValue(animData.anim, out animLookupData))
      {
        if ((UnityEngine.Object) this.showWhenMissing != (UnityEngine.Object) null)
          this.showWhenMissing.SetActive(true);
        if (true)
        {
          this.TriggerStop();
          return;
        }
      }
      else if ((UnityEngine.Object) this.showWhenMissing != (UnityEngine.Object) null)
        this.showWhenMissing.SetActive(false);
    }
    this.curAnim = this.GetAnim(animLookupData.animIndex);
    int offset = 0;
    if (animData.mode == KAnim.PlayMode.Loop && this.randomiseLoopedOffset)
      offset = UnityEngine.Random.Range(0, this.curAnim.numFrames - 1);
    this.prevAnimFrame = -1;
    this.curAnimFrameIdx = this.GetFrameIdxFromOffset(offset);
    this.currentFrame = this.curAnimFrameIdx;
    this.mode = animData.mode;
    this.playSpeed = animData.speed * this.PlaySpeedMultiplier;
    this.SetElapsedTime((float) offset / this.curAnim.frameRate + animData.timeOffset);
    this.synchronizer.Sync();
    this.StartAnimEventSequence();
    this.AnimEnter(animData.anim);
  }

  public bool GetSymbolVisiblity(KAnimHashedString symbol)
  {
    return !this.hiddenSymbolsSet.Contains(symbol);
  }

  public void SetSymbolVisiblity(KAnimHashedString symbol, bool is_visible)
  {
    if (is_visible)
      this.hiddenSymbolsSet.Remove(symbol);
    else if (!this.hiddenSymbolsSet.Contains(symbol))
      this.hiddenSymbolsSet.Add(symbol);
    if (this.curBuild == null)
      return;
    this.UpdateHiddenSymbol(symbol);
  }

  public void BatchSetSymbolsVisiblity(HashSet<KAnimHashedString> symbols, bool is_visible)
  {
    foreach (KAnimHashedString symbol in symbols)
    {
      if (is_visible)
        this.hiddenSymbolsSet.Remove(symbol);
      else if (!this.hiddenSymbolsSet.Contains(symbol))
        this.hiddenSymbolsSet.Add(symbol);
    }
    if (this.curBuild == null)
      return;
    this.UpdateHiddenSymbolSet(symbols);
  }

  public void AddAnimOverrides(KAnimFile kanim_file, float priority = 0.0f)
  {
    if ((UnityEngine.Object) kanim_file == (UnityEngine.Object) null)
      Debug.LogError((object) $"AddAnimOverrides tried to add a null override to {this.gameObject.name} at position {this.transform.position}");
    if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
    {
      SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
      DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, "Anim overrides containing additional symbols require a symbol override controller.");
      component.AddBuildOverride(kanim_file.GetData());
    }
    this.overrideAnimFiles.Add(new KAnimControllerBase.OverrideAnimFileData()
    {
      priority = priority,
      file = kanim_file
    });
    this.overrideAnimFiles.Sort((Comparison<KAnimControllerBase.OverrideAnimFileData>) ((a, b) => b.priority.CompareTo(a.priority)));
    this.RebuildOverrides(kanim_file);
  }

  public void RemoveAnimOverrides(KAnimFile kanim_file)
  {
    if ((UnityEngine.Object) kanim_file == (UnityEngine.Object) null)
      Debug.LogError((object) $"RemoveAnimOverrides tried to remove a null override to {this.gameObject.name} at position {this.transform.position}");
    if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
    {
      SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
      DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, "Anim overrides containing additional symbols require a symbol override controller.");
      component.TryRemoveBuildOverride(kanim_file.GetData());
    }
    for (int index = 0; index < this.overrideAnimFiles.Count; ++index)
    {
      if ((UnityEngine.Object) this.overrideAnimFiles[index].file == (UnityEngine.Object) kanim_file)
      {
        this.overrideAnimFiles.RemoveAt(index);
        break;
      }
    }
    this.RebuildOverrides(kanim_file);
  }

  private void RebuildOverrides(KAnimFile kanim_file)
  {
    bool flag = false;
    this.overrideAnims.Clear();
    for (int index1 = 0; index1 < this.overrideAnimFiles.Count; ++index1)
    {
      KAnimControllerBase.OverrideAnimFileData overrideAnimFile = this.overrideAnimFiles[index1];
      KAnimFileData data = overrideAnimFile.file.GetData();
      for (int index2 = 0; index2 < data.animCount; ++index2)
      {
        KAnim.Anim anim = data.GetAnim(index2);
        if (anim.animFile.hashName != data.hashName)
          Debug.LogError((object) $"How did we get an anim from another file? [{data.name}] != [{anim.animFile.name}] for anim [{index2}]");
        KAnimControllerBase.AnimLookupData animLookupData = new KAnimControllerBase.AnimLookupData();
        animLookupData.animIndex = anim.index;
        HashedString key = new HashedString(anim.name);
        if (!this.overrideAnims.ContainsKey(key))
          this.overrideAnims[key] = animLookupData;
        if (this.curAnim != null && this.curAnim.hash == key && (UnityEngine.Object) overrideAnimFile.file == (UnityEngine.Object) kanim_file)
          flag = true;
      }
    }
    if (!flag)
      return;
    this.Restart((HashedString) this.curAnim.name, this.mode, this.playSpeed);
  }

  public bool HasAnimation(HashedString anim_name)
  {
    bool flag = anim_name.IsValid;
    if (flag)
    {
      int num = this.anims.ContainsKey(anim_name) ? 1 : 0;
      flag = (num | (num != 0 ? (false ? 1 : 0) : (this.overrideAnims.ContainsKey(anim_name) ? 1 : 0))) != 0;
    }
    return flag;
  }

  public KAnim.Anim GetAnim(HashedString anim_name)
  {
    KAnim.Anim anim = (KAnim.Anim) null;
    if (anim_name.IsValid)
    {
      KAnimControllerBase.AnimLookupData animLookupData;
      if (this.anims.TryGetValue(anim_name, out animLookupData))
        anim = this.GetAnim(animLookupData.animIndex);
      else if (this.overrideAnims.TryGetValue(anim_name, out animLookupData))
        anim = this.GetAnim(animLookupData.animIndex);
    }
    return anim;
  }

  public bool HasAnimationFile(KAnimHashedString anim_file_name)
  {
    KAnimFile match = (KAnimFile) null;
    return this.TryGetAnimationFile(anim_file_name, out match);
  }

  public bool TryGetAnimationFile(KAnimHashedString anim_file_name, out KAnimFile match)
  {
    match = (KAnimFile) null;
    if (!anim_file_name.IsValid())
      return false;
    KAnimFileData kanimFileData1 = (KAnimFileData) null;
    int index1 = 0;
    int index2 = this.overrideAnimFiles.Count - 1;
    int num1 = (int) ((double) this.overrideAnimFiles.Count * 0.5);
    while (num1 > 0 && (UnityEngine.Object) match == (UnityEngine.Object) null && index1 < num1)
    {
      if ((UnityEngine.Object) this.overrideAnimFiles[index1].file != (UnityEngine.Object) null)
        kanimFileData1 = this.overrideAnimFiles[index1].file.GetData();
      if (kanimFileData1 != null && kanimFileData1.hashName.HashValue == anim_file_name.HashValue)
      {
        match = this.overrideAnimFiles[index1].file;
        break;
      }
      if ((UnityEngine.Object) this.overrideAnimFiles[index2].file != (UnityEngine.Object) null)
        kanimFileData1 = this.overrideAnimFiles[index2].file.GetData();
      if (kanimFileData1 != null && kanimFileData1.hashName.HashValue == anim_file_name.HashValue)
        match = this.overrideAnimFiles[index2].file;
      ++index1;
      --index2;
    }
    if ((UnityEngine.Object) match == (UnityEngine.Object) null && this.overrideAnimFiles.Count % 2 != 0)
    {
      if ((UnityEngine.Object) this.overrideAnimFiles[index1].file != (UnityEngine.Object) null)
        kanimFileData1 = this.overrideAnimFiles[index1].file.GetData();
      if (kanimFileData1 != null && kanimFileData1.hashName.HashValue == anim_file_name.HashValue)
        match = this.overrideAnimFiles[index1].file;
    }
    KAnimFileData kanimFileData2 = (KAnimFileData) null;
    if ((UnityEngine.Object) match == (UnityEngine.Object) null && this.animFiles != null)
    {
      int index3 = 0;
      int index4 = this.animFiles.Length - 1;
      int num2 = (int) ((double) this.animFiles.Length * 0.5);
      while (num2 > 0 && (UnityEngine.Object) match == (UnityEngine.Object) null && index3 < num2)
      {
        if ((UnityEngine.Object) this.animFiles[index3] != (UnityEngine.Object) null)
          kanimFileData2 = this.animFiles[index3].GetData();
        if (kanimFileData2 != null && kanimFileData2.hashName.HashValue == anim_file_name.HashValue)
        {
          match = this.animFiles[index3];
          break;
        }
        if ((UnityEngine.Object) this.animFiles[index4] != (UnityEngine.Object) null)
          kanimFileData2 = this.animFiles[index4].GetData();
        if (kanimFileData2 != null && kanimFileData2.hashName.HashValue == anim_file_name.HashValue)
          match = this.animFiles[index4];
        ++index3;
        --index4;
      }
      if ((UnityEngine.Object) match == (UnityEngine.Object) null && this.animFiles.Length % 2 != 0)
      {
        if ((UnityEngine.Object) this.animFiles[index3] != (UnityEngine.Object) null)
          kanimFileData2 = this.animFiles[index3].GetData();
        if (kanimFileData2 != null && kanimFileData2.hashName.HashValue == anim_file_name.HashValue)
          match = this.animFiles[index3];
      }
    }
    return (UnityEngine.Object) match != (UnityEngine.Object) null;
  }

  public void AddAnims(KAnimFile anim_file)
  {
    KAnimFileData data = anim_file.GetData();
    if (data == null)
    {
      Debug.LogError((object) "AddAnims() Null animfile data");
    }
    else
    {
      this.maxSymbols = Mathf.Max(this.maxSymbols, data.maxVisSymbolFrames);
      for (int index = 0; index < data.animCount; ++index)
      {
        KAnim.Anim anim = data.GetAnim(index);
        if (anim.animFile.hashName != data.hashName)
          Debug.LogErrorFormat("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", (object) data.name, (object) anim.animFile.name, (object) index);
        this.anims[anim.hash] = new KAnimControllerBase.AnimLookupData()
        {
          animIndex = anim.index
        };
      }
      if (!this.usingNewSymbolOverrideSystem || data.buildIndex == -1 || data.build.symbols == null || data.build.symbols.Length == 0)
        return;
      this.GetComponent<SymbolOverrideController>().AddBuildOverride(anim_file.GetData(), -1);
    }
  }

  public KAnimFile[] AnimFiles
  {
    get => this.animFiles;
    set
    {
      DebugUtil.AssertArgs((value.Length != 0 ? 1 : 0) != 0, (object) "Controller has no anim files.", (object) this.gameObject);
      DebugUtil.AssertArgs(((UnityEngine.Object) value[0] != (UnityEngine.Object) null ? 1 : 0) != 0, (object) "First anim file needs to be non-null.", (object) this.gameObject);
      DebugUtil.AssertArgs((value[0].IsBuildLoaded ? 1 : 0) != 0, (object) "First anim file needs to be the build file.", (object) this.gameObject);
      for (int index = 0; index < value.Length; ++index)
        DebugUtil.AssertArgs(((UnityEngine.Object) value[index] != (UnityEngine.Object) null ? 1 : 0) != 0, (object) "Anim file is null", (object) this.gameObject);
      this.animFiles = new KAnimFile[value.Length];
      for (int index = 0; index < value.Length; ++index)
        this.animFiles[index] = value[index];
    }
  }

  public IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> OverrideAnimFiles
  {
    get => (IReadOnlyList<KAnimControllerBase.OverrideAnimFileData>) this.overrideAnimFiles;
  }

  public void Stop()
  {
    if (this.curAnim != null)
      this.StopAnimEventSequence();
    this.animQueue.Clear();
    this.stopped = true;
    if (this.onAnimComplete != null)
      this.onAnimComplete(this.curAnim == null ? HashedString.Invalid : this.curAnim.hash);
    this.OnStop();
  }

  public void StopAndClear()
  {
    if (!this.stopped)
      this.Stop();
    this.bounds.center = Vector3.zero;
    this.bounds.extents = Vector3.zero;
    if (this.OnUpdateBounds == null)
      return;
    this.OnUpdateBounds(this.bounds);
  }

  public float GetPositionPercent() => this.GetElapsedTime() / this.GetDuration();

  public void SetPositionPercent(float percent)
  {
    if (this.curAnim == null)
      return;
    this.SetElapsedTime(percent * (float) this.curAnim.numFrames / this.curAnim.frameRate);
    if (this.currentFrame == this.curAnim.GetFrameIdx(this.mode, this.elapsedTime))
      return;
    this.SetDirty();
    this.UpdateAnimEventSequenceTime();
    this.SuspendUpdates(false);
  }

  protected void StartAnimEventSequence()
  {
    if (this.layering.GetIsLayer() || this.aem == null)
      return;
    this.eventManagerHandle = this.aem.PlayAnim(this, this.curAnim, this.mode, this.elapsedTime, this.visibilityType == KAnimControllerBase.VisibilityType.Always);
  }

  protected void UpdateAnimEventSequenceTime()
  {
    if (!this.eventManagerHandle.IsValid() || this.aem == null)
      return;
    this.aem.SetElapsedTime(this.eventManagerHandle, this.elapsedTime);
  }

  protected void StopAnimEventSequence()
  {
    if (!this.eventManagerHandle.IsValid() || this.aem == null)
      return;
    if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
      this.SetElapsedTime(this.aem.GetElapsedTime(this.eventManagerHandle));
    this.aem.StopAnim(this.eventManagerHandle);
    this.eventManagerHandle = HandleVector<int>.InvalidHandle;
  }

  protected void DestroySelf()
  {
    if (this.onDestroySelf != null)
      this.onDestroySelf(this.gameObject);
    else
      Util.KDestroyGameObject(this.gameObject);
  }

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
    this.hiddenSymbols.Clear();
    this.hiddenSymbols = new List<KAnimHashedString>((IEnumerable<KAnimHashedString>) this.hiddenSymbolsSet);
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    this.hiddenSymbolsSet = new HashSet<KAnimHashedString>((IEnumerable<KAnimHashedString>) this.hiddenSymbols);
    this.hiddenSymbols.Clear();
  }

  public struct OverrideAnimFileData
  {
    public float priority;
    public KAnimFile file;
  }

  public struct AnimLookupData
  {
    public int animIndex;
  }

  public struct AnimData
  {
    public HashedString anim;
    public KAnim.PlayMode mode;
    public float speed;
    public float timeOffset;
  }

  public enum VisibilityType
  {
    Default,
    OffscreenUpdate,
    Always,
  }

  public delegate void KAnimEvent(HashedString name);
}
