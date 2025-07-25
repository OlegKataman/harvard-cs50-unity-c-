using System.Collections.Generic;
using UnityEngine;

namespace june.lessons.lesson5_optimization
{
    public class PerceptronDemo2 : MonoBehaviour
    {
        void Start()
        {
            // Упрощенные данные с целыми числами
            List<int[]> trainingData = new List<int[]>
            {
                new int[] { 3, 8, -2, 0 },  // поддельная (-1)
                new int[] { 4, 8, -2, -1 },  // поддельная (-1)
                new int[] { 3, -2, 1, 0 },  // настоящая (1)
                new int[] { 3, 9, -4, -3 },  // поддельная (-1)
                new int[] { 0, -4, 4, 0 },  // настоящая (1)
                new int[] { 4, 9, -3, -3 }   // поддельная (-1)
            };

            List<int> labels = new List<int> { -1, -1, 1, -1, 1, -1 };

            // Создаем и обучаем перцептрон
            var perceptron = this.gameObject.AddComponent<PerceptronInt>();
            perceptron.Initialize(trainingData[0].Length);
            perceptron.Train(trainingData, labels);

            // Тестируем
            int[] testSample1 = { 3, 8, -2, 0 };  // поддельная (ожидается -1)
            int[] testSample2 = { 2, -2, 2, 1 };  // настоящая (ожидается 1)

            Debug.Log("Test 1 (expected -1): " + perceptron.Predict(testSample1));
            Debug.Log("Test 2 (expected 1): " + perceptron.Predict(testSample2));
        }
    }
}