using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// Runtime logic: switches the ElementConverter on/off.
    /// Runs when the building is enabled (not disabled by the player), the top
    /// tile is not overpressurized, and the internal water buffer is not full.
    /// </summary>
    public class SAlgaeTerrarium : KMonoBehaviour, ISim1000ms
    {
#pragma warning disable 649
        [MyCmpReq] private Operational operational;
        [MyCmpReq] private Storage storage;
        [MyCmpGet] private KBatchedAnimController anim;
#pragma warning restore 649

        private bool wasActive;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            UpdateState();
        }

        public void Sim1000ms(float dt) => UpdateState();

        private void UpdateState()
        {
            bool canRun = operational.IsOperational
                          && !IsOverPressure()
                          && storage.RemainingCapacity() > 0.5f;

            operational.SetActive(canRun);

            if (anim != null && canRun != wasActive)
                anim.Play(canRun ? "on" : "off", KAnim.PlayMode.Loop);
            wasActive = canRun;
        }

        private bool IsOverPressure()
        {
            // Oxygen is emitted on the upper tile of the 1x2 building.
            int cell = Grid.CellAbove(Grid.PosToCell(this));
            if (!Grid.IsValidCell(cell)) return true;
            return Grid.Mass[cell] >= SAlgaeTerrariumConfig.MAX_GAS_PRESSURE_KG
                   && Grid.Element[cell].IsGas;
        }
    }
}
