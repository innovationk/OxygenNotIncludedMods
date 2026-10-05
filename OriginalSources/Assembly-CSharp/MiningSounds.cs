// Decompiled with JetBrains decompiler
// Type: MiningSounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
  private static HashedString HASH_PERCENTCOMPLETE = (HashedString) "percentComplete";
  [MyCmpGet]
  private LoopingSounds loopingSounds;
  private FMODAsset miningSound;
  private EventReference miningSoundEvent;
  private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>((Action<MiningSounds, object>) ((component, data) => component.OnStartMiningSound(data)));
  private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>((Action<MiningSounds, object>) ((component, data) => component.OnStopMiningSound(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<MiningSounds>(-1762453998, MiningSounds.OnStartMiningSoundDelegate);
    this.Subscribe<MiningSounds>(939543986, MiningSounds.OnStopMiningSoundDelegate);
  }

  private void OnStartMiningSound(object data)
  {
    if ((UnityEngine.Object) this.miningSound != (UnityEngine.Object) null || !(data is Element element))
      return;
    string miningSound = element.substance.GetMiningSound();
    switch (miningSound)
    {
      case null:
        break;
      case "":
        break;
      default:
        if (this.IsTargetCellLiquid())
          break;
        this.miningSoundEvent = RuntimeManager.PathToEventReference(GlobalAssets.GetSound("Mine_" + miningSound));
        DebugUtil.DevAssert(!this.miningSoundEvent.IsNull, "Failed to find mining sound event for element");
        if (this.miningSoundEvent.IsNull)
          break;
        this.loopingSounds.StartSound(this.miningSoundEvent);
        break;
    }
  }

  private void OnStopMiningSound(object data)
  {
    if (this.miningSoundEvent.IsNull)
      return;
    this.loopingSounds.StopSound(this.miningSoundEvent);
    this.miningSound = (FMODAsset) null;
  }

  public void SetPercentComplete(float progress)
  {
    if (this.miningSoundEvent.IsNull)
      return;
    this.loopingSounds.SetParameter(this.miningSoundEvent, MiningSounds.HASH_PERCENTCOMPLETE, progress);
  }

  private bool IsTargetCellLiquid()
  {
    WorkerBase component;
    if (!this.TryGetComponent<WorkerBase>(out component))
      return false;
    Workable workable = component.GetWorkable();
    return !((UnityEngine.Object) workable == (UnityEngine.Object) null) && Grid.IsLiquid(Grid.PosToCell((KMonoBehaviour) workable));
  }
}
