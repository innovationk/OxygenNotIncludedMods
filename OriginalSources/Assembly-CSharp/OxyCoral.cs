// Decompiled with JetBrains decompiler
// Type: OxyCoral
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OxyCoral : 
  GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>
{
  private const float WILD_PLANTED_RATE_MODIFIER = 0.25f;
  private const string ANIM_NAME_GROW = "grow";
  private const string ANIM_NAME_IDLE = "idle_loop";
  private const string ANIM_NAME_PRODUCING_OXYGEN = "oxygen_idle_loop";
  private const string ANIM_NAME_WILTED = "wilt";
  public OxyCoral.NoProducing noProducing;
  public GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State producing;
  public GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State grow;
  private StateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.BoolParameter HasPlayedGrowAnim;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.grow;
    this.grow.ParamTransition<bool>((StateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.Parameter<bool>) this.HasPlayedGrowAnim, (GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State) this.noProducing, GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.IsTrue).PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State) this.noProducing).Exit((StateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State.Callback) (smi => smi.sm.HasPlayedGrowAnim.Set(true, smi)));
    this.noProducing.DefaultState(this.noProducing.noLight);
    this.noProducing.noLight.EventTransition(GameHashes.Uprooted, this.noProducing.dead).EventTransition(GameHashes.Wilt, this.noProducing.wilted, new StateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.Transition.ConditionCallback(OxyCoral.IsWilted)).PlayAnim("idle_loop", KAnim.PlayMode.Loop).UpdateTransition(this.producing, new Func<OxyCoral.Instance, float, bool>(OxyCoral.IsThereEnoughtLight));
    this.noProducing.wilted.TriggerOnEnter(GameHashes.PoopStationUpdate).TriggerOnExit(GameHashes.PoopStationUpdate).EventTransition(GameHashes.Uprooted, this.noProducing.dead).EventTransition(GameHashes.WiltRecover, this.noProducing.noLight, GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.Not(new StateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.Transition.ConditionCallback(OxyCoral.IsWilted))).PlayAnim("wilt");
    this.noProducing.dead.Enter((StateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State.Callback) (smi =>
    {
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
      smi.Trigger(1623392196);
      smi.DestroySelf((object) null);
    }));
    this.producing.ToggleStatusItem(Db.Get().CreatureStatusItems.BubbleGasProduction, (Func<OxyCoral.Instance, object>) (smi => (object) new Tuple<SimHashes, float>(SimHashes.Oxygen, smi.IsWild ? smi.def.OxygenProductionRate * 0.25f : smi.def.OxygenProductionRate))).EventTransition(GameHashes.Uprooted, this.noProducing.dead).EventTransition(GameHashes.Wilt, this.noProducing.wilted).UpdateTransition(this.noProducing.noLight, new Func<OxyCoral.Instance, float, bool>(OxyCoral.LightLostUpdate)).PlayAnim("oxygen_idle_loop", KAnim.PlayMode.Loop).Update(new System.Action<OxyCoral.Instance, float>(OxyCoral.ProduceOxygenUpdate), UpdateRate.SIM_1000ms);
  }

  private static bool IsWilted(OxyCoral.Instance smi) => smi.IsWilted;

  private static bool IsThereEnoughtLight(OxyCoral.Instance smi, float dt)
  {
    return smi.IsThereEnoughLight();
  }

  private static bool LightLostUpdate(OxyCoral.Instance smi, float dt) => !smi.IsThereEnoughLight();

  private static void ProduceOxygenUpdate(OxyCoral.Instance smi, float dt)
  {
    smi.ProduceOxygenUpdate(dt);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float OxygenProductionRate;
    public int MinLuxRequired;
    public CellOffset[] OutputBubbleCells;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      return new List<Descriptor>()
      {
        new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP, (object) ElementLoader.FindElementByHash(SimHashes.Oxygen).name, (object) GameUtil.GetFormattedMass(this.OxygenProductionRate, GameUtil.TimeSlice.PerSecond)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, (object) ElementLoader.FindElementByHash(SimHashes.Oxygen).name, (object) GameUtil.GetFormattedMass(this.OxygenProductionRate, GameUtil.TimeSlice.PerSecond))),
        new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.MinLuxRequired)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.MinLuxRequired)), Descriptor.DescriptorType.Requirement)
      };
    }
  }

  public class NoProducing : 
    GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State
  {
    public GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State noLight;
    public GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State wilted;
    public GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.State dead;
  }

  public new class Instance : 
    GameStateMachine<OxyCoral, OxyCoral.Instance, IStateMachineTarget, OxyCoral.Def>.GameInstance,
    IPoopStation
  {
    private PrimaryElement primaryElement;
    private WiltCondition wiltCondition;
    private ReceptacleMonitor receptacleMonitor;
    private string cachedName;
    private GameObject poopUser;

    public bool IsWild => !this.receptacleMonitor.Replanted;

    public bool IsWilted
    {
      get => (UnityEngine.Object) this.wiltCondition == (UnityEngine.Object) null || this.wiltCondition.IsWilting();
    }

    public Instance(IStateMachineTarget master, OxyCoral.Def def)
      : base(master, def)
    {
      this.receptacleMonitor = this.GetComponent<ReceptacleMonitor>();
      this.primaryElement = this.GetComponent<PrimaryElement>();
      this.wiltCondition = this.GetComponent<WiltCondition>();
      this.cachedName = master.gameObject.GetProperName();
    }

    public override void StartSM()
    {
      this.RegisterPoopStation();
      if (!this.IsWild)
        Tutorial.Instance.oxygenGenerators.Add(this.gameObject);
      base.StartSM();
    }

    public bool IsThereEnoughLight()
    {
      int num1 = 0;
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      for (int index = 0; index < this.def.OutputBubbleCells.Length; ++index)
      {
        int i = Grid.OffsetCell(cell, this.def.OutputBubbleCells[index]);
        float num2 = (float) Grid.LightIntensity[i];
        num1 = (double) num2 > (double) num1 ? (int) num2 : num1;
      }
      return num1 >= this.def.MinLuxRequired;
    }

    public void ProduceOxygenUpdate(float dt)
    {
      int gameCell = Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) this), this.def.OutputBubbleCells[UnityEngine.Random.Range(0, this.def.OutputBubbleCells.Length)]);
      float oxygenProductionRate = this.def.OxygenProductionRate;
      if (this.IsWild)
        oxygenProductionRate *= 0.25f;
      float mass = oxygenProductionRate * dt;
      if ((double) mass < 9.9999997171806854E-10)
        return;
      this.CreateOxygenBubble(gameCell, mass);
    }

    private void CreateOxygenBubble(int gameCell, float mass)
    {
      Vector3 posCcc = Grid.CellToPosCCC(gameCell, Grid.SceneLayer.BuildingFront);
      BubbleManager.instance.SpawnBubble(SimHashes.Oxygen, (Vector2) posCcc, mass, this.primaryElement.Temperature, BubbleManager.Disease.None);
      ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, mass, this.cachedName);
    }

    protected override void OnCleanUp()
    {
      Tutorial.Instance.oxygenGenerators.Remove(this.gameObject);
      this.UnregisterPoopStation();
      base.OnCleanUp();
    }

    public float GetPoopCapacity() => 10.000001f;

    public bool IsUserCompatibleWithPoopStation(KPrefabID userPrefabID)
    {
      return userPrefabID.HasTag((Tag) "ParrotFish");
    }

    public GameObject GetPoopStationObject() => this.gameObject;

    public GameObject GetCurrentPoopStationUser() => this.poopUser;

    public float GetAvailablePoopCapacityPercentage()
    {
      return this.GetAvailablePoopCapacity() / this.GetPoopCapacity();
    }

    public float GetAvailablePoopCapacity()
    {
      if (this.IsWild)
        return 0.0f;
      Storage component = this.receptacleMonitor.smi.ReceptacleObject.GetComponent<Storage>();
      float poopCapacity = this.GetPoopCapacity();
      float b = poopCapacity - component.GetMassAvailable(SimHashes.Lime);
      return Mathf.Clamp(Mathf.Min(component.capacityKg, b), 0.0f, poopCapacity);
    }

    private bool CanAcceptMorePoop() => (double) this.GetAvailablePoopCapacity() > 0.0;

    public bool IsPoopStationOperational()
    {
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.noProducing.dead))
        return false;
      return this.IsWild || this.CanAcceptMorePoop();
    }

    public string[] GetPoopingAnimNames() => (string[]) null;

    public void RegisterPoopStation()
    {
      Components.PoopStations.Add(this.gameObject.GetMyWorldId(), (IPoopStation) this);
    }

    public void UnregisterPoopStation()
    {
      Components.PoopStations.Remove(this.gameObject.GetMyWorldId(), (IPoopStation) this);
    }

    public PoopData GetPoopData()
    {
      return !this.IsWild ? new PoopData(false, this.receptacleMonitor.smi.ReceptacleObject.GetComponent<Storage>(), (string) CREATURES.POOP.PLANT_POOP_STATION_WILD, global::Def.GetUISprite((object) this.gameObject).first) : new PoopData(true, (Storage) null, (string) CREATURES.POOP.PLANT_POOP_STATION_WILD, global::Def.GetUISprite((object) this.gameObject).first);
    }

    public void PlayPoopStationAnim(string animName, KAnim.PlayMode playMode)
    {
    }

    public void ClearPoopStationUser(GameObject userRequestingClearing)
    {
      if (!((UnityEngine.Object) this.poopUser == (UnityEngine.Object) userRequestingClearing))
        return;
      this.poopUser = (GameObject) null;
      this.Trigger(-984476291);
    }

    public bool AttemptToReservePoopStation(GameObject userRequestingReserve)
    {
      if ((UnityEngine.Object) this.poopUser != (UnityEngine.Object) null && (UnityEngine.Object) this.poopUser != (UnityEngine.Object) userRequestingReserve)
        return false;
      this.poopUser = userRequestingReserve;
      return true;
    }

    public void DestroySelf(object o)
    {
      CreatureHelpers.DeselectCreature(this.gameObject);
      Util.KDestroyGameObject(this.gameObject);
    }
  }
}
