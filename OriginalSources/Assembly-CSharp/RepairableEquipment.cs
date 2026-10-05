// Decompiled with JetBrains decompiler
// Type: RepairableEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
public class RepairableEquipment : KMonoBehaviour
{
  public DefHandle defHandle;
  [Serialize]
  public string facadeID;

  public EquipmentDef def
  {
    get => this.defHandle.Get<EquipmentDef>();
    set => this.defHandle.Set<EquipmentDef>(value);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.def.AdditionalTags == null)
      return;
    foreach (Tag additionalTag in this.def.AdditionalTags)
      this.GetComponent<KPrefabID>().AddTag(additionalTag);
  }

  protected override void OnSpawn()
  {
    if (this.facadeID.IsNullOrWhiteSpace())
      return;
    KAnim.Build.Symbol symbol = Db.GetEquippableFacades().Get(this.facadeID).AnimFile.GetData().build.GetSymbol((KAnimHashedString) "object");
    SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
    component.TryRemoveSymbolOverride((HashedString) "object");
    component.AddSymbolOverride((HashedString) "object", symbol);
  }
}
