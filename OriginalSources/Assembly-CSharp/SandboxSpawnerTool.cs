// Decompiled with JetBrains decompiler
// Type: SandboxSpawnerTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxSpawnerTool : InterfaceTool
{
  protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
  private int currentCell;
  private string soundPath = GlobalAssets.GetSound("SandboxTool_Spawner");
  [SerializeField]
  private GameObject fxPrefab;

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    this.currentCell = Grid.PosToCell(cursorPos);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    this.Place(Grid.PosToCell(cursor_pos));
  }

  private void Place(int cell)
  {
    if (!Grid.IsValidBuildingCell(cell))
      return;
    string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
    GameObject prefab = Assets.GetPrefab((Tag) stringSetting);
    if (prefab.HasTag(GameTags.BaseMinion))
      this.SpawnMinion(stringSetting);
    else if ((Object) prefab.GetComponent<Building>() != (Object) null)
    {
      BuildingDef def = prefab.GetComponent<Building>().Def;
      def.Build(cell, Orientation.Neutral, (Storage) null, (IList<Tag>) def.DefaultElements(), 298.15f);
    }
    else
    {
      KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
      Grid.SceneLayer sceneLayer = (Object) component == (Object) null ? Grid.SceneLayer.Creatures : component.sceneLayer;
      GameObject go = GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(this.currentCell, sceneLayer), sceneLayer);
      if ((Object) go.GetComponent<Pickupable>() != (Object) null && !go.HasTag(GameTags.Creature))
        go.transform.position += Vector3.up * (Grid.CellSizeInMeters / 3f);
      if ((Object) go.GetComponent<ElementChunk>() != (Object) null)
      {
        go.GetComponent<PrimaryElement>().Mass = 100f;
        go.GetComponent<PrimaryElement>().Temperature = prefab.GetComponent<PrimaryElement>().Element.defaultValues.temperature;
      }
      go.SetActive(true);
    }
    GameUtil.KInstantiate(this.fxPrefab, Grid.CellToPosCCC(this.currentCell, Grid.SceneLayer.FXFront), Grid.SceneLayer.FXFront).GetComponent<KAnimControllerBase>().Play((HashedString) "placer");
    KFMOD.PlayUISound(this.soundPath);
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.entitySelector.row.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
  }

  private void SpawnMinion(string prefabID)
  {
    GameObject prefab = Assets.GetPrefab((Tag) prefabID);
    Tag model = (Tag) prefabID;
    GameObject gameObject = Util.KInstantiate(prefab);
    gameObject.name = prefab.name;
    Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
    Vector3 posCbc = Grid.CellToPosCBC(this.currentCell, Grid.SceneLayer.Move);
    gameObject.transform.SetLocalPosition(posCbc);
    gameObject.SetActive(true);
    new MinionStartingStats(model, false).Apply(gameObject);
    gameObject.GetMyWorld().SetDupeVisited();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.SandboxCopyElement))
    {
      int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
      List<ObjectLayer> objectLayerList = new List<ObjectLayer>();
      objectLayerList.Add(ObjectLayer.Pickupables);
      objectLayerList.Add(ObjectLayer.Plants);
      objectLayerList.Add(ObjectLayer.Minion);
      objectLayerList.Add(ObjectLayer.Building);
      if (Grid.IsValidCell(cell))
      {
        foreach (ObjectLayer layer in objectLayerList)
        {
          GameObject go = Grid.Objects[cell, (int) layer];
          if ((bool) (Object) go)
          {
            SandboxToolParameterMenu.instance.settings.SetStringSetting("SandboxTools.SelectedEntity", go.PrefabID().ToString());
            break;
          }
        }
      }
    }
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }
}
