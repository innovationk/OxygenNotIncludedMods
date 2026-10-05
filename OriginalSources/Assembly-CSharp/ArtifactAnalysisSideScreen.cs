// Decompiled with JetBrains decompiler
// Type: ArtifactAnalysisSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ArtifactAnalysisSideScreen : SideScreenContent
{
  [SerializeField]
  private GameObject rowPrefab;
  private GameObject targetArtifactStation;
  [SerializeField]
  private GameObject rowContainer;
  private Dictionary<string, GameObject> rows = new Dictionary<string, GameObject>();
  private GameObject undiscoveredRow;

  public override string GetTitle()
  {
    return (UnityEngine.Object) this.targetArtifactStation != (UnityEngine.Object) null ? string.Format(base.GetTitle(), (object) this.targetArtifactStation.GetProperName()) : base.GetTitle();
  }

  public override void ClearTarget()
  {
    this.targetArtifactStation = (GameObject) null;
    base.ClearTarget();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetSMI<ArtifactAnalysisStation.StatesInstance>() != null;
  }

  private void RefreshRows()
  {
    if ((UnityEngine.Object) this.undiscoveredRow == (UnityEngine.Object) null)
    {
      this.undiscoveredRow = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
      HierarchyReferences component = this.undiscoveredRow.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("label").SetText((string) STRINGS.UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED);
      component.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED_TOOLTIP);
      component.GetReference<Image>("icon").sprite = Assets.GetSprite((HashedString) "unknown");
      component.GetReference<Image>("icon").color = Color.grey;
    }
    List<string> analyzedArtifactIds = ArtifactSelector.Instance.GetAnalyzedArtifactIDs();
    this.undiscoveredRow.SetActive(analyzedArtifactIds.Count == 0);
    foreach (string str in analyzedArtifactIds)
    {
      if (!this.rows.ContainsKey(str))
      {
        GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
        this.rows.Add(str, gameObject);
        GameObject artifactPrefab = Assets.GetPrefab((Tag) str);
        HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
        component.GetReference<LocText>("label").SetText(artifactPrefab.GetProperName());
        component.GetReference<Image>("icon").sprite = Def.GetUISprite((object) artifactPrefab, str).first;
        component.GetComponent<KButton>().onClick += (System.Action) (() => this.OpenEvent(artifactPrefab));
      }
    }
  }

  private void OpenEvent(GameObject artifactPrefab)
  {
    SimpleEvent.StatesInstance smi = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.ArtifactReveal).smi as SimpleEvent.StatesInstance;
    smi.artifact = artifactPrefab;
    artifactPrefab.GetComponent<KPrefabID>();
    artifactPrefab.GetComponent<InfoDescription>();
    string key = $"STRINGS.UI.SPACEARTIFACTS.{artifactPrefab.PrefabID().Name.ToUpper().Replace("ARTIFACT_", "")}.ARTIFACT";
    string str = $"<b>{artifactPrefab.GetProperName()}</b>";
    StringEntry stringEntry;
    ref StringEntry local = ref stringEntry;
    Strings.TryGet(key, out local);
    if (stringEntry != null && !stringEntry.String.IsNullOrWhiteSpace())
      str = $"{str}\n\n{stringEntry.String}";
    if (str != null && !str.IsNullOrWhiteSpace())
      smi.SetTextParameter("desc", str);
    smi.ShowEventPopup();
  }

  public override void SetTarget(GameObject target)
  {
    this.targetArtifactStation = target;
    base.SetTarget(target);
    this.RefreshRows();
  }
}
