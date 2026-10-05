// Decompiled with JetBrains decompiler
// Type: FishFeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

#nullable disable
public class FishFeeder : 
  GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
  public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State notoperational;
  public FishFeeder.OperationalState operational;
  public static HashedString[] ballSymbols;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.notoperational;
    this.root.Enter(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.SetupFishFeederTopAndBot)).Exit(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.CleanupFishFeederTopAndBot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnStorageChange)).EventHandler(GameHashes.RefreshUserMenu, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnRefreshUserMenu));
    this.notoperational.TagTransition(GameTags.Operational, (GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State) this.operational);
    this.operational.DefaultState(this.operational.on).TagTransition(GameTags.Operational, this.notoperational, true);
    this.operational.on.DoNothing();
    int length = 19;
    FishFeeder.ballSymbols = new HashedString[length];
    for (int index = 0; index < length; ++index)
      FishFeeder.ballSymbols[index] = (HashedString) ("ball" + index.ToString());
  }

  private static void SetupFishFeederTopAndBot(FishFeeder.Instance smi)
  {
    Storage storage = smi.Get<Storage>();
    smi.fishFeederTop = new FishFeeder.FishFeederTop(smi, FishFeeder.ballSymbols, storage.Capacity());
    smi.fishFeederTop.RefreshStorage();
    smi.fishFeederBot = new FishFeeder.FishFeederBot(smi, 10f, FishFeeder.ballSymbols);
    smi.fishFeederBot.RefreshStorage();
    smi.fishFeederTop.ToggleMutantSeedFetches(smi.ForbidMutantSeeds);
    smi.UpdateMutantSeedStatusItem();
  }

  private static void CleanupFishFeederTopAndBot(FishFeeder.Instance smi)
  {
    smi.fishFeederTop.Cleanup();
  }

  private static void MoveStoredContentsToConsumeOffset(FishFeeder.Instance smi)
  {
    foreach (GameObject data in smi.GetComponent<Storage>().items)
    {
      if (!((UnityEngine.Object) data == (UnityEngine.Object) null))
        FishFeeder.OnStorageChange(smi, (object) data);
    }
  }

  private static void OnStorageChange(FishFeeder.Instance smi, object data)
  {
    if ((UnityEngine.Object) data == (UnityEngine.Object) null)
      return;
    smi.fishFeederTop.RefreshStorage();
    smi.fishFeederBot.RefreshStorage();
  }

  private static void OnRefreshUserMenu(FishFeeder.Instance smi, object data)
  {
    if (!DlcManager.FeatureRadiationEnabled())
      return;
    Game.Instance.userMenu.AddButton(smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", (string) (smi.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT), (System.Action) (() =>
    {
      smi.ForbidMutantSeeds = !smi.ForbidMutantSeeds;
      FishFeeder.OnRefreshUserMenu(smi, (object) null);
    }), tooltipText: (string) UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.FISH_FEEDER_TOOLTIP));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class OperationalState : 
    GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State
  {
    public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State on;
  }

  public new class Instance : 
    GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameInstance
  {
    private StatusItem mutantSeedStatusItem;
    public FishFeeder.FishFeederTop fishFeederTop;
    public FishFeeder.FishFeederBot fishFeederBot;
    [Serialize]
    private bool forbidMutantSeeds;

    public bool ForbidMutantSeeds
    {
      get => this.forbidMutantSeeds;
      set
      {
        this.forbidMutantSeeds = value;
        this.fishFeederTop.ToggleMutantSeedFetches(this.forbidMutantSeeds);
        this.UpdateMutantSeedStatusItem();
      }
    }

    public Instance(IStateMachineTarget master, FishFeeder.Def def)
      : base(master, def)
    {
      this.mutantSeedStatusItem = new StatusItem("FISHFEEDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettingsDelegate));
    }

    private void OnCopySettingsDelegate(object data)
    {
      GameObject go = (GameObject) data;
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
        return;
      FishFeeder.Instance smi = go.GetSMI<FishFeeder.Instance>();
      if (smi == null)
        return;
      this.ForbidMutantSeeds = smi.ForbidMutantSeeds;
    }

    public void UpdateMutantSeedStatusItem()
    {
      this.gameObject.GetComponent<KSelectable>().ToggleStatusItem(this.mutantSeedStatusItem, Game.IsDlcActiveForCurrentSave("EXPANSION1_ID") && !this.forbidMutantSeeds);
    }
  }

  public class FishFeederTop : IRenderEveryTick
  {
    private FishFeeder.Instance smi;
    private float mass;
    private float targetMass;
    private HashedString[] ballSymbols;
    private float massPerBall;
    private float timeSinceLastBallAppeared;

    public FishFeederTop(FishFeeder.Instance smi, HashedString[] ball_symbols, float capacity)
    {
      this.smi = smi;
      this.ballSymbols = ball_symbols;
      this.massPerBall = capacity / (float) ball_symbols.Length;
      this.FillFeeder(this.mass);
      SimAndRenderScheduler.instance.Add((object) this);
    }

    private void FillFeeder(float mass)
    {
      KBatchedAnimController component = this.smi.GetComponent<KBatchedAnimController>();
      for (int index = 0; index < this.ballSymbols.Length; ++index)
      {
        bool is_visible = (double) mass > (double) (index + 1) * (double) this.massPerBall;
        component.SetSymbolVisiblity((KAnimHashedString) this.ballSymbols[index], is_visible);
      }
    }

    public void RefreshStorage()
    {
      float num = 0.0f;
      foreach (GameObject gameObject in this.smi.GetComponent<Storage>().items)
      {
        if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
          num += gameObject.GetComponent<PrimaryElement>().Mass;
      }
      this.targetMass = num;
      this.timeSinceLastBallAppeared = 0.0f;
    }

    public void RenderEveryTick(float dt)
    {
      this.timeSinceLastBallAppeared += dt;
      if ((double) Mathf.Abs(this.targetMass - this.mass) <= 1.0 || (double) this.timeSinceLastBallAppeared <= 0.02500000037252903)
        return;
      this.mass += Mathf.Min(this.massPerBall, this.targetMass - this.mass);
      this.FillFeeder(this.mass);
      this.timeSinceLastBallAppeared = 0.0f;
    }

    public void Cleanup() => SimAndRenderScheduler.instance.Remove((object) this);

    public void ToggleMutantSeedFetches(bool allow)
    {
      StorageLocker component = this.smi.GetComponent<StorageLocker>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.UpdateForbiddenTag(GameTags.MutatedSeed, !allow);
    }
  }

  public class FishFeederBot
  {
    private KBatchedAnimController anim;
    private Storage topStorage;
    private Storage botStorage;
    private bool refreshingStorage;
    private FishFeeder.Instance smi;
    private float massPerBall;
    private static readonly HashedString HASH_FEEDBALL = (HashedString) "feedball";

    public FishFeederBot(FishFeeder.Instance smi, float mass_per_ball, HashedString[] ball_symbols)
    {
      this.smi = smi;
      this.massPerBall = mass_per_ball;
      this.anim = GameUtil.KInstantiate(Assets.GetPrefab((Tag) nameof (FishFeederBot)), smi.transform.GetPosition(), Grid.SceneLayer.Front).GetComponent<KBatchedAnimController>();
      this.anim.transform.SetParent(smi.transform);
      this.anim.gameObject.SetActive(true);
      this.anim.SetSceneLayer(Grid.SceneLayer.Building);
      this.anim.Play((HashedString) "ball");
      this.anim.Stop();
      foreach (HashedString ballSymbol in ball_symbols)
        this.anim.SetSymbolVisiblity((KAnimHashedString) ballSymbol, false);
      foreach (Storage component in smi.gameObject.GetComponents<Storage>())
      {
        if (component.storageID == (Tag) nameof (FishFeederBot))
          this.botStorage = component;
        else if (component.storageID == (Tag) "FishFeederTop")
          this.topStorage = component;
      }
      if (this.botStorage.IsEmpty())
        return;
      this.SetBallSymbol(this.botStorage.items[0].gameObject);
      this.anim.Play((HashedString) "ball");
    }

    public void RefreshStorage()
    {
      if (this.refreshingStorage)
        return;
      this.refreshingStorage = true;
      foreach (GameObject gameObject in this.botStorage.items)
      {
        if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
        {
          int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
          gameObject.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore));
        }
      }
      if (this.botStorage.IsEmpty())
      {
        float num = 0.0f;
        foreach (GameObject gameObject in this.topStorage.items)
        {
          if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
            num += gameObject.GetComponent<PrimaryElement>().Mass;
        }
        if ((double) num > 0.0)
        {
          Pickupable pickupable = this.topStorage.items[0].GetComponent<Pickupable>().Take(this.massPerBall);
          this.botStorage.Store(pickupable.gameObject);
          this.SetBallSymbol(pickupable.gameObject);
          this.anim.Play((HashedString) "ball");
        }
        else
          this.anim.SetSymbolVisiblity((KAnimHashedString) FishFeeder.FishFeederBot.HASH_FEEDBALL, false);
      }
      this.refreshingStorage = false;
    }

    private void SetBallSymbol(GameObject stored_go)
    {
      if ((UnityEngine.Object) stored_go == (UnityEngine.Object) null)
        return;
      this.anim.SetSymbolVisiblity((KAnimHashedString) FishFeeder.FishFeederBot.HASH_FEEDBALL, true);
      KAnim.Build build = stored_go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
      KAnim.Build.Symbol source_symbol = (build.GetSymbol((KAnimHashedString) "algae") ?? build.GetSymbol((KAnimHashedString) "object")) ?? build.GetSymbol((KAnimHashedString) build.name);
      if (source_symbol != null)
        this.anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(FishFeeder.FishFeederBot.HASH_FEEDBALL, source_symbol);
      this.anim.SetBatchGroupOverride(new HashedString(nameof (FishFeeder) + stored_go.GetComponent<KPrefabID>().PrefabTag.Name));
      int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
      stored_go.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingUse));
    }
  }
}
