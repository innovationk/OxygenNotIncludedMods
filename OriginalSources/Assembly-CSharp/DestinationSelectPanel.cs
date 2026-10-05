// Decompiled with JetBrains decompiler
// Type: DestinationSelectPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/DestinationSelectPanel")]
public class DestinationSelectPanel : KMonoBehaviour
{
  [SerializeField]
  private GameObject asteroidPrefab;
  [SerializeField]
  private KButtonDrag dragTarget;
  [SerializeField]
  private MultiToggle leftArrowButton;
  [SerializeField]
  private MultiToggle rightArrowButton;
  [SerializeField]
  private RectTransform asteroidContainer;
  [SerializeField]
  private float asteroidFocusScale = 2f;
  [SerializeField]
  private float asteroidXSeparation = 240f;
  [SerializeField]
  private float focusScaleSpeed = 0.5f;
  [SerializeField]
  private float centeringSpeed = 0.5f;
  [SerializeField]
  private GameObject moonContainer;
  [SerializeField]
  private GameObject moonPrefab;
  private static int chosenClusterCategorySetting;
  private float offset;
  private int selectedIndex = -1;
  private List<DestinationAsteroid2> asteroids = new List<DestinationAsteroid2>();
  private int numAsteroids;
  private List<string> clusterKeys;
  private Dictionary<string, string> clusterStartWorlds;
  private Dictionary<string, ColonyDestinationAsteroidBeltData> asteroidData = new Dictionary<string, ColonyDestinationAsteroidBeltData>();
  private Vector2 dragStartPos;
  private Vector2 dragLastPos;
  private bool isDragging;
  private const string debugFmt = "{world}: {seed} [{traits}] {{settings}}";

  public static int ChosenClusterCategorySetting
  {
    get => DestinationSelectPanel.chosenClusterCategorySetting;
    set => DestinationSelectPanel.chosenClusterCategorySetting = value;
  }

  public event Action<ColonyDestinationAsteroidBeltData> OnAsteroidClicked;

  private float min => this.asteroidContainer.rect.x + this.offset;

  private float max => this.min + this.asteroidContainer.rect.width;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.dragTarget.onBeginDrag += new System.Action(this.BeginDrag);
    this.dragTarget.onDrag += new System.Action(this.Drag);
    this.dragTarget.onEndDrag += new System.Action(this.EndDrag);
    this.leftArrowButton.onClick += new System.Action(this.ClickLeft);
    this.rightArrowButton.onClick += new System.Action(this.ClickRight);
  }

  private void BeginDrag()
  {
    this.dragStartPos = (Vector2) KInputManager.GetMousePos();
    this.dragLastPos = this.dragStartPos;
    this.isDragging = true;
    KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Start"));
  }

  private void Drag()
  {
    Vector2 mousePos = (Vector2) KInputManager.GetMousePos();
    float num = mousePos.x - this.dragLastPos.x;
    this.dragLastPos = mousePos;
    this.offset += num;
    int selectedIndex1 = this.selectedIndex;
    this.selectedIndex = Mathf.RoundToInt(-this.offset / this.asteroidXSeparation);
    this.selectedIndex = Mathf.Clamp(this.selectedIndex, 0, this.clusterStartWorlds.Count - 1);
    int selectedIndex2 = this.selectedIndex;
    if (selectedIndex1 == selectedIndex2)
      return;
    this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
    KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll"));
  }

  private void EndDrag()
  {
    this.Drag();
    this.isDragging = false;
    KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Stop"));
  }

  private void ClickLeft()
  {
    this.selectedIndex = Mathf.Clamp(this.selectedIndex - 1, 0, this.clusterKeys.Count - 1);
    this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
  }

  private void ClickRight()
  {
    this.selectedIndex = Mathf.Clamp(this.selectedIndex + 1, 0, this.clusterKeys.Count - 1);
    this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
  }

  public void Init()
  {
    this.clusterKeys = new List<string>();
    this.clusterStartWorlds = new Dictionary<string, string>();
    this.UpdateDisplayedClusters();
  }

  public void Uninit()
  {
  }

  private void Update()
  {
    if (!this.isDragging)
    {
      float num1 = this.offset + (float) this.selectedIndex * this.asteroidXSeparation;
      float num2 = 0.0f;
      if ((double) num1 != 0.0)
        num2 = -num1;
      float b = Mathf.Clamp(num2, (float) (-(double) this.asteroidXSeparation * 2.0), this.asteroidXSeparation * 2f);
      if ((double) b != 0.0)
      {
        float a = this.centeringSpeed * Time.unscaledDeltaTime;
        float num3 = b * this.centeringSpeed * Time.unscaledDeltaTime;
        if ((double) num3 > 0.0 && (double) num3 < (double) a)
          num3 = Mathf.Min(a, b);
        else if ((double) num3 < 0.0 && (double) num3 > -(double) a)
          num3 = Mathf.Max(-a, b);
        this.offset += num3;
      }
    }
    UnityEngine.Rect rect = this.asteroidContainer.rect;
    float x1 = rect.min.x;
    rect = this.asteroidContainer.rect;
    float x2 = rect.max.x;
    this.offset = Mathf.Clamp(this.offset, (float) -(this.clusterStartWorlds.Count - 1) * this.asteroidXSeparation + x1, x2);
    this.RePlaceAsteroids();
    for (int index = 0; index < this.moonContainer.transform.childCount; ++index)
      this.moonContainer.transform.GetChild(index).GetChild(0).SetLocalPosition(new Vector3(0.0f, (float) (1.5 + 3.0 * (double) Mathf.Sin((float) (((double) index + (double) Time.realtimeSinceStartup) * 1.25))), 0.0f));
  }

  public void UpdateDisplayedClusters()
  {
    this.clusterKeys.Clear();
    this.clusterStartWorlds.Clear();
    this.asteroidData.Clear();
    foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
    {
      if ((!DlcManager.FeatureClusterSpaceEnabled() || !(keyValuePair.Key == "clusters/SandstoneDefault")) && keyValuePair.Value.clusterCategory == (ClusterLayout.ClusterCategory) DestinationSelectPanel.ChosenClusterCategorySetting)
      {
        this.clusterKeys.Add(keyValuePair.Key);
        ColonyDestinationAsteroidBeltData asteroidBeltData = new ColonyDestinationAsteroidBeltData(keyValuePair.Value.GetStartWorld(), 0, keyValuePair.Key);
        this.asteroidData[keyValuePair.Key] = asteroidBeltData;
        this.clusterStartWorlds.Add(keyValuePair.Key, keyValuePair.Value.GetStartWorld());
      }
    }
    this.clusterKeys.Sort((Comparison<string>) ((a, b) => SettingsCache.clusterLayouts.clusterCache[a].menuOrder.CompareTo(SettingsCache.clusterLayouts.clusterCache[b].menuOrder)));
  }

  [ContextMenu("RePlaceAsteroids")]
  public void RePlaceAsteroids()
  {
    this.BeginAsteroidDrawing();
    for (int index = 0; index < this.clusterKeys.Count; ++index)
    {
      float x = this.offset + (float) index * this.asteroidXSeparation;
      string clusterKey = this.clusterKeys[index];
      float iconScale = this.asteroidData[clusterKey].GetStartWorld.iconScale;
      this.GetAsteroid(clusterKey, index == this.selectedIndex ? this.asteroidFocusScale * iconScale : iconScale).transform.SetLocalPosition(new Vector3(x, index == this.selectedIndex ? (float) (5.0 + 10.0 * (double) Mathf.Sin(Time.realtimeSinceStartup * 1f)) : 0.0f, 0.0f));
    }
    this.EndAsteroidDrawing();
  }

  private void BeginAsteroidDrawing() => this.numAsteroids = 0;

  private void ShowMoons(ColonyDestinationAsteroidBeltData asteroid)
  {
    if (asteroid.worlds.Count > 0)
    {
      while (this.moonContainer.transform.childCount < asteroid.worlds.Count)
        UnityEngine.Object.Instantiate<GameObject>(this.moonPrefab, this.moonContainer.transform);
      for (int index1 = 0; index1 < asteroid.worlds.Count; ++index1)
      {
        KBatchedAnimController componentInChildren = this.moonContainer.transform.GetChild(index1).GetComponentInChildren<KBatchedAnimController>();
        int index2 = (index1 - 1 + asteroid.worlds.Count / 2) % asteroid.worlds.Count;
        ProcGen.World world = asteroid.worlds[index2];
        KAnimFile anim = Assets.GetAnim((HashedString) (world.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : world.asteroidIcon));
        if ((UnityEngine.Object) anim != (UnityEngine.Object) null)
        {
          componentInChildren.SetVisiblity(true);
          componentInChildren.SwapAnims(new KAnimFile[1]
          {
            anim
          });
          componentInChildren.initialMode = KAnim.PlayMode.Loop;
          componentInChildren.initialAnim = "idle_loop";
          componentInChildren.gameObject.SetActive(true);
          if (componentInChildren.HasAnimation((HashedString) componentInChildren.initialAnim))
            componentInChildren.Play((HashedString) componentInChildren.initialAnim, KAnim.PlayMode.Loop);
          componentInChildren.transform.parent.gameObject.SetActive(true);
        }
      }
      for (int count = asteroid.worlds.Count; count < this.moonContainer.transform.childCount; ++count)
      {
        KBatchedAnimController componentInChildren = this.moonContainer.transform.GetChild(count).GetComponentInChildren<KBatchedAnimController>();
        if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
          componentInChildren.SetVisiblity(false);
        this.moonContainer.transform.GetChild(count).gameObject.SetActive(false);
      }
    }
    else
    {
      foreach (KBatchedAnimController componentsInChild in this.moonContainer.GetComponentsInChildren<KBatchedAnimController>())
        componentsInChild.SetVisiblity(false);
    }
  }

  private DestinationAsteroid2 GetAsteroid(string name, float scale)
  {
    DestinationAsteroid2 asteroid;
    if (this.numAsteroids < this.asteroids.Count)
    {
      asteroid = this.asteroids[this.numAsteroids];
    }
    else
    {
      asteroid = Util.KInstantiateUI<DestinationAsteroid2>(this.asteroidPrefab, this.asteroidContainer.gameObject);
      asteroid.OnClicked += this.OnAsteroidClicked;
      this.asteroids.Add(asteroid);
    }
    asteroid.SetAsteroid(this.asteroidData[name]);
    this.asteroidData[name].TargetScale = scale;
    this.asteroidData[name].Scale += (this.asteroidData[name].TargetScale - this.asteroidData[name].Scale) * this.focusScaleSpeed * Time.unscaledDeltaTime;
    asteroid.transform.localScale = Vector3.one * this.asteroidData[name].Scale;
    ++this.numAsteroids;
    return asteroid;
  }

  private void EndAsteroidDrawing()
  {
    for (int index = 0; index < this.asteroids.Count; ++index)
      this.asteroids[index].gameObject.SetActive(index < this.numAsteroids);
  }

  public ColonyDestinationAsteroidBeltData SelectCluster(string name, int seed)
  {
    this.selectedIndex = this.clusterKeys.IndexOf(name);
    this.asteroidData[name].ReInitialize(seed);
    return this.asteroidData[name];
  }

  public string GetDefaultAsteroid()
  {
    foreach (string clusterKey in this.clusterKeys)
    {
      if (this.asteroidData[clusterKey].Layout.menuOrder == 0)
        return clusterKey;
    }
    return this.clusterKeys.First<string>();
  }

  public ColonyDestinationAsteroidBeltData SelectDefaultAsteroid(int seed)
  {
    this.selectedIndex = 0;
    string key = this.asteroidData.Keys.First<string>();
    this.asteroidData[key].ReInitialize(seed);
    return this.asteroidData[key];
  }

  public void ScrollLeft()
  {
    this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[Mathf.Max(this.selectedIndex - 1, 0)]]);
  }

  public void ScrollRight()
  {
    this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[Mathf.Min(this.selectedIndex + 1, this.clusterStartWorlds.Count - 1)]]);
  }

  private void DebugCurrentSetting()
  {
    ColonyDestinationAsteroidBeltData asteroidBeltData = this.asteroidData[this.clusterKeys[this.selectedIndex]];
    string str1 = "{world}: {seed} [{traits}] {{settings}}";
    string startWorldName = asteroidBeltData.startWorldName;
    string newValue1 = asteroidBeltData.seed.ToString();
    string str2 = str1.Replace("{world}", startWorldName).Replace("{seed}", newValue1);
    List<AsteroidDescriptor> traitDescriptors = asteroidBeltData.GetTraitDescriptors();
    string[] strArray = new string[traitDescriptors.Count];
    for (int index = 0; index < traitDescriptors.Count; ++index)
      strArray[index] = traitDescriptors[index].text;
    string newValue2 = string.Join(", ", strArray);
    string str3 = str2.Replace("{traits}", newValue2);
    switch (CustomGameSettings.Instance.customGameMode)
    {
      case CustomGameSettings.CustomGameMode.Survival:
        str3 = str3.Replace("{settings}", "Survival");
        break;
      case CustomGameSettings.CustomGameMode.Nosweat:
        str3 = str3.Replace("{settings}", "Nosweat");
        break;
      case CustomGameSettings.CustomGameMode.Custom:
        List<string> stringList = new List<string>();
        foreach (KeyValuePair<string, SettingConfig> qualitySetting in CustomGameSettings.Instance.QualitySettings)
        {
          if (qualitySetting.Value.coordinate_range >= 0L)
          {
            SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(qualitySetting.Key);
            if (currentQualitySetting.id != qualitySetting.Value.GetDefaultLevelId())
              stringList.Add($"{qualitySetting.Value.label}={currentQualitySetting.label}");
          }
        }
        str3 = str3.Replace("{settings}", string.Join(", ", stringList.ToArray()));
        break;
    }
    Debug.Log((object) str3);
  }
}
