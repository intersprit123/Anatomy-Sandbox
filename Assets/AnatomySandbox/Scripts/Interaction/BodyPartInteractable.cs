using UnityEngine;
using AnatomySandbox.Simulation;

namespace AnatomySandbox.Interaction
{
    public enum BodySystem
    {
        Body,
        Skeleton,
        Muscles,
        Respiratory,
        Circulatory,
        Digestive,
        Nervous
    }

    /// <summary>
    /// Add to a collider on an organ/body-part object to support selection and toy removal.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class BodyPartInteractable : MonoBehaviour
    {
        [SerializeField] private string displayName = "Body part";
        [SerializeField] private BodySystem system = BodySystem.Body;
        [SerializeField] private bool removable = true;
        [SerializeField] private GameObject visualRoot;

        public string DisplayName => displayName;
        public BodySystem System => system;
        public bool IsRemoved { get; private set; }

        public event System.Action<BodyPartInteractable> Selected;
        private AnatomySimulationController simulation;

        private void Awake()
        {
            if (visualRoot == null) visualRoot = gameObject;
            simulation = FindFirstObjectByType<AnatomySimulationController>();
        }

        private void OnMouseDown()
        {
            // Unity forwards touch taps to OnMouseDown when a collider is hit.
            Selected?.Invoke(this);
        }

        public void RemoveOrRestore()
        {
            if (!removable) return;
            IsRemoved = !IsRemoved;
            if (visualRoot != null) visualRoot.SetActive(!IsRemoved);

            if (simulation != null)
            {
                string key = system.ToString();
                simulation.SetSystemDisrupted(key, IsRemoved);
            }
        }

        public void Restore()
        {
            if (!IsRemoved) return;
            IsRemoved = false;
            if (visualRoot != null) visualRoot.SetActive(true);
            if (simulation != null) simulation.SetSystemDisrupted(system.ToString(), false);
        }
    }
}
