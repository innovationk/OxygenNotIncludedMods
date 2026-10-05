// Decompiled with JetBrains decompiler
// Type: DissolvingElementDiseaseEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DissolvingElementDiseaseEmitter : DiseaseEmitter
{
  protected SpawnFXHashes spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
  protected float massDecayScale = 0.0001f;
  protected float massConversionRatio = 0.5f;
  protected const float massForMinEmission = 200f;
  protected const float massForMaxEmission = 1000f;
  private float minimumBubbleSize = 0.1f;
  private Guid statusItemGUID;
  protected float massDecayAccumulation;

  public SimHashes DissolveTargetElement { get; private set; }

  public float CurrentAverageDissolveRate { get; private set; }

  public PrimaryElement PrimaryElement { get; private set; }

  public DissolvingElementDiseaseEmitter(SimHashes dissolveTargetElement)
  {
    this.DissolveTargetElement = dissolveTargetElement;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Init();
    this.UpdateStatusItem();
  }

  private void Init() => this.PrimaryElement = this.GetComponent<PrimaryElement>();

  private void Update()
  {
    this.EvaluateEmissionCondition();
    this.MakeDiseaseAndBubbles();
  }

  protected virtual void EvaluateEmissionCondition()
  {
  }

  protected void UpdateStatusItem()
  {
    if (this.enableEmitter)
      this.statusItemGUID = this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.DissolvingElementDissolving, (object) this);
    else
      this.statusItemGUID = this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.DissolvingElementDormant, (object) this);
  }

  protected void MakeDiseaseAndBubbles()
  {
    if ((double) Time.deltaTime == 0.0 || !this.enableEmitter)
      return;
    this.CurrentAverageDissolveRate = this.massDecayScale * Mathf.Clamp(this.PrimaryElement.Mass, 200f, 1000f);
    float num1 = this.CurrentAverageDissolveRate * Time.deltaTime;
    if ((double) this.PrimaryElement.Mass > (double) num1)
    {
      this.massDecayAccumulation += num1;
      if ((double) this.massDecayAccumulation < (double) this.minimumBubbleSize)
        return;
      float num2 = Mathf.Min(this.PrimaryElement.Mass, this.massDecayAccumulation);
      this.PrimaryElement.Mass -= num2;
      BubbleManager.instance.SpawnBubble(this.DissolveTargetElement, (Vector2) this.transform.position, num2 * this.massConversionRatio, this.PrimaryElement.Temperature, BubbleManager.Disease.None);
      this.massDecayAccumulation = 0.0f;
      this.SpawnVisualFX();
    }
    else
    {
      this.SpawnVisualFX();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
  }

  protected void SpawnVisualFX()
  {
    if (this.spawnFXHash == SpawnFXHashes.None)
      return;
    this.transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    Game.Instance.SpawnFX(this.spawnFXHash, this.transform.GetPosition(), 0.0f);
  }
}
