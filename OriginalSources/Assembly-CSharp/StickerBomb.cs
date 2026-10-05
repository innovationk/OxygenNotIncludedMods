// Decompiled with JetBrains decompiler
// Type: StickerBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class StickerBomb : StateMachineComponent<StickerBomb.StatesInstance>
{
  [Serialize]
  public string stickerType;
  [Serialize]
  public string stickerName;
  private HandleVector<int>.Handle partitionerEntry;
  private List<int> cellOffsets;

  protected override void OnSpawn()
  {
    if (this.stickerName.IsNullOrWhiteSpace())
    {
      Debug.LogError((object) ("Missing sticker db entry for " + this.stickerType));
    }
    else
    {
      DbStickerBomb dbStickerBomb = Db.GetStickerBombs().Get(this.stickerName);
      this.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[1]
      {
        dbStickerBomb.animFile
      });
    }
    this.cellOffsets = StickerBomb.BuildCellOffsets(this.transform.GetPosition());
    this.smi.destroyTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.STICKER_DURATION;
    this.smi.StartSM();
    Extents extents = this.GetComponent<OccupyArea>().GetExtents();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("StickerBomb.OnSpawn", (object) this.gameObject, new Extents(extents.x - 1, extents.y - 1, extents.width + 2, extents.height + 2), GameScenePartitioner.Instance.objectLayers[2], new Action<object>(this.OnFoundationCellChanged));
    base.OnSpawn();
  }

  [System.Runtime.Serialization.OnDeserialized]
  public void OnDeserialized()
  {
    if (!this.stickerName.IsNullOrWhiteSpace() || this.stickerType.IsNullOrWhiteSpace())
      return;
    string[] strArray = this.stickerType.Split('_', StringSplitOptions.None);
    if (strArray.Length != 2)
      return;
    this.stickerName = strArray[1];
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnFoundationCellChanged(object data)
  {
    if (StickerBomb.CanPlaceSticker(this.cellOffsets))
      return;
    Util.KDestroyGameObject(this.gameObject);
  }

  public static List<int> BuildCellOffsets(Vector3 position)
  {
    List<int> intList = new List<int>();
    int num = (double) position.x % 1.0 < 0.5 ? 1 : 0;
    bool flag = (double) position.y % 1.0 > 0.5;
    int cell = Grid.PosToCell(position);
    intList.Add(cell);
    if (num != 0)
    {
      intList.Add(Grid.CellLeft(cell));
      if (flag)
      {
        intList.Add(Grid.CellAbove(cell));
        intList.Add(Grid.CellUpLeft(cell));
      }
      else
      {
        intList.Add(Grid.CellBelow(cell));
        intList.Add(Grid.CellDownLeft(cell));
      }
    }
    else
    {
      intList.Add(Grid.CellRight(cell));
      if (flag)
      {
        intList.Add(Grid.CellAbove(cell));
        intList.Add(Grid.CellUpRight(cell));
      }
      else
      {
        intList.Add(Grid.CellBelow(cell));
        intList.Add(Grid.CellDownRight(cell));
      }
    }
    return intList;
  }

  public static bool CanPlaceSticker(List<int> offsets)
  {
    foreach (int offset in offsets)
    {
      if (Grid.IsCellOpenToSpace(offset))
        return false;
    }
    return true;
  }

  public void SetStickerType(string newStickerType)
  {
    if (newStickerType == null)
      newStickerType = "sticker";
    DbStickerBomb randomSticker = Db.GetStickerBombs().GetRandomSticker();
    this.stickerName = randomSticker.Id;
    this.stickerType = $"{newStickerType}_{randomSticker.Id}";
    this.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[1]
    {
      randomSticker.animFile
    });
  }

  public class StatesInstance(StickerBomb master) : 
    GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.GameInstance(master)
  {
    [Serialize]
    public float destroyTime;

    public string GetStickerAnim(string type) => $"{type}_{this.master.stickerType}";
  }

  public class States : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb>
  {
    public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State destroy;
    public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State sparkle;
    public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State idle;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.Transition(this.destroy, (StateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.Transition.ConditionCallback) (smi => (double) GameClock.Instance.GetTime() >= (double) smi.destroyTime)).DefaultState(this.idle);
      this.idle.PlayAnim((Func<StickerBomb.StatesInstance, string>) (smi => smi.GetStickerAnim("idle"))).ScheduleGoTo((Func<StickerBomb.StatesInstance, float>) (smi => (float) UnityEngine.Random.Range(20, 30)), (StateMachine.BaseState) this.sparkle);
      this.sparkle.PlayAnim((Func<StickerBomb.StatesInstance, string>) (smi => smi.GetStickerAnim("sparkle"))).OnAnimQueueComplete(this.idle);
      this.destroy.Enter((StateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State.Callback) (smi => Util.KDestroyGameObject((Component) smi.master)));
    }
  }
}
