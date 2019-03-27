using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Admob : MonoBehaviour {

    private BannerView bannerView;
    private InterstitialAd interstitial;

    public void Start()
    {
#if UNITY_ANDROID
            string appId = "ca-app-pub-2260202932541442~4375086794";
#elif UNITY_IPHONE
        string appId = "ca-app-pub-2260202932541442~3552959710";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
        this.RequestInterstitial();
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2260202932541442/9817494475";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-2260202932541442/2239878045";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    private void RequestInterstitial()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-2260202932541442/6376942914";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-2260202932541442/8887556186";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    #region Interstitial callback handlers

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
        GameController.instance.restartReady = true;
        // Reset interstitial
        this.RequestInterstitial();
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
        GameController.instance.restartReady = true;
        // Reset interstitial
        this.RequestInterstitial();
    }

    #endregion
}