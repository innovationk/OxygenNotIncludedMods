// Decompiled with JetBrains decompiler
// Type: ApproachableLocator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ApproachableLocator : IEntityConfig
{
  public static readonly string ID = nameof (ApproachableLocator);

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(ApproachableLocator.ID, ApproachableLocator.ID, false);
    entity.AddTag(GameTags.NotConversationTopic);
    entity.AddOrGet<Approachable>();
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
