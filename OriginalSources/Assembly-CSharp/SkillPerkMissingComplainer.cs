// Decompiled with JetBrains decompiler
// Type: SkillPerkMissingComplainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SkillPerkMissingComplainer")]
public class SkillPerkMissingComplainer : KMonoBehaviour
{
  public string requiredSkillPerk;
  private int skillUpdateHandle = -1;
  private Guid workStatusItemHandle;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!string.IsNullOrEmpty(this.requiredSkillPerk))
      this.skillUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
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
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || string.IsNullOrEmpty(this.requiredSkillPerk))
      return;
    bool flag = MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId());
    if (!flag && this.workStatusItemHandle == Guid.Empty)
    {
      this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk, (object) this.requiredSkillPerk);
    }
    else
    {
      if (!flag || !(this.workStatusItemHandle != Guid.Empty))
        return;
      component.RemoveStatusItem(this.workStatusItemHandle);
      this.workStatusItemHandle = Guid.Empty;
    }
  }
}
