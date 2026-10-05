// Decompiled with JetBrains decompiler
// Type: KAnimSynchronizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KAnimSynchronizer
{
  private string idle_anim = "idle_default";
  private KAnimControllerBase masterController;
  private List<KAnimControllerBase> Targets = new List<KAnimControllerBase>();
  private readonly Dictionary<KAnimControllerBase, KAnimSynchronizer.TranslateAnimName> targetTranslators = new Dictionary<KAnimControllerBase, KAnimSynchronizer.TranslateAnimName>();
  private List<KAnimSynchronizedController> SyncedControllers = new List<KAnimSynchronizedController>();

  public string IdleAnim
  {
    get => this.idle_anim;
    set => this.idle_anim = value;
  }

  public KAnimSynchronizer(KAnimControllerBase master_controller)
  {
    this.masterController = master_controller;
  }

  private void Clear(KAnimControllerBase controller)
  {
    controller.Play((HashedString) this.IdleAnim, KAnim.PlayMode.Loop);
  }

  public void Add(KAnimControllerBase controller, KAnimSynchronizer.TranslateAnimName translate = null)
  {
    this.Targets.Add(controller);
    if (translate == null)
      return;
    this.targetTranslators[controller] = translate;
  }

  public void Remove(KAnimControllerBase controller)
  {
    this.Clear(controller);
    this.Targets.Remove(controller);
    this.targetTranslators.Remove(controller);
  }

  public void RemoveWithoutIdleAnim(KAnimControllerBase controller)
  {
    this.Targets.Remove(controller);
    this.targetTranslators.Remove(controller);
  }

  private void Clear(KAnimSynchronizedController controller)
  {
    controller.Play((HashedString) this.IdleAnim, KAnim.PlayMode.Loop);
  }

  public void Add(KAnimSynchronizedController controller) => this.SyncedControllers.Add(controller);

  public void Remove(KAnimSynchronizedController controller)
  {
    this.Clear(controller);
    this.SyncedControllers.Remove(controller);
  }

  public void Clear()
  {
    foreach (KAnimControllerBase target in this.Targets)
    {
      if (!((Object) target == (Object) null) && target.AnimFiles != null)
        this.Clear(target);
    }
    this.Targets.Clear();
    this.targetTranslators.Clear();
    foreach (KAnimSynchronizedController syncedController in this.SyncedControllers)
    {
      if (!((Object) syncedController.synchronizedController == (Object) null) && syncedController.synchronizedController.AnimFiles != null)
        this.Clear(syncedController);
    }
    this.SyncedControllers.Clear();
  }

  public void Sync(KAnimControllerBase controller)
  {
    if ((Object) this.masterController == (Object) null || (Object) controller == (Object) null)
      return;
    KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
    if (currentAnim == null)
      return;
    KAnimSynchronizer.TranslateAnimName translateAnimName;
    this.targetTranslators.TryGetValue(controller, out translateAnimName);
    string anim_name = translateAnimName != null ? translateAnimName(currentAnim.name) : currentAnim.name;
    if (!string.IsNullOrEmpty(controller.defaultAnim) && !controller.HasAnimation((HashedString) anim_name))
    {
      controller.Play((HashedString) controller.defaultAnim, KAnim.PlayMode.Loop);
    }
    else
    {
      KAnim.PlayMode mode = this.masterController.GetMode();
      float playSpeed = this.masterController.GetPlaySpeed();
      float elapsedTime = this.masterController.GetElapsedTime();
      controller.Play((HashedString) anim_name, mode, playSpeed, elapsedTime);
      Facing component = controller.GetComponent<Facing>();
      if ((Object) component != (Object) null)
      {
        float target_x = component.transform.GetPosition().x + (this.masterController.FlipX ? -0.5f : 0.5f);
        component.Face(target_x);
      }
      else
      {
        controller.FlipX = this.masterController.FlipX;
        controller.FlipY = this.masterController.FlipY;
      }
    }
  }

  public void SyncController(KAnimSynchronizedController controller)
  {
    if ((Object) this.masterController == (Object) null || controller == null)
      return;
    KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
    string anim_name = currentAnim != null ? currentAnim.name + controller.Postfix : string.Empty;
    if (!string.IsNullOrEmpty(controller.synchronizedController.defaultAnim) && !controller.synchronizedController.HasAnimation((HashedString) anim_name))
    {
      controller.Play((HashedString) controller.synchronizedController.defaultAnim, KAnim.PlayMode.Loop);
    }
    else
    {
      if (currentAnim == null)
        return;
      KAnim.PlayMode mode = this.masterController.GetMode();
      float playSpeed = this.masterController.GetPlaySpeed();
      float elapsedTime = this.masterController.GetElapsedTime();
      controller.Play((HashedString) anim_name, mode, playSpeed, elapsedTime);
      Facing component = controller.synchronizedController.GetComponent<Facing>();
      if ((Object) component != (Object) null)
      {
        float target_x = component.transform.GetPosition().x + (this.masterController.FlipX ? -0.5f : 0.5f);
        component.Face(target_x);
      }
      else
      {
        controller.synchronizedController.FlipX = this.masterController.FlipX;
        controller.synchronizedController.FlipY = this.masterController.FlipY;
      }
    }
  }

  public void Sync()
  {
    for (int index = 0; index < this.Targets.Count; ++index)
      this.Sync(this.Targets[index]);
    for (int index = 0; index < this.SyncedControllers.Count; ++index)
      this.SyncController(this.SyncedControllers[index]);
  }

  public void SyncTime()
  {
    float elapsedTime = this.masterController.GetElapsedTime();
    for (int index = 0; index < this.Targets.Count; ++index)
    {
      KAnimControllerBase target = this.Targets[index];
      target.SetElapsedTime(elapsedTime);
      KBatchedAnimController kbatchedAnimController = target as KBatchedAnimController;
      if ((Object) kbatchedAnimController != (Object) null)
        kbatchedAnimController.UpdateFromSync();
    }
    for (int index = 0; index < this.SyncedControllers.Count; ++index)
      this.SyncedControllers[index].synchronizedController.SetElapsedTime(elapsedTime);
  }

  public delegate string TranslateAnimName(string masterAnimName);
}
