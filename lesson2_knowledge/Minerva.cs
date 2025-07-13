using UnityEngine;

namespace june.lessons.lesson2_knowledge
{
    public sealed class Minerva
    {
        private void Start()
        {
            var minerva = new Symbol("Minerva");
            var gryffindor = new Symbol("Gryffindor");
            
            var person = new Predicate("Person", minerva);
            var house = new Predicate("House", gryffindor);
            var belongsTo = new Predicate("BelongsTo", minerva, gryffindor);

            var knowledge = new And(
                person, // Person(Minerva)
                house, // House(Gryffindor)
                new Not(new Predicate("House", minerva)), // ¬House(Minerva)
                belongsTo // BelongsTo(Minerva, Gryffindor)
            );
            
            var check = ModelCheck.Check(knowledge, belongsTo);
            Debug.Log($"Minerva in Gryffindor: {check}");
        }
    }
}