// Decompiled with JetBrains decompiler
// Type: AutoPlumberSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AutoPlumberSideScreen : SideScreenContent
{
  public KButton activateButton;
  public KButton powerButton;
  public KButton pipesButton;
  public KButton solidsButton;
  public KButton minionButton;
  private Building building;

  protected override void OnSpawn()
  {
    this.activateButton.onClick += (System.Action) (() => DevAutoPlumber.AutoPlumbBuilding(this.building));
    this.powerButton.onClick += (System.Action) (() => DevAutoPlumber.DoElectricalPlumbing(this.building));
    this.pipesButton.onClick += (System.Action) (() => DevAutoPlumber.DoLiquidAndGasPlumbing(this.building));
    this.solidsButton.onClick += (System.Action) (() => DevAutoPlumber.SetupSolidOreDelivery(this.building));
    this.minionButton.onClick += (System.Action) (() => this.SpawnMinion());
  }

  private void SpawnMinion()
  {
    MinionStartingStats minionStartingStats = new MinionStartingStats(false, isDebugMinion: true);
    GameObject prefab = Assets.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
    GameObject gameObject = Util.KInstantiate(prefab);
    gameObject.name = prefab.name;
    Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
    Vector3 pos = Grid.CellToPos(Grid.PosToCell((KMonoBehaviour) this.building), CellAlignment.Bottom, Grid.SceneLayer.Move);
    gameObject.transform.SetLocalPosition(pos);
    gameObject.SetActive(true);
    minionStartingStats.Apply(gameObject);
  }

  public override int GetSideScreenSortOrder() => -150;

  public override bool IsValidForTarget(GameObject target)
  {
    return DebugHandler.InstantBuildMode && (UnityEngine.Object) target.GetComponent<Building>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    this.building = target.GetComponent<Building>();
  }

  public override void ClearTarget()
  {
  }
}
