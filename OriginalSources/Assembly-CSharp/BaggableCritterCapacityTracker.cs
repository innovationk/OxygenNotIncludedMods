// Decompiled with JetBrains decompiler
// Type: BaggableCritterCapacityTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaggableCritterCapacityTracker : KMonoBehaviour, ISim1000ms, IUserControlledCapacity
{
  public int maximumCreatures = 40;
  public bool requireLiquidOffset;
  public CellOffset cavityOffset;
  public bool filteredCount;
  public System.Action onCountChanged;
  private int cavityCell;
  [MyCmpReq]
  private TreeFilterable filter;
  [MyCmpGet]
  private Operational operational;
  private static readonly Operational.Flag isInLiquid = new Operational.Flag(nameof (isInLiquid), Operational.Flag.Type.Requirement);
  [MyCmpGet]
  private KSelectable selectable;
  private static StatusItem capacityStatusItem;
  private HandleVector<int>.Handle partitionerEntry;

  [Serialize]
  public int creatureLimit { get; set; } = 20;

  public int storedCreatureCount { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.cavityCell = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.cavityOffset);
    this.filter = this.GetComponent<TreeFilterable>();
    this.filter.OnFilterChanged += new Action<HashSet<Tag>>(this.RefreshCreatureCount);
    this.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
    if (this.requireLiquidOffset)
    {
      this.partitionerEntry = GameScenePartitioner.Instance.Add("BaggableCritterCapacityTracker.OnSpawn", (object) this.gameObject, new Extents(this.cavityCell, new CellOffset[1]
      {
        new CellOffset(0, 0)
      }), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
      this.OnLiquidChanged((object) null);
    }
    else
      this.Subscribe(144050788, new Action<object>(this.RefreshCreatureCount));
  }

  private void OnBuildingStrawChanged(object o)
  {
    this.UpdateCavityCell(((BuildingPointStraw) o).GetBottomCellOffset());
  }

  public void UpdateCavityCell(CellOffset newOffset)
  {
    this.cavityOffset = newOffset;
    this.cavityCell = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.cavityOffset);
    if (this.requireLiquidOffset)
    {
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
      this.partitionerEntry = GameScenePartitioner.Instance.Add("BaggableCritterCapacityTracker.UpdateCavityCell", (object) this.gameObject, new Extents(this.cavityCell, new CellOffset[1]
      {
        new CellOffset(0, 0)
      }), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
      this.OnLiquidChanged((object) null);
    }
    this.RefreshCreatureCount();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (BaggableCritterCapacityTracker.capacityStatusItem == null)
    {
      BaggableCritterCapacityTracker.capacityStatusItem = new StatusItem("CritterCapacity", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      BaggableCritterCapacityTracker.capacityStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IUserControlledCapacity controlledCapacity = (IUserControlledCapacity) data;
        string newValue1 = Util.FormatWholeNumber(Mathf.Floor(controlledCapacity.AmountStored));
        string newValue2 = Util.FormatWholeNumber(controlledCapacity.UserMaxCapacity);
        str = str.Replace("{Stored}", newValue1).Replace("{StoredUnits}", (string) ((int) controlledCapacity.AmountStored == 1 ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS)).Replace("{Capacity}", newValue2).Replace("{CapacityUnits}", (string) ((int) controlledCapacity.UserMaxCapacity == 1 ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS));
        return str;
      });
    }
    this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, BaggableCritterCapacityTracker.capacityStatusItem, (object) this);
    this.Subscribe(360192579, new Action<object>(this.OnBuildingStrawChanged));
  }

  protected override void OnCleanUp()
  {
    if (this.requireLiquidOffset)
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.filter.OnFilterChanged -= new Action<HashSet<Tag>>(this.RefreshCreatureCount);
    this.Unsubscribe(144050788);
    base.OnCleanUp();
  }

  private void OnLiquidChanged(object data)
  {
    if (!this.requireLiquidOffset)
      return;
    bool on = Grid.IsLiquid(this.cavityCell);
    if (on)
      this.RefreshCreatureCount();
    this.operational.SetFlag(BaggableCritterCapacityTracker.isInLiquid, on);
    this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !on, (object) this);
    this.selectable.ToggleStatusItem(BaggableCritterCapacityTracker.capacityStatusItem, on, (object) this);
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    BaggableCritterCapacityTracker component = gameObject.GetComponent<BaggableCritterCapacityTracker>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.creatureLimit = component.creatureLimit;
  }

  public void RefreshCreatureCount(object data = null)
  {
    int storedCreatureCount = this.storedCreatureCount;
    this.storedCreatureCount = !this.requireLiquidOffset ? this.RefreshOtherCreatureCount() : this.RefreshSwimmingCreatureCount();
    if (this.onCountChanged == null || this.storedCreatureCount == storedCreatureCount)
      return;
    this.onCountChanged();
  }

  private int RefreshOtherCreatureCount()
  {
    int num = 0;
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.cavityCell);
    if (cavityForCell != null)
    {
      foreach (KPrefabID creature in cavityForCell.creatures)
      {
        if (!creature.HasTag(GameTags.Creatures.Bagged) && !creature.HasTag(GameTags.Trapped) && (!this.filteredCount || this.filter.AcceptedTags.Contains(creature.PrefabTag)))
          ++num;
      }
    }
    return num;
  }

  private int RefreshSwimmingCreatureCount()
  {
    return FishOvercrowingManager.Instance.GetFishInPondCount(this.cavityCell, this.filter.AcceptedTags);
  }

  public void Sim1000ms(float dt) => this.RefreshCreatureCount();

  float IUserControlledCapacity.UserMaxCapacity
  {
    get => (float) this.creatureLimit;
    set
    {
      this.creatureLimit = Mathf.RoundToInt(value);
      if (this.onCountChanged == null)
        return;
      this.onCountChanged();
    }
  }

  float IUserControlledCapacity.AmountStored => (float) this.storedCreatureCount;

  float IUserControlledCapacity.MinCapacity => 0.0f;

  float IUserControlledCapacity.MaxCapacity => (float) this.maximumCreatures;

  bool IUserControlledCapacity.WholeValues => true;

  LocString IUserControlledCapacity.CapacityUnits
  {
    get => UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.UNITS_SUFFIX;
  }
}
