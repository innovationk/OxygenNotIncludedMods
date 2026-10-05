// Decompiled with JetBrains decompiler
// Type: WorldSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldSelector : KScreen, ISim4000ms
{
  public static WorldSelector Instance;
  public Dictionary<int, MultiToggle> worldRows;
  public TextStyleSetting titleTextSetting;
  public TextStyleSetting bodyTextSetting;
  public GameObject worldRowPrefab;
  public GameObject worldRowContainer;
  private Dictionary<int, ColonyDiagnostic.DiagnosticResult.Opinion> previousWorldDiagnosticStatus = new Dictionary<int, ColonyDiagnostic.DiagnosticResult.Opinion>();
  private Dictionary<int, List<GameObject>> worldStatusIcons = new Dictionary<int, List<GameObject>>();

  public static void DestroyInstance() => WorldSelector.Instance = (WorldSelector) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    WorldSelector.Instance = this;
  }

  protected override void OnSpawn()
  {
    if (!DlcManager.FeatureClusterSpaceEnabled())
    {
      this.Deactivate();
    }
    else
    {
      base.OnSpawn();
      this.worldRows = new Dictionary<int, MultiToggle>();
      this.SpawnToggles();
      this.RefreshToggles();
      Game.Instance.Subscribe(1983128072, (Action<object>) (data => this.RefreshToggles()));
      Game.Instance.Subscribe(-521212405, (Action<object>) (data => this.RefreshToggles()));
      Game.Instance.Subscribe(880851192, (Action<object>) (data => this.SortRows()));
      ClusterManager.Instance.Subscribe(-1280433810, (Action<object>) (data => this.AddWorld(data)));
      ClusterManager.Instance.Subscribe(-1078710002, (Action<object>) (data => this.RemoveWorld(data)));
      ClusterManager.Instance.Subscribe(1943181844, (Action<object>) (data => this.RefreshToggles()));
    }
  }

  private void SpawnToggles()
  {
    foreach (KeyValuePair<int, MultiToggle> worldRow in this.worldRows)
      Util.KDestroyGameObject((Component) worldRow.Value);
    this.worldRows.Clear();
    foreach (int key in ClusterManager.Instance.GetWorldIDsSorted())
    {
      MultiToggle component = Util.KInstantiateUI(this.worldRowPrefab, this.worldRowContainer).GetComponent<MultiToggle>();
      this.worldRows.Add(key, component);
      this.previousWorldDiagnosticStatus.Add(key, ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
      int id = key;
      component.onClick += (System.Action) (() => this.OnWorldRowClicked(id));
      component.GetComponentInChildren<AlertVignette>().worldID = key;
    }
  }

  private void AddWorld(object data)
  {
    int key = ((Boxed<int>) data).value;
    MultiToggle component = Util.KInstantiateUI(this.worldRowPrefab, this.worldRowContainer).GetComponent<MultiToggle>();
    this.worldRows.Add(key, component);
    this.previousWorldDiagnosticStatus.Add(key, ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
    int id = key;
    component.onClick += (System.Action) (() => this.OnWorldRowClicked(id));
    component.GetComponentInChildren<AlertVignette>().worldID = key;
    this.RefreshToggles();
  }

  private void RemoveWorld(object data)
  {
    int key = ((Boxed<int>) data).value;
    MultiToggle cmp;
    if (this.worldRows.TryGetValue(key, out cmp))
      cmp.DeleteObject();
    this.worldRows.Remove(key);
    this.previousWorldDiagnosticStatus.Remove(key);
    this.RefreshToggles();
  }

  public void OnWorldRowClicked(int id)
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(id);
    if (!((UnityEngine.Object) world != (UnityEngine.Object) null) || !world.IsDiscovered)
      return;
    CameraController.Instance.ActiveWorldStarWipe(id);
  }

  private void RefreshToggles()
  {
    foreach (KeyValuePair<int, MultiToggle> worldRow in this.worldRows)
    {
      WorldContainer world = ClusterManager.Instance.GetWorld(worldRow.Key);
      ClusterGridEntity component1 = world.GetComponent<ClusterGridEntity>();
      HierarchyReferences component2 = worldRow.Value.GetComponent<HierarchyReferences>();
      if ((UnityEngine.Object) world != (UnityEngine.Object) null)
      {
        component2.GetReference<Image>("Icon").sprite = component1.GetUISprite();
        component2.GetReference<LocText>("Label").SetText(world.GetComponent<ClusterGridEntity>().Name);
      }
      else
        component2.GetReference<Image>("Icon").sprite = Assets.GetSprite((HashedString) "unknown_far");
      if (worldRow.Key == CameraController.Instance.cameraActiveCluster)
      {
        worldRow.Value.ChangeState(1);
        worldRow.Value.gameObject.SetActive(true);
      }
      else if ((UnityEngine.Object) world != (UnityEngine.Object) null && world.IsDiscovered)
      {
        worldRow.Value.ChangeState(0);
        worldRow.Value.gameObject.SetActive(true);
      }
      else
      {
        worldRow.Value.ChangeState(0);
        worldRow.Value.gameObject.SetActive(false);
      }
      this.RefreshToggleTooltips();
      worldRow.Value.GetComponentInChildren<AlertVignette>().worldID = worldRow.Key;
    }
    this.RefreshWorldStatus();
    this.SortRows();
  }

  private void RefreshWorldStatus()
  {
    foreach (KeyValuePair<int, MultiToggle> worldRow in this.worldRows)
    {
      if (!this.worldStatusIcons.ContainsKey(worldRow.Key))
        this.worldStatusIcons.Add(worldRow.Key, new List<GameObject>());
      foreach (GameObject original in this.worldStatusIcons[worldRow.Key])
        Util.KDestroyGameObject(original);
      LocText reference = worldRow.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("StatusLabel");
      reference.SetText(ClusterManager.Instance.GetWorld(worldRow.Key).GetStatus());
      reference.color = ColonyDiagnosticScreen.GetDiagnosticIndicationColor(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(worldRow.Key));
    }
  }

  private void RefreshToggleTooltips()
  {
    int num = 0;
    List<int> asteroidIdsSorted = ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();
    foreach (KeyValuePair<int, MultiToggle> worldRow in this.worldRows)
    {
      ClusterGridEntity component1 = ClusterManager.Instance.GetWorld(worldRow.Key).GetComponent<ClusterGridEntity>();
      ToolTip component2 = worldRow.Value.GetComponent<ToolTip>();
      component2.ClearMultiStringTooltip();
      WorldContainer world = ClusterManager.Instance.GetWorld(worldRow.Key);
      if ((UnityEngine.Object) world != (UnityEngine.Object) null)
      {
        component2.AddMultiStringTooltip(component1.Name, this.titleTextSetting);
        if (!world.IsModuleInterior)
        {
          int idx = asteroidIdsSorted.IndexOf(world.id);
          if (idx != -1 && idx <= 9)
          {
            component2.AddMultiStringTooltip(" ", this.bodyTextSetting);
            if (KInputManager.currentControllerIsGamepad)
              component2.AddMultiStringTooltip(STRINGS.UI.FormatAsHotkey(GameUtil.GetActionString(this.IdxToHotkeyAction(idx))), this.bodyTextSetting);
            else
              component2.AddMultiStringTooltip(STRINGS.UI.FormatAsHotkey($"[{GameUtil.GetActionString(this.IdxToHotkeyAction(idx))}]"), this.bodyTextSetting);
          }
        }
      }
      else
        component2.AddMultiStringTooltip((string) STRINGS.UI.CLUSTERMAP.UNKNOWN_DESTINATION, this.titleTextSetting);
      if (ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(world.id) < ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
        component2.AddMultiStringTooltip(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultTooltip(world.id), this.bodyTextSetting);
      ++num;
    }
  }

  private void SortRows()
  {
    List<KeyValuePair<int, MultiToggle>> list = this.worldRows.ToList<KeyValuePair<int, MultiToggle>>();
    list.Sort((Comparison<KeyValuePair<int, MultiToggle>>) ((x, y) => (ClusterManager.Instance.GetWorld(x.Key).IsModuleInterior ? float.PositiveInfinity : ClusterManager.Instance.GetWorld(x.Key).DiscoveryTimestamp).CompareTo(ClusterManager.Instance.GetWorld(y.Key).IsModuleInterior ? float.PositiveInfinity : ClusterManager.Instance.GetWorld(y.Key).DiscoveryTimestamp)));
    for (int index = 0; index < list.Count; ++index)
      list[index].Value.transform.SetSiblingIndex(index);
    foreach (KeyValuePair<int, MultiToggle> keyValuePair1 in list)
    {
      keyValuePair1.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Indent").anchoredPosition = Vector2.zero;
      keyValuePair1.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Status").anchoredPosition = Vector2.right * 24f;
      WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair1.Key);
      if (world.ParentWorldId != world.id && world.ParentWorldId != (int) byte.MaxValue)
      {
        foreach (KeyValuePair<int, MultiToggle> keyValuePair2 in list)
        {
          if (keyValuePair2.Key == world.ParentWorldId)
          {
            int siblingIndex = keyValuePair2.Value.gameObject.transform.GetSiblingIndex();
            keyValuePair1.Value.gameObject.transform.SetSiblingIndex(siblingIndex + 1);
            keyValuePair1.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Indent").anchoredPosition = Vector2.right * 32f;
            keyValuePair1.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Status").anchoredPosition = Vector2.right * -8f;
            break;
          }
        }
      }
    }
  }

  private Action IdxToHotkeyAction(int idx)
  {
    Action hotkeyAction;
    switch (idx)
    {
      case 0:
        hotkeyAction = Action.SwitchActiveWorld1;
        break;
      case 1:
        hotkeyAction = Action.SwitchActiveWorld2;
        break;
      case 2:
        hotkeyAction = Action.SwitchActiveWorld3;
        break;
      case 3:
        hotkeyAction = Action.SwitchActiveWorld4;
        break;
      case 4:
        hotkeyAction = Action.SwitchActiveWorld5;
        break;
      case 5:
        hotkeyAction = Action.SwitchActiveWorld6;
        break;
      case 6:
        hotkeyAction = Action.SwitchActiveWorld7;
        break;
      case 7:
        hotkeyAction = Action.SwitchActiveWorld8;
        break;
      case 8:
        hotkeyAction = Action.SwitchActiveWorld9;
        break;
      case 9:
        hotkeyAction = Action.SwitchActiveWorld10;
        break;
      default:
        Debug.LogError((object) "Action must be a SwitchActiveWorld Action");
        hotkeyAction = Action.SwitchActiveWorld1;
        break;
    }
    return hotkeyAction;
  }

  public void Sim4000ms(float dt)
  {
    foreach (KeyValuePair<int, MultiToggle> worldRow in this.worldRows)
    {
      ColonyDiagnostic.DiagnosticResult.Opinion diagnosticResult = ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(worldRow.Key);
      ColonyDiagnosticScreen.SetIndication(diagnosticResult, worldRow.Value.GetComponent<HierarchyReferences>().GetReference("Indicator").gameObject);
      if (this.previousWorldDiagnosticStatus[worldRow.Key] > diagnosticResult && ClusterManager.Instance.activeWorldId != worldRow.Key)
        this.TriggerVisualNotification(worldRow.Key, diagnosticResult);
      this.previousWorldDiagnosticStatus[worldRow.Key] = diagnosticResult;
    }
    this.RefreshWorldStatus();
    this.RefreshToggleTooltips();
  }

  public void TriggerVisualNotification(
    int worldID,
    ColonyDiagnostic.DiagnosticResult.Opinion result)
  {
    foreach (KeyValuePair<int, MultiToggle> worldRow in this.worldRows)
    {
      if (worldRow.Key == worldID)
      {
        KFMOD.PlayUISound(GlobalAssets.GetSound(ColonyDiagnosticScreen.notificationSoundsInactive[result]));
        if (worldRow.Value.gameObject.activeInHierarchy)
          worldRow.Value.StartCoroutine(this.VisualNotificationRoutine(worldRow.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject, worldRow.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Indicator"), worldRow.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Spacer").gameObject));
      }
    }
  }

  private IEnumerator VisualNotificationRoutine(
    GameObject contentGameObject,
    RectTransform indicator,
    GameObject spacer)
  {
    Vector2 defaultIndicatorSize = new Vector2(8f, 8f);
    RectTransform contentRect = contentGameObject.rectTransform();
    float bounceDuration = 1f;
    float i;
    for (i = 0.0f; (double) i < (double) bounceDuration; i += Time.unscaledDeltaTime)
    {
      contentRect.anchoredPosition = Vector2.left * (Mathf.Sin(i * 3.14159274f) * 50f);
      indicator.sizeDelta = defaultIndicatorSize + Vector2.one * (float) Mathf.RoundToInt(Mathf.Sin((float) (6.0 * (3.1415927410125732 * ((double) i / (double) bounceDuration)))));
      yield return (object) 0;
    }
    for (i = 0.0f; (double) i < (double) bounceDuration; i += Time.unscaledDeltaTime)
    {
      contentRect.anchoredPosition = Vector2.left * (Mathf.Sin(i * 3.14159274f) * 25f);
      indicator.sizeDelta = defaultIndicatorSize + Vector2.one * (float) Mathf.RoundToInt(Mathf.Sin((float) (6.0 * (3.1415927410125732 * ((double) i / (double) bounceDuration)))));
      yield return (object) 0;
    }
    for (i = 0.0f; (double) i < (double) bounceDuration; i += Time.unscaledDeltaTime)
    {
      contentRect.anchoredPosition = Vector2.left * (Mathf.Sin(i * 3.14159274f) * 12f);
      indicator.sizeDelta = defaultIndicatorSize + Vector2.one * (float) Mathf.RoundToInt(Mathf.Sin((float) (6.0 * (3.1415927410125732 * ((double) i / (double) bounceDuration)))));
      yield return (object) 0;
    }
    contentRect.anchoredPosition = Vector2.zero;
    defaultIndicatorSize = new Vector2(8f, 8f);
    indicator.sizeDelta = defaultIndicatorSize;
    contentGameObject.rectTransform().localPosition = (Vector3) Vector2.zero;
  }
}
