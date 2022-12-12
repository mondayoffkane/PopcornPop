using UnityEngine;
using Adverty;
using MondayOFF;

public class EverydayAdTestScript : MonoBehaviour {
    [SerializeField] GameObject _bigBadCanvas = default;
    [SerializeField] GameObject _IAPGameObject = default;
    [SerializeField] RectTransform _playonAdUnitAnchor = default;

#if UNITY_EDITOR
    private void OnValidate() {
        var isIAPactive =
#if UNITY_PURCHASING
        true
#else
        false
#endif
;
        _IAPGameObject.SetActive(isIAPactive);
    }
#endif

    private void Start() {
        AdsManager.OnRewardedAdLoaded -= OnRewarededReady;
        AdsManager.OnRewardedAdLoaded += OnRewarededReady;

        AdsManager.OnBeforeInterstitial -= ShowBadCanvas;
        AdsManager.OnBeforeInterstitial += ShowBadCanvas;

        AdsManager.OnAfterInterstitial -= HideBadCanvas;
        AdsManager.OnAfterInterstitial += HideBadCanvas;


        AdsManager.OnBeforeRewarded -= ShowBadCanvas;
        AdsManager.OnBeforeRewarded += ShowBadCanvas;

        AdsManager.OnAfterRewarded -= HideBadCanvas;
        AdsManager.OnAfterRewarded += HideBadCanvas;

#if UNITY_PURCHASING
        IAPManager.OnBeforePurchase -= ShowBadCanvas;
        IAPManager.OnBeforePurchase += ShowBadCanvas;

        IAPManager.OnAfterPurchase -= HideBadCanvas;
        IAPManager.OnAfterPurchase += HideBadCanvas;
#endif

        // UserData userData = new UserData(AgeSegment.Unknown, Gender.Unknown);
        // Adverty.AdvertySDK.Init(userData);

        Adverty.AdvertySettings.SetMainCamera(Camera.main);
    }

    private void ShowBadCanvas() {
        Debug.Log("SHOW LOADING CANVAS");
        _bigBadCanvas.SetActive(true);
    }

    private void HideBadCanvas() {
        Debug.Log("HIDE LOADING CANVAS");
        _bigBadCanvas.SetActive(false);
    }

    private void HideBadCanvas(bool isSuccess) {
#if UNITY_PURCHASING
        string message = default;
        if (isSuccess) {
            message = "blue>Successful";
        } else {
            message = "red>Failed";
        }
        Debug.Log($"Purcahse <color={message}</color>");
#endif
        Debug.Log("HIDE LOADING CANVAS");
        _bigBadCanvas.SetActive(false);
    }

    private void OnRewarededReady() {
        Debug.Log("REWARED IS READY");
    }

    public void Ads_InitializeAdsManager() {
        AdsManager.Initialize();
    }

    public void Ads_ShowIS() {
        AdsManager.ShowInterstitial();
    }

    public void Ads_ShowRV() {
        AdsManager.ShowRewarded(() => Debug.Log("-- Your Reward --"));
    }

    public void Ads_ShowBN() {
        AdsManager.ShowBanner();
    }

    public void Ads_HideBN() {
        AdsManager.HideBanner();
    }

    public void Ads_SetPlayOnPos() {
        AdsManager.LinkLogoToRectTransform(PlayOnSDK.Position.Centered, _playonAdUnitAnchor, _playonAdUnitAnchor.GetComponentInParent<Canvas>());
    }

    public void Ads_ShowPlayOn() {
        AdsManager.ShowPlayOn();
    }

    public void Ads_HidePlayOn() {
        AdsManager.HidePlayOn();
    }

    public void Ads_DisableIS() {
        // Either method works. DisableAdType(AdType) is useful when disabling multiple ad types.
        // AdsManager.DisableIS();
        AdsManager.DisableAdType(AdType.Interstitial);
    }

    public void Ads_DisableRV() {
        // AdsManager.DisableRV();
        AdsManager.DisableAdType(AdType.Rewarded);
    }

    public void Ads_DisableBN() {
        // AdsManager.DisableBN();
        AdsManager.DisableAdType(AdType.Banner);
    }

    public void IAP_InitializeIAPManager() {
#if UNITY_PURCHASING
        IAPManager.Initialze();
#else
        Debug.Log("[EVERYDAY] UNITY_PURCHASING is not enabled");
#endif
    }

    public void IAP_RegisterCallbackConsumable() {
#if UNITY_PURCHASING
        IAPManager.RegisterProduct("Consumable", () => {
            Debug.Log("[EVERYDAY] Purchased Consumable");
        });
#else
        Debug.Log("[EVERYDAY] UNITY_PURCHASING is not enabled");
#endif
    }

    public void IAP_PurchaseConsumable() {
#if UNITY_PURCHASING
        IAPManager.PurchaseProduct("Consumable");
#else
        Debug.Log("[EVERYDAY] UNITY_PURCHASING is not enabled");
#endif
    }

    public void IAP_RegisterCallbackNonConsumable() {
#if UNITY_PURCHASING
        IAPManager.RegisterProduct("Non Consumable", () => {
            Debug.Log("[EVERYDAY] Purchased Non Consumable");
            PlayerPrefs.SetInt("Non Consumable", 1);
            PlayerPrefs.Save();
        });
#else
        Debug.Log("[EVERYDAY] UNITY_PURCHASING is not enabled");
#endif
    }

    public void IAP_PurchaseNonConsumable() {
#if UNITY_PURCHASING
        if (PlayerPrefs.GetInt("Non Consumable", 0) > 0) {
            Debug.Log("[EVERYDAY] Non Consumable is already purchased! Do you really want to re-purchase it?");
            return;
        }

        IAPManager.PurchaseProduct("Non Consumable");
#else
        Debug.Log("[EVERYDAY] UNITY_PURCHASING is not enabled");
#endif
    }

    public void IAP_RestorePurchase() {
#if UNITY_PURCHASING
        IAPManager.RestorePurchase();
#else
        Debug.Log("[EVERYDAY] UNITY_PURCHASING is not enabled");
#endif
    }

    public void Events_TryStage(int stageNum) {
        EventTracker.TryStage(stageNum);
    }

    public void Events_ClearStage(int stageNum) {
        EventTracker.ClearStage(stageNum);
    }
}
