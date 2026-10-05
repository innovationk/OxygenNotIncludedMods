// Decompiled with JetBrains decompiler
// Type: UIRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/prefabs/UIRotator")]
public class UIRotator : KMonoBehaviour
{
  public float minRotationSpeed = 1f;
  public float maxRotationSpeed = 1f;
  public float rotationSpeed = 1f;

  protected override void OnPrefabInit()
  {
    this.rotationSpeed = Random.Range(this.minRotationSpeed, this.maxRotationSpeed);
  }

  private void Update()
  {
    this.GetComponent<RectTransform>().Rotate(0.0f, 0.0f, this.rotationSpeed * Time.unscaledDeltaTime);
  }
}
