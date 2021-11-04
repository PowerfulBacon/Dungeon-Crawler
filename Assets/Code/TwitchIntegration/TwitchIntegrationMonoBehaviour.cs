using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchIntegrationMonoBehaviour : MonoBehaviour
{

    TwitchIntegration twitchIntegration = new TwitchIntegration();

    // Update is called once per frame
    void Update()
    {
        foreach (TwitchMessage message in twitchIntegration.GetMessages())
        {
            
        }
    }
}
