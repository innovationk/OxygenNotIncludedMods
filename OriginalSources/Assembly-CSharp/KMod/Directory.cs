// Decompiled with JetBrains decompiler
// Type: KMod.Directory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
namespace KMod;

internal struct Directory(string root) : IFileSource
{
  private AliasDirectory file_system = new AliasDirectory(root, root, Application.streamingAssetsPath, true);
  private string root = root;

  public string GetRoot() => this.root;

  public bool Exists() => System.IO.Directory.Exists(this.GetRoot());

  public bool Exists(string relative_path)
  {
    return this.Exists() && new DirectoryInfo(FileSystem.Normalize(System.IO.Path.Combine(this.root, relative_path))).Exists;
  }

  public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
  {
    relative_root = relative_root ?? "";
    string path = FileSystem.Normalize(System.IO.Path.Combine(this.root, relative_root));
    DirectoryInfo directoryInfo = new DirectoryInfo(path);
    if (!directoryInfo.Exists)
    {
      Debug.LogError((object) $"Cannot iterate over ${path}, this directory does not exist");
    }
    else
    {
      foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
        file_system_items.Add(new FileSystemItem()
        {
          name = fileSystemInfo.Name,
          type = fileSystemInfo is DirectoryInfo ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File
        });
    }
  }

  public IFileDirectory GetFileSystem() => (IFileDirectory) this.file_system;

  public bool TryCopyTo(string path, List<string> extensions = null)
  {
    try
    {
      return Directory.CopyDirectory(this.root, path, extensions).error == Directory.CopyDirectoryResult.Error.None;
    }
    catch (UnauthorizedAccessException ex)
    {
      FileUtil.ErrorDialog(FileUtil.ErrorType.UnauthorizedAccess, path, (string) null, (string) null);
      return false;
    }
    catch (IOException ex)
    {
      FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, path, (string) null, (string) null);
      return false;
    }
    catch
    {
      throw;
    }
  }

  public string Read(string relative_path)
  {
    try
    {
      using (FileStream fileStream = File.OpenRead(System.IO.Path.Combine(this.root, relative_path)))
      {
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, (int) fileStream.Length);
        return Encoding.UTF8.GetString(numArray);
      }
    }
    catch
    {
      return string.Empty;
    }
  }

  private static Directory.CopyDirectoryResult CopyDirectory(
    string sourceDirName,
    string destDirName,
    List<string> extensions)
  {
    DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
    if (!directoryInfo.Exists)
      return new Directory.CopyDirectoryResult()
      {
        error = Directory.CopyDirectoryResult.Error.Read,
        fileCount = 0
      };
    if (!FileUtil.CreateDirectory(destDirName))
      return new Directory.CopyDirectoryResult()
      {
        error = Directory.CopyDirectoryResult.Error.Write,
        fileCount = 0
      };
    FileInfo[] files = directoryInfo.GetFiles();
    Directory.CopyDirectoryResult copyDirectoryResult1 = new Directory.CopyDirectoryResult()
    {
      error = Directory.CopyDirectoryResult.Error.None,
      fileCount = 0
    };
    foreach (FileInfo fileInfo in files)
    {
      bool flag = extensions == null || extensions.Count == 0;
      if (extensions != null)
      {
        foreach (string str in extensions)
        {
          if (str == System.IO.Path.GetExtension(fileInfo.Name).ToLower())
          {
            flag = true;
            break;
          }
        }
      }
      if (flag)
      {
        string destFileName = System.IO.Path.Combine(destDirName, fileInfo.Name);
        if (fileInfo.CopyTo(destFileName, false) == null)
        {
          copyDirectoryResult1.error = Directory.CopyDirectoryResult.Error.Write;
          return copyDirectoryResult1;
        }
        ++copyDirectoryResult1.fileCount;
      }
    }
    foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
    {
      string destDirName1 = System.IO.Path.Combine(destDirName, directory.Name);
      Directory.CopyDirectoryResult copyDirectoryResult2 = Directory.CopyDirectory(directory.FullName, destDirName1, extensions);
      copyDirectoryResult1.fileCount += copyDirectoryResult2.fileCount;
      if (copyDirectoryResult2.error != Directory.CopyDirectoryResult.Error.None)
      {
        copyDirectoryResult1.error = copyDirectoryResult2.error;
        return copyDirectoryResult1;
      }
    }
    if (copyDirectoryResult1.fileCount == 0)
      FileUtil.DeleteDirectory(destDirName);
    return copyDirectoryResult1;
  }

  public void Dispose()
  {
  }

  private struct CopyDirectoryResult
  {
    public Directory.CopyDirectoryResult.Error error;
    public int fileCount;

    public enum Error
    {
      None,
      Read,
      Write,
    }
  }
}
