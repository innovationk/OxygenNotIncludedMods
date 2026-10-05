// Decompiled with JetBrains decompiler
// Type: Painting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Painting : Artable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.multitoolContext = (HashedString) "paint";
    this.multitoolHitEffectTag = (Tag) "fx_paint_splash";
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Paintings.Add(this);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Paintings.Remove(this);
  }

  public override void SetStage(string stage_id, bool skip_effect)
  {
    base.SetStage(stage_id, skip_effect);
    if (Db.GetArtableStages().Get(stage_id) != null)
      return;
    Debug.LogError((object) ("Missing stage: " + stage_id));
  }
}
