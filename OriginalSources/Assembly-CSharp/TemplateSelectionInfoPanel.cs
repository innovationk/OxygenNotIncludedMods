// Decompiled with JetBrains decompiler
// Type: TemplateSelectionInfoPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/TemplateSelectionInfoPanel")]
public class TemplateSelectionInfoPanel : KMonoBehaviour, IRender1000ms
{
  [SerializeField]
  private GameObject prefab_detail_label;
  [SerializeField]
  private GameObject current_detail_container;
  [SerializeField]
  private LocText saved_detail_label;
  [SerializeField]
  private KButton save_button;
  private Func<List<int>, string>[] details = new Func<List<int>, string>[10]
  {
    new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalMass),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageMass),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageTemperature),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalJoules),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.JoulesPerKilogram),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.MassPerElement),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalRadiation),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageRadiation),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageGerms),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalGerms)
  };
  private static List<Tuple<Element, float>> mass_per_element = new List<Tuple<Element, float>>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.details.Length; ++index)
      Util.KInstantiateUI(this.prefab_detail_label, this.current_detail_container, true);
    this.RefreshDetails();
    this.save_button.onClick += new System.Action(this.SaveCurrentDetails);
  }

  public void SaveCurrentDetails()
  {
    string str = "";
    for (int index = 0; index < this.details.Length; ++index)
      str = $"{str}{this.details[index](DebugBaseTemplateButton.Instance.SelectedCells)}\n";
    this.saved_detail_label.text = str + "\n\n" + this.saved_detail_label.text;
  }

  public void Render1000ms(float dt) => this.RefreshDetails();

  public void RefreshDetails()
  {
    for (int index = 0; index < this.details.Length; ++index)
      this.current_detail_container.transform.GetChild(index).GetComponent<LocText>().text = this.details[index](DebugBaseTemplateButton.Instance.SelectedCells);
  }

  private static string TotalMass(List<int> cells)
  {
    float mass = 0.0f;
    foreach (int cell in cells)
      mass += Grid.Mass[cell];
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_MASS, (object) GameUtil.GetFormattedMass(mass));
  }

  private static string AverageMass(List<int> cells)
  {
    float num = 0.0f;
    foreach (int cell in cells)
      num += Grid.Mass[cell];
    float mass = num / (float) cells.Count;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_MASS, (object) GameUtil.GetFormattedMass(mass));
  }

  private static string AverageTemperature(List<int> cells)
  {
    float num = 0.0f;
    foreach (int cell in cells)
      num += Grid.Temperature[cell];
    float temp = num / (float) cells.Count;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_TEMPERATURE, (object) GameUtil.GetFormattedTemperature(temp));
  }

  private static string TotalJoules(List<int> cells)
  {
    List<GameObject> ignoreObjects = new List<GameObject>();
    float joules = 0.0f;
    foreach (int cell in cells)
    {
      joules += (float) ((double) Grid.Element[cell].specificHeatCapacity * (double) Grid.Temperature[cell] * ((double) Grid.Mass[cell] * 1000.0));
      joules += TemplateSelectionInfoPanel.GetCellEntityEnergy(cell, ref ignoreObjects);
    }
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_JOULES, (object) GameUtil.GetFormattedJoules(joules, "F5"));
  }

  private static string AverageGerms(List<int> cells)
  {
    float num1 = 0.0f;
    foreach (int cell in cells)
      num1 += (float) Grid.DiseaseCount[cell];
    float num2 = num1 / (float) cells.Count;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_GERMS, (object) num2);
  }

  private static string TotalGerms(List<int> cells)
  {
    float num = 0.0f;
    foreach (int cell in cells)
      num += (float) Grid.DiseaseCount[cell];
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_GERMS, (object) num);
  }

  private static float GetCellEntityEnergy(int cell, ref List<GameObject> ignoreObjects)
  {
    float cellEntityEnergy = 0.0f;
    for (int layer = 0; layer < 45; ++layer)
    {
      GameObject gameObject1 = Grid.Objects[cell, layer];
      if (!((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null) && !ignoreObjects.Contains(gameObject1))
      {
        ignoreObjects.Add(gameObject1);
        PrimaryElement component1 = gameObject1.GetComponent<PrimaryElement>();
        if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && component1.Element != null)
        {
          float num1 = component1.Mass;
          Building component2 = gameObject1.GetComponent<Building>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            num1 = component2.Def.MassForTemperatureModification;
            if (component2.Def.IsFoundation)
              num1 = 0.0f;
          }
          float num2 = num1 * 1000f * component1.Element.specificHeatCapacity * component1.Temperature;
          cellEntityEnergy += num2;
          Storage[] components = gameObject1.GetComponents<Storage>();
          if (components != null)
          {
            float num3 = 0.0f;
            foreach (Storage storage in components)
            {
              foreach (GameObject gameObject2 in storage.items)
              {
                PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
                if (!((UnityEngine.Object) component3 == (UnityEngine.Object) null))
                  num3 += component3.Mass * 1000f * component3.Element.specificHeatCapacity * component3.Temperature;
              }
            }
            cellEntityEnergy += num3;
          }
          Conduit component4 = gameObject1.GetComponent<Conduit>();
          if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
          {
            ConduitFlow.ConduitContents contents = component4.GetFlowManager().GetContents(cell);
            if ((double) contents.mass > 0.0)
            {
              Element elementByHash = ElementLoader.FindElementByHash(contents.element);
              float num4 = contents.mass * 1000f * elementByHash.specificHeatCapacity * contents.temperature;
              cellEntityEnergy += num4;
            }
          }
          if ((UnityEngine.Object) gameObject1.GetComponent<SolidConduit>() != (UnityEngine.Object) null)
          {
            SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
            SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
            if (contents.pickupableHandle.IsValid())
            {
              Pickupable pickupable = solidConduitFlow.GetPickupable(contents.pickupableHandle);
              if ((bool) (UnityEngine.Object) pickupable)
              {
                PrimaryElement component5 = pickupable.GetComponent<PrimaryElement>();
                if ((double) component5.Mass > 0.0)
                {
                  float num5 = component5.Mass * 1000f * component5.Element.specificHeatCapacity * component5.Temperature;
                  cellEntityEnergy += num5;
                }
              }
            }
          }
        }
      }
    }
    return cellEntityEnergy;
  }

  private static string JoulesPerKilogram(List<int> cells)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (int cell in cells)
    {
      num1 += (float) ((double) Grid.Element[cell].specificHeatCapacity * (double) Grid.Temperature[cell] * ((double) Grid.Mass[cell] * 1000.0));
      num2 += Grid.Mass[cell];
    }
    float joules = num1 / num2;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.JOULES_PER_KILOGRAM, (object) GameUtil.GetFormattedJoules(joules));
  }

  private static string TotalRadiation(List<int> cells)
  {
    float rads = 0.0f;
    foreach (int cell in cells)
      rads += Grid.Radiation[cell];
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_RADS, (object) GameUtil.GetFormattedRads(rads));
  }

  private static string AverageRadiation(List<int> cells)
  {
    float num = 0.0f;
    foreach (int cell in cells)
      num += Grid.Radiation[cell];
    float rads = num / (float) cells.Count;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_RADS, (object) GameUtil.GetFormattedRads(rads));
  }

  private static string MassPerElement(List<int> cells)
  {
    TemplateSelectionInfoPanel.mass_per_element.Clear();
    foreach (int cell in cells)
    {
      bool flag = false;
      for (int index = 0; index < TemplateSelectionInfoPanel.mass_per_element.Count; ++index)
      {
        if (TemplateSelectionInfoPanel.mass_per_element[index].first == Grid.Element[cell])
        {
          TemplateSelectionInfoPanel.mass_per_element[index].second += Grid.Mass[cell];
          flag = true;
          break;
        }
      }
      if (!flag)
        TemplateSelectionInfoPanel.mass_per_element.Add(new Tuple<Element, float>(Grid.Element[cell], Grid.Mass[cell]));
    }
    TemplateSelectionInfoPanel.mass_per_element.Sort((Comparison<Tuple<Element, float>>) ((a, b) =>
    {
      if ((double) a.second > (double) b.second)
        return -1;
      return (double) b.second > (double) a.second ? 1 : 0;
    }));
    string str = "";
    foreach (Tuple<Element, float> tuple in TemplateSelectionInfoPanel.mass_per_element)
      str = $"{str}{tuple.first.name}: {GameUtil.GetFormattedMass(tuple.second)}\n";
    return str;
  }
}
