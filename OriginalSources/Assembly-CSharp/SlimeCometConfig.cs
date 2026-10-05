// Decompiled with JetBrains decompiler
// Type: SlimeCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class SlimeCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "SlimeComet";
  public const int ADDED_CELLS = 2;
  private const SimHashes element = SimHashes.SlimeMold;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    float mass = ElementLoader.FindElementByHash(SimHashes.SlimeMold).defaultValues.mass;
    GameObject prefab = BaseCometConfig.BaseComet(SlimeCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.SLIMECOMET.NAME, "meteor_slime_kanim", SimHashes.SlimeMold, new Vector2((float) ((double) mass * 0.800000011920929 * 2.0), (float) ((double) mass * 1.2000000476837158 * 2.0)), new Vector2(310.15f, 323.15f), "Meteor_slimeball_Impact", 7, SimHashes.ContaminatedOxygen, SpawnFXHashes.MeteorImpactSlime, 0.6f);
    Comet component = prefab.GetComponent<Comet>();
    component.entityDamage = 0;
    component.totalTileDamage = 0.0f;
    component.explosionOreCount = new Vector2I(1, 2);
    component.explosionSpeedRange = new Vector2(4f, 7f);
    component.addTiles = 2;
    component.addTilesMinHeight = 1;
    component.addTilesMaxHeight = 2;
    component.diseaseIdx = Db.Get().Diseases.GetIndex((HashedString) "SlimeLung");
    component.addDiseaseCount = (int) ((double) component.EXHAUST_RATE * 100000.0);
    return prefab;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
    go.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex((HashedString) "SlimeLung"), (int) ((double) Random.Range(0.9f, 1.2f) * 50.0 * 100000.0), "Meteor");
  }
}
