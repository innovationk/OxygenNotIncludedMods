// Decompiled with JetBrains decompiler
// Type: MinionBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MinionBrain : Brain
{
  [MyCmpReq]
  public Navigator Navigator;
  [MyCmpGet]
  public OxygenBreather OxygenBreather;
  private float lastResearchCompleteEmoteTime;
  private static readonly EventSystem.IntraObjectHandler<MinionBrain> AnimTrackStoredItemDelegate = new EventSystem.IntraObjectHandler<MinionBrain>((Action<MinionBrain, object>) ((component, data) => component.AnimTrackStoredItem(data)));
  private static readonly EventSystem.IntraObjectHandler<MinionBrain> OnUnstableGroundImpactDelegate = new EventSystem.IntraObjectHandler<MinionBrain>((Action<MinionBrain, object>) ((component, data) => component.OnUnstableGroundImpact(data)));

  public bool IsCellClear(int cell)
  {
    if (Grid.Reserved[cell])
      return false;
    GameObject gameObject = Grid.Objects[cell, 0];
    return (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) gameObject) ? 0 : (!gameObject.GetComponent<Navigator>().IsMoving() ? 1 : 0)) == 0;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Navigator.SetAbilities((PathFinderAbilities) new MinionPathFinderAbilities(this.Navigator));
    this.Subscribe<MinionBrain>(-1697596308, MinionBrain.AnimTrackStoredItemDelegate);
    this.Subscribe<MinionBrain>(-975551167, MinionBrain.OnUnstableGroundImpactDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (GameObject go in this.GetComponent<Storage>().items)
      this.AddAnimTracker(go);
    Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
  }

  private void AnimTrackStoredItem(object data)
  {
    Storage component = this.GetComponent<Storage>();
    GameObject go = (GameObject) data;
    this.RemoveTracker(go);
    if (!component.items.Contains(go))
      return;
    this.AddAnimTracker(go);
  }

  private void AddAnimTracker(GameObject go)
  {
    KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.AnimFiles == null || component.AnimFiles.Length == 0 || !((UnityEngine.Object) component.AnimFiles[0] != (UnityEngine.Object) null) || !component.GetComponent<Pickupable>().trackOnPickup)
      return;
    KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.useTargetPoint = false;
    kbatchedAnimTracker.fadeOut = false;
    kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
    kbatchedAnimTracker.forceAlwaysVisible = true;
  }

  private void RemoveTracker(GameObject go)
  {
    KBatchedAnimTracker component = go.GetComponent<KBatchedAnimTracker>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }

  public override void UpdateBrain()
  {
    base.UpdateBrain();
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      return;
    if (!Game.Instance.savedInfo.discoveredSurface && World.Instance.zoneRenderData.GetSubWorldZoneType(Grid.PosToCell(this.gameObject)) == SubWorld.ZoneType.Space)
    {
      Game.Instance.savedInfo.discoveredSurface = true;
      DiscoveredSpaceMessage discoveredSpaceMessage = new DiscoveredSpaceMessage(this.gameObject.transform.GetPosition());
      Messenger.Instance.QueueMessage((Message) discoveredSpaceMessage);
      Game.Instance.Trigger(-818188514, (object) this.gameObject);
    }
    if (Game.Instance.savedInfo.discoveredOilField || World.Instance.zoneRenderData.GetSubWorldZoneType(Grid.PosToCell(this.gameObject)) != SubWorld.ZoneType.OilField)
      return;
    Game.Instance.savedInfo.discoveredOilField = true;
  }

  private void RegisterReactEmotePair(string reactable_id, Emote emote, float max_trigger_time)
  {
    if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    ReactionMonitor.Instance smi = this.gameObject.GetSMI<ReactionMonitor.Instance>();
    if (smi == null)
      return;
    EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) this.gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteIdle, emote);
    SelfEmoteReactable reactable = new SelfEmoteReactable(this.gameObject, (HashedString) reactable_id, Db.Get().ChoreTypes.Cough, max_trigger_time);
    emoteChore.PairReactable(reactable);
    reactable.SetEmote(emote);
    reactable.PairEmote(emoteChore);
    smi.AddOneshotReactable(reactable);
  }

  private void OnResearchComplete(object data)
  {
    if ((double) Time.time - (double) this.lastResearchCompleteEmoteTime <= 1.0)
      return;
    this.RegisterReactEmotePair("ResearchComplete", Db.Get().Emotes.Minion.ResearchComplete, 3f);
    this.lastResearchCompleteEmoteTime = Time.time;
  }

  public Notification CreateCollapseNotification()
  {
    MinionIdentity component = this.GetComponent<MinionIdentity>();
    return new Notification((string) MISC.NOTIFICATIONS.TILECOLLAPSE.NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.TILECOLLAPSE.TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + component.GetProperName()));
  }

  public void RemoveCollapseNotification(Notification notification)
  {
    Vector3 position = notification.clickFocus.GetPosition() with
    {
      z = -40f
    };
    WorldContainer myWorld = notification.clickFocus.gameObject.GetMyWorld();
    if ((UnityEngine.Object) myWorld != (UnityEngine.Object) null && myWorld.IsDiscovered)
      GameUtil.FocusCameraOnWorld(myWorld.id, position);
    this.gameObject.AddOrGet<Notifier>().Remove(notification);
  }

  private void OnUnstableGroundImpact(object data)
  {
    GameObject telepad = GameUtil.GetTelepad(this.gameObject.GetMyWorld().id);
    Navigator component = this.GetComponent<Navigator>();
    Assignable assignable = this.GetComponent<MinionIdentity>().GetSoleOwner().GetAssignable(Db.Get().AssignableSlots.Bed);
    int num = !((UnityEngine.Object) assignable != (UnityEngine.Object) null) ? 0 : (component.CanReach(Grid.PosToCell(assignable.transform.GetPosition())) ? 1 : 0);
    bool flag = (UnityEngine.Object) telepad != (UnityEngine.Object) null && component.CanReach(Grid.PosToCell(telepad.transform.GetPosition()));
    if (num != 0 || flag)
      return;
    this.RegisterReactEmotePair("UnstableGroundShock", Db.Get().Emotes.Minion.Shock, 1f);
    Notification notification = this.CreateCollapseNotification();
    notification.customClickCallback = (Notification.ClickCallback) (o => this.RemoveCollapseNotification(notification));
    this.gameObject.AddOrGet<Notifier>().Add(notification);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Game.Instance.Unsubscribe(-107300940, new Action<object>(this.OnResearchComplete));
  }
}
