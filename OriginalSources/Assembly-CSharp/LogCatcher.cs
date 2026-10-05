// Decompiled with JetBrains decompiler
// Type: LogCatcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LogCatcher : ILogHandler
{
  private ILogHandler def;

  public LogCatcher(ILogHandler old) => this.def = old;

  void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
  {
    string str1 = exception.ToString();
    string str2 = context != (UnityEngine.Object) null ? context.ToString() : (string) null;
    if (str1 == "False" || str2 == "False")
      Debug.LogError((object) "False only message!");
    this.def.LogException(exception, context);
  }

  void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
  {
    if (string.Format(format, args) == "False")
      Debug.LogError((object) "False only message!");
    this.def.LogFormat(logType, context, format, args);
  }
}
