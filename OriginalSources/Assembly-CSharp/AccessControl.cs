// Decompiled with JetBrains decompiler
// Type: AccessControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AccessControl")]
public class AccessControl : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private bool isTeleporter;
  private int[] registeredBuildingCells;
  [Serialize]
  [Obsolete("Added support for Robots Access Controls, use savedPermissionsById", false)]
  private List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> savedPermissions = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
  [Serialize]
  [Obsolete("Added support for Robots Access Controls, use defaultPermissionByTag", false)]
  private AccessControl.Permission _defaultPermission;
  [Serialize]
  private List<KeyValuePair<Tag, AccessControl.Permission>> defaultPermissionByTag = new List<KeyValuePair<Tag, AccessControl.Permission>>();
  [Serialize]
  private List<KeyValuePair<int, AccessControl.Permission>> savedPermissionsById = new List<KeyValuePair<int, AccessControl.Permission>>();
  [Serialize]
  public bool registered = true;
  [Serialize]
  public bool controlEnabled;
  public Door.ControlState overrideAccess;
  private static StatusItem accessControlActive;
  private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate = new EventSystem.IntraObjectHandler<AccessControl>((Action<AccessControl, object>) ((component, data) => component.OnControlStateChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AccessControl>((Action<AccessControl, object>) ((component, data) => component.OnCopySettings(data)));

  private int GetTagId(Tag game_tag) => GridRestrictionSerializer.Instance.GetTagId(game_tag);

  public AccessControl.Permission GetDefaultPermission(Tag groupTag)
  {
    foreach (KeyValuePair<Tag, AccessControl.Permission> keyValuePair in this.defaultPermissionByTag)
    {
      if (keyValuePair.Key == groupTag)
        return keyValuePair.Value;
    }
    return AccessControl.Permission.Both;
  }

  public void SetDefaultPermission(Tag groupTag, AccessControl.Permission permission)
  {
    bool flag = false;
    KeyValuePair<Tag, AccessControl.Permission> keyValuePair = new KeyValuePair<Tag, AccessControl.Permission>(groupTag, permission);
    for (int index = 0; index < this.defaultPermissionByTag.Count; ++index)
    {
      if (this.defaultPermissionByTag[index].Key == groupTag)
      {
        this.defaultPermissionByTag[index] = keyValuePair;
        flag = true;
        break;
      }
    }
    if (!flag)
      this.defaultPermissionByTag.Add(keyValuePair);
    this.SetStatusItem();
    this.SetGridRestrictions(this.GetTagId(groupTag), permission);
  }

  public bool Online => true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (AccessControl.accessControlActive == null)
      AccessControl.accessControlActive = new StatusItem("accessControlActive", (string) BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME, (string) BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Subscribe<AccessControl>(279163026, AccessControl.OnControlStateChangedDelegate);
    this.Subscribe<AccessControl>(-905833192, AccessControl.OnCopySettingsDelegate);
  }

  [Obsolete("Added support for Robots Access Controls")]
  private void CheckForBadData()
  {
    List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> keyValuePairList = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
    foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> savedPermission in this.savedPermissions)
    {
      if ((UnityEngine.Object) savedPermission.Key.Get() == (UnityEngine.Object) null)
        keyValuePairList.Add(savedPermission);
    }
    foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in keyValuePairList)
      this.savedPermissions.Remove(keyValuePair);
  }

  private void UpgradeSavePreRobotDoorPermission()
  {
    ListPool<Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.PooledList pooledList = ListPool<Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.Allocate();
    for (int index = this.savedPermissions.Count - 1; index >= 0; --index)
    {
      KPrefabID kpid = this.savedPermissions[index].Key.Get();
      if ((UnityEngine.Object) kpid != (UnityEngine.Object) null)
      {
        MinionIdentity component = kpid.GetComponent<MinionIdentity>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          pooledList.Add(new Tuple<MinionAssignablesProxy, AccessControl.Permission>(component.assignableProxy.Get(), this.savedPermissions[index].Value));
          this.savedPermissions.RemoveAt(index);
          this.ClearGridRestrictions(kpid);
        }
      }
    }
    foreach (Tuple<MinionAssignablesProxy, AccessControl.Permission> tuple in (List<Tuple<MinionAssignablesProxy, AccessControl.Permission>>) pooledList)
      this.SetPermission(tuple.first, tuple.second);
    pooledList.Recycle();
  }

  private void UpgradeSavesToPostRobotDoorPermissions()
  {
    if (this._defaultPermission != AccessControl.Permission.Both)
    {
      this.SetDefaultPermission(GameTags.Minions.Models.Standard, this._defaultPermission);
      this.SetDefaultPermission(GameTags.Minions.Models.Bionic, this._defaultPermission);
      this._defaultPermission = AccessControl.Permission.Both;
    }
    foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> savedPermission in this.savedPermissions)
      this.SetPermission(savedPermission.Key.Get().GetComponent<MinionAssignablesProxy>(), savedPermission.Value);
    this.savedPermissions.Clear();
  }

  protected override void OnSpawn()
  {
    this.isTeleporter = (UnityEngine.Object) this.GetComponent<NavTeleporter>() != (UnityEngine.Object) null;
    base.OnSpawn();
    if (this.savedPermissions.Count > 0)
      this.CheckForBadData();
    if (this.registered)
    {
      this.RegisterInGrid(true);
      this.RestorePermissions();
    }
    this.UpgradeSavePreRobotDoorPermission();
    this.UpgradeSavesToPostRobotDoorPermissions();
    this.SetStatusItem();
  }

  protected override void OnCleanUp()
  {
    this.RegisterInGrid(false);
    base.OnCleanUp();
  }

  private void OnControlStateChanged(object data)
  {
    this.overrideAccess = ((Boxed<Door.ControlState>) data).value;
    this.SetStatusItem();
  }

  private void OnCopySettings(object data)
  {
    AccessControl component = ((GameObject) data).GetComponent<AccessControl>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.savedPermissionsById.Clear();
    foreach (KeyValuePair<int, AccessControl.Permission> keyValuePair in component.savedPermissionsById)
      this.SetPermission(keyValuePair.Key, keyValuePair.Value);
    this.defaultPermissionByTag = new List<KeyValuePair<Tag, AccessControl.Permission>>((IEnumerable<KeyValuePair<Tag, AccessControl.Permission>>) component.defaultPermissionByTag);
    foreach (KeyValuePair<Tag, AccessControl.Permission> keyValuePair in this.defaultPermissionByTag)
      this.SetGridRestrictions(this.GetTagId(keyValuePair.Key), keyValuePair.Value);
  }

  public void SetRegistered(bool newRegistered)
  {
    if (newRegistered && !this.registered)
    {
      this.RegisterInGrid(true);
      this.RestorePermissions();
    }
    else
    {
      if (newRegistered || !this.registered)
        return;
      this.RegisterInGrid(false);
    }
  }

  private void SetPermission(int id, AccessControl.Permission permission)
  {
    bool flag = false;
    for (int index = 0; index < this.savedPermissionsById.Count; ++index)
    {
      if (this.savedPermissionsById[index].Key == id)
      {
        flag = true;
        KeyValuePair<int, AccessControl.Permission> keyValuePair = this.savedPermissionsById[index];
        this.savedPermissionsById[index] = new KeyValuePair<int, AccessControl.Permission>(keyValuePair.Key, permission);
        break;
      }
    }
    if (!flag)
      this.savedPermissionsById.Add(new KeyValuePair<int, AccessControl.Permission>(id, permission));
    this.SetStatusItem();
    this.SetGridRestrictions(id, permission);
  }

  public void SetPermission(MinionAssignablesProxy key, AccessControl.Permission permission)
  {
    this.SetPermission(key.GetComponent<KPrefabID>().InstanceID, permission);
  }

  public void SetPermission(Tag gameTag, AccessControl.Permission permission)
  {
    this.SetPermission(this.GetTagId(gameTag), permission);
  }

  private void RestorePermissions()
  {
    foreach (KeyValuePair<Tag, AccessControl.Permission> keyValuePair in this.defaultPermissionByTag)
      this.SetGridRestrictions(this.GetTagId(keyValuePair.Key), keyValuePair.Value);
    foreach (KeyValuePair<int, AccessControl.Permission> keyValuePair in this.savedPermissionsById)
      this.SetGridRestrictions(keyValuePair.Key, keyValuePair.Value);
  }

  private void RegisterInGrid(bool register)
  {
    Building component1 = this.GetComponent<Building>();
    OccupyArea component2 = this.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null && (UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    if (register)
    {
      Rotatable component3 = this.GetComponent<Rotatable>();
      Grid.Restriction.Orientation orientation = this.isTeleporter ? Grid.Restriction.Orientation.SingleCell : ((UnityEngine.Object) component3 == (UnityEngine.Object) null || component3.GetOrientation() == Orientation.Neutral ? Grid.Restriction.Orientation.Vertical : Grid.Restriction.Orientation.Horizontal);
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        this.registeredBuildingCells = component1.PlacementCells;
        foreach (int registeredBuildingCell in this.registeredBuildingCells)
          Grid.RegisterRestriction(registeredBuildingCell, orientation);
      }
      else
      {
        foreach (CellOffset occupiedCellsOffset in component2.OccupiedCellsOffsets)
          Grid.RegisterRestriction(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) component2), occupiedCellsOffset), orientation);
      }
      if (this.isTeleporter)
        Grid.RegisterRestriction(this.GetComponent<NavTeleporter>().GetCell(), orientation);
    }
    else
    {
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        if (component1.GetMyWorldId() != (int) byte.MaxValue && this.registeredBuildingCells != null)
        {
          foreach (int registeredBuildingCell in this.registeredBuildingCells)
            Grid.UnregisterRestriction(registeredBuildingCell);
          this.registeredBuildingCells = (int[]) null;
        }
      }
      else
      {
        foreach (CellOffset occupiedCellsOffset in component2.OccupiedCellsOffsets)
          Grid.UnregisterRestriction(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) component2), occupiedCellsOffset));
      }
      if (this.isTeleporter)
      {
        int cell = this.GetComponent<NavTeleporter>().GetCell();
        if (cell != Grid.InvalidCell)
          Grid.UnregisterRestriction(cell);
      }
    }
    this.registered = register;
  }

  private void SetGridRestrictions(int id, AccessControl.Permission permission)
  {
    if (!this.registered || !this.isSpawned)
      return;
    Building component1 = this.GetComponent<Building>();
    OccupyArea component2 = this.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null && (UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    int minionInstanceID = id;
    Grid.Restriction.Directions directions = (Grid.Restriction.Directions) 0;
    switch (permission)
    {
      case AccessControl.Permission.Both:
        directions = (Grid.Restriction.Directions) 0;
        break;
      case AccessControl.Permission.GoLeft:
        directions = Grid.Restriction.Directions.Right;
        break;
      case AccessControl.Permission.GoRight:
        directions = Grid.Restriction.Directions.Left;
        break;
      case AccessControl.Permission.Neither:
        directions = Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right;
        break;
    }
    if (this.isTeleporter)
      directions = directions == (Grid.Restriction.Directions) 0 ? (Grid.Restriction.Directions) 0 : Grid.Restriction.Directions.Teleport;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      foreach (int registeredBuildingCell in this.registeredBuildingCells)
        Grid.SetRestriction(registeredBuildingCell, minionInstanceID, directions);
    }
    else
    {
      foreach (CellOffset occupiedCellsOffset in component2.OccupiedCellsOffsets)
        Grid.SetRestriction(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) component2), occupiedCellsOffset), minionInstanceID, directions);
    }
    if (!this.isTeleporter)
      return;
    Grid.SetRestriction(this.GetComponent<NavTeleporter>().GetCell(), minionInstanceID, directions);
  }

  private void ClearGridRestrictions(KPrefabID kpid)
  {
    if ((UnityEngine.Object) kpid == (UnityEngine.Object) null)
      return;
    Building component1 = this.GetComponent<Building>();
    OccupyArea component2 = this.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null && (UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    int instanceId = kpid.InstanceID;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      foreach (int registeredBuildingCell in this.registeredBuildingCells)
        Grid.ClearRestriction(registeredBuildingCell, instanceId);
    }
    else
    {
      foreach (CellOffset occupiedCellsOffset in component2.OccupiedCellsOffsets)
        Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) component2), occupiedCellsOffset), instanceId);
    }
  }

  private void ClearGridRestrictions(int id, Tag default_id)
  {
    Building component1 = this.GetComponent<Building>();
    OccupyArea component2 = this.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null && (UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    int minionInstanceID = this.GetTagId(default_id);
    if (id != Tag.Invalid.GetHash())
      minionInstanceID = id;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      foreach (int registeredBuildingCell in this.registeredBuildingCells)
        Grid.ClearRestriction(registeredBuildingCell, minionInstanceID);
    }
    else
    {
      foreach (CellOffset occupiedCellsOffset in component2.OccupiedCellsOffsets)
        Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) component2), occupiedCellsOffset), minionInstanceID);
    }
  }

  public AccessControl.Permission GetSetPermission(MinionAssignablesProxy key)
  {
    return this.GetSetPermission(key.GetComponent<KPrefabID>().InstanceID, key.GetMinionModel());
  }

  public AccessControl.Permission GetSetPermission(Tag robotTag)
  {
    return this.GetSetPermission(this.GetTagId(robotTag), GameTags.Robot);
  }

  public AccessControl.Permission GetSetPermission(int primary_id, Tag secondary_id)
  {
    AccessControl.Permission defaultPermission = this.GetDefaultPermission(secondary_id);
    for (int index = 0; index < this.savedPermissionsById.Count; ++index)
    {
      KeyValuePair<int, AccessControl.Permission> keyValuePair = this.savedPermissionsById[index];
      if (keyValuePair.Key == primary_id)
      {
        keyValuePair = this.savedPermissionsById[index];
        defaultPermission = keyValuePair.Value;
        break;
      }
    }
    return defaultPermission;
  }

  public void ClearPermission(MinionAssignablesProxy key)
  {
    KPrefabID component = key.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      this.ClearPermission(component.InstanceID, key.GetMinionModel());
    this.SetStatusItem();
    this.ClearGridRestrictions(component.InstanceID, key.GetMinionModel());
  }

  public void ClearPermission(Tag tag, Tag default_key)
  {
    this.ClearPermission(this.GetTagId(tag), default_key);
  }

  private void ClearPermission(int key, Tag default_key)
  {
    for (int index = 0; index < this.savedPermissionsById.Count; ++index)
    {
      if (this.savedPermissionsById[index].Key == key)
      {
        this.savedPermissionsById.RemoveAt(index);
        break;
      }
    }
    this.SetStatusItem();
    this.ClearGridRestrictions(key, default_key);
  }

  public bool IsDefaultPermission(MinionAssignablesProxy key)
  {
    KPrefabID component = key.GetComponent<KPrefabID>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) || this.IsDefaultPermission(component.InstanceID);
  }

  public bool IsDefaultPermission(Tag robotTag)
  {
    return this.IsDefaultPermission(this.GetTagId(robotTag));
  }

  private bool IsDefaultPermission(int id)
  {
    bool flag = false;
    for (int index = 0; index < this.savedPermissionsById.Count; ++index)
    {
      if (this.savedPermissionsById[index].Key == id)
      {
        flag = true;
        break;
      }
    }
    return !flag;
  }

  private void SetStatusItem()
  {
    if (this.overrideAccess == Door.ControlState.Locked)
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, (StatusItem) null);
    else if (this.defaultPermissionByTag.Any<KeyValuePair<Tag, AccessControl.Permission>>((Func<KeyValuePair<Tag, AccessControl.Permission>, bool>) (default_permission => default_permission.Value != 0)) || this.savedPermissionsById.Count > 0)
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, AccessControl.accessControlActive);
    else
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, (StatusItem) null);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.ACCESS_CONTROL, (string) UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL);
    descriptors.Add(descriptor);
    return descriptors;
  }

  public enum Permission
  {
    Both,
    GoLeft,
    GoRight,
    Neither,
  }
}
