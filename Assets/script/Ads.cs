using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
        Advertisement.Initialize("041851cf60884ee9bb5b98119bde0ea6d3555cbf43ecaf26a5a6da151fe1ca1c");
    }
    public void showAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}
