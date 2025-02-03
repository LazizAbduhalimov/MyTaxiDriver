using System;
using Client.Game;
using PoolSystem.Alternative;
using UnityEngine;

namespace Client
{
    public class CarsPoolContainer : MonoBehaviour
    {
        public PoolMono<TaxiMb> Pool { get; private set; }

        [SerializeField] private int _poolCount = 5;
        [SerializeField] private bool _autoExpand = true;
        [SerializeField] private TaxiMb _poolObject;
        [SerializeField] private Transform _container;

        private void OnValidate()
        {
            if (_container == null)
                _container = transform;
        }

        private void Awake()
        {
            if (_poolObject == null)
                Debug.Log(name);
            Pool = new PoolMono<TaxiMb>(_poolObject, _poolCount, _container);
            Pool.AutoExpand = _autoExpand;
        }

        public TaxiMb GetFromPool(Vector3 position)
        {
            if (Pool.HasFreeElement(out var taxiMb))
            {
                taxiMb.transform.position = position;
                return taxiMb;
            }

            if (Pool.AutoExpand)
            {
                var created = Pool.CreateObject(true);
                created.transform.position = position;
                created.Init();
                return created;
            }

            throw new Exception($"There is no free element of type <{typeof(TaxiMb)}> in pool");
        }
    }
}