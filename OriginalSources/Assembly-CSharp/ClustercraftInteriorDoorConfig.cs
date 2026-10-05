// Decompiled with JetBrains decompiler
// Type: ClustercraftInteriorDoorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClustercraftInteriorDoorConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "ClustercraftInteriorDoor";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string id = ClustercraftInteriorDoorConfig.ID;
    string name = (string) STRINGS.BUILDINGS.PREFABS.CLUSTERCRAFTINTERIORDOOR.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.CLUSTERCRAFTINTERIORDOOR.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "rocket_hatch_door_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 400f, anim, "closed", Grid.SceneLayer.TileMain, 1, 2, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Gravitas
    });
    placedEntity.AddTag(GameTags.NotRoomAssignable);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    placedEntity.AddOrGet<Operational>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.InteriorWall;
    placedEntity.AddOrGet<ClustercraftInteriorDoor>();
    placedEntity.AddOrGet<AssignmentGroupController>().generateGroupOnStart = false;
    placedEntity.AddOrGet<NavTeleporter>().offset = new CellOffset(1, 0);
    placedEntity.AddOrGet<AccessControl>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
  }

  public void OnSpawn(GameObject inst)
  {
    PrimaryElement component1 = inst.GetComponent<PrimaryElement>();
    OccupyArea component2 = inst.GetComponent<OccupyArea>();
    int cell = Grid.PosToCell(inst);
    CellOffset[] occupiedCellsOffsets = component2.OccupiedCellsOffsets;
    int[] numArray = new int[occupiedCellsOffsets.Length];
    for (int index = 0; index < occupiedCellsOffsets.Length; ++index)
    {
      CellOffset offset = occupiedCellsOffsets[index];
      int num = Grid.OffsetCell(cell, offset);
      numArray[index] = num;
    }
    for (int index = 0; index < numArray.Length; ++index)
    {
      int num = numArray[index];
      Grid.HasDoor[num] = true;
      SimMessages.SetCellProperties(num, (byte) 8);
      Grid.RenderedByWorld[num] = false;
      World.Instance.groundRenderer.MarkDirty(num);
      SimMessages.ReplaceAndDisplaceElement(num, component1.ElementID, CellEventLogger.Instance.DoorClose, component1.Mass / 2f, component1.Temperature);
      SimMessages.SetCellProperties(num, (byte) 4);
    }
  }
}
