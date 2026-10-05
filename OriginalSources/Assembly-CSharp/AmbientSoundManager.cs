// Decompiled with JetBrains decompiler
// Type: AmbientSoundManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AmbientSoundManager")]
public class AmbientSoundManager : KMonoBehaviour
{
  [MyCmpAdd]
  private LoopingSounds loopingSounds;

  public static AmbientSoundManager Instance { get; private set; }

  public static void Destroy() => AmbientSoundManager.Instance = (AmbientSoundManager) null;

  protected override void OnPrefabInit() => AmbientSoundManager.Instance = this;

  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    AmbientSoundManager.Instance = (AmbientSoundManager) null;
  }
}
