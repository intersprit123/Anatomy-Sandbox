using UnityEngine;
using AnatomySandbox.Simulation;

namespace AnatomySandbox.Animation
{
    /// <summary>
    /// Adds subtle breathing motion to a chest mesh or chest parent transform.
    /// Use a dedicated chest transform, not the root of the whole character.
    /// </summary>
    public sealed class ChestBreathing : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float expansionAmount = 0.025f;
        [SerializeField, Min(0.1f)] private float inhaleExhaleSmoothness = 5f;

        private Vector3 initialScale;
        private AnatomySimulationController simulation;

        private void Awake()
        {
            initialScale = transform.localScale;
            simulation = FindFirstObjectByType<AnatomySimulationController>();
        }

        private void Update()
        {
            float breathsPerMinute = simulation != null
                ? simulation.Current.breathsPerMinute
                : VitalSigns.HealthyBaseline.breathsPerMinute;

            float cycleSeconds = 60f / Mathf.Max(1f, breathsPerMinute);
            float phase = (Time.time % cycleSeconds) / cycleSeconds;
            // Smooth inhale for the first half, exhale for the second half.
            float breathing = 0.5f - 0.5f * Mathf.Cos(phase * Mathf.PI * 2f);
            Vector3 targetScale = initialScale;
            targetScale.x *= 1f + breathing * expansionAmount;
            targetScale.z *= 1f + breathing * expansionAmount;
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                targetScale,
                Time.deltaTime * inhaleExhaleSmoothness);
        }
    }
}
