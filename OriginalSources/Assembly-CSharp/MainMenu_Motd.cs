// Decompiled with JetBrains decompiler
// Type: MainMenu_Motd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[Serializable]
public class MainMenu_Motd
{
  [SerializeField]
  private MotdBox boxA;
  [SerializeField]
  private MotdBox boxB;
  [SerializeField]
  private MotdBox boxC;
  private MotdDataFetchRequest motdDataFetchRequest;

  public void Setup()
  {
    this.CleanUp();
    this.boxA.gameObject.SetActive(false);
    this.boxB.gameObject.SetActive(false);
    this.boxC.gameObject.SetActive(false);
    this.motdDataFetchRequest = new MotdDataFetchRequest();
    this.motdDataFetchRequest.Fetch(MotdDataFetchRequest.BuildUrl());
    this.motdDataFetchRequest.OnComplete((Action<MotdData>) (motdData => this.RecieveMotdData(motdData)));
  }

  public void CleanUp()
  {
    if (this.motdDataFetchRequest == null)
      return;
    this.motdDataFetchRequest.Dispose();
    this.motdDataFetchRequest = (MotdDataFetchRequest) null;
  }

  private void RecieveMotdData(MotdData motdData)
  {
    if (motdData == null || motdData.boxesLive == null || motdData.boxesLive.Count == 0)
    {
      Debug.LogWarning((object) "MOTD Error: failed to get valid motd data, hiding ui.");
      this.boxA.gameObject.SetActive(false);
      this.boxB.gameObject.SetActive(false);
      this.boxC.gameObject.SetActive(false);
    }
    else
    {
      List<MotdData_Box> boxes = motdData.boxesLive.StableSort<MotdData_Box>((Comparison<MotdData_Box>) ((a, b) => this.CalcScore(a).CompareTo(this.CalcScore(b)))).ToList<MotdData_Box>();
      MotdData_Box box1 = ConsumeBox("PatchNotes");
      MotdData_Box box2 = ConsumeBox("News");
      MotdData_Box box3 = ConsumeBox("Skins");
      if (box1 != null)
      {
        this.boxA.Config(new MotdBox.PageData[1]
        {
          this.ConvertToPageData(box1)
        });
        this.boxA.gameObject.SetActive(true);
      }
      if (box2 != null)
      {
        this.boxB.Config(new MotdBox.PageData[1]
        {
          this.ConvertToPageData(box2)
        });
        this.boxB.gameObject.SetActive(true);
      }
      if (box3 == null)
        return;
      this.boxC.Config(new MotdBox.PageData[1]
      {
        this.ConvertToPageData(box3)
      });
      this.boxC.gameObject.SetActive(true);

      MotdData_Box ConsumeBox(string idealTag)
      {
        if (boxes.Count == 0)
          return (MotdData_Box) null;
        int index1 = -1;
        for (int index2 = 0; index2 < boxes.Count; ++index2)
        {
          if (string.Compare(boxes[index2].category, idealTag, StringComparison.InvariantCultureIgnoreCase) == 0)
          {
            index1 = index2;
            break;
          }
        }
        if (index1 < 0)
          index1 = boxes.Count - 1;
        MotdData_Box motdDataBox = boxes[index1];
        boxes.RemoveAt(index1);
        return motdDataBox;
      }
    }
  }

  private int CalcScore(MotdData_Box box) => 0;

  private MotdBox.PageData ConvertToPageData(MotdData_Box box)
  {
    return new MotdBox.PageData()
    {
      Texture = box.resolvedImage,
      HeaderText = box.title,
      ImageText = box.text,
      URL = box.href
    };
  }
}
