// Decompiled with JetBrains decompiler
// Type: Klei.AI.Emote
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class Emote : Resource
{
  private HashedString animSetName = (HashedString) (string) null;
  private KAnimFile animSet;
  private HashedString swimAnimSetName = (HashedString) (string) null;
  private KAnimFile swimAnimSet;
  private List<EmoteStep> emoteSteps = new List<EmoteStep>();

  public int StepCount => this.emoteSteps != null ? this.emoteSteps.Count : 0;

  public KAnimFile AnimSet
  {
    get
    {
      if (this.animSetName != HashedString.Invalid && (Object) this.animSet == (Object) null)
        this.animSet = Assets.GetAnim(this.animSetName);
      return this.animSet;
    }
  }

  public bool IsValid { get; private set; }

  public KAnimFile ManifestSwimAnimSet()
  {
    if (this.swimAnimSetName != (HashedString) (string) null && (Object) this.swimAnimSet == (Object) null)
      this.swimAnimSet = Assets.GetAnim(this.swimAnimSetName);
    return this.swimAnimSet;
  }

  public Emote(
    ResourceSet parent,
    string emoteId,
    EmoteStep[] defaultSteps,
    string animSetName = null,
    string swimAnimSetName = null)
    : base(emoteId, parent)
  {
    this.emoteSteps.AddRange((IEnumerable<EmoteStep>) defaultSteps);
    this.animSetName = (HashedString) animSetName;
    this.swimAnimSetName = (HashedString) swimAnimSetName;
    this.IsValid = this.Validate();
  }

  private bool Validate()
  {
    KAnimFileData data = (Object) this.AnimSet == (Object) null ? (KAnimFileData) null : this.AnimSet.GetData();
    if (data == null)
      return false;
    for (int index1 = 0; index1 < this.StepCount; ++index1)
    {
      bool flag = false;
      for (int index2 = 0; index2 < data.animCount; ++index2)
      {
        if ((HashedString) data.GetAnim(index2).name == this.emoteSteps[index1].anim)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        Debug.LogWarningFormat("Emote AnimFile [{0}] does not have animations for emote step [{1}]", (object) this.animSetName, (object) this.emoteSteps[index1].anim);
        return false;
      }
    }
    return true;
  }

  public void ApplyAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
  {
    KAnimFile kanim_file = (Object) overrideSet != (Object) null ? overrideSet : this.AnimSet;
    if ((Object) kanim_file == (Object) null || (Object) animController == (Object) null)
      return;
    animController.AddAnimOverrides(kanim_file);
  }

  public void RemoveAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
  {
    KAnimFile kanim_file = (Object) overrideSet != (Object) null ? overrideSet : this.AnimSet;
    if ((Object) kanim_file == (Object) null || (Object) animController == (Object) null)
      return;
    animController.RemoveAnimOverrides(kanim_file);
  }

  public void CollectStepAnims(out HashedString[] emoteAnims, int iterations)
  {
    emoteAnims = new HashedString[this.emoteSteps.Count * iterations];
    for (int index = 0; index < emoteAnims.Length; ++index)
      emoteAnims[index] = this.emoteSteps[index % this.emoteSteps.Count].anim;
  }

  public bool IsValidStep(int stepIdx) => stepIdx >= 0 && stepIdx < this.emoteSteps.Count;

  public EmoteStep this[int stepIdx]
  {
    get => !this.IsValidStep(stepIdx) ? (EmoteStep) null : this.emoteSteps[stepIdx];
  }

  public int GetStepIndex(HashedString animName)
  {
    int index = 0;
    bool condition = false;
    for (; index < this.emoteSteps.Count; ++index)
    {
      if (this.emoteSteps[index].anim == animName)
      {
        condition = true;
        break;
      }
    }
    Debug.Assert(condition, (object) $"Could not find emote step {animName} for emote {this.Id}!");
    return index;
  }
}
