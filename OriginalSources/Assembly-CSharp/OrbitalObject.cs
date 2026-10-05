// Decompiled with JetBrains decompiler
// Type: OrbitalObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalObject")]
[SerializationConfig(MemberSerialization.OptIn)]
public class OrbitalObject : KMonoBehaviour, IRenderEveryTick
{
  private WorldContainer world;
  private OrbitalData orbitData;
  private KBatchedAnimController animController;
  [Serialize]
  private string animFilename;
  [Serialize]
  private string initialAnim;
  [Serialize]
  private Vector3 worldOrbitingOrigin;
  [Serialize]
  private int orbitingWorldId;
  [Serialize]
  private float angle;
  [Serialize]
  public int timeoffset;
  [Serialize]
  public string orbitalDBId;

  public void Init(
    string orbit_data_name,
    WorldContainer orbiting_world,
    List<Ref<OrbitalObject>> orbiting_obj)
  {
    OrbitalData data = Db.Get().OrbitalTypeCategories.Get(orbit_data_name);
    if ((Object) orbiting_world != (Object) null)
    {
      this.orbitingWorldId = orbiting_world.id;
      this.world = orbiting_world;
      this.worldOrbitingOrigin = this.GetWorldOrigin(this.world, data);
    }
    else
      this.worldOrbitingOrigin = new Vector3((float) Grid.WidthInCells * 0.5f, (float) Grid.HeightInCells * data.yGridPercent, 0.0f);
    this.animFilename = data.animFile;
    this.initialAnim = this.GetInitialAnim(data);
    this.angle = this.GetAngle(data);
    this.timeoffset = this.GetTimeOffset(orbiting_obj);
    this.orbitalDBId = data.Id;
  }

  protected override void OnSpawn()
  {
    this.world = ClusterManager.Instance.GetWorld(this.orbitingWorldId);
    this.orbitData = Db.Get().OrbitalTypeCategories.Get(this.orbitalDBId);
    this.gameObject.SetActive(false);
    KBatchedAnimController kbatchedAnimController = this.gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = this.initialAnim;
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) this.animFilename)
    };
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
    this.animController = kbatchedAnimController;
  }

  public void RenderEveryTick(float dt)
  {
    bool behind;
    Vector3 worldPos = this.CalculateWorldPos(450f, out behind);
    Vector3 position = worldPos;
    if ((double) this.orbitData.periodInCycles > 0.0)
    {
      position.x = worldPos.x / (float) Grid.WidthInCells;
      position.y = worldPos.y / (float) Grid.HeightInCells;
      position.x = Camera.main.ViewportToWorldPoint(position).x;
      position.y = Camera.main.ViewportToWorldPoint(position).y;
    }
    bool flag = (!this.orbitData.rotatesBehind || !behind) && ((Object) this.world == (Object) null || ClusterManager.Instance.activeWorldId == this.world.id);
    this.animController.Offset = (position - this.gameObject.transform.position) with
    {
      z = 0.0f
    };
    this.gameObject.transform.SetPosition(position with
    {
      x = this.worldOrbitingOrigin.x,
      y = this.worldOrbitingOrigin.y
    });
    if ((double) this.orbitData.periodInCycles > 0.0)
      this.gameObject.transform.localScale = Vector3.one * (CameraController.Instance.baseCamera.orthographicSize / this.orbitData.distance);
    else
      this.gameObject.transform.localScale = Vector3.one * this.orbitData.distance;
    if (this.gameObject.activeSelf == flag)
      return;
    this.gameObject.SetActive(flag);
  }

  private Vector3 CalculateWorldPos(float time, out bool behind)
  {
    Vector3 worldPos;
    if ((double) this.orbitData.periodInCycles > 0.0)
    {
      float num1 = this.orbitData.periodInCycles * 600f;
      float f = (float) ((((double) time + (double) this.timeoffset) / (double) num1 - (double) (int) (((double) time + (double) this.timeoffset) / (double) num1)) * 2.0 * 3.1415927410125732);
      float num2 = 0.5f * this.orbitData.radiusScale * (float) this.world.WorldSize.x;
      Vector3 vector3 = new Vector3(Mathf.Cos(f), 0.0f, Mathf.Sin(f));
      behind = (double) vector3.z > (double) this.orbitData.behindZ;
      worldPos = (this.worldOrbitingOrigin + Quaternion.Euler(this.angle, 0.0f, 0.0f) * (vector3 * num2)) with
      {
        z = this.orbitData.GetRenderZ == null ? this.orbitData.renderZ : this.orbitData.GetRenderZ()
      };
    }
    else
    {
      behind = false;
      worldPos = this.worldOrbitingOrigin with
      {
        z = this.orbitData.GetRenderZ == null ? this.orbitData.renderZ : this.orbitData.GetRenderZ()
      };
    }
    return worldPos;
  }

  private string GetInitialAnim(OrbitalData data)
  {
    if (!data.initialAnim.IsNullOrWhiteSpace())
      return data.initialAnim;
    KAnimFileData data1 = Assets.GetAnim((HashedString) data.animFile).GetData();
    int index = new KRandom().Next(0, data1.animCount - 1);
    return data1.GetAnim(index).name;
  }

  private Vector3 GetWorldOrigin(WorldContainer wc, OrbitalData data)
  {
    return (Object) wc != (Object) null ? new Vector3((float) wc.WorldOffset.x + (float) wc.WorldSize.x * data.xGridPercent, (float) wc.WorldOffset.y + (float) wc.WorldSize.y * data.yGridPercent, 0.0f) : new Vector3((float) Grid.WidthInCells * data.xGridPercent, (float) Grid.HeightInCells * data.yGridPercent, 0.0f);
  }

  private float GetAngle(OrbitalData data) => Random.Range(data.minAngle, data.maxAngle);

  private int GetTimeOffset(List<Ref<OrbitalObject>> orbiting_obj)
  {
    List<int> intList = new List<int>();
    foreach (Ref<OrbitalObject> @ref in orbiting_obj)
    {
      if ((Object) @ref.Get().world == (Object) this.world)
        intList.Add(@ref.Get().timeoffset);
    }
    int timeOffset = Random.Range(0, 600);
    while (intList.Contains(timeOffset))
      timeOffset = Random.Range(0, 600);
    return timeOffset;
  }
}
