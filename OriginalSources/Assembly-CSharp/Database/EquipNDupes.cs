// Decompiled with JetBrains decompiler
// Type: Database.EquipNDupes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
namespace Database;

public class EquipNDupes : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private AssignableSlot equipmentSlot;
  private int numToEquip;

  public EquipNDupes(AssignableSlot equipmentSlot, int numToEquip)
  {
    this.equipmentSlot = equipmentSlot;
    this.numToEquip = numToEquip;
  }

  public override bool Success()
  {
    int num = 0;
    foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
    {
      Equipment equipment = minionIdentity.GetEquipment();
      if ((Object) equipment != (Object) null && equipment.IsSlotOccupied(this.equipmentSlot))
        ++num;
    }
    return num >= this.numToEquip;
  }

  public void Deserialize(IReader reader)
  {
    string id = reader.ReadKleiString();
    this.equipmentSlot = Db.Get().AssignableSlots.Get(id);
    this.numToEquip = reader.ReadInt32();
  }

  public override string GetProgress(bool complete)
  {
    int num = 0;
    foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
    {
      Equipment equipment = minionIdentity.GetEquipment();
      if ((Object) equipment != (Object) null && equipment.IsSlotOccupied(this.equipmentSlot))
        ++num;
    }
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CLOTHE_DUPES, (object) (complete ? this.numToEquip : num), (object) this.numToEquip);
  }
}
