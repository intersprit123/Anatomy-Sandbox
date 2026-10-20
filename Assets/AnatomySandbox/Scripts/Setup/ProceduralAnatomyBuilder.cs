using UnityEngine;
using AnatomySandbox.Interaction;

namespace AnatomySandbox.Setup
{
    /// <summary>
    /// Creates a clean, stylized anatomy mannequin from primitive meshes at runtime.
    /// This is a placeholder asset builder until detailed licensed anatomy meshes are imported.
    /// Add to an empty GameObject in a Unity scene and press Play.
    /// </summary>
    public sealed class ProceduralAnatomyBuilder : MonoBehaviour
    {
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private bool includeInternalOrgans = true;
        [SerializeField] private bool createCameraAndLighting = true;

        private Transform anatomyRoot;

        private void Start()
        {
            if (buildOnStart) Build();
        }

        [ContextMenu("Build Anatomy Model")]
        public void Build()
        {
            Transform existing = transform.Find("GeneratedAnatomy");
            if (existing != null)
            {
                if (Application.isPlaying) Destroy(existing.gameObject);
                else DestroyImmediate(existing.gameObject);
            }

            GameObject root = new GameObject("GeneratedAnatomy");
            root.transform.SetParent(transform, false);
            anatomyRoot = root.transform;
            anatomyRoot.gameObject.AddComponent<ModelOrbitController>();
            anatomyRoot.gameObject.AddComponent<AnatomySandbox.Simulation.AnatomySimulationController>();

            Material skin = MakeMaterial("Warm skin", new Color(0.72f, 0.43f, 0.34f));
            Material muscle = MakeMaterial("Muscle red", new Color(0.62f, 0.12f, 0.19f));
            Material bone = MakeMaterial("Ivory bone", new Color(0.88f, 0.82f, 0.68f));
            Material organ = MakeMaterial("Organ coral", new Color(0.78f, 0.19f, 0.24f));
            Material blue = MakeMaterial("Vein blue", new Color(0.12f, 0.36f, 0.75f));
            Material dark = MakeMaterial("Dark detail", new Color(0.10f, 0.12f, 0.16f));

            // Human silhouette, approximately 1.8 Unity units tall.
            Part("Head", PrimitiveType.Sphere, new Vector3(0f, 1.58f, 0f), new Vector3(0.28f, 0.31f, 0.25f), skin, BodySystem.Body);
            Part("Neck", PrimitiveType.Cylinder, new Vector3(0f, 1.27f, 0f), new Vector3(0.105f, 0.16f, 0.105f), skin, BodySystem.Body);
            Part("Torso", PrimitiveType.Capsule, new Vector3(0f, 0.82f, 0f), new Vector3(0.48f, 0.56f, 0.27f), skin, BodySystem.Body);
            Part("Pelvis", PrimitiveType.Sphere, new Vector3(0f, 0.28f, 0f), new Vector3(0.35f, 0.24f, 0.24f), skin, BodySystem.Body);

            // Arms are grouped as upper arm and forearm parts.
            for (int side = -1; side <= 1; side += 2)
            {
                float s = side;
                Part(side < 0 ? "Left upper arm" : "Right upper arm", PrimitiveType.Capsule,
                    new Vector3(s * 0.58f, 0.91f, 0f), new Vector3(0.13f, 0.34f, 0.13f), skin, BodySystem.Body,
                    new Vector3(0f, 0f, -s * 12f));
                Part(side < 0 ? "Left forearm" : "Right forearm", PrimitiveType.Capsule,
                    new Vector3(s * 0.72f, 0.43f, 0f), new Vector3(0.105f, 0.30f, 0.105f), skin, BodySystem.Body,
                    new Vector3(0f, 0f, -s * 7f));
                Part(side < 0 ? "Left hand" : "Right hand", PrimitiveType.Sphere,
                    new Vector3(s * 0.79f, 0.10f, 0f), new Vector3(0.12f, 0.16f, 0.07f), skin, BodySystem.Body);
                Part(side < 0 ? "Left thigh" : "Right thigh", PrimitiveType.Capsule,
                    new Vector3(s * 0.19f, -0.13f, 0f), new Vector3(0.16f, 0.39f, 0.17f), skin, BodySystem.Body);
                Part(side < 0 ? "Left lower leg" : "Right lower leg", PrimitiveType.Capsule,
                    new Vector3(s * 0.19f, -0.67f, 0f), new Vector3(0.12f, 0.34f, 0.13f), skin, BodySystem.Body);
                Part(side < 0 ? "Left foot" : "Right foot", PrimitiveType.Capsule,
                    new Vector3(s * 0.19f, -0.99f, 0.075f), new Vector3(0.12f, 0.17f, 0.22f), skin, BodySystem.Body,
                    new Vector3(90f, 0f, 0f));
            }

            // Front-facing simplified skeletal landmarks, visible through the torso.
            Part("Spine", PrimitiveType.Capsule, new Vector3(0f, 0.78f, -0.20f), new Vector3(0.055f, 0.47f, 0.055f), bone, BodySystem.Skeleton);
            Part("Sternum", PrimitiveType.Capsule, new Vector3(0f, 0.98f, -0.265f), new Vector3(0.045f, 0.20f, 0.035f), bone, BodySystem.Skeleton);

            // Organs sit slightly forward so they are visible when the torso is selected/removed.
            if (includeInternalOrgans)
            {
                Part("Left lung", PrimitiveType.Sphere, new Vector3(-0.15f, 0.99f, -0.285f), new Vector3(0.14f, 0.22f, 0.07f), muscle, BodySystem.Respiratory);
                Part("Right lung", PrimitiveType.Sphere, new Vector3(0.15f, 0.99f, -0.285f), new Vector3(0.14f, 0.22f, 0.07f), muscle, BodySystem.Respiratory);
                Part("Heart", PrimitiveType.Sphere, new Vector3(0.035f, 0.88f, -0.36f), new Vector3(0.105f, 0.12f, 0.09f), organ, BodySystem.Circulatory);
                Part("Liver", PrimitiveType.Sphere, new Vector3(0.13f, 0.65f, -0.29f), new Vector3(0.19f, 0.09f, 0.07f), organ, BodySystem.Digestive);
                Part("Stomach", PrimitiveType.Sphere, new Vector3(-0.12f, 0.62f, -0.30f), new Vector3(0.12f, 0.12f, 0.07f), organ, BodySystem.Digestive);
                Part("Kidney left", PrimitiveType.Sphere, new Vector3(-0.20f, 0.48f, -0.25f), new Vector3(0.07f, 0.10f, 0.05f), blue, BodySystem.Digestive);
                Part("Kidney right", PrimitiveType.Sphere, new Vector3(0.20f, 0.48f, -0.25f), new Vector3(0.07f, 0.10f, 0.05f), blue, BodySystem.Digestive);
            }

            if (createCameraAndLighting) EnsureCameraAndLight();
        }

        private GameObject Part(string partName, PrimitiveType shape, Vector3 position, Vector3 scale, Material material, BodySystem system, Vector3? rotation = null)
        {
            GameObject part = GameObject.CreatePrimitive(shape);
            part.name = partName;
            part.transform.SetParent(anatomyRoot, false);
            part.transform.localPosition = position;
            part.transform.localScale = scale;
            if (rotation.HasValue) part.transform.localEulerAngles = rotation.Value;

            Renderer renderer = part.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            BodyPartInteractable interactable = part.AddComponent<BodyPartInteractable>();
            // Configure system through serialized defaults is not possible at runtime; use the
            // public setup helper added to BodyPartInteractable below when available.
            interactable.Configure(partName, system, true);
            return part;
        }

        private Material MakeMaterial(string materialName, Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            Material material = new Material(shader);
            material.name = materialName;
            material.color = color;
            return material;
        }

        private void EnsureCameraAndLight()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                GameObject cameraObject = new GameObject("Main Camera");
                cameraObject.tag = "MainCamera";
                cam = cameraObject.AddComponent<Camera>();
            }

            cam.transform.position = new Vector3(0f, 0.42f, -4.1f);
            cam.transform.LookAt(new Vector3(0f, 0.35f, 0f));
            cam.fieldOfView = 38f;
            cam.backgroundColor = new Color(0.025f, 0.04f, 0.05f);
            cam.clearFlags = CameraClearFlags.SolidColor;

            if (FindFirstObjectByType<Light>() == null)
            {
                GameObject lightObject = new GameObject("Anatomy Key Light");
                Light light = lightObject.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.25f;
                lightObject.transform.rotation = Quaternion.Euler(35f, -25f, 0f);
            }
        }
    }
}
