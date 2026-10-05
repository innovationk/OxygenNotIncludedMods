// Decompiled with JetBrains decompiler
// Type: OxyRockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OxyRockConfig : IOreConfig
{
  public SimHashes ElementID => SimHashes.OxyRock;

  public SimHashes SublimeElementID => SimHashes.Oxygen;

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
    sublimates.info = new Sublimates.Info(0.0100000007f, 0.00500000035f, 1.8f, 0.7f, this.SublimeElementID);
    return solidOreEntity;
  }
}
