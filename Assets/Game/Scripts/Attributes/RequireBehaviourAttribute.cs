using System;
using UnityEngine;

namespace AltaTestWork
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireBehaviour : Attribute
    {
        public Type[] Types { get; private set; } = Array.Empty<Type>();

        public RequireBehaviour(params Type[] behaviours)
        {
            Types = behaviours;
        }
    }
}