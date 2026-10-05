// Decompiled with JetBrains decompiler
// Type: PlantFiberProducer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PlantFiberProducer : KMonoBehaviour
{
  public float amount;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe(1272413801, new Action<object>(this.OnHarvest));
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe(1272413801, new Action<object>(this.OnHarvest));
  }

  private void OnHarvest(object obj)
  {
    Harvestable harvestable = (Harvestable) obj;
    if (!((UnityEngine.Object) harvestable != (UnityEngine.Object) null) || !((UnityEngine.Object) harvestable.completed_by != (UnityEngine.Object) null) || !harvestable.completed_by.GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanSalvagePlantFiber))
      return;
    this.SpawnPlantFiber();
  }

  private GameObject SpawnPlantFiber()
  {
    Vector3 position = this.gameObject.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f);
    GameObject prefab = Assets.GetPrefab(new Tag("PlantFiber"));
    GameObject go = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.Ore);
    PrimaryElement component1 = this.gameObject.GetComponent<PrimaryElement>();
    PrimaryElement component2 = go.GetComponent<PrimaryElement>();
    component2.Temperature = component1.Temperature;
    component2.Mass = this.amount;
    go.SetActive(true);
    string properName = go.GetProperName();
    PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) prefab).first, PopFXManager.Instance.sprite_Plus, properName, go.transform, Vector3.zero);
    return go;
  }
}
