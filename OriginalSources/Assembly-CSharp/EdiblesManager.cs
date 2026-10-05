// Decompiled with JetBrains decompiler
// Type: EdiblesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/EdiblesManager")]
public class EdiblesManager : KMonoBehaviour
{
  private static List<EdiblesManager.FoodInfo> s_allFoodTypes = new List<EdiblesManager.FoodInfo>();
  private static Dictionary<string, EdiblesManager.FoodInfo> s_allFoodMap = new Dictionary<string, EdiblesManager.FoodInfo>();
  private static List<EdiblesManager.FoodInfo> s_loadedFoodTypes;

  public static List<EdiblesManager.FoodInfo> GetAllLoadedFoodTypes()
  {
    return EdiblesManager.s_allFoodTypes.Where<EdiblesManager.FoodInfo>(new Func<EdiblesManager.FoodInfo, bool>(DlcManager.IsCorrectDlcSubscribed)).ToList<EdiblesManager.FoodInfo>();
  }

  public static void ClearSaveFoodCache()
  {
    EdiblesManager.s_loadedFoodTypes = (List<EdiblesManager.FoodInfo>) null;
  }

  public static List<EdiblesManager.FoodInfo> GetAllFoodTypes()
  {
    if (EdiblesManager.s_loadedFoodTypes == null)
      EdiblesManager.s_loadedFoodTypes = EdiblesManager.s_allFoodTypes.Where<EdiblesManager.FoodInfo>(new Func<EdiblesManager.FoodInfo, bool>(Game.IsCorrectDlcActiveForCurrentSave)).ToList<EdiblesManager.FoodInfo>();
    Debug.Assert((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null, (object) "Call GetAllLoadedFoodTypes from the frontend");
    return EdiblesManager.s_loadedFoodTypes;
  }

  public static EdiblesManager.FoodInfo GetFoodInfo(string foodID)
  {
    string key = foodID.Replace("Compost", "");
    EdiblesManager.FoodInfo foodInfo = (EdiblesManager.FoodInfo) null;
    EdiblesManager.s_allFoodMap.TryGetValue(key, out foodInfo);
    return foodInfo;
  }

  public static bool TryGetFoodInfo(string foodID, out EdiblesManager.FoodInfo info)
  {
    info = (EdiblesManager.FoodInfo) null;
    if (string.IsNullOrEmpty(foodID))
      return false;
    info = EdiblesManager.GetFoodInfo(foodID);
    return info != null;
  }

  public class FoodInfo : IConsumableUIItem, IHasDlcRestrictions
  {
    public string Id;
    public string Name;
    public string Description;
    public float CaloriesPerUnit;
    public float PreserveTemperature;
    public float RotTemperature;
    public float StaleTime;
    public float SpoilTime;
    public bool CanRot;
    public int Quality;
    public List<string> Effects;
    private string[] requiredDlcIds;
    private string[] forbiddenDlcIds;

    public string[] GetRequiredDlcIds() => this.requiredDlcIds;

    public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

    [Obsolete("Use constructor with required/forbidden instead")]
    public FoodInfo(
      string id,
      string dlcId,
      float caloriesPerUnit,
      int quality,
      float preserveTemperatue,
      float rotTemperature,
      float spoilTime,
      bool can_rot)
      : this(id, caloriesPerUnit, quality, preserveTemperatue, rotTemperature, spoilTime, can_rot)
    {
      if (!(dlcId != ""))
        return;
      this.requiredDlcIds = new string[1]{ dlcId };
    }

    public FoodInfo(
      string id,
      float caloriesPerUnit,
      int quality,
      float preserveTemperatue,
      float rotTemperature,
      float spoilTime,
      bool can_rot,
      string[] requiredDlcIds = null,
      string[] forbiddenDlcIds = null)
    {
      this.Id = id;
      this.requiredDlcIds = requiredDlcIds;
      this.forbiddenDlcIds = forbiddenDlcIds;
      this.CaloriesPerUnit = caloriesPerUnit;
      this.Quality = quality;
      this.PreserveTemperature = preserveTemperatue;
      this.RotTemperature = rotTemperature;
      this.StaleTime = spoilTime / 2f;
      this.SpoilTime = spoilTime;
      this.CanRot = can_rot;
      this.Name = (string) Strings.Get($"STRINGS.ITEMS.FOOD.{id.ToUpper()}.NAME");
      this.Description = (string) Strings.Get($"STRINGS.ITEMS.FOOD.{id.ToUpper()}.DESC");
      this.Effects = new List<string>();
      EdiblesManager.s_allFoodTypes.Add(this);
      EdiblesManager.s_allFoodMap[this.Id] = this;
    }

    public EdiblesManager.FoodInfo AddEffects(
      List<string> effects,
      string[] requiredDlcIds = null,
      string[] forbiddenDlcIds = null)
    {
      if (DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
        this.Effects.AddRange((IEnumerable<string>) effects);
      return this;
    }

    public string ConsumableId => this.Id;

    public string ConsumableName => this.Name;

    public int MajorOrder => this.Quality;

    public int MinorOrder => (int) this.CaloriesPerUnit;

    public bool Display => (double) this.CaloriesPerUnit != 0.0;
  }
}
