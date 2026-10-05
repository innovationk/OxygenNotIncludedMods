// Decompiled with JetBrains decompiler
// Type: BatchAnimCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BatchAnimCamera : MonoBehaviour
{
  private static readonly float pan_speed = 5f;
  private static readonly float zoom_speed = 5f;
  public static Bounds bounds = new Bounds(new Vector3(0.0f, 0.0f, -50f), new Vector3(0.0f, 0.0f, 50f));
  private float zoom_min = 1f;
  private float zoom_max = 100f;
  private Camera cam;
  private bool do_pan;
  private Vector3 last_pan;

  private void Awake() => this.cam = this.GetComponent<Camera>();

  private void Update()
  {
    if (Input.GetKey(KeyCode.RightArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.right * BatchAnimCamera.pan_speed * Time.deltaTime);
    if (Input.GetKey(KeyCode.LeftArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.left * BatchAnimCamera.pan_speed * Time.deltaTime);
    if (Input.GetKey(KeyCode.UpArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.up * BatchAnimCamera.pan_speed * Time.deltaTime);
    if (Input.GetKey(KeyCode.DownArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.down * BatchAnimCamera.pan_speed * Time.deltaTime);
    this.ClampToBounds();
    if (Input.GetKey(KeyCode.LeftShift))
    {
      if (Input.GetMouseButtonDown(0))
      {
        this.do_pan = true;
        this.last_pan = KInputManager.GetMousePos();
      }
      else if (Input.GetMouseButton(0) && this.do_pan)
      {
        Vector3 viewportPoint = this.cam.ScreenToViewportPoint(this.last_pan - KInputManager.GetMousePos());
        this.transform.Translate(new Vector3(viewportPoint.x * BatchAnimCamera.pan_speed, viewportPoint.y * BatchAnimCamera.pan_speed, 0.0f), Space.World);
        this.ClampToBounds();
        this.last_pan = KInputManager.GetMousePos();
      }
    }
    if (Input.GetMouseButtonUp(0))
      this.do_pan = false;
    float axis = Input.GetAxis("Mouse ScrollWheel");
    if ((double) axis == 0.0)
      return;
    this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView - axis * BatchAnimCamera.zoom_speed, this.zoom_min, this.zoom_max);
  }

  private void ClampToBounds()
  {
    this.transform.SetPosition(this.transform.GetPosition() with
    {
      x = Mathf.Clamp(this.transform.GetPosition().x, BatchAnimCamera.bounds.min.x, BatchAnimCamera.bounds.max.x),
      y = Mathf.Clamp(this.transform.GetPosition().y, BatchAnimCamera.bounds.min.y, BatchAnimCamera.bounds.max.y),
      z = Mathf.Clamp(this.transform.GetPosition().z, BatchAnimCamera.bounds.min.z, BatchAnimCamera.bounds.max.z)
    });
  }

  private void OnDrawGizmosSelected()
  {
    DebugExtension.DebugBounds(BatchAnimCamera.bounds, Color.red);
  }
}
