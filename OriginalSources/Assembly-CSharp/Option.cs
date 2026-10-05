// Decompiled with JetBrains decompiler
// Type: Option
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
public static class Option
{
  public static Option<T> Some<T>(T value) => new Option<T>(value);

  public static Option<T> Maybe<T>(T value)
  {
    return ((object) value).IsNullOrDestroyed() ? new Option<T>() : new Option<T>(value);
  }

  public static Option.Internal.Value_None None => new Option.Internal.Value_None();

  public static bool AllHaveValues(params Option.Internal.Value_HasValue[] options)
  {
    if (options == null || options.Length == 0)
      return false;
    for (int index = 0; index < options.Length; ++index)
    {
      if (!options[index].HasValue)
        return false;
    }
    return true;
  }

  public static class Internal
  {
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct Value_None
    {
    }

    public readonly struct Value_HasValue(bool hasValue)
    {
      public readonly bool HasValue = hasValue;
    }
  }
}
