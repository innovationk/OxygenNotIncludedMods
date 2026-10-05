// Decompiled with JetBrains decompiler
// Type: CreatureFeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
  public Storage[] storages;
  public string effectId;
  public CellOffset feederOffset = CellOffset.none;
  private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>((Action<CreatureFeeder, object>) ((component, data) => component.OnAteFromStorage(data)));

  protected override void OnSpawn()
  {
    this.storages = this.GetComponents<Storage>();
    Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
    this.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
  }

  protected override void OnCleanUp()
  {
    Components.CreatureFeeders.Remove(this.GetMyWorldId(), this);
  }

  private void OnAteFromStorage(object data)
  {
    if (string.IsNullOrEmpty(this.effectId))
      return;
    (data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
  }

  public bool StoragesAreEmpty()
  {
    foreach (Storage storage in this.storages)
    {
      if (!((UnityEngine.Object) storage == (UnityEngine.Object) null) && storage.Count > 0)
        return false;
    }
    return true;
  }

  public Vector2I GetTargetFeederCell()
  {
    return Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.feederOffset));
  }
}
