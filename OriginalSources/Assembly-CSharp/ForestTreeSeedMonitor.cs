// Decompiled with JetBrains decompiler
// Type: ForestTreeSeedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
public class ForestTreeSeedMonitor : KMonoBehaviour
{
  [Serialize]
  private bool hasExtraSeedAvailable;

  public bool ExtraSeedAvailable => this.hasExtraSeedAvailable;

  public void ExtractExtraSeed()
  {
    if (!this.hasExtraSeedAvailable)
      return;
    this.hasExtraSeedAvailable = false;
    Vector3 position = this.transform.position with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
    };
    Util.KInstantiate(Assets.GetPrefab((Tag) "ForestTreeSeed"), position).SetActive(true);
  }

  public void TryRollNewSeed()
  {
    if (this.hasExtraSeedAvailable || Random.Range(0, 100) >= 5)
      return;
    this.hasExtraSeedAvailable = true;
  }
}
