// Decompiled with JetBrains decompiler
// Type: Klei.AI.StoryTraitStateMachine`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public abstract class StoryTraitStateMachine<TStateMachine, TInstance, TDef> : 
  GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef>
  where TStateMachine : StoryTraitStateMachine<TStateMachine, TInstance, TDef>
  where TInstance : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitInstance
  where TDef : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitDef
{
  public class TraitDef : StateMachine.BaseDef
  {
    public string InitalLoreId;
    public string CompleteLoreId;
    public Story Story;
    public StoryCompleteData CompletionData;
    public StoryManager.PopupInfo EventIntroInfo = new StoryManager.PopupInfo()
    {
      PopupType = EventInfoDataHelper.PopupType.NONE
    };
    public StoryManager.PopupInfo EventCompleteInfo = new StoryManager.PopupInfo()
    {
      PopupType = EventInfoDataHelper.PopupType.NONE
    };
  }

  public class TraitInstance : 
    GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef>.GameInstance
  {
    protected int buildingActivatedHandle = -1;
    private int onObjectSelectedHandle = -1;
    protected Notifier notifier;
    protected KSelectable selectable;

    public TraitInstance(StateMachineController master)
      : base(master)
    {
      StoryManager.Instance.ForceCreateStory(this.def.Story, this.gameObject.GetMyWorldId());
      this.buildingActivatedHandle = master.Subscribe(-1909216579, new System.Action<object>(this.OnBuildingActivated));
    }

    public TraitInstance(StateMachineController master, TDef def)
      : base(master, def)
    {
      StoryManager.Instance.ForceCreateStory(def.Story, this.gameObject.GetMyWorldId());
      this.buildingActivatedHandle = master.Subscribe(-1909216579, new System.Action<object>(this.OnBuildingActivated));
    }

    public override void StartSM()
    {
      this.selectable = this.GetComponent<KSelectable>();
      this.notifier = this.gameObject.AddOrGet<Notifier>();
      base.StartSM();
      this.onObjectSelectedHandle = this.Subscribe(-1503271301, new System.Action<object>(this.OnObjectSelect));
      if (this.buildingActivatedHandle == -1)
        this.buildingActivatedHandle = this.master.Subscribe(-1909216579, new System.Action<object>(this.OnBuildingActivated));
      this.TriggerStoryEvent(StoryInstance.State.DISCOVERED);
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      this.Unsubscribe(ref this.onObjectSelectedHandle);
      this.Unsubscribe(ref this.buildingActivatedHandle);
    }

    public void TriggerStoryEvent(StoryInstance.State storyEvent)
    {
      switch (storyEvent)
      {
        case StoryInstance.State.RETROFITTED:
          break;
        case StoryInstance.State.NOT_STARTED:
          break;
        case StoryInstance.State.DISCOVERED:
          StoryManager.Instance.DiscoverStoryEvent(this.def.Story);
          break;
        case StoryInstance.State.IN_PROGRESS:
          StoryManager.Instance.BeginStoryEvent(this.def.Story);
          break;
        case StoryInstance.State.COMPLETE:
          StoryManager.Instance.CompleteStoryEvent(this.def.Story, Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this.master), this.def.CompletionData.KeepSakeSpawnOffset), Grid.SceneLayer.Ore));
          break;
        default:
          throw new NotImplementedException(storyEvent.ToString());
      }
    }

    protected virtual void OnBuildingActivated(object activated)
    {
      if (!((Boxed<bool>) activated).value)
        return;
      this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
    }

    protected virtual void OnObjectSelect(object clicked)
    {
      if (!((Boxed<bool>) clicked).value)
        return;
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(this.def.Story.HashId);
      if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE)
      {
        this.OnNotificationClicked();
      }
      else
      {
        if (StoryManager.Instance.HasDisplayedPopup(this.def.Story, EventInfoDataHelper.PopupType.BEGIN))
          return;
        this.DisplayPopup(this.def.EventIntroInfo);
      }
    }

    public virtual void CompleteEvent()
    {
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(this.def.Story.HashId);
      if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
        return;
      this.DisplayPopup(this.def.EventCompleteInfo);
    }

    public virtual void OnCompleteStorySequence()
    {
      this.TriggerStoryEvent(StoryInstance.State.COMPLETE);
    }

    protected void DisplayPopup(StoryManager.PopupInfo info)
    {
      if (info.PopupType == EventInfoDataHelper.PopupType.NONE)
        return;
      StoryInstance storyInstance = StoryManager.Instance.DisplayPopup(this.def.Story, info, new System.Action(this.OnPopupClosed), new Notification.ClickCallback(this.OnNotificationClicked));
      if (storyInstance == null || info.DisplayImmediate)
        return;
      this.selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, (object) this.smi);
      this.notifier.Add(storyInstance.Notification);
    }

    public void OnNotificationClicked(object data = null)
    {
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(this.def.Story.HashId);
      if (storyInstance == null)
        return;
      this.selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
      this.notifier.Remove(storyInstance.Notification);
      if (storyInstance.PendingType == EventInfoDataHelper.PopupType.COMPLETE)
        this.ShowEventCompleteUI();
      else if (storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
        this.ShowEventNormalUI();
      else
        this.ShowEventBeginUI();
    }

    public virtual void OnPopupClosed()
    {
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(this.def.Story.HashId);
      if (storyInstance == null)
        return;
      if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
        Game.Instance.unlocks.Unlock(this.def.CompleteLoreId);
      else
        Game.Instance.unlocks.Unlock(this.def.InitalLoreId);
    }

    protected virtual void ShowEventBeginUI()
    {
    }

    protected virtual void ShowEventNormalUI()
    {
    }

    protected virtual void ShowEventCompleteUI()
    {
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(this.def.Story.HashId);
      if (storyInstance == null)
        return;
      Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this.master), this.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
      StoryManager.Instance.CompleteStoryEvent(this.def.Story, (MonoBehaviour) this.master, new FocusTargetSequence.Data()
      {
        WorldId = this.master.GetMyWorldId(),
        OrthographicSize = 6f,
        TargetSize = 6f,
        Target = posCcc,
        PopupData = storyInstance.EventInfo,
        CompleteCB = new System.Action(this.OnCompleteStorySequence),
        CanCompleteCB = (Func<bool>) null
      });
    }
  }
}
