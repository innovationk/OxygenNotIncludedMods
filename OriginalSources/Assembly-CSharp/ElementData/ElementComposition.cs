// Decompiled with JetBrains decompiler
// Type: ElementData.ElementComposition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using VYaml.Annotations;
using VYaml.Emitter;
using VYaml.Parser;
using VYaml.Serialization;

#nullable enable
namespace ElementData;

[YamlObject(NamingConvention.LowerCamelCase)]
public class ElementComposition
{
  public 
  #nullable disable
  string elementID { get; set; }

  public float percentage { get; set; }

  [Preserve]
  public static void __RegisterVYamlFormatter()
  {
    GeneratedResolver.Register<ElementComposition>((IYamlFormatter<ElementComposition>) new ElementComposition.ElementCompositionGeneratedFormatter());
  }

  [Preserve]
  public class ElementCompositionGeneratedFormatter : 
    IYamlFormatter<
    #nullable enable
    ElementComposition?>,
    IYamlFormatter
  {
    private static readonly byte[] elementIDKeyUtf8Bytes = new byte[9]
    {
      (byte) 101,
      (byte) 108,
      (byte) 101,
      (byte) 109,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 73,
      (byte) 68
    };
    private static readonly byte[] percentageKeyUtf8Bytes = new byte[10]
    {
      (byte) 112 /*0x70*/,
      (byte) 101,
      (byte) 114,
      (byte) 99,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 97,
      (byte) 103,
      (byte) 101
    };

    [Preserve]
    public void Serialize(
      ref Utf8YamlEmitter emitter,
      ElementComposition? value,
      YamlSerializationContext context)
    {
      if (value == null)
      {
        emitter.WriteNull();
      }
      else
      {
        emitter.BeginMapping();
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementComposition.ElementCompositionGeneratedFormatter.elementIDKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementComposition.ElementCompositionGeneratedFormatter.elementIDKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.elementID);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementComposition.ElementCompositionGeneratedFormatter.percentageKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementComposition.ElementCompositionGeneratedFormatter.percentageKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.percentage);
        emitter.EndMapping();
      }
    }

    [Preserve]
    public ElementComposition? Deserialize(
      ref YamlParser parser,
      YamlDeserializationContext context)
    {
      if (parser.IsNullScalar())
      {
        parser.Read();
        return (ElementComposition) null;
      }
      parser.ReadWithVerify(ParseEventType.MappingStart);
      string str = (string) null;
      float num = 0.0f;
      while (!parser.End && parser.CurrentEventType != ParseEventType.MappingEnd)
      {
        if (parser.CurrentEventType != ParseEventType.Scalar)
          throw new YamlSerializerException(parser.CurrentMark, "Custom type deserialization supports only string key");
        ReadOnlySpan<byte> span;
        if (!parser.TryGetScalarAsSpan(out span))
          throw new YamlSerializerException(parser.CurrentMark, "Custom type deserialization supports only string key");
        if (context.Options.NamingConvention != NamingConvention.LowerCamelCase)
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(span, NamingConvention.LowerCamelCase, out threadStaticBuffer, out written);
          span = Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written));
        }
        switch (span.Length)
        {
          case 9:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementComposition.ElementCompositionGeneratedFormatter.elementIDKeyUtf8Bytes)))
            {
              parser.Read();
              str = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 10:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementComposition.ElementCompositionGeneratedFormatter.percentageKeyUtf8Bytes)))
            {
              parser.Read();
              num = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
        }
        parser.Read();
        parser.SkipCurrentNode();
      }
      parser.ReadWithVerify(ParseEventType.MappingEnd);
      return new ElementComposition()
      {
        elementID = str,
        percentage = num
      };
    }
  }
}
