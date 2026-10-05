// Decompiled with JetBrains decompiler
// Type: FullBodyUIMinionWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class FullBodyUIMinionWidget : KMonoBehaviour
{
  [SerializeField]
  private GameObject duplicantAnimAnchor;
  public const float UI_MINION_PORTRAIT_ANIM_SCALE = 0.38f;

  public KBatchedAnimController animController { get; private set; }

  protected override void OnSpawn() => this.TrySpawnDisplayMinion();

  private void TrySpawnDisplayMinion()
  {
    if (!((UnityEngine.Object) this.animController == (UnityEngine.Object) null))
      return;
    this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("FullMinionUIPortrait")), this.duplicantAnimAnchor.gameObject).GetComponent<KBatchedAnimController>();
    this.animController.gameObject.SetActive(true);
    this.animController.animScale = 0.38f;
  }

  private void InitializeAnimator()
  {
    this.TrySpawnDisplayMinion();
    this.animController.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop);
    Accessorizer component = this.animController.GetComponent<Accessorizer>();
    for (int index = component.GetAccessories().Count - 1; index >= 0; --index)
      component.RemoveAccessory(component.GetAccessories()[index].Get());
  }

  public void SetDefaultPortraitAnimator()
  {
    MinionIdentity minionIdentity = Components.MinionIdentities.Count > 0 ? Components.MinionIdentities[0] : (MinionIdentity) null;
    HashedString id = (UnityEngine.Object) minionIdentity != (UnityEngine.Object) null ? minionIdentity.personalityResourceId : (HashedString) Db.Get().Personalities.resources.GetRandom<Personality>().Id;
    this.InitializeAnimator();
    this.animController.GetComponent<Accessorizer>().ApplyMinionPersonality(Db.Get().Personalities.Get(id));
    Accessorizer component = (UnityEngine.Object) minionIdentity != (UnityEngine.Object) null ? minionIdentity.GetComponent<Accessorizer>() : (Accessorizer) null;
    KAnim.Build.Symbol hair_symbol = (KAnim.Build.Symbol) null;
    KAnim.Build.Symbol hat_hair_symbol = (KAnim.Build.Symbol) null;
    if ((bool) (UnityEngine.Object) component)
    {
      hair_symbol = component.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
      hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
    }
    this.UpdateHatOverride((string) null, hair_symbol, hat_hair_symbol);
    this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, (StoredMinionIdentity) null);
  }

  public void SetPortraitAnimator(IAssignableIdentity assignableIdentity)
  {
    if (assignableIdentity == null || assignableIdentity.IsNull())
    {
      this.SetDefaultPortraitAnimator();
    }
    else
    {
      this.InitializeAnimator();
      string current_hat = "";
      MinionIdentity minionIdentity;
      StoredMinionIdentity storedMinionIdentity;
      this.GetMinionIdentity(assignableIdentity, out minionIdentity, out storedMinionIdentity);
      Accessorizer component1 = this.animController.GetComponent<Accessorizer>();
      KAnim.Build.Symbol hair_symbol = (KAnim.Build.Symbol) null;
      KAnim.Build.Symbol hat_hair_symbol = (KAnim.Build.Symbol) null;
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
      {
        Accessorizer component2 = minionIdentity.GetComponent<Accessorizer>();
        foreach (ResourceRef<Accessory> accessory in component2.GetAccessories())
          component1.AddAccessory(accessory.Get());
        current_hat = minionIdentity.GetComponent<MinionResume>().CurrentHat;
        hair_symbol = component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
        hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
      }
      else if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null)
      {
        foreach (ResourceRef<Accessory> accessory in storedMinionIdentity.accessories)
          component1.AddAccessory(accessory.Get());
        current_hat = storedMinionIdentity.currentHat;
        hair_symbol = storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
        hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
      }
      this.UpdateHatOverride(current_hat, hair_symbol, hat_hair_symbol);
      this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, storedMinionIdentity);
    }
  }

  private void UpdateHatOverride(
    string current_hat,
    KAnim.Build.Symbol hair_symbol,
    KAnim.Build.Symbol hat_hair_symbol)
  {
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, !string.IsNullOrEmpty(current_hat));
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, string.IsNullOrEmpty(current_hat));
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, !string.IsNullOrEmpty(current_hat));
    SymbolOverrideController component = this.animController.GetComponent<SymbolOverrideController>();
    if (hair_symbol != null)
      component.AddSymbolOverride((HashedString) "snapto_hair_always", hair_symbol, 1);
    if (hat_hair_symbol == null)
      return;
    component.AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HatHair.targetSymbolId, hat_hair_symbol, 1);
  }

  private void UpdateClothingOverride(
    SymbolOverrideController symbolOverrideController,
    MinionIdentity identity,
    StoredMinionIdentity storedMinionIdentity)
  {
    string[] source = (string[]) null;
    if ((UnityEngine.Object) identity != (UnityEngine.Object) null)
      source = identity.GetComponent<WearableAccessorizer>().GetClothingItemsIds(ClothingOutfitUtility.OutfitType.Clothing);
    else if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null)
      source = storedMinionIdentity.GetClothingItemIds(ClothingOutfitUtility.OutfitType.Clothing);
    if (source == null)
      return;
    this.animController.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, ((IEnumerable<string>) source).Select<string, ClothingItemResource>((Func<string, ClothingItemResource>) (i => Db.Get().Permits.ClothingItems.Get(i))));
  }

  public void UpdateEquipment(Equippable equippable, KAnimFile animFile)
  {
    this.animController.GetComponent<WearableAccessorizer>().ApplyEquipment(equippable, animFile);
  }

  public void RemoveEquipment(Equippable equippable)
  {
    this.animController.GetComponent<WearableAccessorizer>().RemoveEquipment(equippable);
  }

  private void GetMinionIdentity(
    IAssignableIdentity assignableIdentity,
    out MinionIdentity minionIdentity,
    out StoredMinionIdentity storedMinionIdentity)
  {
    if (assignableIdentity is MinionAssignablesProxy)
    {
      minionIdentity = ((MinionAssignablesProxy) assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
      storedMinionIdentity = ((MinionAssignablesProxy) assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
    }
    else
    {
      minionIdentity = assignableIdentity as MinionIdentity;
      storedMinionIdentity = assignableIdentity as StoredMinionIdentity;
    }
  }
}
