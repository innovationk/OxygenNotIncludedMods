// Decompiled with JetBrains decompiler
// Type: TransformingPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class TransformingPlant : KMonoBehaviour
{
  public string transformPlantId;
  public Func<object, bool> eventDataCondition;
  public bool useGrowthTimeRatio;
  public bool keepPlantablePlotStorage = true;
  public string fxKAnim;
  public string fxAnim;
  private static readonly EventSystem.IntraObjectHandler<TransformingPlant> OnTransformationEventDelegate = new EventSystem.IntraObjectHandler<TransformingPlant>((Action<TransformingPlant, object>) ((component, data) => component.DoPlantTransform(data)));

  public void SubscribeToTransformEvent(GameHashes eventHash)
  {
    this.Subscribe<TransformingPlant>((int) eventHash, TransformingPlant.OnTransformationEventDelegate);
  }

  public void UnsubscribeToTransformEvent(GameHashes eventHash)
  {
    this.Unsubscribe<TransformingPlant>((int) eventHash, TransformingPlant.OnTransformationEventDelegate);
  }

  private void DoPlantTransform(object data)
  {
    if (this.eventDataCondition != null && !this.eventDataCondition(data))
      return;
    GameObject plant = GameUtil.KInstantiate(Assets.GetPrefab(this.transformPlantId.ToTag()), Grid.SceneLayer.BuildingBack);
    plant.transform.SetPosition(this.transform.GetPosition());
    MutantPlant component1 = this.GetComponent<MutantPlant>();
    MutantPlant component2 = plant.GetComponent<MutantPlant>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) plant != (UnityEngine.Object) null)
    {
      component1.CopyMutationsTo(component2);
      PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
    }
    plant.SetActive(true);
    Growing component3 = this.GetComponent<Growing>();
    Growing component4 = plant.GetComponent<Growing>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && (UnityEngine.Object) component4 != (UnityEngine.Object) null)
    {
      float percent = component3.PercentGrown();
      if (this.useGrowthTimeRatio)
      {
        AmountInstance amountInstance1 = component3.GetAmounts().Get(Db.Get().Amounts.Maturity);
        AmountInstance amountInstance2 = component4.GetAmounts().Get(Db.Get().Amounts.Maturity);
        float num = amountInstance1.GetMax() / amountInstance2.GetMax();
        percent = Mathf.Clamp01(percent * num);
      }
      component4.OverrideMaturityLevel(percent);
    }
    PrimaryElement component5 = plant.GetComponent<PrimaryElement>();
    PrimaryElement component6 = this.GetComponent<PrimaryElement>();
    component5.Temperature = component6.Temperature;
    component5.AddDisease(component6.DiseaseIdx, component6.DiseaseCount, "TransformedPlant");
    plant.GetComponent<Effects>().CopyEffects(this.GetComponent<Effects>());
    HarvestDesignatable component7 = this.GetComponent<HarvestDesignatable>();
    HarvestDesignatable component8 = plant.GetComponent<HarvestDesignatable>();
    if ((UnityEngine.Object) component7 != (UnityEngine.Object) null && (UnityEngine.Object) component8 != (UnityEngine.Object) null)
      component8.SetHarvestWhenReady(component7.HarvestWhenReady);
    Prioritizable component9 = this.GetComponent<Prioritizable>();
    Prioritizable component10 = plant.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component9 != (UnityEngine.Object) null && (UnityEngine.Object) component10 != (UnityEngine.Object) null)
      component10.SetMasterPriority(component9.GetMasterPriority());
    PlantablePlot receptacle = this.GetComponent<ReceptacleMonitor>().GetReceptacle();
    if ((UnityEngine.Object) receptacle != (UnityEngine.Object) null)
      receptacle.ReplacePlant(plant, this.keepPlantablePlotStorage);
    Util.KDestroyGameObject(this.gameObject);
    if (this.fxKAnim == null)
      return;
    KBatchedAnimController effect = FXHelpers.CreateEffect(this.fxKAnim, plant.transform.position, layer: Grid.SceneLayer.FXFront);
    effect.Play((HashedString) this.fxAnim);
    effect.destroyOnAnimComplete = true;
  }
}
