// Decompiled with JetBrains decompiler
// Type: ArtifactSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ArtifactSelector : KMonoBehaviour
{
  public static ArtifactSelector Instance;
  [Serialize]
  private Dictionary<ArtifactType, List<string>> placedArtifacts = new Dictionary<ArtifactType, List<string>>();
  [Serialize]
  private int analyzedArtifactCount;
  [Serialize]
  private int analyzedSpaceArtifactCount;
  [Serialize]
  private List<string> analyzedArtifatIDs = new List<string>();
  private const string DEFAULT_ARTIFACT_ID = "artifact_officemug";

  public int AnalyzedArtifactCount => this.analyzedArtifactCount;

  public int AnalyzedSpaceArtifactCount => this.analyzedSpaceArtifactCount;

  public List<string> GetAnalyzedArtifactIDs() => this.analyzedArtifatIDs;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ArtifactSelector.Instance = this;
    this.placedArtifacts.Add(ArtifactType.Terrestrial, new List<string>());
    this.placedArtifacts.Add(ArtifactType.Space, new List<string>());
    this.placedArtifacts.Add(ArtifactType.Any, new List<string>());
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    int num1 = 0;
    int num2 = 0;
    foreach (string analyzedArtifatId in this.analyzedArtifatIDs)
    {
      switch (this.GetArtifactType(analyzedArtifatId))
      {
        case ArtifactType.Space:
          ++num2;
          continue;
        case ArtifactType.Terrestrial:
          ++num1;
          continue;
        default:
          continue;
      }
    }
    if (num1 > this.analyzedArtifactCount)
      this.analyzedArtifactCount = num1;
    if (num2 <= this.analyzedSpaceArtifactCount)
      return;
    this.analyzedSpaceArtifactCount = num2;
  }

  public bool RecordArtifactAnalyzed(string id)
  {
    if (this.analyzedArtifatIDs.Contains(id))
      return false;
    this.analyzedArtifatIDs.Add(id);
    return true;
  }

  public void IncrementAnalyzedTerrestrialArtifacts() => ++this.analyzedArtifactCount;

  public void IncrementAnalyzedSpaceArtifacts() => ++this.analyzedSpaceArtifactCount;

  public string GetUniqueArtifactID(ArtifactType artifactType = ArtifactType.Any)
  {
    List<string> stringList = new List<string>();
    foreach (string str in ArtifactConfig.artifactItems[artifactType])
    {
      if (!this.placedArtifacts[artifactType].Contains(str) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) Assets.GetPrefab(str.ToTag()).GetComponent<KPrefabID>()))
        stringList.Add(str);
    }
    string uniqueArtifactId = "artifact_officemug";
    if (stringList.Count == 0 && artifactType != ArtifactType.Any)
    {
      foreach (string str in ArtifactConfig.artifactItems[ArtifactType.Any])
      {
        if (!this.placedArtifacts[ArtifactType.Any].Contains(str) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) Assets.GetPrefab(str.ToTag()).GetComponent<KPrefabID>()))
        {
          stringList.Add(str);
          artifactType = ArtifactType.Any;
        }
      }
    }
    if (stringList.Count != 0)
      uniqueArtifactId = stringList[Random.Range(0, stringList.Count)];
    this.placedArtifacts[artifactType].Add(uniqueArtifactId);
    return uniqueArtifactId;
  }

  public void ReserveArtifactID(string artifactID, ArtifactType artifactType = ArtifactType.Any)
  {
    if (this.placedArtifacts[artifactType].Contains(artifactID))
      DebugUtil.Assert(true, $"Tried to add {artifactID} to placedArtifacts but it already exists in the list!");
    this.placedArtifacts[artifactType].Add(artifactID);
  }

  public ArtifactType GetArtifactType(string artifactID)
  {
    if (this.placedArtifacts[ArtifactType.Terrestrial].Contains(artifactID))
      return ArtifactType.Terrestrial;
    return this.placedArtifacts[ArtifactType.Space].Contains(artifactID) ? ArtifactType.Space : ArtifactType.Any;
  }
}
