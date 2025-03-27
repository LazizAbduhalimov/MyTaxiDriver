using System.Collections;
using System.Collections.Generic;
using Module.Bank;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Version.Controll.Branch
{
    public class FlagShower : MonoBehaviour
    {
        [FormerlySerializedAs("GameObject1")] public GameObject _flagTransfrom;
        private bool _relieved;

        private void Update()
        {
            if (_relieved) return;
            if (Bank.Coins == -10_000)
            {
                _flagTransfrom.SetActive(true);
                _relieved = true;
            }
        }
    }
}
