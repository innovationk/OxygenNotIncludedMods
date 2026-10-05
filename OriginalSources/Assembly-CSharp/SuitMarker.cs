// Decompiled with JetBrains decompiler
// Type: SuitMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SuitMarker")]
public class SuitMarker : KMonoBehaviour
{
  [MyCmpGet]
  private Building building;
  private SuitMarker.SuitMarkerReactable equipReactable;
  private SuitMarker.SuitMarkerReactable unequipReactable;
  private bool hasAvailableSuit;
  [Serialize]
  private bool onlyTraverseIfUnequipAvailable;
  private Grid.SuitMarker.Flags gridFlags;
  private int cell;
  public Tag[] LockerTags;
  public PathFinder.PotentialPath.Flags PathFlag;
  public KAnimFile interactAnim = Assets.GetAnim((HashedString) "anim_equip_clothing_kanim");
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((Action<SuitMarker, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((Action<SuitMarker, object>) ((component, data) => component.OnOperationalChanged(((Boxed<bool>) data).value)));
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRotatedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((Action<SuitMarker, object>) ((component, data) => component.isRotated = ((Rotatable) data).IsRotated));

  private bool OnlyTraverseIfUnequipAvailable
  {
    get
    {
      DebugUtil.Assert(this.onlyTraverseIfUnequipAvailable == ((this.gridFlags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != 0));
      return this.onlyTraverseIfUnequipAvailable;
    }
    set
    {
      this.onlyTraverseIfUnequipAvailable = value;
      this.UpdateGridFlag(Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable, this.onlyTraverseIfUnequipAvailable);
    }
  }

  private bool isRotated
  {
    get => (this.gridFlags & Grid.SuitMarker.Flags.Rotated) != 0;
    set => this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
  }

  private bool isOperational
  {
    get => (this.gridFlags & Grid.SuitMarker.Flags.Operational) != 0;
    set => this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnlyTraverseIfUnequipAvailable = this.onlyTraverseIfUnequipAvailable;
    Debug.Assert((UnityEngine.Object) this.interactAnim != (UnityEngine.Object) null, (object) "interactAnim is null");
    this.Subscribe<SuitMarker>(493375141, SuitMarker.OnRefreshUserMenuDelegate);
    this.isOperational = this.GetComponent<Operational>().IsOperational;
    this.Subscribe<SuitMarker>(-592767678, SuitMarker.OnOperationalChangedDelegate);
    this.isRotated = this.GetComponent<Rotatable>().IsRotated;
    this.Subscribe<SuitMarker>(-1643076535, SuitMarker.OnRotatedDelegate);
    this.CreateNewEquipReactable();
    this.CreateNewUnequipReactable();
    this.cell = Grid.PosToCell((KMonoBehaviour) this);
    Grid.RegisterSuitMarker(this.cell);
    this.GetComponent<KAnimControllerBase>().Play((HashedString) "no_suit");
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits);
    this.RefreshTraverseIfUnequipStatusItem();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
  }

  private void CreateNewEquipReactable()
  {
    this.equipReactable = (SuitMarker.SuitMarkerReactable) new SuitMarker.EquipSuitReactable(this);
  }

  private void CreateNewUnequipReactable()
  {
    this.unequipReactable = (SuitMarker.SuitMarkerReactable) new SuitMarker.UnequipSuitReactable(this);
  }

  public void GetAttachedLockers(List<SuitLocker> suit_lockers)
  {
    int num1 = this.isRotated ? 1 : -1;
    int num2 = 1;
    while (true)
    {
      int cell = Grid.OffsetCell(this.cell, num2 * num1, 0);
      GameObject gameObject = Grid.Objects[cell, 1];
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        KPrefabID component1 = gameObject.GetComponent<KPrefabID>();
        if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
        {
          if (component1.IsAnyPrefabID(this.LockerTags))
          {
            SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
            if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null))
            {
              Operational component3 = gameObject.GetComponent<Operational>();
              if ((!((UnityEngine.Object) component3 != (UnityEngine.Object) null) || component3.GetFlag(BuildingEnabledButton.EnabledFlag)) && !suit_lockers.Contains(component2))
                suit_lockers.Add(component2);
            }
            else
              goto label_10;
          }
          else
            goto label_9;
        }
        ++num2;
      }
      else
        break;
    }
    return;
label_9:
    return;
label_10:;
  }

  public static bool DoesTraversalDirectionRequireSuit(
    int source_cell,
    int dest_cell,
    Grid.SuitMarker.Flags flags)
  {
    return Grid.CellColumn(dest_cell) > Grid.CellColumn(source_cell) == ((flags & Grid.SuitMarker.Flags.Rotated) == (Grid.SuitMarker.Flags) 0);
  }

  public bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell)
  {
    return SuitMarker.DoesTraversalDirectionRequireSuit(source_cell, dest_cell, this.gridFlags);
  }

  private void Update()
  {
    ListPool<SuitLocker, SuitMarker>.PooledList suit_lockers = ListPool<SuitLocker, SuitMarker>.Allocate();
    this.GetAttachedLockers((List<SuitLocker>) suit_lockers);
    int emptyLockerCount = 0;
    int fullLockerCount = 0;
    KPrefabID kprefabId = (KPrefabID) null;
    foreach (SuitLocker suitLocker in (List<SuitLocker>) suit_lockers)
    {
      if (suitLocker.CanDropOffSuit())
        ++emptyLockerCount;
      if ((UnityEngine.Object) suitLocker.GetPartiallyChargedOutfit() != (UnityEngine.Object) null)
        ++fullLockerCount;
      if ((UnityEngine.Object) kprefabId == (UnityEngine.Object) null)
        kprefabId = suitLocker.GetStoredOutfit();
    }
    suit_lockers.Recycle();
    bool flag = (UnityEngine.Object) kprefabId != (UnityEngine.Object) null;
    if (flag != this.hasAvailableSuit)
    {
      this.GetComponent<KAnimControllerBase>().Play((HashedString) (flag ? "off" : "no_suit"));
      this.hasAvailableSuit = flag;
    }
    Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
  }

  private void RefreshTraverseIfUnequipStatusItem()
  {
    if (this.OnlyTraverseIfUnequipAvailable)
    {
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable);
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime);
    }
    else
    {
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable);
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime);
    }
  }

  private void OnEnableTraverseIfUnequipAvailable()
  {
    this.OnlyTraverseIfUnequipAvailable = true;
    this.RefreshTraverseIfUnequipStatusItem();
  }

  private void OnDisableTraverseIfUnequipAvailable()
  {
    this.OnlyTraverseIfUnequipAvailable = false;
    this.RefreshTraverseIfUnequipStatusItem();
  }

  private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
  {
    if (state)
      this.gridFlags |= flag;
    else
      this.gridFlags &= ~flag;
  }

  private void OnOperationalChanged(bool isOperational)
  {
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
    this.isOperational = isOperational;
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, !this.OnlyTraverseIfUnequipAvailable ? new KIconButtonMenu.ButtonInfo("action_clearance", (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.NAME, new System.Action(this.OnEnableTraverseIfUnequipAvailable), tooltipText: (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_clearance", (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.NAME, new System.Action(this.OnDisableTraverseIfUnequipAvailable), tooltipText: (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.TOOLTIP));
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.isSpawned)
      Grid.UnregisterSuitMarker(this.cell);
    if (this.equipReactable != null)
      this.equipReactable.Cleanup();
    if (this.unequipReactable != null)
      this.unequipReactable.Cleanup();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), (GameObject) null);
  }

  private class EquipSuitReactable(SuitMarker marker) : SuitMarker.SuitMarkerReactable((HashedString) nameof (EquipSuitReactable), marker)
  {
    public override bool InternalCanBegin(
      GameObject newReactor,
      Navigator.ActiveTransition transition)
    {
      return !newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && base.InternalCanBegin(newReactor, transition);
    }

    protected override void InternalBegin()
    {
      base.InternalBegin();
      this.suitMarker.CreateNewEquipReactable();
    }

    protected override bool MovingTheRightWay(
      GameObject newReactor,
      Navigator.ActiveTransition transition)
    {
      bool flag = transition.navGridTransition.x < 0;
      return this.IsRocketDoorExitEquip(newReactor, transition) || flag == this.suitMarker.isRotated;
    }

    private bool IsRocketDoorExitEquip(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      bool flag = transition.end != NavType.Teleport && transition.start != NavType.Teleport;
      return transition.navGridTransition.x == 0 && new_reactor.GetMyWorld().IsModuleInterior && !flag;
    }

    protected override void Run()
    {
      ListPool<SuitLocker, SuitMarker>.PooledList suit_lockers = ListPool<SuitLocker, SuitMarker>.Allocate();
      this.suitMarker.GetAttachedLockers((List<SuitLocker>) suit_lockers);
      SuitLocker suitLocker = (SuitLocker) null;
      for (int index = 0; index < suit_lockers.Count; ++index)
      {
        float suitScore = suit_lockers[index].GetSuitScore();
        if ((double) suitScore >= 1.0)
        {
          suitLocker = suit_lockers[index];
          break;
        }
        if ((UnityEngine.Object) suitLocker == (UnityEngine.Object) null || (double) suitScore > (double) suitLocker.GetSuitScore())
          suitLocker = suit_lockers[index];
      }
      suit_lockers.Recycle();
      if (!((UnityEngine.Object) suitLocker != (UnityEngine.Object) null))
        return;
      Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
      SuitWearer.Instance smi = this.reactor.GetSMI<SuitWearer.Instance>();
      suitLocker.EquipTo(equipment);
      smi.UnreserveSuits();
      this.suitMarker.Update();
    }
  }

  private class UnequipSuitReactable(SuitMarker marker) : SuitMarker.SuitMarkerReactable((HashedString) nameof (UnequipSuitReactable), marker)
  {
    public override bool InternalCanBegin(
      GameObject newReactor,
      Navigator.ActiveTransition transition)
    {
      Navigator component = newReactor.GetComponent<Navigator>();
      return (!newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) ? 0 : (!((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : ((component.flags & this.suitMarker.PathFlag) != 0 ? 1 : 0))) != 0 && base.InternalCanBegin(newReactor, transition);
    }

    protected override void InternalBegin()
    {
      base.InternalBegin();
      this.suitMarker.CreateNewUnequipReactable();
    }

    protected override bool MovingTheRightWay(
      GameObject newReactor,
      Navigator.ActiveTransition transition)
    {
      bool flag = transition.navGridTransition.x < 0;
      return transition.navGridTransition.x != 0 && flag != this.suitMarker.isRotated;
    }

    protected override void Run()
    {
      Navigator component = this.reactor.GetComponent<Navigator>();
      Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
      if ((!((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : ((component.flags & this.suitMarker.PathFlag) != 0 ? 1 : 0)) != 0)
      {
        ListPool<SuitLocker, SuitMarker>.PooledList suit_lockers = ListPool<SuitLocker, SuitMarker>.Allocate();
        this.suitMarker.GetAttachedLockers((List<SuitLocker>) suit_lockers);
        SuitLocker suitLocker = (SuitLocker) null;
        for (int index = 0; (UnityEngine.Object) suitLocker == (UnityEngine.Object) null && index < suit_lockers.Count; ++index)
        {
          if (suit_lockers[index].CanDropOffSuit())
            suitLocker = suit_lockers[index];
        }
        suit_lockers.Recycle();
        if ((UnityEngine.Object) suitLocker != (UnityEngine.Object) null)
        {
          suitLocker.UnequipFrom(equipment);
          component.GetSMI<SuitWearer.Instance>().UnreserveSuits();
          this.suitMarker.Update();
          return;
        }
      }
      Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
      if (!((UnityEngine.Object) assignable != (UnityEngine.Object) null))
        return;
      assignable.Unassign();
      Notification notification = new Notification((string) MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP));
      assignable.GetComponent<Notifier>().Add(notification);
    }
  }

  private abstract class SuitMarkerReactable : Reactable
  {
    protected SuitMarker suitMarker;
    protected float startTime;

    public SuitMarkerReactable(HashedString id, SuitMarker suit_marker)
      : base(suit_marker.gameObject, id, Db.Get().ChoreTypes.SuitMarker, 1, 1)
    {
      this.suitMarker = suit_marker;
    }

    public override bool InternalCanBegin(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
        return false;
      if ((UnityEngine.Object) this.suitMarker == (UnityEngine.Object) null)
      {
        this.Cleanup();
        return false;
      }
      return this.suitMarker.isOperational && this.MovingTheRightWay(new_reactor, transition);
    }

    protected override void InternalBegin()
    {
      this.startTime = Time.time;
      KBatchedAnimController component1 = this.reactor.GetComponent<KBatchedAnimController>();
      component1.AddAnimOverrides(this.suitMarker.interactAnim, 1f);
      component1.Play((HashedString) "working_pre");
      component1.Queue((HashedString) "working_loop");
      component1.Queue((HashedString) "working_pst");
      if (!this.suitMarker.HasTag(GameTags.JetSuitBlocker))
        return;
      KBatchedAnimController component2 = this.suitMarker.GetComponent<KBatchedAnimController>();
      component2.Play((HashedString) "working_pre");
      component2.Queue((HashedString) "working_loop");
      component2.Queue((HashedString) "working_pst");
    }

    public override void Update(float dt)
    {
      Facing component = (bool) (UnityEngine.Object) this.reactor ? this.reactor.GetComponent<Facing>() : (Facing) null;
      if ((bool) (UnityEngine.Object) component && (bool) (UnityEngine.Object) this.suitMarker)
        component.SetFacing(this.suitMarker.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
      if ((double) Time.time - (double) this.startTime <= 2.7999999523162842)
        return;
      if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null && (UnityEngine.Object) this.suitMarker != (UnityEngine.Object) null)
      {
        this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
        this.Run();
      }
      this.Cleanup();
    }

    protected override void InternalEnd()
    {
      if (!((UnityEngine.Object) this.reactor != (UnityEngine.Object) null))
        return;
      this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
    }

    protected override void InternalCleanup()
    {
    }

    protected abstract bool MovingTheRightWay(
      GameObject reactor,
      Navigator.ActiveTransition transition);

    protected abstract void Run();
  }
}
