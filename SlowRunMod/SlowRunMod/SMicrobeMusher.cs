using KSerialization;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// Runtime logic: every SECONDS_PER_OMELETTE (while enabled and below the
    /// world cap) spawns Omelettes on the building's bottom-left tile.
    /// The progress timer is saved with the game.
    /// </summary>
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SMicrobeMusher : KMonoBehaviour, ISim1000ms
    {
#pragma warning disable 649
        [MyCmpReq] private Operational operational;
        [MyCmpGet] private KBatchedAnimController anim;
#pragma warning restore 649

        [Serialize] private float elapsed;

        private bool wasActive;

        private static Tag OmeletteTag => CookedEggConfig.ID.ToTag();

        protected override void OnSpawn()
        {
            base.OnSpawn();
            wasActive = !operational.IsActive; // force first anim refresh
        }

        public void Sim1000ms(float dt)
        {
            bool canRun = operational.IsOperational && !WorldIsFull();
            operational.SetActive(canRun);

            if (anim != null && canRun != wasActive)
                anim.Play(canRun ? "working_loop" : "off", KAnim.PlayMode.Loop);
            wasActive = canRun;

            if (!canRun) return;

            elapsed += dt;
            if (elapsed < SMicrobeMusherConfig.SECONDS_PER_OMELETTE) return;
            elapsed -= SMicrobeMusherConfig.SECONDS_PER_OMELETTE;

            SpawnOmelette();
        }

        private bool WorldIsFull()
        {
            float cap = SMicrobeMusherConfig.MAX_OMELETTES_IN_WORLD;
            if (cap <= 0f) return false;

            WorldContainer world = this.GetMyWorld();
            if (world == null || world.worldInventory == null) return false;

            // Food mass in kg == units for Omelettes (1 unit = 1 kg).
            return world.worldInventory.GetAmount(OmeletteTag, false) >= cap;
        }

        private void SpawnOmelette()
        {
            GameObject prefab = Assets.GetPrefab(OmeletteTag);
            if (prefab == null)
            {
                Debug.LogWarning("[SMicrobeMusher] Omelette prefab not found: " + CookedEggConfig.ID);
                return;
            }

            Vector3 pos = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
            GameObject food = GameUtil.KInstantiate(prefab, pos, Grid.SceneLayer.Ore);

            PrimaryElement pe = food.GetComponent<PrimaryElement>();
            pe.Units = SMicrobeMusherConfig.OMELETTES_PER_BATCH;
            pe.Temperature = SMicrobeMusherConfig.FOOD_TEMP_K;

            food.SetActive(true);
            if (PopFXManager.Instance != null)
                PopFXManager.Instance.SpawnFX(
                    PopFXManager.Instance.sprite_Resource, "+1 Omelette", food.transform);
        }
    }
}
