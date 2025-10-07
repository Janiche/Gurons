using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System.IO;
using CodeStage.AntiCheat.ObscuredTypes;

public class PatchData
{

    public static void UpdateTotalGurons()
    {
        int totalGurons = 0;

        for (int i = 0; i < GlobalVars.maxPlanets; i++)
        {
            totalGurons += GlobalVars.saveData.gameData.planetData[i].maxBlox;
        }

        GlobalVars.saveData.gameData.totalBlox = totalGurons;
    }


    public static void UpdatePlanetProgress()
    {
        List<ObscuredInt> _planetUnlock = new List<ObscuredInt>()
        {
            30,
            60,
            80,
            120,
            150,
            200
        };


        int planets = GlobalVars.saveData.gameData.planetData.Length;

        for (int i = 0; i < planets; i++)
        {
            if (GlobalVars.saveData.gameData.planetData[i].bestRecord >= _planetUnlock[i])
            {
                GlobalVars.saveData.gameData.planetData[i].unlocked = true;
                GlobalVars.saveData.gameData.planetData[i].maxBlox = _planetUnlock[i];
            }
        }

    }

    public static void UpdateAchievements()
    {
        //ACTUALIZA PRIMER LOGIN
        if (GlobalVars.saveData.playerData.firstLogin == null)
        {
            GlobalVars.saveData.playerData.firstLogin = System.DateTime.Now.Date;
        }

        //A
    }

}
