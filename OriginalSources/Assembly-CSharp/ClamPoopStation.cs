// Decompiled with JetBrains decompiler
// Type: ClamPoopStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class ClamPoopStation : KMonoBehaviour, IPoopStation
{
  private static Tag[] ALLOWED_USERS_IDS = new Tag[3]
  {
    (Tag) "CrabFreshWater",
    (Tag) "Crab",
    (Tag) "CrabWood"
  };
  private ReceptacleMonitor receptacleMonitor;
  private Harvestable harvestable;
  private GameObject poopUser;

  public bool IsWild => !this.receptacleMonitor.Replanted;

  public bool IsOnPlanterBox
  {
    get
    {
      return !this.IsWild && (Object) this.receptacleMonitor.smi.ReceptacleObject != (Object) null && this.receptacleMonitor.smi.ReceptacleObject is PlantablePlot && (this.receptacleMonitor.smi.ReceptacleObject as PlantablePlot).IsOffGround;
    }
  }

  protected override void OnPrefabInit()
  {
    this.receptacleMonitor = this.GetComponent<ReceptacleMonitor>();
    this.harvestable = this.GetComponent<Harvestable>();
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    this.RegisterPoopStation();
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    this.UnregisterPoopStation();
    base.OnCleanUp();
  }

  public bool IsUserCompatibleWithPoopStation(KPrefabID userPrefabID)
  {
    return userPrefabID.HasAnyTags(ClamPoopStation.ALLOWED_USERS_IDS);
  }

  public GameObject GetPoopStationObject() => this.gameObject;

  public GameObject GetCurrentPoopStationUser() => this.poopUser;

  public bool IsPoopStationOperational()
  {
    if ((Object) this.harvestable == (Object) null)
      return true;
    if (this.harvestable.CanBeHarvested)
      return false;
    return this.IsWild || this.CanAcceptMorePoop();
  }

  public string[] GetPoopingAnimNames() => (string[]) null;

  public void RegisterPoopStation()
  {
    Components.PoopStations.Add(this.gameObject.GetMyWorldId(), (IPoopStation) this);
  }

  public void UnregisterPoopStation()
  {
    Components.PoopStations.Remove(this.gameObject.GetMyWorldId(), (IPoopStation) this);
  }

  public PoopData GetPoopData()
  {
    return !this.IsWild ? new PoopData(false, this.receptacleMonitor.smi.ReceptacleObject.GetComponent<Storage>(), (string) CREATURES.POOP.PLANT_POOP_STATION_WILD, Def.GetUISprite((object) this.gameObject).first) : new PoopData(true, (Storage) null, (string) CREATURES.POOP.PLANT_POOP_STATION_WILD, Def.GetUISprite((object) this.gameObject).first);
  }

  public float GetPoopCapacity() => 70f;

  public float GetAvailablePoopCapacityPercentage()
  {
    return this.GetAvailablePoopCapacity() / this.GetPoopCapacity();
  }

  public float GetAvailablePoopCapacity()
  {
    if (this.IsWild)
      return 0.0f;
    Storage component = this.receptacleMonitor.smi.ReceptacleObject.GetComponent<Storage>();
    float poopCapacity = this.GetPoopCapacity();
    float b = poopCapacity - component.GetMassAvailable(SimHashes.Sand);
    return Mathf.Clamp(Mathf.Min(component.capacityKg, b), 0.0f, poopCapacity);
  }

  private bool CanAcceptMorePoop() => (double) this.GetAvailablePoopCapacity() > 0.0;

  public void PlayPoopStationAnim(string animName, KAnim.PlayMode playMode)
  {
  }

  public void ClearPoopStationUser(GameObject userRequestingClearing)
  {
    if (!((Object) this.poopUser == (Object) userRequestingClearing))
      return;
    this.poopUser = (GameObject) null;
    this.Trigger(-984476291, (object) null);
  }

  public bool AttemptToReservePoopStation(GameObject userRequestingReserve)
  {
    if ((Object) this.poopUser != (Object) null && (Object) this.poopUser != (Object) userRequestingReserve)
      return false;
    this.poopUser = userRequestingReserve;
    return true;
  }
}
