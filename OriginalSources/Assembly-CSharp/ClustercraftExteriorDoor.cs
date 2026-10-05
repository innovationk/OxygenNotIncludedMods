// Decompiled with JetBrains decompiler
// Type: ClustercraftExteriorDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
public class ClustercraftExteriorDoor : KMonoBehaviour
{
  public string interiorTemplateName;
  private ClustercraftInteriorDoor targetDoor;
  [Serialize]
  private int targetWorldId = -1;
  private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>((Action<ClustercraftExteriorDoor, object>) ((component, data) => component.OnLaunch(data)));
  private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLandDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>((Action<ClustercraftExteriorDoor, object>) ((component, data) => component.OnLand(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.targetWorldId < 0)
    {
      GameObject gameObject = this.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
      WorldContainer rocketInteriorWorld = ClusterManager.Instance.CreateRocketInteriorWorld(gameObject, this.interiorTemplateName, (System.Action) (() => this.PairWithInteriorDoor()));
      if ((UnityEngine.Object) rocketInteriorWorld != (UnityEngine.Object) null)
        this.targetWorldId = rocketInteriorWorld.id;
    }
    else
      this.PairWithInteriorDoor();
    this.Subscribe<ClustercraftExteriorDoor>(-1277991738, ClustercraftExteriorDoor.OnLaunchDelegate);
    this.Subscribe<ClustercraftExteriorDoor>(-887025858, ClustercraftExteriorDoor.OnLandDelegate);
  }

  protected override void OnCleanUp()
  {
    ClusterManager.Instance.DestoryRocketInteriorWorld(this.targetWorldId, this);
    base.OnCleanUp();
  }

  private void PairWithInteriorDoor()
  {
    foreach (ClustercraftInteriorDoor craftInteriorDoor in Components.ClusterCraftInteriorDoors)
    {
      if (craftInteriorDoor.GetMyWorldId() == this.targetWorldId)
      {
        this.SetTarget(craftInteriorDoor);
        break;
      }
    }
    if ((UnityEngine.Object) this.targetDoor == (UnityEngine.Object) null)
      Debug.LogWarning((object) "No ClusterCraftInteriorDoor found on world");
    WorldContainer targetWorld = this.GetTargetWorld();
    int myWorldId = this.GetMyWorldId();
    if ((UnityEngine.Object) targetWorld != (UnityEngine.Object) null && myWorldId != -1)
      targetWorld.SetParentIdx(myWorldId);
    if (this.gameObject.GetComponent<KSelectable>().IsSelected)
      RocketModuleSideScreen.instance.UpdateButtonStates();
    this.Trigger(-1118736034, (object) null);
    targetWorld.gameObject.Trigger(-1118736034);
  }

  public void SetTarget(ClustercraftInteriorDoor target)
  {
    this.targetDoor = target;
    target.GetComponent<AssignmentGroupController>().SetGroupID(this.GetComponent<AssignmentGroupController>().AssignmentGroupID);
    this.GetComponent<NavTeleporter>().TwoWayTarget(target.GetComponent<NavTeleporter>());
  }

  public bool HasTargetWorld() => (UnityEngine.Object) this.targetDoor != (UnityEngine.Object) null;

  public WorldContainer GetTargetWorld()
  {
    Debug.Assert((UnityEngine.Object) this.targetDoor != (UnityEngine.Object) null, (object) "Clustercraft Exterior Door has no targetDoor");
    return this.targetDoor.GetMyWorld();
  }

  public void FerryMinion(GameObject minion)
  {
    Vector3 vector3 = Vector3.left * 3f;
    minion.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this.targetDoor.transform.position + vector3), CellAlignment.Bottom, Grid.SceneLayer.Move));
    ClusterManager.Instance.MigrateMinion(minion.GetComponent<MinionIdentity>(), this.targetDoor.GetMyWorldId());
  }

  private void OnLaunch(object data)
  {
    NavTeleporter component = this.GetComponent<NavTeleporter>();
    component.EnableTwoWayTarget(false);
    component.Deregister();
    WorldContainer targetWorld = this.GetTargetWorld();
    if (!((UnityEngine.Object) targetWorld != (UnityEngine.Object) null))
      return;
    targetWorld.SetParentIdx(targetWorld.id);
  }

  private void OnLand(object data)
  {
    this.GetComponent<NavTeleporter>().EnableTwoWayTarget(true);
    WorldContainer targetWorld = this.GetTargetWorld();
    if (!((UnityEngine.Object) targetWorld != (UnityEngine.Object) null))
      return;
    int myWorldId = this.GetMyWorldId();
    targetWorld.SetParentIdx(myWorldId);
  }

  public int TargetCell() => this.targetDoor.GetComponent<NavTeleporter>().GetCell();

  public ClustercraftInteriorDoor GetInteriorDoor() => this.targetDoor;
}
