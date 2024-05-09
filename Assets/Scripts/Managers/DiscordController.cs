using Lachee.Discord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscordController : MonoBehaviour
{

    public Presence presence;


    private void Start()
    {
        //Register to a presence change
        presence.startTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        DiscordManager.current.SetPresence(presence);
    }

}