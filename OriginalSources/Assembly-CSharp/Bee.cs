// Decompiled with JetBrains decompiler
// Type: Bee
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class Bee : KMonoBehaviour
{
  public float radiationOutputAmount;
  private Dictionary<HashedString, float> radiationModifiers = new Dictionary<HashedString, float>();
  private float unhappyRadiationMod = 0.1f;
  private float awakeRadiationMod;
  private HashedString unhappyRadiationModKey = (HashedString) "UNHAPPY";
  private HashedString awakeRadiationModKey = (HashedString) "AWAKE";
  private static readonly EventSystem.IntraObjectHandler<Bee> OnAttackDelegate = new EventSystem.IntraObjectHandler<Bee>((Action<Bee, object>) ((component, data) => component.OnAttack(data)));
  private static readonly EventSystem.IntraObjectHandler<Bee> OnSleepDelegate = new EventSystem.IntraObjectHandler<Bee>((Action<Bee, object>) ((component, data) => component.StartSleep()));
  private static readonly EventSystem.IntraObjectHandler<Bee> OnWakeUpDelegate = new EventSystem.IntraObjectHandler<Bee>((Action<Bee, object>) ((component, data) => component.StopSleep()));
  private static readonly EventSystem.IntraObjectHandler<Bee> OnDeathDelegate = new EventSystem.IntraObjectHandler<Bee>((Action<Bee, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Bee> OnUnhappyDelegate = new EventSystem.IntraObjectHandler<Bee>((Action<Bee, object>) ((component, data) => component.AddRadiationModifier(component.unhappyRadiationModKey, component.unhappyRadiationMod)));
  private static readonly EventSystem.IntraObjectHandler<Bee> OnSatisfiedDelegate = new EventSystem.IntraObjectHandler<Bee>((Action<Bee, object>) ((component, data) => component.RemoveRadiationMod(component.unhappyRadiationModKey)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Bee>(-739654666, Bee.OnAttackDelegate);
    this.Subscribe<Bee>(-1283701846, Bee.OnSleepDelegate);
    this.Subscribe<Bee>(-2090444759, Bee.OnWakeUpDelegate);
    this.Subscribe<Bee>(1623392196, Bee.OnDeathDelegate);
    this.Subscribe<Bee>(49018834, Bee.OnSatisfiedDelegate);
    this.Subscribe<Bee>(-647798969, Bee.OnUnhappyDelegate);
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "tag", false);
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "snapto_tag", false);
    this.StopSleep();
  }

  private void OnDeath(object data)
  {
    PrimaryElement component1 = this.GetComponent<PrimaryElement>();
    Storage component2 = this.GetComponent<Storage>();
    byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
    component2.AddOre(SimHashes.NuclearWaste, BeeTuning.WASTE_DROPPED_ON_DEATH, component1.Temperature, index, BeeTuning.GERMS_DROPPED_ON_DEATH);
    component2.DropAll(this.transform.position, true, true);
  }

  private void StartSleep()
  {
    this.RemoveRadiationMod(this.awakeRadiationModKey);
    this.GetComponent<ElementConsumer>().EnableConsumption(true);
  }

  private void StopSleep()
  {
    this.AddRadiationModifier(this.awakeRadiationModKey, this.awakeRadiationMod);
    this.GetComponent<ElementConsumer>().EnableConsumption(false);
  }

  private void AddRadiationModifier(HashedString name, float mod)
  {
    this.radiationModifiers.Add(name, mod);
    this.RefreshRadiationOutput();
  }

  private void RemoveRadiationMod(HashedString name)
  {
    this.radiationModifiers.Remove(name);
    this.RefreshRadiationOutput();
  }

  public void RefreshRadiationOutput()
  {
    float radiationOutputAmount = this.radiationOutputAmount;
    foreach (KeyValuePair<HashedString, float> radiationModifier in this.radiationModifiers)
      radiationOutputAmount *= radiationModifier.Value;
    RadiationEmitter component = this.GetComponent<RadiationEmitter>();
    component.SetEmitting(true);
    component.emitRads = radiationOutputAmount;
    component.Refresh();
  }

  private void OnAttack(object data)
  {
    if (!(((Boxed<Tag>) data).value == GameTags.Creatures.Attack))
      return;
    this.GetComponent<Health>().Damage(this.GetComponent<Health>().hitPoints);
  }

  public KPrefabID FindHiveInRoom()
  {
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject);
    Navigator component = this.gameObject.GetComponent<Navigator>();
    int num = int.MaxValue;
    KPrefabID hiveInRoom = (KPrefabID) null;
    foreach (BeeHive.StatesInstance statesInstance in Components.BeeHives.Items)
    {
      if (Game.Instance.roomProber.GetRoomOfGameObject(statesInstance.gameObject) == roomOfGameObject)
      {
        int navigationCost = component.GetNavigationCost(Grid.PosToCell(statesInstance.transform.GetLocalPosition()));
        if (navigationCost < num)
        {
          num = navigationCost;
          hiveInRoom = statesInstance.GetComponent<KPrefabID>();
        }
      }
    }
    return hiveInRoom;
  }
}
