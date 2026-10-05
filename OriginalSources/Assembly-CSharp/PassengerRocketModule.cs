// Decompiled with JetBrains decompiler
// Type: PassengerRocketModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PassengerRocketModule : KMonoBehaviour
{
  public EventReference interiorReverbSnapshot;
  [Serialize]
  private PassengerRocketModule.RequestCrewState passengersRequested;
  private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<PassengerRocketModule>(GameTags.RocketOnGround, (Action<PassengerRocketModule, object>) ((component, data) => component.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release)));
  private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnClustercraftStateChanged = new EventSystem.IntraObjectHandler<PassengerRocketModule>((Action<PassengerRocketModule, object>) ((cmp, data) => cmp.RefreshClusterStateForAudio()));
  private static EventSystem.IntraObjectHandler<PassengerRocketModule> RefreshDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>((Action<PassengerRocketModule, object>) ((cmp, data) =>
  {
    cmp.RefreshOrders();
    cmp.RefreshClusterStateForAudio();
  }));
  private static EventSystem.IntraObjectHandler<PassengerRocketModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>((Action<PassengerRocketModule, object>) ((component, data) => component.ClearMinionAssignments(data)));
  private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>((Action<PassengerRocketModule, object>) ((component, data) => component.OnReachableChanged(data)));

  public PassengerRocketModule.RequestCrewState PassengersRequested => this.passengersRequested;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    GameUtil.SubscribeToTags<PassengerRocketModule>(this, PassengerRocketModule.OnRocketOnGroundTagDelegate, false);
    this.Subscribe<PassengerRocketModule>(-1547247383, PassengerRocketModule.OnClustercraftStateChanged);
    this.Subscribe<PassengerRocketModule>(1655598572, PassengerRocketModule.RefreshDelegate);
    this.Subscribe<PassengerRocketModule>(191901966, PassengerRocketModule.RefreshDelegate);
    this.Subscribe<PassengerRocketModule>(-71801987, PassengerRocketModule.RefreshDelegate);
    this.Subscribe<PassengerRocketModule>(-1277991738, PassengerRocketModule.OnLaunchDelegate);
    this.Subscribe<PassengerRocketModule>(-1432940121, PassengerRocketModule.OnReachableChangedDelegate);
    new ReachabilityMonitor.Instance(this.GetComponent<Workable>()).StartSM();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    base.OnCleanUp();
  }

  private void OnAssignmentGroupChanged(object data) => this.RefreshOrders();

  private void RefreshClusterStateForAudio()
  {
    if (!((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null))
      return;
    WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
    if (!((UnityEngine.Object) activeWorld != (UnityEngine.Object) null) || !activeWorld.IsModuleInterior || !((UnityEngine.Object) this.GetComponent<RocketModuleCluster>().CraftInterface == (UnityEngine.Object) activeWorld.GetComponent<Clustercraft>().ModuleInterface))
      return;
    ClusterManager.Instance.UpdateRocketInteriorAudio();
  }

  private void OnReachableChanged(object data)
  {
    int num = ((Boxed<bool>) data).value ? 1 : 0;
    KSelectable component = this.GetComponent<KSelectable>();
    if (num != 0)
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable);
    else
      component.AddStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, (object) this);
  }

  public void RequestCrewBoard(
    PassengerRocketModule.RequestCrewState requestBoard)
  {
    this.passengersRequested = requestBoard;
    this.RefreshOrders();
  }

  public bool ShouldCrewGetIn()
  {
    CraftModuleInterface craftInterface = this.GetComponent<RocketModuleCluster>().CraftInterface;
    if (this.passengersRequested == PassengerRocketModule.RequestCrewState.Request)
      return true;
    return craftInterface.IsLaunchRequested() && craftInterface.CheckPreppedForLaunch();
  }

  private void RefreshOrders()
  {
    if (!this.HasTag(GameTags.RocketOnGround) || !this.GetComponent<ClustercraftExteriorDoor>().HasTargetWorld())
      return;
    int cell = this.GetComponent<NavTeleporter>().GetCell();
    int index = this.GetComponent<ClustercraftExteriorDoor>().TargetCell();
    bool restrict = this.ShouldCrewGetIn();
    if (restrict)
    {
      foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      {
        bool flag1 = Game.Instance.assignmentManager.assignment_groups[this.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember((IAssignableIdentity) minionIdentity.assignableProxy.Get());
        bool flag2 = minionIdentity.GetMyWorldId() == (int) Grid.WorldIdx[index];
        RocketPassengerMonitor.Instance smi = minionIdentity.GetSMI<RocketPassengerMonitor.Instance>();
        if (smi != null)
        {
          if (!flag2 & flag1)
            smi.SetMoveTarget(index);
          else if (flag2 && !flag1)
            smi.SetMoveTarget(cell);
          else
            smi.ClearMoveTarget(index);
        }
      }
    }
    else
    {
      foreach (Component cmp in Components.LiveMinionIdentities.Items)
      {
        RocketPassengerMonitor.Instance smi = cmp.GetSMI<RocketPassengerMonitor.Instance>();
        if (smi != null)
        {
          smi.ClearMoveTarget(cell);
          smi.ClearMoveTarget(index);
        }
      }
    }
    for (int idx = 0; idx < Components.LiveMinionIdentities.Count; ++idx)
      this.RefreshAccessStatus(Components.LiveMinionIdentities[idx], restrict);
  }

  private void RefreshAccessStatus(MinionIdentity minion, bool restrict)
  {
    ClustercraftInteriorDoor interiorDoor = this.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
    AccessControl component1 = this.GetComponent<AccessControl>();
    AccessControl component2 = interiorDoor.GetComponent<AccessControl>();
    if (restrict)
    {
      if (Game.Instance.assignmentManager.assignment_groups[this.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember((IAssignableIdentity) minion.assignableProxy.Get()))
      {
        component1.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
        component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
      }
      else
      {
        component1.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
        component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
      }
    }
    else
    {
      component1.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
      component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
    }
  }

  public bool CheckPilotBoarded()
  {
    GameObject dupePilot = this.GetDupePilot();
    return (UnityEngine.Object) dupePilot != (UnityEngine.Object) null && dupePilot.GetMyWorldId() == (int) Grid.WorldIdx[this.GetComponent<ClustercraftExteriorDoor>().TargetCell()];
  }

  public GameObject GetDupePilot()
  {
    ICollection<IAssignableIdentity> members = (ICollection<IAssignableIdentity>) this.GetComponent<AssignmentGroupController>().GetMembers();
    if (members.Count == 0)
      return (GameObject) null;
    List<IAssignableIdentity> assignableIdentityList = new List<IAssignableIdentity>();
    foreach (IAssignableIdentity assignableIdentity in (IEnumerable<IAssignableIdentity>) members)
    {
      MinionAssignablesProxy assignablesProxy = (MinionAssignablesProxy) assignableIdentity;
      if ((UnityEngine.Object) assignablesProxy != (UnityEngine.Object) null)
      {
        MinionResume component = assignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
          assignableIdentityList.Add(assignableIdentity);
      }
    }
    if (assignableIdentityList.Count == 0)
      return (GameObject) null;
    foreach (MinionAssignablesProxy assignablesProxy in assignableIdentityList)
    {
      GameObject targetGameObject = assignablesProxy.GetTargetGameObject();
      if (targetGameObject.GetMyWorldId() == (int) Grid.WorldIdx[this.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
        return targetGameObject;
    }
    return ((MinionAssignablesProxy) assignableIdentityList[0]).GetTargetGameObject();
  }

  public Tuple<int, int> GetCrewBoardedFraction()
  {
    ICollection<IAssignableIdentity> members = (ICollection<IAssignableIdentity>) this.GetComponent<AssignmentGroupController>().GetMembers();
    if (members.Count == 0)
      return new Tuple<int, int>(0, 0);
    int num = 0;
    foreach (MinionAssignablesProxy assignablesProxy in (IEnumerable<IAssignableIdentity>) members)
    {
      if (assignablesProxy.GetTargetGameObject().GetMyWorldId() != (int) Grid.WorldIdx[this.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
        ++num;
    }
    return new Tuple<int, int>(members.Count - num, members.Count);
  }

  public bool HasCrewAssigned()
  {
    return this.GetComponent<AssignmentGroupController>().GetMembers().Count > 0;
  }

  public int GetCrewCount() => this.GetComponent<AssignmentGroupController>().GetMembers().Count;

  public bool CheckPassengersBoarded(bool require_pilot = true)
  {
    ICollection<IAssignableIdentity> members = (ICollection<IAssignableIdentity>) this.GetComponent<AssignmentGroupController>().GetMembers();
    if (members.Count == 0)
      return false;
    if (require_pilot)
    {
      bool flag = false;
      foreach (MinionAssignablesProxy assignablesProxy in (IEnumerable<IAssignableIdentity>) members)
      {
        if ((UnityEngine.Object) assignablesProxy != (UnityEngine.Object) null)
        {
          MinionResume component = assignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return false;
    }
    foreach (MinionAssignablesProxy assignablesProxy in (IEnumerable<IAssignableIdentity>) members)
    {
      if (assignablesProxy.GetTargetGameObject().GetMyWorldId() != (int) Grid.WorldIdx[this.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
        return false;
    }
    return true;
  }

  public bool CheckExtraPassengers()
  {
    ClustercraftExteriorDoor component = this.GetComponent<ClustercraftExteriorDoor>();
    if (component.HasTargetWorld())
    {
      byte worldId = Grid.WorldIdx[component.TargetCell()];
      List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems((int) worldId);
      string assignmentGroupId = this.GetComponent<AssignmentGroupController>().AssignmentGroupID;
      for (int index = 0; index < worldItems.Count; ++index)
      {
        if (!Game.Instance.assignmentManager.assignment_groups[assignmentGroupId].HasMember((IAssignableIdentity) worldItems[index].assignableProxy.Get()))
          return true;
      }
    }
    return false;
  }

  public void RemoveRocketPassenger(MinionIdentity minion)
  {
    if (!((UnityEngine.Object) minion != (UnityEngine.Object) null))
      return;
    string assignmentGroupId = this.GetComponent<AssignmentGroupController>().AssignmentGroupID;
    MinionAssignablesProxy member = minion.assignableProxy.Get();
    if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupId].HasMember((IAssignableIdentity) member))
      Game.Instance.assignmentManager.assignment_groups[assignmentGroupId].RemoveMember((IAssignableIdentity) member);
    this.RefreshOrders();
  }

  public void RemovePassengersOnOtherWorlds()
  {
    ClustercraftExteriorDoor component1 = this.GetComponent<ClustercraftExteriorDoor>();
    if (!component1.HasTargetWorld())
      return;
    int myWorldId = component1.GetMyWorldId();
    string assignmentGroupId = this.GetComponent<AssignmentGroupController>().AssignmentGroupID;
    foreach (MinionIdentity component2 in Components.LiveMinionIdentities.Items)
    {
      MinionAssignablesProxy member = component2.assignableProxy.Get();
      if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupId].HasMember((IAssignableIdentity) member) && component2.GetMyParentWorldId() != myWorldId)
        Game.Instance.assignmentManager.assignment_groups[assignmentGroupId].RemoveMember((IAssignableIdentity) member);
    }
  }

  public void ClearMinionAssignments(object data)
  {
    foreach (IAssignableIdentity member in Game.Instance.assignmentManager.assignment_groups[this.GetComponent<AssignmentGroupController>().AssignmentGroupID].GetMembers())
      Game.Instance.assignmentManager.RemoveFromWorld(member, this.GetMyWorldId());
  }

  public enum RequestCrewState
  {
    Release,
    Request,
  }
}
