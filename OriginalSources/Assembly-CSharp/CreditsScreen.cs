// Decompiled with JetBrains decompiler
// Type: CreditsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CreditsScreen : KModalScreen
{
  public GameObject entryPrefab;
  public GameObject teamHeaderPrefab;
  private Dictionary<string, GameObject> teamContainers = new Dictionary<string, GameObject>();
  public Transform entryContainer;
  public KButton CloseButton;
  public TextAsset[] creditsFiles;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (TextAsset creditsFile in this.creditsFiles)
      this.AddCredits(creditsFile);
    this.CloseButton.onClick += new System.Action(this.Close);
  }

  public void Close() => this.Deactivate();

  private void AddCredits(TextAsset csv)
  {
    string[,] strArray = CSVReader.SplitCsvGrid(csv.text, csv.name);
    List<string> list = new List<string>();
    for (int index = 1; index < strArray.GetLength(1); ++index)
    {
      string str = $"{strArray[0, index]} {strArray[1, index]}";
      if (!(str == " "))
        list.Add(str);
    }
    list.Shuffle<string>();
    string key = strArray[0, 0];
    GameObject gameObject = Util.KInstantiateUI(this.teamHeaderPrefab, this.entryContainer.gameObject, true);
    gameObject.GetComponent<LocText>().text = key;
    this.teamContainers.Add(key, gameObject);
    foreach (string str in list)
      Util.KInstantiateUI(this.entryPrefab, this.teamContainers[key], true).GetComponent<LocText>().text = str;
  }
}
