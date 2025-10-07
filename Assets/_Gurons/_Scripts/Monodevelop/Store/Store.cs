using UnityEngine;
using System.Collections.Generic;

using System.IO;
using Newtonsoft.Json;

//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Extension;
using System;
//using UnityEngine.Purchasing.Extension;
//using System.Collections.ObjectModel;

public class Store //: IStoreListener
{

    #region DELEGADAS

    public delegate void StoreEvents(Constants._Currencies _type, double _amount);
    public static event StoreEvents OnChangeValue;
    public static void ChangeValue(Constants._Currencies _type, double _amount)
    {
        if (OnChangeValue != null)
        {
            OnChangeValue(_type, _amount);
        }
    }

    public delegate void PurchaseEvents(BuyStatus _status, FailedReason _reason, string productId);
    public static event PurchaseEvents OnPurchaseSuccess;
    public static event PurchaseEvents OnPurchaseFail;

    public static void PurchaseSuccess(BuyStatus _status, FailedReason _reason, string productId)
    {
        if (OnPurchaseSuccess != null)
        {
            OnPurchaseSuccess(_status, _reason, productId);
        }
    }
    public static void PurchaseFail(BuyStatus _status, FailedReason _reason, string productId)
    {
        if (OnPurchaseFail != null)
        {
            OnPurchaseFail(_status, _reason, productId);
        }
    }

    #endregion

    #region VARIABLES

    private bool _initialized = false;

    private static Store _instance;

    private List<StoreProduct> activeButtons = new List<StoreProduct>();
    public StoreData storeData;
    private string storePath;
    private bool readDecoded = false;




    //ProductCatalog _catalog;
    /*
        private IStoreController _controller;
        private IExtensionProvider _extension;

    */

    //----------- CONSTRUCTOR STORE -------------\

    private Store()
    {
        if (PlayerPrefs.HasKey("Decoded"))
        {
            if (PlayerPrefs.GetInt("Decoded") == 1)
            {
                readDecoded = true;
            }
        }

#if UNITY_EDITOR
        storePath = Application.streamingAssetsPath + "/Storedata.bsd";
#elif UNITY_ANDROID
        storePath = Application.persistentDataPath + "/Storedata.txt";
#endif
        Load();

        //----------- INITIALIZE STORE -------------\

        //_catalog = ProductCatalog.LoadDefaultCatalog();
        /*
                StandardPurchasingModule module = StandardPurchasingModule.Instance();
                module.useFakeStoreUIMode = FakeStoreUIMode.Default;

                ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
                builder.useCatalogProvider = true;

                IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, _catalog);

                UnityPurchasing.Initialize(this, builder);
                */

    }

    static Store()
    {
        _instance = new Store();
    }

    public static Store instance
    {
        get { return _instance; }
    }

    #endregion

    #region FUNCIONES TIENDA

    private void InitialicePurchase(string productId)
    {
        /*
        if (_controller == null)
        {
            Debug.LogError("Purchase failed because Purchasing was not initialized correctly");
            OnPurchaseFailed(null, PurchaseFailureReason.PurchasingUnavailable);
            return;
        }
        else
        {
            _controller.InitiatePurchase(productId);
        }
        */
    }

    /// <summary>
    /// Obtiene stock del producto
    /// </summary>
    /// <returns>Cantidad de Stock.</returns>
    /// <param name="productId">Product identifier.</param>
    public int GetBalance(Constants._Currencies productId)
    {
        int balance = 0;

        switch (productId)
        {
            case Constants._Currencies.one_life:
                balance = storeData.lifes;
                break;

            case Constants._Currencies.diamond_currency:
                //balance = storeData.diamonds;
                balance = storeData.diamonds;
                break;

            case Constants._Currencies.coin_currency:
                balance = storeData.coins;
                break;

            case Constants._Currencies.extra_chance:
                balance = storeData.extraChance;
                break;

            case Constants._Currencies.double_score:
                balance = storeData.doubleScore;
                break;

            case Constants._Currencies.one_accelerator:
                balance = storeData.oneAccelerator;
                break;

            case Constants._Currencies.five_accelerator:
                balance = storeData.fiveAccelerator;
                break;

        }
        return balance;
    }

    public int GetBalance(string productId)
    {
        Constants._Currencies currencies = (Constants._Currencies)System.Enum.Parse(typeof(Constants._Currencies), productId, true);
        return GetBalance(currencies);
    }

    public int GetBought(Constants._Currencies productId)
    {
        int balance = 0;

        switch (productId)
        {
            case Constants._Currencies.one_life:
                balance = storeData.bought.lifes;
                break;

            case Constants._Currencies.diamond_currency:
                //balance = storeData.diamonds;
                balance = storeData.bought.diamonds;
                break;

            case Constants._Currencies.coin_currency:
                balance = storeData.bought.coins;
                break;

            case Constants._Currencies.extra_chance:
                balance = storeData.bought.extraChance;
                break;

            case Constants._Currencies.double_score:
                balance = storeData.bought.doubleScore;
                break;

            case Constants._Currencies.one_accelerator:
                balance = storeData.bought.oneAccelerator;
                break;

            case Constants._Currencies.five_accelerator:
                balance = storeData.bought.fiveAccelerator;
                break;

        }
        return balance;
    }

    public int GetSpent(Constants._Currencies productId)
    {
        int balance = 0;

        switch (productId)
        {
            case Constants._Currencies.one_life:
                balance = storeData.spent.lifes;
                break;

            case Constants._Currencies.diamond_currency:
                //balance = storeData.diamonds;
                balance = storeData.spent.diamonds;
                break;

            case Constants._Currencies.coin_currency:
                balance = storeData.spent.coins;
                break;

            case Constants._Currencies.extra_chance:
                balance = storeData.spent.extraChance;
                break;

            case Constants._Currencies.double_score:
                balance = storeData.spent.doubleScore;
                break;

            case Constants._Currencies.one_accelerator:
                balance = storeData.spent.oneAccelerator;
                break;

            case Constants._Currencies.five_accelerator:
                balance = storeData.spent.fiveAccelerator;
                break;

        }
        return balance;
    }

    /// <summary>
    /// Obtiene información del producto
    /// </summary>
    /// <returns>The product.</returns>
    /// <param name="productId">Product identifier.</param>
    /// 
    /*
    public Product GetProduct(string productId)
    {
        //------  OBTENER PRODUCTO POR PRODUCT ID
        _controller.products.WithID(productId);

        Product _p = null;

        if (_p != null)
        {
            return _p;
        }
        return null;
    }
    */

    /// <summary>
    /// Obtiene el precio localizado a la region
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public string GetLocalizedPrice(string productId)
    {
        /*
        Product _product = _controller.products.WithID(productId);

        if (_product != null)
        {
            ProductMetadata _meta = _product.metadata;
            return _meta.localizedPriceString;
        }
        */
        return string.Empty;
    }

    /// <summary>
    /// Regala una cantidad de objeto
    /// </summary>
    /// <param name="productType">El tipo de producto (life/diamond).</param>
    /// <param name="amount">Cantidad.</param>
    public void GiveProduct(Constants._Currencies productType, int amount)
    {
        switch (productType)
        {
            case Constants._Currencies.one_life:
                storeData.lifes += amount;
                break;

            case Constants._Currencies.diamond_currency:
                storeData.diamonds += amount;
                break;

            case Constants._Currencies.coin_currency:
                storeData.coins += amount;
                break;

            case Constants._Currencies.extra_chance:
                storeData.extraChance += amount;
                break;

            case Constants._Currencies.double_score:
                storeData.doubleScore += amount;
                break;

            case Constants._Currencies.one_accelerator:
                storeData.oneAccelerator += amount;
                break;

            case Constants._Currencies.five_accelerator:
                storeData.fiveAccelerator += amount;
                break;
        }
        ChangeValue(productType, amount);
        Save();
    }

    /// <summary>
    /// Quita una cantidad de objeto
    /// </summary>
    /// <param name="productType">El tipo de producto (life/diamond).</param>
    /// <param name="amount">Cantidad.</param>
    public void TakeProduct(Constants._Currencies productType, int amount)
    {
        switch (productType)
        {
            case Constants._Currencies.one_life:
                storeData.lifes -= amount;
                storeData.lifes = (int)Mathf.Clamp(storeData.lifes, 0f, Mathf.Infinity);
                break;

            case Constants._Currencies.diamond_currency:
                storeData.diamonds -= (int)amount;
                storeData.diamonds = (int)Mathf.Clamp(storeData.diamonds, 0f, Mathf.Infinity);
                break;

            case Constants._Currencies.coin_currency:
                storeData.coins -= (int)amount;
                storeData.coins = (int)Mathf.Clamp(storeData.coins, 0f, Mathf.Infinity);
                break;

            case Constants._Currencies.extra_chance:
                storeData.extraChance -= (int)amount;
                storeData.extraChance = (int)Mathf.Clamp(storeData.extraChance, 0f, Mathf.Infinity);
                break;

            case Constants._Currencies.double_score:
                storeData.doubleScore -= (int)amount;
                storeData.doubleScore = (int)Mathf.Clamp(storeData.doubleScore, 0f, Mathf.Infinity);
                break;

            case Constants._Currencies.one_accelerator:
                storeData.oneAccelerator -= (int)amount;
                storeData.oneAccelerator = (int)Mathf.Clamp(storeData.oneAccelerator, 0f, Mathf.Infinity);
                break;

            case Constants._Currencies.five_accelerator:
                storeData.fiveAccelerator -= (int)amount;
                storeData.fiveAccelerator = (int)Mathf.Clamp(storeData.fiveAccelerator, 0f, Mathf.Infinity);
                break;

        }
        ChangeValue(productType, (double)amount);

        _AddSpent(productType, amount);


        Save();
    }

    /// <summary>
    /// Compra productos utilizando dinero
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    public void BuyProductWithMarket(string productId)
    {
        /*
        Product _product = _controller.products.WithID(productId);

        if (_product != null && _product.availableToPurchase)
        {

            InitialicePurchase(productId);
        }
        else
        {
            PurchaseFail(BuyStatus.failed, FailedReason.ProductUnavailable, productId);
        }
        */
    }

    /// <summary>
    /// Compra productos utilizando virtual currencies
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    public void BuyProductWithCurrency(string productId, int price, Constants._Currencies payWith, Constants._Currencies boughtItem, int boughtAmount)
    {
        // --------------  PURCHASE SUCCESS --------------\\
        if (GetBalance(payWith) >= price)
        {
            GiveProduct(boughtItem, boughtAmount);

            TakeProduct(payWith, price);


            ChangeValue(boughtItem, boughtAmount);

            _AddBought(boughtItem, boughtAmount);

            PurchaseSuccess(BuyStatus.completed, FailedReason.None, productId);

            Save();
        }
        else
        {
            // --------------  PURCHASE FAILED --------------\\
            if (payWith == Constants._Currencies.coin_currency)
            {
                PurchaseFail(BuyStatus.failed, FailedReason.InsufficientCoins, productId);
            }
            else if (payWith == Constants._Currencies.diamond_currency)
            {
                PurchaseFail(BuyStatus.failed, FailedReason.InsufficientFounds, productId);
            }
        }
    }

    private void _AddBought(Constants._Currencies _bought, int amount)
    {
        switch (_bought)
        {
            case Constants._Currencies.one_life:
                storeData.bought.lifes += amount;
                break;

            case Constants._Currencies.diamond_currency:
                storeData.bought.diamonds += amount;
                break;

            case Constants._Currencies.coin_currency:
                storeData.bought.coins += amount;
                break;

            case Constants._Currencies.extra_chance:
                storeData.bought.extraChance += amount;
                break;

            case Constants._Currencies.double_score:
                storeData.bought.doubleScore += amount;
                break;

            case Constants._Currencies.one_accelerator:
                storeData.bought.oneAccelerator += amount;
                break;

            case Constants._Currencies.five_accelerator:
                storeData.bought.fiveAccelerator += amount;
                break;
        }
    }

    private void _AddSpent(Constants._Currencies _spent, int amount)
    {
        switch (_spent)
        {
            case Constants._Currencies.one_life:
                storeData.spent.lifes += amount;
                break;

            case Constants._Currencies.diamond_currency:
                storeData.spent.diamonds += amount;
                break;

            case Constants._Currencies.coin_currency:
                storeData.spent.coins += amount;
                break;

            case Constants._Currencies.extra_chance:
                storeData.spent.extraChance += amount;
                break;

            case Constants._Currencies.double_score:
                storeData.spent.doubleScore += amount;
                break;

            case Constants._Currencies.one_accelerator:
                storeData.spent.oneAccelerator += amount;
                break;

            case Constants._Currencies.five_accelerator:
                storeData.spent.fiveAccelerator += amount;
                break;
        }
    }
    #endregion

    #region FUNCIONES DATOS

    /// <summary>
    /// Guarda datos de tienda
    /// </summary>
    public void Save()
    {
        string json = JsonConvert.SerializeObject(storeData, Formatting.Indented, new ObscuredValueConverter());
        string coded = SaveSystem.EncodeData(json);
        //Debug.LogWarning(json);

        File.WriteAllText(storePath, json);
    }

    /// <summary>
    /// Carga datos de Tienda
    /// </summary>
    public void Load()
    {
        if (File.Exists(storePath))
        {
            string json = File.ReadAllText(storePath);
            string decoded = string.Empty;

            if (readDecoded)
            {
                decoded = SaveSystem.DecodeData(json);
            }

            else
            {
                decoded = json;
            }

            //string decoded = SaveSystem.DecodeData(json);
            storeData = JsonConvert.DeserializeObject<StoreData>(json, new ObscuredValueConverter());
        }
        else
        {
            storeData = new StoreData();
        }
    }

    #endregion



    #region INTERFACE

    /*
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extension = extensions;

            Debug.Log("In-App Purchasing OnInitialized: PASS");
            _initialized = true;
        }
        */

    /*
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("In-App Purchasing OnInitializeFailed. InitializationFailureReason:" + error);
        _initialized = false;

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        string _productId = product.definition.id;

        FailedReason _failedReason = FailedReason.Unknown;

        PurchaseFail(BuyStatus.failed, _failedReason, _productId);

    }

    public void OnPurchaseComplete(Product product)
    {
        Debug.LogError("C: " + product.definition.id);

    }

    
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        bool validPurchase = true;

        if (validPurchase)
        {
            Debug.Log("Processing purchase of product: " + e.purchasedProduct.transactionID);

            Product p = e.purchasedProduct;

            _controller.ConfirmPendingPurchase(p);

            string _productId = p.definition.id;

            Constants._Currencies _boughtItem = Constants._Currencies.none;
            int _boughtAmount = 0;

            switch (_productId)
            {
                case Constants.IAP_Constants.Product_Single_life:
                    _boughtItem = Constants._Currencies.one_life;
                    _boughtAmount = 1;

                    break;

                case Constants.IAP_Constants.Product_Small_lifes_bottle:
                    _boughtItem = Constants._Currencies.one_life;
                    _boughtAmount = 3;
                    break;

                case Constants.IAP_Constants.Product_Medium_lifes_bottle:
                    _boughtItem = Constants._Currencies.one_life;
                    _boughtAmount = 15;
                    break;

                case Constants.IAP_Constants.Product_Big_lifes_bottle:
                    _boughtItem = Constants._Currencies.one_life;
                    _boughtAmount = 25;
                    break;

                case Constants.IAP_Constants.Product_Small_diamond_bag:
                    _boughtItem = Constants._Currencies.diamond_currency;
                    _boughtAmount = 25;
                    break;

                case Constants.IAP_Constants.Product_Medium_diamond_bag:
                    _boughtItem = Constants._Currencies.diamond_currency;
                    _boughtAmount = 60;
                    break;

                case Constants.IAP_Constants.Product_Big_diamond_bag:
                    _boughtItem = Constants._Currencies.diamond_currency;
                    _boughtAmount = 100;
                    break;

                case Constants.IAP_Constants.Product_Small_coins_box:
                    _boughtItem = Constants._Currencies.coin_currency;
                    _boughtAmount = 400;
                    break;

                case Constants.IAP_Constants.Product_Medium_coins_box:
                    _boughtItem = Constants._Currencies.coin_currency;
                    _boughtAmount = 1000;
                    break;

                case Constants.IAP_Constants.Product_Big_coins_box:
                    _boughtItem = Constants._Currencies.coin_currency;
                    _boughtAmount = 1800;
                    break;
            }

            GiveProduct(_boughtItem, _boughtAmount);

            _AddBought(_boughtItem, _boughtAmount);

            PurchaseSuccess(BuyStatus.completed, FailedReason.None, e.purchasedProduct.definition.id);


        }

        else
        {
            PurchaseFail(BuyStatus.failed, FailedReason.None, e.purchasedProduct.definition.id);
        }

        return PurchaseProcessingResult.Complete;
    }
    */

    #endregion
}


