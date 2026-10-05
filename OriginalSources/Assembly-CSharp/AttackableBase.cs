// Decompiled with JetBrains decompiler
// Type: AttackableBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/AttackableBase")]
public class AttackableBase : Workable, IApproachable
{
  private HandleVector<int>.Handle scenePartitionerEntry;
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<AttackableBase>(GameTags.Dead, (Action<AttackableBase, object>) ((component, data) => component.OnDefeated(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((Action<AttackableBase, object>) ((component, data) => component.OnDefeated(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> SetupScenePartitionerDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((Action<AttackableBase, object>) ((component, data) => component.SetupScenePartitioner(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnCellChangedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((Action<AttackableBase, object>) ((component, data) => GameScenePartitioner.Instance.UpdatePosition(component.scenePartitionerEntry, Grid.PosToCell(component.gameObject))));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.attributeConverter = Db.Get().AttributeConverters.AttackDamage;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
    this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
    this.SetupScenePartitioner();
    this.Subscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
    GameUtil.SubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate, true);
    this.Subscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
    this.Subscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
  }

  public float GetDamageMultiplier()
  {
    return this.attributeConverter != null && (UnityEngine.Object) this.worker != (UnityEngine.Object) null ? Mathf.Max(1f + this.worker.GetAttributeConverter(this.attributeConverter.Id).Evaluate(), 0.1f) : 1f;
  }

  private void SetupScenePartitioner(object data = null)
  {
    Extents extents = new Extents(Grid.PosToXY(this.transform.GetPosition()).x, Grid.PosToXY(this.transform.GetPosition()).y, 1, 1);
    this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(this.gameObject.name, (object) this.GetComponent<FactionAlignment>(), extents, GameScenePartitioner.Instance.attackableEntitiesLayer, (Action<object>) null);
  }

  private void OnDefeated(object data = null)
  {
    GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
  }

  public override float GetEfficiencyMultiplier(WorkerBase worker) => 1f;

  protected override void OnCleanUp()
  {
    this.Unsubscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
    GameUtil.UnsubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate);
    this.Unsubscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
    this.Unsubscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
    GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
    base.OnCleanUp();
  }
}
