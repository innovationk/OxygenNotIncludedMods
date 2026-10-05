// Decompiled with JetBrains decompiler
// Type: HarvestableOverlayWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/HarvestableOverlayWidget")]
public class HarvestableOverlayWidget : KMonoBehaviour
{
  [SerializeField]
  private GameObject vertical_container;
  [SerializeField]
  private GameObject bar;
  private const int icons_per_row = 2;
  private const float bar_fill_range = 19f;
  private const float bar_fill_offset = 3f;
  private static Color growing_color = new Color(0.9843137f, 0.6901961f, 0.23137255f, 1f);
  private static Color wilting_color = new Color(0.5647059f, 0.5647059f, 0.5647059f, 1f);
  [SerializeField]
  private Sprite sprite_liquid;
  [SerializeField]
  private Sprite sprite_atmosphere;
  [SerializeField]
  private Sprite sprite_pressure;
  [SerializeField]
  private Sprite sprite_temperature;
  [SerializeField]
  private Sprite sprite_resource;
  [SerializeField]
  private Sprite sprite_light;
  [SerializeField]
  private Sprite sprite_receptacle;
  [SerializeField]
  private GameObject horizontal_container_prefab;
  private GameObject[] horizontal_containers = new GameObject[7];
  [SerializeField]
  private GameObject icon_gameobject_prefab;
  private Dictionary<WiltCondition.Condition, GameObject> condition_icons = new Dictionary<WiltCondition.Condition, GameObject>();
  private Dictionary<WiltCondition.Condition, Sprite> condition_sprites = new Dictionary<WiltCondition.Condition, Sprite>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.condition_sprites.Add(WiltCondition.Condition.AtmosphereElement, this.sprite_atmosphere);
    this.condition_sprites.Add(WiltCondition.Condition.Darkness, this.sprite_light);
    this.condition_sprites.Add(WiltCondition.Condition.Drowning, this.sprite_liquid);
    this.condition_sprites.Add(WiltCondition.Condition.DryingOut, this.sprite_liquid);
    this.condition_sprites.Add(WiltCondition.Condition.Fertilized, this.sprite_resource);
    this.condition_sprites.Add(WiltCondition.Condition.IlluminationComfort, this.sprite_light);
    this.condition_sprites.Add(WiltCondition.Condition.Irrigation, this.sprite_liquid);
    this.condition_sprites.Add(WiltCondition.Condition.Pressure, this.sprite_pressure);
    this.condition_sprites.Add(WiltCondition.Condition.Temperature, this.sprite_temperature);
    this.condition_sprites.Add(WiltCondition.Condition.Receptacle, this.sprite_receptacle);
    for (int index = 0; index < this.horizontal_containers.Length; ++index)
    {
      GameObject gameObject = Util.KInstantiateUI(this.horizontal_container_prefab, this.vertical_container);
      this.horizontal_containers[index] = gameObject;
    }
    for (int key = 0; key < 15; ++key)
    {
      if (this.condition_sprites.ContainsKey((WiltCondition.Condition) key))
      {
        GameObject gameObject = Util.KInstantiateUI(this.icon_gameobject_prefab, this.gameObject);
        gameObject.GetComponent<Image>().sprite = this.condition_sprites[(WiltCondition.Condition) key];
        this.condition_icons.Add((WiltCondition.Condition) key, gameObject);
      }
    }
  }

  public void Refresh(HarvestDesignatable target_harvestable)
  {
    Image reference = this.bar.GetComponent<HierarchyReferences>().GetReference("Fill") as Image;
    if (target_harvestable.growingStateManager != null)
    {
      float num1 = target_harvestable.growingStateManager.PercentGrown();
      reference.rectTransform.offsetMin = new Vector2(reference.rectTransform.offsetMin.x, 3f);
      if (this.bar.activeSelf != !target_harvestable.CanBeHarvested())
        this.bar.SetActive(!target_harvestable.CanBeHarvested());
      float num2 = target_harvestable.CanBeHarvested() ? 3f : (float) (19.0 - 19.0 * (double) num1 + 3.0);
      reference.rectTransform.offsetMax = new Vector2(reference.rectTransform.offsetMax.x, -num2);
    }
    else if (this.bar.activeSelf)
      this.bar.SetActive(false);
    WiltCondition component = target_harvestable.GetComponent<WiltCondition>();
    if ((Object) component != (Object) null)
    {
      for (int index = 0; index < this.horizontal_containers.Length; ++index)
        this.horizontal_containers[index].SetActive(false);
      foreach (KeyValuePair<WiltCondition.Condition, GameObject> conditionIcon in this.condition_icons)
        conditionIcon.Value.SetActive(false);
      if (component.IsWilting())
      {
        this.vertical_container.SetActive(true);
        reference.color = HarvestableOverlayWidget.wilting_color;
        List<WiltCondition.Condition> conditionList = component.CurrentWiltSources();
        if (conditionList.Count <= 0)
          return;
        for (int index = 0; index < conditionList.Count; ++index)
        {
          if (this.condition_icons.ContainsKey(conditionList[index]))
          {
            this.condition_icons[conditionList[index]].SetActive(true);
            this.horizontal_containers[index / 2].SetActive(true);
            this.condition_icons[conditionList[index]].transform.SetParent(this.horizontal_containers[index / 2].transform);
          }
        }
      }
      else
      {
        this.vertical_container.SetActive(false);
        reference.color = HarvestableOverlayWidget.growing_color;
      }
    }
    else
    {
      reference.color = HarvestableOverlayWidget.growing_color;
      this.vertical_container.SetActive(false);
    }
  }
}
