// Decompiled with JetBrains decompiler
// Type: FXHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class FXHelpers
{
  public static KBatchedAnimController CreateEffect(
    string anim_file_name,
    Vector3 position,
    Transform parent = null,
    bool update_looping_sounds_position = false,
    Grid.SceneLayer layer = Grid.SceneLayer.Front,
    bool set_inactive = false)
  {
    KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.EffectTemplateId), position, layer).GetComponent<KBatchedAnimController>();
    component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_name);
    component.name = anim_file_name;
    if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
      component.transform.SetParent(parent, false);
    component.transform.SetPosition(position);
    if (update_looping_sounds_position)
      component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
    KAnimFile anim = Assets.GetAnim((HashedString) anim_file_name);
    if ((UnityEngine.Object) anim == (UnityEngine.Object) null)
      Debug.LogWarning((object) ("Missing effect anim: " + anim_file_name));
    else
      component.AnimFiles = new KAnimFile[1]{ anim };
    if (!set_inactive)
      component.gameObject.SetActive(true);
    return component;
  }

  public static KBatchedAnimController CreateEffect(
    string[] anim_file_names,
    Vector3 position,
    Transform parent = null,
    bool update_looping_sounds_position = false,
    Grid.SceneLayer layer = Grid.SceneLayer.Front,
    bool set_inactive = false)
  {
    KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.EffectTemplateId), position, layer).GetComponent<KBatchedAnimController>();
    component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
    component.name = anim_file_names[0];
    if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
      component.transform.SetParent(parent, false);
    component.transform.SetPosition(position);
    if (update_looping_sounds_position)
      component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
    component.AnimFiles = ((IEnumerable<string>) anim_file_names).Select<string, (string, KAnimFile)>((Func<string, (string, KAnimFile)>) (name => (name, Assets.GetAnim((HashedString) name)))).Where<(string, KAnimFile)>((Func<(string, KAnimFile), bool>) (e =>
    {
      if (!((UnityEngine.Object) e.anim == (UnityEngine.Object) null))
        return true;
      Debug.LogWarning((object) ("Missing effect anim: " + e.name));
      return false;
    })).Select<(string, KAnimFile), KAnimFile>((Func<(string, KAnimFile), KAnimFile>) (e => e.anim)).ToArray<KAnimFile>();
    if (!set_inactive)
      component.gameObject.SetActive(true);
    return component;
  }

  public static KBatchedAnimController CreateEffectOverride(
    string[] anim_file_names,
    Vector3 position,
    Transform parent = null,
    bool update_looping_sounds_position = false,
    Grid.SceneLayer layer = Grid.SceneLayer.Front,
    bool set_inactive = false)
  {
    KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.EffectTemplateOverrideId), position, layer).GetComponent<KBatchedAnimController>();
    component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
    component.name = anim_file_names[0];
    if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
      component.transform.SetParent(parent, false);
    component.transform.SetPosition(position);
    if (update_looping_sounds_position)
      component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
    component.AnimFiles = ((IEnumerable<string>) anim_file_names).Select<string, (string, KAnimFile)>((Func<string, (string, KAnimFile)>) (name => (name, Assets.GetAnim((HashedString) name)))).Where<(string, KAnimFile)>((Func<(string, KAnimFile), bool>) (e =>
    {
      if (!((UnityEngine.Object) e.anim == (UnityEngine.Object) null))
        return true;
      Debug.LogWarning((object) ("Missing effect anim: " + e.name));
      return false;
    })).Select<(string, KAnimFile), KAnimFile>((Func<(string, KAnimFile), KAnimFile>) (e => e.anim)).ToArray<KAnimFile>();
    if (!set_inactive)
      component.gameObject.SetActive(true);
    return component;
  }
}
