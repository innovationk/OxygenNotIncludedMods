// Decompiled with JetBrains decompiler
// Type: UserNavigation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/UserNavigation")]
public class UserNavigation : KMonoBehaviour
{
  [Serialize]
  private List<UserNavigation.NavPoint> hotkeyNavPoints = new List<UserNavigation.NavPoint>();
  [Serialize]
  private Dictionary<int, UserNavigation.NavPoint> worldCameraPositions = new Dictionary<int, UserNavigation.NavPoint>();

  public UserNavigation()
  {
    for (Action action = Action.SetUserNav1; action <= Action.SetUserNav10; ++action)
      this.hotkeyNavPoints.Add(UserNavigation.NavPoint.Invalid);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.Subscribe(1983128072, (Action<object>) (worlds =>
    {
      Tuple<int, int> tuple = (Tuple<int, int>) worlds;
      int first = tuple.first;
      int second = tuple.second;
      int cell = Grid.PosToCell(CameraController.Instance.transform.position);
      if (!Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] != second)
      {
        WorldContainer world = ClusterManager.Instance.GetWorld(second);
        CameraController.Instance.SetPosition(new Vector3(Mathf.Clamp(CameraController.Instance.transform.position.x, world.minimumBounds.x, world.maximumBounds.x), Mathf.Clamp(CameraController.Instance.transform.position.y, world.minimumBounds.y, world.maximumBounds.y), CameraController.Instance.transform.position.z));
      }
      this.worldCameraPositions[second] = new UserNavigation.NavPoint()
      {
        pos = CameraController.Instance.transform.position,
        orthoSize = CameraController.Instance.targetOrthographicSize
      };
      if (!this.worldCameraPositions.ContainsKey(first))
      {
        WorldContainer world = ClusterManager.Instance.GetWorld(first);
        Vector2I vector2I = world.WorldOffset + new Vector2I(world.Width / 2, world.Height / 2);
        this.worldCameraPositions.Add(first, new UserNavigation.NavPoint()
        {
          pos = new Vector3((float) vector2I.x, (float) vector2I.y),
          orthoSize = CameraController.Instance.targetOrthographicSize
        });
      }
      CameraController.Instance.SetTargetPosForWorldChange(this.worldCameraPositions[first].pos, this.worldCameraPositions[first].orthoSize, false);
    }));
  }

  public void SetWorldCameraStartPosition(int world_id, Vector3 start_pos)
  {
    if (!this.worldCameraPositions.ContainsKey(world_id))
      this.worldCameraPositions.Add(world_id, new UserNavigation.NavPoint()
      {
        pos = new Vector3(start_pos.x, start_pos.y),
        orthoSize = CameraController.Instance.targetOrthographicSize
      });
    else
      this.worldCameraPositions[world_id] = new UserNavigation.NavPoint()
      {
        pos = new Vector3(start_pos.x, start_pos.y),
        orthoSize = CameraController.Instance.targetOrthographicSize
      };
  }

  private static int GetIndex(Action action)
  {
    int index = -1;
    if (Action.SetUserNav1 <= action && action <= Action.SetUserNav10)
      index = (int) (action - 15);
    else if (Action.GotoUserNav1 <= action && action <= Action.GotoUserNav10)
      index = (int) (action - 25);
    return index;
  }

  private void SetHotkeyNavPoint(Action action, Vector3 pos, float ortho_size)
  {
    int index = UserNavigation.GetIndex(action);
    if (index < 0)
      return;
    this.hotkeyNavPoints[index] = new UserNavigation.NavPoint()
    {
      pos = pos,
      orthoSize = ortho_size
    };
    EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_set"), Vector3.zero);
    int num = (int) instance.setParameterByName("userNavPoint_ID", (float) index);
    KFMOD.EndOneShot(instance);
  }

  private void GoToHotkeyNavPoint(Action action)
  {
    int index = UserNavigation.GetIndex(action);
    if (index < 0)
      return;
    UserNavigation.NavPoint hotkeyNavPoint = this.hotkeyNavPoints[index];
    if (!hotkeyNavPoint.IsValid())
      return;
    CameraController.Instance.SetTargetPos(hotkeyNavPoint.pos, hotkeyNavPoint.orthoSize, true);
    EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_recall"), Vector3.zero);
    int num = (int) instance.setParameterByName("userNavPoint_ID", (float) index);
    KFMOD.EndOneShot(instance);
  }

  public bool Handle(KButtonEvent e)
  {
    bool flag = false;
    for (Action action = Action.GotoUserNav1; action <= Action.GotoUserNav10; ++action)
    {
      if (e.TryConsume(action))
      {
        this.GoToHotkeyNavPoint(action);
        flag = true;
        break;
      }
    }
    if (!flag)
    {
      for (Action action = Action.SetUserNav1; action <= Action.SetUserNav10; ++action)
      {
        if (e.TryConsume(action))
        {
          Camera baseCamera = CameraController.Instance.baseCamera;
          Vector3 position = baseCamera.transform.GetPosition();
          this.SetHotkeyNavPoint(action, position, baseCamera.orthographicSize);
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  [Serializable]
  private struct NavPoint
  {
    public Vector3 pos;
    public float orthoSize;
    public static readonly UserNavigation.NavPoint Invalid = new UserNavigation.NavPoint()
    {
      pos = Vector3.zero,
      orthoSize = 0.0f
    };

    public bool IsValid() => (double) this.orthoSize != 0.0;
  }
}
