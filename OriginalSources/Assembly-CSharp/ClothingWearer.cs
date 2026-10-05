// Decompiled with JetBrains decompiler
// Type: ClothingWearer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ClothingWearer")]
public class ClothingWearer : KMonoBehaviour
{
  private DecorProvider decorProvider;
  private SchedulerHandle spawnApplyClothesHandle;
  private AttributeModifier decorModifier;
  private AttributeModifier conductivityModifier;
  [Serialize]
  public ClothingWearer.ClothingInfo currentClothing;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.decorProvider = this.GetComponent<DecorProvider>();
    if (this.decorModifier == null)
      this.decorModifier = new AttributeModifier("Decor", 0.0f, (string) DUPLICANTS.MODIFIERS.CLOTHING.NAME, is_readonly: false);
    if (this.conductivityModifier != null)
      return;
    AttributeInstance attributeInstance = this.gameObject.GetAttributes().Get("ThermalConductivityBarrier");
    this.conductivityModifier = new AttributeModifier("ThermalConductivityBarrier", ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, (string) DUPLICANTS.MODIFIERS.CLOTHING.NAME, is_readonly: false);
    AttributeModifier conductivityModifier = this.conductivityModifier;
    attributeInstance.Add(conductivityModifier);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.decorProvider.decor.Add(this.decorModifier);
    this.decorProvider.decorRadius.Add(new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 3f));
    Traits component = this.GetComponent<Traits>();
    string format = (string) UI.OVERLAYS.DECOR.CLOTHING;
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (component.HasTrait("DecorUp"))
        format = (string) UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORUP;
      else if (component.HasTrait("DecorDown"))
        format = (string) UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORDOWN;
    }
    this.decorProvider.overrideName = string.Format(format, (object) this.gameObject.GetProperName());
    if (this.currentClothing == null)
      this.ChangeToDefaultClothes();
    else
      this.ChangeClothes(this.currentClothing);
    this.spawnApplyClothesHandle = GameScheduler.Instance.Schedule("ApplySpawnClothes", 2f, (Action<object>) (obj => this.GetComponent<CreatureSimTemperatureTransfer>().RefreshRegistration()), (object) null, (SchedulerGroup) null);
  }

  protected override void OnCleanUp()
  {
    this.spawnApplyClothesHandle.ClearScheduler();
    base.OnCleanUp();
  }

  public void ChangeClothes(ClothingWearer.ClothingInfo clothingInfo)
  {
    this.decorProvider.baseRadius = 3f;
    this.currentClothing = clothingInfo;
    this.conductivityModifier.Description = clothingInfo.name;
    this.conductivityModifier.SetValue(this.currentClothing.conductivityMod);
    this.decorModifier.SetValue((float) this.currentClothing.decorMod);
  }

  public void ChangeToDefaultClothes()
  {
    this.ChangeClothes(new ClothingWearer.ClothingInfo(ClothingWearer.ClothingInfo.BASIC_CLOTHING.name, ClothingWearer.ClothingInfo.BASIC_CLOTHING.decorMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.homeostasisEfficiencyMultiplier));
  }

  public class ClothingInfo
  {
    [Serialize]
    public string name = "";
    [Serialize]
    public int decorMod;
    [Serialize]
    public float conductivityMod;
    [Serialize]
    public float homeostasisEfficiencyMultiplier;
    public static readonly ClothingWearer.ClothingInfo BASIC_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME, -5, 1f / 400f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo WARM_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.WARM_VEST.NAME, 0, 0.008f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo COOL_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.COOL_VEST.NAME, -10, 0.0005f, 0.0f);
    public static readonly ClothingWearer.ClothingInfo FANCY_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.FUNKY_VEST.NAME, 30, 1f / 400f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo CUSTOM_CLOTHING = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 1f / 400f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo SLEEP_CLINIC_PAJAMAS = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 1f / 400f, -1.25f);
    public static readonly ClothingWearer.ClothingInfo DRY_SUIT = new ClothingWearer.ClothingInfo((string) EQUIPMENT.PREFABS.DRYSUIT.NAME, 0, 0.008f, -1.25f);

    public ClothingInfo(
      string _name,
      int _decor,
      float _temperature,
      float _homeostasisEfficiencyMultiplier)
    {
      this.name = _name;
      this.decorMod = _decor;
      this.conductivityMod = _temperature;
      this.homeostasisEfficiencyMultiplier = _homeostasisEfficiencyMultiplier;
    }

    public static void OnEquipVest(Equippable eq, ClothingWearer.ClothingInfo clothingInfo)
    {
      if ((UnityEngine.Object) eq == (UnityEngine.Object) null || eq.assignee == null)
        return;
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if ((UnityEngine.Object) soleOwner == (UnityEngine.Object) null)
        return;
      ClothingWearer component = (soleOwner.GetComponent<MinionAssignablesProxy>().target as KMonoBehaviour).GetComponent<ClothingWearer>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.ChangeClothes(clothingInfo);
      else
        Debug.LogWarning((object) "Clothing item cannot be equipped to assignee because they lack ClothingWearer component");
    }

    public static void OnUnequipVest(Equippable eq)
    {
      if (!((UnityEngine.Object) eq != (UnityEngine.Object) null) || eq.assignee == null)
        return;
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if ((UnityEngine.Object) soleOwner == (UnityEngine.Object) null)
        return;
      MinionAssignablesProxy component1 = soleOwner.GetComponent<MinionAssignablesProxy>();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
        return;
      GameObject targetGameObject = component1.GetTargetGameObject();
      if ((UnityEngine.Object) targetGameObject == (UnityEngine.Object) null)
        return;
      ClothingWearer component2 = targetGameObject.GetComponent<ClothingWearer>();
      if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
        return;
      component2.ChangeToDefaultClothes();
    }

    public static void SetupVest(GameObject go)
    {
      go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
      Equippable equippable = go.GetComponent<Equippable>();
      if ((UnityEngine.Object) equippable == (UnityEngine.Object) null)
        equippable = go.AddComponent<Equippable>();
      equippable.SetQuality(QualityLevel.Poor);
      go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
    }
  }
}
