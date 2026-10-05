// Decompiled with JetBrains decompiler
// Type: VYaml.Serialization.TagFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using VYaml.Emitter;
using VYaml.Parser;

#nullable disable
namespace VYaml.Serialization;

public class TagFormatter : IYamlFormatter<Tag>, IYamlFormatter
{
  public static readonly TagFormatter Instance = new TagFormatter();

  public void Serialize(ref Utf8YamlEmitter emitter, Tag value, YamlSerializationContext context)
  {
    emitter.BeginSequence(SequenceStyle.Flow);
    emitter.WriteString(value.Name);
    emitter.EndSequence();
  }

  public Tag Deserialize(ref YamlParser parser, YamlDeserializationContext context)
  {
    if (parser.IsNullScalar())
    {
      parser.Read();
      return new Tag();
    }
    parser.ReadWithVerify(ParseEventType.SequenceStart);
    string tag_string = parser.ReadScalarAsString();
    parser.ReadWithVerify(ParseEventType.SequenceEnd);
    return TagManager.Create(tag_string);
  }
}
