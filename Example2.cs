using System.Collections.Generic;
using UnityEngine;

namespace june.lesson2_tue01
{
    public static class Inventory
    {
        public static bool HasItem(string key)
        {
            return true;
        }
    }

    public class Door
    {
        public bool IsLocked = true;
    }
    
    public class DoorController : MonoBehaviour
    {
        // Логические переменные
        private Symbol hasKey = new Symbol("hasKey");
        private Symbol isDoorLocked = new Symbol("isDoorLocked");
        private LogicalExpression canOpenDoor;

        void Start()
        {
            // Условие: "есть ключ И дверь не заперта"
            canOpenDoor = new And(hasKey, new Not(isDoorLocked));
        }

        void Update()
        {
            // Текущее состояние игры
            var model = new Dictionary<string, bool>
            {
                { "hasKey", Inventory.HasItem("Key") },
                { "isDoorLocked", GetComponent<Door>().IsLocked }
            };

            // Проверка условия
            if (canOpenDoor.Evaluate(model))
            {
                OpenDoor();
            }
        }

        void OpenDoor()
        {
            Debug.Log("Дверь открыта!");
        }
    }
}