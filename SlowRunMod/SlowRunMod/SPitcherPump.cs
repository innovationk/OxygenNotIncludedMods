using System.Collections.Generic;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// Runtime logic: keeps the storage full of the liquid selected in the
    /// side screen. Changing the selection empties the old liquid.
    /// </summary>
    public class SPitcherPump : KMonoBehaviour, ISim200ms
    {
#pragma warning disable 649
        [MyCmpReq] private Storage storage;
        [MyCmpReq] private Filterable filterable;
#pragma warning restore 649

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (!IsValidLiquid(filterable.SelectedTag))
                filterable.SelectedTag = SPitcherPumpConfig.DEFAULT_LIQUID.CreateTag();

            filterable.onFilterChanged += OnFilterChanged;
            Refill();
        }

        protected override void OnCleanUp()
        {
            if (filterable != null)
                filterable.onFilterChanged -= OnFilterChanged;
            base.OnCleanUp();
        }

        public void Sim200ms(float dt) => Refill();

        private void OnFilterChanged(Tag tag)
        {
            // Delete whatever liquid isn't the new selection, then refill.
            var toRemove = new List<GameObject>();
            foreach (GameObject item in storage.items)
                if (item != null && item.PrefabID() != tag)
                    toRemove.Add(item);

            foreach (GameObject item in toRemove)
            {
                storage.Remove(item);
                Util.KDestroyGameObject(item);
            }
            Refill();
        }

        private void Refill()
        {
            Tag tag = filterable.SelectedTag;
            if (!IsValidLiquid(tag)) return;

            float missing = storage.capacityKg - storage.MassStored();
            if (missing <= 0.01f) return;

            Element element = ElementLoader.GetElement(tag);
            storage.AddLiquid(element.id, missing, LiquidTemperature(element),
                              byte.MaxValue, 0);   // no germs
        }

        private static bool IsValidLiquid(Tag tag)
        {
            if (!tag.IsValid) return false;
            Element element = ElementLoader.GetElement(tag);
            return element != null && element.IsLiquid;
        }

        /// <summary>20 °C when the liquid can exist there, else mid-range.</summary>
        private static float LiquidTemperature(Element element)
        {
            float t = SPitcherPumpConfig.PREFERRED_TEMP_K;
            if (t > element.lowTemp + 2f && t < element.highTemp - 2f)
                return t;
            return (element.lowTemp + element.highTemp) / 2f;
        }
    }
}
