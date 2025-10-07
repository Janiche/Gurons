using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bermost
{
    [CreateAssetMenu(menuName = "Bermost/Reward/Scratch")]
    public class ScratchPrize : ScriptableObject
    {
        public string _prizeLabel = string.Empty;
        public Constants._Currencies _product;
        public Sprite _prizeIcon;
        public int _amount = 1;
        public float _percent = 0;
    }

}
