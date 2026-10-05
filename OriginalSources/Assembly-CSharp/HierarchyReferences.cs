// Decompiled with JetBrains decompiler
// Type: HierarchyReferences
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/HierarchyReferences")]
public class HierarchyReferences : KMonoBehaviour
{
  public ElementReference[] references;

  public bool HasReference(string name)
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
        return true;
    }
    return false;
  }

  public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
      {
        if (reference.behaviour is SpecifiedType)
          return (SpecifiedType) reference.behaviour;
        Debug.LogError((object) string.Format("Behavior is not specified type"));
      }
    }
    Debug.LogError((object) $"Could not find UI reference '{name}' or convert to specified type)");
    return default (SpecifiedType);
  }

  public SpecifiedType TryGetReference<SpecifiedType>(string name) where SpecifiedType : Component
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
      {
        if (reference.behaviour is SpecifiedType)
          return (SpecifiedType) reference.behaviour;
        Debug.LogError((object) string.Format("Behavior is not specified type"));
      }
    }
    return default (SpecifiedType);
  }

  public Component GetReference(string name)
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
        return reference.behaviour;
    }
    Debug.LogWarning((object) $"Couldn't find reference to object named {name} Make sure the name matches the field in the inspector.");
    return (Component) null;
  }
}
