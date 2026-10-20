using System;
using UnityEngine;

namespace AnatomySandbox.Simulation
{
    [Serializable]
    public struct VitalSigns
    {
        [Range(0f, 100f)] public float oxygenPercent;
        [Range(30f, 200f)] public float heartRateBpm;
        [Range(4f, 50f)] public float breathsPerMinute;
        [Range(40f, 220f)] public float systolicPressure;
        [Range(20f, 140f)] public float diastolicPressure;
        [Range(0f, 100f)] public float circulationPercent;
        [Range(0f, 100f)] public float hydrationPercent;

        public static VitalSigns HealthyBaseline => new VitalSigns
        {
            oxygenPercent = 98f,
            heartRateBpm = 72f,
            breathsPerMinute = 14f,
            systolicPressure = 120f,
            diastolicPressure = 80f,
            circulationPercent = 100f,
            hydrationPercent = 100f
        };
    }
}
