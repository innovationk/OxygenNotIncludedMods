// Decompiled with JetBrains decompiler
// Type: TMPro.Examples.TextMeshProFloatingText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace TMPro.Examples;

public class TextMeshProFloatingText : MonoBehaviour
{
  public Font TheFont;
  private GameObject m_floatingText;
  private TextMeshPro m_textMeshPro;
  private TextMesh m_textMesh;
  private Transform m_transform;
  private Transform m_floatingText_Transform;
  private Transform m_cameraTransform;
  private Vector3 lastPOS = Vector3.zero;
  private Quaternion lastRotation = Quaternion.identity;
  public int SpawnType;
  public bool IsTextObjectScaleStatic;
  private static WaitForEndOfFrame k_WaitForEndOfFrame = new WaitForEndOfFrame();
  private static WaitForSeconds[] k_WaitForSecondsRandom = new WaitForSeconds[20]
  {
    new WaitForSeconds(0.05f),
    new WaitForSeconds(0.1f),
    new WaitForSeconds(0.15f),
    new WaitForSeconds(0.2f),
    new WaitForSeconds(0.25f),
    new WaitForSeconds(0.3f),
    new WaitForSeconds(0.35f),
    new WaitForSeconds(0.4f),
    new WaitForSeconds(0.45f),
    new WaitForSeconds(0.5f),
    new WaitForSeconds(0.55f),
    new WaitForSeconds(0.6f),
    new WaitForSeconds(0.65f),
    new WaitForSeconds(0.7f),
    new WaitForSeconds(0.75f),
    new WaitForSeconds(0.8f),
    new WaitForSeconds(0.85f),
    new WaitForSeconds(0.9f),
    new WaitForSeconds(0.95f),
    new WaitForSeconds(1f)
  };

  private void Awake()
  {
    this.m_transform = this.transform;
    this.m_floatingText = new GameObject(this.name + " floating text");
    this.m_cameraTransform = Camera.main.transform;
  }

  private void Start()
  {
    if (this.SpawnType == 0)
    {
      this.m_textMeshPro = this.m_floatingText.AddComponent<TextMeshPro>();
      this.m_textMeshPro.rectTransform.sizeDelta = new Vector2(3f, 3f);
      this.m_floatingText_Transform = this.m_floatingText.transform;
      this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0.0f, 15f, 0.0f);
      this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
      this.m_textMeshPro.color = (Color) new Color32((byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), byte.MaxValue);
      this.m_textMeshPro.fontSize = 24f;
      this.m_textMeshPro.fontFeatures.Clear();
      this.m_textMeshPro.text = string.Empty;
      this.m_textMeshPro.isTextObjectScaleStatic = this.IsTextObjectScaleStatic;
      this.StartCoroutine(this.DisplayTextMeshProFloatingText());
    }
    else if (this.SpawnType == 1)
    {
      this.m_floatingText_Transform = this.m_floatingText.transform;
      this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0.0f, 15f, 0.0f);
      this.m_textMesh = this.m_floatingText.AddComponent<TextMesh>();
      this.m_textMesh.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
      this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
      this.m_textMesh.color = (Color) new Color32((byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), byte.MaxValue);
      this.m_textMesh.anchor = TextAnchor.LowerCenter;
      this.m_textMesh.fontSize = 24;
      this.StartCoroutine(this.DisplayTextMeshFloatingText());
    }
    else
    {
      int spawnType = this.SpawnType;
    }
  }

  public IEnumerator DisplayTextMeshProFloatingText()
  {
    float CountDuration = 2f;
    float starting_Count = Random.Range(5f, 20f);
    float current_Count = starting_Count;
    Vector3 start_pos = this.m_floatingText_Transform.position;
    Color32 start_color = (Color32) this.m_textMeshPro.color;
    float alpha = (float) byte.MaxValue;
    float fadeDuration = 3f / starting_Count * CountDuration;
    while ((double) current_Count > 0.0)
    {
      current_Count -= Time.deltaTime / CountDuration * starting_Count;
      if ((double) current_Count <= 3.0)
        alpha = Mathf.Clamp(alpha - (float) ((double) Time.deltaTime / (double) fadeDuration * (double) byte.MaxValue), 0.0f, (float) byte.MaxValue);
      this.m_textMeshPro.text = ((int) current_Count).ToString();
      this.m_textMeshPro.color = (Color) new Color32(start_color.r, start_color.g, start_color.b, (byte) alpha);
      this.m_floatingText_Transform.position += new Vector3(0.0f, starting_Count * Time.deltaTime, 0.0f);
      if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
      {
        this.lastPOS = this.m_cameraTransform.position;
        this.lastRotation = this.m_cameraTransform.rotation;
        this.m_floatingText_Transform.rotation = this.lastRotation;
        Vector3 vector3 = this.m_transform.position - this.lastPOS;
        this.m_transform.forward = new Vector3(vector3.x, 0.0f, vector3.z);
      }
      yield return (object) TextMeshProFloatingText.k_WaitForEndOfFrame;
    }
    yield return (object) TextMeshProFloatingText.k_WaitForSecondsRandom[Random.Range(0, 19)];
    this.m_floatingText_Transform.position = start_pos;
    this.StartCoroutine(this.DisplayTextMeshProFloatingText());
  }

  public IEnumerator DisplayTextMeshFloatingText()
  {
    float CountDuration = 2f;
    float starting_Count = Random.Range(5f, 20f);
    float current_Count = starting_Count;
    Vector3 start_pos = this.m_floatingText_Transform.position;
    Color32 start_color = (Color32) this.m_textMesh.color;
    float alpha = (float) byte.MaxValue;
    float fadeDuration = 3f / starting_Count * CountDuration;
    while ((double) current_Count > 0.0)
    {
      current_Count -= Time.deltaTime / CountDuration * starting_Count;
      if ((double) current_Count <= 3.0)
        alpha = Mathf.Clamp(alpha - (float) ((double) Time.deltaTime / (double) fadeDuration * (double) byte.MaxValue), 0.0f, (float) byte.MaxValue);
      this.m_textMesh.text = ((int) current_Count).ToString();
      this.m_textMesh.color = (Color) new Color32(start_color.r, start_color.g, start_color.b, (byte) alpha);
      this.m_floatingText_Transform.position += new Vector3(0.0f, starting_Count * Time.deltaTime, 0.0f);
      if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
      {
        this.lastPOS = this.m_cameraTransform.position;
        this.lastRotation = this.m_cameraTransform.rotation;
        this.m_floatingText_Transform.rotation = this.lastRotation;
        Vector3 vector3 = this.m_transform.position - this.lastPOS;
        this.m_transform.forward = new Vector3(vector3.x, 0.0f, vector3.z);
      }
      yield return (object) TextMeshProFloatingText.k_WaitForEndOfFrame;
    }
    yield return (object) TextMeshProFloatingText.k_WaitForSecondsRandom[Random.Range(0, 20)];
    this.m_floatingText_Transform.position = start_pos;
    this.StartCoroutine(this.DisplayTextMeshFloatingText());
  }
}
