// Decompiled with JetBrains decompiler
// Type: Database.RoomTypeCategories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
{
  public RoomTypeCategory None;
  public RoomTypeCategory Food;
  public RoomTypeCategory Sleep;
  public RoomTypeCategory Recreation;
  public RoomTypeCategory Bathroom;
  public RoomTypeCategory Bionic;
  public RoomTypeCategory Hospital;
  public RoomTypeCategory Industrial;
  public RoomTypeCategory Agricultural;
  public RoomTypeCategory Park;
  public RoomTypeCategory Science;

  private RoomTypeCategory Add(string id, string name, string colorName, string icon)
  {
    RoomTypeCategory resource = new RoomTypeCategory(id, name, colorName, icon);
    this.Add(resource);
    return resource;
  }

  public RoomTypeCategories(ResourceSet parent)
    : base(nameof (RoomTypeCategories), parent)
  {
    this.Initialize();
    this.None = this.Add(nameof (None), (string) ROOMS.CATEGORY.NONE.NAME, "roomNone", "unknown");
    this.Food = this.Add(nameof (Food), (string) ROOMS.CATEGORY.FOOD.NAME, "roomFood", "ui_room_food");
    this.Sleep = this.Add(nameof (Sleep), (string) ROOMS.CATEGORY.SLEEP.NAME, "roomSleep", "ui_room_sleep");
    this.Recreation = this.Add(nameof (Recreation), (string) ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
    if (DlcManager.IsContentSubscribed("DLC3_ID"))
      this.Bionic = this.Add(nameof (Bionic), (string) ROOMS.CATEGORY.BIONIC.NAME, "roomBionic", "ui_room_bionicupkeep");
    this.Bathroom = this.Add(nameof (Bathroom), (string) ROOMS.CATEGORY.BATHROOM.NAME, "roomBathroom", "ui_room_bathroom");
    this.Hospital = this.Add(nameof (Hospital), (string) ROOMS.CATEGORY.HOSPITAL.NAME, "roomHospital", "ui_room_hospital");
    this.Industrial = this.Add(nameof (Industrial), (string) ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
    this.Agricultural = this.Add(nameof (Agricultural), (string) ROOMS.CATEGORY.AGRICULTURAL.NAME, "roomAgricultural", "ui_room_agricultural");
    this.Park = this.Add(nameof (Park), (string) ROOMS.CATEGORY.PARK.NAME, "roomPark", "ui_room_park");
    this.Science = this.Add(nameof (Science), (string) ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
  }
}
