// Decompiled with JetBrains decompiler
// Type: PopFxGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopFxGroup : KMonoBehaviour
{
  public static readonly Vector3 INVALID_SPAWN_POSITION = Vector3.one * -1f;
  public const float INVALID_PADDING = -1f;
  public const float SPAWN_COOLDOWN = 0.1f;
  public const float MAX_PADDING_MULTIPLIER = 2f;
  public const int MAX_ITEM_COUNT_PADDING = 3;
  public const float INDIVIDUAL_PADDING = 1f;
  public Queue<PopFX> spawnQueue = new Queue<PopFX>();
  private float padding = -1f;
  private int lastKeyUsed = -1;
  private bool isLive;
  private float lastSpawnTimeStamp = float.MinValue;
  private Vector3 spawnPosition = PopFxGroup.INVALID_SPAWN_POSITION;

  public void WakeUp(int key)
  {
    if (this.isLive)
      return;
    this.isLive = true;
    this.lastSpawnTimeStamp = float.MinValue;
    this.lastKeyUsed = key;
  }

  public void Enqueue(PopFX effect) => this.spawnQueue.Enqueue(effect);

  public void Update()
  {
    if (!this.isLive || !PopFXManager.Instance.Ready() || (double) Time.unscaledTime - (double) this.lastSpawnTimeStamp < 0.10000000149011612)
      return;
    this.padding = (double) this.padding == -1.0 ? (float) (Mathf.Min(this.spawnQueue.Count, 3) - 1) * 1f : Mathf.Max(this.padding - 1f, 0.0f);
    PopFX popFx = this.spawnQueue.Count > 0 ? this.spawnQueue.Dequeue() : (PopFX) null;
    if ((Object) popFx != (Object) null)
    {
      if (this.spawnPosition == PopFxGroup.INVALID_SPAWN_POSITION)
        this.spawnPosition = popFx.StartPos;
      popFx.Run(this.spawnPosition, Vector3.up * this.padding);
      this.lastSpawnTimeStamp = Time.unscaledTime;
    }
    else
      this.Recycle();
  }

  public void Recycle()
  {
    this.isLive = false;
    this.lastSpawnTimeStamp = float.MinValue;
    this.spawnPosition = PopFxGroup.INVALID_SPAWN_POSITION;
    this.padding = -1f;
    while (this.spawnQueue.Count > 0)
      this.spawnQueue.Dequeue().Recycle();
    PopFXManager.Instance.RecycleFxGroup(this.lastKeyUsed, this);
    this.lastKeyUsed = -1;
    this.gameObject.SetActive(false);
  }
}
