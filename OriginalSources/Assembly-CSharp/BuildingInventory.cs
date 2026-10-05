// Decompiled with JetBrains decompiler
// Type: BuildingInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BuildingInventory : KMonoBehaviour
{
  public static BuildingInventory Instance;
  private Dictionary<Tag, HashSet<BuildingComplete>> Buildings = new Dictionary<Tag, HashSet<BuildingComplete>>();

  public static void DestroyInstance() => BuildingInventory.Instance = (BuildingInventory) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    BuildingInventory.Instance = this;
  }

  public HashSet<BuildingComplete> GetBuildings(Tag tag) => this.Buildings[tag];

  public int BuildingCount(Tag tag)
  {
    return !this.Buildings.ContainsKey(tag) ? 0 : this.Buildings[tag].Count;
  }

  public int BuildingCountForWorld_BAD_PERF(Tag tag, int worldId)
  {
    if (!this.Buildings.ContainsKey(tag))
      return 0;
    int num = 0;
    foreach (KMonoBehaviour component in this.Buildings[tag])
    {
      if (component.GetMyWorldId() == worldId)
        ++num;
    }
    return num;
  }

  public void RegisterBuilding(BuildingComplete building)
  {
    Tag prefabTag = building.prefabid.PrefabTag;
    HashSet<BuildingComplete> buildingCompleteSet;
    if (!this.Buildings.TryGetValue(prefabTag, out buildingCompleteSet))
    {
      buildingCompleteSet = new HashSet<BuildingComplete>();
      this.Buildings[prefabTag] = buildingCompleteSet;
    }
    buildingCompleteSet.Add(building);
  }

  public void UnregisterBuilding(BuildingComplete building)
  {
    Tag prefabTag = building.prefabid.PrefabTag;
    HashSet<BuildingComplete> buildingCompleteSet;
    if (!this.Buildings.TryGetValue(prefabTag, out buildingCompleteSet))
      DebugUtil.DevLogError($"Unregistering building {prefabTag} before it was registered.");
    else
      DebugUtil.DevAssert(buildingCompleteSet.Remove(building), $"Building {prefabTag} was not found to be removed");
  }
}
