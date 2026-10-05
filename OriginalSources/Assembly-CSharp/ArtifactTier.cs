// Decompiled with JetBrains decompiler
// Type: ArtifactTier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ArtifactTier
{
  public EffectorValues decorValues;
  public StringKey name_key;
  public float payloadDropChance;

  public ArtifactTier(StringKey str_key, EffectorValues values, float payload_drop_chance)
  {
    this.decorValues = values;
    this.name_key = str_key;
    this.payloadDropChance = payload_drop_chance;
  }
}
