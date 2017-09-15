// Copyright 2014 Google Inc. All Rights Reserved.

/// Positions to place an ad.
typedef NS_ENUM(NSUInteger, GADAdPosition) {
  kGADAdPositionCustom = -1,              ///< Custom ad position.
  kGADAdPositionTopOfScreen = 0,          ///< Top of screen.
  kGADAdPositionBottomOfScreen = 1,       ///< Bottom of screen.
  kGADAdPositionTopLeftOfScreen = 2,      ///< Top left of screen.
  kGADAdPositionTopRightOfScreen = 3,     ///< Top right of screen.
  kGADAdPositionBottomLeftOfScreen = 4,   ///< Bottom left of screen.
  kGADAdPositionBottomRightOfScreen = 5,  ///< Bottom right of screen.
  kGADAdPositionCenterOfScreen = 6        ///< Bottom right of screen.
};

typedef NS_ENUM(NSInteger, GADUAdSize) { kGADUAdSizeUseFullWidth = -1 };

/// Base type representing a GADU* pointer.
typedef const void *GADUTypeRef;

/// Type representing a Unity banner client.
typedef const void *GADUTypeBannerClientRef;

/// Type representing a Unity interstitial client.
typedef const void *GADUTypeInterstitialClientRef;

/// Type representing a Unity reward based video client.
typedef const void *GADUTypeRewardBasedVideoAdClientRef;

/// Type representing a Unity ad loader.
typedef const void *GADUTypeAdLoaderClientRef;

/// Type representing a Unity native custom template ad.
typedef const void *GADUTypeNativeCustomTemplateAdClientRef;

/// Type representing a Unity native express ad client.
typedef const void *GADUTypeNativeExpressAdClientRef;

/// Type representing a GADUBanner.
typedef const void *GADUTypeBannerRef;

/// Type representing a GADUInterstitial.
typedef const void *GADUTypeInterstitialRef;

/// Type representing a GADURewardBasedVideoAd.
typedef const void *GADUTypeRewardBasedVideoAdRef;

/// Type representing a GADUAdLoader.
typedef const void *GADUTypeAdLoaderRef;

/// Type representing a GADUNativeCustomTemplateAd.
typedef const void *GADUTypeNativeCustomTemplateAdRef;

/// Type representing a GADUNativeExpressAd.
typedef const void *GADUTypeNativeExpressAdRef;

/// Type representing a GADURequest.
typedef const void *GADUTypeRequestRef;

/// Type representing a NSMutableDictionary of extras.
typedef const void *GADUTypeMutableDictionaryRef;

/// Type representing a GADUAdNetworkExtras.
typedef const void *GADUTypeAdNetworkExtrasRef;

/// Callback for when a banner ad request was successfully loaded.
typedef void (*GADUAdViewDidReceiveAdCallback)(GADUTypeBannerClientRef *bannerClient);

/// Callback for when a banner ad request failed.
typedef void (*GADUAdViewDidFailToReceiveAdWithErrorCallback)(GADUTypeBannerClientRef *bannerClient,
                                                              const char *error);

/// Callback for when a full screen view is about to be presented as a result of a banner click.
typedef void (*GADUAdViewWillPresentScreenCallback)(GADUTypeBannerClientRef *bannerClient);

/// Callback for when a full screen view is about to be dismissed.
typedef void (*GADUAdViewWillDismissScreenCallback)(GADUTypeBannerClientRef *bannerClient);

/// Callback for when a full screen view has just been dismissed.
typedef void (*GADUAdViewDidDismissScreenCallback)(GADUTypeBannerClientRef *bannerClient);

/// Callback for when an application will background or terminate as a result of a banner click.
typedef void (*GADUAdViewWillLeaveApplicationCallback)(GADUTypeBannerClientRef *bannerClient);

/// Callback for when a interstitial ad request was successfully loaded.
typedef void (*GADUInterstitialDidReceiveAdCallback)(
    GADUTypeInterstitialClientRef *interstitialClient);

/// Callback for when an interstitial ad request failed.
typedef void (*GADUInterstitialDidFailToReceiveAdWithErrorCallback)(
    GADUTypeInterstitialClientRef *interstitialClient, const char *error);

/// Callback for when an interstitial is about to be presented.
typedef void (*GADUInterstitialWillPresentScreenCallback)(
    GADUTypeInterstitialClientRef *interstitialClient);

/// Callback for when an interstitial is about to be dismissed.
typedef void (*GADUInterstitialWillDismissScreenCallback)(
    GADUTypeInterstitialClientRef *interstitialClient);

/// Callback for when an interstitial has just been dismissed.
typedef void (*GADUInterstitialDidDismissScreenCallback)(
    GADUTypeInterstitialClientRef *interstitialClient);

/// Callback for when an application will background or terminate because of an interstitial click.
typedef void (*GADUInterstitialWillLeaveApplicationCallback)(
    GADUTypeInterstitialClientRef *interstitialClient);

/// Callback for when a reward based video ad request was successfully loaded.
typedef void (*GADURewardBasedVideoAdDidReceiveAdCallback)(
    GADUTypeRewardBasedVideoAdClientRef *rewardBasedVideoClient);

/// Callback for when a reward based video ad request failed.
typedef void (*GADURewardBasedVideoAdDidFailToReceiveAdWithErrorCallback)(
    GADUTypeRewardBasedVideoAdClientRef *rewardBasedVideoClient, const char *error);

/// Callback for when a reward based video is opened.
typedef void (*GADURewardBasedVideoAdDidOpenCallback)(
    GADUTypeRewardBasedVideoAdClientRef *rewardBasedVideoClient);

/// Callback for when a reward based video has started to play.
typedef void (*GADURewardBasedVideoAdDidStartPlayingCallback)(
    GADUTypeRewardBasedVideoAdClientRef *rewardBasedVideoClient);

/// Callback for when a reward based video is closed.
typedef void (*GADURewardBasedVideoAdDidCloseCallback)(
    GADUTypeRewardBasedVideoAdClientRef *rewardBasedVideoClient);

/// Callback for when a user is rewarded by a reward based video.
typedef void (*GADURewardBasedVideoAdDidRewardCallback)(
    GADUTypeRewardBasedVideoAdClientRef *rewardBasedVideoClient, const char *rewardType,
    double rewardAmount);

/// Callback for when an application will background or terminate because of an reward based video
/// click.
typedef void (*GADURewardBasedVideoAdWillLeaveApplicationCallback)(
    GADUTypeRewardBasedVideoAdClientRef *rewardBasedVideoClient);

/// Callback for when a native express ad request was successfully loaded.
typedef void (*GADUNativeExpressAdViewDidReceiveAdCallback)(
    GADUTypeNativeExpressAdClientRef *nativeExpressClient);

/// Callback for when a native express ad request failed.
typedef void (*GADUNativeExpressAdViewDidFailToReceiveAdWithErrorCallback)(
    GADUTypeNativeExpressAdClientRef *nativeExpressClient, const char *error);

/// Callback for when a full screen view is about to be presented as a result of a native express
/// click.
typedef void (*GADUNativeExpressAdViewWillPresentScreenCallback)(
    GADUTypeNativeExpressAdClientRef *nativeExpressClient);

/// Callback for when a full screen view is about to be dismissed.
typedef void (*GADUNativeExpressAdViewWillDismissScreenCallback)(
    GADUTypeNativeExpressAdClientRef *nativeExpressClient);

/// Callback for when a full screen view has just been dismissed.
typedef void (*GADUNativeExpressAdViewDidDismissScreenCallback)(
    GADUTypeNativeExpressAdClientRef *nativeExpressClient);

/// Callback for when an application will background or terminate as a result of a native express
/// click.
typedef void (*GADUNativeExpressAdViewWillLeaveApplicationCallback)(
    GADUTypeNativeExpressAdClientRef *nativeExpressClient);

/// Callback for when a native custom template ad request was successfully loaded.
typedef void (*GADUAdLoaderDidReceiveNativeCustomTemplateAdCallback)(
    GADUTypeAdLoaderClientRef *adLoader, GADUTypeNativeCustomTemplateAdRef nativeCustomTemplateAd,
    const char *templateID);

/// Callback for when a native ad request failed.
typedef void (*GADUAdLoaderDidFailToReceiveAdWithErrorCallback)(GADUTypeAdLoaderClientRef *adLoader,
                                                                const char *error);

/// Callback for when a native custom template ad is clicked.
typedef void (*GADUNativeCustomTemplateDidReceiveClickCallback)(
    GADUTypeNativeCustomTemplateAdClientRef *nativeCustomTemplateAd, const char *assetName);
