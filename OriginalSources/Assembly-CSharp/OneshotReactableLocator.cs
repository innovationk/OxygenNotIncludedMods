// Decompiled with JetBrains decompiler
// Type: OneshotReactableLocator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OneshotReactableLocator : IEntityConfig
{
  public static readonly string ID = nameof (OneshotReactableLocator);

  public static EmoteReactable CreateOneshotReactable(
    GameObject source,
    float lifetime,
    string id,
    ChoreType chore_type,
    int range_width = 15,
    int range_height = 15,
    float min_reactor_time = 20f)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) OneshotReactableLocator.ID), source.transform.GetPosition());
    EmoteReactable oneshotReactable = new EmoteReactable(gameObject, (HashedString) id, chore_type, range_width, range_height, 100000f, min_reactor_time);
    oneshotReactable.AddPrecondition(OneshotReactableLocator.ReactorIsNotSource(source));
    OneshotReactableHost component = gameObject.GetComponent<OneshotReactableHost>();
    component.lifetime = lifetime;
    component.SetReactable((Reactable) oneshotReactable);
    gameObject.SetActive(true);
    return oneshotReactable;
  }

  private static Reactable.ReactablePrecondition ReactorIsNotSource(GameObject source)
  {
    return (Reactable.ReactablePrecondition) ((reactor, transition) => (Object) reactor != (Object) source);
  }

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(OneshotReactableLocator.ID, OneshotReactableLocator.ID, false);
    entity.AddTag(GameTags.NotConversationTopic);
    entity.AddOrGet<OneshotReactableHost>();
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
