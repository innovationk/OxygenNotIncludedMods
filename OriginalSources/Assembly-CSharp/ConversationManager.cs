// Decompiled with JetBrains decompiler
// Type: ConversationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ConversationManager")]
public class ConversationManager : KMonoBehaviour, ISim200ms
{
  private List<Conversation> conversations;
  private Dictionary<MinionIdentity, float> lastConvoTimeByMinion;
  private readonly Dictionary<MinionIdentity, Conversation> minionConversations = new Dictionary<MinionIdentity, Conversation>();
  private List<System.Type> convoTypes = new List<System.Type>()
  {
    typeof (RecentThingConversation),
    typeof (AmountStateConversation),
    typeof (CurrentJobConversation)
  };
  private static readonly Tag[] invalidConvoTags = new Tag[5]
  {
    GameTags.Asleep,
    GameTags.BionicBedTime,
    GameTags.HoldingBreath,
    GameTags.Dead,
    GameTags.SuppressConversation
  };

  protected override void OnPrefabInit()
  {
    this.conversations = new List<Conversation>();
    this.lastConvoTimeByMinion = new Dictionary<MinionIdentity, float>();
    this.simRenderLoadBalance = true;
  }

  public void Sim200ms(float dt)
  {
    for (int index1 = this.conversations.Count - 1; index1 >= 0; --index1)
    {
      Conversation conversation = this.conversations[index1];
      for (int index2 = conversation.minions.Count - 1; index2 >= 0; --index2)
      {
        MinionIdentity minion = conversation.minions[index2];
        if (!this.ValidMinionTags(minion) || !this.MinionCloseEnoughToConvo(minion, conversation))
        {
          conversation.minions.RemoveAt(index2);
          if ((UnityEngine.Object) conversation.lastTalked == (UnityEngine.Object) minion)
            conversation.lastTalked = (MinionIdentity) null;
        }
        else
          this.minionConversations[minion] = conversation;
      }
      if (conversation.minions.Count <= 1)
        this.conversations.RemoveAt(index1);
      else if (!((UnityEngine.Object) conversation.lastTalked != (UnityEngine.Object) null) || !conversation.lastTalked.GetComponent<KPrefabID>().HasTag(GameTags.DoNotInterruptMe))
      {
        int num1 = (UnityEngine.Object) conversation.minions.Find((Predicate<MinionIdentity>) (match => match.HasTag(GameTags.CommunalDining))) != (UnityEngine.Object) null ? 1 : 0;
        bool flag = true;
        if (num1 == 0 && conversation.numUtterances >= TuningData<ConversationManager.Tuning>.Get().maxUtterances)
        {
          flag = false;
        }
        else
        {
          bool isOpeningLine = conversation.numUtterances == 0;
          int num2 = (UnityEngine.Object) conversation.minions.Find((Predicate<MinionIdentity>) (match => !match.HasTag(GameTags.Partying))) == (UnityEngine.Object) null ? 1 : 0;
          float num3 = isOpeningLine ? TuningData<ConversationManager.Tuning>.Get().delayBeforeStart : TuningData<ConversationManager.Tuning>.Get().delayBetweenUtterances;
          if (num2 != 0)
            num3 = 0.0f;
          float num4 = isOpeningLine ? 0.0f : TuningData<ConversationManager.Tuning>.Get().speakTime;
          if (num2 != 0)
            num4 /= 4f;
          float num5 = num4 + num3;
          if ((double) GameClock.Instance.GetTime() > (double) conversation.lastTalkedTime + (double) num5)
            flag = this.TryContinueConversation(conversation, isOpeningLine);
        }
        if (!flag)
          this.conversations.RemoveAt(index1);
      }
    }
    foreach (MinionIdentity minionIdentity1 in Components.LiveMinionIdentities.Items)
    {
      if (this.ValidMinionTags(minionIdentity1) && !this.minionConversations.ContainsKey(minionIdentity1) && !this.MinionOnCooldown(minionIdentity1))
      {
        foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
        {
          if (!((UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) minionIdentity1) && this.ValidMinionTags(minionIdentity2))
          {
            Conversation conversation;
            if (this.minionConversations.TryGetValue(minionIdentity2, out conversation))
            {
              if (conversation.minions.Count < TuningData<ConversationManager.Tuning>.Get().maxDupesPerConvo && (double) (this.GetCentroid(conversation) - minionIdentity1.transform.GetPosition()).magnitude < (double) TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5)
              {
                conversation.minions.Add(minionIdentity1);
                this.minionConversations[minionIdentity1] = conversation;
                break;
              }
            }
            else if (!this.MinionOnCooldown(minionIdentity2) && (double) (minionIdentity2.transform.GetPosition() - minionIdentity1.transform.GetPosition()).magnitude < (double) TuningData<ConversationManager.Tuning>.Get().maxDistance)
            {
              conversation = new Conversation();
              conversation.minions.Add(minionIdentity1);
              conversation.minions.Add(minionIdentity2);
              System.Type convoType = this.convoTypes[UnityEngine.Random.Range(0, this.convoTypes.Count)];
              conversation.conversationType = (ConversationType) Activator.CreateInstance(convoType);
              conversation.lastTalkedTime = GameClock.Instance.GetTime();
              this.conversations.Add(conversation);
              this.minionConversations[minionIdentity1] = conversation;
              this.minionConversations[minionIdentity2] = conversation;
              break;
            }
          }
        }
      }
    }
    this.minionConversations.Clear();
  }

  private bool TryContinueConversation(Conversation conversation, bool isOpeningLine)
  {
    ListPool<int, ConversationManager>.PooledList list = ListPool<int, ConversationManager>.Allocate();
    int num = -1;
    list.Capacity = Math.Max(list.Capacity, conversation.minions.Count);
    for (int index = 0; index != conversation.minions.Count; ++index)
    {
      if ((UnityEngine.Object) conversation.minions[index] == (UnityEngine.Object) conversation.lastTalked)
        num = index;
      else
        list.Add(index);
    }
    list.Shuffle<int>();
    if (num != -1)
      list.Add(num);
    if (isOpeningLine)
    {
      MinionIdentity minion = conversation.minions[list[0]];
      conversation.conversationType.NewTarget(minion);
    }
    bool flag = false;
    foreach (int index in (List<int>) list)
    {
      MinionIdentity minion = conversation.minions[index];
      if (this.DoTalking(conversation, minion))
      {
        flag = true;
        break;
      }
    }
    list.Recycle();
    return flag;
  }

  private bool DoTalking(Conversation conversation, MinionIdentity new_speaker)
  {
    DebugUtil.Assert(conversation != null, "conversation was null");
    DebugUtil.Assert((UnityEngine.Object) new_speaker != (UnityEngine.Object) null, "new_speaker was null");
    DebugUtil.Assert(conversation.conversationType != null, "conversation.conversationType was null");
    Conversation.Topic nextTopic = conversation.conversationType.GetNextTopic(new_speaker, conversation.lastTopic);
    if (nextTopic == null || nextTopic.mode == Conversation.ModeType.End)
      return false;
    Thought thoughtForTopic = this.GetThoughtForTopic(conversation, nextTopic);
    if (thoughtForTopic == null)
      return false;
    ThoughtGraph.Instance smi = new_speaker.GetSMI<ThoughtGraph.Instance>();
    if (smi == null)
      return false;
    if ((UnityEngine.Object) conversation.lastTalked != (UnityEngine.Object) null)
      conversation.lastTalked.Trigger(25860745, (object) conversation.lastTalked.gameObject);
    smi.AddThought(thoughtForTopic);
    conversation.lastTopic = nextTopic;
    conversation.lastTalked = new_speaker;
    conversation.lastTalkedTime = GameClock.Instance.GetTime();
    DebugUtil.Assert(this.lastConvoTimeByMinion != null, "lastConvoTimeByMinion was null");
    this.lastConvoTimeByMinion[conversation.lastTalked] = GameClock.Instance.GetTime();
    Effects component = conversation.lastTalked.GetComponent<Effects>();
    DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, "effects was null");
    component.Add("GoodConversation", true);
    Conversation.Mode mode = Conversation.Topic.Modes[(int) nextTopic.mode];
    DebugUtil.Assert(mode != null, "mode was null");
    ConversationManager.StartedTalkingEvent data = new ConversationManager.StartedTalkingEvent()
    {
      talker = new_speaker.gameObject,
      anim = mode.anim
    };
    foreach (MinionIdentity minion in conversation.minions)
    {
      if (!(bool) (UnityEngine.Object) minion)
        DebugUtil.DevAssert(false, "minion in conversation.minions was null");
      else
        minion.Trigger(-594200555, (object) data);
    }
    ++conversation.numUtterances;
    return true;
  }

  public bool TryGetConversation(MinionIdentity minion, out Conversation conversation)
  {
    return this.minionConversations.TryGetValue(minion, out conversation);
  }

  private Vector3 GetCentroid(Conversation conversation)
  {
    Vector3 zero = Vector3.zero;
    foreach (MinionIdentity minion in conversation.minions)
    {
      if (!((UnityEngine.Object) minion == (UnityEngine.Object) null))
        zero += minion.transform.GetPosition();
    }
    return zero / (float) conversation.minions.Count;
  }

  private Thought GetThoughtForTopic(Conversation conversation, Conversation.Topic topic)
  {
    if (string.IsNullOrEmpty(topic.topic))
    {
      DebugUtil.DevAssert(false, "topic.topic was null");
      return (Thought) null;
    }
    Sprite sprite = conversation.conversationType.GetSprite(topic.topic);
    if (!((UnityEngine.Object) sprite != (UnityEngine.Object) null))
      return (Thought) null;
    Conversation.Mode mode = Conversation.Topic.Modes[(int) topic.mode];
    return new Thought("Topic_" + topic.topic, (ResourceSet) null, sprite, mode.icon, mode.voice, "bubble_chatter", mode.mouth, DUPLICANTS.THOUGHTS.CONVERSATION.TOOLTIP, true, TuningData<ConversationManager.Tuning>.Get().speakTime);
  }

  private bool ValidMinionTags(MinionIdentity minion)
  {
    return !((UnityEngine.Object) minion == (UnityEngine.Object) null) && !minion.GetComponent<KPrefabID>().HasAnyTags(ConversationManager.invalidConvoTags);
  }

  private bool MinionCloseEnoughToConvo(MinionIdentity minion, Conversation conversation)
  {
    return (double) (this.GetCentroid(conversation) - minion.transform.GetPosition()).magnitude < (double) TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5;
  }

  private bool MinionOnCooldown(MinionIdentity minion)
  {
    float num1;
    if (minion.GetComponent<KPrefabID>().HasTag(GameTags.AlwaysConverse) || !this.lastConvoTimeByMinion.TryGetValue(minion, out num1))
      return false;
    float num2 = GameClock.Instance.GetTime() - TuningData<ConversationManager.Tuning>.Get().minionCooldownTime;
    return (double) num1 > (double) num2;
  }

  public class Tuning : TuningData<ConversationManager.Tuning>
  {
    public float cyclesBeforeFirstConversation;
    public float maxDistance;
    public int maxDupesPerConvo;
    public float minionCooldownTime;
    public float speakTime;
    public float delayBetweenUtterances;
    public float delayBeforeStart;
    public int maxUtterances;
  }

  public class StartedTalkingEvent
  {
    public GameObject talker;
    public string anim;
  }
}
