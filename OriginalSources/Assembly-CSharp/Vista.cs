// Decompiled with JetBrains decompiler
// Type: Vista
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Vista : KMonoBehaviour
{
  public string prefabName;
  public string audioName;
  public Grid.SceneLayer sceneLayer;
  public GameObject visualizer;
  public int width;
  public int height;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.transform.SetPosition(new Vector3(this.transform.position.x, this.transform.position.y, Grid.GetLayerZ(this.sceneLayer)));
    this.visualizer = UnityEngine.Object.Instantiate<GameObject>(Assets.instance.vistasPrefabs.Find((Predicate<GameObject>) (p => p.name == this.prefabName)));
    this.visualizer.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (float) this.height / 2f, this.transform.position.z);
    this.visualizer.transform.SetParent(this.transform, true);
    this.visualizer.gameObject.SetActive(true);
    if (string.IsNullOrEmpty(this.audioName))
      return;
    LoopingSounds component = this.GetComponent<LoopingSounds>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.StartSound(GlobalAssets.GetSound(this.audioName));
  }
}
