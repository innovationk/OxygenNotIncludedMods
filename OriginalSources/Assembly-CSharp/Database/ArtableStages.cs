// Decompiled with JetBrains decompiler
// Type: Database.ArtableStages
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class ArtableStages : ResourceSet<ArtableStage>
{
  [Obsolete("Use ArtableStages with required/forbidden")]
  public ArtableStage Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    string anim,
    int decor_value,
    bool cheer_on_complete,
    string status_id,
    string prefabId,
    string symbolname,
    string[] dlcIds)
  {
    DlcRestrictionsUtil.TemporaryHelperObject objectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIds);
    return this.Add(id, name, desc, rarity, animFile, anim, decor_value, cheer_on_complete, status_id, prefabId, symbolname, objectFromAllowList.GetRequiredDlcIds(), objectFromAllowList.GetForbiddenDlcIds());
  }

  public ArtableStage Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    string anim,
    int decor_value,
    bool cheer_on_complete,
    string status_id,
    string prefabId,
    string symbolname,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
  {
    ArtableStatusItem status_item = Db.Get().ArtableStatuses.Get(status_id);
    ArtableStage artableStage = new ArtableStage(id, name, desc, rarity, animFile, anim, decor_value, cheer_on_complete, status_item, prefabId, symbolname, requiredDlcIds, forbiddenDlcIds);
    this.resources.Add(artableStage);
    return artableStage;
  }

  public ArtableStages(ResourceSet parent)
    : base(nameof (ArtableStages), parent)
  {
    foreach (ArtableInfo artable in Blueprints.Get().all.artables)
      this.Add(artable.id, artable.name, artable.desc, artable.rarity, artable.animFile, artable.anim, artable.decor_value, artable.cheer_on_complete, artable.status_id, artable.prefabId, artable.symbolname, artable.GetRequiredDlcIds(), artable.GetForbiddenDlcIds());
  }

  public List<ArtableStage> GetPrefabStages(Tag prefab_id)
  {
    return this.resources.FindAll((Predicate<ArtableStage>) (stage => (Tag) stage.prefabId == prefab_id));
  }

  public ArtableStage DefaultPrefabStage(Tag prefab_id)
  {
    return this.GetPrefabStages(prefab_id).Find((Predicate<ArtableStage>) (stage => stage.statusItem == Db.Get().ArtableStatuses.AwaitingArting));
  }
}
