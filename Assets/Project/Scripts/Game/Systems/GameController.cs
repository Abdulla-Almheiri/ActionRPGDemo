using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Systems
{
    public abstract class GameController : MonoBehaviour, IInitializable
    {
        public Game Game { get; protected set; }

        public virtual bool Initialize(Game game)
        {
            Game = game;
            if(Game == null)
            {
                return false;
            }
            return true;
        }

    }

}