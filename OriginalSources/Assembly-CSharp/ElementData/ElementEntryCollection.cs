// Decompiled with JetBrains decompiler
// Type: ElementData.ElementEntryCollection
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
public class ElementEntryCollection
{
  public 
  #nullable disable
  ElementEntry[] elements { get; set; }

  [Preserve]
  public static void __RegisterVYamlFormatter()
  {
    GeneratedResolver.Register<ElementEntryCollection>((IYamlFormatter<ElementEntryCollection>) new ElementEntryCollection.ElementEntryCollectionGeneratedFormatter());
  }

  [Preserve]
  public class ElementEntryCollectionGeneratedFormatter : 
    IYamlFormatter<
    #nullable enable
    ElementEntryCollection?>,
    IYamlFormatter
  {
    private static readonly byte[] elementsKeyUtf8Bytes = new byte[8]
    {
      (byte) 101,
      (byte) 108,
      (byte) 101,
      (byte) 109,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 115
    };

    [Preserve]
    public void Serialize(
      ref Utf8YamlEmitter emitter,
      ElementEntryCollection? value,
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
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntryCollection.ElementEntryCollectionGeneratedFormatter.elementsKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntryCollection.ElementEntryCollectionGeneratedFormatter.elementsKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<ElementEntry[]>(ref emitter, value.elements);
        emitter.EndMapping();
      }
    }

    [Preserve]
    public ElementEntryCollection? Deserialize(
      ref YamlParser parser,
      YamlDeserializationContext context)
    {
      if (parser.IsNullScalar())
      {
        parser.Read();
        return (ElementEntryCollection) null;
      }
      parser.ReadWithVerify(ParseEventType.MappingStart);
      ElementEntry[] elementEntryArray = (ElementEntry[]) null;
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
        if (span.Length == 8 && MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntryCollection.ElementEntryCollectionGeneratedFormatter.elementsKeyUtf8Bytes)))
        {
          parser.Read();
          elementEntryArray = context.DeserializeWithAlias<ElementEntry[]>(ref parser);
        }
        else
        {
          parser.Read();
          parser.SkipCurrentNode();
        }
      }
      parser.ReadWithVerify(ParseEventType.MappingEnd);
      return new ElementEntryCollection()
      {
        elements = elementEntryArray
      };
    }
  }
}
