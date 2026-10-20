using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using AnatomySandbox.Setup;

namespace AnatomySandbox.Editor
{
    /// <summary>
    /// Creates a minimal playable startup scene and builds the Android APK in CI.
    /// Called by GitHub Actions through Unity's -executeMethod option.
    /// </summary>
    public static class AndroidBuildSetup
    {
        private const string ScenePath = "Assets/AnatomySandbox/Scenes/MainLab.unity";
        private const string OutputPath = "Builds/Android/AnatomySandbox.apk";
        private const string PackageName = "com.intersprit.anatomysandbox";

        public static void BuildAndroid()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
                Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));

                ConfigurePlayerSettings();
                CreateStartupScene();

                EditorBuildSettings.scenes = new[]
                {
                    new EditorBuildSettingsScene(ScenePath, true)
                };

                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

                BuildPlayerOptions options = new BuildPlayerOptions
                {
                    scenes = new[] { ScenePath },
                    locationPathName = OutputPath,
                    target = BuildTarget.Android,
                    options = BuildOptions.None
                };

                BuildReport report = BuildPipeline.BuildPlayer(options);
                if (report.summary.result != BuildResult.Succeeded)
                {
                    throw new Exception("Android build failed: " + report.summary.result +
                        " (" + report.summary.totalErrors + " errors). See Unity build log.");
                }

                Debug.Log("ANATOMY_SANDBOX_APK_PATH=" + Path.GetFullPath(OutputPath));
                Debug.Log("Android APK build succeeded. Size: " + report.summary.totalSize + " bytes.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                throw;
            }
        }

        private static void ConfigurePlayerSettings()
        {
            PlayerSettings.productName = "Anatomy Sandbox";
            PlayerSettings.companyName = "Intersprit";
            PlayerSettings.applicationIdentifier = PackageName;
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            PlayerSettings.stripEngineCode = true;
            PlayerSettings.runInBackground = false;

#pragma warning disable CS0618
            // The existing orbit controller uses UnityEngine.Input (legacy input API).
            PlayerSettings.activeInputHandler = 0;
#pragma warning restore CS0618
        }

        private static void CreateStartupScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject bootstrap = new GameObject("Anatomy Sandbox");
            bootstrap.AddComponent<ProceduralAnatomyBuilder>();

            // A small, non-interactive title canvas gives the first build a clear app identity.
            GameObject canvasObject = new GameObject("Startup UI", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);

            GameObject titleObject = new GameObject("Title", typeof(RectTransform), typeof(UnityEngine.UI.Text));
            titleObject.transform.SetParent(canvasObject.transform, false);
            RectTransform rect = titleObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.08f, 0.90f);
            rect.anchorMax = new Vector2(0.92f, 0.98f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            UnityEngine.UI.Text title = titleObject.GetComponent<UnityEngine.UI.Text>();
            title.text = "ANATOMY SANDBOX";
            title.alignment = TextAnchor.MiddleCenter;
            title.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            title.fontSize = 38;
            title.fontStyle = FontStyle.Bold;
            title.color = Color.white;
            title.horizontalOverflow = HorizontalWrapMode.Overflow;
            title.verticalOverflow = VerticalWrapMode.Overflow;

            EditorSceneManager.SaveScene(scene, ScenePath);
        }
    }
}
