// Decompiled with JetBrains decompiler
// Type: Option`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
[DebuggerDisplay("has_value={hasValue} {value}")]
[Serializable]
public readonly struct Option<T>(T value) : IEquatable<Option<T>>, IEquatable<T>
{
  [Serialize]
  private readonly bool hasValue = true;
  [Serialize]
  private readonly T value = value;

  public bool HasValue => this.hasValue;

  public T Value => this.Unwrap();

  public T Unwrap()
  {
    if (!this.hasValue)
      throw new Exception($"Tried to get a value for a Option<{typeof (T).FullName}>, but hasValue is false");
    return this.value;
  }

  public T UnwrapOr(T fallback_value, string warn_on_fallback = null)
  {
    if (this.hasValue)
      return this.value;
    if (warn_on_fallback != null)
      DebugUtil.DevAssert(false, $"Failed to unwrap a Option<{typeof (T).FullName}>: {warn_on_fallback}");
    return fallback_value;
  }

  public T UnwrapOrElse(Func<T> get_fallback_value_fn, string warn_on_fallback = null)
  {
    if (this.hasValue)
      return this.value;
    if (warn_on_fallback != null)
      DebugUtil.DevAssert(false, $"Failed to unwrap a Option<{typeof (T).FullName}>: {warn_on_fallback}");
    return get_fallback_value_fn();
  }

  public T UnwrapOrDefault() => !this.hasValue ? default (T) : this.value;

  public T Expect(string msg_on_fail)
  {
    if (!this.hasValue)
      throw new Exception(msg_on_fail);
    return this.value;
  }

  public bool IsSome() => this.hasValue;

  public bool IsNone() => !this.hasValue;

  public Option<U> AndThen<U>(Func<T, U> fn)
  {
    return this.IsNone() ? (Option<U>) Option.None : Option.Maybe<U>(fn(this.value));
  }

  public Option<U> AndThen<U>(Func<T, Option<U>> fn)
  {
    return this.IsNone() ? (Option<U>) Option.None : fn(this.value);
  }

  public static implicit operator Option<T>(T value) => Option.Maybe<T>(value);

  public static explicit operator T(Option<T> option) => option.Unwrap();

  public static implicit operator Option<T>(Option.Internal.Value_None value) => new Option<T>();

  public static implicit operator Option.Internal.Value_HasValue(Option<T> value)
  {
    return new Option.Internal.Value_HasValue(value.hasValue);
  }

  public void Deconstruct(out bool hasValue, out T value)
  {
    hasValue = this.hasValue;
    value = this.value;
  }

  public bool Equals(Option<T> other)
  {
    return EqualityComparer<bool>.Default.Equals(this.hasValue, other.hasValue) && EqualityComparer<T>.Default.Equals(this.value, other.value);
  }

  public override bool Equals(object obj) => obj is Option<T> other && this.Equals(other);

  public static bool operator ==(Option<T> lhs, Option<T> rhs) => lhs.Equals(rhs);

  public static bool operator !=(Option<T> lhs, Option<T> rhs) => !(lhs == rhs);

  public override int GetHashCode()
  {
    return (-363764631 * -1521134295 + this.hasValue.GetHashCode()) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.value);
  }

  public override string ToString() => !this.hasValue ? "None" : $"{this.value}";

  public static bool operator ==(Option<T> lhs, T rhs) => lhs.Equals(rhs);

  public static bool operator !=(Option<T> lhs, T rhs) => !(lhs == rhs);

  public static bool operator ==(T lhs, Option<T> rhs) => rhs.Equals(lhs);

  public static bool operator !=(T lhs, Option<T> rhs) => !(lhs == rhs);

  public bool Equals(T other)
  {
    return this.HasValue && EqualityComparer<T>.Default.Equals(this.value, other);
  }

  public static Option<T> None => new Option<T>();
}
