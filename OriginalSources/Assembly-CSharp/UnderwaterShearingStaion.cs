// Decompiled with JetBrains decompiler
// Type: UnderwaterShearingStaion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UnderwaterShearingStaion : KMonoBehaviour
{
  private KBatchedAnimController symbolController;
  private static HashedString SYMBOL_HASH = (HashedString) "object";
  private Storage storage;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.storage = this.GetComponent<Storage>();
    this.SetupShearableSymbol();
  }

  public void UpdateShearableSymbol(Tag item_tag)
  {
    this.symbolController.gameObject.SetActive(true);
    this.symbolController.SwapAnims(Assets.GetPrefab(item_tag).GetComponent<KBatchedAnimController>().AnimFiles);
    this.symbolController.Play((HashedString) "idle1", KAnim.PlayMode.Loop);
  }

  public void HideShearableSymbol() => this.symbolController.gameObject.SetActive(false);

  public void SetupShearableSymbol()
  {
    KBatchedAnimController component = this.gameObject.GetComponent<KBatchedAnimController>();
    KBatchedAnimController[] componentsInChildren = this.gameObject.GetComponentsInChildren<KBatchedAnimController>(true);
    GameObject gameObject = Util.NewGameObject(this.gameObject, this.gameObject.name + ".ore_symbol");
    gameObject.SetActive(false);
    Vector3 column = (Vector3) component.GetSymbolTransform(UnderwaterShearingStaion.SYMBOL_HASH, out bool _).GetColumn(3) with
    {
      z = component.transform.GetPosition().z - 0.05f
    };
    gameObject.transform.SetPosition(column);
    this.symbolController = gameObject.AddComponent<KBatchedAnimController>();
    this.symbolController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "hematite_kanim")
    };
    this.symbolController.initialAnim = "idle1";
    component.SetSymbolVisiblity((KAnimHashedString) UnderwaterShearingStaion.SYMBOL_HASH, false);
    foreach (KAnimControllerBase kanimControllerBase in componentsInChildren)
      kanimControllerBase.SetSymbolVisiblity((KAnimHashedString) UnderwaterShearingStaion.SYMBOL_HASH, false);
    KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.symbol = UnderwaterShearingStaion.SYMBOL_HASH;
    kbatchedAnimTracker.offset = Vector3.zero;
  }
}
