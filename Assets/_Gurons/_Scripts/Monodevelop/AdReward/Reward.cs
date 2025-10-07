using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


namespace Bermost
{
    [CreateAssetMenu(menuName = "Bermost/Reward/Reward")]
    [System.Serializable]
    public class Reward : ScriptableObject
    {
        public Constants._Currencies productId = Constants._Currencies.none;
        public ObscuredInt amount = 0;
        public Sprite icon = null;
    }
}