// Decompiled with JetBrains decompiler
// Type: Database.EquippableFacades
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Database;

public class EquippableFacades : ResourceSet<EquippableFacadeResource>
{
  public EquippableFacades(ResourceSet parent)
    : base(nameof (EquippableFacades), parent)
  {
    this.Initialize();
    foreach (EquippableFacadeInfo equippableFacade in Blueprints.Get().all.equippableFacades)
      this.Add(equippableFacade.id, equippableFacade.name, equippableFacade.desc, equippableFacade.rarity, equippableFacade.defID, equippableFacade.buildOverride, equippableFacade.animFile, equippableFacade.GetRequiredDlcIds(), equippableFacade.GetForbiddenDlcIds());
  }

  [Obsolete("Please use Add(...) with required forbidden")]
  public void Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string defID,
    string buildOverride,
    string animFile)
  {
    this.Add(id, name, desc, rarity, defID, buildOverride, animFile, (string[]) null, (string[]) null);
  }

  [Obsolete("Please use Add(...) with required forbidden")]
  public void Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string defID,
    string buildOverride,
    string animFile,
    string[] dlcIds)
  {
    DlcRestrictionsUtil.TemporaryHelperObject objectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIds);
    this.resources.Add(new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, objectFromAllowList.GetRequiredDlcIds(), objectFromAllowList.GetForbiddenDlcIds()));
  }

  public void Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string defID,
    string buildOverride,
    string animFile,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
  {
    this.resources.Add(new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, requiredDlcIds, forbiddenDlcIds));
  }
}
