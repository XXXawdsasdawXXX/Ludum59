using System;
using System.Collections.Generic;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters
{
    public class Trigger : MonoBehaviour
    {
        public event Action<GameObject> Enter;
        public event Action<GameObject> Exit;
        public ReactiveProperty<bool> OnTrigger { get; } = new(false);

        public List<GameObject> ObjectsInTrigger { get; } = new List<GameObject>();
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            OnTrigger.PropertyValue = true;
            Enter?.Invoke(col.gameObject);
            ObjectsInTrigger.Add(col.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTrigger.PropertyValue = false;
            Exit?.Invoke(other.gameObject);
            ObjectsInTrigger.Add(other.gameObject);
        }
    }
}