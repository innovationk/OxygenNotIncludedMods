// Decompiled with JetBrains decompiler
// Type: PopFXManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopFXManager : KScreen
{
  public static PopFXManager Instance;
  private GameObject Prefab_PopFxGroup;
  public GameObject Prefab_PopFX;
  public List<PopFX> Pool = new List<PopFX>();
  public List<PopFxGroup> GroupPool = new List<PopFxGroup>();
  public Dictionary<int, PopFxGroup> AliveGroups = new Dictionary<int, PopFxGroup>();
  public Sprite sprite_Plus;
  public Sprite sprite_Negative;
  public Sprite sprite_Resource;
  public Sprite sprite_Building;
  public Sprite sprite_Research;
  private bool ready;

  public static void DestroyInstance() => PopFXManager.Instance = (PopFXManager) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    PopFXManager.Instance = this;
    this.Prefab_PopFxGroup = new GameObject("Prefab_PopFxGroup");
    this.Prefab_PopFxGroup.AddComponent<PopFxGroup>();
    this.Prefab_PopFxGroup.transform.SetParent(this.Prefab_PopFX.transform.parent);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ready = true;
    if (GenericGameSettings.instance.disablePopFx)
      return;
    for (int index = 0; index < 20; ++index)
    {
      PopFxGroup popFxGroup = this.CreatePopFxGroup();
      this.Pool.Add(this.CreatePopFX());
      this.GroupPool.Add(popFxGroup);
    }
  }

  public bool Ready() => this.ready;

  public PopFX SpawnFX(
    Sprite mainIcon,
    string text,
    Transform target_transform,
    float lifetime = 1.5f,
    bool track_target = false)
  {
    return this.SpawnFX(mainIcon, text, target_transform, Vector3.zero, lifetime, track_target);
  }

  public PopFX SpawnFX(
    Sprite mainIcon,
    string text,
    Transform target_transform,
    Vector3 offset,
    float lifetime = 1.5f,
    bool track_target = false,
    bool force_spawn = false)
  {
    return this.SpawnFX(mainIcon, (Sprite) null, text, target_transform, offset, lifetime, track_target: track_target, force_spawn: force_spawn);
  }

  public PopFX SpawnFX(
    Sprite mainIcon,
    Sprite secondaryIcon,
    string text,
    Transform target_transform,
    Vector3 offset,
    float lifetime = 1.5f,
    bool selfAdjustPositionIfInGroup = true,
    bool track_target = false,
    bool force_spawn = false)
  {
    if (GenericGameSettings.instance.disablePopFx)
      return (PopFX) null;
    if (Game.IsQuitting())
      return (PopFX) null;
    Vector3 pos = offset;
    if ((Object) target_transform != (Object) null)
      pos += target_transform.GetPosition();
    int cell = Grid.PosToCell(pos);
    if (!force_spawn && (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell) || (Object) CameraController.Instance != (Object) null && !CameraController.Instance.IsVisiblePosExtended(pos)))
      return (PopFX) null;
    PopFX popFx = this.GetOrCreatePopFX(mainIcon, secondaryIcon, text, target_transform, offset, selfAdjustPositionIfInGroup, lifetime, track_target);
    PopFxGroup popFxGroup;
    if (!this.AliveGroups.TryGetValue(cell, out popFxGroup) || (Object) popFxGroup == (Object) null)
    {
      if (this.GroupPool.Count > 0)
      {
        popFxGroup = this.GroupPool[0];
        this.GroupPool[0].gameObject.SetActive(true);
        this.GroupPool.RemoveAt(0);
      }
      else
      {
        popFxGroup = this.CreatePopFxGroup();
        popFxGroup.gameObject.SetActive(true);
      }
      this.AliveGroups.Add(cell, popFxGroup);
    }
    popFxGroup.Enqueue(popFx);
    popFxGroup.WakeUp(cell);
    return popFx;
  }

  private PopFX GetOrCreatePopFX(
    Sprite mainIcon,
    Sprite secondaryIcon,
    string text,
    Transform target_transform,
    Vector3 offset,
    bool selfAdjustPositionIfInGroup = true,
    float lifetime = 1.5f,
    bool track_target = false)
  {
    PopFX popFx;
    if (this.Pool.Count > 0)
    {
      popFx = this.Pool[0];
      this.Pool[0].Setup(mainIcon, secondaryIcon, text, target_transform, offset, selfAdjustPositionIfInGroup, lifetime, track_target);
      this.Pool.RemoveAt(0);
    }
    else
    {
      popFx = this.CreatePopFX();
      popFx.Setup(mainIcon, secondaryIcon, text, target_transform, offset, selfAdjustPositionIfInGroup, lifetime, track_target);
    }
    return popFx;
  }

  private PopFX CreatePopFX()
  {
    int num = this.Prefab_PopFX.gameObject.activeInHierarchy ? 1 : 0;
    GameObject gameObject = Util.KInstantiate(this.Prefab_PopFX, this.gameObject, "Pooled_PopFX");
    gameObject.transform.localScale = Vector3.one;
    return gameObject.GetComponent<PopFX>();
  }

  private PopFxGroup CreatePopFxGroup()
  {
    GameObject gameObject = Util.KInstantiate(this.Prefab_PopFxGroup, this.gameObject, "Pooled_PopFxGroup");
    gameObject.transform.localScale = Vector3.one;
    return gameObject.GetComponent<PopFxGroup>();
  }

  public void RecycleFX(PopFX fx) => this.Pool.Add(fx);

  public void RecycleFxGroup(int key, PopFxGroup fx)
  {
    this.AliveGroups.Remove(key);
    this.GroupPool.Add(fx);
  }
}
