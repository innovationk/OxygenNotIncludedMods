// Decompiled with JetBrains decompiler
// Type: CurrentJobConversation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CurrentJobConversation : ConversationType
{
  public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>()
  {
    {
      Conversation.ModeType.Query,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Statement
      }
    },
    {
      Conversation.ModeType.Satisfaction,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Agreement
      }
    },
    {
      Conversation.ModeType.Nominal,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Musing
      }
    },
    {
      Conversation.ModeType.Dissatisfaction,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Disagreement
      }
    },
    {
      Conversation.ModeType.Stressing,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Disagreement
      }
    },
    {
      Conversation.ModeType.Agreement,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Query,
        Conversation.ModeType.End
      }
    },
    {
      Conversation.ModeType.Disagreement,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Query,
        Conversation.ModeType.End
      }
    },
    {
      Conversation.ModeType.Musing,
      new List<Conversation.ModeType>()
      {
        Conversation.ModeType.Query,
        Conversation.ModeType.End
      }
    }
  };

  public CurrentJobConversation() => this.id = nameof (CurrentJobConversation);

  public override void NewTarget(MinionIdentity speaker) => this.target = "hows_role";

  public override Conversation.Topic GetNextTopic(
    MinionIdentity speaker,
    Conversation.Topic lastTopic)
  {
    if (lastTopic == null)
      return new Conversation.Topic(this.target, Conversation.ModeType.Query);
    List<Conversation.ModeType> transition = CurrentJobConversation.transitions[lastTopic.mode];
    Conversation.ModeType mode = transition[Random.Range(0, transition.Count)];
    if (mode != Conversation.ModeType.Statement)
      return new Conversation.Topic(this.target, mode);
    this.target = this.GetRoleForSpeaker(speaker);
    return new Conversation.Topic(this.target, this.GetModeForRole(speaker, this.target));
  }

  public override Sprite GetSprite(string topic)
  {
    if (topic == "hows_role")
      return Assets.GetSprite((HashedString) "crew_state_role");
    return Db.Get().Skills.TryGet(topic) != null ? Assets.GetSprite((HashedString) Db.Get().Skills.Get(topic).hat) : (Sprite) null;
  }

  private unsafe Conversation.ModeType GetModeForRole(MinionIdentity speaker, string roleId)
  {
    if (!speaker.TryGetComponent<MinionResume>(out MinionResume _))
      return Conversation.ModeType.Nominal;
    AttributeInstance attributeInstance1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) speaker);
    if (attributeInstance1 == null)
      return Conversation.ModeType.Nominal;
    AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) speaker);
    if (attributeInstance2 == null)
      return Conversation.ModeType.Nominal;
    float totalValue = attributeInstance2.GetTotalValue();
    if ((double) totalValue <= 0.0)
      return Conversation.ModeType.Nominal;
    float* numPtr = stackalloc float[3]
    {
      0.5f,
      0.25f,
      0.25f
    };
    float num1 = attributeInstance1.GetTotalValue() / totalValue;
    for (int index = 0; index != 3; ++index)
    {
      float num2 = numPtr[index];
      num1 -= num2;
      if ((double) num1 < 0.0)
      {
        switch (index)
        {
          case 0:
            return Conversation.ModeType.Stressing;
          case 1:
            return Conversation.ModeType.Dissatisfaction;
          case 2:
            return Conversation.ModeType.Nominal;
          default:
            continue;
        }
      }
    }
    return Conversation.ModeType.Satisfaction;
  }

  private string GetRoleForSpeaker(MinionIdentity speaker)
  {
    return speaker.GetComponent<MinionResume>().CurrentRole;
  }
}
