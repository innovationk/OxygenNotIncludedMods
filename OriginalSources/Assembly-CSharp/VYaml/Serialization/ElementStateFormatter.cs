// Decompiled with JetBrains decompiler
// Type: VYaml.Serialization.ElementStateFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using VYaml.Emitter;
using VYaml.Parser;

#nullable disable
namespace VYaml.Serialization;

public class ElementStateFormatter : IYamlFormatter<Element.State>, IYamlFormatter
{
  public static readonly ElementStateFormatter Instance = new ElementStateFormatter();
  private static readonly (Element.State flag, string name)[] Flags = new (Element.State, string)[3]
  {
    (Element.State.TemperatureInsulated, "TemperatureInsulated"),
    (Element.State.Unstable, "Unstable"),
    (Element.State.Unbreakable, "Unbreakable")
  };

  public void Serialize(
    ref Utf8YamlEmitter emitter,
    Element.State value,
    YamlSerializationContext context)
  {
    List<string> values = new List<string>()
    {
      (value & Element.State.Solid).ToString()
    };
    foreach ((Element.State flag, string name) in ElementStateFormatter.Flags)
    {
      if ((value & flag) != Element.State.Vacuum)
        values.Add(name);
    }
    emitter.WriteString(string.Join(", ", (IEnumerable<string>) values));
  }

  public Element.State Deserialize(ref YamlParser parser, YamlDeserializationContext context)
  {
    if (parser.IsNullScalar())
    {
      parser.Read();
      return Element.State.Vacuum;
    }
    string str1 = parser.ReadScalarAsString();
    if (str1 == null)
      return Element.State.Vacuum;
    Element.State state = Element.State.Vacuum;
    foreach (string str2 in str1.Split(',', StringSplitOptions.None))
    {
      string str3 = str2.Trim();
      Element.State result;
      if (Enum.TryParse<Element.State>(str3, true, out result))
        state |= result;
      else
        Debug.LogWarning((object) $"ElementStateFormatter: Unknown state flag '{str3}'");
    }
    return state;
  }
}
