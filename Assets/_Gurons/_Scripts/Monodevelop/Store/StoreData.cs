using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class StoreData
{

    public ObscuredInt diamonds = 10;
    public ObscuredInt lifes = 0;
    public ObscuredInt coins = 0;
    public ObscuredInt extraChance = 0;
    public ObscuredInt doubleScore = 0;
    public ObscuredInt oneAccelerator = 0;
    public ObscuredInt fiveAccelerator = 0;

    public Bought bought = new Bought();
    public Spent spent = new Spent();

    public StoreData()
    {
        diamonds = 10;
        lifes = 6;
        coins = 0;
        extraChance = 0;
        doubleScore = 0;
        oneAccelerator = 0;
        fiveAccelerator = 0;

        bought = new Bought();
        spent = new Spent();
    }
}
[System.Serializable]
public class Bought
{
    public ObscuredInt diamonds = 0;
    public ObscuredInt lifes = 0;
    public ObscuredInt coins = 0;
    public ObscuredInt extraChance = 0;
    public ObscuredInt doubleScore = 0;
    public ObscuredInt oneAccelerator = 0;
    public ObscuredInt fiveAccelerator = 0;
}

[System.Serializable]
public class Spent
{
    public ObscuredInt diamonds = 0;
    public ObscuredInt lifes = 0;
    public ObscuredInt coins = 0;
    public ObscuredInt extraChance = 0;
    public ObscuredInt doubleScore = 0;
    public ObscuredInt oneAccelerator = 0;
    public ObscuredInt fiveAccelerator = 0;
}


