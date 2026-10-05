// Decompiled with JetBrains decompiler
// Type: Timelapser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Timelapser")]
public class Timelapser : KMonoBehaviour
{
  private bool screenshotActive;
  private bool screenshotPending;
  private bool previewScreenshot;
  private string previewSaveGamePath = "";
  private bool screenshotToday;
  private List<int> worldsToScreenshot = new List<int>();
  private HashedString activeOverlay;
  private Camera freezeCamera;
  private RenderTexture bufferRenderTexture;
  private Vector3 camPosition;
  private float camSize;
  private bool debugScreenShot;
  private Vector2Int previewScreenshotResolution = new Vector2Int(Grid.WidthInCells * 2, Grid.HeightInCells * 2);
  private const int DEFAULT_SCREENSHOT_INTERVAL = 10;
  private int[] timelapseScreenshotCycles = new int[100]
  {
    1,
    2,
    3,
    4,
    5,
    6,
    7,
    8,
    9,
    10,
    11,
    12,
    13,
    14,
    15,
    16 /*0x10*/,
    17,
    18,
    19,
    20,
    21,
    22,
    23,
    24,
    25,
    26,
    27,
    28,
    29,
    30,
    31 /*0x1F*/,
    32 /*0x20*/,
    33,
    34,
    35,
    36,
    37,
    38,
    39,
    40,
    41,
    42,
    43,
    44,
    45,
    46,
    47,
    48 /*0x30*/,
    49,
    50,
    55,
    60,
    65,
    70,
    75,
    80 /*0x50*/,
    85,
    90,
    95,
    100,
    110,
    120,
    130,
    140,
    150,
    160 /*0xA0*/,
    170,
    180,
    190,
    200,
    210,
    220,
    230,
    240 /*0xF0*/,
    250,
    260,
    270,
    280,
    290,
    200,
    310,
    320,
    330,
    340,
    350,
    360,
    370,
    380,
    390,
    400,
    410,
    420,
    430,
    440,
    450,
    460,
    470,
    480,
    490,
    500
  };

  public bool CapturingTimelapseScreenshot => this.screenshotActive;

  public Texture2D freezeTexture { get; private set; }

  protected override void OnPrefabInit()
  {
    this.RefreshRenderTextureSize();
    Game.Instance.Subscribe(75424175, new Action<object>(this.RefreshRenderTextureSize));
    this.freezeCamera = CameraController.Instance.timelapseFreezeCamera;
    if ((double) this.CycleTimeToScreenshot() > 0.0)
      this.OnNewDay();
    GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
    this.OnResize();
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.StartCoroutine(this.Render());
  }

  private void OnResize()
  {
    if ((UnityEngine.Object) this.freezeTexture != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.freezeTexture);
    this.freezeTexture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight, TextureFormat.ARGB32, false);
  }

  private void RefreshRenderTextureSize(object data = null)
  {
    if (this.previewScreenshot)
    {
      if ((UnityEngine.Object) this.bufferRenderTexture != (UnityEngine.Object) null)
        this.bufferRenderTexture.DestroyRenderTexture();
      this.bufferRenderTexture = new RenderTexture(this.previewScreenshotResolution.x, this.previewScreenshotResolution.y, 32 /*0x20*/, RenderTextureFormat.ARGB32);
      this.bufferRenderTexture.name = "Timelapser.PreviewScreenshot";
    }
    else
    {
      if (!this.timelapseUserEnabled)
        return;
      if ((UnityEngine.Object) this.bufferRenderTexture != (UnityEngine.Object) null)
        this.bufferRenderTexture.DestroyRenderTexture();
      this.bufferRenderTexture = new RenderTexture(SaveGame.Instance.TimelapseResolution.x, SaveGame.Instance.TimelapseResolution.y, 32 /*0x20*/, RenderTextureFormat.ARGB32);
      this.bufferRenderTexture.name = "Timelapser.Timelapse";
    }
  }

  private bool timelapseUserEnabled => SaveGame.Instance.TimelapseResolution.x > 0;

  private void OnNewDay(object data = null)
  {
    if (this.worldsToScreenshot.Count == 0)
      DebugUtil.LogArgs((object) "Timelapse.OnNewDay but worldsToScreenshot is not empty");
    int cycle = GameClock.Instance.GetCycle();
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (worldContainer.IsDiscovered && !worldContainer.IsModuleInterior)
      {
        if ((double) worldContainer.DiscoveryTimestamp + (double) cycle > (double) this.timelapseScreenshotCycles[this.timelapseScreenshotCycles.Length - 1])
        {
          if ((double) worldContainer.DiscoveryTimestamp + (double) (cycle % 10) == 0.0)
          {
            this.screenshotToday = true;
            this.worldsToScreenshot.Add(worldContainer.id);
          }
        }
        else
        {
          for (int index = 0; index < this.timelapseScreenshotCycles.Length; ++index)
          {
            if ((int) worldContainer.DiscoveryTimestamp + cycle == this.timelapseScreenshotCycles[index])
            {
              this.screenshotToday = true;
              this.worldsToScreenshot.Add(worldContainer.id);
            }
          }
        }
      }
    }
  }

  private void Update()
  {
    if (this.screenshotToday)
    {
      if ((double) this.CycleTimeToScreenshot() > 0.0 && GameClock.Instance.GetCycle() != 0)
        return;
      if (!this.timelapseUserEnabled)
      {
        this.screenshotToday = false;
        this.worldsToScreenshot.Clear();
      }
      else
      {
        if (PlayerController.Instance.CanDrag())
          return;
        CameraController.Instance.ForcePanningState(false);
        this.screenshotToday = false;
        this.SaveScreenshot();
      }
    }
    else
      this.screenshotToday = !this.screenshotPending && this.worldsToScreenshot.Count > 0;
  }

  private float CycleTimeToScreenshot()
  {
    return (float) (300.0 - (double) GameClock.Instance.GetTime() % 600.0);
  }

  private IEnumerator Render()
  {
    while (true)
    {
      do
      {
        yield return (object) SequenceUtil.WaitForEndOfFrame;
      }
      while (!this.screenshotPending);
      int world_id = this.previewScreenshot ? ClusterManager.Instance.GetStartWorld().id : this.worldsToScreenshot[0];
      if (!this.freezeCamera.enabled)
      {
        this.freezeTexture.ReadPixels(new UnityEngine.Rect(0.0f, 0.0f, (float) Camera.main.pixelWidth, (float) Camera.main.pixelHeight), 0, 0);
        this.freezeTexture.Apply();
        this.freezeCamera.gameObject.GetComponent<FillRenderTargetEffect>().SetFillTexture((Texture) this.freezeTexture);
        this.freezeCamera.enabled = true;
        this.screenshotActive = true;
        this.RefreshRenderTextureSize();
        DebugHandler.SetTimelapseMode(true, world_id);
        this.SetPostionAndOrtho(world_id);
        this.activeOverlay = OverlayScreen.Instance.mode;
        OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
      }
      else
      {
        this.RenderAndPrint(world_id);
        if (!this.previewScreenshot)
          this.worldsToScreenshot.Remove(world_id);
        this.freezeCamera.enabled = false;
        DebugHandler.SetTimelapseMode(false);
        this.screenshotPending = false;
        this.previewScreenshot = false;
        this.debugScreenShot = false;
        this.previewSaveGamePath = "";
        OverlayScreen.Instance.ToggleOverlay(this.activeOverlay, false);
        this.screenshotActive = false;
      }
    }
  }

  public void InitialScreenshot()
  {
    this.worldsToScreenshot.Add(ClusterManager.Instance.GetStartWorld().id);
    this.SaveScreenshot();
  }

  private void SaveScreenshot() => this.screenshotPending = true;

  public void SaveColonyPreview(string saveFileName)
  {
    this.previewSaveGamePath = saveFileName;
    this.previewScreenshot = true;
    this.SaveScreenshot();
  }

  private void SetPostionAndOrtho(int world_id)
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(world_id);
    if ((UnityEngine.Object) world == (UnityEngine.Object) null)
      return;
    float num1 = 0.0f;
    this.camSize = CameraController.Instance.overlayCamera.orthographicSize;
    this.camPosition = CameraController.Instance.transform.position;
    if (world.IsStartWorld)
    {
      GameObject telepad = GameUtil.GetTelepad(world_id);
      if ((UnityEngine.Object) telepad == (UnityEngine.Object) null)
        return;
      Vector3 position1 = telepad.transform.GetPosition();
      foreach (KMonoBehaviour kmonoBehaviour in Components.BuildingCompletes.Items)
      {
        Vector3 position2 = kmonoBehaviour.transform.GetPosition();
        float num2 = (float) this.bufferRenderTexture.width / (float) this.bufferRenderTexture.height;
        Vector3 vector3 = position1 - position2;
        num1 = Mathf.Max(num1, vector3.x / num2, vector3.y);
      }
      CameraController.Instance.OrthographicSize = Mathf.Max(num1 + 10f, 18f);
      CameraController.Instance.SetPosition(new Vector3(telepad.transform.position.x, telepad.transform.position.y, CameraController.Instance.transform.position.z));
    }
    else
    {
      CameraController.Instance.OrthographicSize = (float) (world.WorldSize.y / 2);
      CameraController.Instance.SetPosition(new Vector3((float) (world.WorldOffset.x + world.WorldSize.x / 2), (float) (world.WorldOffset.y + world.WorldSize.y / 2), CameraController.Instance.transform.position.z));
    }
  }

  private void RenderAndPrint(int world_id)
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(world_id);
    if ((UnityEngine.Object) world == (UnityEngine.Object) null)
      return;
    if (world.IsStartWorld)
    {
      GameObject telepad = GameUtil.GetTelepad(world.id);
      if ((UnityEngine.Object) telepad == (UnityEngine.Object) null)
      {
        Debug.Log((object) "No telepad present, aborting screenshot.");
        return;
      }
      CameraController.Instance.SetPosition(telepad.transform.position with
      {
        z = CameraController.Instance.transform.position.z
      });
    }
    else
      CameraController.Instance.SetPosition(new Vector3((float) (world.WorldOffset.x + world.WorldSize.x / 2), (float) (world.WorldOffset.y + world.WorldSize.y / 2), CameraController.Instance.transform.position.z));
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = this.bufferRenderTexture;
    CameraController.Instance.RenderForTimelapser(ref this.bufferRenderTexture);
    this.WriteToPng(this.bufferRenderTexture, world_id);
    CameraController.Instance.OrthographicSize = this.camSize;
    CameraController.Instance.SetPosition(this.camPosition);
    RenderTexture.active = active;
  }

  public void WriteToPng(RenderTexture renderTex, int world_id = -1)
  {
    Texture2D tex = new Texture2D(renderTex.width, renderTex.height, TextureFormat.ARGB32, false);
    tex.ReadPixels(new UnityEngine.Rect(0.0f, 0.0f, (float) renderTex.width, (float) renderTex.height), 0, 0);
    tex.Apply();
    byte[] png = tex.EncodeToPNG();
    UnityEngine.Object.Destroy((UnityEngine.Object) tex);
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    string str1 = System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
    if (!Directory.Exists(str1))
      Directory.CreateDirectory(str1);
    string path2 = RetireColonyUtility.StripInvalidCharacters(SaveGame.Instance.BaseName);
    if (!this.previewScreenshot)
    {
      string path = System.IO.Path.Combine(str1, path2);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string path1 = path;
      string str2;
      if (world_id >= 0)
      {
        string name = ClusterManager.Instance.GetWorld(world_id).GetComponent<ClusterGridEntity>().Name;
        string str3 = System.IO.Path.Combine(path1, world_id.ToString("D5"));
        if (!Directory.Exists(str3))
          Directory.CreateDirectory(str3);
        str2 = System.IO.Path.Combine(str3, name);
      }
      else
        str2 = System.IO.Path.Combine(path1, path2);
      DebugUtil.LogArgs((object) "Saving screenshot to", (object) str2);
      string format = "0000.##";
      string str4 = $"{str2}_cycle_{GameClock.Instance.GetCycle().ToString(format)}";
      if (this.debugScreenShot)
      {
        string[] strArray = new string[11];
        strArray[0] = str4;
        strArray[1] = "_";
        strArray[2] = System.DateTime.Now.Day.ToString();
        strArray[3] = "-";
        System.DateTime now = System.DateTime.Now;
        int num = now.Month;
        strArray[4] = num.ToString();
        strArray[5] = "_";
        now = System.DateTime.Now;
        num = now.Hour;
        strArray[6] = num.ToString();
        strArray[7] = "-";
        now = System.DateTime.Now;
        num = now.Minute;
        strArray[8] = num.ToString();
        strArray[9] = "-";
        now = System.DateTime.Now;
        num = now.Second;
        strArray[10] = num.ToString();
        str4 = string.Concat(strArray);
      }
      File.WriteAllBytes(str4 + ".png", png);
    }
    else
    {
      string path = System.IO.Path.ChangeExtension(this.previewSaveGamePath, ".png");
      DebugUtil.LogArgs((object) "Saving screenshot to", (object) path);
      File.WriteAllBytes(path, png);
    }
  }
}
