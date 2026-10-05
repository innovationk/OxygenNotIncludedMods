// Decompiled with JetBrains decompiler
// Type: SaveLoadRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SaveLoadRoot")]
public class SaveLoadRoot : KMonoBehaviour
{
  public Tag associatedTag;
  private bool hasOnSpawnRun;
  private bool registered = true;
  [SerializeField]
  private List<string> m_optionalComponentTypeNames = new List<string>();
  private static Dictionary<string, ISerializableComponentManager> serializableComponentManagers;
  private static Dictionary<System.Type, string> sTypeToString = new Dictionary<System.Type, string>();

  public static void DestroyStatics()
  {
    SaveLoadRoot.serializableComponentManagers = (Dictionary<string, ISerializableComponentManager>) null;
  }

  protected override void OnPrefabInit()
  {
    if (SaveLoadRoot.serializableComponentManagers != null)
      return;
    SaveLoadRoot.serializableComponentManagers = new Dictionary<string, ISerializableComponentManager>();
    foreach (FieldInfo field in typeof (GameComps).GetFields())
    {
      IComponentManager componentManager = (IComponentManager) field.GetValue((object) null);
      if (typeof (ISerializableComponentManager).IsAssignableFrom(componentManager.GetType()))
      {
        System.Type type = componentManager.GetType();
        SaveLoadRoot.serializableComponentManagers[type.ToString()] = (ISerializableComponentManager) componentManager;
      }
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.registered)
      SaveLoader.Instance.saveManager.Register(this);
    this.hasOnSpawnRun = true;
  }

  public void DeclareOptionalComponent<T>() where T : KMonoBehaviour
  {
    this.m_optionalComponentTypeNames.Add(typeof (T).ToString());
  }

  public void SetRegistered(bool registered)
  {
    if (this.registered == registered)
      return;
    this.registered = registered;
    if (!this.hasOnSpawnRun)
      return;
    if (registered)
      SaveLoader.Instance.saveManager.Register(this);
    else
      SaveLoader.Instance.saveManager.Unregister(this);
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SaveLoader.Instance.saveManager != (UnityEngine.Object) null)
      SaveLoader.Instance.saveManager.Unregister(this);
    if (!GameComps.WhiteBoards.Has((object) this.gameObject))
      return;
    GameComps.WhiteBoards.Remove(this.gameObject);
  }

  public void Save(BinaryWriter writer)
  {
    Transform transform = this.transform;
    writer.Write(transform.GetPosition());
    writer.Write(transform.rotation);
    writer.Write(transform.localScale);
    byte num = 0;
    writer.Write(num);
    this.SaveWithoutTransform(writer);
  }

  public void SaveWithoutTransform(BinaryWriter writer)
  {
    KMonoBehaviour[] components = this.GetComponents<KMonoBehaviour>();
    if (components == null)
      return;
    int num1 = 0;
    foreach (KMonoBehaviour kmonoBehaviour in components)
    {
      if ((kmonoBehaviour is ISaveLoadableDetails || kmonoBehaviour != null) && !kmonoBehaviour.GetType().IsDefined(typeof (SkipSaveFileSerialization), false))
        ++num1;
    }
    foreach (KeyValuePair<string, ISerializableComponentManager> componentManager in SaveLoadRoot.serializableComponentManagers)
    {
      if (componentManager.Value.Has((object) this.gameObject))
        ++num1;
    }
    writer.Write(num1);
    foreach (KMonoBehaviour kmonoBehaviour in components)
    {
      if ((kmonoBehaviour is ISaveLoadableDetails || kmonoBehaviour != null) && !kmonoBehaviour.GetType().IsDefined(typeof (SkipSaveFileSerialization), false))
      {
        writer.WriteKleiString(kmonoBehaviour.GetType().ToString());
        long position1 = writer.BaseStream.Position;
        writer.Write(0);
        long position2 = writer.BaseStream.Position;
        if (kmonoBehaviour is ISaveLoadableDetails)
        {
          ISaveLoadableDetails saveLoadableDetails = (ISaveLoadableDetails) kmonoBehaviour;
          Serializer.SerializeTypeless((object) kmonoBehaviour, writer);
          BinaryWriter writer1 = writer;
          saveLoadableDetails.Serialize(writer1);
        }
        else if (kmonoBehaviour != null)
          Serializer.SerializeTypeless((object) kmonoBehaviour, writer);
        long position3 = writer.BaseStream.Position;
        long num2 = position3 - position2;
        writer.BaseStream.Position = position1;
        writer.Write((int) num2);
        writer.BaseStream.Position = position3;
      }
    }
    foreach (KeyValuePair<string, ISerializableComponentManager> componentManager1 in SaveLoadRoot.serializableComponentManagers)
    {
      ISerializableComponentManager componentManager2 = componentManager1.Value;
      if (componentManager2.Has((object) this.gameObject))
      {
        string key = componentManager1.Key;
        writer.WriteKleiString(key);
        componentManager2.Serialize(this.gameObject, writer);
      }
    }
  }

  public static SaveLoadRoot Load(Tag tag, IReader reader)
  {
    return SaveLoadRoot.Load(SaveLoader.Instance.saveManager.GetPrefab(tag), reader);
  }

  public static SaveLoadRoot Load(GameObject prefab, IReader reader)
  {
    Vector3 position = reader.ReadVector3();
    Quaternion rotation = reader.ReadQuaternion();
    Vector3 scale = reader.ReadVector3();
    int num = (int) reader.ReadByte();
    if (SaveManager.DEBUG_OnlyLoadThisCellsObjects > -1)
    {
      Vector3 pos = Grid.CellToPos(SaveManager.DEBUG_OnlyLoadThisCellsObjects);
      if (((double) position.x < (double) pos.x || (double) position.x >= (double) pos.x + 1.0 || (double) position.y < (double) pos.y || (double) position.y >= (double) pos.y + 1.0) && prefab.name != "SaveGame")
        prefab = (GameObject) null;
      else
        Debug.Log((object) ("Keeping " + prefab.name));
    }
    return SaveLoadRoot.Load(prefab, position, rotation, scale, reader);
  }

  public static SaveLoadRoot Load(
    GameObject prefab,
    Vector3 position,
    Quaternion rotation,
    Vector3 scale,
    IReader reader)
  {
    SaveLoadRoot saveLoadRoot = (SaveLoadRoot) null;
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      GameObject gameObject = Util.KInstantiate(prefab, position, rotation, initialize_id: false);
      gameObject.transform.localScale = scale;
      gameObject.SetActive(true);
      saveLoadRoot = gameObject.GetComponent<SaveLoadRoot>();
      if ((UnityEngine.Object) saveLoadRoot != (UnityEngine.Object) null)
      {
        try
        {
          SaveLoadRoot.LoadInternal(gameObject, reader);
        }
        catch (ArgumentException ex)
        {
          DebugUtil.LogErrorArgs((UnityEngine.Object) gameObject, (object) "Failed to load SaveLoadRoot ", (object) ex.Message, (object) "\n", (object) ex.StackTrace);
        }
      }
      else
        Debug.Log((object) "missing SaveLoadRoot", (UnityEngine.Object) gameObject);
    }
    else
      SaveLoadRoot.LoadInternal((GameObject) null, reader);
    return saveLoadRoot;
  }

  private static void LoadInternal(GameObject gameObject, IReader reader)
  {
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    KMonoBehaviour[] components = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponents<KMonoBehaviour>() : (KMonoBehaviour[]) null;
    int num1 = reader.ReadInt32();
    for (int index1 = 0; index1 < num1; ++index1)
    {
      string key = reader.ReadKleiString();
      int length = reader.ReadInt32();
      int position = reader.Position;
      ISerializableComponentManager componentManager;
      if (SaveLoadRoot.serializableComponentManagers.TryGetValue(key, out componentManager))
      {
        componentManager.Deserialize(gameObject, reader);
      }
      else
      {
        int num2 = 0;
        dictionary.TryGetValue(key, out num2);
        KMonoBehaviour kmonoBehaviour = (KMonoBehaviour) null;
        int num3 = 0;
        if (components != null)
        {
          for (int index2 = 0; index2 < components.Length; ++index2)
          {
            System.Type type = components[index2].GetType();
            string str;
            if (!SaveLoadRoot.sTypeToString.TryGetValue(type, out str))
            {
              str = type.ToString();
              SaveLoadRoot.sTypeToString[type] = str;
            }
            if (str == key)
            {
              if (num3 == num2)
              {
                kmonoBehaviour = components[index2];
                break;
              }
              ++num3;
            }
          }
        }
        if ((UnityEngine.Object) kmonoBehaviour == (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
          int index3;
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && (index3 = component.m_optionalComponentTypeNames.IndexOf(key)) != -1)
          {
            DebugUtil.DevAssert(num2 == 0 && num3 == 0, $"Implementation does not support multiple components with optional components, type {key}, {num2}, {num3}. Using only the first one and skipping the rest.");
            System.Type type = System.Type.GetType(component.m_optionalComponentTypeNames[index3]);
            if (num3 == 0)
              kmonoBehaviour = (KMonoBehaviour) gameObject.AddComponent(type);
          }
        }
        if ((UnityEngine.Object) kmonoBehaviour == (UnityEngine.Object) null)
          reader.SkipBytes(length);
        else if (kmonoBehaviour == null && !(kmonoBehaviour is ISaveLoadableDetails))
        {
          DebugUtil.LogErrorArgs((object) "Component", (object) key, (object) "is not ISaveLoadable");
          reader.SkipBytes(length);
        }
        else
        {
          dictionary[key] = num3 + 1;
          if (kmonoBehaviour is ISaveLoadableDetails)
          {
            ISaveLoadableDetails saveLoadableDetails = (ISaveLoadableDetails) kmonoBehaviour;
            Deserializer.DeserializeTypeless((object) kmonoBehaviour, reader);
            IReader reader1 = reader;
            saveLoadableDetails.Deserialize(reader1);
          }
          else
            Deserializer.DeserializeTypeless((object) kmonoBehaviour, reader);
          if (reader.Position != position + length)
          {
            DebugUtil.LogWarningArgs((object) "Expected to be at offset", (object) (position + length), (object) "but was only at offset", (object) reader.Position, (object) ". Skipping to catch up.");
            reader.SkipBytes(position + length - reader.Position);
          }
        }
      }
    }
  }
}
