using UnityEngine;

//using UnityEngine.UI;

[CreateAssetMenu (menuName = "Bermost/Planet")]
public class PlanetBP : ScriptableObject 
{
    [Tooltip("Identificador del Planeta")] public int id=0;
    [Tooltip("Nombre del Planeta")] public string _name="";
    [Tooltip("Sprite a mostrar")] public Sprite _sprite=null;
    [Tooltip("Gravedad del Planeta")] public float _gravity = -9.81f;
    [Tooltip("Viento del Planeta")] public float _wind = 0;
    [Tooltip("Gurons necesarios para completar planeta")] public int _toComplete = 30;
    [Tooltip("Valor para desbloquear con diamantes")] public int _unlockCost = 40;
}
