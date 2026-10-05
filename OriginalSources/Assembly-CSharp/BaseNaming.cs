// Decompiled with JetBrains decompiler
// Type: BaseNaming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/BaseNaming")]
public class BaseNaming : KMonoBehaviour
{
  [SerializeField]
  private KInputTextField inputField;
  [SerializeField]
  private KButton shuffleBaseNameButton;
  private MinionSelectScreen minionSelectScreen;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GenerateBaseName();
    this.shuffleBaseNameButton.onClick += new System.Action(this.GenerateBaseName);
    this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
    this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnEditing));
    this.minionSelectScreen = this.GetComponent<MinionSelectScreen>();
  }

  private bool CheckBaseName(string newName)
  {
    if (string.IsNullOrEmpty(newName))
      return true;
    string prefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
    string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
    if ((UnityEngine.Object) this.minionSelectScreen != (UnityEngine.Object) null)
    {
      bool flag;
      try
      {
        flag = ((Directory.Exists(System.IO.Path.Combine(prefixAndCreateFolder, newName)) ? 1 : 0) | (cloudSavePrefix == null ? (false ? 1 : 0) : (Directory.Exists(System.IO.Path.Combine(cloudSavePrefix, newName)) ? 1 : 0))) != 0;
      }
      catch (Exception ex)
      {
        flag = true;
        Debug.Log((object) $"Base Naming / Warning / {ex}");
      }
      if (flag)
      {
        this.minionSelectScreen.SetProceedButtonActive(false, string.Format((string) UI.IMMIGRANTSCREEN.DUPLICATE_COLONY_NAME, (object) newName));
        return false;
      }
      this.minionSelectScreen.SetProceedButtonActive(true);
    }
    return true;
  }

  private void OnEditing(string newName)
  {
    Util.ScrubInputField(this.inputField);
    this.CheckBaseName(this.inputField.text);
  }

  private void OnEndEdit(string newName)
  {
    if (Localization.HasDirtyWords(newName))
    {
      this.inputField.text = this.GenerateBaseNameString();
      newName = this.inputField.text;
    }
    if (string.IsNullOrEmpty(newName))
      return;
    if (newName.EndsWith(" "))
      newName = newName.TrimEnd(' ');
    if (!this.CheckBaseName(newName))
      return;
    this.inputField.text = newName;
    SaveGame.Instance.SetBaseName(newName);
    string path3 = System.IO.Path.ChangeExtension(newName, ".sav");
    string prefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
    string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
    string path1 = prefixAndCreateFolder;
    if (SaveLoader.GetCloudSavesAvailable() && Game.Instance.SaveToCloudActive && cloudSavePrefix != null)
      path1 = cloudSavePrefix;
    SaveLoader.SetActiveSaveFilePath(System.IO.Path.Combine(path1, newName, path3));
  }

  private void GenerateBaseName()
  {
    string baseNameString = this.GenerateBaseNameString();
    ((TMP_Text) this.inputField.placeholder).text = baseNameString;
    this.inputField.text = baseNameString;
    this.OnEndEdit(baseNameString);
  }

  private string GenerateBaseNameString()
  {
    string fullString = this.ReplaceStringWithRandom(LocString.GetStrings(typeof (NAMEGEN.COLONY.FORMATS)).GetRandom<string>(), "{noun}", LocString.GetStrings(typeof (NAMEGEN.COLONY.NOUN)));
    string[] strings = LocString.GetStrings(typeof (NAMEGEN.COLONY.ADJECTIVE));
    return this.ReplaceStringWithRandom(this.ReplaceStringWithRandom(this.ReplaceStringWithRandom(this.ReplaceStringWithRandom(fullString, "{adjective}", strings), "{adjective2}", strings), "{adjective3}", strings), "{adjective4}", strings);
  }

  private string ReplaceStringWithRandom(
    string fullString,
    string replacementKey,
    string[] replacementValues)
  {
    return !fullString.Contains(replacementKey) ? fullString : fullString.Replace(replacementKey, replacementValues.GetRandom<string>());
  }
}
