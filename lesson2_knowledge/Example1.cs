using UnityEngine;

namespace june.lessons
{
    public sealed class Example1 : MonoBehaviour
    {
        private void Start()
        {
            // Пример: (A ∨ B) ∧ (¬B ∨ C) → (A ∨ C)
            var A = new Symbol("A");
            var B = new Symbol("B");
            var C = new Symbol("C");

            var expr1 = new Or(A, B);
            var expr2 = new Or(new Not(B), C);
            var knowledge = new And(expr1, expr2);
            var query = new Or(A, C);

            bool result = ModelCheck.Check(knowledge, query);
            Debug.Log($"Does {knowledge} entail {query}? {result}");
        }
    }
}