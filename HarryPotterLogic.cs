using UnityEngine;

namespace june.lesson2_tue01
{
    public sealed class HarryPotterLogic : MonoBehaviour
    {
        private void Start()
        {
            var rain = new Symbol("rain");
            var hagrid = new Symbol("hagrid");
            var dumbledore = new Symbol("dumbledore");

            var knowledge = new And(
                new Implication(new Not(rain), hagrid),
                new Or(hagrid, dumbledore),
                new Not(new And(hagrid, dumbledore)),
                dumbledore
            );

            Debug.Log("Знания: " + knowledge.ToString()); // Выведет: ((¬rain → hagrid) ∧ (hagrid ∨ dumbledore) ∧ ¬(hagrid ∧ dumbledore) ∧ dumbledore)
            Debug.Log("Результат: " + ModelCheck.Check(knowledge, rain)); // True
            Debug.Log("Результат: " + ModelCheck.Check(knowledge, dumbledore)); // True
            Debug.Log("Результат: " + ModelCheck.Check(knowledge, hagrid)); // False
            Debug.Log("Результат: " + ModelCheck.Check(knowledge, new Or(hagrid, dumbledore))); // True
        }
    }
}