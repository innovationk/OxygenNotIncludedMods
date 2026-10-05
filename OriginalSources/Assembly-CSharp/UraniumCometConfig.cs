// Decompiled with JetBrains decompiler
// Type: UraniumCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class UraniumCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static readonly string ID = "UraniumComet";
  private const SimHashes element = SimHashes.UraniumOre;
  private const int ADDED_CELLS = 6;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    float mass = ElementLoader.FindElementByHash(SimHashes.UraniumOre).defaultValues.mass;
    GameObject prefab = BaseCometConfig.BaseComet(UraniumCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.URANIUMORECOMET.NAME, "meteor_uranium_kanim", SimHashes.UraniumOre, new Vector2((float) ((double) mass * 0.800000011920929 * 6.0), (float) ((double) mass * 1.2000000476837158 * 6.0)), new Vector2(323.15f, 403.15f), "Meteor_Nuclear_Impact", 3, explosionEffect: SpawnFXHashes.MeteorImpactUranium, size: 0.6f);
    Comet component = prefab.GetComponent<Comet>();
    component.explosionOreCount = new Vector2I(1, 2);
    component.entityDamage = 15;
    component.totalTileDamage = 0.0f;
    component.addTiles = 6;
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
