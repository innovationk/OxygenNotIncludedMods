// Decompiled with JetBrains decompiler
// Type: MissileSelectionSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MissileSelectionSideScreen : SideScreenContent
{
  private IMissileSelectionInterface targetMissileLauncher;
  [SerializeField]
  private GameObject rowPrefab;
  [SerializeField]
  private GameObject listContainer;
  [SerializeField]
  private LocText headerLabel;
  private List<Tag> ammunitiontags = new List<Tag>()
  {
    (Tag) "MissileBasic"
  };
  private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

  public override int GetSideScreenSortOrder() => 500;

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IMissileSelectionInterface>() != null || target.GetSMI<IMissileSelectionInterface>() != null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetMissileLauncher = target.GetComponent<IMissileSelectionInterface>();
    if (this.targetMissileLauncher == null)
      this.targetMissileLauncher = target.GetSMI<IMissileSelectionInterface>();
    this.Build();
  }

  private void Build()
  {
    foreach (KeyValuePair<Tag, GameObject> row in this.rows)
      Util.KDestroyGameObject(row.Value);
    this.rows.Clear();
    this.ammunitiontags = this.targetMissileLauncher.GetValidAmmunitionTags();
    this.UpdateLongRangeMissiles();
    foreach (Tag ammunitiontag in this.ammunitiontags)
    {
      GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer);
      gameObject.gameObject.name = ammunitiontag.ProperName();
      this.rows.Add(ammunitiontag, gameObject);
    }
    this.Refresh();
  }

  private void UpdateLongRangeMissiles()
  {
    if (DlcManager.IsExpansion1Active())
    {
      foreach (Tag cosmicBlastShotType in MissileLauncherConfig.CosmicBlastShotTypes)
      {
        if (!this.ammunitiontags.Contains(cosmicBlastShotType))
          this.ammunitiontags.Add(cosmicBlastShotType);
      }
    }
    else if (GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.IdHash) == null)
    {
      this.ammunitiontags.Remove((Tag) "MissileLongRange");
    }
    else
    {
      if (this.ammunitiontags.Contains((Tag) "MissileLongRange"))
        return;
      this.ammunitiontags.Add((Tag) "MissileLongRange");
    }
  }

  private void Refresh()
  {
    foreach (KeyValuePair<Tag, GameObject> row in this.rows)
    {
      KeyValuePair<Tag, GameObject> kvp = row;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite((object) kvp.Key).first;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite((object) kvp.Key).second;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = (System.Action) (() =>
      {
        this.targetMissileLauncher.ChangeAmmunition(kvp.Key, !this.targetMissileLauncher.AmmunitionIsAllowed(kvp.Key));
        this.targetMissileLauncher.OnRowToggleClick();
        DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
        this.Refresh();
      });
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.targetMissileLauncher.AmmunitionIsAllowed(kvp.Key) ? 1 : 0);
      kvp.Value.SetActive(true);
    }
  }

  public override string GetTitle() => (string) STRINGS.UI.UISIDESCREENS.MISSILESELECTIONSIDESCREEN.TITLE;
}
