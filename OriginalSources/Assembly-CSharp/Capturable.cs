// Decompiled with JetBrains decompiler
// Type: Capturable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Capturable")]
public class Capturable : Workable
{
  [MyCmpAdd]
  private Baggable baggable;
  [MyCmpAdd]
  private Prioritizable prioritizable;
  public bool allowCapture = true;
  [Serialize]
  private bool markedForCapture;
  private Chore chore;
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>((Action<Capturable, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>((Action<Capturable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>((Action<Capturable, object>) ((component, data) => component.OnTagsChanged(data)));

  public bool IsMarkedForCapture => this.markedForCapture;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Capturables.Add(this);
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.resetProgressOnStop = true;
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.multitoolContext = (HashedString) "capture";
    this.multitoolHitEffectTag = (Tag) "fx_capture_splash";
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
    this.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
    this.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
    if (this.markedForCapture)
      Prioritizable.AddRef(this.gameObject);
    this.UpdateStatusItem();
    this.UpdateChore();
    this.SetWorkTime(10f);
  }

  protected override void OnCleanUp()
  {
    Components.Capturables.Remove(this);
    base.OnCleanUp();
  }

  public override Vector3 GetTargetPoint()
  {
    Vector3 targetPoint = this.transform.GetPosition();
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      targetPoint = component.bounds.center;
    targetPoint.z = 0.0f;
    return targetPoint;
  }

  private void OnDeath(object data)
  {
    this.allowCapture = false;
    this.markedForCapture = false;
    this.UpdateChore();
  }

  private void OnTagsChanged(object _) => this.MarkForCapture(this.markedForCapture);

  public void MarkForCapture(bool mark)
  {
    PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
    this.MarkForCapture(mark, priority);
  }

  public void MarkForCapture(bool mark, PrioritySetting priority, bool updateMarkedPriority = false)
  {
    mark = mark && this.IsCapturable();
    if (this.markedForCapture && !mark)
      Prioritizable.RemoveRef(this.gameObject);
    else if (!this.markedForCapture & mark)
    {
      Prioritizable.AddRef(this.gameObject);
      Prioritizable component = this.GetComponent<Prioritizable>();
      if ((bool) (UnityEngine.Object) component)
        component.SetMasterPriority(priority);
    }
    else if (((!updateMarkedPriority ? 0 : (this.markedForCapture ? 1 : 0)) & (mark ? 1 : 0)) != 0)
    {
      Prioritizable component = this.GetComponent<Prioritizable>();
      if ((bool) (UnityEngine.Object) component)
        component.SetMasterPriority(priority);
    }
    this.markedForCapture = mark;
    this.UpdateStatusItem();
    this.UpdateChore();
  }

  public bool IsCapturable()
  {
    return this.allowCapture && !this.gameObject.HasTag(GameTags.Trapped) && !this.gameObject.HasTag(GameTags.Stored) && !this.gameObject.HasTag(GameTags.Creatures.Bagged);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.IsCapturable())
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.markedForCapture ? new KIconButtonMenu.ButtonInfo("action_capture", (string) UI.USERMENUACTIONS.CAPTURE.NAME, (System.Action) (() => this.MarkForCapture(true)), tooltipText: (string) UI.USERMENUACTIONS.CAPTURE.TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_capture", (string) UI.USERMENUACTIONS.CANCELCAPTURE.NAME, (System.Action) (() => this.MarkForCapture(false)), tooltipText: (string) UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP));
  }

  private void UpdateStatusItem()
  {
    this.shouldShowSkillPerkStatusItem = this.markedForCapture;
    this.UpdateStatusItem((object) null);
    if (this.markedForCapture)
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, (object) this);
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture);
  }

  private void UpdateChore()
  {
    if (this.markedForCapture && this.chore == null)
    {
      this.chore = (Chore) new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, (IStateMachineTarget) this, on_begin: new Action<Chore>(this.OnChoreBegins), on_end: new Action<Chore>(this.OnChoreEnds), is_preemptable: true);
    }
    else
    {
      if (this.markedForCapture || this.chore == null)
        return;
      this.chore.Cancel("not marked for capture");
      this.chore = (Chore) null;
    }
  }

  private void OnChoreBegins(Chore chore)
  {
    if (this.gameObject.GetSMI<FlopStates.Instance>() != null)
      return;
    IdleStates.Instance smi = this.gameObject.GetSMI<IdleStates.Instance>();
    if (smi == null)
      return;
    smi.GoTo((StateMachine.BaseState) smi.sm.root);
    smi.GetComponent<Navigator>().Stop();
  }

  private void OnChoreEnds(Chore chore)
  {
    if (this.gameObject.GetSMI<FlopStates.Instance>() != null)
      return;
    IdleStates.Instance smi = this.gameObject.GetSMI<IdleStates.Instance>();
    smi?.GoTo(smi.sm.GetDefaultState());
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    this.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.StunnedForCapture);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.StunnedForCapture);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    int num1 = this.NaturalBuildingCell();
    if (Grid.Solid[num1])
    {
      int num2 = Grid.CellAbove(num1);
      if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
        num1 = num2;
    }
    this.MarkForCapture(false);
    this.baggable.SetWrangled();
    this.baggable.transform.SetPosition(Grid.CellToPosCCC(num1, Grid.SceneLayer.Ore));
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    if (this.allowCapture)
      descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE));
    return descriptors;
  }
}
