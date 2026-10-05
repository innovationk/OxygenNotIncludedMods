// Decompiled with JetBrains decompiler
// Type: PasteBaseTemplateScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using ProcGen;
using STRINGS;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class PasteBaseTemplateScreen : KScreen
{
  public static PasteBaseTemplateScreen Instance;
  public GameObject button_list_container;
  public GameObject prefab_paste_button;
  public GameObject prefab_directory_button;
  public KButton button_directory_up;
  public LocText directory_path_text;
  private List<GameObject> m_template_buttons = new List<GameObject>();
  private static readonly string NO_DIRECTORY = "NONE";
  private string m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    PasteBaseTemplateScreen.Instance = this;
    TemplateCache.Init();
    this.button_directory_up.onClick += new System.Action(this.UpDirectory);
    this.ConsumeMouseScroll = true;
    this.RefreshStampButtons();
  }

  protected override void OnForcedCleanUp()
  {
    PasteBaseTemplateScreen.Instance = (PasteBaseTemplateScreen) null;
    base.OnForcedCleanUp();
  }

  [ContextMenu("Refresh")]
  public void RefreshStampButtons()
  {
    this.directory_path_text.text = this.m_CurrentDirectory;
    this.button_directory_up.isInteractable = this.m_CurrentDirectory != PasteBaseTemplateScreen.NO_DIRECTORY;
    foreach (UnityEngine.Object templateButton in this.m_template_buttons)
      UnityEngine.Object.Destroy(templateButton);
    this.m_template_buttons.Clear();
    if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
    {
      this.directory_path_text.text = "";
      foreach (string str in DlcManager.RELEASED_VERSIONS)
      {
        string dlcId = str;
        if (Game.IsDlcActiveForCurrentSave(dlcId))
        {
          GameObject gameObject = Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.UpdateDirectory(SettingsCache.GetScope(dlcId)));
          gameObject.GetComponentInChildren<LocText>().text = dlcId == "" ? UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.BASE_GAME_FOLDER_NAME.text : SettingsCache.GetScope(dlcId);
          this.m_template_buttons.Add(gameObject);
        }
      }
    }
    else
    {
      string path = TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory);
      if (Directory.Exists(path))
      {
        foreach (string directory in Directory.GetDirectories(path))
        {
          string directory_name = System.IO.Path.GetFileNameWithoutExtension(directory);
          GameObject gameObject = Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.UpdateDirectory(directory_name));
          gameObject.GetComponentInChildren<LocText>().text = directory_name;
          this.m_template_buttons.Add(gameObject);
        }
      }
      ListPool<FileHandle, PasteBaseTemplateScreen>.PooledList result = ListPool<FileHandle, PasteBaseTemplateScreen>.Allocate();
      FileSystem.GetFiles(TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory), "*.yaml", (ICollection<FileHandle>) result);
      foreach (FileHandle fileHandle in (List<FileHandle>) result)
      {
        string file_path_no_extension = System.IO.Path.GetFileNameWithoutExtension(fileHandle.full_path);
        GameObject gameObject = Util.KInstantiateUI(this.prefab_paste_button, this.button_list_container, true);
        gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OnClickPasteButton(file_path_no_extension));
        gameObject.GetComponentInChildren<LocText>().text = file_path_no_extension;
        this.m_template_buttons.Add(gameObject);
      }
    }
  }

  private void UpdateDirectory(string relativePath)
  {
    if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
      this.m_CurrentDirectory = "";
    this.m_CurrentDirectory = FileSystem.CombineAndNormalize(this.m_CurrentDirectory, relativePath);
    this.RefreshStampButtons();
  }

  private void UpDirectory()
  {
    int length = this.m_CurrentDirectory.LastIndexOf("/");
    if (length > 0)
    {
      this.m_CurrentDirectory = this.m_CurrentDirectory.Substring(0, length);
    }
    else
    {
      string dlcId;
      string path;
      SettingsCache.GetDlcIdAndPath(this.m_CurrentDirectory, out dlcId, out path);
      this.m_CurrentDirectory = !path.IsNullOrWhiteSpace() ? SettingsCache.GetScope(dlcId) : PasteBaseTemplateScreen.NO_DIRECTORY;
    }
    this.RefreshStampButtons();
  }

  private void OnClickPasteButton(string template_name)
  {
    if (template_name == null)
      return;
    string templatePath = FileSystem.CombineAndNormalize(this.m_CurrentDirectory, template_name);
    DebugTool.Instance.DeactivateTool();
    DebugBaseTemplateButton.Instance.ClearSelection();
    DebugBaseTemplateButton.Instance.nameField.text = templatePath;
    TemplateContainer template = TemplateCache.GetTemplate(templatePath);
    StampTool.Instance.Activate(template, true);
  }
}
