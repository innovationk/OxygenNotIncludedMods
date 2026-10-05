// Decompiled with JetBrains decompiler
// Type: MinionStorageDataHolder_StaticHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public static class MinionStorageDataHolder_StaticHelpers
{
  public static void UpdateData<T>(
    this MinionStorageDataHolder dataHolderComponent,
    MinionStorageDataHolder.DataPackData data)
  {
    dataHolderComponent.Internal_UpdateData(typeof (T).ToString(), data);
  }

  public static MinionStorageDataHolder.DataPack GetDataPack<T>(
    this MinionStorageDataHolder dataHolderComponent)
  {
    return dataHolderComponent.Internal_GetDataPack(typeof (T).ToString());
  }
}
