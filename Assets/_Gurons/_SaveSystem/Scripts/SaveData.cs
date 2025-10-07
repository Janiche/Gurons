//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Net;

using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class SaveData
{
    public string version = UnityEngine.Application.version;
    public PlayerData playerData = new PlayerData();
    public GameData gameData = new GameData();
    public RewardData rewardData = new RewardData();
    public AchivementData achivementData = new AchivementData();
}

[System.Serializable]
public class PlayerData
{
    public ObscuredString playerId = "";
    public ObscuredBool tutorial = false;
    public ObscuredBool inGame = false;
    public ObscuredBool rated = false;
    public ObscuredInt scratchIndex = 0;
    public System.DateTime? firstLogin = null;
    public System.DateTime lastLogin = System.DateTime.Now.Date;
    public System.DateTime nextRate = System.DateTime.Now.Date.AddDays(GlobalVars.timeToRate);
    public System.DateTime lastGiven = System.DateTime.Now;
}

[System.Serializable]
public class GameData
{
    public ObscuredInt planetToUnlock = -1;

    //PATCH 1.1.1=>1.2.0: NO EXISTE
    public ObscuredInt totalBlox = 0;

    //PATCH 1.2.1=>1.2.2: NO EXISTE
    public ObscuredInt totalLoses = 0;  //X
    public ObscuredInt totalFails = 0;
    public ObscuredInt totalCenter = 0;
    public ObscuredInt totalSide = 0;


    public PlanetData[] planetData =
        {
        new PlanetData("earth",     0, 0, false),
        new PlanetData("fire",      0, 0, false),
        new PlanetData("ice",       0, 0, false),
        new PlanetData("sand",      0, 0, false),
        new PlanetData("plant",     0, 0, false),
        new PlanetData("amethyst",  0, 0, false)
        };
}

[System.Serializable]
public class PlanetData
{
    public ObscuredString name = "";
    public ObscuredInt bestRecord = 0;
    public ObscuredInt maxBlox = 0;
    public ObscuredBool unlocked = false;

    //public PlanetData(string _n, int _b, bool _u)
    public PlanetData(string _n, int _b, int _m, bool _u)
    {
        name = _n;
        bestRecord = _b;
        maxBlox = _m;
        unlocked = _u;
    }
}

[System.Serializable]
public class RewardData
{
    public ObscuredBool _assigned = false;
    public ObscuredInt _currentDay = 0;
    public ObscuredInt _currentAd = 0;
}

[System.Serializable]
public class AchivementData
{
    public List<Achivement> achivements = new List<Achivement>();
}




[System.Serializable]
public class Achivement
{
    public ObscuredInt id = -1;
    public ObscuredString name = string.Empty;
    public ObscuredBool unlocked = false;
    public ObscuredBool claimed = false;

    public Achivement() { }

    public Achivement(int i, string nombre, bool estado)
    {
        id = i;
        name = nombre;
        unlocked = estado;
        claimed = false;
    }

    public Achivement(int i, string nombre, bool estado, bool reclamado)
    {
        id = i;
        name = nombre;
        unlocked = estado;
        claimed = reclamado;
    }
}


