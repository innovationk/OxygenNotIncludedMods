// Decompiled with JetBrains decompiler
// Type: UIDupeRandomizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIDupeRandomizer : MonoBehaviour
{
  [Tooltip("Enable this to allow for a chance for skill hats to appear")]
  public bool applyHat = true;
  [Tooltip("Enable this to allow for a chance for suit helmets to appear (ie. atmosuit and leadsuit)")]
  public bool applySuit = true;
  public UIDupeRandomizer.AnimChoice[] anims;
  private AccessorySlots slots;

  protected virtual void Start()
  {
    this.slots = Db.Get().AccessorySlots;
    for (int minion_idx = 0; minion_idx < this.anims.Length; ++minion_idx)
    {
      this.anims[minion_idx].curBody = (KAnimFile) null;
      this.GetNewBody(minion_idx);
    }
  }

  protected void GetNewBody(int minion_idx)
  {
    Personality random = Db.Get().Personalities.GetRandom(true, false);
    foreach (KBatchedAnimController minion in this.anims[minion_idx].minions)
      this.Apply(minion, random);
  }

  private void Apply(KBatchedAnimController dupe, Personality personality)
  {
    KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personality);
    SymbolOverrideController component = dupe.GetComponent<SymbolOverrideController>();
    component.RemoveAllSymbolOverrides();
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Hair.Lookup(bodyData.hair));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Eyes.Lookup(bodyData.eyes));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.HeadShape.Lookup(bodyData.headShape));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Mouth.Lookup(bodyData.mouth));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Body.Lookup(bodyData.body));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Arm.Lookup(bodyData.arms));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLower.Lookup(bodyData.armslower));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Belt.Lookup(bodyData.belt));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Hand.Lookup(bodyData.hand));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Neck.Lookup(bodyData.neck));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Cuff.Lookup(bodyData.cuff));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Pelvis.Lookup(bodyData.pelvis));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Leg.Lookup(bodyData.legs));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Foot.Lookup(bodyData.foot));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.LegSkin.Lookup(bodyData.legSkin));
    if (this.applySuit && (double) UnityEngine.Random.value < 0.15000000596046448)
    {
      component.AddBuildOverride(Assets.GetAnim((HashedString) "body_oxygen_kanim").GetData(), 6);
      dupe.SetSymbolVisiblity((KAnimHashedString) "snapto_neck", true);
      dupe.SetSymbolVisiblity((KAnimHashedString) "belt", false);
    }
    else
      dupe.SetSymbolVisiblity((KAnimHashedString) "snapto_neck", false);
    if (this.applyHat && (double) UnityEngine.Random.value < 0.5)
    {
      List<string> stringList = new List<string>();
      foreach (Skill resource in Db.Get().Skills.resources)
      {
        if (resource.requiredDuplicantModel.IsNullOrWhiteSpace() || (Tag) resource.requiredDuplicantModel == personality.model)
          stringList.Add(resource.hat);
      }
      string id = stringList[UnityEngine.Random.Range(0, stringList.Count)];
      UIDupeRandomizer.AddAccessory(dupe, this.slots.Hat.Lookup(id));
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
    }
    else
    {
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
    }
    dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, false);
    dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, false);
  }

  public static KAnimHashedString AddAccessory(KBatchedAnimController minion, Accessory accessory)
  {
    if (accessory == null)
      return (KAnimHashedString) HashedString.Invalid;
    SymbolOverrideController component = minion.GetComponent<SymbolOverrideController>();
    DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, minion.name + " is missing symbol override controller");
    component.TryRemoveSymbolOverride((HashedString) accessory.slot.targetSymbolId);
    component.AddSymbolOverride((HashedString) accessory.slot.targetSymbolId, accessory.symbol);
    minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, true);
    return accessory.slot.targetSymbolId;
  }

  public KAnimHashedString AddRandomAccessory(
    KBatchedAnimController minion,
    List<Accessory> choices)
  {
    Accessory choice = choices[UnityEngine.Random.Range(1, choices.Count)];
    return UIDupeRandomizer.AddAccessory(minion, choice);
  }

  public void Randomize()
  {
    if (this.slots == null)
      return;
    for (int minion_idx = 0; minion_idx < this.anims.Length; ++minion_idx)
      this.GetNewBody(minion_idx);
  }

  protected virtual void Update()
  {
  }

  [Serializable]
  public struct AnimChoice
  {
    public string anim_name;
    public List<KBatchedAnimController> minions;
    public float minSecondsBetweenAction;
    public float maxSecondsBetweenAction;
    public float lastWaitTime;
    public KAnimFile curBody;
  }
}
