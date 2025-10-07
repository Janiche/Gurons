using UnityEngine;

namespace Bermost
{

    [CreateAssetMenu(menuName = "Bermost/Reward/Day")]
    [System.Serializable]
    public class Day : ScriptableObject
    {
        public Ad[] ads = new Ad[5];
    }

    [System.Serializable]
    public class Ad
    {
        public int id = 0;
        public Reward reward = null;
    }


}