// Decompiled with JetBrains decompiler
// Type: CO2Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CO2Manager")]
public class CO2Manager : KMonoBehaviour, ISim33ms
{
  private const float CO2Lifetime = 3f;
  [SerializeField]
  private Vector3 acceleration;
  [SerializeField]
  private CO2 prefab;
  [SerializeField]
  private GameObject breathPrefab;
  [SerializeField]
  private GameObject exhaustPrefab;
  [SerializeField]
  private Color tintColour;
  private List<CO2> co2Items = new List<CO2>();
  private GameObjectPool breathPool;
  private GameObjectPool exhaustPool;
  private GameObjectPool co2Pool;
  public static CO2Manager instance;

  public static void DestroyInstance() => CO2Manager.instance = (CO2Manager) null;

  protected override void OnPrefabInit()
  {
    CO2Manager.instance = this;
    this.prefab.gameObject.SetActive(false);
    this.breathPrefab.SetActive(false);
    this.exhaustPrefab.SetActive(false);
    this.co2Pool = new GameObjectPool(new Func<GameObject>(this.InstantiateCO2), new Action<GameObject>(CO2Manager.Deactivate), 16 /*0x10*/);
    this.breathPool = new GameObjectPool(new Func<GameObject>(this.InstantiateBreath), new Action<GameObject>(CO2Manager.Deactivate), 16 /*0x10*/);
    this.exhaustPool = new GameObjectPool(new Func<GameObject>(this.InstantiateExhaust), new Action<GameObject>(CO2Manager.Deactivate), 16 /*0x10*/);
  }

  private GameObject InstantiateCO2()
  {
    GameObject gameObject = GameUtil.KInstantiate((Component) this.prefab, Grid.SceneLayer.Front);
    gameObject.SetActive(false);
    return gameObject;
  }

  private static void Deactivate(GameObject _)
  {
  }

  private GameObject InstantiateBreath()
  {
    GameObject gameObject = GameUtil.KInstantiate(this.breathPrefab, Grid.SceneLayer.Front);
    gameObject.SetActive(false);
    return gameObject;
  }

  private GameObject InstantiateExhaust()
  {
    GameObject gameObject = GameUtil.KInstantiate(this.exhaustPrefab, Grid.SceneLayer.Front);
    gameObject.SetActive(false);
    return gameObject;
  }

  public void Sim33ms(float dt)
  {
    Vector2I xy1 = new Vector2I();
    Vector2I xy2 = new Vector2I();
    Vector3 vector3 = this.acceleration * dt;
    int count = this.co2Items.Count;
    for (int index = 0; index < count; ++index)
    {
      CO2 co2Item = this.co2Items[index];
      co2Item.velocity += vector3;
      co2Item.lifetimeRemaining -= dt;
      Grid.PosToXY(co2Item.transform.GetPosition(), out xy1);
      co2Item.transform.SetPosition(co2Item.transform.GetPosition() + co2Item.velocity * dt);
      Grid.PosToXY(co2Item.transform.GetPosition(), out xy2);
      int num = Grid.XYToCell(xy1.x, xy1.y);
      for (int y = xy1.y; y >= xy2.y; --y)
      {
        int cell = Grid.XYToCell(xy1.x, y);
        bool flag1 = !Grid.IsValidCell(cell) || (double) co2Item.lifetimeRemaining <= 0.0;
        if (!flag1)
        {
          Element element = Grid.Element[cell];
          flag1 = element.IsLiquid || element.IsSolid || ((uint) Grid.Properties[cell] & 1U) > 0U;
        }
        if (flag1)
        {
          int gameCell = cell;
          bool flag2 = false;
          if (num != cell)
          {
            gameCell = num;
            flag2 = true;
          }
          else
          {
            int spawnCell;
            if (CO2Manager.TryFindBreathableSpawnCell(cell, out spawnCell))
            {
              flag2 = true;
              gameCell = spawnCell;
            }
          }
          if (flag2)
          {
            co2Item.TriggerDestroy();
            SimMessages.ModifyMass(gameCell, co2Item.mass, byte.MaxValue, 0, CellEventLogger.Instance.CO2ManagerFixedUpdate, co2Item.temperature, SimHashes.CarbonDioxide);
            --count;
            this.co2Items[index] = this.co2Items[count];
            this.co2Items.RemoveAt(count);
            break;
          }
        }
        num = cell;
      }
    }
  }

  private static bool TryFindBreathableSpawnCell(int cell, out int spawnCell)
  {
    bool flag = false;
    int num1 = -1;
    int num2 = -1;
    bool breathableSpawnCell = false;
    foreach (CellOffset offset in GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS)
    {
      int cell1 = Grid.OffsetCell(cell, offset);
      if (Grid.IsValidCell(cell1))
      {
        Element element = Grid.Element[cell1];
        if (element.id == SimHashes.CarbonDioxide || element.HasTag(GameTags.Breathable))
        {
          num1 = cell1;
          flag = true;
          breathableSpawnCell = true;
          break;
        }
        if (element.IsGas)
        {
          num2 = cell1;
          breathableSpawnCell = true;
        }
      }
    }
    spawnCell = flag ? num1 : num2;
    return breathableSpawnCell;
  }

  public CO2 SpawnCO2(Vector3 position, float mass, float temperature, bool flip)
  {
    return this.SpawnCO2(position, mass, temperature, flip, 0.0f);
  }

  public CO2 SpawnCO2(Vector3 position, float mass, float temperature, bool flip, float rotation)
  {
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    GameObject instance = this.co2Pool.GetInstance();
    instance.transform.SetPosition(position);
    instance.SetActive(true);
    CO2 component1 = instance.GetComponent<CO2>();
    component1.mass = mass;
    component1.temperature = temperature;
    component1.velocity = Vector3.zero;
    component1.lifetimeRemaining = 3f;
    KBatchedAnimController component2 = component1.GetComponent<KBatchedAnimController>();
    component2.TintColour = (Color32) this.tintColour;
    component2.onDestroySelf = new Action<GameObject>(this.OnDestroyCO2);
    component2.FlipX = flip;
    component1.StartLoop();
    this.co2Items.Add(component1);
    return component1;
  }

  public void SpawnBreath(Vector3 position, float mass, float temperature, bool flip)
  {
    if (Grid.IsVisiblyInLiquid((Vector2) position) && !CO2Manager.TryFindBreathableSpawnCell(Grid.PosToCell(position), out int _))
    {
      BubbleManager.instance.SpawnBubble(SimHashes.CarbonDioxide, (Vector2) position, mass, temperature, BubbleManager.Disease.None);
    }
    else
    {
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
      this.SpawnCO2(position, mass, temperature, flip);
      GameObject instance = this.breathPool.GetInstance();
      instance.transform.SetPosition(position);
      instance.SetActive(true);
      KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
      component.TintColour = (Color32) this.tintColour;
      component.onDestroySelf = new Action<GameObject>(this.OnDestroyBreath);
      component.FlipX = flip;
      component.Play((HashedString) "breath");
    }
  }

  public void SpawnExhaust(
    Vector3 position,
    Vector3 velocity,
    int co2Cell,
    float mass,
    float temperature)
  {
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    float num = Mathf.Repeat(Vector3.Angle(Vector3.down, velocity) * Mathf.Sign(velocity.x), 360f);
    SimMessages.ModifyMass(co2Cell, mass, byte.MaxValue, 0, CellEventLogger.Instance.CO2ManagerFixedUpdate, temperature, SimHashes.CarbonDioxide);
    GameObject instance = this.exhaustPool.GetInstance();
    instance.transform.SetPosition(position);
    instance.SetActive(true);
    CO2 co2 = instance.AddOrGet<CO2>();
    co2.mass = mass;
    co2.temperature = temperature;
    co2.lifetimeRemaining = 3f;
    co2.affectedByGravity = false;
    KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
    component.onDestroySelf = new Action<GameObject>(this.OnDestroyExhaust);
    component.Rotation = num;
    component.Play((HashedString) "smoke_particle");
  }

  private void OnDestroyCO2(GameObject co2_go)
  {
    co2_go.SetActive(false);
    this.co2Pool.ReleaseInstance(co2_go);
  }

  private void OnDestroyBreath(GameObject breath_go)
  {
    breath_go.SetActive(false);
    this.breathPool.ReleaseInstance(breath_go);
  }

  private void OnDestroyExhaust(GameObject go)
  {
    go.SetActive(false);
    this.exhaustPool.ReleaseInstance(go);
  }
}
