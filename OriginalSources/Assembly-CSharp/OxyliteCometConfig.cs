// Decompiled with JetBrains decompiler
// Type: OxyliteCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class OxyliteCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "OxyliteComet";
  private const int ADDED_CELLS = 6;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
    GameObject prefab = BaseCometConfig.BaseComet(OxyliteCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.OXYLITECOMET.NAME, "meteor_oxylite_kanim", SimHashes.OxyRock, new Vector2((float) ((double) mass * 0.800000011920929 * 6.0), (float) ((double) mass * 1.2000000476837158 * 6.0)), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Oxygen, SpawnFXHashes.MeteorImpactIce, 0.6f);
    Comet component = prefab.GetComponent<Comet>();
    component.entityDamage = 0;
    component.totalTileDamage = 0.0f;
    component.addTiles = 6;
    component.addTilesMinHeight = 2;
    component.addTilesMaxHeight = 8;
    return prefab;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
