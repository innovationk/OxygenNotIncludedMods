// Decompiled with JetBrains decompiler
// Type: VYaml.Serialization.Vector2fFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using VYaml.Emitter;
using VYaml.Parser;

#nullable disable
namespace VYaml.Serialization;

public class Vector2fFormatter : IYamlFormatter<Vector2f>, IYamlFormatter
{
  public static readonly Vector2fFormatter Instance = new Vector2fFormatter();

  public void Serialize(
    ref Utf8YamlEmitter emitter,
    Vector2f value,
    YamlSerializationContext context)
  {
    emitter.BeginMapping();
    emitter.WriteString("X");
    emitter.WriteFloat(value.x);
    emitter.WriteString("Y");
    emitter.WriteFloat(value.y);
    emitter.EndMapping();
  }

  public Vector2f Deserialize(ref YamlParser parser, YamlDeserializationContext context)
  {
    if (parser.IsNullScalar())
    {
      parser.Read();
      return new Vector2f();
    }
    parser.ReadWithVerify(ParseEventType.MappingStart);
    parser.ReadScalarAsString();
    double a = (double) parser.ReadScalarAsFloat();
    parser.ReadScalarAsString();
    float num = parser.ReadScalarAsFloat();
    parser.ReadWithVerify(ParseEventType.MappingEnd);
    double b = (double) num;
    return new Vector2f((float) a, (float) b);
  }
}
