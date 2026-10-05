// Decompiled with JetBrains decompiler
// Type: AttackEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class AttackEffect
{
  public string effectID;
  public float effectProbability;

  public AttackEffect(string ID, float probability)
  {
    this.effectID = ID;
    this.effectProbability = probability;
  }
}
