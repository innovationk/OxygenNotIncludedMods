// Decompiled with JetBrains decompiler
// Type: Def
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class Def : ScriptableObject
{
  public string PrefabID;
  public Tag Tag;
  private static Dictionary<Tuple<KAnimFile, string, bool>, Sprite> knownUISprites = new Dictionary<Tuple<KAnimFile, string, bool>, Sprite>();
  public const string DEFAULT_SPRITE = "unknown";

  public virtual void InitDef() => this.Tag = TagManager.Create(this.PrefabID);

  public virtual string Name => (string) null;

  public static Tuple<Sprite, Color> GetUISprite(object item, string animName = "ui", bool centered = false)
  {
    switch (item)
    {
      case Substance _:
        return Def.GetUISprite((object) ElementLoader.FindElementByHash((item as Substance).elementID), animName, centered);
      case Element _:
        if ((item as Element).IsSolid)
          return new Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim((item as Element).substance.anim, animName, centered), Color.white);
        if ((item as Element).IsLiquid)
          return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "element_liquid"), (Color) (item as Element).substance.uiColour);
        return (item as Element).IsGas ? new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "element_gas"), (Color) (item as Element).substance.uiColour) : new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "unknown_far"), Color.black);
      case AsteroidGridEntity _:
        return new Tuple<Sprite, Color>(((ClusterGridEntity) item).GetUISprite(), Color.white);
      case GameObject _:
        GameObject go = item as GameObject;
        if (ElementLoader.GetElement(go.PrefabID()) != null)
          return Def.GetUISprite((object) ElementLoader.GetElement(go.PrefabID()), animName, centered);
        KPrefabID component1 = go.GetComponent<KPrefabID>();
        CreatureBrain component2 = go.GetComponent<CreatureBrain>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          animName = component2.symbolPrefix + "ui";
        SpaceArtifact component3 = go.GetComponent<SpaceArtifact>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          animName = component3.GetUIAnim();
        MultiMinionDiningTable.Seat component4 = go.GetComponent<MultiMinionDiningTable.Seat>();
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
          go = component4.DiningTable.gameObject;
        if (component1.HasTag(GameTags.Egg))
        {
          IncubationMonitor.Def def = go.GetDef<IncubationMonitor.Def>();
          if (def != null)
          {
            GameObject prefab = Assets.GetPrefab(def.spawnedCreature);
            if ((bool) (UnityEngine.Object) prefab)
            {
              CreatureBrain component5 = prefab.GetComponent<CreatureBrain>();
              if ((bool) (UnityEngine.Object) component5 && !string.IsNullOrEmpty(component5.symbolPrefix))
                animName = component5.symbolPrefix + animName;
            }
          }
        }
        if (component1.HasTag(GameTags.BionicUpgrade))
          animName = BionicUpgradeComponentConfig.UpgradesData[component1.PrefabID()].uiAnimName;
        KBatchedAnimController component6 = go.GetComponent<KBatchedAnimController>();
        if ((bool) (UnityEngine.Object) component6)
        {
          Sprite fromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component6.AnimFiles[0], animName, centered);
          return new Tuple<Sprite, Color>(fromMultiObjectAnim, (UnityEngine.Object) fromMultiObjectAnim != (UnityEngine.Object) null ? Color.white : Color.clear);
        }
        if ((UnityEngine.Object) go.GetComponent<Building>() != (UnityEngine.Object) null)
        {
          Sprite uiSprite = go.GetComponent<Building>().Def.GetUISprite(animName, centered);
          return new Tuple<Sprite, Color>(uiSprite, (UnityEngine.Object) uiSprite != (UnityEngine.Object) null ? Color.white : Color.clear);
        }
        Debug.LogWarningFormat("Can't get sprite for type {0} (no KBatchedAnimController)", (object) item.ToString());
        return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "unknown"), Color.grey);
      case string _:
        if (Db.Get().Amounts.Exists(item as string))
          return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) Db.Get().Amounts.Get(item as string).uiSprite), Color.white);
        return Db.Get().Attributes.Exists(item as string) ? new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) Db.Get().Attributes.Get(item as string).uiSprite), Color.white) : Def.GetUISprite((object) (item as string).ToTag(), animName, centered);
      case Tag tag3:
        if (ElementLoader.GetElement(tag3) != null)
          return Def.GetUISprite((object) ElementLoader.GetElement((Tag) item), animName, centered);
        if ((UnityEngine.Object) Assets.GetPrefab((Tag) item) != (UnityEngine.Object) null)
          return Def.GetUISprite((object) Assets.GetPrefab((Tag) item), animName, centered);
        Tag tag1 = (Tag) item;
        if ((UnityEngine.Object) Assets.GetSprite((HashedString) tag1.Name) != (UnityEngine.Object) null)
        {
          tag1 = (Tag) item;
          return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) tag1.Name), Color.white);
        }
        foreach (Tag tag2 in GameTags.Creatures.Species.AllSpecies_REFLECTION())
        {
          if (tag2 == (Tag) item)
          {
            foreach (CreatureBrain asListOfComponent in Assets.GetPrefabsWithComponentAsListOfComponents<CreatureBrain>())
            {
              if (asListOfComponent.species == (Tag) item && asListOfComponent.HasTag(GameTags.OriginalCreature))
                return Def.GetUISprite((object) asListOfComponent.gameObject);
            }
          }
        }
        break;
    }
    return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "unknown"), Color.grey);
  }

  public static Tuple<Sprite, Color> GetUISprite(Tag prefabID, string facadeID)
  {
    return (UnityEngine.Object) Assets.GetPrefab(prefabID).GetComponent<Equippable>() != (UnityEngine.Object) null && !facadeID.IsNullOrWhiteSpace() ? Db.GetEquippableFacades().Get(facadeID).GetUISprite() : Def.GetUISprite((object) prefabID);
  }

  public static Sprite GetFacadeUISprite(string facadeID)
  {
    return Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim((HashedString) Db.GetBuildingFacades().Get(facadeID).AnimFile));
  }

  public static Sprite GetUISpriteFromMultiObjectAnim(
    KAnimFile animFile,
    string animName = "ui",
    bool centered = false,
    string symbolName = "")
  {
    Tuple<KAnimFile, string, bool> key = new Tuple<KAnimFile, string, bool>(animFile, animName, centered);
    if (Def.knownUISprites.ContainsKey(key))
      return Def.knownUISprites[key];
    if ((UnityEngine.Object) animFile == (UnityEngine.Object) null)
    {
      DebugUtil.LogWarningArgs((object) animName, (object) "missing Anim File");
      return Assets.GetSprite((HashedString) "unknown");
    }
    Sprite spriteFromKanimFile = Def.GetSpriteFromKAnimFile(animFile, (KAnimFileData) null, (KAnim.Build) null, (KBatchGroupData) null, animName, centered, symbolName);
    if ((UnityEngine.Object) spriteFromKanimFile == (UnityEngine.Object) null)
      return Assets.GetSprite((HashedString) "unknown");
    spriteFromKanimFile.name = $"{spriteFromKanimFile.texture.name}:{animName}:{centered}";
    Def.knownUISprites[key] = spriteFromKanimFile;
    return spriteFromKanimFile;
  }

  public static Sprite GetSpriteFromKAnimFile(
    KAnimFile animFile,
    KAnimFileData kafd,
    KAnim.Build build,
    KBatchGroupData batchGroupData,
    string animName = "ui",
    bool centered = false,
    string symbolName = "")
  {
    kafd = kafd == null ? animFile.GetData() : kafd;
    if (kafd == null)
    {
      DebugUtil.LogWarningArgs((object) animName, (object) "KAnimFileData is null");
      return (Sprite) null;
    }
    build = build == null ? kafd.build : build;
    if (build == null)
      return (Sprite) null;
    if (string.IsNullOrEmpty(symbolName))
      symbolName = animName;
    KAnimHashedString symbol_name = new KAnimHashedString(symbolName);
    KAnim.Build.Symbol symbol = build.GetSymbol(symbol_name);
    if (symbol == null)
    {
      DebugUtil.LogWarningArgs((object) animFile.name, (object) animName, (object) "placeSymbol [", (object) symbolName, (object) "] is missing");
      return (Sprite) null;
    }
    int frame = 0;
    KAnim.Build.SymbolFrameInstance symbolFrameInstance = batchGroupData == null ? symbol.GetFrame(frame) : symbol.GetFrame(frame, batchGroupData);
    Texture2D texture = batchGroupData == null ? build.GetTexture(0) : build.GetTexture(0, batchGroupData);
    Debug.Assert((UnityEngine.Object) texture != (UnityEngine.Object) null, (object) ("Invalid texture on " + animFile.name));
    float x1 = symbolFrameInstance.uvMin.x;
    float x2 = symbolFrameInstance.uvMax.x;
    float y1 = symbolFrameInstance.uvMax.y;
    float y2 = symbolFrameInstance.uvMin.y;
    int num1 = (int) ((double) texture.width * (double) Mathf.Abs(x2 - x1));
    int num2 = (int) ((double) texture.height * (double) Mathf.Abs(y2 - y1));
    float num3 = Mathf.Abs(symbolFrameInstance.bboxMax.x - symbolFrameInstance.bboxMin.x);
    UnityEngine.Rect rect = new UnityEngine.Rect();
    rect.width = (float) num1;
    rect.height = (float) num2;
    rect.x = (float) (int) ((double) texture.width * (double) x1);
    rect.y = (float) (int) ((double) texture.height * (double) y1);
    float pixelsPerUnit = 100f;
    if (num1 != 0)
      pixelsPerUnit = (float) (100.0 / ((double) num3 / (double) num1));
    Sprite spriteFromKanimFile = Sprite.Create(texture, rect, centered ? new Vector2(0.5f, 0.5f) : Vector2.zero, pixelsPerUnit, 0U, SpriteMeshType.FullRect);
    spriteFromKanimFile.name = $"{texture.name}:{animName}:{centered}";
    return spriteFromKanimFile;
  }

  public static KAnimFile GetAnimFileFromPrefabWithTag(
    GameObject prefab,
    string desiredAnimName,
    out string animName)
  {
    animName = desiredAnimName;
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return (KAnimFile) null;
    CreatureBrain component1 = prefab.GetComponent<CreatureBrain>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      animName = component1.symbolPrefix + animName;
    SpaceArtifact component2 = prefab.GetComponent<SpaceArtifact>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      animName = component2.GetUIAnim();
    if (prefab.HasTag(GameTags.Egg))
    {
      IncubationMonitor.Def def = prefab.GetDef<IncubationMonitor.Def>();
      if (def != null)
      {
        GameObject prefab1 = Assets.GetPrefab(def.spawnedCreature);
        if ((bool) (UnityEngine.Object) prefab1)
        {
          CreatureBrain component3 = prefab1.GetComponent<CreatureBrain>();
          if ((bool) (UnityEngine.Object) component3 && !string.IsNullOrEmpty(component3.symbolPrefix))
            animName = component3.symbolPrefix + animName;
        }
      }
    }
    return prefab.GetComponent<KBatchedAnimController>().AnimFiles[0];
  }

  public static KAnimFile GetAnimFileFromPrefabWithTag(
    Tag prefabID,
    string desiredAnimName,
    out string animName)
  {
    return Def.GetAnimFileFromPrefabWithTag(Assets.GetPrefab(prefabID), desiredAnimName, out animName);
  }
}
