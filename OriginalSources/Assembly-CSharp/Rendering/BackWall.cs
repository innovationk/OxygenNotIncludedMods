// Decompiled with JetBrains decompiler
// Type: rendering.BackWall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace rendering;

public class BackWall : MonoBehaviour
{
  [SerializeField]
  public Material backwallMaterial;
  [SerializeField]
  public Texture2DArray array;

  private void Awake() => this.backwallMaterial.SetTexture("images", (Texture) this.array);
}
