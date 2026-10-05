// Decompiled with JetBrains decompiler
// Type: ParallaxBackgroundObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
public class ParallaxBackgroundObject : KMonoBehaviour
{
  private static Mesh mesh;
  private static int? layer;
  private static float? depth;
  [SerializeField]
  private Sprite sprite;
  [SerializeField]
  private float parallaxFactor = 1f;
  [Range(0.0f, 5f)]
  public float scaleMin = 0.25f;
  [Range(0.0f, 5f)]
  public float scaleMax = 3f;
  [Serialize]
  private bool visible = true;
  private const string SHADER_DAMAGED_TIME_VARIABLE_NAME = "_LastTimeDamaged";
  private const string SHADER_PLAYER_CLICKED_TIME_VARIABLE_NAME = "_LastTimePlayerClickedNotification";
  private const string SHADER_SIZE_PROGRESS_VARIABLE_NAME = "_SizeProgress";
  private const string SHADER_EXPLOSION_START_TIME_VARIABLE_NAME = "_LastTimeExploding";
  [SerializeField]
  private Material material;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float normalizedDistance;
  [SerializeField]
  private bool distanceUpdate;
  [SerializeField]
  private Vector2 startOffset;
  [SerializeField]
  private Vector2 endOffset;
  [Serialize]
  public int? worldId;
  public ParallaxBackgroundObject.IMotion motion;

  public static Mesh Mesh
  {
    get
    {
      if ((UnityEngine.Object) ParallaxBackgroundObject.mesh == (UnityEngine.Object) null)
        ParallaxBackgroundObject.mesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
      return ParallaxBackgroundObject.mesh;
    }
  }

  public static int Layer
  {
    get
    {
      ParallaxBackgroundObject.layer.GetValueOrDefault();
      if (!ParallaxBackgroundObject.layer.HasValue)
        ParallaxBackgroundObject.layer = new int?(LayerMask.NameToLayer("Default"));
      return ParallaxBackgroundObject.layer.Value;
    }
  }

  public static float Depth
  {
    get
    {
      ParallaxBackgroundObject.depth.GetValueOrDefault();
      if (!ParallaxBackgroundObject.depth.HasValue)
        ParallaxBackgroundObject.depth = new float?(Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.8f);
      return ParallaxBackgroundObject.depth.Value;
    }
  }

  private void OnActiveWorldChanged(object data)
  {
    if (!this.worldId.HasValue)
      return;
    this.visible = ((Tuple<int, int>) data).first == this.worldId.Value;
  }

  public void Initialize(string texture) => this.sprite = Assets.GetSprite((HashedString) texture);

  public void SetVisibilityState(bool visible) => this.visible = visible;

  public void PlayPlayerClickFeedback()
  {
    this.material.SetFloat("_LastTimePlayerClickedNotification", Time.unscaledTime);
  }

  public void PlayExplosion() => this.material.SetFloat("_LastTimeExploding", Time.unscaledTime);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
    this.distanceUpdate = true;
    this.startOffset = new Vector2(0.0f, 0.0f);
    this.endOffset = new Vector2(0.5f, 0.2f);
    this.material = new Material(Assets.GetMaterial("BGPlanet"));
    this.material.SetTexture("_MainTex", (Texture) this.sprite.texture);
    this.material.SetFloat("_LastTimeDamaged", float.MinValue);
    this.material.SetFloat("_LastTimePlayerClickedNotification", float.MinValue);
    this.material.SetFloat("_SizeProgress", 0.0f);
    this.material.renderQueue = RenderQueues.Stars;
  }

  public void TriggerShaderDamagedEffect(int _)
  {
    this.material.SetFloat("_LastTimeDamaged", Time.unscaledTime);
  }

  public float lastScaleUsed { private set; get; } = 1f;

  private void LateUpdate()
  {
    if (this.motion == null || !this.visible)
      return;
    if (this.distanceUpdate)
    {
      float duration = this.motion.GetDuration();
      this.normalizedDistance = (double) duration == 0.0 ? 1f : 1f - Mathf.Pow(this.motion.GetETA() / duration, 4f);
      this.motion.OnNormalizedDistanceChanged(this.normalizedDistance);
    }
    this.material.color = Color.Lerp(new Color(0.168627456f, 0.227450982f, 0.360784322f, 0.0f), Color.white, this.normalizedDistance);
    float num1 = Mathf.Lerp(this.scaleMin, this.scaleMax, this.normalizedDistance);
    this.lastScaleUsed = num1;
    Vector2 vector2 = Vector2.Lerp(this.startOffset, this.endOffset, this.normalizedDistance);
    Vector3 position = CameraController.Instance.baseCamera.transform.position;
    Vector3 vector3_1 = new Vector3(position.x * this.parallaxFactor, position.y * this.parallaxFactor, ParallaxBackgroundObject.Depth);
    float num2 = CameraController.Instance.baseCamera.orthographicSize / 1f;
    Vector3 vector3_2 = (Vector3) vector2 * num2;
    Vector3 vector1 = vector3_1 + vector3_2;
    Vector3 vector3 = num1 * num2 * Vector3.one;
    Quaternion q = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0.0f, 0.0f, -20f), this.normalizedDistance);
    this.material.SetFloat("_UnscaledTime", Time.unscaledTime);
    this.material.SetVector("_Random", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value));
    this.material.SetFloat("_SizeProgress", this.normalizedDistance);
    Graphics.DrawMesh(ParallaxBackgroundObject.Mesh, Matrix4x4.Translate(vector1) * Matrix4x4.Scale(vector3) * Matrix4x4.Rotate(q), this.material, ParallaxBackgroundObject.Layer);
  }

  public interface IMotion
  {
    float GetETA();

    float GetDuration();

    void OnNormalizedDistanceChanged(float normalizedDistance);
  }
}
