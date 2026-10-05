// Decompiled with JetBrains decompiler
// Type: OilFloaterMovementSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
internal class OilFloaterMovementSound : KMonoBehaviour
{
  public string sound;
  public bool isPlayingSound;
  public bool isMoving;
  private ulong cellChangedHandlerID;
  private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>((Action<OilFloaterMovementSound, object>) ((component, data) => component.OnObjectMovementStateChanged(data)));
  private static readonly Action<object> UpdateSoundDispatcher = (Action<object>) (obj => Unsafe.As<OilFloaterMovementSound>(obj).UpdateSound());

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.sound = GlobalAssets.GetSound(this.sound);
    this.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
    this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, OilFloaterMovementSound.UpdateSoundDispatcher, (object) this, nameof (OilFloaterMovementSound));
  }

  private void OnObjectMovementStateChanged(object data)
  {
    this.isMoving = Boxed<GameHashes>.Unbox(data) == GameHashes.ObjectMovementWakeUp;
    this.UpdateSound();
  }

  private void UpdateSound()
  {
    bool flag = this.isMoving && this.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
    if (flag == this.isPlayingSound)
      return;
    LoopingSounds component = this.GetComponent<LoopingSounds>();
    if (flag)
      component.StartSound(this.sound);
    else
      component.StopSound(this.sound);
    this.isPlayingSound = flag;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
  }
}
