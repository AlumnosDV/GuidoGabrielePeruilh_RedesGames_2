using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : NetworkBehaviour
{

    [Networked]
    private int PlayersInGame { get; set; }

    [Networked]
    private float Timer { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
            Timer += Runner.DeltaTime;

        //EventManager.TriggerEvent("UpdateTimer", FormatDate(Timer));
    }

    private string FormatDate(float myTime)
    {
        int hours = Mathf.FloorToInt(myTime / 3600);
        int minutes = Mathf.FloorToInt((myTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(myTime % 60);

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
