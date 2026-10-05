// Decompiled with JetBrains decompiler
// Type: EntitySplitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/EntitySplitter")]
public class EntitySplitter : KMonoBehaviour
{
  public float maxStackSize = PrimaryElement.MAX_MASS;
  private static readonly EventSystem.IntraObjectHandler<EntitySplitter> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<EntitySplitter>((Action<EntitySplitter, object>) ((component, data) => component.OnAbsorb(data)));
  private static bool _empty_other_notified = false;

  protected static Pickupable OnTakeBehavior(Pickupable p, float a) => EntitySplitter.Split(p, a);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Pickupable pickupable = this.GetComponent<Pickupable>();
    if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
      Debug.LogError((object) (this.name + " does not have a pickupable component!"));
    pickupable.OnTake += new Func<Pickupable, float, Pickupable>(EntitySplitter.OnTakeBehavior);
    Rottable.Instance rottable = this.gameObject.GetSMI<Rottable.Instance>();
    pickupable.absorbable = true;
    pickupable.CanAbsorb = (Func<Pickupable, bool>) (other => EntitySplitter.CanFirstAbsorbSecond(pickupable, rottable, other, this.maxStackSize));
    this.Subscribe<EntitySplitter>(-2064133523, EntitySplitter.OnAbsorbDelegate);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Pickupable component = this.GetComponent<Pickupable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.OnTake -= new Func<Pickupable, float, Pickupable>(EntitySplitter.OnTakeBehavior);
  }

  public static bool CanFirstAbsorbSecond(
    Pickupable pickupable,
    Rottable.Instance rottable,
    Pickupable other,
    float maxStackSize)
  {
    if ((UnityEngine.Object) other == (UnityEngine.Object) null)
      return false;
    KPrefabID kprefabId1 = pickupable.KPrefabID;
    KPrefabID kprefabId2 = other.KPrefabID;
    if ((UnityEngine.Object) kprefabId1 == (UnityEngine.Object) null || (UnityEngine.Object) kprefabId2 == (UnityEngine.Object) null || kprefabId1.PrefabTag != kprefabId2.PrefabTag || (double) pickupable.TotalAmount + (double) other.TotalAmount > (double) maxStackSize || kprefabId1.HasTag(GameTags.MarkedForMove) || kprefabId2.HasTag(GameTags.MarkedForMove) || (double) pickupable.PrimaryElement.Mass + (double) other.PrimaryElement.Mass > (double) maxStackSize)
      return false;
    if (rottable != null)
    {
      Rottable.Instance smi = other.GetSMI<Rottable.Instance>();
      if (smi == null || !rottable.IsRotLevelStackable(smi))
        return false;
    }
    bool flag = kprefabId1.HasTag(GameTags.SpicedFood);
    if (flag != kprefabId2.HasTag(GameTags.SpicedFood))
      return false;
    Edible component1 = kprefabId1.GetComponent<Edible>();
    Edible component2 = kprefabId2.GetComponent<Edible>();
    if (flag && !component1.CanAbsorb(component2))
      return false;
    if (kprefabId1.HasTag(GameTags.Seed) || kprefabId1.HasTag(GameTags.CropSeed) || kprefabId1.HasTag(GameTags.Compostable))
    {
      MutantPlant component3 = pickupable.GetComponent<MutantPlant>();
      MutantPlant component4 = other.GetComponent<MutantPlant>();
      if (((UnityEngine.Object) component3 != (UnityEngine.Object) null || (UnityEngine.Object) component4 != (UnityEngine.Object) null) && ((UnityEngine.Object) component3 == (UnityEngine.Object) null != ((UnityEngine.Object) component4 == (UnityEngine.Object) null) || kprefabId1.HasTag(GameTags.UnidentifiedSeed) != kprefabId2.HasTag(GameTags.UnidentifiedSeed) || component3.SubSpeciesID != component4.SubSpeciesID))
        return false;
    }
    return true;
  }

  public static Pickupable Split(Pickupable pickupable, float amount, GameObject prefab = null)
  {
    if ((double) amount >= (double) pickupable.TotalAmount && (UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return pickupable;
    Storage storage = pickupable.storage;
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      prefab = Assets.GetPrefab(pickupable.KPrefabID.PrefabID());
    GameObject parent = (GameObject) null;
    if ((UnityEngine.Object) pickupable.transform.parent != (UnityEngine.Object) null)
      parent = pickupable.transform.parent.gameObject;
    GameObject context = GameUtil.KInstantiate(prefab, pickupable.transform.GetPosition(), Grid.SceneLayer.Ore, parent);
    Debug.Assert((UnityEngine.Object) context != (UnityEngine.Object) null, (object) "WTH, the GO is null, shouldn't happen on instantiate");
    Pickupable component = context.GetComponent<Pickupable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      Debug.LogError((object) ("Edible::OnTake() No Pickupable component for " + context.name), (UnityEngine.Object) context);
    context.SetActive(true);
    component.TotalAmount = Mathf.Min(amount, pickupable.TotalAmount);
    component.PrimaryElement.Temperature = pickupable.PrimaryElement.Temperature;
    bool keepZeroMassObject = pickupable.PrimaryElement.KeepZeroMassObject;
    pickupable.PrimaryElement.KeepZeroMassObject = true;
    pickupable.TotalAmount -= amount;
    component.Trigger(1335436905, (object) pickupable);
    pickupable.PrimaryElement.KeepZeroMassObject = keepZeroMassObject;
    pickupable.TotalAmount += 0.0f;
    if ((UnityEngine.Object) storage != (UnityEngine.Object) null)
    {
      storage.Trigger(-1697596308, (object) pickupable.gameObject);
      storage.Trigger(-778359855, (object) storage);
    }
    IExtendSplitting[] components = pickupable.GetComponents<IExtendSplitting>();
    if (components != null)
    {
      for (int index = 0; index < components.Length; ++index)
        components[index].OnSplitTick(component);
    }
    return component;
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
      return;
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    PrimaryElement primaryElement = pickupable.PrimaryElement;
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    float num = component.Temperature;
    float mass1 = component.Mass;
    float mass2 = primaryElement.Mass;
    if ((double) mass1 > 0.0 && (double) mass2 > 0.0)
      num = SimUtil.CalculateFinalTemperature(mass1, num, mass2, primaryElement.Temperature);
    else if ((double) mass2 > 0.0)
      num = primaryElement.Temperature;
    if ((double) mass2 == 0.0 && !EntitySplitter._empty_other_notified)
    {
      EntitySplitter._empty_other_notified = true;
      KCrashReporter.ReportDevNotification("EntitySplitter::OnAbsorb other_pe is 0 mass", Environment.StackTrace, $"{this.ToString()} <- {pickupable.ToString()}");
    }
    component.SetMassTemperature(mass1 + mass2, num);
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null))
      return;
    string sound = GlobalAssets.GetSound("Ore_absorb");
    Vector3 position = pickupable.transform.GetPosition() with
    {
      z = 0.0f
    };
    if (sound == null || !CameraController.Instance.IsAudibleSound(position, (HashedString) sound))
      return;
    KFMOD.PlayOneShot(sound, position);
  }
}
