// Decompiled with JetBrains decompiler
// Type: NotificationExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class NotificationExtensions
{
  public static string ReduceMessages(this List<Notification> notifications, bool countNames = true)
  {
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    foreach (Notification notification in notifications)
    {
      int num = 0;
      if (!dictionary.TryGetValue(notification.NotifierName, out num))
        dictionary[notification.NotifierName] = 0;
      dictionary[notification.NotifierName] = num + 1;
    }
    string str = "";
    foreach (KeyValuePair<string, int> keyValuePair in dictionary)
    {
      if (countNames)
        str = $"{str}\n{keyValuePair.Key}({keyValuePair.Value.ToString()})";
      else
        str = $"{str}\n{keyValuePair.Key}";
    }
    return str;
  }
}
