// Decompiled with JetBrains decompiler
// Type: AlgaeCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class AlgaeCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "AlgaeComet";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject prefab = BaseCometConfig.BaseComet(AlgaeCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.ALGAECOMET.NAME, "meteor_algae_kanim", SimHashes.Algae, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_algae_Impact", 7, SimHashes.Void, SpawnFXHashes.MeteorImpactAlgae, 0.3f);
    Comet component = prefab.GetComponent<Comet>();
    component.explosionOreCount = new Vector2I(2, 4);
    component.explosionSpeedRange = new Vector2(4f, 7f);
    component.entityDamage = 0;
    component.totalTileDamage = 0.0f;
    return prefab;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
