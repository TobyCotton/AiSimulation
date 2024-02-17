using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World
{
    // ~ public interface
    static World()
    {
        WorldState = new WorldStates();
    }

    public static World Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return WorldState;
    }

    // ~ private interface
    private World() { }

    private static readonly World instance = new World();
    private static WorldStates WorldState;
}
