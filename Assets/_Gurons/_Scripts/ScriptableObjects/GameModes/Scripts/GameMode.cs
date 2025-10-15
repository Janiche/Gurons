using System.Collections.Generic;
using UnityEngine;


public class GameMode : ScriptableObject
{
    #region Variables

    [Header("Game Mode")]
    public int minChances = 0; //CANTIDAD MÍNIMA DE OPORTUNIDADES
    public int maxChances = 2; //CANTIDAD MÁXIMA DE OPORTUNIDADES

    protected Transform _gameManagerTransform;
    #endregion

    #region FUNCTIONS

    public virtual void _Init()
    { }

    /// <summary>
    /// Suma puntaje
    /// </summary>
    /// <param name="score">Score a sumar.</param>
    public virtual void AddScore(int score)
    {
        GlobalVars.score += (GlobalVars.powerUpMultiplier * GlobalVars.scoreMultiplier * score);
        GlobalVars.blox++;
        GameEvents.ChangeScore();


        //PATCH 1.1.1=>1.2.0
        //GlobalVars.saveData.gameData.totalBlox++;
    }

    /// <summary>
    /// Genera un powerUp
    /// </summary>
    public virtual void GenPowerUp()
    {
    }

    /// <summary>
    /// Termina el juego, actualizando data o mostrando pantallas finales
    /// </summary>
    public virtual void _EndGame()
    {
    }

    /// <summary>
    /// Resta oportunidades y valida si llegó a cero para hacer injugable
    /// </summary>
    public virtual void _FailGuron()
    {
    }

    /// <summary>
    /// Termina el posicionamiento del Guron
    /// </summary>
    public virtual void _EndGuron()
    { }

    /// <summary>
    /// Metodo gatillado al colocar un guron en el centro
    /// </summary>
    public virtual void _CenterGuron()
    { }

    /// <summary>
    /// Metodo gatillado al colocar un guron en el lado
    /// </summary>
    public virtual void _SideGuron()
    { }

    /// <summary>
    /// Configura el transform del GameManager
    /// </summary>
    /// <param name="_t">T.</param>
    public void _SetTransform(Transform _t)
    {
        _gameManagerTransform = _t;
    }

    protected virtual void _EvalCondition()
    {
    }

    public virtual void OnConversationEnd()
    {

    }
    #endregion


}
