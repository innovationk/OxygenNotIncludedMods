// Decompiled with JetBrains decompiler
// Type: CO2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour
{
  [Serialize]
  [NonSerialized]
  public Vector3 velocity = Vector3.zero;
  [Serialize]
  [NonSerialized]
  public float mass;
  [Serialize]
  [NonSerialized]
  public float temperature;
  [Serialize]
  [NonSerialized]
  public float lifetimeRemaining;
  [Serialize]
  [NonSerialized]
  public string kAnimFileName = "exhale_kanim";
  [Serialize]
  [NonSerialized]
  public string anim_name_pre = "exhale_pre";
  [Serialize]
  [NonSerialized]
  public string anim_name_loop = "exhale_loop";
  [Serialize]
  [NonSerialized]
  public string anim_name_pst = "exhale_pst";
  [Serialize]
  [NonSerialized]
  public bool affectedByGravity = true;

  protected override void OnSpawn() => base.OnSpawn();

  public void StartLoop()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) this.anim_name_pre);
    component.Play((HashedString) this.anim_name_loop, KAnim.PlayMode.Loop);
  }

  public void TriggerDestroy()
  {
    this.GetComponent<KBatchedAnimController>().Play((HashedString) this.anim_name_pst);
  }
}
