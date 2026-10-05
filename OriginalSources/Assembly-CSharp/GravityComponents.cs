// Decompiled with JetBrains decompiler
// Type: GravityComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using UnityEngine;

#nullable disable
public class GravityComponents : KGameObjectComponentManager<GravityComponent>
{
  private const float Acceleration = -9.8f;
  private static Tag[] LANDS_ON_FAKEFLOOR = new Tag[3]
  {
    GameTags.BaseMinion,
    GameTags.Creatures.Walker,
    GameTags.Creatures.Hoverer
  };

  public HandleVector<int>.Handle Add(
    GameObject go,
    Vector2 initial_velocity,
    Action<Transform> on_landed = null)
  {
    bool land_on_fake_floors = false;
    KPrefabID component = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      land_on_fake_floors = component.HasAnyTags(GravityComponents.LANDS_ON_FAKEFLOOR);
    bool mayLeaveWorld = (UnityEngine.Object) go.GetComponent<MinionIdentity>() != (UnityEngine.Object) null;
    return this.Add(go, new GravityComponent(go.transform, on_landed, initial_velocity, land_on_fake_floors, mayLeaveWorld));
  }

  public override void FixedUpdate(float dt)
  {
    GravityComponents.Tuning tuning = TuningData<GravityComponents.Tuning>.Get();
    float num1 = tuning.maxVelocity * tuning.maxVelocity;
    for (int index = 0; index < this.data.Count; ++index)
    {
      GravityComponent gravityComponent = this.data[index];
      if ((double) gravityComponent.elapsedTime >= 0.0 && !((UnityEngine.Object) gravityComponent.transform == (UnityEngine.Object) null) && !this.IsInCleanupList(gravityComponent.transform.gameObject))
      {
        Vector3 position = gravityComponent.transform.GetPosition();
        Vector2 pos1 = (Vector2) position;
        Vector2 data = new Vector2(gravityComponent.velocity.x, gravityComponent.velocity.y + -9.8f * dt);
        float sqrMagnitude = data.sqrMagnitude;
        if ((double) sqrMagnitude > (double) num1)
          data *= tuning.maxVelocity / Mathf.Sqrt(sqrMagnitude);
        int cell1 = Grid.PosToCell(pos1);
        float groundOffset = GravityComponent.GetGroundOffset(gravityComponent);
        bool flag1 = Grid.IsVisiblyInLiquid(pos1 - new Vector2(0.0f, groundOffset));
        if (flag1)
        {
          flag1 = true;
          float b = (float) (gravityComponent.transform.GetInstanceID() % 1000) / 1000f * 0.25f;
          float num2 = tuning.maxVelocityInLiquid + b * tuning.maxVelocityInLiquid;
          if ((double) sqrMagnitude > (double) num2 * (double) num2)
          {
            float a = Mathf.Sqrt(sqrMagnitude);
            data = data / a * Mathf.Lerp(a, b, dt * (float) (5.0 + 5.0 * (double) b));
          }
        }
        gravityComponent.velocity = data;
        gravityComponent.elapsedTime += dt;
        Vector2 vector2 = pos1 + data * dt;
        Vector2 pos2 = vector2 with
        {
          y = vector2.y - groundOffset
        };
        bool flag2 = Grid.IsVisiblyInLiquid(pos2);
        if (!flag1 & flag2)
          Game.Instance.SpawnFX(SpawnFXHashes.SplashStep, new Vector3(vector2.x, vector2.y, Grid.GetLayerZ(Grid.SceneLayer.Front)) + new Vector3(-0.38f, 0.75f, -0.1f), 0.0f);
        bool flag3 = false;
        int cell2 = Grid.PosToCell(pos2);
        if (Grid.IsValidCell(cell2))
        {
          if ((double) data.sqrMagnitude > 0.20000000298023224 && Grid.IsValidCell(cell1) && !Grid.Element[cell1].IsLiquid && Grid.Element[cell2].IsLiquid)
          {
            AmbienceType ambience = Grid.Element[cell2].substance.GetAmbience();
            if (ambience != AmbienceType.None)
            {
              EventReference event_ref = Sounds.Instance.OreSplashSoundsMigrated[(int) ambience];
              if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null && CameraController.Instance.IsAudibleSound((Vector3) vector2, event_ref))
                SoundEvent.PlayOneShot(event_ref, (Vector3) vector2);
            }
          }
          bool flag4 = Grid.Solid[cell2];
          if (!flag4 && gravityComponent.landOnFakeFloors && Grid.FakeFloor[cell2])
          {
            Navigator component = gravityComponent.transform.GetComponent<Navigator>();
            if ((bool) (UnityEngine.Object) component)
            {
              flag4 = component.NavGrid.NavTable.IsValid(cell2);
              if (!flag4)
              {
                int cell3 = Grid.CellAbove(cell2);
                flag4 = component.NavGrid.NavTable.IsValid(cell3, NavType.Hover);
              }
            }
          }
          if (flag4)
          {
            Vector3 posCbc = Grid.CellToPosCBC(Grid.CellAbove(cell2), Grid.SceneLayer.Move);
            vector2.y = posCbc.y + groundOffset;
            gravityComponent.velocity.x = 0.0f;
            flag3 = true;
          }
          else
          {
            Vector2 pos3 = pos2;
            pos3.x -= gravityComponent.extents.x;
            int cell4 = Grid.PosToCell(pos3);
            if (Grid.IsValidCell(cell4) && Grid.Solid[cell4])
            {
              vector2.x = Mathf.Floor(vector2.x - gravityComponent.extents.x) + (1f + gravityComponent.extents.x);
              gravityComponent.velocity.x = -0.1f * gravityComponent.velocity.x;
            }
            else
            {
              Vector3 pos4 = (Vector3) pos2;
              pos4.x += gravityComponent.extents.x;
              int cell5 = Grid.PosToCell(pos4);
              if (Grid.IsValidCell(cell5) && Grid.Solid[cell5])
              {
                vector2.x = Mathf.Floor(vector2.x + gravityComponent.extents.x) - gravityComponent.extents.x;
                gravityComponent.velocity.x = -0.1f * gravityComponent.velocity.x;
              }
            }
          }
        }
        this.data[index] = gravityComponent;
        int cell6 = Grid.PosToCell(vector2);
        if (gravityComponent.mayLeaveWorld || !Grid.IsValidCell(cell1) || Grid.WorldIdx[cell1] == byte.MaxValue || Grid.IsValidCellInWorld(cell6, (int) Grid.WorldIdx[cell1]))
        {
          gravityComponent.transform.SetPosition(new Vector3(vector2.x, vector2.y, position.z));
          if (flag3)
          {
            gravityComponent.transform.gameObject.BoxingTrigger<Vector2>(1188683690, data);
            if (gravityComponent.onLanded != null)
              gravityComponent.onLanded(gravityComponent.transform);
          }
        }
      }
    }
  }

  public class Tuning : TuningData<GravityComponents.Tuning>
  {
    public float maxVelocity;
    public float maxVelocityInLiquid;
  }
}
