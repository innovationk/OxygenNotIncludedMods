// Decompiled with JetBrains decompiler
// Type: QuestManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestManager : KMonoBehaviour
{
  private static QuestManager instance;
  [Serialize]
  private Dictionary<int, Dictionary<HashedString, QuestInstance>> ownerToQuests = new Dictionary<int, Dictionary<HashedString, QuestInstance>>();

  protected override void OnPrefabInit()
  {
    if ((Object) QuestManager.instance != (Object) null)
    {
      Object.Destroy((Object) QuestManager.instance);
    }
    else
    {
      QuestManager.instance = this;
      base.OnPrefabInit();
    }
  }

  public static void DestroyInstance() => QuestManager.instance = (QuestManager) null;

  public static QuestInstance InitializeQuest(Tag ownerId, Quest quest)
  {
    QuestInstance qInst;
    if (!QuestManager.TryGetQuest(ownerId.GetHash(), quest, out qInst))
      qInst = QuestManager.instance.ownerToQuests[ownerId.GetHash()][quest.IdHash] = new QuestInstance(quest);
    qInst.Initialize(quest);
    return qInst;
  }

  public static QuestInstance InitializeQuest(HashedString ownerId, Quest quest)
  {
    QuestInstance qInst;
    if (!QuestManager.TryGetQuest(ownerId.HashValue, quest, out qInst))
      qInst = QuestManager.instance.ownerToQuests[ownerId.HashValue][quest.IdHash] = new QuestInstance(quest);
    qInst.Initialize(quest);
    return qInst;
  }

  public static QuestInstance GetInstance(Tag ownerId, Quest quest)
  {
    QuestInstance qInst;
    QuestManager.TryGetQuest(ownerId.GetHash(), quest, out qInst);
    return qInst;
  }

  public static QuestInstance GetInstance(HashedString ownerId, Quest quest)
  {
    QuestInstance qInst;
    QuestManager.TryGetQuest(ownerId.HashValue, quest, out qInst);
    return qInst;
  }

  public static bool CheckState(HashedString ownerId, Quest quest, Quest.State state)
  {
    QuestInstance qInst;
    QuestManager.TryGetQuest(ownerId.HashValue, quest, out qInst);
    return qInst != null && qInst.CurrentState == state;
  }

  public static bool CheckState(Tag ownerId, Quest quest, Quest.State state)
  {
    QuestInstance qInst;
    QuestManager.TryGetQuest(ownerId.GetHash(), quest, out qInst);
    return qInst != null && qInst.CurrentState == state;
  }

  private static bool TryGetQuest(int ownerId, Quest quest, out QuestInstance qInst)
  {
    qInst = (QuestInstance) null;
    Dictionary<HashedString, QuestInstance> dictionary;
    if (!QuestManager.instance.ownerToQuests.TryGetValue(ownerId, out dictionary))
      dictionary = QuestManager.instance.ownerToQuests[ownerId] = new Dictionary<HashedString, QuestInstance>();
    return dictionary.TryGetValue(quest.IdHash, out qInst);
  }
}
