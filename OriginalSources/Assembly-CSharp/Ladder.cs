// Decompiled with JetBrains decompiler
// Type: Ladder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Ladder")]
public class Ladder : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public float upwardsMovementSpeedMultiplier = 1f;
  public float downwardsMovementSpeedMultiplier = 1f;
  public bool isPole;
  public CellOffset[] offsets = new CellOffset[1]
  {
    CellOffset.none
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Rotatable component = this.GetComponent<Rotatable>();
    foreach (CellOffset offset1 in this.offsets)
    {
      CellOffset offset2 = offset1;
      if ((Object) component != (Object) null)
        offset2 = component.GetRotatedCellOffset(offset1);
      int i = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), offset2);
      Grid.HasPole[i] = this.isPole;
      Grid.HasLadder[i] = !this.isPole;
    }
    this.GetComponent<KPrefabID>().AddTag(GameTags.Ladders);
    Components.Ladders.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Rotatable component = this.GetComponent<Rotatable>();
    foreach (CellOffset offset1 in this.offsets)
    {
      CellOffset offset2 = offset1;
      if ((Object) component != (Object) null)
        offset2 = component.GetRotatedCellOffset(offset1);
      int num = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), offset2);
      if ((Object) Grid.Objects[num, 24] == (Object) null)
      {
        Grid.HasPole[num] = false;
        Grid.HasLadder[num] = false;
      }
    }
    Components.Ladders.Remove(this);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = (List<Descriptor>) null;
    if ((double) this.upwardsMovementSpeedMultiplier != 1.0)
    {
      descriptors = new List<Descriptor>();
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.GetFormattedPercent((float) ((double) this.upwardsMovementSpeedMultiplier * 100.0 - 100.0))), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.GetFormattedPercent((float) ((double) this.upwardsMovementSpeedMultiplier * 100.0 - 100.0))));
      descriptors.Add(descriptor);
    }
    return descriptors;
  }
}
