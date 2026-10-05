// Decompiled with JetBrains decompiler
// Type: Database.PermitResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public abstract class PermitResource : Resource, IHasDlcRestrictions
{
  public string Description;
  public PermitCategory Category;
  public PermitRarity Rarity;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;

  public PermitResource(
    string id,
    string Name,
    string Desc,
    PermitCategory permitCategory,
    PermitRarity rarity,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
    : base(id, Name)
  {
    DebugUtil.DevAssert(Name != null, $"Name must be provided for permit with id \"{id}\" of type {this.GetType().Name}");
    DebugUtil.DevAssert(Desc != null, $"Description must be provided for permit with id \"{id}\" of type {this.GetType().Name}");
    this.Description = Desc;
    this.Category = permitCategory;
    this.Rarity = rarity;
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
  }

  public abstract PermitPresentationInfo GetPermitPresentationInfo();

  public bool IsOwnableOnServer()
  {
    return this.Rarity != PermitRarity.Universal && this.Rarity != PermitRarity.UniversalLocked;
  }

  public bool IsUnlocked()
  {
    return this.Rarity == PermitRarity.Universal || PermitItems.IsPermitUnlocked(this);
  }

  public string GetDlcIdFrom() => DlcManager.GetMostSignificantDlc((IHasDlcRestrictions) this);

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;
}
