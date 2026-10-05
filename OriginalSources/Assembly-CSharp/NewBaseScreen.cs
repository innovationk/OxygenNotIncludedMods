// Decompiled with JetBrains decompiler
// Type: NewBaseScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using ProcGenGame;
using UnityEngine;

#nullable disable
public class NewBaseScreen : KScreen
{
  public static NewBaseScreen Instance;
  [SerializeField]
  private CanvasGroup[] disabledUIElements;
  public EventReference ScanSoundMigrated;
  public EventReference BuildBaseSoundMigrated;
  private ITelepadDeliverable[] m_minionStartingStats;
  private Cluster m_clusterLayout;

  public override float GetSortKey() => 1f;

  protected override void OnPrefabInit()
  {
    NewBaseScreen.Instance = this;
    base.OnPrefabInit();
    TimeOfDay.Instance.SetScale(0.0f);
  }

  protected override void OnForcedCleanUp()
  {
    NewBaseScreen.Instance = (NewBaseScreen) null;
    base.OnForcedCleanUp();
  }

  public static Vector2I SetInitialCamera()
  {
    Vector2I vector2I = SaveLoader.Instance.cachedGSD.baseStartPos + ClusterManager.Instance.GetStartWorld().WorldOffset;
    Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.OffsetCell(0, vector2I.x, vector2I.y), 0, -2), Grid.SceneLayer.Background);
    CameraController.Instance.SetMaxOrthographicSize(40f);
    CameraController.Instance.SnapTo(posCcc);
    CameraController.Instance.SetTargetPos(posCcc, 20f, false);
    CameraController.Instance.OrthographicSize = 40f;
    CameraSaveData.valid = false;
    return vector2I;
  }

  protected override void OnActivate()
  {
    if (this.disabledUIElements != null)
    {
      foreach (CanvasGroup disabledUiElement in this.disabledUIElements)
      {
        if ((Object) disabledUiElement != (Object) null)
          disabledUiElement.interactable = false;
      }
    }
    NewBaseScreen.SetInitialCamera();
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause(false);
    this.Final();
  }

  public void Init(Cluster clusterLayout, ITelepadDeliverable[] startingMinionStats)
  {
    this.m_clusterLayout = clusterLayout;
    this.m_minionStartingStats = startingMinionStats;
  }

  protected override void OnDeactivate()
  {
    Game.Instance.Trigger(-122303817, (object) null);
    if (this.disabledUIElements == null)
      return;
    foreach (CanvasGroup disabledUiElement in this.disabledUIElements)
    {
      if ((Object) disabledUiElement != (Object) null)
        disabledUiElement.interactable = true;
    }
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    Action[] actionArray = new Action[4]
    {
      Action.SpeedUp,
      Action.SlowDown,
      Action.TogglePause,
      Action.CycleSpeed
    };
    if (e.Consumed)
      return;
    int index = 0;
    while (index < actionArray.Length && !e.TryConsume(actionArray[index]))
      ++index;
  }

  private void Final()
  {
    SpeedControlScreen.Instance.Unpause(false);
    GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
    if ((bool) (Object) telepad)
      this.SpawnMinions(telepad);
    Game.Instance.baseAlreadyCreated = true;
    this.Deactivate();
  }

  private void SpawnMinions(GameObject start_pad)
  {
    int cell1 = Grid.PosToCell(start_pad);
    if (cell1 == -1)
    {
      Debug.LogWarning((object) "No headquarters in saved base template. Cannot place minions. Confirm there is a headquarters saved to the base template, or consider creating a new one.");
    }
    else
    {
      int x;
      int y;
      Grid.CellToXY(cell1, out x, out y);
      if (Grid.WidthInCells < 64 /*0x40*/)
        return;
      int baseLeft = this.m_clusterLayout.currentWorld.BaseLeft;
      int baseRight = this.m_clusterLayout.currentWorld.BaseRight;
      Db.Get().effects.Get("AnewHope");
      Telepad component = start_pad.GetComponent<Telepad>();
      for (int index = 0; index < this.m_minionStartingStats.Length; ++index)
      {
        MinionStartingStats minionStartingStat = (MinionStartingStats) this.m_minionStartingStats[index];
        int cell2 = Grid.XYToCell(x + index % (baseRight - baseLeft) + 1, y);
        GameObject prefab = Assets.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(minionStartingStat.personality.model));
        GameObject gameObject = Util.KInstantiate(prefab);
        gameObject.name = prefab.name;
        Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
        gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Move));
        gameObject.SetActive(true);
        minionStartingStat.Apply(gameObject);
        bool extra_power_banks = minionStartingStat.personality.model == GameTags.Minions.Models.Bionic;
        if ((Object) component != (Object) null)
          component.AddNewBaseMinion(gameObject, extra_power_banks);
      }
      component.ScheduleNewBaseEvents();
      ClusterManager.Instance.activeWorld.SetDupeVisited();
    }
  }
}
