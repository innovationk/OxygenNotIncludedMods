// Decompiled with JetBrains decompiler
// Type: SelfChargingElectrobank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
public class SelfChargingElectrobank : Electrobank
{
  [Serialize]
  private float lifetimeRemaining = 90000f;
  private KSelectable selectable;
  private Guid lifetimeStatus = Guid.Empty;

  public float LifetimeRemaining => this.lifetimeRemaining;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.selectable = this.GetComponent<KSelectable>();
    this.selectable.AddStatusItem(Db.Get().MiscStatusItems.ElectrobankSelfCharging, (object) 60f);
    this.lifetimeStatus = this.selectable.AddStatusItem(Db.Get().MiscStatusItems.ElectrobankLifetimeRemaining, (object) this);
    Components.SelfChargingElectrobanks.Add(this.gameObject.GetMyWorldId(), this);
    if ((double) this.lifetimeRemaining > 0.0)
      return;
    this.Delete();
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Mass = 20f;
  }

  public override void Sim200ms(float dt)
  {
    base.Sim200ms(dt);
    if ((double) this.lifetimeRemaining > 0.0)
    {
      double num = (double) this.AddPower(dt * 60f);
      this.lifetimeRemaining -= dt;
    }
    else
      this.Explode();
  }

  public override void Explode()
  {
    Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, this.gameObject.transform.position, 0.0f);
    KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode"), this.gameObject.transform.position);
    this.LaunchNearbyStuff();
    SimMessages.AddRemoveSubstance(Grid.PosToCell(this.transform.position), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, 20f, 3000f, Db.Get().Diseases.GetIndex((HashedString) Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt(1E+07f));
    if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null)
    {
      Storage component1 = this.transform.parent.GetComponent<Storage>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        Health component2 = component1.GetComponent<Health>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.Damage(500f);
      }
    }
    this.Delete();
  }

  private void Delete()
  {
    if (this.IsNullOrDestroyed() || this.gameObject.IsNullOrDestroyed())
      return;
    this.gameObject.DeleteObject();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.SelfChargingElectrobanks.Remove(this.gameObject.GetMyWorldId(), this);
  }
}
