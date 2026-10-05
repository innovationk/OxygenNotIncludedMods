// Decompiled with JetBrains decompiler
// Type: ToiletWorkableClean
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableClean")]
public class ToiletWorkableClean : Workable
{
  [Serialize]
  public int timesCleaned;
  private static readonly HashedString[] CLEAN_GUNK_ANIMS = new HashedString[2]
  {
    (HashedString) "degunk_pre",
    (HashedString) "degunk_loop"
  };
  private static readonly HashedString[] CLEAN_ANIMS = new HashedString[2]
  {
    (HashedString) "unclog_pre",
    (HashedString) "unclog_loop"
  };
  private static readonly HashedString[] PST_ANIM = new HashedString[1]
  {
    new HashedString("unclog_pst")
  };
  private static readonly HashedString[] PST_GUNK_ANIM = new HashedString[1]
  {
    new HashedString("degunk_pst")
  };

  public bool IsCloggedByGunk { private set; get; }

  public void SetIsCloggedByGunk(bool isIt)
  {
    this.IsCloggedByGunk = isIt;
    this.workAnims = this.IsCloggedByGunk ? ToiletWorkableClean.CLEAN_GUNK_ANIMS : ToiletWorkableClean.CLEAN_ANIMS;
    this.workingPstComplete = this.IsCloggedByGunk ? ToiletWorkableClean.PST_GUNK_ANIM : ToiletWorkableClean.PST_ANIM;
    this.workingPstFailed = this.IsCloggedByGunk ? ToiletWorkableClean.PST_GUNK_ANIM : ToiletWorkableClean.PST_ANIM;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
    this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
    this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    ToiletWorkableUse component = this.gameObject.GetComponent<ToiletWorkableUse>();
    if ((Object) component != (Object) null && this.IsCloggedByGunk && (Object) this.gameObject.GetComponent<FlushToilet>() == (Object) null)
      LiquidSourceManager.Instance.CreateChunk(SimHashes.LiquidGunk, component.lastAmountOfWasteMassRemovedFromDupe, DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL, byte.MaxValue, 0, Grid.CellToPos(Grid.PosToCell(this.gameObject), CellAlignment.Top, Grid.SceneLayer.Ore));
    ++this.timesCleaned;
    base.OnCompleteWork(worker);
  }
}
