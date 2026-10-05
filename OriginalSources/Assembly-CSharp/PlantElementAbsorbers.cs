// Decompiled with JetBrains decompiler
// Type: PlantElementAbsorbers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class PlantElementAbsorbers : KCompactedVector<PlantElementAbsorber>
{
  private bool updating;
  private List<HandleVector<int>.Handle> queuedRemoves = new List<HandleVector<int>.Handle>();

  public HandleVector<int>.Handle Add(
    Storage storage,
    PlantElementAbsorber.ConsumeInfo[] consumed_elements)
  {
    if (consumed_elements == null || consumed_elements.Length == 0)
      return HandleVector<int>.InvalidHandle;
    if (consumed_elements.Length == 1)
      return this.Allocate(new PlantElementAbsorber()
      {
        storage = storage,
        consumedElements = (PlantElementAbsorber.ConsumeInfo[]) null,
        localInfo = new PlantElementAbsorber.LocalInfo()
        {
          tag = consumed_elements[0].tag,
          massConsumptionRate = consumed_elements[0].massConsumptionRate
        }
      });
    return this.Allocate(new PlantElementAbsorber()
    {
      storage = storage,
      consumedElements = consumed_elements,
      localInfo = new PlantElementAbsorber.LocalInfo()
      {
        tag = Tag.Invalid,
        massConsumptionRate = 0.0f
      }
    });
  }

  public HandleVector<int>.Handle Remove(HandleVector<int>.Handle h)
  {
    if (this.updating)
      this.queuedRemoves.Add(h);
    else
      this.Free(h);
    return HandleVector<int>.InvalidHandle;
  }

  public void Sim200ms(float dt)
  {
    int count = this.data.Count;
    this.updating = true;
    ListPool<PlantElementAbsorber.Planner.ConsumeCommand, PlantElementAbsorbers>.PooledList consumeCommands = ListPool<PlantElementAbsorber.Planner.ConsumeCommand, PlantElementAbsorbers>.Allocate();
    for (int index = 0; index < count; ++index)
    {
      PlantElementAbsorber plantElementAbsorber = this.data[index];
      consumeCommands.Clear();
      if (plantElementAbsorber.PlanConsume(dt, (List<PlantElementAbsorber.Planner.ConsumeCommand>) consumeCommands))
      {
        foreach (PlantElementAbsorber.Planner.ConsumeCommand consumeCommand in (List<PlantElementAbsorber.Planner.ConsumeCommand>) consumeCommands)
        {
          consumeCommand.primaryElement.Mass -= consumeCommand.deltaMass;
          plantElementAbsorber.storage.Trigger(-1697596308, (object) consumeCommand.primaryElement.gameObject);
        }
        this.data[index] = plantElementAbsorber;
      }
    }
    consumeCommands.Recycle();
    this.updating = false;
    for (int index = 0; index < this.queuedRemoves.Count; ++index)
      this.Remove(this.queuedRemoves[index]);
    this.queuedRemoves.Clear();
  }

  public override void Clear()
  {
    base.Clear();
    for (int index = 0; index < this.data.Count; ++index)
      this.data[index].Clear();
    this.data.Clear();
    this.handles.Clear();
  }

  public PlantElementAbsorbers()
    : base()
  {
  }
}
