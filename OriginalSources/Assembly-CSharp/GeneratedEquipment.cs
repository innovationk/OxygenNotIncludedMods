// Decompiled with JetBrains decompiler
// Type: GeneratedEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class GeneratedEquipment
{
  public static void LoadGeneratedEquipment(List<System.Type> types)
  {
    System.Type type1 = typeof (IEquipmentConfig);
    List<System.Type> typeList = new List<System.Type>();
    foreach (System.Type type2 in types)
    {
      if (type1.IsAssignableFrom(type2) && !type2.IsAbstract && !type2.IsInterface)
        typeList.Add(type2);
    }
    foreach (System.Type type3 in typeList)
    {
      object instance = Activator.CreateInstance(type3);
      try
      {
        EquipmentConfigManager.Instance.RegisterEquipment(instance as IEquipmentConfig);
      }
      catch (Exception ex)
      {
        DebugUtil.LogException((UnityEngine.Object) null, $"Exception in RegisterEquipment for type {type3.FullName} from {type3.Assembly.GetName().Name}", ex);
      }
    }
  }
}
