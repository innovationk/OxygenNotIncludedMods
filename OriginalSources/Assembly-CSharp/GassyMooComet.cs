// Decompiled with JetBrains decompiler
// Type: GassyMooComet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GassyMooComet : Comet
{
  public const float MOO_ANGLE = 15f;
  public Vector3 mooSpawnImpactOffset = new Vector3(-0.5f, 0.0f, 0.0f);
  private bool? initialFlipState;

  public void SetCustomInitialFlip(bool state) => this.initialFlipState = new bool?(state);

  public override void RandomizeVelocity()
  {
    bool flag = false;
    byte id = Grid.WorldIdx[Grid.PosToCell(this.gameObject.transform.position)];
    WorldContainer world = ClusterManager.Instance.GetWorld((int) id);
    if ((Object) world == (Object) null)
      return;
    int num1 = world.WorldOffset.x + world.Width / 2;
    if (Grid.PosToXY(this.gameObject.transform.position).x > num1)
      flag = true;
    if (this.initialFlipState.HasValue)
      flag = this.initialFlipState.Value;
    float f = (float) ((flag ? -75.0 : -105.0) * 3.1415927410125732 / 180.0);
    float num2 = Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
    this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
    this.GetComponent<KBatchedAnimController>().FlipX = flag;
  }

  protected override void SpawnCraterPrefabs()
  {
    KBatchedAnimController animController = this.GetComponent<KBatchedAnimController>();
    animController.Play((HashedString) "landing");
    animController.onAnimComplete += (KAnimControllerBase.KAnimEvent) (obj =>
    {
      if (this.craterPrefabs != null && this.craterPrefabs.Length != 0)
      {
        byte world = Grid.WorldIdx[Grid.PosToCell(this.gameObject.transform.position)];
        float x = 0.0f;
        int cell1 = Grid.PosToCell(this.transform.GetPosition());
        int cell2 = Grid.OffsetCell(cell1, 0, 1);
        int num1 = Grid.OffsetCell(cell1, 0, -1);
        int num2 = !Grid.IsValidCellInWorld(cell2, (int) world) ? num1 : cell2;
        if (Grid.Solid[num2])
        {
          bool flipX = animController.FlipX;
          int num3 = Grid.OffsetCell(num2, -1, 0);
          int num4 = Grid.OffsetCell(num2, 2, 0);
          if (!flipX && Grid.IsValidCell(num3) && !Grid.Solid[num3])
            num2 = num3;
          else if (flipX && Grid.IsValidCell(num4) && !Grid.Solid[num4])
            num2 = num4;
        }
        else
          x = this.gameObject.transform.position.x - Mathf.Floor(this.gameObject.transform.position.x);
        Vector3 position = Grid.CellToPos(num2) + new Vector3(x, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Creatures));
        GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) this.craterPrefabs[Random.Range(0, this.craterPrefabs.Length)]), position);
        Vector3 pos = gameObject.transform.position + this.mooSpawnImpactOffset;
        if (!Grid.Solid[Grid.PosToCell(pos)])
          gameObject.transform.position = pos;
        gameObject.GetComponent<KBatchedAnimController>().FlipX = animController.FlipX;
        gameObject.SetActive(true);
      }
      Util.KDestroyGameObject(this.gameObject);
    });
  }
}
