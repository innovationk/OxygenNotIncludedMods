// Decompiled with JetBrains decompiler
// Type: PlantElementAbsorber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public struct PlantElementAbsorber
{
  public Storage storage;
  public PlantElementAbsorber.LocalInfo localInfo;
  public PlantElementAbsorber.ConsumeInfo[] consumedElements;

  public void Clear()
  {
    this.storage = (Storage) null;
    this.consumedElements = (PlantElementAbsorber.ConsumeInfo[]) null;
  }

  private static bool CheckPreConditions(Storage storage, float dt)
  {
    if ((Object) storage == (Object) null)
      return false;
    if ((double) dt > 0.0)
      return true;
    DebugUtil.DevLogError("A delta time of 0 will produce degenerate consumeCommands.");
    return false;
  }

  public static bool PlanConsume(
    Storage storage,
    PlantElementAbsorber.LocalInfo localInfo,
    float dt,
    List<PlantElementAbsorber.Planner.ConsumeCommand> consumeCommands)
  {
    if (!PlantElementAbsorber.CheckPreConditions(storage, dt))
      return false;
    PlantElementAbsorber.Planner planner = new PlantElementAbsorber.Planner(storage);
    int num = planner.PlanConsume(localInfo.massConsumptionRate * dt, localInfo.tag, consumeCommands) ? 1 : 0;
    planner.Recycle();
    return num != 0;
  }

  public static bool PlanConsume(
    Storage storage,
    PlantElementAbsorber.ConsumeInfo[] consumedElements,
    float dt,
    List<PlantElementAbsorber.Planner.ConsumeCommand> consumeCommands)
  {
    if (!PlantElementAbsorber.CheckPreConditions(storage, dt))
      return false;
    PlantElementAbsorber.Planner planner = new PlantElementAbsorber.Planner(storage);
    foreach (PlantElementAbsorber.ConsumeInfo consumedElement in consumedElements)
    {
      consumeCommands?.Clear();
      if (planner.PlanConsume(consumedElement.massConsumptionRate * dt, consumedElement.tag, consumeCommands))
        return OnExit(true);
    }
    return OnExit(false);

    bool OnExit(bool result)
    {
      planner.Recycle();
      return result;
    }
  }

  public readonly bool PlanConsume(
    float dt,
    List<PlantElementAbsorber.Planner.ConsumeCommand> consumeCommands)
  {
    return this.consumedElements != null ? PlantElementAbsorber.PlanConsume(this.storage, this.consumedElements, dt, consumeCommands) : PlantElementAbsorber.PlanConsume(this.storage, this.localInfo, dt, consumeCommands);
  }

  public static float FindLargest(
    Storage storage,
    PlantElementAbsorber.ConsumeInfo[] consumedElements)
  {
    if ((Object) storage == (Object) null || consumedElements.Length == 0)
      return 0.0f;
    PlantElementAbsorber.Planner planner = new PlantElementAbsorber.Planner(storage);
    float largest = -1f;
    foreach (PlantElementAbsorber.ConsumeInfo consumedElement in consumedElements)
    {
      float num = planner.Sum(consumedElement.tag);
      if ((double) largest == -1.0 || (double) num > (double) largest)
        largest = num;
    }
    planner.Recycle();
    return largest;
  }

  public readonly float FindLargest()
  {
    if (this.consumedElements != null)
      return PlantElementAbsorber.FindLargest(this.storage, this.consumedElements);
    if ((Object) this.storage == (Object) null)
      return 0.0f;
    PlantElementAbsorber.Planner planner = new PlantElementAbsorber.Planner(this.storage);
    double largest = (double) planner.Sum(this.localInfo.tag);
    planner.Recycle();
    return (float) largest;
  }

  public struct ConsumeInfo(Tag tag, float mass_consumption_rate)
  {
    public Tag tag = tag;
    public float massConsumptionRate = mass_consumption_rate;
  }

  public struct LocalInfo
  {
    public Tag tag;
    public float massConsumptionRate;
  }

  public readonly struct Planner
  {
    private readonly ListPool<KPrefabID, PlantElementAbsorber.Planner>.PooledList prefabIds;
    private readonly ListPool<PrimaryElement, PlantElementAbsorber.Planner>.PooledList primaryElements;

    public Planner(Storage storage)
    {
      this.prefabIds = ListPool<KPrefabID, PlantElementAbsorber.Planner>.Allocate();
      this.primaryElements = ListPool<PrimaryElement, PlantElementAbsorber.Planner>.Allocate();
      DebugUtil.DevAssert((Object) storage != (Object) null, "Initializing Planner with a null Storage.");
      if ((Object) storage == (Object) null)
        return;
      foreach (GameObject gameObject in storage.items)
      {
        KPrefabID component1;
        PrimaryElement component2;
        if (gameObject.TryGetComponent<KPrefabID>(out component1) && gameObject.TryGetComponent<PrimaryElement>(out component2))
        {
          this.prefabIds.Add(component1);
          this.primaryElements.Add(component2);
        }
      }
    }

    public bool PlanConsume(
      float requiredMass,
      Tag tag,
      List<PlantElementAbsorber.Planner.ConsumeCommand> consumeCommands)
    {
      for (int index = 0; index != this.prefabIds.Count; ++index)
      {
        if (this.prefabIds[index].HasTag(tag))
        {
          PrimaryElement primaryElement = this.primaryElements[index];
          float num = Mathf.Min(requiredMass, primaryElement.Mass);
          requiredMass -= num;
          if (consumeCommands != null)
            consumeCommands.Add(new PlantElementAbsorber.Planner.ConsumeCommand()
            {
              primaryElement = primaryElement,
              deltaMass = num
            });
          if ((double) requiredMass <= 0.0)
            return true;
        }
      }
      return false;
    }

    public float Sum(Tag tag)
    {
      float num = 0.0f;
      for (int index = 0; index != this.prefabIds.Count; ++index)
      {
        if (this.prefabIds[index].HasTag(tag))
          num += this.primaryElements[index].Mass;
      }
      return num;
    }

    public void Recycle()
    {
      this.prefabIds.Recycle();
      this.primaryElements.Recycle();
    }

    public struct ConsumeCommand
    {
      public PrimaryElement primaryElement;
      public float deltaMass;
    }
  }
}
