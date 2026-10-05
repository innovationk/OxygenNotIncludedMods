// Decompiled with JetBrains decompiler
// Type: BleachStoneCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class BleachStoneCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "BleachStoneComet";
  private const int ADDED_CELLS = 1;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
    GameObject prefab = BaseCometConfig.BaseComet(BleachStoneCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.BLEACHSTONECOMET.NAME, "meteor_bleachstone_kanim", SimHashes.BleachStone, new Vector2((float) ((double) mass * 0.800000011920929 * 1.0), (float) ((double) mass * 1.2000000476837158 * 1.0)), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", exhaustElement: SimHashes.ChlorineGas, explosionEffect: SpawnFXHashes.MeteorImpactIce, size: 0.6f);
    Comet component = prefab.GetComponent<Comet>();
    component.explosionOreCount = new Vector2I(2, 4);
    component.explosionSpeedRange = new Vector2(4f, 7f);
    component.entityDamage = 0;
    component.totalTileDamage = 0.0f;
    component.addTiles = 1;
    component.addTilesMinHeight = 1;
    component.addTilesMaxHeight = 1;
    return prefab;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
