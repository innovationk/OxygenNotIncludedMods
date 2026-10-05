// Decompiled with JetBrains decompiler
// Type: LonelyMinionMailbox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LonelyMinionMailbox : KMonoBehaviour
{
  public LonelyMinionHouse.Instance House;

  public void Initialize(LonelyMinionHouse.Instance house)
  {
    this.House = house;
    SingleEntityReceptacle component = this.GetComponent<SingleEntityReceptacle>();
    component.occupyingObjectRelativePosition = this.transform.InverseTransformPoint(house.GetParcelPosition());
    component.occupyingObjectRelativePosition.z = -1f;
    StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
    storyInstance.StoryStateChanged += new Action<StoryInstance.State>(this.OnStoryStateChanged);
    this.OnStoryStateChanged(storyInstance.CurrentState);
  }

  protected override void OnSpawn()
  {
    if (!StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion))
      return;
    this.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
  }

  protected override void OnCleanUp()
  {
    StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId).StoryStateChanged -= new Action<StoryInstance.State>(this.OnStoryStateChanged);
  }

  private void OnStoryStateChanged(StoryInstance.State state)
  {
    QuestInstance quest = QuestManager.GetInstance(this.House.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
    if (state == StoryInstance.State.IN_PROGRESS)
    {
      this.Subscribe(-731304873, new Action<object>(this.OnStorageChanged));
      SingleEntityReceptacle entityReceptacle = this.gameObject.AddOrGet<SingleEntityReceptacle>();
      entityReceptacle.enabled = true;
      entityReceptacle.AddAdditionalCriteria((Func<GameObject, bool>) (candidate =>
      {
        EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(candidate.GetComponent<KPrefabID>().PrefabTag.Name);
        int valueHandle = 0;
        if (foodInfo == null)
          return false;
        return quest.DataSatisfiesCriteria(new Quest.ItemData()
        {
          CriteriaId = LonelyMinionConfig.FoodCriteriaId,
          QualifyingTag = GameTags.Edible,
          CurrentValue = (float) foodInfo.Quality
        }, ref valueHandle);
      }));
      RootMenu.Instance.Refresh();
      this.OnStorageChanged((object) entityReceptacle.Occupant);
    }
    if (state != StoryInstance.State.COMPLETE)
      return;
    this.Unsubscribe(-731304873, new Action<object>(this.OnStorageChanged));
    this.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
  }

  private void OnStorageChanged(object data)
  {
    this.House.MailboxContentChanged(data as GameObject);
  }
}
