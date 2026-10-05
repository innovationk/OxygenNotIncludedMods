// Decompiled with JetBrains decompiler
// Type: Unsealable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Unsealable")]
public class Unsealable : Workable
{
  [Serialize]
  public bool facingRight;
  [Serialize]
  public bool unsealed;

  private Unsealable()
  {
  }

  public override CellOffset[] GetOffsets(int cell)
  {
    return this.facingRight ? OffsetGroups.RightOnly : OffsetGroups.LeftOnly;
  }

  protected override void OnPrefabInit()
  {
    this.faceTargetWhenWorking = true;
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_door_poi_kanim")
    };
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(3f);
    if (!this.unsealed)
      return;
    Deconstructable component = this.GetComponent<Deconstructable>();
    if (!((Object) component != (Object) null))
      return;
    component.allowDeconstruction = true;
  }

  protected override void OnStartWork(WorkerBase worker) => base.OnStartWork(worker);

  protected override void OnCompleteWork(WorkerBase worker)
  {
    this.unsealed = true;
    base.OnCompleteWork(worker);
    Deconstructable component = this.GetComponent<Deconstructable>();
    if (!((Object) component != (Object) null))
      return;
    component.allowDeconstruction = true;
    Game.Instance.Trigger(1980521255, (object) this.gameObject);
  }
}
