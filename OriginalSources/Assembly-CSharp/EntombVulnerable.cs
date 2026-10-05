// Decompiled with JetBrains decompiler
// Type: EntombVulnerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

#nullable disable
public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpGet]
  private Operational operational;
  private OccupyArea _occupyArea;
  [Serialize]
  private bool isEntombed;
  private StatusItem DefaultEntombedStatusItem = Db.Get().CreatureStatusItems.Entombed;
  [NonSerialized]
  private StatusItem EntombedStatusItem;
  private bool showStatusItemOnEntombed = true;
  public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);
  private HandleVector<int>.Handle partitionerEntry;
  private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (Func<int, object, bool>) ((cell, data) => EntombVulnerable.IsCellSafeCB(cell, data));

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  public bool GetEntombed => this.isEntombed;

  public void SetStatusItem(StatusItem si)
  {
    bool statusItemOnEntombed = this.showStatusItemOnEntombed;
    this.SetShowStatusItemOnEntombed(false);
    this.EntombedStatusItem = si;
    this.SetShowStatusItemOnEntombed(statusItemOnEntombed);
  }

  public void SetShowStatusItemOnEntombed(bool val)
  {
    this.showStatusItemOnEntombed = val;
    if (!this.isEntombed || this.EntombedStatusItem == null)
      return;
    if (this.showStatusItemOnEntombed)
      this.selectable.AddStatusItem(this.EntombedStatusItem);
    else
      this.selectable.RemoveStatusItem(this.EntombedStatusItem);
  }

  public string WiltStateString
  {
    get
    {
      return Db.Get().CreatureStatusItems.Entombed.resolveStringCallback((string) CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, (object) this.gameObject);
    }
  }

  public WiltCondition.Condition[] Conditions
  {
    get
    {
      return new WiltCondition.Condition[1]
      {
        WiltCondition.Condition.Entombed
      };
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.EntombedStatusItem == null)
      this.EntombedStatusItem = this.DefaultEntombedStatusItem;
    this.partitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntombVulnerable), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
    this.CheckEntombed();
    if (!this.isEntombed)
      return;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Entombed);
    this.Trigger(-1089732772, (object) BoxedBools.True);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnSolidChanged(object data) => this.CheckEntombed();

  private void CheckEntombed()
  {
    int cell = Grid.PosToCell(this.gameObject.transform.GetPosition());
    if (!Grid.IsValidCell(cell))
      return;
    if (!this.IsCellSafe(cell))
    {
      if (!this.isEntombed)
      {
        this.isEntombed = true;
        if (this.showStatusItemOnEntombed)
          this.selectable.AddStatusItem(this.EntombedStatusItem, (object) this.gameObject);
        this.GetComponent<KPrefabID>().AddTag(GameTags.Entombed);
        this.Trigger(-1089732772, (object) BoxedBools.True);
      }
    }
    else if (this.isEntombed)
    {
      this.isEntombed = false;
      this.selectable.RemoveStatusItem(this.EntombedStatusItem);
      this.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
      this.Trigger(-1089732772, (object) BoxedBools.False);
    }
    if (!((UnityEngine.Object) this.operational != (UnityEngine.Object) null))
      return;
    this.operational.SetFlag(EntombVulnerable.notEntombedFlag, !this.isEntombed);
  }

  public bool IsCellSafe(int cell)
  {
    return this.occupyArea.TestArea(cell, (object) null, EntombVulnerable.IsCellSafeCBDelegate);
  }

  private static bool IsCellSafeCB(int cell, object data)
  {
    return Grid.IsValidCell(cell) && !Grid.Solid[cell];
  }
}
