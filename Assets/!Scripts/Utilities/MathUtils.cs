using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Shir0.Math
{
    public static class MathUtils
    {
        public static float NearestMultiple(float v, float denominator)
        {
            return Mathf.Round(v / denominator) * denominator;
        }
    }
}