// Decompiled with JetBrains decompiler
// Type: SequenceUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class SequenceUtil
{
  private static UnityEngine.WaitForEndOfFrame waitForEndOfFrame = (UnityEngine.WaitForEndOfFrame) null;
  private static UnityEngine.WaitForFixedUpdate waitForFixedUpdate = (UnityEngine.WaitForFixedUpdate) null;
  private static Dictionary<float, UnityEngine.WaitForSeconds> scaledTimeCache = new Dictionary<float, UnityEngine.WaitForSeconds>();
  private static Dictionary<float, UnityEngine.WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, UnityEngine.WaitForSecondsRealtime>();

  public static YieldInstruction WaitForNextFrame => (YieldInstruction) null;

  public static YieldInstruction WaitForEndOfFrame
  {
    get
    {
      if (SequenceUtil.waitForEndOfFrame == null)
        SequenceUtil.waitForEndOfFrame = new UnityEngine.WaitForEndOfFrame();
      return (YieldInstruction) SequenceUtil.waitForEndOfFrame;
    }
  }

  public static YieldInstruction WaitForFixedUpdate
  {
    get
    {
      if (SequenceUtil.waitForFixedUpdate == null)
        SequenceUtil.waitForFixedUpdate = new UnityEngine.WaitForFixedUpdate();
      return (YieldInstruction) SequenceUtil.waitForFixedUpdate;
    }
  }

  public static YieldInstruction WaitForSeconds(float duration)
  {
    UnityEngine.WaitForSeconds waitForSeconds;
    if (!SequenceUtil.scaledTimeCache.TryGetValue(duration, out waitForSeconds))
      waitForSeconds = SequenceUtil.scaledTimeCache[duration] = new UnityEngine.WaitForSeconds(duration);
    return (YieldInstruction) waitForSeconds;
  }

  public static UnityEngine.WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
  {
    UnityEngine.WaitForSecondsRealtime forSecondsRealtime;
    if (!SequenceUtil.reailTimeWaitCache.TryGetValue(duration, out forSecondsRealtime))
      forSecondsRealtime = SequenceUtil.reailTimeWaitCache[duration] = new UnityEngine.WaitForSecondsRealtime(duration);
    return forSecondsRealtime;
  }
}
