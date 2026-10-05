// Decompiled with JetBrains decompiler
// Type: ModUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using TUNING;
using UnityEngine;

#nullable disable
public static class ModUtil
{
  public static void AddBuildingToPlanScreen(HashedString category, string building_id)
  {
    ModUtil.AddBuildingToPlanScreen(category, building_id, "uncategorized");
  }

  public static void AddBuildingToPlanScreen(
    HashedString category,
    string building_id,
    string subcategoryID)
  {
    ModUtil.AddBuildingToPlanScreen(category, building_id, subcategoryID, (string) null);
  }

  public static void AddBuildingToPlanScreen(
    HashedString category,
    string building_id,
    string subcategoryID,
    string relativeBuildingId,
    ModUtil.BuildingOrdering ordering = ModUtil.BuildingOrdering.After)
  {
    int index1 = BUILDINGS.PLANORDER.FindIndex((Predicate<PlanScreen.PlanInfo>) (x => x.category == category));
    if (index1 < 0)
    {
      Debug.LogWarning((object) $"Mod: Unable to add '{building_id}' as category '{category}' does not exist");
    }
    else
    {
      List<KeyValuePair<string, string>> andSubcategoryData = BUILDINGS.PLANORDER[index1].buildingAndSubcategoryData;
      KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(building_id, subcategoryID);
      if (relativeBuildingId == null)
      {
        andSubcategoryData.Add(keyValuePair);
      }
      else
      {
        int index2 = andSubcategoryData.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == relativeBuildingId));
        if (index2 == -1)
        {
          andSubcategoryData.Add(keyValuePair);
          Debug.LogWarning((object) $"Mod: Building '{relativeBuildingId}' doesn't exist, inserting '{building_id}' at the end of the list instead");
        }
        else
        {
          int index3 = ordering == ModUtil.BuildingOrdering.After ? index2 + 1 : Mathf.Max(index2, 0);
          andSubcategoryData.Insert(index3, keyValuePair);
        }
      }
    }
  }

  [Obsolete("Use PlanScreen instead")]
  public static void AddBuildingToHotkeyBuildMenu(
    HashedString category,
    string building_id,
    Action hotkey)
  {
    BuildMenu.DisplayInfo info = BuildMenu.OrderedBuildings.GetInfo(category);
    if (info.category != category)
      return;
    (info.data as IList<BuildMenu.BuildingInfo>).Add(new BuildMenu.BuildingInfo(building_id, hotkey));
  }

  public static KAnimFile AddKAnimMod(string name, KAnimFile.Mod anim_mod)
  {
    KAnimFile instance = ScriptableObject.CreateInstance<KAnimFile>();
    instance.mod = anim_mod;
    instance.name = name;
    AnimCommandFile akf = new AnimCommandFile();
    KAnimGroupFile.GroupFile gf = new KAnimGroupFile.GroupFile();
    gf.groupID = akf.GetGroupName(instance);
    gf.commandDirectory = "assets/" + name;
    akf.AddGroupFile(gf);
    if (KAnimGroupFile.GetGroupFile().AddAnimMod(gf, akf, instance) == KAnimGroupFile.AddModResult.Added)
      Assets.ModLoadedKAnims.Add(instance);
    return instance;
  }

  public static KAnimFile AddKAnim(
    string name,
    TextAsset anim_file,
    TextAsset build_file,
    IList<Texture2D> textures)
  {
    KAnimFile instance = ScriptableObject.CreateInstance<KAnimFile>();
    instance.Initialize(anim_file, build_file, textures);
    instance.name = name;
    AnimCommandFile akf = new AnimCommandFile();
    KAnimGroupFile.GroupFile gf = new KAnimGroupFile.GroupFile();
    gf.groupID = akf.GetGroupName(instance);
    gf.commandDirectory = "assets/" + name;
    akf.AddGroupFile(gf);
    KAnimGroupFile.GetGroupFile().AddAnimFile(gf, akf, instance);
    Assets.ModLoadedKAnims.Add(instance);
    return instance;
  }

  public static KAnimFile AddKAnim(
    string name,
    TextAsset anim_file,
    TextAsset build_file,
    Texture2D texture)
  {
    return ModUtil.AddKAnim(name, anim_file, build_file, (IList<Texture2D>) new List<Texture2D>()
    {
      texture
    });
  }

  public static Substance CreateSubstance(
    string name,
    Element.State state,
    KAnimFile kanim,
    Material material,
    Color32 colour,
    Color32 ui_colour,
    Color32 conduit_colour)
  {
    return new Substance()
    {
      name = name,
      nameTag = TagManager.Create(name),
      elementID = (SimHashes) Hash.SDBMLower(name),
      anim = kanim,
      colour = colour,
      uiColour = ui_colour,
      conduitColour = conduit_colour,
      material = material,
      renderedByWorld = (state & Element.State.Solid) == Element.State.Solid
    };
  }

  public static void RegisterForTranslation(System.Type locstring_tree_root)
  {
    Localization.RegisterForTranslation(locstring_tree_root);
    Localization.GenerateStringsTemplate(locstring_tree_root, System.IO.Path.Combine(KMod.Manager.GetDirectory(), "strings_templates"));
  }

  public static Texture2D LoadTexture(string path)
  {
    Texture2D tex = (Texture2D) null;
    if (File.Exists(path))
    {
      byte[] data = File.ReadAllBytes(path);
      tex = new Texture2D(2, 2);
      tex.LoadImage(data);
    }
    else
      Debug.LogWarning((object) $"ModUtil: Texture file '{path}' not found");
    return tex;
  }

  public enum BuildingOrdering
  {
    Before,
    After,
  }
}
