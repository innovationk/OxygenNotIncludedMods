// Decompiled with JetBrains decompiler
// Type: PlantableSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/PlantableSeed")]
public class PlantableSeed : KMonoBehaviour, IReceptacleDirection, IGameObjectEffectDescriptor
{
  public Tag PlantID;
  public Tag PreviewID;
  [Serialize]
  public float timeUntilSelfPlant;
  public Tag replantGroundTag;
  public string domesticatedDescription;
  public SingleEntityReceptacle.ReceptacleDirection direction;
  [MyCmpGet]
  private Pickupable pickupable;

  public SingleEntityReceptacle.ReceptacleDirection Direction => this.direction;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.PlantableSeeds.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.PlantableSeeds.Remove(this);
    base.OnCleanUp();
  }

  public void TryPlant(bool allow_plant_from_storage = false)
  {
    this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
    if (!allow_plant_from_storage && this.gameObject.HasTag(GameTags.Stored))
      return;
    int cell = Grid.PosToCell(this.gameObject);
    if (!this.TestSuitableGround(cell))
      return;
    Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.PlantID), posCbc, Grid.SceneLayer.BuildingFront);
    MutantPlant component = gameObject.GetComponent<MutantPlant>();
    if ((Object) component != (Object) null)
      this.GetComponent<MutantPlant>().CopyMutationsTo(component);
    gameObject.SetActive(true);
    Pickupable unit = this.pickupable.TakeUnit(1f);
    if ((Object) unit != (Object) null)
    {
      int num = (Object) gameObject.GetComponent<Crop>() != (Object) null ? 1 : 0;
      Util.KDestroyGameObject(unit.gameObject);
    }
    else
      KCrashReporter.Assert(false, "Seed has fractional total amount < 1f");
  }

  public bool TestSuitableGround(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    int index = this.Direction != SingleEntityReceptacle.ReceptacleDirection.Bottom ? Grid.CellBelow(cell) : Grid.CellAbove(cell);
    if (!Grid.IsValidCell(index) || Grid.Foundation[index] || Grid.Element[index].hardness >= (byte) 150 || this.replantGroundTag.IsValid && !Grid.Element[index].HasTag(this.replantGroundTag))
      return false;
    GameObject prefab = Assets.GetPrefab(this.PlantID);
    EntombVulnerable component1 = prefab.GetComponent<EntombVulnerable>();
    if ((Object) component1 != (Object) null && !component1.IsCellSafe(cell))
      return false;
    DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
    if ((Object) component2 != (Object) null && !component2.IsCellSafe(cell))
      return false;
    TemperatureVulnerable component3 = prefab.GetComponent<TemperatureVulnerable>();
    if ((Object) component3 != (Object) null && !component3.IsCellSafe(cell) && Grid.Element[cell].id != SimHashes.Vacuum)
      return false;
    UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
    if ((Object) component4 != (Object) null && !component4.IsSuitableFoundation(cell))
      return false;
    OccupyArea component5 = prefab.GetComponent<OccupyArea>();
    return !((Object) component5 != (Object) null) || component5.CanOccupyArea(cell, ObjectLayer.Building);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
    {
      Descriptor descriptor = new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_CEILING, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_CEILING, Descriptor.DescriptorType.Requirement);
      descriptors.Add(descriptor);
    }
    else if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
    {
      Descriptor descriptor = new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_WALL, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_WALL, Descriptor.DescriptorType.Requirement);
      descriptors.Add(descriptor);
    }
    return descriptors;
  }
}
