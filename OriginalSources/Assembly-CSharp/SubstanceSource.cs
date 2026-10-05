// Decompiled with JetBrains decompiler
// Type: SubstanceSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
  private bool enableRefresh;
  private static readonly float MaxPickupTime = 8f;
  [MyCmpReq]
  public Pickupable pickupable;
  [MyCmpReq]
  private PrimaryElement primaryElement;

  protected override void OnPrefabInit()
  {
    this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);
  }

  protected override void OnSpawn() => this.pickupable.SetWorkTime(10f);

  protected abstract CellOffset[] GetOffsetGroup();

  protected abstract IChunkManager GetChunkManager();

  public SimHashes GetElementID() => this.primaryElement.ElementID;

  public Tag GetElementTag()
  {
    Tag elementTag = Tag.Invalid;
    if ((Object) this.gameObject != (Object) null && (Object) this.primaryElement != (Object) null && this.primaryElement.Element != null)
      elementTag = this.primaryElement.Element.tag;
    return elementTag;
  }

  public Tag GetMaterialCategoryTag()
  {
    Tag materialCategoryTag = Tag.Invalid;
    if ((Object) this.gameObject != (Object) null && (Object) this.primaryElement != (Object) null && this.primaryElement.Element != null)
      materialCategoryTag = this.primaryElement.Element.GetMaterialCategoryTag();
    return materialCategoryTag;
  }
}
