// Decompiled with JetBrains decompiler
// Type: ConversationType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ConversationType
{
  public string id;
  public string target;

  public virtual void NewTarget(MinionIdentity speaker)
  {
  }

  public virtual Conversation.Topic GetNextTopic(
    MinionIdentity speaker,
    Conversation.Topic lastTopic)
  {
    return (Conversation.Topic) null;
  }

  public virtual Sprite GetSprite(string topic) => (Sprite) null;
}
