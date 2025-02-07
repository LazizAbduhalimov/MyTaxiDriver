using System;
using UnityEngine;

namespace Core.Scripts
{
    public class Unit : MonoBehaviour
    {
        private Vector3 Destination;

        private void Start()
        {
            Destination = transform.position;
        }

        private void Update()
        {
            if (Destination != transform.position)
                transform.position = Vector3.MoveTowards(transform.position, Destination, 5 * Time.deltaTime);
        }

        public void Walk(Vector3 destination)
        {
            destination.y = 0.5f;
            Destination = destination;
            Debug.Log("Walk!");
        }
    }
}