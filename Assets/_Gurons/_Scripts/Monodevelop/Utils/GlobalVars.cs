//CLASE ESTÁTICA CON VALORES SENSIBLES DE JUEGO, DE RÁPIDO ACCESO
using CodeStage.AntiCheat.ObscuredTypes;

public static class GlobalVars
{
    //DATOS DE JUEGO
    public static string _transactionContext = string.Empty;
    //public static string gameName = "(The Gurons)";
    public static ObscuredBool connected = false; //TODO ELIMINAR
    public static ObscuredBool started = false;
    public static ObscuredFloat currentTimeScale = 1.0f;
    public static ObscuredBool pirate = false;
    public static ObscuredInt _loses = 0;

    //DATOS DE GUARDADO
    public static SaveData saveData = new SaveData();    //VARIABLE DE DATOS DE AVANCE
    public static PrefData prefData = new PrefData();    //VARIABLE DE DATOS DE PREFERENCIAS
    //public static RewardData rewardData = new RewardData();        //VARIABLE DE DATOS DE PREMIOS DIARIOS

    //SCORE
    public static ObscuredInt blox = 0;     //CANTIDAD DE BLOQUES COLOCADOS
    public static ObscuredInt score = 0;     //PUNTAJE

    public static ObscuredInt scoreMultiplier = 1;   //MULTIPLICADOR DE PUNTAJE

    //POWERUP
    public static ObscuredInt powerUpMultiplier = 1; //MULTIPLICADOR PROPIO DEL COMPORTAMIENTO GURON
    public static ObscuredInt extraChance = 0; //MULTIPLICADOR PROPIO DEL COMPORTAMIENTO GURON
    public static ObscuredBool _chanceAdded = false;
    public static ObscuredBool _scoreAdded = false;

    //PLANETAS   
    public static ObscuredInt currentPlanet = 0;
    public static ObscuredString currentPlanetName = string.Empty;
    public static ObscuredInt nextPlanet = -1;
    public static ObscuredFloat gravity = 9.81f; //GRAVEDAD DEL PLANETA ACTUAL
    public static ObscuredFloat wind = 0;   //VIENTO DEL PLANETA ACTUAL
    public static ObscuredInt bloxToComplete = 0;    //CANTIDAD DE BLOQUES NECESARIOS PARA TERMINAR EL PLANETA ACTUAL
    public static ObscuredInt maxPlanets = 6; //CANTIDAD MÁXIMA DE PLANETAS, SE ACTUALIZA AL ABRIR ESCENA PLANETS

    //RATE APP
    public static ObscuredString store = string.Empty;
    public static ObscuredBool showRate = true;
    public static ObscuredDouble timeToRate = 3;    //TIEMPO A ESPERAR DESDE RATE LATER A NUEVO RATE



}
