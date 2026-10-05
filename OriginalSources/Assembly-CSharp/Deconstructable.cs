// Decompiled with JetBrains decompiler
// Type: Deconstructable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Deconstructable")]
public class Deconstructable : Workable
{
  public Chore chore;
  public bool allowDeconstruction = true;
  public string audioSize;
  public float customWorkTime = -1f;
  private Reconstructable reconstructable;
  [Serialize]
  private bool isMarkedForDeconstruction;
  [Serialize]
  public Tag[] constructionElements;
  public bool looseEntityDeconstructable;
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((Action<Deconstructable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((Action<Deconstructable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((Action<Deconstructable, object>) ((component, data) => component.OnDeconstruct(data)));
  private static bool _0_temp_notified = false;
  private static readonly Vector2 INITIAL_VELOCITY_RANGE = new Vector2(0.5f, 4f);
  private bool destroyed;

  private CellOffset[] placementOffsets
  {
    get
    {
      Building component1 = this.GetComponent<Building>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        CellOffset[] placementOffsets = component1.Def.PlacementOffsets;
        Rotatable component2 = component1.GetComponent<Rotatable>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          placementOffsets = new CellOffset[component1.Def.PlacementOffsets.Length];
          for (int index = 0; index < placementOffsets.Length; ++index)
            placementOffsets[index] = component2.GetRotatedCellOffset(component1.Def.PlacementOffsets[index]);
        }
        return placementOffsets;
      }
      OccupyArea component3 = this.GetComponent<OccupyArea>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        return component3.OccupiedCellsOffsets;
      if (this.looseEntityDeconstructable)
        return new CellOffset[1]{ new CellOffset(0, 0) };
      Debug.Assert(false, (object) "Ack! We put a Deconstructable on something that's neither a Building nor OccupyArea!", (UnityEngine.Object) this);
      return (CellOffset[]) null;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
    this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.minimumAttributeMultiplier = 0.75f;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
    if ((double) this.customWorkTime > 0.0)
    {
      this.SetWorkTime(this.customWorkTime);
    }
    else
    {
      Building component = this.GetComponent<Building>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.IsTilePiece)
        this.SetWorkTime(component.Def.ConstructionTime * 0.5f);
      else
        this.SetWorkTime(30f);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    CellOffset[] filter = (CellOffset[]) null;
    CellOffset[][] table = OffsetGroups.InvertedStandardTable;
    Building component = this.GetComponent<Building>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.IsTilePiece)
    {
      table = OffsetGroups.InvertedStandardTableWithCorners;
      filter = component.Def.ConstructionOffsetFilter;
    }
    this.SetOffsetTable(OffsetGroups.BuildReachabilityTable(this.placementOffsets, table, filter));
    this.Subscribe<Deconstructable>(493375141, Deconstructable.OnRefreshUserMenuDelegate);
    this.Subscribe<Deconstructable>(-111137758, Deconstructable.OnRefreshUserMenuDelegate);
    this.Subscribe<Deconstructable>(2127324410, Deconstructable.OnCancelDelegate);
    this.Subscribe<Deconstructable>(-790448070, Deconstructable.OnDeconstructDelegate);
    if (this.constructionElements == null || this.constructionElements.Length == 0)
    {
      this.constructionElements = new Tag[1];
      this.constructionElements[0] = this.GetComponent<PrimaryElement>().Element.tag;
    }
    if (this.isMarkedForDeconstruction)
      this.QueueDeconstruction(false);
    this.reconstructable = this.GetComponent<Reconstructable>();
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction);
    this.Trigger(1830962028, (object) this);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    this.Trigger(-702296337, (object) this);
    if ((UnityEngine.Object) this.reconstructable != (UnityEngine.Object) null)
      this.reconstructable.TryCommenceReconstruct();
    Building component1 = this.GetComponent<Building>();
    SimCellOccupier component2 = this.GetComponent<SimCellOccupier>();
    if ((UnityEngine.Object) DetailsScreen.Instance != (UnityEngine.Object) null && DetailsScreen.Instance.CompareTargetWith(this.gameObject))
      DetailsScreen.Instance.Show(false);
    PrimaryElement component3 = this.GetComponent<PrimaryElement>();
    float temperature = component3.Temperature;
    byte disease_idx = component3.DiseaseIdx;
    int disease_count = component3.DiseaseCount;
    if ((double) temperature <= 0.0)
    {
      temperature = component3.InternalTemperature;
      if ((double) temperature <= 0.0)
      {
        temperature = 293f;
        if (!Deconstructable._0_temp_notified)
        {
          KCrashReporter.ReportDevNotification("0 temp deconstruction", Environment.StackTrace);
          Deconstructable._0_temp_notified = true;
        }
      }
    }
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      if (component1.Def.TileLayer != ObjectLayer.NumLayers)
      {
        int cell = Grid.PosToCell(this.transform.GetPosition());
        if ((UnityEngine.Object) Grid.Objects[cell, (int) component1.Def.TileLayer] == (UnityEngine.Object) this.gameObject)
        {
          Grid.Objects[cell, (int) component1.Def.ObjectLayer] = (GameObject) null;
          Grid.Objects[cell, (int) component1.Def.TileLayer] = (GameObject) null;
          Grid.Foundation[cell] = false;
          TileVisualizer.RefreshCell(cell, component1.Def.TileLayer, component1.Def.ReplacementLayer);
        }
      }
      component2.DestroySelf((System.Action) (() => this.SpawnPopFxs(this.TriggerDestroy(temperature, disease_idx, disease_count, worker))));
    }
    else
      this.SpawnPopFxs(this.TriggerDestroy(temperature, disease_idx, disease_count));
    if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && !component1.Def.PlayConstructionSounds)
      return;
    string sound = GlobalAssets.GetSound("Finish_Deconstruction_" + (!this.audioSize.IsNullOrWhiteSpace() ? this.audioSize : component1.Def.AudioSize));
    if (sound == null)
      return;
    KMonoBehaviour.PlaySound3DAtLocation(sound, this.gameObject.transform.GetPosition());
  }

  public bool HasBeenDestroyed => this.destroyed;

  public void SpawnPopFxs(List<GameObject> items)
  {
    if (items == null)
      return;
    foreach (GameObject go in items)
    {
      string properName = go.GetProperName();
      PrimaryElement component = go.GetComponent<PrimaryElement>();
      PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) go).first, PopFXManager.Instance.sprite_Plus, $"{properName} {GameUtil.GetFormattedMass(component.Mass)}", go.transform, Vector3.zero);
    }
  }

  public List<GameObject> ForceDestroyAndGetMaterials()
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    return this.TriggerDestroy(component.Temperature, component.DiseaseIdx, component.DiseaseCount);
  }

  private List<GameObject> TriggerDestroy(
    float temperature,
    byte disease_idx,
    int disease_count,
    WorkerBase tile_worker)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || this.destroyed)
      return (List<GameObject>) null;
    if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null)
    {
      Storage component = this.transform.parent.GetComponent<Storage>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.Remove(this.gameObject);
    }
    List<GameObject> gameObjectList = this.SpawnItemsFromConstruction(temperature, disease_idx, disease_count, tile_worker);
    this.destroyed = true;
    this.gameObject.DeleteObject();
    return gameObjectList;
  }

  private List<GameObject> TriggerDestroy(float temperature, byte disease_idx, int disease_count)
  {
    return this.TriggerDestroy(temperature, disease_idx, disease_count, this.worker);
  }

  public void QueueDeconstruction(bool userTriggered)
  {
    if (userTriggered && DebugHandler.InstantBuildMode)
    {
      this.OnCompleteWork((WorkerBase) null);
    }
    else
    {
      if (this.chore != null)
        return;
      BuildingComplete component = this.GetComponent<BuildingComplete>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.ReplacementLayer != ObjectLayer.NumLayers)
      {
        int cell = Grid.PosToCell((KMonoBehaviour) component);
        if ((UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] != (UnityEngine.Object) null)
          return;
      }
      Prioritizable.AddRef(this.gameObject);
      this.chore = (Chore) new WorkChore<Deconstructable>(Db.Get().ChoreTypes.Deconstruct, (IStateMachineTarget) this, only_when_operational: false, is_preemptable: true, ignore_building_assignment: true);
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, (object) this);
      this.isMarkedForDeconstruction = true;
      this.Trigger(2108245096, (object) "Deconstruct");
    }
  }

  private void QueueDeconstruction() => this.QueueDeconstruction(true);

  private void OnDeconstruct()
  {
    if (this.chore == null)
      this.QueueDeconstruction();
    else
      this.CancelDeconstruction();
  }

  public bool IsMarkedForDeconstruction() => this.chore != null;

  public void SetAllowDeconstruction(bool allow)
  {
    this.allowDeconstruction = allow;
    if (this.allowDeconstruction)
      return;
    this.CancelDeconstruction();
  }

  public void SpawnItemsFromConstruction(WorkerBase chore_worker)
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    this.SpawnItemsFromConstruction(component.Temperature, component.DiseaseIdx, component.DiseaseCount, chore_worker);
  }

  private List<GameObject> SpawnItemsFromConstruction(
    float temperature,
    byte disease_idx,
    int disease_count,
    WorkerBase construction_worker)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    if (!this.allowDeconstruction)
      return gameObjectList;
    Building component = this.GetComponent<Building>();
    float[] numArray;
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      numArray = component.Def.Mass;
    else
      numArray = new float[1]
      {
        this.GetComponent<PrimaryElement>().Mass
      };
    for (int index = 0; index < this.constructionElements.Length && numArray.Length > index; ++index)
    {
      GameObject go = this.SpawnItem(this.transform.GetPosition(), this.constructionElements[index], numArray[index], temperature, disease_idx, disease_count, construction_worker);
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
      {
        int cell = Grid.PosToCell(go.transform.GetPosition());
        int num = Grid.CellAbove(cell);
        Vector2 initial_velocity = Grid.IsValidCell(cell) && Grid.Solid[cell] || Grid.IsValidCell(num) && Grid.Solid[num] ? Vector2.zero : new Vector2(UnityEngine.Random.Range(-1f, 1f) * Deconstructable.INITIAL_VELOCITY_RANGE.x, Deconstructable.INITIAL_VELOCITY_RANGE.y);
        if (GameComps.Fallers.Has((object) go))
          GameComps.Fallers.Remove(go);
        GameComps.Fallers.Add(go, initial_velocity);
        gameObjectList.Add(go);
      }
    }
    return gameObjectList;
  }

  public GameObject SpawnItem(
    Vector3 position,
    Tag src_element,
    float src_mass,
    float src_temperature,
    byte disease_idx,
    int disease_count,
    WorkerBase chore_worker)
  {
    GameObject go = (GameObject) null;
    int cell1 = Grid.PosToCell(position);
    CellOffset[] placementOffsets = this.placementOffsets;
    Element element = ElementLoader.GetElement(src_element);
    if (element != null)
    {
      float num = src_mass;
      for (int index1 = 0; (double) index1 < (double) src_mass / 400.0; ++index1)
      {
        int index2 = index1 % placementOffsets.Length;
        int cell2 = Grid.OffsetCell(cell1, placementOffsets[index2]);
        float mass = num;
        if ((double) num > 400.0)
        {
          mass = 400f;
          num -= 400f;
        }
        go = element.substance.SpawnResource(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Ore), mass, src_temperature, disease_idx, disease_count);
        go.Trigger(580035959, (object) chore_worker);
      }
    }
    else
    {
      for (int index3 = 0; (double) index3 < (double) src_mass; ++index3)
      {
        int index4 = index3 % placementOffsets.Length;
        int cell3 = Grid.OffsetCell(cell1, placementOffsets[index4]);
        GameObject prefab = Assets.GetPrefab(src_element);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        {
          go = GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(cell3, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore);
          go.SetActive(true);
          go.Trigger(580035959, (object) chore_worker);
        }
      }
    }
    return go;
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.allowDeconstruction)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore == null ? new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.DECONSTRUCT.NAME, new System.Action(this.OnDeconstruct), tooltipText: (string) UI.USERMENUACTIONS.DECONSTRUCT.TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.DECONSTRUCT.NAME_OFF, new System.Action(this.OnDeconstruct), tooltipText: (string) UI.USERMENUACTIONS.DECONSTRUCT.TOOLTIP_OFF), 0.0f);
  }

  public void CancelDeconstruction()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("Cancelled deconstruction");
    this.chore = (Chore) null;
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction);
    this.ShowProgressBar(false);
    this.isMarkedForDeconstruction = false;
    Prioritizable.RemoveRef(this.gameObject);
    Reconstructable component = this.GetComponent<Reconstructable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.CancelReconstructOrder();
  }

  private void OnCancel(object _) => this.CancelDeconstruction();

  private void OnDeconstruct(object data)
  {
    if (!this.allowDeconstruction && !DebugHandler.InstantBuildMode)
      return;
    this.QueueDeconstruction();
  }
}
