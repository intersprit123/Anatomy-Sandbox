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
    /// Keep this component and its collider active so the part can be restored.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class BodyPartInteractable : MonoBehaviour
    {
        [SerializeField] private string displayName = "Body part";
        [SerializeField] private BodySystem system = BodySystem.Body;
        [SerializeField] private bool removable = true;
        [Tooltip("Optional child containing only the visible meshes. Leave empty to use renderers below this object.")]
        [SerializeField] private Transform visualRoot;

        public string DisplayName => displayName;
        public BodySystem System => system;
        public bool IsRemoved { get; private set; }

        public event System.Action<BodyPartInteractable> Selected;
        private AnatomySimulationController simulation;
        private Renderer[] renderers;

        private void Awake()
        {
            Transform root = visualRoot != null ? visualRoot : transform;
            renderers = root.GetComponentsInChildren<Renderer>(true);
            simulation = FindFirstObjectByType<AnatomySimulationController>();
        }

        private void OnMouseDown()
        {
            // Unity sends OnMouseDown for touch taps on a collider as well as mouse clicks.
            Selected?.Invoke(this);
        }

        public void RemoveOrRestore()
        {
            if (!removable) return;
            SetRemoved(!IsRemoved);
        }

        public void Restore()
        {
            if (IsRemoved) SetRemoved(false);
        }

        private void SetRemoved(bool removed)
        {
            IsRemoved = removed;

            // Hide meshes instead of disabling this GameObject, so the selection collider
            // and this script remain available to restore the part.
            if (renderers != null)
            {
                foreach (Renderer meshRenderer in renderers)
                {
                    if (meshRenderer != null) meshRenderer.enabled = !removed;
                }
            }

            if (simulation != null)
                simulation.SetSystemDisrupted(system.ToString(), removed);
        }
    }
}
