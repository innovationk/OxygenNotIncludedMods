// Decompiled with JetBrains decompiler
// Type: Database.AccessorySlots
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class AccessorySlots : ResourceSet<AccessorySlot>
{
  public AccessorySlot Eyes;
  public AccessorySlot Hair;
  public AccessorySlot HeadShape;
  public AccessorySlot Mouth;
  public AccessorySlot Body;
  public AccessorySlot Arm;
  public AccessorySlot ArmLower;
  public AccessorySlot Hat;
  public AccessorySlot HatHair;
  public AccessorySlot HeadEffects;
  public AccessorySlot Belt;
  public AccessorySlot Neck;
  public AccessorySlot Pelvis;
  public AccessorySlot Leg;
  public AccessorySlot Foot;
  public AccessorySlot Skirt;
  public AccessorySlot Necklace;
  public AccessorySlot Cuff;
  public AccessorySlot Hand;
  public AccessorySlot ArmLowerSkin;
  public AccessorySlot ArmUpperSkin;
  public AccessorySlot LegSkin;

  public AccessorySlots(ResourceSet parent)
    : base(nameof (AccessorySlots), parent)
  {
    parent = (ResourceSet) Db.Get().Accessories;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "head_swap_kanim");
    KAnimFile anim2 = Assets.GetAnim((HashedString) "body_comp_default_kanim");
    KAnimFile anim3 = Assets.GetAnim((HashedString) "body_swap_kanim");
    KAnimFile anim4 = Assets.GetAnim((HashedString) "hair_swap_kanim");
    KAnimFile anim5 = Assets.GetAnim((HashedString) "hat_swap_kanim");
    this.Eyes = new AccessorySlot(nameof (Eyes), (ResourceSet) this, anim1);
    this.Hair = new AccessorySlot(nameof (Hair), (ResourceSet) this, anim4);
    this.HeadShape = new AccessorySlot(nameof (HeadShape), (ResourceSet) this, anim1);
    this.Mouth = new AccessorySlot(nameof (Mouth), (ResourceSet) this, anim1);
    this.Hat = new AccessorySlot(nameof (Hat), (ResourceSet) this, anim5, 4);
    this.HatHair = new AccessorySlot("Hat_Hair", (ResourceSet) this, anim4);
    this.HeadEffects = new AccessorySlot("HeadFX", (ResourceSet) this, anim1);
    this.Body = new AccessorySlot("Torso", (ResourceSet) this, new KAnimHashedString("torso"), anim3);
    this.Arm = new AccessorySlot("Arm_Sleeve", (ResourceSet) this, new KAnimHashedString("arm_sleeve"), anim3);
    this.ArmLower = new AccessorySlot("Arm_Lower_Sleeve", (ResourceSet) this, new KAnimHashedString("arm_lower_sleeve"), anim3);
    this.Belt = new AccessorySlot(nameof (Belt), (ResourceSet) this, new KAnimHashedString("belt"), anim2);
    this.Neck = new AccessorySlot(nameof (Neck), (ResourceSet) this, new KAnimHashedString("neck"), anim2);
    this.Pelvis = new AccessorySlot(nameof (Pelvis), (ResourceSet) this, new KAnimHashedString("pelvis"), anim2);
    this.Foot = new AccessorySlot(nameof (Foot), (ResourceSet) this, new KAnimHashedString("foot"), anim2, Assets.GetAnim((HashedString) "shoes_basic_black_kanim"));
    this.Leg = new AccessorySlot(nameof (Leg), (ResourceSet) this, new KAnimHashedString("leg"), anim2);
    this.Necklace = new AccessorySlot(nameof (Necklace), (ResourceSet) this, new KAnimHashedString("necklace"), anim2);
    this.Cuff = new AccessorySlot(nameof (Cuff), (ResourceSet) this, new KAnimHashedString("cuff"), anim2);
    this.Hand = new AccessorySlot(nameof (Hand), (ResourceSet) this, new KAnimHashedString("hand_paint"), anim2);
    this.Skirt = new AccessorySlot(nameof (Skirt), (ResourceSet) this, new KAnimHashedString("skirt"), anim3);
    this.ArmLowerSkin = new AccessorySlot("Arm_Lower", (ResourceSet) this, new KAnimHashedString("arm_lower"), anim3);
    this.ArmUpperSkin = new AccessorySlot("Arm_Upper", (ResourceSet) this, new KAnimHashedString("arm_upper"), anim3);
    this.LegSkin = new AccessorySlot("Leg_Skin", (ResourceSet) this, new KAnimHashedString("leg_skin"), anim3);
    foreach (AccessorySlot resource in this.resources)
      resource.AddAccessories(resource.AnimFile, parent);
    Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim((HashedString) "body_lonelyminion_kanim"), parent, this);
    Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim((HashedString) "body_sena_kanim"), parent, this);
    if (!DlcManager.IsContentSubscribed("DLC5_ID"))
      return;
    Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim((HashedString) "body_kai_kanim"), parent, this);
    Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim((HashedString) "body_minnow_kanim"), parent, this);
  }

  public AccessorySlot Find(KAnimHashedString symbol_name)
  {
    foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
    {
      if (symbol_name == resource.targetSymbolId)
        return resource;
    }
    return (AccessorySlot) null;
  }
}
