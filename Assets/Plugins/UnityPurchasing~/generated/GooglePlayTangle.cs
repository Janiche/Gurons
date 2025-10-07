#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("f7mVuIirJU4ZsL9gDp8h0mQ0LbcaGOuHOTnPE5RRttzFfYd14QsVEimIJssIhlk+RglKtSnR/+7bdbluphSXtKabkJ+8EN4QYZuXl5eTlpW4aCy/deltpHi9Rv1bnbwwSxKyBcyxI7HJq3v+k9r+LcbMkqKCGmc9FJeZlqYUl5yUFJeXlgA4cXmKBaFQ/XfEqrfmWEsS4wtKqUT7ulUtELcSyqEi24EjDHuhQjBy2OoU2rEi1QEV9aqMGIDpavDjLS27+Bf8ipmNbHZGjlhn0nP0Eht/StiYU/qoHTa2iIgVWvr2CzxJ/V5Z2nCxxV9+86k6PlfS5hqPZhmfb5RcPqlnkDbFFp46BAT4w/bFcoTmew4IliXidAF0KBGaeJq2VZSVl5aX");
        private static int[] order = new int[] { 13,11,12,13,8,10,11,12,10,11,10,12,12,13,14 };
        private static int key = 150;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
