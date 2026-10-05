// Decompiled with JetBrains decompiler
// Type: EmptyMilkSeparatorWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class EmptyMilkSeparatorWorkable : Workable
{
  public System.Action OnWork_PST_Begins;
  private static readonly HashedString DROPPED_SYMBOL_HASH = (HashedString) "object";
  private const string DROPPED_SYMBOL_NAME = "object";
  private const string FAT_ON_HAND_SYMBOL_NAME = "fat_goop";
  private KBatchedAnimController droppedItemController;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workLayer = Grid.SceneLayer.BuildingFront;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
    this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_milk_separator_kanim")
    };
    this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.SetWorkTime(15f);
    this.synchronizeAnims = true;
    this.SetupDroppedItemSymbol();
  }

  private void SetupDroppedItemSymbol()
  {
    KBatchedAnimController component = this.gameObject.GetComponent<KBatchedAnimController>();
    GameObject gameObject = Util.NewGameObject(this.gameObject, this.gameObject.name + ".dropped_item_symbol");
    gameObject.SetActive(false);
    Vector3 column = (Vector3) component.GetSymbolTransform(EmptyMilkSeparatorWorkable.DROPPED_SYMBOL_HASH, out bool _).GetColumn(3) with
    {
      z = component.transform.GetPosition().z - 0.05f
    };
    gameObject.transform.SetPosition(column);
    this.droppedItemController = gameObject.AddComponent<KBatchedAnimController>();
    this.droppedItemController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "milkfat_kanim")
    };
    this.droppedItemController.initialAnim = "idle1";
    component.SetSymbolVisiblity((KAnimHashedString) EmptyMilkSeparatorWorkable.DROPPED_SYMBOL_HASH, false);
    KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.symbol = EmptyMilkSeparatorWorkable.DROPPED_SYMBOL_HASH;
    kbatchedAnimTracker.offset = Vector3.zero;
  }

  public override void OnPendingCompleteWork(WorkerBase worker)
  {
    System.Action onWorkPstBegins = this.OnWork_PST_Begins;
    if (onWorkPstBegins != null)
      onWorkPstBegins();
    this.ShowDroppedItemSymbol();
    this.TintDupeFatInHandSymbol(worker);
    base.OnPendingCompleteWork(worker);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    this.HideDroppedItemSymbol();
    this.ClearDupeFatInHandColor(worker);
  }

  private void ShowDroppedItemSymbol()
  {
    MilkSeparator.Instance smi = this.gameObject.GetSMI<MilkSeparator.Instance>();
    if (smi == null)
      return;
    bool flag = (double) smi.MilkFatStored >= (double) smi.CaviarStored;
    this.droppedItemController.SwapAnims(new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) (flag ? "milkfat_kanim" : "caviar_kanim"))
    });
    this.droppedItemController.gameObject.SetActive(true);
    this.droppedItemController.Play((HashedString) (flag ? "idle2" : "object"), KAnim.PlayMode.Loop);
  }

  private void HideDroppedItemSymbol() => this.droppedItemController.gameObject.SetActive(false);

  private void TintDupeFatInHandSymbol(WorkerBase worker)
  {
    if ((UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    MilkSeparator.Instance smi = this.gameObject.GetSMI<MilkSeparator.Instance>();
    if (smi == null)
      return;
    worker.GetComponent<KBatchedAnimController>().SetSymbolTint((KAnimHashedString) "fat_goop", smi.GetFatColor());
  }

  private void ClearDupeFatInHandColor(WorkerBase worker)
  {
    if ((UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    KBatchedAnimController component = worker.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetSymbolTint((KAnimHashedString) "fat_goop", Color.white);
  }
}
