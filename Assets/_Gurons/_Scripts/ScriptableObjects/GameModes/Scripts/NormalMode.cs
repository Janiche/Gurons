using UnityEngine;

[CreateAssetMenu(menuName = "Bermost/GameMode/Normal")]
public class NormalMode : GameMode
{
    PlanetData pd = null;
    PlanetData currentPd = null;

    public override void _Init()
    {

        GameManager._instance._game = true;

        //ASIGNA INFORMACIÓN SOBRE SIGUIENTE PLANETA

        if (GlobalVars.nextPlanet < GlobalVars.maxPlanets)
        {
            pd = GlobalVars.saveData.gameData.planetData[GlobalVars.nextPlanet];
        }

        currentPd = GlobalVars.saveData.gameData.planetData[GlobalVars.currentPlanet];

        GameManager._instance._GenerateGuron();
    }

    public override void _CenterGuron()
    {
        AddScore(2);
        //GlobalVars.saveData.gameData.totalCenter++;
        //GlobalVars.saveData.gameData.totalBlox++;
        //Debug.LogWarning("CENTER");
        _UpdatePlanetScore();
        GameManager._instance._CenterPrize();
    }

    public override void _SideGuron()
    {
        AddScore(1);
        //GlobalVars.saveData.gameData.totalSide++;
        //GlobalVars.saveData.gameData.totalBlox++;
        //Debug.LogWarning("SIDE");
        _UpdatePlanetScore();
        GameManager._instance._SetCenterIndex(0);
    }

    public override void _EndGuron()
    {
        //GENERA NUEVO GURON
        GameManager._instance._GenerateGuron();

        //GameEvents.CheckGuronAchievement();

        //EXISTE UN SIGUIENTE PLANETA AL CUAL IR
        if (GlobalVars.maxPlanets <= GlobalVars.nextPlanet)
            return;

        //SI EL SIGUIENTE PLANETA NO HA SIDO DESBLOQUEADO
        if (pd.unlocked)
            return;

        //DESBLOQUEA SIGUIENTE PLANETA
        _EvalCondition();

    }

    public override void _FailGuron()
    {
        GameManager._instance._SetCenterIndex(0);
        //GlobalVars.saveData.gameData.totalFails++;
    }

    public override void _EndGame()
    {
        //GlobalVars.saveData.gameData.totalLoses++;
        //GameEvents.CheckLosesAchievement();
        GameEvents.GameOver();
        _UpdatePlanetScore();
        //SaveSystem.SaveData();
    }

    private void _UpdatePlanetScore()
    {
        if (GlobalVars.score > currentPd.maxBlox)
        {
            currentPd.maxBlox = GlobalVars.score;
            currentPd.maxBlox = Mathf.Clamp(currentPd.maxBlox, 0, GlobalVars.bloxToComplete);
        }

        if (GlobalVars.score > currentPd.bestRecord)
        {
            currentPd.bestRecord = GlobalVars.score;
        }




        //if (GlobalVars.blox > currentPd.maxBlox)
        //{
        //    currentPd.maxBlox = GlobalVars.blox;

        //    currentPd.maxBlox = Mathf.Clamp(currentPd.maxBlox, 0, GlobalVars.bloxToComplete);
        //}

        //if (GlobalVars.score > currentPd.bestRecord)
        //{
        //    currentPd.bestRecord = GlobalVars.score;
        //}
    }

    protected override void _EvalCondition()
    {
        //GameEvents.CheckPlanetAchievement();

        if ((currentPd.maxBlox) >= GlobalVars.bloxToComplete)
        {
            GlobalVars.saveData.gameData.planetToUnlock = GlobalVars.nextPlanet;
            pd.unlocked = true;
            _UpdatePlanetScore();
            GameEvents.UnlockPlanet();
        }
    }
}
