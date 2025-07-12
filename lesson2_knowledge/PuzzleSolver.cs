using System.Collections.Generic;
using UnityEngine;

namespace june.lessons.lesson2_knowledge
{
    public class PuzzleSolver : MonoBehaviour
    {
        public void SolveHarryPotterPuzzle()
        {
            // 1. Определяем персонажей и факультеты
            string[] people = { "Gilderoy", "Pomona", "Minerva", "Horace" };
            string[] houses = { "Gryffindor", "Hufflepuff", "Ravenclaw", "Slytherin" };

            // 2. Создаем список всех выражений для базы знаний
            List<LogicalExpression> knowledgeExpressions = new List<LogicalExpression>();

            // 3. Добавляем правила для всех персонажей
            foreach (var person in people)
            {
                // 3.1. Каждый человек в ровно одном факультете
                List<LogicalExpression> personHouses = new List<LogicalExpression>();
                foreach (var house in houses)
                {
                    personHouses.Add(new Symbol($"{person}_{house}"));
                }

                knowledgeExpressions.Add(new Or(personHouses.ToArray()));

                // 3.2. Не может быть в нескольких факультетах одновременно
                for (int i = 0; i < houses.Length; i++)
                {
                    for (int j = i + 1; j < houses.Length; j++)
                    {
                        knowledgeExpressions.Add(
                            new Implication(
                                new Symbol($"{person}_{houses[i]}"),
                                new Not(new Symbol($"{person}_{houses[j]}")))
                        );
                    }
                }
            }

            // 4. Добавляем конкретные подсказки
            knowledgeExpressions.AddRange(new LogicalExpression[]
            {
                new Or(new Symbol("Minerva_Gryffindor"), new Symbol("Minerva_Ravenclaw")),
                new Not(new Symbol("Pomona_Slytherin")),
                new Or(new Symbol("Horace_Hufflepuff"), new Symbol("Horace_Slytherin"))
            });

            // 5. Создаем итоговую базу знаний
            var knowledge = new And(knowledgeExpressions.ToArray());

            // Перебираем все символы
            foreach (var person in people)
            {
                foreach (var house in houses)
                {
                    var symbol = new Symbol($"{person}_{house}");
                    if (ModelCheck.Check(knowledge, symbol))
                    {
                        Debug.Log($"{person} is in {house}");
                    }
                }
            }
        }
    }
}