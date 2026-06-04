using UnityEngine;

namespace cats
{
    public class TestReward : MonoBehaviour
    {
        public void Start()
        {
            CurrencyWallet.Instance.Add(100);
        }
    }
}