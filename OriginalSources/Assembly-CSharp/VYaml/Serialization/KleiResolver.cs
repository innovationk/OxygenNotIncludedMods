// Decompiled with JetBrains decompiler
// Type: VYaml.Serialization.KleiResolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable enable
namespace VYaml.Serialization;

public class KleiResolver : IYamlFormatterResolver
{
  public static readonly KleiResolver Instance = new KleiResolver();
  public static readonly Dictionary<System.Type, IYamlFormatter> FormatterMap = new Dictionary<System.Type, IYamlFormatter>()
  {
    {
      typeof (Vector2f),
      (IYamlFormatter) Vector2fFormatter.Instance
    },
    {
      typeof (SimHashes),
      (IYamlFormatter) SimHashesFormatter.Instance
    },
    {
      typeof (Tag),
      (IYamlFormatter) TagFormatter.Instance
    },
    {
      typeof (Element.State),
      (IYamlFormatter) ElementStateFormatter.Instance
    }
  };

  public IYamlFormatter<T>? GetFormatter<T>() => KleiResolver.FormatterCache<T>.Formatter;

  private static class FormatterCache<T>
  {
    public static readonly IYamlFormatter<T>? Formatter;

    static FormatterCache()
    {
      IYamlFormatter yamlFormatter1;
      if (KleiResolver.FormatterMap.TryGetValue(typeof (T), out yamlFormatter1) && yamlFormatter1 is IYamlFormatter<T> yamlFormatter2)
        KleiResolver.FormatterCache<T>.Formatter = yamlFormatter2;
      else
        KleiResolver.FormatterCache<T>.Formatter = (IYamlFormatter<T>) null;
    }
  }
}
