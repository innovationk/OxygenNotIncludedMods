// Decompiled with JetBrains decompiler
// Type: HighEnergyParticleStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HighEnergyParticleStorage : KMonoBehaviour, IStorage
{
  [Serialize]
  [SerializeField]
  private float particles;
  [Serialize]
  public float capacity = float.MaxValue;
  public bool showInUI = true;
  public bool showCapacityStatusItem;
  public bool showCapacityAsMainStatus;
  public bool autoStore;
  [Serialize]
  public bool receiverOpen = true;
  [MyCmpGet]
  private LogicPorts _logicPorts;
  public string PORT_ID = "";
  private static StatusItem capacityStatusItem;

  public float Particles => this.particles;

  public bool allowUIItemRemoval { get; set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.autoStore)
    {
      HighEnergyParticlePort component = this.gameObject.GetComponent<HighEnergyParticlePort>();
      component.onParticleCapture += new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture);
      component.onParticleCaptureAllowed += new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed);
    }
    this.SetupStorageStatusItems();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateLogicPorts();
  }

  private void UpdateLogicPorts()
  {
    if (!((UnityEngine.Object) this._logicPorts != (UnityEngine.Object) null))
      return;
    this._logicPorts.SendSignal((HashedString) this.PORT_ID, this.IsFull() ? 1 : 0);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!this.autoStore)
      return;
    this.gameObject.GetComponent<HighEnergyParticlePort>().onParticleCapture -= new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture);
  }

  private void OnParticleCapture(HighEnergyParticle particle)
  {
    float amount = Mathf.Min(particle.payload, this.capacity - this.particles);
    double num = (double) this.Store(amount);
    particle.payload -= amount;
    if ((double) particle.payload <= 0.0)
      return;
    this.gameObject.GetComponent<HighEnergyParticlePort>().Uncapture(particle);
  }

  private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
  {
    return (double) this.particles < (double) this.capacity && this.receiverOpen;
  }

  private void DeltaParticles(float delta)
  {
    this.particles += delta;
    if ((double) this.particles <= 0.0)
      this.Trigger(155636535, (object) this.transform.gameObject);
    this.Trigger(-1837862626, (object) this.transform.gameObject);
    this.UpdateLogicPorts();
  }

  public float Store(float amount)
  {
    float delta = Mathf.Min(amount, this.RemainingCapacity());
    this.DeltaParticles(delta);
    return delta;
  }

  public float ConsumeAndGet(float amount)
  {
    amount = Mathf.Min(this.Particles, amount);
    this.DeltaParticles(-amount);
    return amount;
  }

  [ContextMenu("Trigger Stored Event")]
  public void DEBUG_TriggerStorageEvent()
  {
    this.Trigger(-1837862626, (object) this.transform.gameObject);
  }

  [ContextMenu("Trigger Zero Event")]
  public void DEBUG_TriggerZeroEvent()
  {
    double num = (double) this.ConsumeAndGet(this.particles + 1f);
  }

  public float ConsumeAll() => this.ConsumeAndGet(this.particles);

  public bool HasRadiation() => (double) this.Particles > 0.0;

  public GameObject Drop(GameObject go, bool do_disease_transfer = true) => (GameObject) null;

  public List<GameObject> GetItems()
  {
    return new List<GameObject>() { this.gameObject };
  }

  public bool IsFull() => (double) this.RemainingCapacity() <= 0.0;

  public bool IsEmpty() => (double) this.Particles == 0.0;

  public float Capacity() => this.capacity;

  public float RemainingCapacity() => Mathf.Max(this.capacity - this.Particles, 0.0f);

  public bool ShouldShowInUI() => this.showInUI;

  public float GetAmountAvailable(Tag tag)
  {
    return tag != GameTags.HighEnergyParticle ? 0.0f : this.Particles;
  }

  public void ConsumeIgnoringDisease(Tag tag, float amount)
  {
    DebugUtil.DevAssert(tag == GameTags.HighEnergyParticle, "Consuming non-particle tag as amount");
    double num = (double) this.ConsumeAndGet(amount);
  }

  private void SetupStorageStatusItems()
  {
    if (HighEnergyParticleStorage.capacityStatusItem == null)
    {
      HighEnergyParticleStorage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      HighEnergyParticleStorage.capacityStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        HighEnergyParticleStorage energyParticleStorage = (HighEnergyParticleStorage) data;
        string newValue1 = Util.FormatWholeNumber(energyParticleStorage.particles);
        string newValue2 = Util.FormatWholeNumber(energyParticleStorage.capacity);
        str = str.Replace("{Stored}", newValue1);
        str = str.Replace("{Capacity}", newValue2);
        str = str.Replace("{Units}", (string) UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES);
        return str;
      });
    }
    if (!this.showCapacityStatusItem)
      return;
    if (this.showCapacityAsMainStatus)
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, HighEnergyParticleStorage.capacityStatusItem, (object) this);
    else
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, HighEnergyParticleStorage.capacityStatusItem, (object) this);
  }
}
