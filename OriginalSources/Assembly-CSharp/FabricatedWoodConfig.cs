// Decompiled with JetBrains decompiler
// Type: FabricatedWoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FabricatedWoodConfig : IOreConfig
{
  public const string ID = "FabricatedWood";
  public static readonly Tag TAG = TagManager.Create("FabricatedWood");

  public SimHashes ElementID => SimHashes.FabricatedWood;

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID);
    solidOreEntity.GetComponent<KPrefabID>().RemoveTag(GameTags.HideFromSpawnTool);
    return solidOreEntity;
  }
}
