using UnityEngine;

namespace MondayOFF {
    internal class EverydaySettings : ScriptableObject {
        [Header("Enable verbose logging")]
        [SerializeField] internal bool verboseLogging = false;

        [Header("Ad Settings")]
        [SerializeField] internal AdSettings adSettings = default;
    }
}