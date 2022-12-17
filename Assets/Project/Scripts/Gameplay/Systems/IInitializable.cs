using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos;

namespace Chaos.Systems
{
    public interface IInitializable
    {
        Game Game { get; }
        bool Initialize(Game game);
    }
}