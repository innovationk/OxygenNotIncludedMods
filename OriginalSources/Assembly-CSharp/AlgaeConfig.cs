// Decompiled with JetBrains decompiler
// Type: AlgaeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AlgaeConfig : IOreConfig
{
  public SimHashes ElementID => SimHashes.Algae;

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID, new List<Tag>()
    {
      GameTags.Life
    });
    DissolvingAlgae dissolvingAlgae = solidOreEntity.AddOrGet<DissolvingAlgae>();
    dissolvingAlgae.emitRange = (byte) 1;
    dissolvingAlgae.emitCount = 1000;
    return solidOreEntity;
  }
}
