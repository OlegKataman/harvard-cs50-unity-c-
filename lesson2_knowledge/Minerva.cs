namespace june.lessons.lesson2_knowledge
{
    public class Minerva
    {
        private void Start()
        {
            var knowledge = new And(
                new Symbol("Person_Minerva"),
                new Symbol("House_Gryffindor"),
                new Not(new Symbol("House_Minerva")),
                new Symbol("BelongsTo_Minerva_Gryffindor")
            );

            // Проверка: принадлежит ли Минерва Гриффиндору?
            var check = ModelCheck.Check(knowledge, new Symbol("BelongsTo_Minerva_Gryffindor")); // true
        }
    }
}