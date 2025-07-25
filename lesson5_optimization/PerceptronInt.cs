using System.Collections.Generic;
using UnityEngine;

namespace june.lessons.lesson5_optimization
{
    public class PerceptronInt : MonoBehaviour
    {
        public float[] weights;
        public float learningRate = 0.1f;
        public int epochs = 100;

        public void Initialize(int inputSize)
        {
            weights = new float[inputSize + 1]; // +1 для bias
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = Random.Range(-1f, 1f);
            }
        }

        public void Train(List<int[]> trainingInputs, List<int> trainingLabels)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                int errors = 0;

                for (int i = 0; i < trainingInputs.Count; i++)
                {
                    int[] inputs = trainingInputs[i];
                    int target = trainingLabels[i];

                    int prediction = Predict(inputs);
                    int error = target - prediction;

                    if (error != 0) errors++;

                    // Обновляем bias и веса
                    weights[0] += learningRate * error;
                    for (int j = 0; j < inputs.Length; j++)
                    {
                        weights[j + 1] += learningRate * error * inputs[j];
                    }
                }

                if (errors == 0) break;
            }
        }

        private int Activation(float value)
        {
            return value >= 0 ? 1 : -1;
        }

        public int Predict(int[] inputs)
        {
            float sum = weights[0]; // bias
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * weights[i + 1];
            }
            return Activation(sum);
        }
    }
}