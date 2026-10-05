// Decompiled with JetBrains decompiler
// Type: EntityConfigManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/EntityConfigManager")]
public class EntityConfigManager : KMonoBehaviour
{
  public static EntityConfigManager Instance;

  public static void DestroyInstance() => EntityConfigManager.Instance = (EntityConfigManager) null;

  protected override void OnPrefabInit() => EntityConfigManager.Instance = this;

  private static int GetSortOrder(System.Type type)
  {
    foreach (Attribute customAttribute in type.GetCustomAttributes(true))
    {
      if (customAttribute.GetType() == typeof (EntityConfigOrder))
        return (customAttribute as EntityConfigOrder).sortOrder;
    }
    return 0;
  }

  public void LoadGeneratedEntities(List<System.Type> types)
  {
    System.Type type1 = typeof (IEntityConfig);
    System.Type type2 = typeof (IMultiEntityConfig);
    List<EntityConfigManager.ConfigEntry> configEntryList = new List<EntityConfigManager.ConfigEntry>();
    foreach (System.Type type3 in types)
    {
      if ((type1.IsAssignableFrom(type3) || type2.IsAssignableFrom(type3)) && !type3.IsAbstract && !type3.IsInterface)
      {
        int sortOrder = EntityConfigManager.GetSortOrder(type3);
        EntityConfigManager.ConfigEntry configEntry = new EntityConfigManager.ConfigEntry()
        {
          type = type3,
          sortOrder = sortOrder
        };
        configEntryList.Add(configEntry);
      }
    }
    configEntryList.Sort((Comparison<EntityConfigManager.ConfigEntry>) ((x, y) => x.sortOrder.CompareTo(y.sortOrder)));
    foreach (EntityConfigManager.ConfigEntry configEntry in configEntryList)
    {
      object instance = Activator.CreateInstance(configEntry.type);
      if (instance is IEntityConfig)
      {
        IEntityConfig config = instance as IEntityConfig;
        string[] requiredDlcIds = (string[]) null;
        string[] forbiddenDlcIds = (string[]) null;
        if (config.GetDlcIds() != null)
        {
          DlcManager.ConvertAvailableToRequireAndForbidden(config.GetDlcIds(), out requiredDlcIds, out forbiddenDlcIds);
          DebugUtil.DevLogError($"{configEntry.type} implements GetDlcIds, which is obsolete.");
        }
        else if (instance is IHasDlcRestrictions hasDlcRestrictions)
        {
          requiredDlcIds = hasDlcRestrictions.GetRequiredDlcIds();
          forbiddenDlcIds = hasDlcRestrictions.GetForbiddenDlcIds();
        }
        if (DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
          this.RegisterEntity(config, requiredDlcIds, forbiddenDlcIds);
      }
      if (instance is IMultiEntityConfig config1)
      {
        DebugUtil.Assert(!(instance is IHasDlcRestrictions), "IMultiEntityConfig cannot implement IHasDlcRestrictions, wrap the individual config instead.");
        this.RegisterEntities(config1);
      }
    }
  }

  [Conditional("UNITY_EDITOR")]
  private void ValidateEntityConfig(IEntityConfig entityConfig)
  {
    System.Type c = entityConfig != null ? entityConfig.GetType() : throw new ArgumentNullException(nameof (entityConfig));
    System.Type type = typeof (IHasDlcRestrictions);
    int num1 = c.GetMethod("GetRequiredDlcIds", System.Type.EmptyTypes) != (MethodInfo) null ? 1 : 0;
    bool flag1 = c.GetMethod("GetForbiddenDlcIds", System.Type.EmptyTypes) != (MethodInfo) null;
    bool flag2 = type.IsAssignableFrom(c);
    int num2 = flag1 ? 1 : 0;
    if ((num1 | num2) == 0 || flag2)
      return;
    DebugUtil.LogErrorArgs((object) (c.Name + " is an IEntityConfig and has GetRequiredDlcIds or GetForbiddenDlcIds but does not implement IHasDlcRestrictions."));
  }

  [Conditional("UNITY_EDITOR")]
  private void ValidateMultiEntityConfig(IMultiEntityConfig entityConfig)
  {
    System.Type type = entityConfig != null ? entityConfig.GetType() : throw new ArgumentNullException(nameof (entityConfig));
    if (!(type.GetMethod("GetRequiredDlcIds", System.Type.EmptyTypes) != (MethodInfo) null | type.GetMethod("GetForbiddenDlcIds", System.Type.EmptyTypes) != (MethodInfo) null))
      return;
    DebugUtil.LogErrorArgs((object) (type.Name + " is an IMultiEntityConfig and you shouldn't be specifying GetRequiredDlcIds or GetForbiddenDlcIds. Wrap each config in a DLC check instead."));
  }

  public void RegisterEntity(
    IEntityConfig config,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
  {
    GameObject prefab = config.CreatePrefab();
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return;
    KPrefabID component = prefab.GetComponent<KPrefabID>();
    component.requiredDlcIds = requiredDlcIds;
    component.forbiddenDlcIds = forbiddenDlcIds;
    component.prefabInitFn += new KPrefabID.PrefabFn(config.OnPrefabInit);
    component.prefabSpawnFn += new KPrefabID.PrefabFn(config.OnSpawn);
    Assets.AddPrefab(component);
  }

  public void RegisterEntities(IMultiEntityConfig config)
  {
    foreach (GameObject prefab in config.CreatePrefabs())
    {
      KPrefabID component = prefab.GetComponent<KPrefabID>();
      component.prefabInitFn += new KPrefabID.PrefabFn(config.OnPrefabInit);
      component.prefabSpawnFn += new KPrefabID.PrefabFn(config.OnSpawn);
      Assets.AddPrefab(component);
    }
  }

  private struct ConfigEntry
  {
    public System.Type type;
    public int sortOrder;
  }
}
