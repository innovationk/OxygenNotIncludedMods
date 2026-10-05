// Decompiled with JetBrains decompiler
// Type: Database.BuildingFacades
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class BuildingFacades : ResourceSet<BuildingFacadeResource>
{
  public BuildingFacades(ResourceSet parent)
    : base(nameof (BuildingFacades), parent)
  {
    this.Initialize();
    foreach (BuildingFacadeInfo buildingFacade in Blueprints.Get().all.buildingFacades)
      this.Add(buildingFacade.id, (LocString) buildingFacade.name, (LocString) buildingFacade.desc, buildingFacade.rarity, buildingFacade.prefabId, buildingFacade.animFile, buildingFacade.workables, buildingFacade.GetRequiredDlcIds(), buildingFacade.GetForbiddenDlcIds(), buildingFacade.data);
  }

  public void Add(
    string id,
    LocString Name,
    LocString Desc,
    PermitRarity rarity,
    string prefabId,
    string animFile,
    Dictionary<string, string> workables = null,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    Dictionary<string, string> data = null)
  {
    this.resources.Add(new BuildingFacadeResource(id, (string) Name, (string) Desc, rarity, prefabId, animFile, workables, requiredDlcIds, forbiddenDlcIds, data));
  }

  [Obsolete("Use overload with data parameter")]
  public void Add(
    string id,
    LocString Name,
    LocString Desc,
    PermitRarity rarity,
    string prefabId,
    string animFile,
    Dictionary<string, string> workables = null,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
  {
    this.Add(id, Name, Desc, rarity, prefabId, animFile, workables, requiredDlcIds, forbiddenDlcIds, (Dictionary<string, string>) null);
  }

  public void PostProcess()
  {
    foreach (BuildingFacadeResource resource in this.resources)
      resource.Init();
  }
}
