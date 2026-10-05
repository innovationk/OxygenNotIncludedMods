// Decompiled with JetBrains decompiler
// Type: LogicModeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LogicModeUI : ScriptableObject
{
  [Header("Base Assets")]
  public Sprite inputSprite;
  public Sprite outputSprite;
  public Sprite resetSprite;
  public GameObject prefab;
  public GameObject ribbonInputPrefab;
  public GameObject ribbonOutputPrefab;
  public GameObject controlInputPrefab;
  [Header("Colouring")]
  public Color32 colourOn = new Color32((byte) 0, byte.MaxValue, (byte) 0, (byte) 0);
  public Color32 colourOff = new Color32(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0);
  public Color32 colourDisconnected = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
  public Color32 colourOnProtanopia = new Color32((byte) 179, (byte) 204, (byte) 0, (byte) 0);
  public Color32 colourOffProtanopia = new Color32((byte) 166, (byte) 51, (byte) 102, (byte) 0);
  public Color32 colourOnDeuteranopia = new Color32((byte) 128 /*0x80*/, (byte) 0, (byte) 128 /*0x80*/, (byte) 0);
  public Color32 colourOffDeuteranopia = new Color32(byte.MaxValue, (byte) 153, (byte) 0, (byte) 0);
  public Color32 colourOnTritanopia = new Color32((byte) 51, (byte) 102, byte.MaxValue, (byte) 0);
  public Color32 colourOffTritanopia = new Color32(byte.MaxValue, (byte) 153, (byte) 0, (byte) 0);
}
