﻿// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System;
using System.Linq;

namespace ConsoleApp1ML.Model
{
    [CustomMappingFactoryAttribute(nameof(LabelMapping))]
    public class LabelMapping : CustomMappingFactory<LabelMappingInput, LabelMappingOutput>
    {
        // This is the custom mapping. We now separate it into a method, so that we can use it both in training and in loading.
        public static void Mapping(LabelMappingInput input, LabelMappingOutput output)
        {
            string[] label = new string[] { "cloudy", "raining", "sunny", "sunrise" };

            var values = input.output1.GetValues().ToArray();
            var maxVal = values.Max();
            var exp = values.Select(v => Math.Exp(v - maxVal));
            var sumExp = exp.Sum();

            exp.Select(v => (float)(v / sumExp)).ToArray();
            output.score = exp.Select(v => (float)(v / sumExp)).ToArray();

            var maxValue = output.score.Max();
            var maxValueIndex = Array.IndexOf(output.score, maxValue);
            output.label = label[maxValueIndex];
        }
        // This factory method will be called when loading the model to get the mapping operation.
        public override Action<LabelMappingInput, LabelMappingOutput> GetMapping()
        {
            return Mapping;
        }
    }
    public class LabelMappingInput
    {
        [ColumnName("output1")]
        [VectorType(1,4)]
        public VBuffer<float> output1;
    }
    public class LabelMappingOutput
    {

        [ColumnName("PredictedLabel")]
        public string label { get; set; }

        [ColumnName("Score")]
        public float[] score { get; set; }
    }
}

