using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace GameTypes
{
    public class GameType
    {
        public const int Brute = 2; 
        public const int Alchemist = 3; 
        public const int Expert = 4; 
        public const int Ghost = 5; 
        public const int Acrobat = 6; 
        public const int Technomancer = 7;
    }
    
    public class CharacterType : GameType
    {
        public const int Scoundrel = 1; 
    }

    [UsedImplicitly]
    public class NodeType : GameType
    {
        public const int None = 0;
        public const int Random = 1;
    }
}
