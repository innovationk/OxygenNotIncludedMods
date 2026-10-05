// Decompiled with JetBrains decompiler
// Type: OreSizeVisualizerComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OreSizeVisualizerComponents : KGameObjectComponentManager<OreSizeVisualizerData>
{
  private static readonly OreSizeVisualizerComponents.MassTier[] MassTiers = new OreSizeVisualizerComponents.MassTier[3]
  {
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle1",
      massRequired = 50f,
      colliderRadius = 0.15f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle2",
      massRequired = 600f,
      colliderRadius = 0.2f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle3",
      massRequired = float.MaxValue,
      colliderRadius = 0.25f
    }
  };
  private static readonly OreSizeVisualizerComponents.MassTier[] PokeShellMassTiers = new OreSizeVisualizerComponents.MassTier[3]
  {
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle1",
      massRequired = 45f,
      colliderRadius = 0.15f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle2",
      massRequired = 90f,
      colliderRadius = 0.2f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle3",
      massRequired = float.MaxValue,
      colliderRadius = 0.25f
    }
  };
  private static readonly OreSizeVisualizerComponents.MassTier[] WoodPokeShellMassTiers = new OreSizeVisualizerComponents.MassTier[3]
  {
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle1",
      massRequired = 75f,
      colliderRadius = 0.15f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle2",
      massRequired = 150f,
      colliderRadius = 0.2f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle3",
      massRequired = float.MaxValue,
      colliderRadius = 0.25f
    }
  };
  private static readonly OreSizeVisualizerComponents.MassTier[] PlantMatterMassTiers = new OreSizeVisualizerComponents.MassTier[3]
  {
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle1",
      massRequired = 10f,
      colliderRadius = 0.15f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle2",
      massRequired = 50f,
      colliderRadius = 0.2f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle3",
      massRequired = float.MaxValue,
      colliderRadius = 0.25f
    }
  };
  private static readonly Dictionary<OreSizeVisualizerComponents.TiersSetType, OreSizeVisualizerComponents.MassTier[]> TierSets = new Dictionary<OreSizeVisualizerComponents.TiersSetType, OreSizeVisualizerComponents.MassTier[]>()
  {
    [OreSizeVisualizerComponents.TiersSetType.Ores] = OreSizeVisualizerComponents.MassTiers,
    [OreSizeVisualizerComponents.TiersSetType.PokeShells] = OreSizeVisualizerComponents.PokeShellMassTiers,
    [OreSizeVisualizerComponents.TiersSetType.WoodPokeShells] = OreSizeVisualizerComponents.WoodPokeShellMassTiers,
    [OreSizeVisualizerComponents.TiersSetType.PlantFiber] = OreSizeVisualizerComponents.PlantMatterMassTiers
  };
  private static Action<object, object> OnMassChangedDispatcher = (Action<object, object>) ((context, data) => OreSizeVisualizerComponents.OnMassChanged((HandleVector<int>.Handle) context, data));

  public HandleVector<int>.Handle Add(GameObject go)
  {
    HandleVector<int>.Handle h = this.Add(go, new OreSizeVisualizerData(go));
    this.OnPrefabInit(h);
    return h;
  }

  public static HashedString GetAnimForMass(float mass)
  {
    return OreSizeVisualizerComponents.GetAnimForMass(OreSizeVisualizerComponents.TiersSetType.Ores, mass);
  }

  public static HashedString GetAnimForMass(
    OreSizeVisualizerComponents.TiersSetType tierType,
    float mass)
  {
    for (int index = 0; index < OreSizeVisualizerComponents.TierSets[tierType].Length; ++index)
    {
      if ((double) mass <= (double) OreSizeVisualizerComponents.TierSets[tierType][index].massRequired)
        return OreSizeVisualizerComponents.TierSets[tierType][index].animName;
    }
    return HashedString.Invalid;
  }

  protected override void OnPrefabInit(HandleVector<int>.Handle handle)
  {
    OreSizeVisualizerData data = this.GetData(handle);
    data.absorbHandle = data.primaryElement.Subscribe(-2064133523, OreSizeVisualizerComponents.OnMassChangedDispatcher, (object) handle);
    data.splitFromChunkHandle = data.primaryElement.Subscribe(1335436905, OreSizeVisualizerComponents.OnMassChangedDispatcher, (object) handle);
    this.SetData(handle, data);
  }

  protected override void OnSpawn(HandleVector<int>.Handle handle)
  {
    OreSizeVisualizerComponents.OnMassChanged(handle, (object) null);
  }

  protected override void OnCleanUp(HandleVector<int>.Handle handle)
  {
    OreSizeVisualizerData data = this.GetData(handle);
    if (!((UnityEngine.Object) data.primaryElement != (UnityEngine.Object) null))
      return;
    data.primaryElement.Unsubscribe(ref data.absorbHandle);
    data.primaryElement.Unsubscribe(ref data.splitFromChunkHandle);
  }

  private static void OnMassChanged(HandleVector<int>.Handle handle, object other_data)
  {
    OreSizeVisualizerData data = GameComps.OreSizeVisualizers.GetData(handle);
    PrimaryElement primaryElement = data.primaryElement;
    float mass = primaryElement.Mass;
    OreSizeVisualizerComponents.MassTier massTier = new OreSizeVisualizerComponents.MassTier();
    OreSizeVisualizerComponents.MassTier[] tierSet = OreSizeVisualizerComponents.TierSets[data.tierSetType];
    for (int index = 0; index < tierSet.Length; ++index)
    {
      if ((double) mass <= (double) tierSet[index].massRequired)
      {
        massTier = tierSet[index];
        break;
      }
    }
    primaryElement.GetComponent<KBatchedAnimController>().Play(massTier.animName);
    KCircleCollider2D component = primaryElement.GetComponent<KCircleCollider2D>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.radius = massTier.colliderRadius;
    primaryElement.Trigger(1807976145, (object) null);
  }

  private struct MassTier
  {
    public HashedString animName;
    public float massRequired;
    public float colliderRadius;
  }

  public enum TiersSetType
  {
    Ores,
    PokeShells,
    WoodPokeShells,
    PlantFiber,
  }
}
