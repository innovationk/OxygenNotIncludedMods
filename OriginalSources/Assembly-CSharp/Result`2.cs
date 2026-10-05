// Decompiled with JetBrains decompiler
// Type: Result`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public readonly struct Result<TSuccess, TError>
{
  private readonly Option<TSuccess> successValue;
  private readonly Option<TError> errorValue;

  private Result(TSuccess successValue, TError errorValue)
  {
    this.successValue = (Option<TSuccess>) successValue;
    this.errorValue = (Option<TError>) errorValue;
  }

  public bool IsOk() => this.successValue.IsSome();

  public bool IsErr() => this.errorValue.IsSome() || this.successValue.IsNone();

  public TSuccess Unwrap()
  {
    if (this.successValue.IsSome())
      return this.successValue.Unwrap();
    if (this.errorValue.IsSome())
      throw new Exception("Tried to unwrap result that is an Err()");
    throw new Exception("Tried to unwrap result that isn't initialized with an Err() or Ok() value");
  }

  public Option<TSuccess> Ok() => this.successValue;

  public Option<TError> Err() => this.errorValue;

  public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Ok<TSuccess> value)
  {
    return new Result<TSuccess, TError>(value.value, default (TError));
  }

  public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Err<TError> value)
  {
    return new Result<TSuccess, TError>(default (TSuccess), value.value);
  }
}
