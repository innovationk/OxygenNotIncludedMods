// Decompiled with JetBrains decompiler
// Type: MultiSkillPerkMissingComplainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MultiSkillPerkMissingComplainer")]
public class MultiSkillPerkMissingComplainer : KMonoBehaviour
{
  public string[] requiredSkillPerks;
  private KSelectable selectable;
  private int skillUpdateHandle = -1;
  private Guid workStatusItemHandle;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.selectable = this.GetComponent<KSelectable>();
    if (this.requiredSkillPerks != null && this.requiredSkillPerks.Length > 1)
      this.skillUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
    else
      Debug.LogWarning((object) $"Use SkillPerkMissingComplainer on {this.gameObject.name} if multiple skill perks are not required. This should only be used if a single dupe requires more than one perk.");
    this.UpdateStatusItem();
  }

  protected override void OnCleanUp()
  {
    if (this.skillUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.skillUpdateHandle);
    base.OnCleanUp();
  }

  protected virtual void UpdateStatusItem(object data = null)
  {
    if ((UnityEngine.Object) this.selectable == (UnityEngine.Object) null || this.requiredSkillPerks == null)
      return;
    bool flag = MinionResume.AnyMinionHasAllPerks(this.requiredSkillPerks, this.GetMyWorldId());
    if (!flag && this.workStatusItemHandle == Guid.Empty)
    {
      this.workStatusItemHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksDupeWithMultiSkillPerk, (object) this.requiredSkillPerks);
    }
    else
    {
      if (!flag || !(this.workStatusItemHandle != Guid.Empty))
        return;
      this.selectable.RemoveStatusItem(this.workStatusItemHandle);
      this.workStatusItemHandle = Guid.Empty;
    }
  }
}
