using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using Adverty;

namespace MondayOFF {
    public class PackageExporter {
        [MenuItem("Everyday/Export Package", false, 900)]
        public static void ExplortEverydayPackage() {
            var exportPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"Builds/Everyday_v{EveryDay.Version}.unitypackage");

            if (!EditorUtility.DisplayDialog("Export Path", $"{exportPath}", "Ok", "Cancel")) {
                return;
            }

            CleanUpSetttings();

            var projectPath = Path.GetDirectoryName(Application.dataPath);
            File.Copy(Path.Combine(projectPath, "ReadMe.md"), "Assets/MondayOFF/ReadMe.md");
            File.Copy(Path.Combine(projectPath, "Changelog.md"), "Assets/MondayOFF/Changelog.md");

            AssetDatabase.Refresh();

            var exportedPackageAssetList = new List<string>();
            var assetGUIDs = AssetDatabase.FindAssets("a:assets").ToList();
            foreach (var item in assetGUIDs) {
                exportedPackageAssetList.Add(AssetDatabase.GUIDToAssetPath(item));
            }

            exportedPackageAssetList.Remove("Assets/MondayOFF/Everyday/Core/Editor/PackageExporter.cs");
            exportedPackageAssetList.Remove("Assets/Plugins/Android/AndroidManifest.xml");

            Debug.Log("[EVERYDAY] Exporting package to: " + exportPath);

            AssetDatabase.ExportPackage(
                exportedPackageAssetList.ToArray(),
                exportPath,
                ExportPackageOptions.Interactive
            );
        }
        [MenuItem("Everyday/Export Package For Publishing", false, 900)]
        public static void ExplortEverydayPackageForPublishing() {
            var exportPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"Builds/Everyday_Pub_{DateTime.Now.ToString("yyMMdd")}.unitypackage");

            if (!EditorUtility.DisplayDialog("Export Path", $"{exportPath}", "Ok", "Cancel")) {
                return;
            }

            CleanUpSetttings();

            string[] ExcludedAssets = new string[]{
                // Firebase
                "Assets/Editor Default Resources",
                "Assets/Firebase",
                "Assets/Parse",
                "Assets/Plugins/iOS/Firebase",

                // NiceVibrations
                "Assets/MondayOFF/ThirdParties/NiceVibrations",

                // AdMob Adapter
                "Assets/MaxSdk/Mediation/Google",
                "Assets/Plugins/Android/MaxMediationGoogle.androidlib",


            };



        }

        private static void CleanUpSetttings() {
            // Everyday Settings
            EverydaySettings settings = default;
            var assets = Resources.LoadAll<EverydaySettings>("EverydaySettings");
            if (assets == null || assets.Length <= 0) {
                Debug.Log("NOT found, search all");
                assets = Resources.LoadAll<EverydaySettings>("");
            }
            if (assets.Length != 1) {
                Debug.LogError($"[EVERYDAY] Found 0 or multiple {typeof(EverydaySettings).Name}s in Resources folder. There should only be one.");
            } else {
                settings = assets[0];
            }
            Debug.Assert(settings != null, "NO SETTINGS FOUND");

            settings.adSettings = new AdSettings();

            Facebook.Unity.Settings.FacebookSettings.AppIds = new List<string>() { "0" };
            Facebook.Unity.Settings.FacebookSettings.AppLabels = new List<string>() { "App Name" };
            Facebook.Unity.Settings.FacebookSettings.ClientTokens = new List<string>() { "" };

            EditorUtility.SetDirty(settings);

            AdvertySettings.ClearAPIKey();
            AdvertySettings.SandboxMode = false;

            File.Delete("Assets/Plugins/Android/AndroidManifest.xml");
        }
    }
}