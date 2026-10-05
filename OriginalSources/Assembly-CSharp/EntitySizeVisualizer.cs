// Decompiled with JetBrains decompiler
// Type: EntitySizeVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class EntitySizeVisualizer : KMonoBehaviour
{
  public OreSizeVisualizerComponents.TiersSetType TierSetType;

  protected override void OnPrefabInit()
  {
    GameComps.OreSizeVisualizers.Add(this.gameObject, new OreSizeVisualizerData(this.gameObject)
    {
      tierSetType = this.TierSetType
    });
    base.OnPrefabInit();
  }

  protected override void OnCleanUp()
  {
    GameComps.OreSizeVisualizers.Remove(this.gameObject);
    base.OnCleanUp();
  }
}
