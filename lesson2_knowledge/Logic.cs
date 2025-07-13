using System;
using System.Collections.Generic;
using System.Linq;

namespace june.lessons
{
    // Базовый класс для всех логических выражений
    public abstract class LogicalExpression
    {
        public abstract bool Evaluate(Dictionary<string, bool> model);
        public abstract HashSet<string> Symbols();
        public abstract string ToString();
    }

    // Логический символ (переменная)
    public class Symbol : LogicalExpression
    {
        public string Name { get; }

        public Symbol(string name)
        {
            Name = name;
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            if (!model.ContainsKey(Name))
            {
                throw new Exception($"Variable {Name} not in model");
            }

            return model[Name];
        }

        public override HashSet<string> Symbols()
        {
            return new HashSet<string> { Name };
        }

        public override string ToString()
        {
            return Name;
        }
    }

    // Класс для предикатов (логика первого порядка)
    public class Predicate : LogicalExpression
    {
        public string Name { get; }
        public Symbol[] Arguments { get; }

        public Predicate(string name, params Symbol[] args)
        {
            Name = name;
            Arguments = args;
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            // Создаем ключ для предиката (например: "BelongsTo(Minerva,Gryffindor)")
            string key = $"{Name}({string.Join(",", Arguments.Select(a => a.Name))})";
            return model.TryGetValue(key, out bool value) && value;
        }

        public override HashSet<string> Symbols()
        {
            var symbols = new HashSet<string>();
            foreach (var arg in Arguments)
            {
                symbols.UnionWith(arg.Symbols());
            }

            return symbols;
        }

        public override string ToString() => $"{Name}({string.Join(",", Arguments.Select(a => a.Name))})";
    }

    // Логическое "НЕ"
    public class Not : LogicalExpression
    {
        public LogicalExpression Expression { get; }

        public Not(LogicalExpression expression)
        {
            Expression = expression;
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return !Expression.Evaluate(model);
        }

        public override HashSet<string> Symbols()
        {
            return Expression.Symbols();
        }

        public override string ToString()
        {
            return $"¬{Expression.ToString()}";
        }
    }

    // Логическое "И"
    public class And : LogicalExpression
    {
        public List<LogicalExpression> Expressions { get; }

        public And(params LogicalExpression[] expressions)
        {
            Expressions = expressions.ToList();
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return Expressions.All(expr => expr.Evaluate(model));
        }

        public override HashSet<string> Symbols()
        {
            var symbols = new HashSet<string>();
            foreach (var expr in Expressions)
            {
                symbols.UnionWith(expr.Symbols());
            }

            return symbols;
        }

        public override string ToString()
        {
            return $"({string.Join(" ∧ ", Expressions.Select(expr => expr.ToString()))})";
        }
    }

    // Логическое "ИЛИ"
    public class Or : LogicalExpression
    {
        public List<LogicalExpression> Expressions { get; }

        public Or(params LogicalExpression[] expressions)
        {
            Expressions = expressions.ToList();
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return Expressions.Any(expr => expr.Evaluate(model));
        }

        public override HashSet<string> Symbols()
        {
            var symbols = new HashSet<string>();
            foreach (var expr in Expressions)
            {
                symbols.UnionWith(expr.Symbols());
            }

            return symbols;
        }

        public override string ToString()
        {
            return $"({string.Join(" ∨ ", Expressions.Select(expr => expr.ToString()))})";
        }
    }

    // Логическая импликация (→)
    public class Implication : LogicalExpression
    {
        public LogicalExpression Antecedent { get; }
        public LogicalExpression Consequent { get; }

        public Implication(LogicalExpression antecedent, LogicalExpression consequent)
        {
            Antecedent = antecedent;
            Consequent = consequent;
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return !Antecedent.Evaluate(model) || Consequent.Evaluate(model);
        }

        public override HashSet<string> Symbols()
        {
            var symbols = new HashSet<string>();
            symbols.UnionWith(Antecedent.Symbols());
            symbols.UnionWith(Consequent.Symbols());
            return symbols;
        }

        public override string ToString()
        {
            return $"({Antecedent.ToString()} → {Consequent.ToString()})";
        }
    }

    // Логическая эквивалентность (↔)
    public class Biconditional : LogicalExpression
    {
        public LogicalExpression Left { get; }
        public LogicalExpression Right { get; }

        public Biconditional(LogicalExpression left, LogicalExpression right)
        {
            Left = left;
            Right = right;
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            return Left.Evaluate(model) == Right.Evaluate(model);
        }

        public override HashSet<string> Symbols()
        {
            var symbols = new HashSet<string>();
            symbols.UnionWith(Left.Symbols());
            symbols.UnionWith(Right.Symbols());
            return symbols;
        }

        public override string ToString()
        {
            return $"({Left} ↔ {Right})";
        }
    }

    // Проверка модели (истинность выражения в данной модели)
    public static class ModelCheck
    {
        public static bool Check(LogicalExpression knowledge, LogicalExpression query)
        {
            var symbols = new HashSet<string>();
            symbols.UnionWith(knowledge.Symbols());
            symbols.UnionWith(query.Symbols());

            return CheckAll(knowledge, query, symbols, new Dictionary<string, bool>());
        }

        private static bool CheckAll(LogicalExpression knowledge, LogicalExpression query,
            HashSet<string> symbols, Dictionary<string, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (knowledge.Evaluate(model))
                {
                    return query.Evaluate(model);
                }

                return true;
            }

            var remainingSymbols = new HashSet<string>(symbols);
            var symbol = remainingSymbols.First();
            remainingSymbols.Remove(symbol);

            // Проверяем оба варианта (true и false) для текущего символа
            var modelTrue = new Dictionary<string, bool>(model) { [symbol] = true };
            var modelFalse = new Dictionary<string, bool>(model) { [symbol] = false };

            return CheckAll(knowledge, query, remainingSymbols, modelTrue) &&
                   CheckAll(knowledge, query, remainingSymbols, modelFalse);
        }
    }
}