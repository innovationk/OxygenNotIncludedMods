// Decompiled with JetBrains decompiler
// Type: EightDirectionUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class EightDirectionUtil
{
  public static readonly Vector3[] normals = new Vector3[8]
  {
    Vector3.up,
    (Vector3.up + Vector3.left).normalized,
    Vector3.left,
    (Vector3.down + Vector3.left).normalized,
    Vector3.down,
    (Vector3.down + Vector3.right).normalized,
    Vector3.right,
    (Vector3.up + Vector3.right).normalized
  };

  public static int GetDirectionIndex(EightDirection direction) => (int) direction;

  public static EightDirection AngleToDirection(int angle)
  {
    return (EightDirection) Mathf.Floor((float) angle / 45f);
  }

  public static Vector3 GetNormal(EightDirection direction)
  {
    return EightDirectionUtil.normals[EightDirectionUtil.GetDirectionIndex(direction)];
  }

  public static float GetAngle(EightDirection direction)
  {
    return (float) (45 * EightDirectionUtil.GetDirectionIndex(direction));
  }
}
