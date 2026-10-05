// Decompiled with JetBrains decompiler
// Type: MaterialNeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MaterialNeeds")]
public static class MaterialNeeds
{
  public static void UpdateNeed(Tag tag, float amount, int worldId)
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
    if ((Object) world != (Object) null)
    {
      Dictionary<Tag, float> materialNeeds = world.materialNeeds;
      float num = 0.0f;
      if (!materialNeeds.TryGetValue(tag, out num))
        materialNeeds[tag] = 0.0f;
      materialNeeds[tag] = num + amount;
    }
    else
      Debug.LogWarning((object) $"MaterialNeeds.UpdateNeed called with invalid worldId {worldId}");
  }

  public static float GetAmount(Tag tag, int worldId, bool includeRelatedWorlds)
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
    float amount = 0.0f;
    if ((Object) world != (Object) null)
    {
      if (!includeRelatedWorlds)
      {
        float num = 0.0f;
        ClusterManager.Instance.GetWorld(worldId).materialNeeds.TryGetValue(tag, out num);
        amount += num;
      }
      else
      {
        int parentWorldId = world.ParentWorldId;
        foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
        {
          if (worldContainer.ParentWorldId == parentWorldId)
          {
            float num = 0.0f;
            if (worldContainer.materialNeeds.TryGetValue(tag, out num))
              amount += num;
          }
        }
      }
      return amount;
    }
    Debug.LogWarning((object) $"MaterialNeeds.GetAmount called with invalid worldId {worldId}");
    return 0.0f;
  }
}
