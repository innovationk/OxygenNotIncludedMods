// Decompiled with JetBrains decompiler
// Type: Assets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using KMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Assets")]
public class Assets : KMonoBehaviour, ISerializationCallbackReceiver
{
  public static List<KAnimFile> ModLoadedKAnims = new List<KAnimFile>();
  private static Action<KPrefabID> OnAddPrefab;
  public static List<BuildingDef> BuildingDefs;
  public List<KPrefabID> PrefabAssets = new List<KPrefabID>();
  public static List<KPrefabID> Prefabs = new List<KPrefabID>();
  private static HashSet<Tag> CountableTags = new HashSet<Tag>();
  private static HashSet<Tag> SolidTransferArmConeyableTags = new HashSet<Tag>();
  public List<Sprite> SpriteAssets;
  public static Dictionary<HashedString, Sprite> Sprites;
  public List<string> videoClipNames;
  private const string VIDEO_ASSET_PATH = "video_webm";
  public List<TintedSprite> TintedSpriteAssets;
  public static List<TintedSprite> TintedSprites;
  public List<Texture2D> TextureAssets;
  public static List<Texture2D> Textures;
  public static List<TextureAtlas> TextureAtlases;
  public List<TextureAtlas> TextureAtlasAssets;
  public static List<Material> Materials;
  public List<Material> MaterialAssets;
  public static List<Shader> Shaders;
  public List<Shader> ShaderAssets;
  public static List<BlockTileDecorInfo> BlockTileDecorInfos;
  public List<BlockTileDecorInfo> BlockTileDecorInfoAssets;
  public Material AnimMaterialAsset;
  public static Material AnimMaterial;
  public DiseaseVisualization DiseaseVisualization;
  public Sprite LegendColourBox;
  public Texture2D invalidAreaTex;
  public List<GameObject> vistasPrefabs = new List<GameObject>();
  public Assets.UIPrefabData UIPrefabAssets;
  public static Assets.UIPrefabData UIPrefabs;
  private static Dictionary<Tag, KPrefabID> PrefabsByTag = new Dictionary<Tag, KPrefabID>();
  private static Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags = new Dictionary<Tag, List<KPrefabID>>();
  public List<KAnimFile> AnimAssets;
  public static List<KAnimFile> Anims;
  private static Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();
  public Font DebugFontAsset;
  public static Font DebugFont;
  public SubstanceTable substanceTable;
  [SerializeField]
  public TextAsset elementAudio;
  [SerializeField]
  public TextAsset personalitiesFile;
  public LogicModeUI logicModeUIData;
  public CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;
  public DigPlacerConfig.DigPlacerAssets digPlacerAssets;
  public MopPlacerConfig.MopPlacerAssets mopPlacerAssets;
  public MovePickupablePlacerConfig.MovePickupablePlacerAssets movePickupToPlacerAssets;
  public ComicData[] comics;
  public static Assets instance;
  private static Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();

  protected override void OnPrefabInit()
  {
    Assets.instance = this;
    if (KPlayerPrefs.HasKey("TemperatureUnit"))
      GameUtil.temperatureUnit = (GameUtil.TemperatureUnit) KPlayerPrefs.GetInt("TemperatureUnit");
    if (KPlayerPrefs.HasKey("MassUnit"))
      GameUtil.massUnit = (GameUtil.MassUnit) KPlayerPrefs.GetInt("MassUnit");
    RecipeManager.DestroyInstance();
    RecipeManager.Get();
    Assets.AnimMaterial = this.AnimMaterialAsset;
    Assets.Prefabs = new List<KPrefabID>(this.PrefabAssets.Where<KPrefabID>((Func<KPrefabID, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)));
    Assets.PrefabsByTag.Clear();
    Assets.PrefabsByAdditionalTags.Clear();
    Assets.CountableTags.Clear();
    Assets.Sprites = new Dictionary<HashedString, Sprite>();
    foreach (Sprite spriteAsset in this.SpriteAssets)
    {
      if (!((UnityEngine.Object) spriteAsset == (UnityEngine.Object) null))
      {
        HashedString key = new HashedString(spriteAsset.name);
        Assets.Sprites.Add(key, spriteAsset);
      }
    }
    Assets.TintedSprites = this.TintedSpriteAssets.Where<TintedSprite>((Func<TintedSprite, bool>) (x => x != null && (UnityEngine.Object) x.sprite != (UnityEngine.Object) null)).ToList<TintedSprite>();
    Assets.Materials = this.MaterialAssets.Where<Material>((Func<Material, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<Material>();
    Assets.Textures = this.TextureAssets.Where<Texture2D>((Func<Texture2D, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<Texture2D>();
    Assets.TextureAtlases = this.TextureAtlasAssets.Where<TextureAtlas>((Func<TextureAtlas, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<TextureAtlas>();
    Assets.BlockTileDecorInfos = this.BlockTileDecorInfoAssets.Where<BlockTileDecorInfo>((Func<BlockTileDecorInfo, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<BlockTileDecorInfo>();
    this.LoadAnims();
    Assets.UIPrefabs = this.UIPrefabAssets;
    Assets.DebugFont = this.DebugFontAsset;
    AsyncLoadManager<IGlobalAsyncLoader>.Run();
    GameAudioSheets.Get().Initialize();
    this.SubstanceListHookup();
    this.CreatePrefabs();
  }

  private void CreatePrefabs()
  {
    Db.Get();
    Assets.BuildingDefs = new List<BuildingDef>();
    foreach (KPrefabID prefabAsset in this.PrefabAssets)
    {
      if (!((UnityEngine.Object) prefabAsset == (UnityEngine.Object) null))
      {
        prefabAsset.InitializeTags(true);
        Assets.AddPrefab(prefabAsset);
      }
    }
    LegacyModMain.Load();
    Db.Get().PostProcess();
    ComplexRecipeManager.Get().PostProcess();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Db.Get();
  }

  private static void TryAddCountableTag(KPrefabID prefab)
  {
    foreach (Tag displayAsUnit in GameTags.DisplayAsUnits)
    {
      if (prefab.HasTag(displayAsUnit))
      {
        Assets.AddCountableTag(prefab.PrefabTag);
        break;
      }
    }
  }

  public static void AddCountableTag(Tag tag) => Assets.CountableTags.Add(tag);

  public static bool IsTagCountable(Tag tag) => Assets.CountableTags.Contains(tag);

  private static void TryAddSolidTransferArmConveyableTag(KPrefabID prefab)
  {
    if (!prefab.HasAnyTags(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
      return;
    Assets.SolidTransferArmConeyableTags.Add(prefab.PrefabTag);
  }

  public static bool IsTagSolidTransferArmConveyable(Tag tag)
  {
    return Assets.SolidTransferArmConeyableTags.Contains(tag);
  }

  private void LoadAnims()
  {
    KAnimBatchManager.DestroyInstance();
    KAnimGroupFile.DestroyInstance();
    KGlobalAnimParser.DestroyInstance();
    KAnimBatchManager.CreateInstance();
    KGlobalAnimParser.CreateInstance();
    KAnimGroupFile.LoadGroupResourceFile();
    if ((UnityEngine.Object) BundledAssetsLoader.instance.Expansion1Assets != (UnityEngine.Object) null)
      this.AnimAssets.AddRange((IEnumerable<KAnimFile>) BundledAssetsLoader.instance.Expansion1Assets.AnimAssets);
    foreach (BundledAssets dlcAssets in BundledAssetsLoader.instance.DlcAssetsList)
      this.AnimAssets.AddRange((IEnumerable<KAnimFile>) dlcAssets.AnimAssets);
    Assets.Anims = this.AnimAssets.Where<KAnimFile>((Func<KAnimFile, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<KAnimFile>();
    Assets.Anims.AddRange((IEnumerable<KAnimFile>) Assets.ModLoadedKAnims);
    Assets.AnimTable.Clear();
    foreach (KAnimFile anim in Assets.Anims)
    {
      if ((UnityEngine.Object) anim != (UnityEngine.Object) null)
      {
        HashedString name = (HashedString) anim.name;
        Assets.AnimTable[name] = anim;
      }
    }
    KAnimGroupFile.MapNamesToAnimFiles(Assets.AnimTable);
    Global.Instance.modManager.Load(Content.Animation);
    Assets.Anims.AddRange((IEnumerable<KAnimFile>) Assets.ModLoadedKAnims);
    foreach (KAnimFile modLoadedKanim in Assets.ModLoadedKAnims)
    {
      if ((UnityEngine.Object) modLoadedKanim != (UnityEngine.Object) null)
      {
        HashedString name = (HashedString) modLoadedKanim.name;
        Assets.AnimTable[name] = modLoadedKanim;
      }
    }
    Debug.Assert(Assets.AnimTable.Count > 0, (object) "Anim Assets not yet loaded");
    KAnimGroupFile.LoadAll();
    foreach (KAnimFile anim in Assets.Anims)
      anim.FinalizeLoading();
    KAnimBatchManager.Instance().CompleteInit();
  }

  private void SubstanceListHookup()
  {
    Dictionary<string, SubstanceTable> substanceTablesByDlc = new Dictionary<string, SubstanceTable>()
    {
      {
        "",
        this.substanceTable
      }
    };
    if ((UnityEngine.Object) BundledAssetsLoader.instance.Expansion1Assets != (UnityEngine.Object) null)
      substanceTablesByDlc["EXPANSION1_ID"] = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable;
    Hashtable substanceList = new Hashtable();
    ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get().entries);
    ElementLoader.Load(ref substanceList, substanceTablesByDlc);
    List<Element> all1 = ElementLoader.elements.FindAll((Predicate<Element>) (e => e.HasTag(GameTags.StartingMetalOre)));
    GameTags.StartingMetalOres = new Tag[all1.Count];
    for (int index = 0; index < all1.Count; ++index)
      GameTags.StartingMetalOres[index] = all1[index].tag;
    GameTags.BasicMetalOres = GameTags.StartingMetalOres.Append<Tag>(GameTags.BasicMetalOres);
    List<Element> all2 = ElementLoader.elements.FindAll((Predicate<Element>) (e => e.HasTag(GameTags.StartingRefinedMetal)));
    GameTags.StartingRefinedMetals = new Tag[all2.Count];
    for (int index = 0; index < all2.Count; ++index)
      GameTags.StartingRefinedMetals[index] = all2[index].tag;
    GameTags.BasicRefinedMetals = GameTags.StartingRefinedMetals.Append<Tag>(GameTags.BasicRefinedMetals);
    List<Element> all3 = ElementLoader.elements.FindAll((Predicate<Element>) (e => e.HasTag(GameTags.AnyWater)));
    PLANTS.SAFE_ELEMENTS.AllWaters = new SimHashes[all3.Count];
    for (int index = 0; index < all3.Count; ++index)
      PLANTS.SAFE_ELEMENTS.AllWaters[index] = all3[index].id;
    PLANTS.SAFE_ELEMENTS.MurkyWaters = PLANTS.SAFE_ELEMENTS.AllWaters.Append<SimHashes>(new SimHashes[1]
    {
      SimHashes.Ink
    });
  }

  public static string GetSimpleSoundEventName(EventReference event_ref)
  {
    return Assets.GetSimpleSoundEventName(KFMOD.GetEventReferencePath(event_ref));
  }

  public static string GetSimpleSoundEventName(string path)
  {
    string simpleSoundEventName = (string) null;
    if (!Assets.simpleSoundEventNames.TryGetValue(path, out simpleSoundEventName))
    {
      int num = path.LastIndexOf('/');
      simpleSoundEventName = num != -1 ? path.Substring(num + 1) : path;
      Assets.simpleSoundEventNames[path] = simpleSoundEventName;
    }
    return simpleSoundEventName;
  }

  private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id)
  {
    int count = defs.Count;
    for (int index = 0; index < count; ++index)
    {
      if (defs[index].PrefabID == prefab_id)
        return defs[index];
    }
    return (BuildingDef) null;
  }

  public static BuildingDef GetBuildingDef(string prefab_id)
  {
    return Assets.GetDef((IList<BuildingDef>) Assets.BuildingDefs, prefab_id);
  }

  public static TintedSprite GetTintedSprite(string name)
  {
    TintedSprite tintedSprite = (TintedSprite) null;
    if (Assets.TintedSprites != null)
    {
      for (int index = 0; index < Assets.TintedSprites.Count; ++index)
      {
        if (Assets.TintedSprites[index].name == name)
        {
          tintedSprite = Assets.TintedSprites[index];
          break;
        }
      }
    }
    return tintedSprite;
  }

  public static Sprite GetSprite(HashedString name)
  {
    Sprite sprite = (Sprite) null;
    if (Assets.Sprites != null)
      Assets.Sprites.TryGetValue(name, out sprite);
    return sprite;
  }

  public static VideoClip GetVideo(string name) => UnityEngine.Resources.Load<VideoClip>("video_webm/" + name);

  public static Texture2D GetTexture(string name)
  {
    Texture2D texture = (Texture2D) null;
    if (Assets.Textures != null)
    {
      for (int index = 0; index < Assets.Textures.Count; ++index)
      {
        if (Assets.Textures[index].name == name)
        {
          texture = Assets.Textures[index];
          break;
        }
      }
    }
    return texture;
  }

  public static ComicData GetComic(string id)
  {
    foreach (ComicData comic in Assets.instance.comics)
    {
      if (comic.name == id)
        return comic;
    }
    return (ComicData) null;
  }

  public static void AddPrefab(KPrefabID prefab)
  {
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return;
    prefab.InitializeTags(true);
    prefab.UpdateSaveLoadTag();
    if (Assets.PrefabsByTag.ContainsKey(prefab.PrefabTag))
    {
      Debug.LogWarning((object) ("Tried loading prefab with duplicate tag, ignoring: " + prefab.PrefabTag.ToString()));
    }
    else
    {
      Assets.PrefabsByTag[prefab.PrefabTag] = prefab;
      foreach (Tag tag in prefab.Tags)
      {
        if (!Assets.PrefabsByAdditionalTags.ContainsKey(tag))
          Assets.PrefabsByAdditionalTags[tag] = new List<KPrefabID>();
        Assets.PrefabsByAdditionalTags[tag].Add(prefab);
      }
      Assets.Prefabs.Add(prefab);
      Assets.TryAddCountableTag(prefab);
      Assets.TryAddSolidTransferArmConveyableTag(prefab);
      if (Assets.OnAddPrefab == null)
        return;
      Assets.OnAddPrefab(prefab);
    }
  }

  public static void RegisterOnAddPrefab(Action<KPrefabID> on_add)
  {
    Assets.OnAddPrefab += on_add;
    foreach (KPrefabID prefab in Assets.Prefabs)
      on_add(prefab);
  }

  public static void UnregisterOnAddPrefab(Action<KPrefabID> on_add)
  {
    Assets.OnAddPrefab -= on_add;
  }

  public static void ClearOnAddPrefab() => Assets.OnAddPrefab = (Action<KPrefabID>) null;

  public static GameObject GetPrefab(Tag tag)
  {
    GameObject prefab = Assets.TryGetPrefab(tag);
    if (!((UnityEngine.Object) prefab == (UnityEngine.Object) null))
      return prefab;
    Debug.LogWarning((object) ("Missing prefab: " + tag.ToString()));
    return prefab;
  }

  public static GameObject TryGetPrefab(Tag tag)
  {
    KPrefabID kprefabId = (KPrefabID) null;
    Assets.PrefabsByTag.TryGetValue(tag, out kprefabId);
    return !((UnityEngine.Object) kprefabId != (UnityEngine.Object) null) ? (GameObject) null : kprefabId.gameObject;
  }

  public static List<GameObject> GetPrefabsWithTag(Tag tag)
  {
    List<GameObject> prefabsWithTag = new List<GameObject>();
    if (Assets.PrefabsByAdditionalTags.ContainsKey(tag))
    {
      for (int index = 0; index < Assets.PrefabsByAdditionalTags[tag].Count; ++index)
        prefabsWithTag.Add(Assets.PrefabsByAdditionalTags[tag][index].gameObject);
    }
    return prefabsWithTag;
  }

  public static List<GameObject> GetPrefabsWithComponent<Type>()
  {
    List<GameObject> prefabsWithComponent = new List<GameObject>();
    for (int index = 0; index < Assets.Prefabs.Count; ++index)
    {
      if ((object) Assets.Prefabs[index].GetComponent<Type>() != null)
        prefabsWithComponent.Add(Assets.Prefabs[index].gameObject);
    }
    return prefabsWithComponent;
  }

  public static List<Type> GetPrefabsWithComponentAsListOfComponents<Type>()
  {
    List<Type> listOfComponents = new List<Type>();
    for (int index = 0; index < Assets.Prefabs.Count; ++index)
    {
      Type component = Assets.Prefabs[index].GetComponent<Type>();
      if ((object) component != null)
        listOfComponents.Add(component);
    }
    return listOfComponents;
  }

  public static GameObject GetPrefabWithComponent<Type>()
  {
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Type>();
    Debug.Assert(prefabsWithComponent.Count > 0, (object) ("There are no prefabs of type " + typeof (Type).Name));
    return prefabsWithComponent[0];
  }

  public static List<Tag> GetPrefabTagsWithComponent<Type>()
  {
    List<Tag> tagsWithComponent = new List<Tag>();
    for (int index = 0; index < Assets.Prefabs.Count; ++index)
    {
      if ((object) Assets.Prefabs[index].GetComponent<Type>() != null)
        tagsWithComponent.Add(Assets.Prefabs[index].PrefabID());
    }
    return tagsWithComponent;
  }

  public static Assets GetInstanceEditorOnly()
  {
    Assets[] objectsOfTypeAll = (Assets[]) UnityEngine.Resources.FindObjectsOfTypeAll(typeof (Assets));
    if (objectsOfTypeAll != null)
    {
      int length = objectsOfTypeAll.Length;
    }
    return objectsOfTypeAll[0];
  }

  public static TextureAtlas GetTextureAtlas(string name)
  {
    foreach (TextureAtlas textureAtlase in Assets.TextureAtlases)
    {
      if (textureAtlase.name == name)
        return textureAtlase;
    }
    return (TextureAtlas) null;
  }

  public static Material GetMaterial(string name)
  {
    foreach (Material material in Assets.Materials)
    {
      if (material.name == name)
        return material;
    }
    return (Material) null;
  }

  public static BlockTileDecorInfo GetBlockTileDecorInfo(string name)
  {
    foreach (BlockTileDecorInfo blockTileDecorInfo in Assets.BlockTileDecorInfos)
    {
      if (blockTileDecorInfo.name == name)
        return blockTileDecorInfo;
    }
    Debug.LogError((object) $"Could not find BlockTileDecorInfo named [{name}]");
    return (BlockTileDecorInfo) null;
  }

  public static KAnimFile GetAnim(HashedString name)
  {
    if (!name.IsValid)
    {
      Debug.LogWarning((object) "Invalid hash name");
      return (KAnimFile) null;
    }
    KAnimFile anim = (KAnimFile) null;
    Assets.AnimTable.TryGetValue(name, out anim);
    if ((UnityEngine.Object) anim == (UnityEngine.Object) null)
      Debug.LogWarning((object) $"Missing Anim: [{name.ToString()}]. You may have to run Collect Anim on the Assets prefab");
    return anim;
  }

  public static bool TryGetAnim(HashedString name, out KAnimFile anim)
  {
    if (!name.IsValid)
    {
      Debug.LogWarning((object) "Invalid hash name");
      anim = (KAnimFile) null;
      return false;
    }
    Assets.AnimTable.TryGetValue(name, out anim);
    return (UnityEngine.Object) anim != (UnityEngine.Object) null;
  }

  public void OnAfterDeserialize()
  {
    this.TintedSpriteAssets = this.TintedSpriteAssets.Where<TintedSprite>((Func<TintedSprite, bool>) (x => x != null && (UnityEngine.Object) x.sprite != (UnityEngine.Object) null)).ToList<TintedSprite>();
    this.TintedSpriteAssets.Sort((Comparison<TintedSprite>) ((a, b) => a.name.CompareTo(b.name)));
  }

  public void OnBeforeSerialize()
  {
  }

  public static void AddBuildingDef(BuildingDef def)
  {
    Assets.BuildingDefs = Assets.BuildingDefs.Where<BuildingDef>((Func<BuildingDef, bool>) (x => x.PrefabID != def.PrefabID)).ToList<BuildingDef>();
    Assets.BuildingDefs.Add(def);
  }

  [Serializable]
  public struct UIPrefabData
  {
    public ProgressBar ProgressBar;
    public HealthBar HealthBar;
    public GameObject ResourceVisualizer;
    public GameObject KAnimVisualizer;
    public Image RegionCellBlocked;
    public RectTransform PriorityOverlayIcon;
    public RectTransform HarvestWhenReadyOverlayIcon;
    public Assets.TableScreenAssets TableScreenWidgets;
  }

  [Serializable]
  public struct TableScreenAssets
  {
    public Material DefaultUIMaterial;
    public Material DesaturatedUIMaterial;
    public GameObject MinionPortrait;
    public GameObject GenericPortrait;
    public GameObject TogglePortrait;
    public GameObject ButtonLabel;
    public GameObject ButtonLabelWhite;
    public GameObject Label;
    public GameObject LabelHeader;
    public GameObject Checkbox;
    public GameObject BlankCell;
    public GameObject SuperCheckbox_Horizontal;
    public GameObject SuperCheckbox_Vertical;
    public GameObject Spacer;
    public GameObject NumericDropDown;
    public GameObject DropDownHeader;
    public GameObject PriorityGroupSelector;
    public GameObject PriorityGroupSelectorHeader;
    public GameObject PrioritizeRowWidget;
    public GameObject PrioritizeRowHeaderWidget;
  }
}
