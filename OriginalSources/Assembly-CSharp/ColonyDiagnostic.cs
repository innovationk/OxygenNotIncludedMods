// Decompiled with JetBrains decompiler
// Type: ColonyDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class ColonyDiagnostic : ISim4000ms, IHasDlcRestrictions
{
  private int clickThroughIndex;
  private List<GameObject> aggregatedUniqueClickThroughObjects = new List<GameObject>();
  public string name;
  public string id;
  public string icon = "icon_errand_operate";
  private Dictionary<string, DiagnosticCriterion> criteria = new Dictionary<string, DiagnosticCriterion>();
  public ColonyDiagnostic.PresentationSetting presentationSetting;
  private ColonyDiagnostic.DiagnosticResult latestResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.NO_DATA);
  public Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color> colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();
  public Tracker tracker;
  protected float trackerSampleCountSeconds = 4f;

  public GameObject GetNextClickThroughObject()
  {
    if (this.aggregatedUniqueClickThroughObjects.Count == 0)
      return (GameObject) null;
    this.clickThroughIndex = (this.clickThroughIndex + 1) % this.aggregatedUniqueClickThroughObjects.Count;
    return this.aggregatedUniqueClickThroughObjects[this.clickThroughIndex];
  }

  public ColonyDiagnostic(int worldID, string name)
  {
    this.worldID = worldID;
    this.name = name;
    this.id = this.GetType().Name;
    this.IsWorldModuleInterior = ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
    this.colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, Constants.NEGATIVE_COLOR);
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Bad, Constants.NEGATIVE_COLOR);
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Warning, Constants.NEGATIVE_COLOR);
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, Constants.WARNING_COLOR);
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, Constants.NEUTRAL_COLOR);
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, Constants.NEUTRAL_COLOR);
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, Constants.NEUTRAL_COLOR);
    this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Good, Constants.POSITIVE_COLOR);
    SimAndRenderScheduler.instance.Add((object) this, true);
  }

  public int worldID { get; protected set; }

  public bool IsWorldModuleInterior { get; private set; }

  public void OnCleanUp() => SimAndRenderScheduler.instance.Remove((object) this);

  public void Sim4000ms(float dt)
  {
    this.SetResult(ColonyDiagnosticUtility.IgnoreFirstUpdate ? ColonyDiagnosticUtility.NoDataResult : this.Evaluate());
  }

  public DiagnosticCriterion[] GetCriteria()
  {
    DiagnosticCriterion[] array = new DiagnosticCriterion[this.criteria.Values.Count];
    this.criteria.Values.CopyTo(array, 0);
    return array;
  }

  public ColonyDiagnostic.DiagnosticResult LatestResult
  {
    get => this.latestResult;
    private set => this.latestResult = value;
  }

  public virtual string GetAverageValueString()
  {
    return this.tracker != null ? this.tracker.FormatValueString(Mathf.Round(this.tracker.GetAverageValue(this.trackerSampleCountSeconds))) : "";
  }

  public virtual string GetCurrentValueString() => "";

  protected void AddCriterion(string id, DiagnosticCriterion criterion)
  {
    if (this.criteria.ContainsKey(id))
      return;
    criterion.SetID(id);
    this.criteria.Add(id, criterion);
  }

  public virtual ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult1 = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "");
    bool flag = false;
    if (!ClusterManager.Instance.GetWorld(this.worldID).IsDiscovered)
      return diagnosticResult1;
    this.aggregatedUniqueClickThroughObjects.Clear();
    foreach (KeyValuePair<string, DiagnosticCriterion> criterion in this.criteria)
    {
      if (ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(this.worldID, this.id, criterion.Key))
      {
        ColonyDiagnostic.DiagnosticResult diagnosticResult2 = criterion.Value.Evaluate();
        if (diagnosticResult2.opinion < diagnosticResult1.opinion || !flag && diagnosticResult2.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
        {
          flag = true;
          diagnosticResult1.opinion = diagnosticResult2.opinion;
          diagnosticResult1.Message = diagnosticResult2.Message;
          diagnosticResult1.clickThroughTarget = diagnosticResult2.clickThroughTarget;
          if (diagnosticResult2.clickThroughObjects != null)
          {
            foreach (GameObject clickThroughObject in diagnosticResult2.clickThroughObjects)
            {
              if (!this.aggregatedUniqueClickThroughObjects.Contains(clickThroughObject))
                this.aggregatedUniqueClickThroughObjects.Add(clickThroughObject);
            }
          }
        }
      }
    }
    return diagnosticResult1;
  }

  public void SetResult(ColonyDiagnostic.DiagnosticResult result) => this.LatestResult = result;

  protected string NO_MINIONS
  {
    get
    {
      return (string) (this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID);
    }
  }

  public virtual string[] GetRequiredDlcIds() => (string[]) null;

  public virtual string[] GetForbiddenDlcIds() => (string[]) null;

  public enum PresentationSetting
  {
    AverageValue,
    CurrentValue,
  }

  public struct DiagnosticResult(
    ColonyDiagnostic.DiagnosticResult.Opinion opinion,
    string message,
    Tuple<Vector3, GameObject> clickThroughTarget = null)
  {
    public ColonyDiagnostic.DiagnosticResult.Opinion opinion = opinion;
    public Tuple<Vector3, GameObject> clickThroughTarget = (Tuple<Vector3, GameObject>) null;
    public List<GameObject> clickThroughObjects = (List<GameObject>) null;
    private string message = message;

    public string Message
    {
      set => this.message = value;
      get => this.message;
    }

    public string GetFormattedMessage()
    {
      string formattedMessage;
      switch (this.opinion)
      {
        case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
          formattedMessage = $"<color={Constants.NEGATIVE_COLOR_STR}>{this.message}</color>";
          break;
        case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
          formattedMessage = $"<color={Constants.NEGATIVE_COLOR_STR}>{this.message}</color>";
          break;
        case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
          formattedMessage = $"<color={Constants.WARNING_COLOR_STR}>{this.message}</color>";
          break;
        case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
        case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
          formattedMessage = $"<color={Constants.WHITE_COLOR_STR}>{this.message}</color>";
          break;
        case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
          formattedMessage = $"<color={Constants.POSITIVE_COLOR_STR}>{this.message}</color>";
          break;
        default:
          formattedMessage = this.message;
          break;
      }
      return formattedMessage;
    }

    public enum Opinion
    {
      Unset,
      DuplicantThreatening,
      Bad,
      Warning,
      Concern,
      Suggestion,
      Tutorial,
      Normal,
      Good,
    }
  }
}
