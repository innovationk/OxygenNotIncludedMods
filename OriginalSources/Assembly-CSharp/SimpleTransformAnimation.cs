// Decompiled with JetBrains decompiler
// Type: SimpleTransformAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimpleTransformAnimation : MonoBehaviour
{
  [SerializeField]
  private Vector3 rotationSpeed;
  [SerializeField]
  private Vector3 translateSpeed;

  private void Start()
  {
  }

  private void Update()
  {
    this.transform.Rotate(this.rotationSpeed * Time.unscaledDeltaTime);
    this.transform.Translate(this.translateSpeed * Time.unscaledDeltaTime);
  }
}
