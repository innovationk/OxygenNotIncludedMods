// Decompiled with JetBrains decompiler
// Type: ToolParameterMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ToolParameterMenu")]
public class ToolParameterMenu : KMonoBehaviour
{
  public GameObject content;
  public GameObject widgetContainer;
  public GameObject widgetPrefab;
  private Dictionary<string, ToolParameterMenu.Widget> widgets = new Dictionary<string, ToolParameterMenu.Widget>();
  private ToolParameterMenu.ToggleData[] currentTogglesData;
  private string lastEnabledFilter;

  public event System.Action onParametersChanged;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ClearMenu();
  }

  private int ToggleStateToMultiToggleInt(ToolParameterMenu.ToggleData data)
  {
    switch (data.state)
    {
      case ToolParameterMenu.ToggleState.On:
        return !data.isToggleInclusive ? 1 : 3;
      case ToolParameterMenu.ToggleState.Off:
        return 0;
      case ToolParameterMenu.ToggleState.Disabled:
        return 2;
      default:
        return 0;
    }
  }

  public void PopulateMenu(ToolParameterMenu.ToggleData[] togglesData)
  {
    this.ClearMenu();
    this.currentTogglesData = togglesData;
    bool flag = true;
    foreach (ToolParameterMenu.ToggleData toggleData in togglesData)
    {
      if (toggleData.isToggleInclusive)
      {
        flag = false;
        break;
      }
    }
    this.widgetContainer.GetComponent<ToggleGroup>().enabled = flag;
    for (int index = 0; index < togglesData.Length; ++index)
    {
      ToolParameterMenu.ToggleData data = togglesData[index];
      GameObject toggleGameObject = this.CreateToggleGameObject(data);
      this.widgets.Add(data.name, new ToolParameterMenu.Widget()
      {
        gameObject = toggleGameObject,
        data = data
      });
    }
    this.content.SetActive(true);
  }

  private GameObject CreateToggleGameObject(ToolParameterMenu.ToggleData data)
  {
    GameObject newWidget = Util.KInstantiateUI(this.widgetPrefab, this.widgetContainer, true);
    LocText componentInChildren1 = newWidget.GetComponentInChildren<LocText>();
    ToolTip componentInChildren2 = newWidget.GetComponentInChildren<ToolTip>();
    MultiToggle componentInChildren3 = newWidget.GetComponentInChildren<MultiToggle>();
    int state = (int) data.state;
    string str = (string) Strings.Get($"STRINGS.UI.TOOLS.FILTERLAYERS.{data.name}.NAME");
    componentInChildren1.text = str;
    if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null)
      componentInChildren2.SetSimpleTooltip((string) Strings.Get($"STRINGS.UI.TOOLS.FILTERLAYERS.{data.name}.TOOLTIP"));
    componentInChildren3.ChangeState(this.ToggleStateToMultiToggleInt(data));
    componentInChildren3.onClick += (System.Action) (() =>
    {
      foreach (KeyValuePair<string, ToolParameterMenu.Widget> widget in this.widgets)
      {
        ToolParameterMenu.Widget clickedWidget = widget.Value;
        ToolParameterMenu.ToggleData data1 = clickedWidget.data;
        if ((UnityEngine.Object) clickedWidget.gameObject == (UnityEngine.Object) newWidget)
        {
          if (data1.state == ToolParameterMenu.ToggleState.Disabled)
            break;
          this.ChangeToSetting(clickedWidget);
          this.OnChange();
          break;
        }
      }
    });
    return newWidget;
  }

  public void ClearMenu()
  {
    this.content.SetActive(false);
    foreach (KeyValuePair<string, ToolParameterMenu.Widget> widget in this.widgets)
      Util.KDestroyGameObject(widget.Value.gameObject);
    this.widgets.Clear();
  }

  private void ChangeToSetting(ToolParameterMenu.Widget clickedWidget)
  {
    ToolParameterMenu.ToggleData data1 = clickedWidget.data;
    if (data1.isToggleInclusive)
    {
      data1.state = data1.state == ToolParameterMenu.ToggleState.Off ? ToolParameterMenu.ToggleState.On : ToolParameterMenu.ToggleState.Off;
      foreach (KeyValuePair<string, ToolParameterMenu.Widget> widget in this.widgets)
      {
        ToolParameterMenu.ToggleData data2 = widget.Value.data;
        if (data2.state != ToolParameterMenu.ToggleState.Disabled && !data1.isToggleInclusive)
          data2.state = ToolParameterMenu.ToggleState.Off;
      }
    }
    else
    {
      foreach (KeyValuePair<string, ToolParameterMenu.Widget> widget in this.widgets)
      {
        ToolParameterMenu.ToggleData data3 = widget.Value.data;
        if (data3.state != ToolParameterMenu.ToggleState.Disabled)
          data3.state = ToolParameterMenu.ToggleState.Off;
      }
      data1.state = ToolParameterMenu.ToggleState.On;
    }
  }

  private void OnChange()
  {
    foreach (KeyValuePair<string, ToolParameterMenu.Widget> widget1 in this.widgets)
    {
      ToolParameterMenu.Widget widget2 = widget1.Value;
      ToolParameterMenu.ToggleData data = widget2.data;
      GameObject gameObject = widget2.gameObject;
      int multiToggleInt = this.ToggleStateToMultiToggleInt(data);
      gameObject.GetComponentInChildren<MultiToggle>().ChangeState(multiToggleInt);
    }
    if (this.onParametersChanged == null)
      return;
    this.onParametersChanged();
  }

  public string GetLastEnabledFilter() => this.lastEnabledFilter;

  public class FILTERLAYERS
  {
    public static string BUILDINGS = nameof (BUILDINGS);
    public static string TILES = nameof (TILES);
    public static string WIRES = nameof (WIRES);
    public static string LIQUIDCONDUIT = "LIQUIDPIPES";
    public static string GASCONDUIT = "GASPIPES";
    public static string SOLIDCONDUIT = "SOLIDCONDUITS";
    public static string CLEANANDCLEAR = nameof (CLEANANDCLEAR);
    public static string DIGPLACER = nameof (DIGPLACER);
    public static string LOGIC = nameof (LOGIC);
    public static string BACKWALL = nameof (BACKWALL);
    public static string NATURALBACKWALL = nameof (NATURALBACKWALL);
    public static string UPROOTPLANTS = nameof (UPROOTPLANTS);
    public static string CONSTRUCTION = nameof (CONSTRUCTION);
    public static string DIG = nameof (DIG);
    public static string CLEAN = nameof (CLEAN);
    public static string OPERATE = nameof (OPERATE);
    public static string METAL = nameof (METAL);
    public static string BUILDABLE = nameof (BUILDABLE);
    public static string FILTER = nameof (FILTER);
    public static string LIQUIFIABLE = nameof (LIQUIFIABLE);
    public static string LIQUID = nameof (LIQUID);
    public static string CONSUMABLEORE = nameof (CONSUMABLEORE);
    public static string ORGANICS = nameof (ORGANICS);
    public static string FARMABLE = nameof (FARMABLE);
    public static string GAS = nameof (GAS);
    public static string MISC = nameof (MISC);
    public static string HEATFLOW = nameof (HEATFLOW);
    public static string ABSOLUTETEMPERATURE = nameof (ABSOLUTETEMPERATURE);
    public static string RELATIVETEMPERATURE = nameof (RELATIVETEMPERATURE);
    public static string ADAPTIVETEMPERATURE = nameof (ADAPTIVETEMPERATURE);
    public static string STATECHANGE = nameof (STATECHANGE);
    public static string ALL = nameof (ALL);
  }

  public class ToggleData
  {
    public string name;
    public bool isToggleInclusive;
    public ToolParameterMenu.ToggleState state;

    public bool IsOn => this.state == ToolParameterMenu.ToggleState.On;

    public ToggleData()
    {
    }

    public ToggleData(string name, ToolParameterMenu.ToggleState state, bool isToggleInclusive = false)
    {
      this.name = name;
      this.state = state;
      this.isToggleInclusive = isToggleInclusive;
    }
  }

  private class Widget
  {
    public GameObject gameObject;
    public ToolParameterMenu.ToggleData data;
  }

  public enum ToggleState
  {
    On,
    Off,
    Disabled,
  }
}
