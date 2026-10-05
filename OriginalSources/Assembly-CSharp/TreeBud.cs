// Decompiled with JetBrains decompiler
// Type: TreeBud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/TreeBud")]
public class TreeBud : KMonoBehaviour
{
  [Serialize]
  public Ref<BuddingTrunk> buddingTrunk;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    PlantBranch.Instance smi = this.gameObject.GetSMI<PlantBranch.Instance>();
    if (smi == null || smi.IsRunning())
      return;
    smi.StartSM();
  }

  public BuddingTrunk GetAndForgetOldTrunk()
  {
    BuddingTrunk andForgetOldTrunk = this.buddingTrunk == null ? (BuddingTrunk) null : this.buddingTrunk.Get();
    this.buddingTrunk = (Ref<BuddingTrunk>) null;
    return andForgetOldTrunk;
  }
}
