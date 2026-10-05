// Decompiled with JetBrains decompiler
// Type: IPoopStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface IPoopStation
{
  bool IsPoopStationOperational();

  GameObject GetPoopStationObject();

  GameObject GetCurrentPoopStationUser();

  float GetPoopCapacity();

  float GetAvailablePoopCapacityPercentage();

  float GetAvailablePoopCapacity();

  string[] GetPoopingAnimNames();

  bool AttemptToReservePoopStation(GameObject user);

  void ClearPoopStationUser(GameObject user);

  void RegisterPoopStation();

  void UnregisterPoopStation();

  PoopData GetPoopData();

  bool IsUserCompatibleWithPoopStation(KPrefabID user);

  void PlayPoopStationAnim(string animName, KAnim.PlayMode playMode);
}
