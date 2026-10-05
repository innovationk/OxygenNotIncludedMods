// Decompiled with JetBrains decompiler
// Type: KYaml
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using VYaml.Serialization;

#nullable disable
public static class KYaml
{
  private static readonly YamlSerializerOptions Options = new YamlSerializerOptions()
  {
    Resolver = (IYamlFormatterResolver) CompositeResolver.Create((IEnumerable<IYamlFormatter>) new IYamlFormatter[4]
    {
      (IYamlFormatter) new Vector2fFormatter(),
      (IYamlFormatter) new TagFormatter(),
      (IYamlFormatter) new SimHashesFormatter(),
      (IYamlFormatter) new ElementStateFormatter()
    }, (IEnumerable<IYamlFormatterResolver>) new IYamlFormatterResolver[2]
    {
      (IYamlFormatterResolver) KleiResolver.Instance,
      (IYamlFormatterResolver) StandardResolver.Instance
    })
  };

  public static bool LoadFile<T>(string path, out T result, KYaml.ErrorHandler errorHandler = null)
  {
    try
    {
      FileHandle fileHandle = FileSystem.FindFileHandle(path);
      if (fileHandle.source == null)
        throw new FileNotFoundException("KYaml tried loading a file that doesn't exist: " + path);
      result = YamlSerializer.Deserialize<T>(ReadOnlyMemory<byte>.op_Implicit(fileHandle.source.ReadBytes(fileHandle.full_path)), KYaml.Options);
      return true;
    }
    catch (Exception ex)
    {
      if (errorHandler != null)
        errorHandler(path, ex);
      result = default (T);
      return false;
    }
  }

  public static bool LoadFile<T>(FileHandle file, out T result, KYaml.ErrorHandler errorHandler = null)
  {
    try
    {
      byte[] numArray = file.source != null ? file.source.ReadBytes(file.full_path) : File.ReadAllBytes(file.full_path);
      result = YamlSerializer.Deserialize<T>(ReadOnlyMemory<byte>.op_Implicit(numArray), KYaml.Options);
      return true;
    }
    catch (Exception ex)
    {
      if (errorHandler != null)
        errorHandler(file.full_path, ex);
      result = default (T);
      return false;
    }
  }

  public struct Error
  {
    public FileHandle file;
    public string message;
    public Exception inner_exception;
    public string text;
    public KYaml.Error.Severity severity;

    public enum Severity
    {
      Fatal,
      Recoverable,
    }
  }

  public delegate void ErrorHandler(string path, Exception exception);
}
