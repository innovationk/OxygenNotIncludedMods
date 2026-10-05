// Decompiled with JetBrains decompiler
// Type: Polymerizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class Polymerizer : StateMachineComponent<Polymerizer.StatesInstance>
{
  [SerializeField]
  public float maxMass = 2.5f;
  [SerializeField]
  public float emitMass = 1f;
  [SerializeField]
  public Tag emitTag;
  [SerializeField]
  public Vector3 emitOffset = Vector3.zero;
  [SerializeField]
  public SimHashes exhaustElement = SimHashes.Vacuum;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpReq]
  private Operational operational;
  [MyCmpGet]
  private ConduitConsumer consumer;
  [MyCmpGet]
  private ElementConverter converter;
  private MeterController plasticMeter;
  private MeterController oilMeter;
  private static readonly EventSystem.IntraObjectHandler<Polymerizer> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Polymerizer>((Action<Polymerizer, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnSpawn()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    this.plasticMeter = new MeterController((KAnimControllerBase) component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0.0f, 0.0f, 0.0f), (string[]) null);
    this.oilMeter = new MeterController((KAnimControllerBase) component, "meter2_target", "meter2", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0.0f, 0.0f, 0.0f), (string[]) null);
    component.SetSymbolVisiblity((KAnimHashedString) "meter_target", true);
    this.UpdateOilMeter();
    this.smi.StartSM();
    this.Subscribe<Polymerizer>(-1697596308, Polymerizer.OnStorageChangedDelegate);
  }

  private void TryEmit()
  {
    GameObject first = this.storage.FindFirst(this.emitTag);
    if (!((UnityEngine.Object) first != (UnityEngine.Object) null))
      return;
    PrimaryElement component = first.GetComponent<PrimaryElement>();
    this.UpdatePercentDone(component);
    this.TryEmit(component);
  }

  private void TryEmit(PrimaryElement primary_elem)
  {
    if ((double) primary_elem.Mass < (double) this.emitMass)
      return;
    this.plasticMeter.SetPositionPercent(0.0f);
    GameObject gameObject = this.storage.Drop(primary_elem.gameObject, true);
    Rotatable component = this.GetComponent<Rotatable>();
    Vector3 vector3 = component.transform.GetPosition() + component.GetRotatedOffset(this.emitOffset);
    int cell = Grid.PosToCell(vector3);
    if (Grid.Solid[cell])
      vector3 += component.GetRotatedOffset(Vector3.left);
    gameObject.transform.SetPosition(vector3);
    PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.exhaustElement);
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    SimMessages.AddRemoveSubstance(Grid.PosToCell(vector3), primaryElement.ElementID, (CellAddRemoveSubstanceEvent) null, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount);
    primaryElement.Mass = 0.0f;
    primaryElement.ModifyDiseaseCount(int.MinValue, "Polymerizer.Exhaust");
  }

  private void UpdatePercentDone(PrimaryElement primary_elem)
  {
    this.plasticMeter.SetPositionPercent(Mathf.Clamp01(primary_elem.Mass / this.emitMass));
  }

  private void OnStorageChanged(object data)
  {
    GameObject go = (GameObject) data;
    if ((UnityEngine.Object) go == (UnityEngine.Object) null || !go.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
      return;
    this.UpdateOilMeter();
  }

  private void UpdateOilMeter()
  {
    float num = 0.0f;
    foreach (GameObject go in this.storage.items)
    {
      if (go.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
      {
        PrimaryElement component = go.GetComponent<PrimaryElement>();
        num += component.Mass;
      }
    }
    this.oilMeter.SetPositionPercent(Mathf.Clamp01(num / this.consumer.capacityKG));
  }

  public class StatesInstance(Polymerizer smi) : 
    GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.GameInstance(smi)
  {
  }

  public class States : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer>
  {
    public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State off;
    public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State on;
    public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State converting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.root.EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.off.EventTransition(GameHashes.OperationalChanged, this.on, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.on.EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => smi.master.converter.CanConvertAtAll()));
      this.converting.Enter("Ready", (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).EventHandler(GameHashes.OnStorageChange, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State.Callback) (smi => smi.master.TryEmit())).EventTransition(GameHashes.OnStorageChange, this.on, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => !smi.master.converter.CanConvertAtAll())).Exit("Ready", (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State.Callback) (smi => smi.master.operational.SetActive(false)));
    }
  }
}
