// Decompiled with JetBrains decompiler
// Type: RecentThingConversation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RecentThingConversation : ConversationType
{
  public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>()
  {
    {
      Conversation.ModeType.Query,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Agreement,
        Conversation.ModeType.Disagreement,
        Conversation.ModeType.Musing
      }
    },
    {
      Conversation.ModeType.Statement,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Agreement,
        Conversation.ModeType.Disagreement,
        Conversation.ModeType.Query,
        Conversation.ModeType.Segue
      }
    },
    {
      Conversation.ModeType.Agreement,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Satisfaction
      }
    },
    {
      Conversation.ModeType.Disagreement,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Dissatisfaction
      }
    },
    {
      Conversation.ModeType.Musing,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Query,
        Conversation.ModeType.Statement,
        Conversation.ModeType.Segue
      }
    },
    {
      Conversation.ModeType.Satisfaction,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Segue,
        Conversation.ModeType.End
      }
    },
    {
      Conversation.ModeType.Nominal,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Segue,
        Conversation.ModeType.End
      }
    },
    {
      Conversation.ModeType.Dissatisfaction,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Segue,
        Conversation.ModeType.End
      }
    }
  };
  private static readonly List<Conversation.ModeType> INITIAL_MODES = new List<Conversation.ModeType>()
  {
    Conversation.ModeType.Query,
    Conversation.ModeType.Statement,
    Conversation.ModeType.Musing
  };

  public RecentThingConversation() => this.id = nameof (RecentThingConversation);

  public override void NewTarget(MinionIdentity speaker)
  {
    this.target = speaker.GetSMI<ConversationMonitor.Instance>().GetATopic();
  }

  public override Conversation.Topic GetNextTopic(
    MinionIdentity speaker,
    Conversation.Topic lastTopic)
  {
    if (string.IsNullOrEmpty(this.target))
      return (Conversation.Topic) null;
    List<Conversation.ModeType> modeTypeList = lastTopic == null ? RecentThingConversation.INITIAL_MODES : RecentThingConversation.transitions[lastTopic.mode];
    Conversation.ModeType mode = modeTypeList[Random.Range(0, modeTypeList.Count)];
    if (mode == Conversation.ModeType.Segue)
    {
      this.NewTarget(speaker);
      mode = RecentThingConversation.INITIAL_MODES[Random.Range(0, RecentThingConversation.INITIAL_MODES.Count)];
    }
    return new Conversation.Topic(this.target, mode);
  }

  public override Sprite GetSprite(string topic)
  {
    return Def.GetUISprite((object) topic, centered: true)?.first;
  }
}
