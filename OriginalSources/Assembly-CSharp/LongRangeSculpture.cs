// Decompiled with JetBrains decompiler
// Type: LongRangeSculpture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class LongRangeSculpture : Sculpture
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = (KAnimFile[]) null;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.multitoolContext = (HashedString) "dig";
    this.multitoolHitEffectTag = (Tag) "fx_dig_splash";
  }
}
