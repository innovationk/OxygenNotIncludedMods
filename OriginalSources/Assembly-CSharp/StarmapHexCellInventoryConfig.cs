// Decompiled with JetBrains decompiler
// Type: StarmapHexCellInventoryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class StarmapHexCellInventoryConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "StarmapHexCellInventory";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity("StarmapHexCellInventory", (string) UI.CLUSTERMAP.HEXCELL_INVENTORY.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<StarmapHexCellInventory>();
    entity.AddOrGet<StarmapHexCellInventoryVisuals>();
    entity.AddOrGet<InfoDescription>().description = (string) UI.CLUSTERMAP.HEXCELL_INVENTORY.DESC;
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
