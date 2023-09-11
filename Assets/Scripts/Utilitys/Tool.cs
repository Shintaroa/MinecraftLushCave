using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Mathematics;
using static UnityEngine.Rendering.DebugUI;
using System;
using System.Reflection;

namespace Aquarium.Utility
{
    public class Tool
    {
        public static float random(Vector2 st)
        {
            return math.frac(sin(dot(st, new Vector2(112.9898f, 78.233f))) * 437.5453f);
        }

        public static float random(float st)
        {
            return math.frac(sin(st * 1312.928f) * 437.5453f);
        }

        public static bool filpACoin(float st)
        {
            return math.frac(sin(st * 712.18f) * 337.513f) > 0.5f ? true : false;
        }
        public static bool filpACoin(float st,float rate)
        {
            return math.frac(sin(st * 712.18f) * 337.513f) > rate ? true : false;
        }

    }
}