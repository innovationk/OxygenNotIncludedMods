// Decompiled with JetBrains decompiler
// Type: FactionAlignment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
  [MyCmpReq]
  public KPrefabID kprefabID;
  [SerializeField]
  public bool canBePlayerTargeted = true;
  [SerializeField]
  public bool updatePrioritizable = true;
  [Serialize]
  private bool alignmentActive = true;
  public FactionManager.FactionID Alignment;
  [Serialize]
  private bool targeted;
  [Serialize]
  private bool targetable = true;
  private bool hasBeenRegisterInPriority;
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, (Action<FactionAlignment, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((Action<FactionAlignment, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((Action<FactionAlignment, object>) ((component, data) => component.SetPlayerTargeted(false)));
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((Action<FactionAlignment, object>) ((component, data) => component.OnQueueDestroyObject()));

  [MyCmpAdd]
  public Health health { get; private set; }

  public AttackableBase attackable { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.health = this.GetComponent<Health>();
    this.attackable = this.GetComponent<AttackableBase>();
    Components.FactionAlignments.Add(this);
    this.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
    this.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
    this.Subscribe<FactionAlignment>(1502190696, FactionAlignment.OnQueueDestroyObjectDelegate);
    if (this.alignmentActive)
      FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
    GameUtil.SubscribeToTags<FactionAlignment>(this, FactionAlignment.OnDeadTagAddedDelegate, true);
    this.SetPlayerTargeted(this.targeted);
    this.UpdateStatusItem();
  }

  protected override void OnPrefabInit()
  {
  }

  private void OnDeath(object data) => this.SetAlignmentActive(false);

  public void SetAlignmentActive(bool active)
  {
    this.SetPlayerTargetable(active);
    this.alignmentActive = active;
    if (active)
      FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
    else
      FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
  }

  public bool IsAlignmentActive()
  {
    return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
  }

  public bool IsPlayerTargeted() => this.targeted;

  public void SetPlayerTargetable(bool state)
  {
    this.targetable = state && this.canBePlayerTargeted;
    if (state)
      return;
    this.SetPlayerTargeted(false);
  }

  public void SetPlayerTargeted(bool state)
  {
    this.targeted = this.canBePlayerTargeted & state && this.targetable;
    if (state)
    {
      if (!Components.PlayerTargeted.Items.Contains(this))
        Components.PlayerTargeted.Add(this);
      this.SetPrioritizable(true);
    }
    else
    {
      Components.PlayerTargeted.Remove(this);
      this.SetPrioritizable(false);
    }
    this.UpdateStatusItem();
  }

  private void UpdateStatusItem()
  {
    if (this.targeted)
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack);
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack);
  }

  private void SetPrioritizable(bool enable)
  {
    Prioritizable component = this.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !this.updatePrioritizable)
      return;
    if (enable && !this.hasBeenRegisterInPriority)
    {
      Prioritizable.AddRef(this.gameObject);
      this.hasBeenRegisterInPriority = true;
    }
    else
    {
      if (enable || !component.IsPrioritizable() || !this.hasBeenRegisterInPriority)
        return;
      Prioritizable.RemoveRef(this.gameObject);
      this.hasBeenRegisterInPriority = false;
    }
  }

  public void SwitchAlignment(FactionManager.FactionID newAlignment)
  {
    this.SetAlignmentActive(false);
    this.Alignment = newAlignment;
    this.SetAlignmentActive(true);
    this.BoxingTrigger<FactionManager.FactionID>(-971105736, newAlignment);
  }

  private void OnQueueDestroyObject()
  {
    FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
    Components.FactionAlignments.Remove(this);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.Alignment == FactionManager.FactionID.Duplicant || !this.canBePlayerTargeted || !this.IsAlignmentActive())
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.targeted ? new KIconButtonMenu.ButtonInfo("action_attack", (string) UI.USERMENUACTIONS.ATTACK.NAME, (System.Action) (() => this.SetPlayerTargeted(true)), tooltipText: (string) UI.USERMENUACTIONS.ATTACK.TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_attack", (string) UI.USERMENUACTIONS.CANCELATTACK.NAME, (System.Action) (() => this.SetPlayerTargeted(false)), tooltipText: (string) UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP));
  }
}
