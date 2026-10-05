// Decompiled with JetBrains decompiler
// Type: DreamJournalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DreamJournalConfig : IEntityConfig
{
  public static Tag ID = new Tag("DreamJournal");
  public const float MASS = 1f;
  public const int FABRICATION_TIME_SECONDS = 300;
  private const string ANIM_FILE = "dream_journal_kanim";
  private const string INITIAL_ANIM = "object";
  public const int MAX_STACK_SIZE = 25;

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public GameObject CreatePrefab()
  {
    KAnimFile anim = Assets.GetAnim((HashedString) "dream_journal_kanim");
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(DreamJournalConfig.ID.Name, (string) ITEMS.DREAMJOURNAL.NAME, (string) ITEMS.DREAMJOURNAL.DESC, 1f, true, anim, "object", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, additionalTags: new List<Tag>()
    {
      GameTags.StoryTraitResource,
      GameTags.PedestalDisplayable
    });
    looseEntity.AddOrGet<EntitySplitter>().maxStackSize = 25f;
    return looseEntity;
  }
}
