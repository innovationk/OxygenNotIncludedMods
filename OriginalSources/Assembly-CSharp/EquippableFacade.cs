// Decompiled with JetBrains decompiler
// Type: EquippableFacade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
public class EquippableFacade : KMonoBehaviour
{
  [Serialize]
  private string _facadeID;
  [Serialize]
  public string BuildOverride;

  public static void AddFacadeToEquippable(Equippable equippable, string facadeID)
  {
    EquippableFacade equippableFacade = equippable.gameObject.AddOrGet<EquippableFacade>();
    equippableFacade.FacadeID = facadeID;
    equippableFacade.BuildOverride = Db.GetEquippableFacades().Get(facadeID).BuildOverride;
    equippableFacade.ApplyAnimOverride();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OverrideName();
    this.ApplyAnimOverride();
  }

  public string FacadeID
  {
    get => this._facadeID;
    private set
    {
      this._facadeID = value;
      this.OverrideName();
    }
  }

  public void ApplyAnimOverride()
  {
    if (this.FacadeID.IsNullOrWhiteSpace())
      return;
    this.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[1]
    {
      Db.GetEquippableFacades().Get(this.FacadeID).AnimFile
    });
  }

  private void OverrideName()
  {
    this.GetComponent<KSelectable>().SetName(EquippableFacade.GetNameOverride(this.GetComponent<Equippable>().def.Id, this.FacadeID));
  }

  public static string GetNameOverride(string defID, string facadeID)
  {
    return facadeID.IsNullOrWhiteSpace() ? (string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{defID.ToUpper()}.NAME") : Db.GetEquippableFacades().Get(facadeID).Name;
  }
}
