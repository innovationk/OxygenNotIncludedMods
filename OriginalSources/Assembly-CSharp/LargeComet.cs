// Decompiled with JetBrains decompiler
// Type: LargeComet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class LargeComet : KMonoBehaviour, ISim33ms
{
  private static HashedString FLYING_SOUND_ID_PARAMETER = (HashedString) "meteorType";
  public string impactSound;
  public string flyingSound;
  public int flyingSoundID;
  public List<KeyValuePair<string, string>> additionalAnimFiles = new List<KeyValuePair<string, string>>();
  public KeyValuePair<string, string> mainAnimFile;
  public bool affectedByDifficulty = true;
  public bool destroyOnExplode = true;
  public bool spawnWithOffset;
  public Vector2I stampLocation;
  public Vector2I crashPosition;
  public Dictionary<int, CellOffset> bottomCellsOffsetOfTemplate;
  public TemplateContainer asteroidTemplate;
  public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();
  private bool hasExploded;
  private float age;
  private int lowestTemplateYLocalPosition;
  private int templateWidth;
  private int worldID;
  private Vector3 previousVisualPosition;
  private Vector3 initialPosition;
  private Vector2I prevCell;
  public System.Action OnImpact;
  [Serialize]
  protected Vector3 offsetPosition;
  [Serialize]
  protected Vector2 velocity;
  [MyCmpGet]
  private KBatchedAnimController anim;
  [MyCmpGet]
  private KSelectable selectable;
  private LoopingSounds loopingSounds;
  private KBatchedAnimController[] child_controllers;
  private List<KAnimControllerBase> additionalAnimControllers = new List<KAnimControllerBase>();
  private KBatchedAnimController mainChildrenAnimController;
  private Vector2I fromStampToCrashPosition;
  private HashSet<int> cellsCentrePassedThrough = new HashSet<int>();
  private Vector3 activeExplosionPosition = Vector3.zero;
  private Material largeCometMaterial;
  private Sprite largeCometTexture;
  private Sprite explosionTexture;
  private float minSeparationBetweenExplosions = 8f;
  private Vector3 lastExplosionPosition;
  private const string LARGE_COMET_SHADER_NAME = "Klei/DLC4/LargeImpactorCometShader";
  private const int MAX_SHADER_EXPLOSION_COUNT = 30;
  private const float EXPLOSION_ANIMATION_FRAME_COUNT = 37f;
  private const float EXPLOSION_ANIMATION_DURATION = 1.23333335f;
  private Vector4[] ShaderExplosions = new Vector4[30];

  public float LandingProgress { private set; get; }

  public Vector3 VisualPosition => this.transform.position + this.anim.Offset;

  public Vector3 VisualPositionCentredImage
  {
    get
    {
      return this.VisualPosition + new Vector3(0.0f, (float) Mathf.Abs(this.lowestTemplateYLocalPosition), 0.0f);
    }
  }

  public Vector2 Velocity
  {
    get => this.velocity;
    set => this.velocity = value;
  }

  private float GetVolume(GameObject gameObject)
  {
    float volume = 1f;
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.selectable != (UnityEngine.Object) null && this.selectable.IsSelected)
      volume = 1f;
    return volume;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.loopingSounds = this.gameObject.GetComponent<LoopingSounds>();
    this.flyingSound = GlobalAssets.GetSound("Meteor_LP");
    this.SetVelocity();
  }

  protected override void OnSpawn()
  {
    this.anim.Offset = this.offsetPosition;
    this.SetupOffset();
    this.child_controllers = this.GetComponents<KBatchedAnimController>();
    foreach (KAnimControllerBase childController in this.child_controllers)
      childController.Offset = this.anim.Offset;
    base.OnSpawn();
    this.StartLoopingSound();
    this.selectable.enabled = (double) this.offsetPosition.x == 0.0 && (double) this.offsetPosition.y == 0.0;
    Vector3 position = this.gameObject.transform.position;
    foreach (KeyValuePair<string, string> additionalAnimFile in this.additionalAnimFiles)
    {
      this.additionalAnimControllers.Add((KAnimControllerBase) this.AddEffectAnim(additionalAnimFile.Key, additionalAnimFile.Value, position));
      position.z -= 1f / 1000f;
    }
    KBatchedAnimController kbatchedAnimController = this.AddEffectAnim(this.mainAnimFile.Key, this.mainAnimFile.Value, position);
    this.additionalAnimControllers.Add((KAnimControllerBase) kbatchedAnimController);
    this.mainChildrenAnimController = kbatchedAnimController;
    this.mainChildrenAnimController.materialType = KAnimBatchGroup.MaterialType.Invisible;
    this.initialPosition = this.VisualPosition;
    this.lowestTemplateYLocalPosition = this.asteroidTemplate.GetTemplateBounds().yMin;
    this.templateWidth = this.asteroidTemplate.GetTemplateBounds().width;
    this.InitializeMaterial();
    CameraController.Instance.RegisterCustomScreenPostProcessingEffect(new Func<RenderTexture, Material>(this.DrawComet));
    this.fromStampToCrashPosition = this.stampLocation - this.crashPosition;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    CameraController.Instance.UnregisterCustomScreenPostProcessingEffect(new Func<RenderTexture, Material>(this.DrawComet));
  }

  private KBatchedAnimController AddEffectAnim(
    string anim_file,
    string anim_name,
    Vector3 startPosition)
  {
    KBatchedAnimController effect = FXHelpers.CreateEffect(anim_file, startPosition);
    effect.Play((HashedString) anim_name, KAnim.PlayMode.Loop);
    effect.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    effect.animScale = 0.1f;
    effect.isMovable = true;
    effect.Offset = this.anim.Offset;
    return effect;
  }

  protected void SetupOffset()
  {
    Vector3 position1 = this.transform.GetPosition();
    Vector3 position2 = this.transform.GetPosition() with
    {
      z = 0.0f
    };
    Vector3 vector3_1 = new Vector3(this.velocity.x, this.velocity.y, 0.0f);
    WorldContainer myWorld = this.gameObject.GetMyWorld();
    float num1 = Mathf.Abs(((float) (myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y) / Mathf.Cos(Vector3.Angle(Vector3.up, -vector3_1) * ((float) Math.PI / 180f)));
    Vector3 vector3_2 = position2 - vector3_1.normalized * num1;
    float num2 = (float) (myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
    if (((double) vector3_2.x < (double) myWorld.WorldOffset.x * (double) Grid.CellSizeInMeters ? 0 : ((double) vector3_2.x <= (double) num2 ? 1 : 0)) == 0)
      num1 = Mathf.Abs(((double) vector3_1.x < 0.0 ? num2 - position2.x : position2.x - (float) myWorld.WorldOffset.x * Grid.CellSizeInMeters) / Mathf.Cos(Vector3.Angle((double) vector3_1.x < 0.0 ? Vector3.right : Vector3.left, -vector3_1) * ((float) Math.PI / 180f)));
    Vector3 vector3_3 = -vector3_1.normalized * num1;
    (position2 + vector3_3).z = position1.z;
    this.offsetPosition = vector3_3;
    this.anim.Offset = this.offsetPosition;
    this.worldID = myWorld.id;
    this.previousVisualPosition = this.VisualPosition;
  }

  public void SetVelocity()
  {
    int num1 = -90;
    float f = (float) ((double) num1 * 3.1415927410125732 / 180.0);
    int num2 = 12;
    this.velocity = new Vector2(-Mathf.Cos(f) * (float) num2, Mathf.Sin(f) * (float) num2);
    this.GetComponent<KBatchedAnimController>().Rotation = (float) -num1 - 90f;
  }

  private void Explode(Vector3 pos)
  {
    this.PlayImpactSound(pos);
    if (this.OnImpact != null)
      this.OnImpact();
    foreach (Component additionalAnimController in this.additionalAnimControllers)
      Util.KDestroyGameObject(additionalAnimController);
    Util.KDestroyGameObject(this.gameObject);
  }

  public void Sim33ms(float dt)
  {
    if (this.hasExploded)
      return;
    if ((double) this.offsetPosition.y > 0.0)
    {
      this.offsetPosition += new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0.0f);
      this.anim.Offset = this.offsetPosition;
      foreach (KAnimControllerBase additionalAnimController in this.additionalAnimControllers)
        additionalAnimController.Offset = this.offsetPosition;
    }
    else
    {
      if (this.anim.Offset != Vector3.zero)
      {
        this.anim.Offset = Vector3.zero;
        foreach (KAnimControllerBase additionalAnimController in this.additionalAnimControllers)
          additionalAnimController.Offset = this.anim.Offset;
      }
      Vector3 position1 = this.transform.GetPosition();
      Vector3 vector3 = position1 + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0.0f);
      this.loopingSounds.UpdateVelocity(this.flyingSound, (Vector2) (vector3 - position1));
      this.transform.SetPosition(vector3);
      Vector3 position2 = vector3;
      foreach (Component additionalAnimController in this.additionalAnimControllers)
      {
        additionalAnimController.transform.SetPosition(position2);
        position2.z -= 1f / 1000f;
      }
      if ((double) vector3.y < (double) this.crashPosition.y)
        this.Explode(vector3);
    }
    Vector2I xy1 = Grid.PosToXY(this.previousVisualPosition);
    Vector2I xy2 = Grid.PosToXY(this.VisualPosition);
    xy1.y = Mathf.Clamp(xy1.y, this.crashPosition.y, int.MaxValue);
    xy2.y = Mathf.Clamp(xy2.y, this.crashPosition.y, int.MaxValue);
    if (xy2.y != xy1.y)
    {
      Grid.CollectCellsInLine(Grid.XYToCell(xy1.x, xy1.y), Grid.XYToCell(xy2.x, xy2.y), this.cellsCentrePassedThrough);
      bool flag = false;
      Vector3 position = Vector3.zero;
      foreach (int cell1 in this.cellsCentrePassedThrough)
      {
        foreach (CellOffset cellOffset in this.bottomCellsOffsetOfTemplate.Values)
        {
          int cell2 = Grid.OffsetCell(Grid.OffsetCell(cell1, 0, Mathf.Abs(this.lowestTemplateYLocalPosition)), cellOffset.x, cellOffset.y);
          if (Grid.IsValidCellInWorld(cell2, this.worldID) && this.DestroyCell(cell2) && !flag && this.IsPositionFarAwayFromOtherExplosions(Grid.CellToPos(cell2)))
          {
            flag = true;
            position = Grid.CellToPos(cell2);
          }
        }
      }
      if (flag)
        this.PlayExplosionEffectOnPosition(position);
    }
    float num = Mathf.Clamp((float) (1.0 - ((double) this.VisualPosition.y - (double) this.crashPosition.y) / ((double) this.initialPosition.y - (double) this.crashPosition.y)), 0.0f, 1f);
    this.mainChildrenAnimController.postProcessingParameters = Mathf.Clamp(Mathf.Ceil(num * (Mathf.Pow(10f, 3f) - 1f)), 0.0f, float.MaxValue);
    this.LandingProgress = num;
    this.previousVisualPosition = this.VisualPosition;
    this.age += dt;
  }

  private bool IsPositionFarAwayFromOtherExplosions(Vector3 position)
  {
    this.activeExplosionPosition.z = position.z;
    for (int index = 0; index < 30; ++index)
    {
      if ((double) this.ShaderExplosions[index].z >= 0.0 && (double) Time.timeSinceLevelLoad - (double) this.ShaderExplosions[index].z < 1.2333333492279053)
      {
        this.activeExplosionPosition.x = this.ShaderExplosions[index].x;
        this.activeExplosionPosition.y = this.ShaderExplosions[index].y;
        if ((double) (this.activeExplosionPosition - position).magnitude < (double) this.minSeparationBetweenExplosions)
          return false;
      }
    }
    return true;
  }

  private void PlayExplosionEffectOnPosition(Vector3 position)
  {
    for (int index = 0; index < 30; ++index)
    {
      if ((double) this.ShaderExplosions[index].z < 0.0 || (double) Time.timeSinceLevelLoad - (double) this.ShaderExplosions[index].z > 1.2333333492279053)
      {
        this.ShaderExplosions[index].x = position.x;
        this.ShaderExplosions[index].y = position.y;
        this.ShaderExplosions[index].z = Time.timeSinceLevelLoad;
        KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode"), position);
        this.lastExplosionPosition = position;
        break;
      }
    }
  }

  private void PlayImpactSound(Vector3 pos)
  {
    if (this.impactSound == null)
      this.impactSound = "Meteor_Large_Impact";
    this.loopingSounds.StopSound(this.flyingSound);
    string sound = GlobalAssets.GetSound(this.impactSound);
    int cell = Grid.PosToCell(pos);
    if (!Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
      return;
    float volume = this.GetVolume(this.gameObject);
    pos.z = 0.0f;
    EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
    int num = (int) instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
    KFMOD.EndOneShot(instance);
  }

  public bool DestroyCell(int cell)
  {
    ListPool<GameObject, LargeComet>.PooledList pooledList = ListPool<GameObject, LargeComet>.Allocate();
    GameObject gameObject = Grid.Objects[cell, 1];
    bool flag1 = (UnityEngine.Object) gameObject != (UnityEngine.Object) null;
    pooledList.Add(gameObject);
    pooledList.Add(Grid.Objects[cell, 2]);
    pooledList.Add(Grid.Objects[cell, 12]);
    pooledList.Add(Grid.Objects[cell, 15]);
    pooledList.Add(Grid.Objects[cell, 16 /*0x10*/]);
    pooledList.Add(Grid.Objects[cell, 19]);
    pooledList.Add(Grid.Objects[cell, 20]);
    pooledList.Add(Grid.Objects[cell, 23]);
    pooledList.Add(Grid.Objects[cell, 26]);
    pooledList.Add(Grid.Objects[cell, 29]);
    pooledList.Add(Grid.Objects[cell, 31 /*0x1F*/]);
    pooledList.Add(Grid.Objects[cell, 30]);
    foreach (MinionIdentity cmp in Components.LiveMinionIdentities.Items)
    {
      if (Grid.PosToCell((KMonoBehaviour) cmp) == cell)
      {
        pooledList.Add(cmp.gameObject);
        ++SaveGame.Instance.ColonyAchievementTracker.deadDupeCounter;
      }
    }
    foreach (GameObject original in (List<GameObject>) pooledList)
    {
      if ((UnityEngine.Object) original != (UnityEngine.Object) null)
        Util.KDestroyGameObject(original);
    }
    this.ClearCellPickupables(cell);
    Element element = ElementLoader.elements[(int) Grid.ElementIdx[cell]];
    if (element.id == SimHashes.Void)
      SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0.0f, 0.0f);
    else
      SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0.0f, 0.0f);
    bool flag2 = flag1 || element.IsSolid;
    pooledList.Recycle();
    return flag2;
  }

  public void ClearCellPickupables(int cell)
  {
    GameObject gameObject1 = Grid.Objects[cell, 3];
    if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null))
      return;
    ObjectLayerListItem objectLayerListItem = gameObject1.GetComponent<Pickupable>().objectLayerListItem;
    while (objectLayerListItem != null)
    {
      GameObject gameObject2 = objectLayerListItem.gameObject;
      objectLayerListItem = objectLayerListItem.nextItem;
      if (!((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null))
        Util.KDestroyGameObject(gameObject2);
    }
  }

  private void InitializeMaterial()
  {
    this.largeCometMaterial = new Material(Shader.Find("Klei/DLC4/LargeImpactorCometShader"));
    this.largeCometTexture = Assets.GetSprite((HashedString) "Demolior_final_broken");
    this.explosionTexture = Assets.GetSprite((HashedString) "contact_explode_fx_animationSheet");
    for (int index = 0; index < 30; ++index)
    {
      this.ShaderExplosions[index] = Vector4.one * -1f;
      this.ShaderExplosions[index].w = (float) (((double) this.minSeparationBetweenExplosions - 1.0) * 2.0);
    }
  }

  private Material DrawComet(RenderTexture source)
  {
    this.largeCometMaterial.SetTexture("_CometTex", (Texture) this.largeCometTexture.texture);
    this.largeCometMaterial.SetTexture("_ExplosionTex", (Texture) this.explosionTexture.texture);
    this.largeCometMaterial.SetVector("_CometWorldPosition", (Vector4) this.VisualPositionCentredImage);
    this.largeCometMaterial.SetFloat("_LandingProgress", this.LandingProgress);
    this.largeCometMaterial.SetFloat("_CometWidth", (float) this.templateWidth);
    this.largeCometMaterial.SetFloat("_CometRatio", (float) this.largeCometTexture.texture.height / (float) this.largeCometTexture.texture.width);
    this.largeCometMaterial.SetFloat("_UnscaledTime", Time.unscaledTime);
    this.largeCometMaterial.SetVectorArray("_ExplosionLocations", this.ShaderExplosions);
    return this.largeCometMaterial;
  }

  private void StartLoopingSound()
  {
    this.loopingSounds.StartSound(this.flyingSound);
    this.loopingSounds.UpdateFirstParameter(this.flyingSound, LargeComet.FLYING_SOUND_ID_PARAMETER, (float) this.flyingSoundID);
  }
}
