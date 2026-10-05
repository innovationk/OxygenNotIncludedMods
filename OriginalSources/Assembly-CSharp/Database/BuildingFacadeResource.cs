// Decompiled with JetBrains decompiler
// Type: Database.BuildingFacadeResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class BuildingFacadeResource : PermitResource
{
  public string PrefabID;
  public string AnimFile;
  public Dictionary<string, string> InteractFile;
  public Dictionary<string, string> Data;

  [Obsolete("Please use constructor with dlcIds parameter")]
  public BuildingFacadeResource(
    string Id,
    string Name,
    string Description,
    PermitRarity Rarity,
    string PrefabID,
    string AnimFile,
    Dictionary<string, string> workables = null)
    : this(Id, Name, Description, Rarity, PrefabID, AnimFile, workables, (string[]) null, (string[]) null)
  {
  }

  [Obsolete("Please use constructor with dlcIds parameter")]
  public BuildingFacadeResource(
    string Id,
    string Name,
    string Description,
    PermitRarity Rarity,
    string PrefabID,
    string AnimFile,
    string[] dlcIds,
    Dictionary<string, string> workables = null)
    : this(Id, Name, Description, Rarity, PrefabID, AnimFile, workables, (string[]) null, (string[]) null)
  {
  }

  [Obsolete("Please use the one with data parameter")]
  public BuildingFacadeResource(
    string Id,
    string Name,
    string Description,
    PermitRarity Rarity,
    string PrefabID,
    string AnimFile,
    Dictionary<string, string> workables = null,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
    : this(Id, Name, Description, Rarity, PrefabID, AnimFile, workables, requiredDlcIds, forbiddenDlcIds, (Dictionary<string, string>) null)
  {
  }

  public BuildingFacadeResource(
    string Id,
    string Name,
    string Description,
    PermitRarity Rarity,
    string PrefabID,
    string AnimFile,
    Dictionary<string, string> workables = null,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    Dictionary<string, string> data = null)
    : base(Id, Name, Description, PermitCategory.Building, Rarity, requiredDlcIds, forbiddenDlcIds)
  {
    this.Id = Id;
    this.PrefabID = PrefabID;
    this.AnimFile = AnimFile;
    this.InteractFile = workables;
    this.Data = data;
  }

  public void Init()
  {
    GameObject prefab = Assets.TryGetPrefab((Tag) this.PrefabID);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return;
    prefab.AddOrGet<BuildingFacade>();
    BuildingDef def = prefab.GetComponent<Building>().Def;
    if (!((UnityEngine.Object) def != (UnityEngine.Object) null))
      return;
    def.AddFacade(this.Id);
    KAnimFileData data1 = def.AnimFiles[0].GetData();
    KAnimFileData data2 = Assets.GetAnim((HashedString) this.AnimFile).GetData();
    for (int index = 0; index < data1.animCount; ++index)
    {
      KAnim.Anim anim1 = data1.GetAnim(index);
      KAnim.Anim anim2 = data2.GetAnim(anim1.name);
      if (anim2 != null)
      {
        bool flag = GameAudioSheets.Get().events.ContainsKey(anim1.id);
        if (!GameAudioSheets.Get().events.ContainsKey(anim2.id) & flag)
          GameAudioSheets.Get().skinToBaseAnim[anim2.id] = anim1.id;
      }
    }
  }

  public override PermitPresentationInfo GetPermitPresentationInfo()
  {
    PermitPresentationInfo presentationInfo = new PermitPresentationInfo();
    presentationInfo.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim((HashedString) this.AnimFile));
    presentationInfo.SetFacadeForPrefabID(this.PrefabID);
    return presentationInfo;
  }
}
