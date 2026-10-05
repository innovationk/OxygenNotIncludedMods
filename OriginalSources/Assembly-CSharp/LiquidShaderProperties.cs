// Decompiled with JetBrains decompiler
// Type: LiquidShaderProperties
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "LiquidShaderProperties", menuName = "Klei/Liquid Shader Properties")]
public class LiquidShaderProperties : ScriptableObject
{
  [SerializeField]
  private LiquidShaderProperties.Entry[] TextureScrollSpeed = new LiquidShaderProperties.Entry[7]
  {
    new LiquidShaderProperties.Entry()
    {
      texture = Substance.SubstanceTexture.Magma,
      scrollSpeed = 0.02f
    },
    new LiquidShaderProperties.Entry()
    {
      texture = Substance.SubstanceTexture.MoltenMetal,
      scrollSpeed = 0.02f
    },
    new LiquidShaderProperties.Entry()
    {
      texture = Substance.SubstanceTexture.Polluted,
      scrollSpeed = 0.02f
    },
    new LiquidShaderProperties.Entry()
    {
      texture = Substance.SubstanceTexture.Oil,
      scrollSpeed = 0.02f
    },
    new LiquidShaderProperties.Entry()
    {
      texture = Substance.SubstanceTexture.Thick,
      scrollSpeed = 0.02f
    },
    new LiquidShaderProperties.Entry()
    {
      texture = Substance.SubstanceTexture.Sap,
      scrollSpeed = 0.02f
    },
    new LiquidShaderProperties.Entry()
    {
      texture = Substance.SubstanceTexture.CrystalFragments,
      scrollSpeed = 0.01f
    }
  };
  private static readonly int SubstanceTextureCount = Enum.GetValues(typeof (Substance.SubstanceTexture)).Length;
  private float[] cachedScrollSpeeds;

  public void ApplyToMaterial(Material material)
  {
    if (this.cachedScrollSpeeds == null)
      this.cachedScrollSpeeds = new float[LiquidShaderProperties.SubstanceTextureCount];
    Array.Clear((Array) this.cachedScrollSpeeds, 0, this.cachedScrollSpeeds.Length);
    for (int index1 = 0; index1 < this.TextureScrollSpeed.Length; ++index1)
    {
      int index2 = (int) (this.TextureScrollSpeed[index1].texture - (byte) 1);
      if (index2 >= 0 && index2 < LiquidShaderProperties.SubstanceTextureCount)
        this.cachedScrollSpeeds[index2] = this.TextureScrollSpeed[index1].scrollSpeed;
    }
    material.SetFloatArray("_TextureScrollSpeeds", this.cachedScrollSpeeds);
  }

  [Serializable]
  public struct Entry
  {
    public Substance.SubstanceTexture texture;
    public float scrollSpeed;
  }
}
