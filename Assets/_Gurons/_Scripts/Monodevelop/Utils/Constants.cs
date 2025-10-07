public class Constants
{
    public enum _UICommand
    {
        _none,
        _toMenu,
        _toStore,
        _retry,
        _adContinue,
        _diamondContinue,
        _noLifeContinue,
        _resume,
        _restart,
        _keepPlaying,
        _nextPlanet,
        _continueTutorial,
        _retryTutorial,
        _skipTutorial,
    }

    public enum _GameMode
    {
        none,
        normal,
        tutorial
    }

    public enum _Tags
    {
        Floor,
        Deprecated,
        Stack,
        Cube,
        Wall,
    }

    public enum _GuronState
    {
        none,
        Balance,
        Fall,
        Hit,
        Idle
    }

    public enum _PoolType
    {
        _none,
        _guron,
        //_powerUp,
        //_feedback,
        _space,
    }

    public enum _SelectionType
    {
        _none,
        _first,
        _random,
        _name,
        _index,
    }

    public enum _PowerUps
    {
        none,
        NewChance,
        NewDiamond,
        NewLife,
        ScoreMultiplier,
        SlowMo,
    }

    public enum _GameContinue
    {
        _none,
        _diamondAndAds,
        _onlyAds,
        _noContinue,
    }

    public enum _Currencies
    {
        none,
        one_life,
        diamond_currency,
        coin_currency,
        extra_chance,
        double_score,
        one_accelerator,
        five_accelerator,
    }

    public enum _CurrencyCondition
    {
        none,
        balance,
        bought,
        spent
    }

    public enum _GuronsCondition
    {
        none,
        place,
        center,
        side,
        fail
    }

    public enum _GameCondition
    {
        firstTime,
        allPlanets,
        timer,

    }

    public enum _SceneName
    {
        none,
        Splash,
        MainMenu,
        Planets,
        Store,
        Settings,
        LoadingScene,
        Game,
    }

    public enum _Planets
    {
        kublener,
        khzar,
        furitsu,
        drosera,
        qum,
        karistal
    }

    public enum BuyContext
    {
        currency,
        market
    }


    public struct TransactionContext
    {
        public const string ON_STORE = "onStore";
        public const string ON_MENU = "onSMenu";
        public const string ON_GAME = "onGame";
    }

    public class Version_Constants
    {
        public const string VERSION_1_1_1 = "1.1.1";
        public const string VERSION_1_2_0 = "1.2.0";
        public const string VERSION_1_2_1 = "1.2.1";
        public const string VERSION_1_2_2 = "1.2.2";
    }

    public class IAP_Constants
    {
        public const string Product_Single_life = "one_life_bag";
        public const string Product_Small_lifes_bottle = "lifes_3_bag";
        public const string Product_Medium_lifes_bottle = "lifes_15_bag";
        public const string Product_Big_lifes_bottle = "lifes_25_bag";
        public const string Product_Small_diamond_bag = "diamonds_25_bag";
        public const string Product_Medium_diamond_bag = "diamonds_60_bag";
        public const string Product_Big_diamond_bag = "diamonds_100_bag";
        public const string Product_Small_coins_box = "coins_400_bag";
        public const string Product_Medium_coins_box = "coins_1000_bag";
        public const string Product_Big_coins_box = "coins_1800_bag";
        public const string Product_Extra_Chance = "extra_chance";
        public const string Product_Double_Score = "double_score";

    }


    public struct Stores
    {
        public const string GOOGLE_PLAY = "googlePlay";
        public const string IOS = "ios";
        public const string HUAWEI_APP_GALLERY = "Huawei";
        public const string AMAZON_APPSTORE = "amazon";
        public const string TPAY = "Tpay";
        public const string XIAOMI = "XiaomiStore";

    }

}
