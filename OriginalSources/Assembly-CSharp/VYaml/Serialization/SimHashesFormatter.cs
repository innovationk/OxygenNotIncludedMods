// Decompiled with JetBrains decompiler
// Type: VYaml.Serialization.SimHashesFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using VYaml.Emitter;
using VYaml.Parser;

#nullable disable
namespace VYaml.Serialization;

public class SimHashesFormatter : IYamlFormatter<SimHashes>, IYamlFormatter
{
  public static readonly SimHashesFormatter Instance = new SimHashesFormatter();

  public void Serialize(
    ref Utf8YamlEmitter emitter,
    SimHashes value,
    YamlSerializationContext context)
  {
    emitter.BeginSequence(SequenceStyle.Flow);
    emitter.WriteString(value.ToString());
    emitter.EndSequence();
  }

  public SimHashes Deserialize(ref YamlParser parser, YamlDeserializationContext context)
  {
    if (parser.IsNullScalar())
    {
      parser.Read();
      return (SimHashes) 0;
    }
    parser.ReadWithVerify(ParseEventType.SequenceStart);
    Element elementByName = ElementLoader.FindElementByName(parser.ReadScalarAsString());
    parser.ReadWithVerify(ParseEventType.SequenceEnd);
    if (elementByName != null)
      return elementByName.id;
    Debug.LogWarning((object) "SimHashesFormatter: Could not find element, defaulting to Unobtanium");
    return SimHashes.Unobtanium;
  }
}
