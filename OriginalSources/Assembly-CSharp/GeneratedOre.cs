// Decompiled with JetBrains decompiler
// Type: GeneratedOre
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GeneratedOre
{
  public static void LoadGeneratedOre(List<System.Type> types)
  {
    System.Type type1 = typeof (IOreConfig);
    HashSet<SimHashes> simHashesSet = new HashSet<SimHashes>();
    foreach (System.Type type2 in types)
    {
      if (type1.IsAssignableFrom(type2) && !type2.IsAbstract && !type2.IsInterface)
      {
        IOreConfig instance = Activator.CreateInstance(type2) as IOreConfig;
        SimHashes elementId = instance.ElementID;
        Element elementByHash = ElementLoader.FindElementByHash(elementId);
        if (elementByHash != null && DlcManager.IsContentSubscribed(elementByHash.dlcId))
        {
          if (elementId != SimHashes.Void)
            simHashesSet.Add(elementId);
          Assets.AddPrefab(instance.CreatePrefab().GetComponent<KPrefabID>());
        }
      }
    }
    foreach (Element element in ElementLoader.elements)
    {
      if (element != null && !simHashesSet.Contains(element.id) && DlcManager.IsContentSubscribed(element.dlcId) && element.substance != null && (UnityEngine.Object) element.substance.anim != (UnityEngine.Object) null)
      {
        GameObject gameObject = (GameObject) null;
        if (element.IsSolid)
          gameObject = EntityTemplates.CreateSolidOreEntity(element.id);
        else if (element.IsLiquid)
          gameObject = EntityTemplates.CreateLiquidOreEntity(element.id);
        else if (element.IsGas)
          gameObject = EntityTemplates.CreateGasOreEntity(element.id);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
      }
    }
  }

  public static SubstanceChunk CreateChunk(
    Element element,
    float mass,
    float temperature,
    byte diseaseIdx,
    int diseaseCount,
    Vector3 position)
  {
    if ((double) temperature <= 0.0)
      DebugUtil.LogWarningArgs((object) "GeneratedOre.CreateChunk tried to create a chunk with a temperature <= 0");
    GameObject prefab = Assets.GetPrefab(element.tag);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      Debug.LogError((object) ("Could not find prefab for element " + element.id.ToString()));
    GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore);
    SubstanceChunk component1 = gameObject.GetComponent<SubstanceChunk>();
    component1.transform.SetPosition(position);
    gameObject.SetActive(true);
    PrimaryElement component2 = component1.GetComponent<PrimaryElement>();
    component2.Mass = mass;
    component2.Temperature = temperature;
    component2.AddDisease(diseaseIdx, diseaseCount, "GeneratedOre.CreateChunk");
    component1.GetComponent<KPrefabID>().InitializeTags();
    return component1;
  }
}
