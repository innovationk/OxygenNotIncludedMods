// Decompiled with JetBrains decompiler
// Type: SubSpeciesInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SubSpeciesInfoScreen : KModalScreen
{
  [SerializeField]
  private KButton renameButton;
  [SerializeField]
  private KButton saveButton;
  [SerializeField]
  private KButton discardButton;
  [SerializeField]
  private RectTransform mutationsList;
  [SerializeField]
  private Image plantIcon;
  [SerializeField]
  private GameObject mutationsItemPrefab;
  private List<GameObject> mutationLineItems = new List<GameObject>();
  private GeneticAnalysisStation targetStation;

  public override bool IsModal() => true;

  protected override void OnSpawn() => base.OnSpawn();

  private void ClearMutations()
  {
    for (int index = this.mutationLineItems.Count - 1; index >= 0; --index)
      Util.KDestroyGameObject(this.mutationLineItems[index]);
    this.mutationLineItems.Clear();
  }

  public void DisplayDiscovery(Tag speciesID, Tag subSpeciesID, GeneticAnalysisStation station)
  {
    this.SetSubspecies(speciesID, subSpeciesID);
    this.targetStation = station;
  }

  private void SetSubspecies(Tag speciesID, Tag subSpeciesID)
  {
    this.ClearMutations();
    PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(speciesID, subSpeciesID);
    this.plantIcon.sprite = Def.GetUISprite((object) Assets.GetPrefab(speciesID)).first;
    foreach (string mutationId in subSpecies.mutationIDs)
    {
      PlantMutation plantMutation = Db.Get().PlantMutations.Get(mutationId);
      GameObject gameObject = Util.KInstantiateUI(this.mutationsItemPrefab, this.mutationsList.gameObject, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("nameLabel").text = plantMutation.Name;
      component.GetReference<LocText>("descriptionLabel").text = plantMutation.description;
      this.mutationLineItems.Add(gameObject);
    }
  }
}
