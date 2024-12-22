using System;
using System.Linq;
using Client.Game;
using LGrid;
using UnityEngine;

namespace Core.Scripts.Game
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private MapIniter mapIniter;
        [SerializeField] private LoadSavings LoadSavings;
        // [SerializeField] private

        private void Start()
        {
            mapIniter.Init();
            LoadSavings.Init();
        }

        private void Update()
        {
            foreach (var c in Map.Instance.Cells)
            {
                if (c.Value.TaxiBase != null) Debug.DrawRay(c.Key, Vector3.up, Color.red);
            }
        }
    }
}