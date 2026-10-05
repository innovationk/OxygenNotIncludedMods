// Decompiled with JetBrains decompiler
// Type: PinkRockCarvedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PinkRockCarvedConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "PinkRockCarved";

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("PinkRockCarved", (string) STRINGS.CREATURES.SPECIES.PINKROCKCARVED.NAME, (string) STRINGS.CREATURES.SPECIES.PINKROCKCARVED.DESC, 1f, true, Assets.GetAnim((HashedString) "pinkrock_decor_kanim"), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.CIRCLE, 0.5f, 0.5f, true, additionalTags: new List<Tag>()
    {
      GameTags.RareMaterials,
      GameTags.MiscPickupable,
      GameTags.PedestalDisplayable,
      GameTags.Experimental,
      GameTags.Ornament
    });
    looseEntity.AddOrGet<OccupyArea>();
    DecorProvider decorProvider = looseEntity.AddOrGet<DecorProvider>();
    decorProvider.SetValues(TUNING.BUILDINGS.DECOR.BONUS.TIER1);
    decorProvider.overrideName = looseEntity.GetProperName();
    Light2D light2D = looseEntity.AddOrGet<Light2D>();
    light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
    light2D.Color = LIGHT2D.PINKROCK_COLOR;
    light2D.Range = 3f;
    light2D.Angle = 0.0f;
    light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
    light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
    light2D.shape = LightShape.Circle;
    light2D.drawOverlay = true;
    light2D.disableOnStore = true;
    looseEntity.GetComponent<KCircleCollider2D>().offset = new Vector2(0.0f, 0.25f);
    looseEntity.AddOrGet<CodexEntryRedirector>().CodexID = "REQUIREMENTCLASSORNAMENT";
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
