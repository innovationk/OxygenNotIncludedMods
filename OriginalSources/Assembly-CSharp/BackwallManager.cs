// Decompiled with JetBrains decompiler
// Type: BackwallManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public static class BackwallManager
{
  private static unsafe ushort* element_idx;
  private static unsafe float* mass;
  private static unsafe float* temperature;

  public static unsafe void UpdateFromSim(Sim.GameDataUpdate* data)
  {
    BackwallManager.element_idx = data->backwallElement;
    BackwallManager.mass = data->backwallMass;
    BackwallManager.temperature = data->backwallTemperature;
    for (int index = 0; index < data->numBackwallShouldTransitionInfos; ++index)
    {
      int gameCell = data->backwallShouldTransitionInfos[index].gameCell;
      BackwallManager.BackwallIndexer backwallIndexer = BackwallManager.At(gameCell);
      Element element1 = backwallIndexer.Element;
      if (element1 != null && !element1.IsVacuum)
      {
        ushort num1 = ushort.MaxValue;
        float mass1 = 0.0f;
        bool flag = false;
        backwallIndexer = BackwallManager.At(gameCell);
        float temperature;
        ushort idx;
        float mass2;
        bool isSolid;
        if ((double) backwallIndexer.Temperature > (double) element1.highTemp)
        {
          backwallIndexer = BackwallManager.At(gameCell);
          temperature = backwallIndexer.Temperature - 1.5f;
          idx = element1.highTempTransition.idx;
          double num2 = 1.0 - (double) element1.highTempTransitionOreMassConversion;
          backwallIndexer = BackwallManager.At(gameCell);
          double mass3 = (double) backwallIndexer.Mass;
          mass2 = (float) (num2 * mass3);
          isSolid = element1.highTempTransition.IsSolid;
          if (element1.highTempTransitionOreID != (SimHashes) 0)
          {
            Element elementByHash = ElementLoader.FindElementByHash(element1.highTempTransitionOreID);
            num1 = elementByHash.idx;
            flag = elementByHash.IsSolid;
            backwallIndexer = BackwallManager.At(gameCell);
            mass1 = backwallIndexer.Mass - mass2;
          }
        }
        else
        {
          backwallIndexer = BackwallManager.At(gameCell);
          if ((double) backwallIndexer.Temperature < (double) element1.lowTemp)
          {
            backwallIndexer = BackwallManager.At(gameCell);
            temperature = backwallIndexer.Temperature + 1.5f;
            idx = element1.lowTempTransition.idx;
            double num3 = 1.0 - (double) element1.lowTempTransitionOreMassConversion;
            backwallIndexer = BackwallManager.At(gameCell);
            double mass4 = (double) backwallIndexer.Mass;
            mass2 = (float) (num3 * mass4);
            isSolid = element1.lowTempTransition.IsSolid;
            if (element1.lowTempTransitionOreID != (SimHashes) 0)
            {
              Element elementByHash = ElementLoader.FindElementByHash(element1.lowTempTransitionOreID);
              num1 = elementByHash.idx;
              flag = elementByHash.IsSolid;
              backwallIndexer = BackwallManager.At(gameCell);
              mass1 = backwallIndexer.Mass - mass2;
            }
          }
          else
            continue;
        }
        if (isSolid)
        {
          SimMessages.SetBackwallData(gameCell, idx, mass2, temperature);
        }
        else
        {
          SimMessages.SetBackwallData(gameCell, ElementLoader.GetElementIndex(SimHashes.Vacuum), 0.0f, 0.0f);
          SimMessages.AddRemoveSubstance(gameCell, idx, CellEventLogger.Instance.OreMelted, mass2, temperature, byte.MaxValue, 0);
        }
        if ((double) mass1 > 1.0 / 1000.0)
        {
          if (flag)
          {
            Element element2 = ElementLoader.elements[(int) num1];
            element2.substance.ActivateSubstanceGameObject(element2.substance.SpawnResource(Grid.CellToPos(gameCell), mass1, temperature, byte.MaxValue, 0, true, manual_activation: true), byte.MaxValue, 0);
          }
          else
            SimMessages.AddRemoveSubstance(gameCell, num1, CellEventLogger.Instance.OreMelted, mass1, temperature, byte.MaxValue, 0);
        }
      }
    }
  }

  public static bool HasBackwall(int cell)
  {
    Element element = BackwallManager.At(cell).Element;
    return element != null && element.IsSolid;
  }

  public static BackwallManager.BackwallIndexer At(int index)
  {
    return new BackwallManager.BackwallIndexer(index);
  }

  public static unsafe void Clear()
  {
    BackwallManager.element_idx = (ushort*) null;
    BackwallManager.mass = (float*) null;
    BackwallManager.temperature = (float*) null;
  }

  public readonly struct BackwallIndexer(int index)
  {
    public readonly int index = index;

    public unsafe Element Element
    {
      get
      {
        return BackwallManager.element_idx[this.index] == ushort.MaxValue ? (Element) null : ElementLoader.elements[(int) BackwallManager.element_idx[this.index]];
      }
    }

    public unsafe float Mass => BackwallManager.mass[this.index];

    public unsafe float Temperature => BackwallManager.temperature[this.index];
  }
}
