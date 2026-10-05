// Decompiled with JetBrains decompiler
// Type: TMPro.Examples.ObjectSpin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace TMPro.Examples;

public class ObjectSpin : MonoBehaviour
{
  public ObjectSpin.MotionType Motion;
  public Vector3 TranslationDistance = new Vector3(5f, 0.0f, 0.0f);
  public float TranslationSpeed = 1f;
  public float SpinSpeed = 5f;
  public int RotationRange = 15;
  private Transform m_transform;
  private float m_time;
  private Vector3 m_prevPOS;
  private Vector3 m_initial_Rotation;
  private Vector3 m_initial_Position;
  private Color32 m_lightColor;

  private void Awake()
  {
    this.m_transform = this.transform;
    this.m_initial_Rotation = this.m_transform.rotation.eulerAngles;
    this.m_initial_Position = this.m_transform.position;
    Light component = this.GetComponent<Light>();
    this.m_lightColor = (Color32) ((Object) component != (Object) null ? component.color : Color.black);
  }

  private void Update()
  {
    switch (this.Motion)
    {
      case ObjectSpin.MotionType.Rotation:
        this.m_transform.Rotate(0.0f, this.SpinSpeed * Time.deltaTime, 0.0f);
        break;
      case ObjectSpin.MotionType.SearchLight:
        this.m_time += this.SpinSpeed * Time.deltaTime;
        this.m_transform.rotation = Quaternion.Euler(this.m_initial_Rotation.x, Mathf.Sin(this.m_time) * (float) this.RotationRange + this.m_initial_Rotation.y, this.m_initial_Rotation.z);
        break;
      case ObjectSpin.MotionType.Translation:
        this.m_time += this.TranslationSpeed * Time.deltaTime;
        float x = this.TranslationDistance.x * Mathf.Cos(this.m_time);
        float z = this.TranslationDistance.y * Mathf.Sin(this.m_time) * Mathf.Cos(this.m_time * 1f);
        float y = this.TranslationDistance.z * Mathf.Sin(this.m_time);
        this.m_transform.position = this.m_initial_Position + new Vector3(x, y, z);
        this.m_prevPOS = this.m_transform.position;
        break;
    }
  }

  public enum MotionType
  {
    Rotation,
    SearchLight,
    Translation,
  }
}
