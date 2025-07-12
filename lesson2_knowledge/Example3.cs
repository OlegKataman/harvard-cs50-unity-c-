using System;
using UnityEngine;

namespace june.lessons.lesson2_knowledge
{
    public sealed class Example3 : MonoBehaviour
    {
        private Symbol _ron = new Symbol("ron in the great hall");
        private Symbol _hermione = new Symbol("hermione is in the library");
        private Symbol _harry = new Symbol("harry is sleeping");

        private void Start()
        {
            // Задаем знания (knowledge) как список дизъюнкций (Or)
            var knowledge = new And(new Or(_ron, _hermione), 
                new Or( new Not(_ron), _harry), 
                new Or(_hermione, _harry));
            
            // Проверяем, следует ли из knowledge, что Harry спит
            var isHarrySleeping = ModelCheck.Check(knowledge, _harry);
            Console.WriteLine($"Harry is sleeping: {isHarrySleeping}"); // Выведет True
        }
    }
}