// Decompiled with JetBrains decompiler
// Type: OxygenBreather
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
[AddComponentMenu("KMonoBehaviour/scripts/OxygenBreather")]
public class OxygenBreather : KMonoBehaviour, ISim200ms
{
  private static Tag[] cannotBreathTags = new Tag[2]
  {
    GameTags.Dead,
    GameTags.SuffocatingIncapacitated
  };
  public float O2toCO2conversion = 0.5f;
  public Vector2 mouthOffset;
  [Serialize]
  public float accumulatedCO2;
  [SerializeField]
  public float minCO2ToEmit = 0.3f;
  private bool hasAir = true;
  private Timer hasAirTimer = new Timer();
  [MyCmpAdd]
  private Notifier notifier;
  [MyCmpGet]
  private Facing facing;
  [MyCmpGet]
  private KSelectable selectable;
  private HandleVector<int>.Handle o2Accumulator = HandleVector<int>.InvalidHandle;
  private HandleVector<int>.Handle co2Accumulator = HandleVector<int>.InvalidHandle;
  private AmountInstance temperature;
  public float lowOxygenThreshold;
  public float noOxygenThreshold;
  private AttributeInstance airConsumptionRate;
  public Action<SimHashes, float, float, byte, int> onBreathableGasConsumed;
  private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<OxygenBreather>(GameTags.Dead, (Action<OxygenBreather, object>) ((component, data) => component.OnDeath(data)));
  private List<OxygenBreather.IGasProvider> gasProviders = new List<OxygenBreather.IGasProvider>();
  private Guid o2StatusItem;
  private Guid cO2StatusItem;

  public KPrefabID prefabID { private set; get; }

  public float ConsumptionRate
  {
    get => this.airConsumptionRate != null ? this.airConsumptionRate.GetTotalValue() : 0.0f;
  }

  public float CO2EmitRate => Game.Instance.accumulators.GetAverageRate(this.co2Accumulator);

  public HandleVector<int>.Handle O2Accumulator => this.o2Accumulator;

  public OxygenBreather.IGasProvider GetCurrentGasProvider()
  {
    if (this.gasProviders.Count == 0)
      return (OxygenBreather.IGasProvider) null;
    OxygenBreather.IGasProvider currentGasProvider = (OxygenBreather.IGasProvider) null;
    for (int index = this.gasProviders.Count - 1; index >= 0; --index)
    {
      OxygenBreather.IGasProvider gasProvider = this.gasProviders[index];
      if (!gasProvider.IsBlocked())
      {
        currentGasProvider = gasProvider;
        if (gasProvider.HasOxygen())
          break;
      }
    }
    return currentGasProvider;
  }

  public bool IsLowOxygen()
  {
    OxygenBreather.IGasProvider currentGasProvider = this.GetCurrentGasProvider();
    return currentGasProvider == null || currentGasProvider.IsLowOxygen();
  }

  public bool HasOxygen => this.hasAir;

  public bool IsOutOfOxygen => !this.hasAir;

  protected override void OnPrefabInit()
  {
    GameUtil.SubscribeToTags<OxygenBreather>(this, OxygenBreather.OnDeadTagAddedDelegate, true);
    this.prefabID = this.GetComponent<KPrefabID>();
  }

  protected override void OnSpawn()
  {
    this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup((Component) this);
    this.o2Accumulator = Game.Instance.accumulators.Add("O2", (KMonoBehaviour) this);
    this.co2Accumulator = Game.Instance.accumulators.Add("CO2", (KMonoBehaviour) this);
    this.o2StatusItem = this.selectable.AddStatusItem(this.gameObject.PrefabID() == (Tag) BionicMinionConfig.ID ? Db.Get().DuplicantStatusItems.BreathingO2Bionic : Db.Get().DuplicantStatusItems.BreathingO2, (object) this);
    this.cO2StatusItem = this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, (object) this);
    this.temperature = Db.Get().Amounts.Temperature.Lookup((Component) this);
    NameDisplayScreen.Instance.RegisterComponent(this.gameObject, (object) this);
  }

  private void BreathableGasConsumed(
    SimHashes elementConsumed,
    float massConsumed,
    float temperature,
    byte disseaseIDX,
    int disseaseCount)
  {
    if (this.prefabID.HasTag(GameTags.Dead) || this.O2Accumulator == HandleVector<int>.Handle.InvalidHandle)
      return;
    if (elementConsumed == SimHashes.ContaminatedOxygen)
      this.BoxingTrigger<float>(-935848905, massConsumed);
    Game.Instance.accumulators.Accumulate(this.O2Accumulator, massConsumed);
    ReportManager.Instance.ReportValueWithPrefabInstanceContext(ReportManager.ReportType.OxygenCreated, -massConsumed, this.prefabID, this.selectable.GetProperName());
    if (this.onBreathableGasConsumed == null)
      return;
    this.onBreathableGasConsumed(elementConsumed, massConsumed, temperature, disseaseIDX, disseaseCount);
  }

  public static void BreathableGasConsumed(
    OxygenBreather breather,
    SimHashes elementConsumed,
    float massConsumed,
    float temperature,
    byte disseaseIDX,
    int disseaseCount)
  {
    if (!((UnityEngine.Object) breather != (UnityEngine.Object) null))
      return;
    breather.BreathableGasConsumed(elementConsumed, massConsumed, temperature, disseaseIDX, disseaseCount);
  }

  public void Sim200ms(float dt)
  {
    if (this.prefabID.HasAnyTags(OxygenBreather.cannotBreathTags))
      return;
    float amount1 = this.airConsumptionRate.GetTotalValue() * dt;
    OxygenBreather.IGasProvider currentGasProvider = this.GetCurrentGasProvider();
    bool flag = currentGasProvider != null && currentGasProvider.ConsumeGas(this, amount1);
    if (flag)
    {
      if (currentGasProvider.ShouldEmitCO2())
      {
        if (this.cO2StatusItem != Guid.Empty)
          this.cO2StatusItem = this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, (object) this);
        float amount2 = amount1 * this.O2toCO2conversion;
        Game.Instance.accumulators.Accumulate(this.co2Accumulator, amount2);
        this.accumulatedCO2 += amount2;
        if ((double) this.accumulatedCO2 >= (double) this.minCO2ToEmit)
        {
          this.accumulatedCO2 -= this.minCO2ToEmit;
          Vector3 position1 = this.transform.GetPosition();
          Vector3 position2 = position1;
          position2.x += this.facing.GetFacing() ? -this.mouthOffset.x : this.mouthOffset.x;
          position2.y += this.mouthOffset.y;
          position2.z -= 0.5f;
          if (Mathf.FloorToInt(position2.x) != Mathf.FloorToInt(position1.x))
            position2.x = Mathf.Floor(position1.x) + (this.facing.GetFacing() ? 0.01f : 0.99f);
          CO2Manager.instance.SpawnBreath(position2, this.minCO2ToEmit, this.temperature.value, this.facing.GetFacing());
        }
      }
      else if (currentGasProvider.ShouldStoreCO2())
      {
        if (this.cO2StatusItem != Guid.Empty)
          this.cO2StatusItem = this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, (object) this);
        Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
        if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
        {
          float amount3 = amount1 * this.O2toCO2conversion;
          Game.Instance.accumulators.Accumulate(this.co2Accumulator, amount3);
          this.accumulatedCO2 += amount3;
          if ((double) this.accumulatedCO2 >= (double) this.minCO2ToEmit)
          {
            this.accumulatedCO2 -= this.minCO2ToEmit;
            equippable.GetComponent<Storage>().AddGasChunk(SimHashes.CarbonDioxide, this.minCO2ToEmit, this.temperature.value, byte.MaxValue, 0, false);
          }
        }
      }
      else if (this.cO2StatusItem != Guid.Empty)
      {
        this.selectable.RemoveStatusItem(this.cO2StatusItem);
        this.cO2StatusItem = Guid.Empty;
      }
    }
    if (flag != this.hasAir)
    {
      this.hasAirTimer.Start();
      if (!this.hasAirTimer.TryStop(2f))
        return;
      this.hasAir = flag;
      this.Trigger(-933153513, (object) BoxedBools.Box(this.hasAir));
    }
    else
      this.hasAirTimer.Stop();
  }

  public void AddGasProvider(OxygenBreather.IGasProvider gas_provider)
  {
    Debug.Assert(gas_provider != null, (object) "Error at OxygenBreather.cs  adding gas provider, the gas provider param is null!");
    Debug.Assert(!this.gasProviders.Contains(gas_provider), (object) "Error at OxygenBreather.cs adding gas provider, the gas provider was already added to the gas providers list!");
    this.gasProviders.Add(gas_provider);
    gas_provider.OnSetOxygenBreather(this);
  }

  public bool RemoveGasProvider(OxygenBreather.IGasProvider provider)
  {
    if (this.gasProviders.Count <= 0 || !this.gasProviders.Contains(provider))
      return false;
    OxygenBreather.IGasProvider gasProvider = this.gasProviders[this.gasProviders.Count - 1];
    this.gasProviders.Remove(provider);
    provider.OnClearOxygenBreather(this);
    return true;
  }

  private void OnDeath(object data)
  {
    this.enabled = false;
    this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2);
    this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.o2Accumulator);
    Game.Instance.accumulators.Remove(this.co2Accumulator);
    this.o2Accumulator = HandleVector<int>.InvalidHandle;
    this.co2Accumulator = HandleVector<int>.InvalidHandle;
    while (this.gasProviders.Count > 0)
      this.RemoveGasProvider(this.gasProviders[this.gasProviders.Count - 1]);
    base.OnCleanUp();
  }

  public interface IGasProvider
  {
    void OnSetOxygenBreather(OxygenBreather oxygen_breather);

    void OnClearOxygenBreather(OxygenBreather oxygen_breather);

    bool ConsumeGas(OxygenBreather oxygen_breather, float amount);

    bool ShouldEmitCO2();

    bool ShouldStoreCO2();

    bool IsLowOxygen();

    bool HasOxygen();

    bool IsBlocked();
  }
}
