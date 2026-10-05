// Decompiled with JetBrains decompiler
// Type: DestroyAfter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/DestroyAfter")]
public class DestroyAfter : KMonoBehaviour
{
  private ParticleSystem[] particleSystems;

  protected override void OnSpawn()
  {
    this.particleSystems = this.gameObject.GetComponentsInChildren<ParticleSystem>(true);
  }

  private bool IsAlive()
  {
    for (int index = 0; index < this.particleSystems.Length; ++index)
    {
      if (this.particleSystems[index].IsAlive(false))
        return true;
    }
    return false;
  }

  private void Update()
  {
    if (this.particleSystems == null || this.IsAlive())
      return;
    this.DeleteObject();
  }
}
