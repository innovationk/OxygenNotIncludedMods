// Decompiled with JetBrains decompiler
// Type: SlimeMoldConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SlimeMoldConfig : IOreConfig
{
  public SimHashes ElementID => SimHashes.SlimeMold;

  public SimHashes SublimeElementID => SimHashes.ContaminatedOxygen;

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
    sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0.0f, this.SublimeElementID);
    return solidOreEntity;
  }
}
