// Decompiled with JetBrains decompiler
// Type: MotdDataFetchRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class MotdDataFetchRequest : IDisposable
{
  private MotdData data;
  private bool isComplete;
  private Action<MotdData> onCompleteFn;

  public void Dispose() => this.onCompleteFn = (Action<MotdData>) null;

  public void Fetch(string url)
  {
    MotdDataFetchRequest.FetchWebMotdJson(url, (Action<MotdData>) (webMotd =>
    {
      this.data = webMotd;
      if (webMotd == null)
      {
        Debug.LogWarning((object) "MOTD Error: failed to get web motd json");
        CompleteWith((MotdData) null);
      }
      else
        MotdDataFetchRequest.FetchWebMotdImagesFor(webMotd, (Action<bool>) (isOk =>
        {
          foreach (MotdData_Box motdDataBox in webMotd.boxesLive)
          {
            if (motdDataBox.resolvedImage.IsNullOrDestroyed())
              isOk = false;
          }
          if (!isOk)
          {
            Debug.LogWarning((object) "MOTD Error: couldn't fetch all web motd images");
            CompleteWith((MotdData) null);
          }
          else
          {
            MotdDataFetchRequest.WriteCachedMotdImages(webMotd);
            CompleteWith(webMotd);
          }
        }));
    }));

    void CompleteWith(MotdData data)
    {
      if (this.isComplete)
        return;
      this.isComplete = true;
      this.data = data;
      if (this.onCompleteFn == null)
        return;
      this.onCompleteFn(data);
    }
  }

  public void OnComplete(Action<MotdData> callbackFn)
  {
    if (this.isComplete)
      callbackFn(this.data);
    else
      this.onCompleteFn += callbackFn;
  }

  public static void FetchWebMotdJson(string url, Action<MotdData> onCompleteFn)
  {
    UnityWebRequest webRequest = UnityWebRequest.Get(url);
    webRequest.timeout = 3;
    webRequest.SetRequestHeader("Content-Type", "application/json");
    webRequest.SendWebRequest().completed += (Action<AsyncOperation>) (operation =>
    {
      if (string.IsNullOrEmpty(webRequest.error))
      {
        onCompleteFn(MotdData.Parse(webRequest.downloadHandler.text));
      }
      else
      {
        Debug.LogWarning((object) ("MOTD Error: failed to fetch web motd. " + webRequest.error));
        onCompleteFn((MotdData) null);
      }
      webRequest.Dispose();
    });
  }

  public static void FetchWebMotdImagesFor(MotdData motdData, Action<bool> onCompleteFn)
  {
    foreach (MotdData_Box motdDataBox in motdData.boxesLive)
    {
      if (motdDataBox.image == null)
      {
        onCompleteFn(false);
        return;
      }
    }
    int imagesToFetchCount = motdData.boxesLive.Count;
    if (imagesToFetchCount == 0)
    {
      onCompleteFn(false);
    }
    else
    {
      int imagesValidCount = 0;
      foreach (MotdData_Box motdDataBox in motdData.boxesLive)
      {
        MotdData_Box box = motdDataBox;
        MotdDataFetchRequest.FetchWebMotdImage(box.image, (Action<Texture2D, bool>) ((resolvedImage, isFromDisk) =>
        {
          --imagesToFetchCount;
          box.resolvedImage = resolvedImage;
          box.resolvedImageIsFromDisk = isFromDisk;
          if ((UnityEngine.Object) box.resolvedImage != (UnityEngine.Object) null)
            ++imagesValidCount;
          if (imagesToFetchCount != 0)
            return;
          onCompleteFn(imagesValidCount == motdData.boxesLive.Count);
        }));
      }
    }
  }

  public static void FetchWebMotdImage(string url, Action<Texture2D, bool> onCompleteFn)
  {
    Texture2D texture2D = MotdDataFetchRequest.ReadCachedMotdImage(url);
    if ((UnityEngine.Object) texture2D != (UnityEngine.Object) null)
    {
      onCompleteFn(texture2D, true);
    }
    else
    {
      UnityWebRequest webRequest = UnityWebRequest.Get(url);
      webRequest.timeout = 3;
      webRequest.SendWebRequest().completed += (Action<AsyncOperation>) (operation =>
      {
        if (string.IsNullOrEmpty(webRequest.error))
        {
          onCompleteFn(MotdDataFetchRequest.ParseImage(webRequest.downloadHandler.data), false);
        }
        else
        {
          Debug.LogWarning((object) $"MOTD Error: failed to fetch web image at {url}. {webRequest.error}");
          onCompleteFn((Texture2D) null, false);
        }
        webRequest.Dispose();
      });
    }
  }

  public static string GetCachePath() => System.IO.Path.Combine(Util.CacheFolder(), "motd");

  public static string GetCachedFilePath(string filePath)
  {
    return System.IO.Path.Combine(Util.CacheFolder(), "motd", System.IO.Path.GetFileName(filePath));
  }

  public static void WriteCachedMotdImages(MotdData data)
  {
    if (data == null)
      return;
    try
    {
      if (!Directory.Exists(MotdDataFetchRequest.GetCachePath()))
        Directory.CreateDirectory(MotdDataFetchRequest.GetCachePath());
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"MOTD Error: Failed to create image cache directory --- {ex}");
    }
    try
    {
      if (Directory.Exists(MotdDataFetchRequest.GetCachePath()))
      {
        foreach (MotdData_Box motdDataBox in data.boxesLive)
        {
          if (motdDataBox.image != null && (UnityEngine.Object) motdDataBox.resolvedImage != (UnityEngine.Object) null && !motdDataBox.resolvedImageIsFromDisk)
            File.WriteAllBytes(MotdDataFetchRequest.GetCachedFilePath(motdDataBox.image), motdDataBox.resolvedImage.EncodeToPNG());
        }
      }
      else
        Debug.LogWarning((object) "MOTD Error: Failed to write cached motd images, couldn't find a valid cache directory");
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"MOTD Error: Failed to write cached motd images --- {ex}");
    }
    try
    {
      if (Directory.Exists(MotdDataFetchRequest.GetCachePath()))
      {
        List<string> stringList = new List<string>(16 /*0x10*/);
        foreach (MotdData_Box motdDataBox in data.boxesLive)
        {
          if (motdDataBox.image != null)
            stringList.Add(MotdDataFetchRequest.GetCachedFilePath(motdDataBox.image));
        }
        foreach (string file in Directory.GetFiles(MotdDataFetchRequest.GetCachePath()))
        {
          if (!stringList.Contains(MotdDataFetchRequest.GetCachedFilePath(file)))
            File.Delete(file);
        }
      }
      else
        Debug.LogWarning((object) "MOTD Error: Failed to clean cached motd images, couldn't find a valid cache directory");
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"MOTD Error: Failed to clean cached motd images --- {ex}");
    }
  }

  public static Texture2D ReadCachedMotdImage(string url)
  {
    string fileName = System.IO.Path.GetFileName(url);
    string cachedFilePath = MotdDataFetchRequest.GetCachedFilePath(fileName);
    if (!File.Exists(cachedFilePath))
      return (Texture2D) null;
    try
    {
      return MotdDataFetchRequest.ParseImage(File.ReadAllBytes(cachedFilePath));
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"MOTD Error: Can't load cached motd image \"{fileName}\" --- {ex}");
      return (Texture2D) null;
    }
  }

  public static string GetLocaleCode()
  {
    Localization.Locale locale = Localization.GetLocale();
    if (locale != null)
    {
      switch (locale.Lang)
      {
        case Localization.Language.Chinese:
        case Localization.Language.Korean:
        case Localization.Language.Russian:
          return locale.Code;
      }
    }
    return (string) null;
  }

  public static Texture2D ParseImage(byte[] buffer)
  {
    if (IsPng(buffer) || IsJpg(buffer))
    {
      Texture2D tex = new Texture2D(0, 0);
      tex.LoadImage(buffer);
      return tex;
    }
    if (IsKleiTex(buffer))
    {
      Debug.LogWarning((object) "MOTD Error: Couldn't load image - KTEX isn't supported yet.");
      return (Texture2D) null;
    }
    Debug.LogWarning((object) "MOTD Error: Couldn't load image - Unsupported image file format.");
    return (Texture2D) null;

    static bool IsPng(byte[] buffer)
    {
      return buffer[0] == (byte) 137 && buffer[1] == (byte) 80 /*0x50*/ && buffer[2] == (byte) 78 && buffer[3] == (byte) 71 && buffer[4] == (byte) 13 && buffer[5] == (byte) 10 && buffer[6] == (byte) 26 && buffer[7] == (byte) 10;
    }

    static bool IsJpg(byte[] buffer)
    {
      return buffer[0] == byte.MaxValue && buffer[1] == (byte) 216 && buffer[6] == (byte) 74 && buffer[7] == (byte) 70 && buffer[8] == (byte) 73 && buffer[9] == (byte) 70;
    }

    static bool IsKleiTex(byte[] buffer)
    {
      return buffer[0] == (byte) 75 && buffer[1] == (byte) 84 && buffer[2] == (byte) 69 && buffer[3] == (byte) 88;
    }
  }

  public static void GetUrlParams(out string platformCode, out string languageCode)
  {
    platformCode = "default";
    Localization.Language? nullable = Localization.GetLocale() == null ? new Localization.Language?() : new Localization.Language?(Localization.GetLocale().Lang);
    if (nullable.HasValue && nullable.GetValueOrDefault() == Localization.Language.Chinese)
      languageCode = "schinese";
    else
      languageCode = "en";
  }

  public static string BuildUrl()
  {
    string platformCode;
    string languageCode;
    MotdDataFetchRequest.GetUrlParams(out platformCode, out languageCode);
    return $"https://motd.klei.com/motd.json/?game=oni&platform={platformCode}&lang={languageCode}";
  }
}
