// Decompiled with JetBrains decompiler
// Type: KMod.Steam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#nullable disable
namespace KMod;

public class Steam : IDistributionPlatform, SteamUGCService.IClient
{
  private Mod MakeMod(SteamUGCService.Mod subscribed)
  {
    if (subscribed == null)
      return (Mod) null;
    if (((int) SteamUGC.GetItemState(subscribed.fileId) & 4) == 0)
      return (Mod) null;
    string steamModID = subscribed.fileId.m_PublishedFileId.ToString();
    Label label = new Label()
    {
      id = steamModID,
      distribution_platform = Label.DistributionPlatform.Steam,
      version = (long) subscribed.lastUpdateTime,
      title = subscribed.title
    };
    string pchFolder;
    if (!SteamUGC.GetItemInstallInfo(subscribed.fileId, out ulong _, out pchFolder, 1024U /*0x0400*/, out uint _))
    {
      Global.Instance.modManager.events.Add(new Event()
      {
        event_type = EventType.InstallInfoInaccessible,
        mod = label
      });
      return (Mod) null;
    }
    if (!File.Exists(pchFolder))
    {
      KCrashReporter.ReportDevNotification("Steam failed to download mod", Environment.StackTrace, $"Skipping installing mod '{subscribed.title}' (https://steamcommunity.com/sharedfiles/filedetails/?id={subscribed.fileId}) '{pchFolder}'", extraCategories: new string[1]
      {
        KCrashReporter.CRASH_CATEGORY.MODSYSTEM
      });
      Global.Instance.modManager.events.Add(new Event()
      {
        event_type = EventType.DownloadFailed,
        mod = label
      });
      return (Mod) null;
    }
    ZipFile file_source = new ZipFile(pchFolder);
    KModHeader header = KModUtil.GetHeader((IFileSource) file_source, label.defaultStaticID, subscribed.title, subscribed.description, false);
    label.title = header.title;
    return new Mod(label, header.staticID, header.description, (IFileSource) file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_STEAM_SUBSCRIPTION, (System.Action) (() => App.OpenWebURL("https://steamcommunity.com/sharedfiles/filedetails/?id=" + steamModID)));
  }

  public void UpdateMods(
    IEnumerable<PublishedFileId_t> added,
    IEnumerable<PublishedFileId_t> updated,
    IEnumerable<PublishedFileId_t> removed,
    IEnumerable<SteamUGCService.Mod> loaded_previews)
  {
    foreach (PublishedFileId_t publishedFileIdT in added)
    {
      SteamUGCService.Mod mod1 = SteamUGCService.Instance.FindMod(publishedFileIdT);
      if (mod1 == null)
      {
        string details = $"Mod Steam PublishedFileId_t {publishedFileIdT}";
        KCrashReporter.ReportDevNotification($"SteamUGCService just told us ADDED id {publishedFileIdT} was valid!", Environment.StackTrace, details);
      }
      else
      {
        Mod mod2 = this.MakeMod(mod1);
        if (mod2 != null)
          Global.Instance.modManager.Subscribe(mod2, (object) this);
      }
    }
    foreach (PublishedFileId_t publishedFileIdT in updated)
    {
      SteamUGCService.Mod mod3 = SteamUGCService.Instance.FindMod(publishedFileIdT);
      if (mod3 == null)
      {
        KCrashReporter.ReportDevNotification("SteamUGCService just told us UPDATED id was valid!", Environment.StackTrace, $"Mod Steam PublishedFileId_t {publishedFileIdT.m_PublishedFileId}");
      }
      else
      {
        Mod mod4 = this.MakeMod(mod3);
        if (mod4 != null)
          Global.Instance.modManager.Update(mod4, (object) this);
      }
    }
    foreach (PublishedFileId_t publishedFileIdT in removed)
      Global.Instance.modManager.Unsubscribe(new Label()
      {
        id = publishedFileIdT.m_PublishedFileId.ToString(),
        distribution_platform = Label.DistributionPlatform.Steam
      }, (object) this);
    if (added.Count<PublishedFileId_t>() != 0)
      Global.Instance.modManager.Sanitize((GameObject) null);
    else
      Global.Instance.modManager.Report((GameObject) null);
  }
}
