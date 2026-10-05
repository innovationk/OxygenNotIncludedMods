// Decompiled with JetBrains decompiler
// Type: ElectrobankTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElectrobankTracker")]
public class ElectrobankTracker : WorldResourceAmountTracker<ElectrobankTracker>, ISaveLoadable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ignoredTags = new Tag[GameTags.BionicIncompatibleBatteries.Count];
    GameTags.BionicIncompatibleBatteries.CopyTo(this.ignoredTags, 0);
    this.itemTag = GameTags.ChargedPortableBattery;
  }

  protected override WorldResourceAmountTracker<ElectrobankTracker>.ItemData GetItemData(
    Pickupable item)
  {
    Electrobank component = item.GetComponent<Electrobank>();
    return new WorldResourceAmountTracker<ElectrobankTracker>.ItemData()
    {
      ID = component.ID,
      amountValue = component.Charge * item.PrimaryElement.Units,
      units = item.PrimaryElement.Units
    };
  }
}
