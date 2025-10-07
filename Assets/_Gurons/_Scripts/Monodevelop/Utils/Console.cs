using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Console : MonoBehaviour
{
    [SerializeField] private TMP_InputField command = null;
    [SerializeField] private TextMeshProUGUI message = null;


    public void EnterCommand()
    {
        int cant = 0;
        string enterCmd = "";

        message.text = ">>";

        try
        {
            string[] cmd = command.text.Split(" "[0]);
            enterCmd = cmd[0];
            cant = (!string.IsNullOrEmpty(cmd[1])) ? int.Parse(cmd[1]) : 0;
        }

        catch (System.Exception e)
        {
            enterCmd = command.text;
        }

        //message.text = string.Empty;

        switch (enterCmd.ToLower())
        {
            case "wannalive":
                Store.instance.GiveProduct(Constants._Currencies.one_life, cant);
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Lives added";
                break;

            case "wannadie":
                //int lives = Store.instance.GetBalance(" one_life");
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Lives removed";

                Store.instance.TakeProduct(Constants._Currencies.one_life, cant);
                break;

            case "wannaplay":
                Store.instance.GiveProduct(Constants._Currencies.diamond_currency, cant);
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Diamonds added";
                break;

            case "wannapay":
                Store.instance.GiveProduct(Constants._Currencies.coin_currency, cant);
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Coins added";
                break;

            case "wannatry":
                Store.instance.GiveProduct(Constants._Currencies.extra_chance, cant);
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Extra Chances added";
                break;

            case "wannadouble":
                Store.instance.GiveProduct(Constants._Currencies.double_score, cant);
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Double Score added";
                break;

            case "wannabepoor":
                Store.instance.TakeProduct(Constants._Currencies.coin_currency, cant);
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Coins removed";
                break;

            case "balance":
                string balance =
                    ">> lives: " + Store.instance.GetBalance(Constants._Currencies.one_life) + "\n" +
                    ">> diamonds: " + Store.instance.GetBalance(Constants._Currencies.diamond_currency) + "\n" +
                    ">> coins: " + Store.instance.GetBalance(Constants._Currencies.coin_currency) + "\n" +
                    ">> extra chances: " + Store.instance.GetBalance(Constants._Currencies.extra_chance) + "\n" +
                    ">> double Score: " + Store.instance.GetBalance(Constants._Currencies.double_score) + "\n";

                message.text += "\n>> " + command.text + "\n" + balance;

                break;
            case "help":
                string help =
                    ">> command i\n" +
                    ">> wannalive: Adds lifes\n" +
                    ">> wannaplay: Adds diamonds\n" +
                    ">> wannapay: Adds Coins\n" +
                    ">> wannatry: Adds Extra chances\n" +
                    ">> wannadouble Add Double Score\n" +
                    ">> wannadie: Removes lifes\n" +
                    ">> wannabepoor: Removes coins\n" +
                    ">> noads: Restart current ad to 0\n" +
                    ">> lockplanet: Lock given planet\n" +
                    ">> unlockplanet: Unlock given planet\n" +
                    ">> situtorial: Activa tutorial\n" +
                    ">> notutorial: Desactiva tutorial\n"

                    ;

                message.text += "\n>> " + command.text + "\n" + help;
                break;

            case "notutorial":
                GlobalVars.saveData.playerData.tutorial = true;
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Tutorial Deactivated";
                SaveSystem.SaveData();
                break;

            case "situtorial":
                GlobalVars.saveData.playerData.tutorial = false;
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Tutorial Activated";
                SaveSystem.SaveData();
                break;

            case "noads":
                GlobalVars.saveData.rewardData._currentAd = 0;
                message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Ads Reseted";
                SaveSystem.SaveData();
                break;

            case "lockplanet":

                if (cant < GlobalVars.maxPlanets)
                {
                    GlobalVars.saveData.gameData.planetData[cant].unlocked = false;
                    message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Planet locked";
                    SaveSystem.SaveData();
                }
                else
                {
                    message.text += "\n>> " + command.text + "\n PLANET NOT FOUND";
                }

                break;

            case "unlockplanet":

                if (cant < GlobalVars.maxPlanets)
                {
                    GlobalVars.saveData.gameData.planetData[cant].unlocked = true;
                    message.text += "\n>> " + command.text + "\n" + cant.ToString() + " Planet unlocked";
                    SaveSystem.SaveData();
                }
                else
                {
                    message.text += "\n>> " + command.text + "\n PLANET NOT FOUND";

                }

                break;
            default:
                message.text += "\n>> " + command.text + "\n COMMAND NOT FOUND";
                break;
        }





    }
}
