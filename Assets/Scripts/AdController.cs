using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdController : MonoBehaviour
{
	RewardBasedVideoAd rewardBasedVideo;
	bool rewardBasedEventHandlersSet = false;
	Text kittyzText;

	void Start ()
	{
		kittyzText = GameObject.Find ("KittyzText").GetComponentInChildren<Text> ();
		Debug.Log ("textObj " + kittyzText.ToString ());
		kittyzText.text = ApplicationController.ac.playerData.kittyz.ToString ();
		Debug.Log ("k: " + ApplicationController.ac.playerData.kittyz.ToString ());
		Debug.Log ("text: " + kittyzText.text);
	}

	public void RequestRewardBasedVideo ()
	{
		//string adUnitId = "ca-app-pub-3940256099942544/5224354917"; testing ID
		string adUnitId = Config.adUnitId;

		rewardBasedVideo = RewardBasedVideoAd.Instance;

		AdRequest request = new AdRequest.Builder ()
			.AddTestDevice (AdRequest.TestDeviceSimulator)       // Simulator.
			.AddTestDevice (Config.myTestDevice1) 
			.Build ();
		rewardBasedVideo.LoadAd (request, adUnitId);
		Debug.Log ("loading ad");
		// Reward based video instance is a singleton. Register handlers once to
		// avoid duplicate events.
		if (!rewardBasedEventHandlersSet) {
			// Ad event fired when the rewarded video ad
			// has been received.
			rewardBasedVideo.OnAdLoaded += HandleOnAdLoaded;
			rewardBasedVideo.OnAdFailedToLoad += HandleOnAdFailedToLoad;
			rewardBasedVideo.OnAdOpening += HandleOnAdOpening;
			rewardBasedVideo.OnAdStarted += HandleOnAdStarted;
			rewardBasedVideo.OnAdRewarded += HandleOnAdRewarded;
			rewardBasedVideo.OnAdClosed += HandleOnAdClosed;
			rewardBasedVideo.OnAdLeavingApplication += HandleOnAdLeavingApplication;

			rewardBasedEventHandlersSet = true;
		}
	}


	public void ShowAd ()
	{
		if (rewardBasedVideo.IsLoaded ()) {
			Debug.Log ("ad ready");
			rewardBasedVideo.Show ();
		} else
			Debug.Log ("not ready");
	}

	public void HandleOnAdLoaded (object sender, EventArgs args)
	{
		Debug.Log ("ad loaded");
		ShowAd ();
	}

	public void HandleOnAdFailedToLoad (object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log ("ad failed to load");
	}

	public void HandleOnAdOpening (object sender, EventArgs args)
	{
	}

	public void HandleOnAdStarted (object sender, EventArgs args)
	{
	}

	public void HandleOnAdClosed (object sender, EventArgs args)
	{
	}

	public void HandleOnAdRewarded (object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		Debug.Log ("User rewarded with: " + amount.ToString () + " " + type);
		ApplicationController.ac.playerData.updateKittys ((int)amount, kittyzText, true);

	}

	public void HandleOnAdLeavingApplication (object sender, EventArgs args)
	{
	}

}
