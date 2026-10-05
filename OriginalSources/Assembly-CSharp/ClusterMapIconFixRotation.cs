// Decompiled with JetBrains decompiler
// Type: ClusterMapIconFixRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClusterMapIconFixRotation : KMonoBehaviour
{
  [MyCmpGet]
  private KBatchedAnimController animController;
  private float rotation;

  private void Update()
  {
    if (!((Object) this.transform.parent != (Object) null))
      return;
    this.rotation = -this.transform.parent.rotation.eulerAngles.z;
    this.animController.Rotation = this.rotation;
  }
}
