// Decompiled with JetBrains decompiler
// Type: BuildingLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/BuildingLoader")]
public class BuildingLoader : KMonoBehaviour
{
  private GameObject previewTemplate;
  private GameObject constructionTemplate;
  public static BuildingLoader Instance;
  private static Dictionary<string, float> postConstructionMassPerUnitOverrides = new Dictionary<string, float>();

  public static void DestroyInstance() => BuildingLoader.Instance = (BuildingLoader) null;

  protected override void OnPrefabInit()
  {
    BuildingLoader.Instance = this;
    this.previewTemplate = this.CreatePreviewTemplate();
    this.constructionTemplate = this.CreateConstructionTemplate();
    Object.DontDestroyOnLoad((Object) this.previewTemplate);
    this.RegisterStandardItemMassOverrides();
  }

  private GameObject CreateTemplate()
  {
    GameObject go = new GameObject();
    go.SetActive(false);
    go.AddOrGet<KPrefabID>();
    go.AddOrGet<KSelectable>();
    go.AddOrGet<StateMachineController>();
    PrimaryElement primaryElement = go.AddOrGet<PrimaryElement>();
    primaryElement.Mass = 1f;
    primaryElement.Temperature = 293f;
    return go;
  }

  private GameObject CreatePreviewTemplate()
  {
    GameObject template = this.CreateTemplate();
    template.AddComponent<BuildingPreview>();
    return template;
  }

  private GameObject CreateConstructionTemplate()
  {
    GameObject template = this.CreateTemplate();
    template.AddOrGet<BuildingUnderConstruction>();
    template.AddOrGet<Constructable>();
    template.AddComponent<Storage>().doDiseaseTransfer = false;
    template.AddOrGet<Prioritizable>();
    template.AddOrGet<Notifier>();
    template.AddOrGet<SaveLoadRoot>();
    return template;
  }

  public GameObject CreateBuilding(BuildingDef def, GameObject go, GameObject parent = null)
  {
    go = Object.Instantiate<GameObject>(go);
    go.name = def.PrefabID;
    if ((Object) parent != (Object) null)
      go.transform.parent = parent.transform;
    go.GetComponent<Building>().Def = def;
    return go;
  }

  private static bool Add2DComponents(
    BuildingDef def,
    GameObject go,
    string initialAnimState = null,
    bool no_collider = false,
    int layer = -1)
  {
    bool required = def.AnimFiles != null && def.AnimFiles.Length != 0;
    if (layer == -1)
      layer = LayerMask.NameToLayer("Default");
    go.layer = layer;
    KBatchedAnimController[] components = go.GetComponents<KBatchedAnimController>();
    if (components.Length > 1)
    {
      for (int index = 2; index < components.Length; ++index)
        Object.DestroyImmediate((Object) components[index]);
    }
    if ((Object) def.BlockTileAtlas == (Object) null)
    {
      KBatchedAnimController kbatchedAnimController = BuildingLoader.UpdateComponentRequirement<KBatchedAnimController>(go, required);
      if ((Object) kbatchedAnimController != (Object) null)
      {
        kbatchedAnimController.AnimFiles = def.AnimFiles;
        if (def.isKAnimTile)
        {
          kbatchedAnimController.initialAnim = (string) null;
        }
        else
        {
          if (def.isUtility && initialAnimState == null)
            initialAnimState = "idle";
          else if ((Object) go.GetComponent<Door>() != (Object) null)
            initialAnimState = "closed";
          kbatchedAnimController.initialAnim = initialAnimState != null ? initialAnimState : def.DefaultAnimState;
          kbatchedAnimController.defaultAnim = kbatchedAnimController.initialAnim;
        }
        kbatchedAnimController.SetFGLayer(def.ForegroundLayer);
        kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.Default;
      }
    }
    KBoxCollider2D kboxCollider2D = BuildingLoader.UpdateComponentRequirement<KBoxCollider2D>(go, required && !no_collider);
    if ((Object) kboxCollider2D != (Object) null)
    {
      kboxCollider2D.offset = (Vector2) new Vector3(0.0f, 0.5f * (float) def.HeightInCells, 0.0f);
      kboxCollider2D.size = (Vector2) new Vector3((float) def.WidthInCells, (float) def.HeightInCells, 0.0f);
    }
    if (def.AnimFiles == null)
      Debug.LogError((object) (def.Name + " Def missing anim files"));
    return required;
  }

  private static T UpdateComponentRequirement<T>(GameObject go, bool required) where T : Component
  {
    T obj = go.GetComponent(typeof (T)) as T;
    if (!required && (Object) obj != (Object) null)
    {
      Object.DestroyImmediate((Object) obj, true);
      obj = default (T);
    }
    else if (required && (Object) obj == (Object) null)
      obj = go.AddComponent(typeof (T)) as T;
    return obj;
  }

  public static KPrefabID AddID(GameObject go, string str)
  {
    KPrefabID kprefabId = go.GetComponent<KPrefabID>();
    if ((Object) kprefabId == (Object) null)
      kprefabId = go.AddComponent<KPrefabID>();
    kprefabId.PrefabTag = new Tag(str);
    kprefabId.SaveLoadTag = kprefabId.PrefabTag;
    kprefabId.InitializeTags(true);
    return kprefabId;
  }

  public GameObject CreateBuildingUnderConstruction(BuildingDef def)
  {
    GameObject building = this.CreateBuilding(def, this.constructionTemplate);
    Object.DontDestroyOnLoad((Object) building);
    building.GetComponent<KSelectable>().SetName(def.Name);
    for (int i = 0; i < def.Mass.Length; ++i)
      building.GetComponent<PrimaryElement>().MassPerUnit += BuildingLoader.GetEffectiveMass(def, i);
    KPrefabID prefab = BuildingLoader.AddID(building, def.PrefabID + "UnderConstruction");
    prefab.AddTag(GameTags.UnderConstruction);
    prefab.SetDlcRestrictions((IHasDlcRestrictions) def);
    BuildingLoader.UpdateComponentRequirement<BuildingCellVisualizer>(building, def.CheckRequiresBuildingCellVisualizer());
    building.GetComponent<Constructable>().SetWorkTime(def.ConstructionTime);
    if (def.Cancellable)
      building.AddOrGet<Cancellable>();
    building.AddComponent<BuildingFacade>();
    Rotatable rotatable = BuildingLoader.UpdateComponentRequirement<Rotatable>(building, def.PermittedRotations != 0);
    if ((bool) (Object) rotatable)
      rotatable.permittedRotations = def.PermittedRotations;
    int layer = LayerMask.NameToLayer("Construction");
    prefab.defaultLayer = layer;
    BuildingLoader.Add2DComponents(def, building, "place", layer: layer);
    BuildingLoader.UpdateComponentRequirement<Vent>(building, false);
    bool required = (Object) def.BuildingComplete.GetComponent<AnimTileable>() != (Object) null;
    BuildingLoader.UpdateComponentRequirement<AnimTileable>(building, required);
    if (def.RequiresPowerInput && def.AddLogicPowerPort)
      GeneratedBuildings.RegisterSingleLogicInputPort(building);
    Assets.AddPrefab(prefab);
    building.PreInit();
    GeneratedBuildings.InitializeHighEnergyParticlePorts(building, def);
    GeneratedBuildings.InitializeLogicPorts(building, def);
    return building;
  }

  public GameObject CreateBuildingComplete(GameObject go, BuildingDef def)
  {
    go.name = def.PrefabID + "Complete";
    go.transform.SetPosition(new Vector3(0.0f, 0.0f, Grid.GetLayerZ(def.SceneLayer)));
    go.GetComponent<KSelectable>().SetName(def.Name);
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.MassPerUnit = 0.0f;
    for (int i = 0; i < def.Mass.Length; ++i)
      component.MassPerUnit += BuildingLoader.GetEffectiveMass(def, i);
    component.Temperature = 273.15f;
    BuildingHP buildingHp = go.AddOrGet<BuildingHP>();
    if (def.Invincible)
      buildingHp.invincible = true;
    buildingHp.SetHitPoints(def.HitPoints);
    if (def.Repairable)
      BuildingLoader.UpdateComponentRequirement<Repairable>(go, true);
    int layer = LayerMask.NameToLayer("Default");
    go.layer = layer;
    go.GetComponent<BuildingComplete>().Def = def;
    if (def.InputConduitType != ConduitType.None || def.OutputConduitType != ConduitType.None)
      go.AddComponent<BuildingConduitEndpoints>();
    if (!BuildingLoader.Add2DComponents(def, go))
      Debug.Log((object) (def.Name + " is not yet a 2d building!"));
    go.AddOrGet<BuildingFacade>();
    BuildingLoader.UpdateComponentRequirement<EnergyConsumer>(go, def.RequiresPowerInput);
    Rotatable rotatable = BuildingLoader.UpdateComponentRequirement<Rotatable>(go, def.PermittedRotations != 0);
    if ((bool) (Object) rotatable)
      rotatable.permittedRotations = def.PermittedRotations;
    if (def.Breakable)
      go.AddComponent<Breakable>();
    ConduitConsumer conduitConsumer = BuildingLoader.UpdateComponentRequirement<ConduitConsumer>(go, def.InputConduitType == ConduitType.Gas || def.InputConduitType == ConduitType.Liquid);
    if ((Object) conduitConsumer != (Object) null)
      conduitConsumer.SetConduitData(def.InputConduitType);
    bool required = def.RequiresPowerInput || def.InputConduitType == ConduitType.Gas || def.InputConduitType == ConduitType.Liquid;
    RequireInputs requireInputs = BuildingLoader.UpdateComponentRequirement<RequireInputs>(go, required);
    if ((Object) requireInputs != (Object) null)
      requireInputs.SetRequirements(def.RequiresPowerInput, def.InputConduitType == ConduitType.Gas || def.InputConduitType == ConduitType.Liquid);
    BuildingLoader.UpdateComponentRequirement<RequireOutputs>(go, def.OutputConduitType != 0);
    BuildingLoader.UpdateComponentRequirement<Operational>(go, !def.isUtility);
    if (def.Floodable)
      go.AddComponent<Floodable>();
    if (def.Disinfectable)
    {
      go.AddOrGet<AutoDisinfectable>();
      go.AddOrGet<Disinfectable>();
    }
    if (def.Overheatable)
    {
      Overheatable overheatable = go.AddComponent<Overheatable>();
      overheatable.baseOverheatTemp = def.OverheatTemperature;
      overheatable.baseFatalTemp = def.FatalHot;
    }
    if (def.Entombable)
      go.AddOrGet<Structure>();
    if (def.RequiresPowerInput && def.AddLogicPowerPort)
    {
      GeneratedBuildings.RegisterSingleLogicInputPort(go);
      go.AddOrGet<LogicOperationalController>();
    }
    BuildingLoader.UpdateComponentRequirement<BuildingCellVisualizer>(go, def.CheckRequiresBuildingCellVisualizer());
    if ((double) def.BaseDecor != 0.0)
    {
      DecorProvider decorProvider = BuildingLoader.UpdateComponentRequirement<DecorProvider>(go, true);
      decorProvider.baseDecor = def.BaseDecor;
      decorProvider.baseRadius = def.BaseDecorRadius;
    }
    if (def.AttachmentSlotTag != Tag.Invalid)
      BuildingLoader.UpdateComponentRequirement<AttachableBuilding>(go, true).attachableToTag = def.AttachmentSlotTag;
    KPrefabID prefab = BuildingLoader.AddID(go, def.PrefabID);
    prefab.defaultLayer = layer;
    Assets.AddPrefab(prefab);
    go.PreInit();
    GeneratedBuildings.InitializeHighEnergyParticlePorts(go, def);
    GeneratedBuildings.InitializeLogicPorts(go, def);
    return go;
  }

  public GameObject CreateBuildingPreview(BuildingDef def)
  {
    GameObject building = this.CreateBuilding(def, this.previewTemplate);
    Object.DontDestroyOnLoad((Object) building);
    int layer = LayerMask.NameToLayer("Place");
    building.transform.SetPosition(new Vector3(0.0f, 0.0f, Grid.GetLayerZ(def.SceneLayer)));
    BuildingLoader.Add2DComponents(def, building, "place", true, layer);
    KAnimControllerBase component1 = building.GetComponent<KAnimControllerBase>();
    if ((Object) component1 != (Object) null)
      component1.fgLayer = Grid.SceneLayer.NoLayer;
    building.AddComponent<BuildingFacade>();
    Rotatable rotatable = BuildingLoader.UpdateComponentRequirement<Rotatable>(building, def.PermittedRotations != 0);
    if ((bool) (Object) rotatable)
      rotatable.permittedRotations = def.PermittedRotations;
    KPrefabID kprefabId = BuildingLoader.AddID(building, def.PrefabID + "Preview");
    kprefabId.defaultLayer = layer;
    kprefabId.SetDlcRestrictions((IHasDlcRestrictions) def);
    building.GetComponent<KSelectable>().SetName(def.Name);
    BuildingLoader.UpdateComponentRequirement<BuildingCellVisualizer>(building, def.CheckRequiresBuildingCellVisualizer());
    KAnimGraphTileVisualizer component2 = building.GetComponent<KAnimGraphTileVisualizer>();
    if ((Object) component2 != (Object) null)
      Object.DestroyImmediate((Object) component2);
    if (def.RequiresPowerInput && def.AddLogicPowerPort)
      GeneratedBuildings.RegisterSingleLogicInputPort(building);
    building.PreInit();
    GeneratedBuildings.InitializeHighEnergyParticlePorts(building, def);
    Assets.AddPrefab(building.GetComponent<KPrefabID>());
    GeneratedBuildings.InitializeLogicPorts(building, def);
    return building;
  }

  private void RegisterStandardItemMassOverrides()
  {
    BuildingLoader.RegisterMassPerUnitOverride("BuildingGasket", 50f);
  }

  public static void RegisterMassPerUnitOverride(string materialCategory, float massPerUnit)
  {
    BuildingLoader.postConstructionMassPerUnitOverrides[materialCategory] = massPerUnit;
  }

  private static float GetEffectiveMass(BuildingDef def, int i)
  {
    float num;
    return BuildingLoader.postConstructionMassPerUnitOverrides.TryGetValue(def.MaterialCategory[i], out num) ? def.Mass[i] * num : def.Mass[i];
  }
}
