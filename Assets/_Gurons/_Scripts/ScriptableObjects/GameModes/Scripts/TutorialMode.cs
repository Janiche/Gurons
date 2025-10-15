using UnityEngine;

[CreateAssetMenu(menuName = "Bermost/GameMode/Tutorial")]

public class TutorialMode : GameMode
{

    bool _guronFailed = false;

    #region FUNCIONES GAMEMODE

    public override void _Init()
    {
        InitTutorial();
    }

    private void InitTutorial()
    {
        //currentState = TutorialState.welcome;
        //GameManager._instance._GenerateGuron();
        TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.welcome);

        /*DialogueLua.SetVariable("FailedGuron", false);
        DialogueLua.SetVariable("isFirstPlaced", false);*/


        GameManager._instance._game = false;

        //DialogueManager.Instance.StartConversation("Tutorial_Start", _gameManagerTransform);

    }

    public override void _CenterGuron()
    {
        AddScore(2);
        GameManager._instance._CenterPrize();

        //switch (TutorialStateMachine._GetState)
        //{
        //    case TutorialStateMachine.TutorialState.waitingFirstPlace:

        //        DialogueLua.SetVariable("isFirstPlaced", true);
        //        break;

        //    case TutorialStateMachine.TutorialState.firstSuccess:
        //        break;
        //}

        ////GameManager._instance._game = false;
        ////GameManager._instance._GenerateGuron();
        //DialogueManager.Instance.StartConversation("Place_Quest", _gameManagerTransform);


        //UpdateState();
        //GameManager.Instance.CenterPrize();
    }

    public override void _SideGuron()
    {
        AddScore(1);
        GameManager._instance._SetCenterIndex(0);

        //switch (TutorialStateMachine._GetState)
        //{
        //    case TutorialStateMachine.TutorialState.waitingFirstPlace:

        //        DialogueLua.SetVariable("isFirstPlaced", true);
        //        break;

        //    case TutorialStateMachine.TutorialState.firstSuccess:
        //        break;
        //}

        ////GameManager._instance._game = false;
        ////GameManager._instance._GenerateGuron();
        //DialogueManager.Instance.StartConversation("Place_Quest", _gameManagerTransform);

    }

    public override void _FailGuron()
    {
        GameManager._instance._SetCenterIndex(0);
        /*int _failed = DialogueLua.GetVariable("FailedGuron").asInt;
        _failed++;
        DialogueLua.SetVariable("FailedGuron", _failed);*/

        _guronFailed = true;

        switch (TutorialStateMachine._GetState)
        {
            case TutorialStateMachine.TutorialState.waitingFirstPlace:

                //DialogueLua.SetVariable("isFirstPlaced", false);
                TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.firstFailed);
                break;
        }


        /*if (_failed == 3)
        {
            TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.missionFailed);
        }*/
        //Debug.Log("FAILED");
        //GameManager._instance._game = false;
        //GameManager._instance._GenerateGuron();
        //DialogueManager.Instance.StartConversation("Place_Quest", _gameManagerTransform);
    }

    public override void _EndGuron()
    {
        if (_guronFailed)
        {
            GameManager._instance._game = false;
        }

        switch (TutorialStateMachine._GetState)
        {
            case TutorialStateMachine.TutorialState.waitingFirstPlace:

                GameManager._instance._game = false;
                //DialogueLua.SetVariable("isFirstPlaced", true);
                TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.firstSuccess);
                //DialogueManager.Instance.StartConversation("Place_Quest", _gameManagerTransform);

                break;

            case TutorialStateMachine.TutorialState.missionStart:

                if (GlobalVars.blox < 11)
                {
                    //DialogueLua.SetVariable("PlacedGuron", GlobalVars.blox - 1);
                }
                else
                {
                    //DialogueLua.SetVariable("PlacedGuron", 10);
                    GameManager._instance._game = false;
                    //DialogueManager.Instance.StartConversation("Place_Quest", _gameManagerTransform);
                    TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.missionDone);
                }

                break;
        }

        //DialogueManager.Instance.StartConversation("Place_Quest", _gameManagerTransform);
        GameManager._instance._GenerateGuron();
    }

    public override void _EndGame()
    {
    }
    #endregion

    #region FUNCIONES PROPIAS TUTORIAL

    public override void OnConversationEnd()
    {
        GameManager._instance._game = true;
        _guronFailed = false;

        //Debug.Log("A: " + TutorialStateMachine._GetState);

        switch (TutorialStateMachine._GetState)
        {
            case TutorialStateMachine.TutorialState.welcome:

                TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.waitingFirstPlace);
                break;

            //ACTIVA FASE DE CENTRO
            case TutorialStateMachine.TutorialState.firstSuccess:
                TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.missionStart);
                break;

            //ACTIVA FASE DE CENTRO
            case TutorialStateMachine.TutorialState.firstFailed:
                TutorialStateMachine._ChangeState(TutorialStateMachine.TutorialState.waitingFirstPlace);
                break;

            //ACTIVA FASE DE POWER UPS
            case TutorialStateMachine.TutorialState.missionStart:
                break;

            //DA INICIO A DESPEDIDA DE TUTORIAL
            case TutorialStateMachine.TutorialState.missionFailed:
                GameEvents.FailedTutorial();
                //SceneLoader.LoadAsyncWithLoader(Constants._SceneName.Game.ToString());

                break;

            //CAMBIA GAMEMODE [SI ES POSIBLE]
            case TutorialStateMachine.TutorialState.missionDone:
                //GlobalVars._planetToUnlock = 0;
                GlobalVars.saveData.gameData.planetToUnlock = 0;
                GameManager._instance._game = false;
                GlobalVars.saveData.playerData.tutorial = true;
                GameEvents.SuccessTutorial();
                //SaveSystem.Save();
                //SceneLoader.LoadAsyncWithLoader(Constants._SceneName.Planets.ToString());
                //_EndGame();
                break;
        }

    }
    #endregion
}
