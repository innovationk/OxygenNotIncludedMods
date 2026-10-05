// Decompiled with JetBrains decompiler
// Type: SymbolOverrideController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SymbolOverrideController")]
public class SymbolOverrideController : KMonoBehaviour
{
  public bool applySymbolOverridesEveryFrame;
  [SerializeField]
  private List<SymbolOverrideController.SymbolEntry> symbolOverrides = new List<SymbolOverrideController.SymbolEntry>();
  private KAnimBatch.AtlasList atlases;
  private KBatchedAnimController animController;
  private FaceGraph faceGraph;
  private bool requiresSorting;

  public SymbolOverrideController.SymbolEntry[] GetSymbolOverrides
  {
    get => this.symbolOverrides.ToArray();
  }

  public int version { get; private set; }

  protected override void OnPrefabInit()
  {
    this.animController = this.GetComponent<KBatchedAnimController>();
    DebugUtil.Assert((UnityEngine.Object) this.GetComponent<KBatchedAnimController>() != (UnityEngine.Object) null, "SymbolOverrideController requires KBatchedAnimController");
    DebugUtil.Assert(this.GetComponent<KBatchedAnimController>().usingNewSymbolOverrideSystem, "SymbolOverrideController requires usingNewSymbolOverrideSystem to be set to true. Try adding the component by calling: SymbolOverrideControllerUtil.AddToPrefab");
    for (int index = 0; index < this.symbolOverrides.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolOverride = this.symbolOverrides[index];
      symbolOverride.sourceSymbol = KAnimBatchManager.Instance().GetBatchGroupData(symbolOverride.sourceSymbolBatchTag).GetSymbol((KAnimHashedString) symbolOverride.sourceSymbolId);
      this.symbolOverrides[index] = symbolOverride;
    }
    this.atlases = new KAnimBatch.AtlasList(0, KAnimBatchManager.MaxAtlasesByMaterialType[(int) this.animController.materialType]);
    this.faceGraph = this.GetComponent<FaceGraph>();
  }

  public int AddSymbolOverride(
    HashedString target_symbol,
    KAnim.Build.Symbol source_symbol,
    int priority = 0)
  {
    if (source_symbol == null)
      throw new Exception("NULL source symbol when overriding: " + target_symbol.ToString());
    SymbolOverrideController.SymbolEntry symbolEntry = new SymbolOverrideController.SymbolEntry()
    {
      targetSymbol = target_symbol,
      sourceSymbol = source_symbol,
      sourceSymbolId = new HashedString(source_symbol.hash.HashValue),
      sourceSymbolBatchTag = source_symbol.build.batchTag,
      priority = priority
    };
    int index = this.GetSymbolOverrideIdx(target_symbol, priority);
    if (index >= 0)
    {
      this.symbolOverrides[index] = symbolEntry;
    }
    else
    {
      index = this.symbolOverrides.Count;
      this.symbolOverrides.Add(symbolEntry);
    }
    this.MarkDirty();
    return index;
  }

  public bool RemoveSymbolOverride(HashedString target_symbol, int priority = 0)
  {
    for (int index = 0; index < this.symbolOverrides.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolOverride = this.symbolOverrides[index];
      if (symbolOverride.targetSymbol == target_symbol && symbolOverride.priority == priority)
      {
        this.symbolOverrides.RemoveAt(index);
        this.MarkDirty();
        return true;
      }
    }
    return false;
  }

  public void RemoveAllSymbolOverrides(int priority = 0)
  {
    this.symbolOverrides.RemoveAll((Predicate<SymbolOverrideController.SymbolEntry>) (x => x.priority >= priority));
    this.MarkDirty();
  }

  public int GetSymbolOverrideIdx(HashedString target_symbol, int priority = 0)
  {
    for (int index = 0; index < this.symbolOverrides.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolOverride = this.symbolOverrides[index];
      if (symbolOverride.targetSymbol == target_symbol && symbolOverride.priority == priority)
        return index;
    }
    return -1;
  }

  public int GetAtlasIdx(Texture2D atlas) => this.atlases.GetAtlasIdx(atlas);

  public void ApplyOverrides()
  {
    if (this.requiresSorting)
    {
      this.symbolOverrides.Sort((Comparison<SymbolOverrideController.SymbolEntry>) ((x, y) => x.priority - y.priority));
      this.requiresSorting = false;
    }
    KAnimBatch batch = this.animController.GetBatch();
    DebugUtil.Assert(batch != null);
    KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.animController.batchGroupID);
    this.atlases.Clear(batch.atlases.Count);
    DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.PooledDictionary pooledDictionary1 = DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.Allocate();
    ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.PooledList pooledList = ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.Allocate();
    for (int index = 0; index < this.symbolOverrides.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolOverride = this.symbolOverrides[index];
      Pair<int, int> pair;
      if (pooledDictionary1.TryGetValue(symbolOverride.targetSymbol, out pair))
      {
        int first = pair.first;
        if (symbolOverride.priority > first)
        {
          int second = pair.second;
          pooledDictionary1[symbolOverride.targetSymbol] = new Pair<int, int>(symbolOverride.priority, second);
          pooledList[second] = symbolOverride;
        }
      }
      else
      {
        pooledDictionary1[symbolOverride.targetSymbol] = new Pair<int, int>(symbolOverride.priority, pooledList.Count);
        pooledList.Add(symbolOverride);
      }
    }
    DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.PooledDictionary pooledDictionary2 = DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.Allocate();
    for (int index = 0; index < pooledList.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolEntry = pooledList[index];
      SymbolOverrideController.BatchGroupInfo batchGroupInfo;
      if (!pooledDictionary2.TryGetValue(symbolEntry.sourceSymbol.build, out batchGroupInfo))
      {
        batchGroupInfo = new SymbolOverrideController.BatchGroupInfo()
        {
          build = symbolEntry.sourceSymbol.build,
          data = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry.sourceSymbol.build.batchTag)
        };
        Texture2D texture = symbolEntry.sourceSymbol.build.GetTexture(0);
        int num = batch.atlases.GetAtlasIdx(texture);
        if (num < 0)
          num = this.atlases.Add(texture);
        batchGroupInfo.atlasIdx = num;
        pooledDictionary2[batchGroupInfo.build] = batchGroupInfo;
      }
      KAnim.Build.Symbol symbol = batchGroupData.GetSymbol((KAnimHashedString) symbolEntry.targetSymbol);
      if (symbol != null)
        this.animController.SetSymbolOverrides(symbol.firstFrameIdx, symbol.numFrames, batchGroupInfo.atlasIdx, batchGroupInfo.data, symbolEntry.sourceSymbol.firstFrameIdx, symbolEntry.sourceSymbol.numFrames);
    }
    pooledDictionary2.Recycle();
    pooledList.Recycle();
    pooledDictionary1.Recycle();
    if (!((UnityEngine.Object) this.faceGraph != (UnityEngine.Object) null))
      return;
    this.faceGraph.ApplyShape();
  }

  public void ApplyAtlases() => this.atlases.Apply(this.animController.GetBatch().matProperties);

  public KAnimBatch.AtlasList GetAtlasList() => this.atlases;

  public new void MarkDirty()
  {
    if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null)
      this.animController.SetDirty();
    ++this.version;
    this.requiresSorting = true;
  }

  [Serializable]
  public struct SymbolEntry
  {
    public HashedString targetSymbol;
    [NonSerialized]
    public KAnim.Build.Symbol sourceSymbol;
    public HashedString sourceSymbolId;
    public HashedString sourceSymbolBatchTag;
    public int priority;
  }

  private struct SymbolToOverride
  {
    public KAnim.Build.Symbol sourceSymbol;
    public HashedString targetSymbol;
    public KBatchGroupData data;
    public int atlasIdx;
  }

  private struct BatchGroupInfo
  {
    public KAnim.Build build;
    public int atlasIdx;
    public KBatchGroupData data;
  }
}
