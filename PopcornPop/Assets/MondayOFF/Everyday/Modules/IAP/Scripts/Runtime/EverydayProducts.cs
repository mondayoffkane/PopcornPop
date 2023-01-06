using UnityEngine;

#if UNITY_PURCHASING
namespace MondayOFF
{
    // [CreateAssetMenu(menuName = "products", fileName = "Products", order = 10)]
    internal class EverydayProducts : ScriptableObject
    {
        [Header("Please enter Product IDs here")] public ProductData[] products = default;
    }

    [System.Serializable]
    internal class ProductData
    {
        public string productID;
        public UnityEngine.Purchasing.ProductType productType;
        public System.Action onPurchase
        {
            set
            {
                _onPurchase = value;
                if (someonePurchased)
                {
                    CompletePurchase();
                }
            }
        }
        public bool isRegistered => _onPurchase != null && _onPurchase.GetInvocationList().Length > 0;


        private System.Action _onPurchase = default;
        bool someonePurchased = false;

        public void CompletePurchase()
        {
            if (isRegistered)
            {
                _onPurchase.Invoke();
            }
            else
            {
                Debug.Log($"[EVERYDAY] {productID} CALLBACK IS NOT REGISTERED");
                someonePurchased = true;
            }
        }
    }
}

#else
namespace MondayOFF {
    internal class EverydayProducts : ScriptableObject { }
}
#endif