using UnityEngine;

namespace AnatomySandbox.Animation
{
    /// <summary>
    /// Subtle pulse/glow animation for a selected organ mesh.
    /// Assign a renderer with a material that supports emission for the glow effect.
    /// </summary>
    public sealed class OrganPulse : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField, Min(0.1f)] private float healthyBeatsPerMinute = 72f;
        [SerializeField, Min(0f)] private float pulseAmount = 0.08f;
        [SerializeField] private Color healthyEmission = new Color(0.05f, 0.3f, 0.18f);
        [SerializeField] private Color alertEmission = new Color(0.55f, 0.06f, 0.025f);

        private Material runtimeMaterial;
        private Vector3 initialScale;
        private AnatomySandbox.Simulation.AnatomySimulationController simulation;

        private void Awake()
        {
            initialScale = transform.localScale;
            if (targetRenderer != null)
                runtimeMaterial = targetRenderer.material;
            simulation = FindFirstObjectByType<AnatomySandbox.Simulation.AnatomySimulationController>();
        }

        private void Update()
        {
            var vitals = simulation != null
                ? simulation.Current
                : AnatomySandbox.Simulation.VitalSigns.HealthyBaseline;

            float bpm = Mathf.Max(1f, vitals.heartRateBpm);
            float beat = Mathf.Max(0f, Mathf.Sin(Time.time * bpm * Mathf.PI / 30f));
            transform.localScale = initialScale * (1f + beat * pulseAmount);

            if (runtimeMaterial != null && runtimeMaterial.HasProperty("_EmissionColor"))
            {
                float alert = Mathf.InverseLerp(100f, 78f, vitals.oxygenPercent);
                runtimeMaterial.SetColor("_EmissionColor", Color.Lerp(healthyEmission, alertEmission, alert));
            }
        }

        private void OnDestroy()
        {
            if (runtimeMaterial != null)
                Destroy(runtimeMaterial);
        }
    }
}
