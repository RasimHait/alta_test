using System;

namespace AltaTestWork
{
    [Flags]
    public enum SceneEntityTag : byte
    {
        None = 0,
        Player = 1 << 0,
        Obstacle = 1 << 1,
        Bullet = 1 << 2
    }
}