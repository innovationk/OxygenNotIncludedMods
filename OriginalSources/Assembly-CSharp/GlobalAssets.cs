// Decompiled with JetBrains decompiler
// Type: GlobalAssets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class GlobalAssets : KMonoBehaviour
{
  private static Dictionary<string, string> SoundTable = new Dictionary<string, string>();
  private static HashSet<string> LowPrioritySounds = new HashSet<string>();
  private static HashSet<string> HighPrioritySounds = new HashSet<string>();
  public ColorSet colorSet;
  public ColorSet[] colorSetOptions;

  public static GlobalAssets Instance { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GlobalAssets.Instance = this;
    if (GlobalAssets.SoundTable.Count == 0)
    {
      Bank[] array1 = (Bank[]) null;
      try
      {
        if (RuntimeManager.StudioSystem.getBankList(out array1) != RESULT.OK)
          array1 = (Bank[]) null;
      }
      catch
      {
        array1 = (Bank[]) null;
      }
      if (array1 != null)
      {
        foreach (Bank bank in array1)
        {
          EventDescription[] array2;
          RESULT eventList = bank.getEventList(out array2);
          string path1;
          if (eventList != RESULT.OK)
          {
            int path2 = (int) bank.getPath(out path1);
            Debug.LogError((object) $"ERROR [{eventList}] loading FMOD events for bank [{path1}]");
          }
          else
          {
            for (int index = 0; index < array2.Length; ++index)
            {
              EventDescription eventDescription = array2[index];
              int path3 = (int) eventDescription.getPath(out path1);
              if (path1 == null)
              {
                int path4 = (int) bank.getPath(out path1);
                GUID id1;
                int id2 = (int) eventDescription.getID(out id1);
                Debug.LogError((object) $"Got a FMOD event with a null path! {eventDescription.ToString()} {id1} in bank {path1}");
              }
              else
              {
                string lowerInvariant = Assets.GetSimpleSoundEventName(path1).ToLowerInvariant();
                if (lowerInvariant.Length > 0 && !GlobalAssets.SoundTable.ContainsKey(lowerInvariant))
                {
                  GlobalAssets.SoundTable[lowerInvariant] = path1;
                  if (path1.ToLower().Contains("lowpriority") || lowerInvariant.Contains("lowpriority"))
                    GlobalAssets.LowPrioritySounds.Add(path1);
                  else if (path1.ToLower().Contains("highpriority") || lowerInvariant.Contains("highpriority"))
                    GlobalAssets.HighPrioritySounds.Add(path1);
                }
              }
            }
          }
        }
      }
    }
    SetDefaults.Initialize();
    GraphicsOptionsScreen.SetColorModeFromPrefs();
    this.AddColorModeStyles();
    LocString.CreateLocStringKeys(typeof (UI));
    LocString.CreateLocStringKeys(typeof (INPUT));
    LocString.CreateLocStringKeys(typeof (GAMEPLAY_EVENTS));
    LocString.CreateLocStringKeys(typeof (ROOMS));
    LocString.CreateLocStringKeys(typeof (BUILDING.STATUSITEMS), "STRINGS.BUILDING.");
    LocString.CreateLocStringKeys(typeof (BUILDING.DETAILS), "STRINGS.BUILDING.");
    LocString.CreateLocStringKeys(typeof (SETITEMS));
    LocString.CreateLocStringKeys(typeof (COLONY_ACHIEVEMENTS));
    LocString.CreateLocStringKeys(typeof (CREATURES));
    LocString.CreateLocStringKeys(typeof (RESEARCH));
    LocString.CreateLocStringKeys(typeof (DUPLICANTS));
    LocString.CreateLocStringKeys(typeof (ITEMS));
    LocString.CreateLocStringKeys(typeof (ROBOTS));
    LocString.CreateLocStringKeys(typeof (ELEMENTS));
    LocString.CreateLocStringKeys(typeof (MISC));
    LocString.CreateLocStringKeys(typeof (VIDEOS));
    LocString.CreateLocStringKeys(typeof (NAMEGEN));
    LocString.CreateLocStringKeys(typeof (WORLDS));
    LocString.CreateLocStringKeys(typeof (CLUSTER_NAMES));
    LocString.CreateLocStringKeys(typeof (SUBWORLDS));
    LocString.CreateLocStringKeys(typeof (WORLD_TRAITS));
    LocString.CreateLocStringKeys(typeof (INPUT_BINDINGS));
    LocString.CreateLocStringKeys(typeof (LORE));
    LocString.CreateLocStringKeys(typeof (CODEX));
    LocString.CreateLocStringKeys(typeof (SUBWORLDS));
    LocString.CreateLocStringKeys(typeof (BLUEPRINTS));
  }

  private void AddColorModeStyles()
  {
    TMP_StyleSheet.instance.AddStyle(new TMP_Style("logic_on", $"<color=#{ColorUtility.ToHtmlStringRGB((Color) this.colorSet.logicOn)}>", "</color>"));
    TMP_StyleSheet.instance.AddStyle(new TMP_Style("logic_off", $"<color=#{ColorUtility.ToHtmlStringRGB((Color) this.colorSet.logicOff)}>", "</color>"));
    TMP_StyleSheet.instance.RefreshStyles();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GlobalAssets.Instance = (GlobalAssets) null;
  }

  public static string GetSound(string name, bool force_no_warning = false)
  {
    if (name == null)
      return (string) null;
    name = name.ToLowerInvariant();
    string sound = (string) null;
    GlobalAssets.SoundTable.TryGetValue(name, out sound);
    return sound;
  }

  public static bool IsLowPriority(string path) => GlobalAssets.LowPrioritySounds.Contains(path);

  public static bool IsHighPriority(string path) => GlobalAssets.HighPrioritySounds.Contains(path);
}
