using UnityEngine;

namespace MondayOFF {
    internal sealed class PlayOn : AdTypeBase {
        private AdUnit _adUnit = new AdUnit(PlayOnSDK.AdUnitType.AudioLogoAd);

        public override void Dispose() {
            _adUnit.Dispose();
            _adUnit = null;
        }

        internal override bool IsReady() {
            if (!PlayOnSDK.IsInitialized()) {
                Debug.Log("[EVERYDAY] PlayOn is not initialized");
                return false;
            }

            return true;
        }

        internal override bool Show() {
            if (!_adUnit.IsAdAvailable()) {
                Debug.Log("[EVERYDAY] PlayOn Ad Unit is not available yet");
                return false;
            }
            Debug.Log("[EVERYDAY] Show PlayOn");
            _adUnit.ShowAd();
            return true;
        }

        internal void Hide() {
            Debug.Log("[EVERYDAY] Hide PlayON");
            _adUnit.CloseAd();
        }

        internal void LinkLogoToRectTransform(PlayOnSDK.Position position, RectTransform rectTransform, Canvas canvas) {
            if (_settings.legacyPlayOn.useLegacyPlayOnPositioning) {
                Debug.Log("[EVERYDAY] Using legacy PlayOn positioning! Do not use PlayOnAnchor");
                return;
            }
            _adUnit.LinkLogoToRectTransform(position, rectTransform, canvas);
        }

        // internal void LinkLogoToPrefab(in AdUnitAnchor adUnitAnchor) {
        //     _adUnit.LinkLogoToPrefab(adUnitAnchor);
        // }

        internal PlayOn(in AdSettings settings) : base(settings) {
            if (settings.legacyPlayOn.useLegacyPlayOnPositioning) {
                _adUnit.SetLogo(settings.legacyPlayOn.playOnLogoAnchor, settings.legacyPlayOn.playOnLogoOffset.x, settings.legacyPlayOn.playOnLogoOffset.y, settings.legacyPlayOn.playOnLogoSize);
            }
            // _adUnit.SetLogo(PlayOnSDK.Position.TopLeft, 0, 150, 64);

            PlayOnSDK.OnInitializationFinished += OnInitialized;

            PlayOnSDK.Initialize(settings.playOnAPIKey, settings.storeID);
        }

        private void OnInitialized() {
            PlayOnSDK.OnInitializationFinished -= OnInitialized;

            var mainThreadDispatcherInstance = UnityMainThreadDispatcher.Instance();
            mainThreadDispatcherInstance.gameObject.AddComponent<ApplicationLifecycleTracker>();

            PlayOnSDK.SetGdprConsent(AdsManager.HAS_USER_CONSENT);
            PlayOnSDK.SetDoNotSell(!AdsManager.HAS_USER_CONSENT, AdsManager.US_PRIVACY_STRING);
            PlayOnSDK.SetIsChildDirected(false);
        }

        private class ApplicationLifecycleTracker : MonoBehaviour {
            private void OnApplicationPause(bool pauseStatus) {
                Debug.Log($"[EVERYDAY] PlayON OnApplicationPause({pauseStatus})");
                PlayOnSDK.onApplicationPause(pauseStatus);
            }
        }
    }
}