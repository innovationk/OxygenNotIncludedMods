// Decompiled with JetBrains decompiler
// Type: Sublimates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Sublimates")]
public class Sublimates : KMonoBehaviour, ISim200ms
{
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private KSelectable selectable;
  [SerializeField]
  public SpawnFXHashes spawnFXHash;
  public bool decayStorage;
  [SerializeField]
  public Sublimates.Info info;
  [Serialize]
  private float sublimatedMass;
  private HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;
  private Sublimates.EmitState lastEmitState = ~Sublimates.EmitState.Emitting;
  private static readonly EventSystem.IntraObjectHandler<Sublimates> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Sublimates>((Action<Sublimates, object>) ((component, data) => component.OnAbsorb(data)));
  private static readonly EventSystem.IntraObjectHandler<Sublimates> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<Sublimates>((Action<Sublimates, object>) ((component, data) => component.OnSplitFromChunk(data)));

  public float Temperature => this.primaryElement.Temperature;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Sublimates>(-2064133523, Sublimates.OnAbsorbDelegate);
    this.Subscribe<Sublimates>(1335436905, Sublimates.OnSplitFromChunkDelegate);
    this.simRenderLoadBalance = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.flowAccumulator = Game.Instance.accumulators.Add("EmittedMass", (KMonoBehaviour) this);
    this.RefreshStatusItem(Sublimates.EmitState.Emitting);
  }

  protected override void OnCleanUp()
  {
    this.flowAccumulator = Game.Instance.accumulators.Remove(this.flowAccumulator);
    base.OnCleanUp();
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
      return;
    Sublimates component = pickupable.GetComponent<Sublimates>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.sublimatedMass += component.sublimatedMass;
  }

  private void OnSplitFromChunk(object data)
  {
    Pickupable pickupable = data as Pickupable;
    PrimaryElement primaryElement = pickupable.PrimaryElement;
    Sublimates component = pickupable.GetComponent<Sublimates>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    float mass1 = this.primaryElement.Mass;
    float mass2 = primaryElement.Mass;
    float num1 = mass1 / (mass2 + mass1);
    this.sublimatedMass = component.sublimatedMass * num1;
    float num2 = 1f - num1;
    component.sublimatedMass *= num2;
  }

  private bool SimMightOffcellOverpressure(int cell, SimHashes offgass)
  {
    SimHashes id = Grid.Element[cell].id;
    if (id == offgass || id == SimHashes.Vacuum)
      return false;
    ReadOnlySpan<int> readOnlySpan1 = Span<int>.op_Implicit(stackalloc int[3]
    {
      Grid.CellLeft(cell),
      Grid.CellRight(cell),
      Grid.CellAbove(cell)
    });
    bool flag = false;
    ReadOnlySpan<int> readOnlySpan2 = readOnlySpan1;
    for (int index1 = 0; index1 < readOnlySpan2.Length; ++index1)
    {
      int index2 = readOnlySpan2[index1];
      if (Grid.IsValidCell(index2))
      {
        if (Grid.Element[index2].id == id)
          return false;
        if (Grid.Element[index2].id == offgass)
        {
          flag = true;
          if ((double) Grid.Mass[index2] < (double) this.info.maxDestinationMass)
            return false;
        }
      }
    }
    return flag;
  }

  public void Sim200ms(float dt)
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (!Grid.IsValidCell(cell))
      return;
    bool flag = this.HasTag(GameTags.Sealed);
    Pickupable component = this.GetComponent<Pickupable>();
    Storage storage = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.storage : (Storage) null;
    if (flag && !this.decayStorage || flag && (UnityEngine.Object) storage != (UnityEngine.Object) null && storage.HasTag(GameTags.CorrosionProof))
      return;
    Element elementByHash = ElementLoader.FindElementByHash(this.info.sublimatedElement);
    if ((double) this.primaryElement.Temperature <= (double) elementByHash.lowTemp)
    {
      this.RefreshStatusItem(Sublimates.EmitState.BlockedOnTemperature);
    }
    else
    {
      float num1 = Grid.Mass[cell];
      if ((double) num1 < (double) this.info.maxDestinationMass)
      {
        float mass1 = this.primaryElement.Mass;
        if ((double) mass1 > 0.0)
        {
          float num2 = Mathf.Min(Mathf.Max(this.info.sublimationRate, this.info.sublimationRate * Mathf.Pow(mass1, this.info.massPower)) * dt, mass1);
          this.sublimatedMass += num2;
          float num3 = mass1 - num2;
          if ((double) this.sublimatedMass <= (double) this.info.minSublimationAmount)
            return;
          float num4 = this.sublimatedMass / this.primaryElement.Mass;
          byte diseaseIdx;
          int disease_count;
          if (this.info.diseaseIdx == byte.MaxValue)
          {
            diseaseIdx = this.primaryElement.DiseaseIdx;
            disease_count = (int) ((double) this.primaryElement.DiseaseCount * (double) num4);
            this.primaryElement.ModifyDiseaseCount(-disease_count, "Sublimates.SimUpdate");
          }
          else
          {
            float num5 = this.sublimatedMass / this.info.sublimationRate;
            diseaseIdx = this.info.diseaseIdx;
            disease_count = (int) ((double) this.info.diseaseCount * (double) num5);
          }
          float mass2 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num1);
          if ((double) mass2 > 0.0 && !this.SimMightOffcellOverpressure(cell, elementByHash.id))
          {
            this.Emit(cell, mass2, this.primaryElement.Temperature, diseaseIdx, disease_count);
            this.sublimatedMass = Mathf.Max(0.0f, this.sublimatedMass - mass2);
            this.primaryElement.Mass = Mathf.Max(0.0f, this.primaryElement.Mass - mass2);
            this.UpdateStorage();
            this.RefreshStatusItem(Sublimates.EmitState.Emitting);
            if (!flag || !this.decayStorage || !((UnityEngine.Object) storage != (UnityEngine.Object) null))
              return;
            storage.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo()
            {
              damage = 1,
              source = (string) BUILDINGS.DAMAGESOURCES.CORROSIVE_ELEMENT,
              popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CORROSIVE_ELEMENT,
              fullDamageEffectName = "smoke_damage_kanim"
            });
          }
          else
            this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
        }
        else if ((double) this.sublimatedMass > 0.0)
        {
          float mass3 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num1);
          if ((double) mass3 > 0.0 && !this.SimMightOffcellOverpressure(cell, elementByHash.id))
          {
            this.Emit(cell, mass3, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount);
            this.sublimatedMass = Mathf.Max(0.0f, this.sublimatedMass - mass3);
            this.primaryElement.Mass = Mathf.Max(0.0f, this.primaryElement.Mass - mass3);
            this.UpdateStorage();
            this.RefreshStatusItem(Sublimates.EmitState.Emitting);
          }
          else
            this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
        }
        else
        {
          if (this.primaryElement.KeepZeroMassObject)
            return;
          Util.KDestroyGameObject(this.gameObject);
        }
      }
      else
        this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
    }
  }

  private void UpdateStorage()
  {
    Pickupable component = this.GetComponent<Pickupable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.storage != (UnityEngine.Object) null))
      return;
    component.storage.Trigger(-1697596308, (object) this.gameObject);
  }

  private void Emit(int cell, float mass, float temperature, byte disease_idx, int disease_count)
  {
    SimMessages.AddRemoveSubstance(cell, this.info.sublimatedElement, CellEventLogger.Instance.SublimatesEmit, mass, temperature, disease_idx, disease_count);
    Game.Instance.accumulators.Accumulate(this.flowAccumulator, mass);
    if (this.spawnFXHash == SpawnFXHashes.None)
      return;
    this.transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    Game.Instance.SpawnFX(this.spawnFXHash, this.transform.GetPosition(), 0.0f);
  }

  public float AvgFlowRate() => Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);

  private void RefreshStatusItem(Sublimates.EmitState newEmitState)
  {
    if (newEmitState == this.lastEmitState)
      return;
    switch (newEmitState)
    {
      case Sublimates.EmitState.Emitting:
        if (this.info.sublimatedElement == SimHashes.Oxygen)
        {
          this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingOxygenAvg, (object) this);
          break;
        }
        this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingGasAvg, (object) this);
        break;
      case Sublimates.EmitState.BlockedOnPressure:
        this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedHighPressure, (object) this);
        break;
      case Sublimates.EmitState.BlockedOnTemperature:
        this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedLowTemperature, (object) this);
        break;
    }
    this.lastEmitState = newEmitState;
  }

  [Serializable]
  public struct Info(
    float rate,
    float min_amount,
    float max_destination_mass,
    float mass_power,
    SimHashes element,
    byte disease_idx = 255 /*0xFF*/,
    int disease_count = 0)
  {
    public float sublimationRate = rate;
    public float minSublimationAmount = min_amount;
    public float maxDestinationMass = max_destination_mass;
    public float massPower = mass_power;
    public byte diseaseIdx = disease_idx;
    public int diseaseCount = disease_count;
    [HashedEnum]
    public SimHashes sublimatedElement = element;
  }

  private enum EmitState
  {
    Emitting,
    BlockedOnPressure,
    BlockedOnTemperature,
  }
}
