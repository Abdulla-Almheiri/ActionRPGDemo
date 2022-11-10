using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace Game.Systems
{
    public interface IInitializable
    {
        Game Game { get; }
        bool Initialize(Game game);
    }
}