// Decompiled with JetBrains decompiler
// Type: SnowballCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class SnowballCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "SnowballComet";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject prefab = BaseCometConfig.BaseComet(SnowballCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.SNOWBALLCOMET.NAME, "meteor_snow_kanim", SimHashes.Snow, new Vector2(3f, 20f), new Vector2(253.15f, 263.15f), "Meteor_snowball_Impact", 5, SimHashes.Void, size: 0.3f);
    Comet component = prefab.GetComponent<Comet>();
    component.entityDamage = 0;
    component.totalTileDamage = 0.0f;
    component.splashRadius = 1;
    component.addTiles = 3;
    component.addTilesMinHeight = 1;
    component.addTilesMaxHeight = 2;
    return prefab;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
