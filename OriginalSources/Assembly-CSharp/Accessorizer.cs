// Decompiled with JetBrains decompiler
// Type: Accessorizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Accessorizer")]
public class Accessorizer : KMonoBehaviour
{
  [Serialize]
  private List<ResourceRef<Accessory>> accessories = new List<ResourceRef<Accessory>>();
  [MyCmpReq]
  private KAnimControllerBase animController;
  [Serialize]
  private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

  public List<ResourceRef<Accessory>> GetAccessories() => this.accessories;

  public void SetAccessories(List<ResourceRef<Accessory>> data) => this.accessories = data;

  public KCompBuilder.BodyData bodyData { get; set; }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    MinionIdentity component = this.GetComponent<MinionIdentity>();
    if ((this.clothingItems.Count > 0 ? 1 : (!((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : (component.nameStringKey == "JORGE" ? 1 : 0))) != 0 || SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
    {
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.bodyData = Accessorizer.UpdateAccessorySlots(component.nameStringKey, ref this.accessories);
      this.accessories.RemoveAll((Predicate<ResourceRef<Accessory>>) (x => x.Get() == null));
    }
    if (this.clothingItems.Count > 0)
    {
      this.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, this.clothingItems.Select<ResourceRef<ClothingItemResource>, ClothingItemResource>((Func<ResourceRef<ClothingItemResource>, ClothingItemResource>) (i => i.Get())));
      this.clothingItems.Clear();
    }
    this.ApplyAccessories();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    MinionIdentity component = this.GetComponent<MinionIdentity>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.bodyData = MinionStartingStats.CreateBodyData(Db.Get().Personalities.Get(component.personalityResourceId));
  }

  public void AddAccessory(Accessory accessory)
  {
    if (accessory == null)
      return;
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
      this.animController = this.GetComponent<KAnimControllerBase>();
    this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) accessory.slot.targetSymbolId, accessory.symbol, accessory.slot.overrideLayer);
    if (this.HasAccessory(accessory))
      return;
    ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
    if (resourceRef == null)
      return;
    this.accessories.Add(resourceRef);
  }

  public void RemoveAccessory(Accessory accessory)
  {
    this.accessories.RemoveAll((Predicate<ResourceRef<Accessory>>) (x => x.Get() == accessory));
    this.animController.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride((HashedString) accessory.slot.targetSymbolId, accessory.slot.overrideLayer);
  }

  public void ApplyAccessories()
  {
    foreach (ResourceRef<Accessory> accessory1 in this.accessories)
    {
      Accessory accessory2 = accessory1.Get();
      if (accessory2 != null)
        this.AddAccessory(accessory2);
    }
  }

  public static KCompBuilder.BodyData UpdateAccessorySlots(
    string nameString,
    ref List<ResourceRef<Accessory>> accessories)
  {
    accessories.RemoveAll((Predicate<ResourceRef<Accessory>>) (acc => acc.Get() == null));
    Personality fromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(nameString);
    if (fromNameStringKey == null)
      return new KCompBuilder.BodyData();
    KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(fromNameStringKey);
    foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
    {
      if (resource.accessories.Count != 0)
      {
        Accessory accessory = (Accessory) null;
        if (resource == Db.Get().AccessorySlots.Body)
          accessory = resource.Lookup(bodyData.body);
        else if (resource == Db.Get().AccessorySlots.Arm)
          accessory = resource.Lookup(bodyData.arms);
        else if (resource == Db.Get().AccessorySlots.ArmLower)
          accessory = resource.Lookup(bodyData.armslower);
        else if (resource == Db.Get().AccessorySlots.ArmLowerSkin)
          accessory = resource.Lookup(bodyData.armLowerSkin);
        else if (resource == Db.Get().AccessorySlots.ArmUpperSkin)
          accessory = resource.Lookup(bodyData.armUpperSkin);
        else if (resource == Db.Get().AccessorySlots.LegSkin)
          accessory = resource.Lookup(bodyData.legSkin);
        else if (resource == Db.Get().AccessorySlots.Leg)
          accessory = resource.Lookup(bodyData.legs);
        else if (resource == Db.Get().AccessorySlots.Belt)
          accessory = resource.Lookup(bodyData.belt);
        else if (resource == Db.Get().AccessorySlots.Neck)
          accessory = resource.Lookup(bodyData.neck);
        else if (resource == Db.Get().AccessorySlots.Pelvis)
          accessory = resource.Lookup(bodyData.pelvis);
        else if (resource == Db.Get().AccessorySlots.Foot)
          accessory = resource.Lookup(bodyData.foot);
        else if (resource == Db.Get().AccessorySlots.Cuff)
          accessory = resource.Lookup(bodyData.cuff);
        else if (resource == Db.Get().AccessorySlots.Hand)
          accessory = resource.Lookup(bodyData.hand);
        if (accessory != null)
        {
          ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
          accessories.RemoveAll((Predicate<ResourceRef<Accessory>>) (old_acc => old_acc.Get().slot == accessory.slot));
          accessories.Add(resourceRef);
        }
      }
    }
    return bodyData;
  }

  public bool HasAccessory(Accessory accessory)
  {
    return this.accessories.Exists((Predicate<ResourceRef<Accessory>>) (x => x.Get() == accessory));
  }

  public Accessory GetAccessory(AccessorySlot slot)
  {
    for (int index = 0; index < this.accessories.Count; ++index)
    {
      if (this.accessories[index].Get() != null && this.accessories[index].Get().slot == slot)
        return this.accessories[index].Get();
    }
    return (Accessory) null;
  }

  public void ApplyMinionPersonality(Personality personality)
  {
    this.bodyData = MinionStartingStats.CreateBodyData(personality);
    this.accessories.Clear();
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
      this.animController = this.GetComponent<KAnimControllerBase>();
    string[] strArray = new string[9]
    {
      "snapTo_hat",
      "snapTo_hat_hair",
      "snapTo_goggles",
      "snapTo_headFX",
      "snapTo_neck",
      "snapTo_chest",
      "snapTo_pivot",
      "skirt",
      "necklace"
    };
    foreach (string str in strArray)
    {
      this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride((HashedString) str);
      this.animController.SetSymbolVisiblity((KAnimHashedString) str, false);
    }
    this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(this.bodyData.eyes));
    this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(this.bodyData.hair));
    this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(this.bodyData.hair)));
    this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(this.bodyData.headShape));
    this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(this.bodyData.mouth));
    this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(this.bodyData.body));
    this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(this.bodyData.arms));
    this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(this.bodyData.armslower));
    this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(this.bodyData.neck));
    this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(this.bodyData.pelvis));
    this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(this.bodyData.legs));
    this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(this.bodyData.foot));
    this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(this.bodyData.hand));
    this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(this.bodyData.cuff));
    this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(this.bodyData.belt));
    this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(this.bodyData.armLowerSkin));
    this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(this.bodyData.armUpperSkin));
    this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(this.bodyData.legSkin));
    this.UpdateHairBasedOnHat();
  }

  public void ApplyBodyData(KCompBuilder.BodyData bodyData)
  {
    this.accessories.Clear();
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
      this.animController = this.GetComponent<KAnimControllerBase>();
    string[] strArray = new string[9]
    {
      "snapTo_hat",
      "snapTo_hat_hair",
      "snapTo_goggles",
      "snapTo_headFX",
      "snapTo_neck",
      "snapTo_chest",
      "snapTo_pivot",
      "skirt",
      "necklace"
    };
    foreach (string str in strArray)
    {
      this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride((HashedString) str);
      this.animController.SetSymbolVisiblity((KAnimHashedString) str, false);
    }
    this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(bodyData.eyes));
    this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair));
    this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
    this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(bodyData.headShape));
    this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(bodyData.mouth));
    this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(bodyData.body));
    this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(bodyData.arms));
    this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(bodyData.armslower));
    this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(bodyData.neck));
    this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(bodyData.pelvis));
    this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(bodyData.legs));
    this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(bodyData.foot));
    this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(bodyData.hand));
    this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(bodyData.cuff));
    this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(bodyData.belt));
    this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
    this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
    this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(bodyData.legSkin));
    this.UpdateHairBasedOnHat();
  }

  public void UpdateHairBasedOnHat()
  {
    if (!this.GetAccessory(Db.Get().AccessorySlots.Hat).IsNullOrDestroyed())
    {
      this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
      this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
    }
    else
    {
      this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
      this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
      this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
    }
  }

  public void GetBodySlots(ref KCompBuilder.BodyData fd)
  {
    fd.eyes = HashedString.Invalid;
    fd.hair = HashedString.Invalid;
    fd.headShape = HashedString.Invalid;
    fd.mouth = HashedString.Invalid;
    fd.neck = HashedString.Invalid;
    fd.body = HashedString.Invalid;
    fd.arms = HashedString.Invalid;
    fd.armslower = HashedString.Invalid;
    fd.hat = HashedString.Invalid;
    fd.faceFX = HashedString.Invalid;
    fd.armLowerSkin = HashedString.Invalid;
    fd.armUpperSkin = HashedString.Invalid;
    fd.legSkin = HashedString.Invalid;
    fd.belt = HashedString.Invalid;
    fd.pelvis = HashedString.Invalid;
    fd.foot = HashedString.Invalid;
    fd.skirt = HashedString.Invalid;
    fd.necklace = HashedString.Invalid;
    fd.cuff = HashedString.Invalid;
    fd.hand = HashedString.Invalid;
    for (int index = 0; index < this.accessories.Count; ++index)
    {
      Accessory accessory = this.accessories[index].Get();
      if (accessory != null)
      {
        if (accessory.slot.Id == "Eyes")
          fd.eyes = accessory.IdHash;
        else if (accessory.slot.Id == "Hair")
          fd.hair = accessory.IdHash;
        else if (accessory.slot.Id == "HeadShape")
          fd.headShape = accessory.IdHash;
        else if (accessory.slot.Id == "Mouth")
          fd.mouth = accessory.IdHash;
        else if (accessory.slot.Id == "Neck")
          fd.neck = accessory.IdHash;
        else if (accessory.slot.Id == "Torso")
          fd.body = accessory.IdHash;
        else if (accessory.slot.Id == "Arm_Sleeve")
          fd.arms = accessory.IdHash;
        else if (accessory.slot.Id == "Arm_Lower_Sleeve")
          fd.armslower = accessory.IdHash;
        else if (accessory.slot.Id == "Hat")
          fd.hat = HashedString.Invalid;
        else if (accessory.slot.Id == "FaceEffect")
          fd.faceFX = HashedString.Invalid;
        else if (accessory.slot.Id == "Arm_Lower")
          fd.armLowerSkin = (HashedString) accessory.Id;
        else if (accessory.slot.Id == "Arm_Upper")
          fd.armUpperSkin = (HashedString) accessory.Id;
        else if (accessory.slot.Id == "Leg_Skin")
          fd.legSkin = (HashedString) accessory.Id;
        else if (accessory.slot.Id == "Leg")
          fd.legs = (HashedString) accessory.Id;
        else if (accessory.slot.Id == "Belt")
          fd.belt = accessory.IdHash;
        else if (accessory.slot.Id == "Pelvis")
          fd.pelvis = accessory.IdHash;
        else if (accessory.slot.Id == "Foot")
          fd.foot = accessory.IdHash;
        else if (accessory.slot.Id == "Cuff")
          fd.cuff = accessory.IdHash;
        else if (accessory.slot.Id == "Skirt")
          fd.skirt = accessory.IdHash;
        else if (accessory.slot.Id == "Hand")
          fd.hand = accessory.IdHash;
      }
    }
  }
}
