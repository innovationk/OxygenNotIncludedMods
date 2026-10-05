// Decompiled with JetBrains decompiler
// Type: GridRestrictionSerializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class GridRestrictionSerializer : KMonoBehaviour, ISaveLoadable
{
  public static GridRestrictionSerializer Instance;
  private List<KeyValuePair<Tag, int>> tagToId = new List<KeyValuePair<Tag, int>>()
  {
    new KeyValuePair<Tag, int>(GameTags.Minions.Models.Standard, -1),
    new KeyValuePair<Tag, int>(GameTags.Minions.Models.Bionic, -2),
    new KeyValuePair<Tag, int>(GameTags.Robot, -3),
    new KeyValuePair<Tag, int>(GameTags.Robots.Models.FetchDrone, -4),
    new KeyValuePair<Tag, int>(GameTags.Robots.Models.ScoutRover, -5),
    new KeyValuePair<Tag, int>(GameTags.Robots.Models.MorbRover, -6)
  };
  private Tag[] robotTypeTags = new Tag[3]
  {
    GameTags.Robots.Models.FetchDrone,
    GameTags.Robots.Models.ScoutRover,
    GameTags.Robots.Models.MorbRover
  };

  public static void DestroyInstance()
  {
    GridRestrictionSerializer.Instance = (GridRestrictionSerializer) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GridRestrictionSerializer.Instance = this;
  }

  public int GetTagId(Tag gameTag)
  {
    foreach (KeyValuePair<Tag, int> keyValuePair in this.tagToId)
    {
      if (keyValuePair.Key == gameTag)
        return keyValuePair.Value;
    }
    DebugUtil.DevAssert(false, $"Gametag {gameTag.Name} has not been added to the valid list of GridRestrictionTagId's before requesting the ID");
    return 0;
  }

  public Tag[] ValidRobotTypes => this.robotTypeTags;
}
