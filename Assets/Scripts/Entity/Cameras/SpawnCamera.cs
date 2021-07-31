using System;
using UnityEngine;

namespace Spawn
{
    public class SpawnCamera : MonoBehaviour
    {
        public static event Action<GameObject> Triggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!"Chiusky".Equals(other.gameObject.tag)) return;
            Triggered?.Invoke(this.gameObject);
        }
    }
}