using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace GameTypes
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class GameType
    {
        public const int Brute = 2; 
        public const int Alchemist = 3; 
        public const int Expert = 4; 
        public const int Ghost = 5; 
        public const int Acrobat = 6; 
        public const int Technomancer = 7;
    }
    
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class CharacterType : GameType
    {
        public const int Scoundrel = 1;

        public static string[] GetTypeNames() => new[]
            {"Scoundrel", "Brute", "Alchemist", "Expert", "Ghost", "Acrobat", "Technomancer"};

        public static int DropdownAsRealValue(int dropdownValue) => dropdownValue + 1;
        
        public static int RealValueAsDropdown(int value) => value - 1;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class NodeType : GameType
    {
        public const int None = 0;
        public const int Random = 1;

        public static string[] GetTypeNames() => new[]
            {"None", "Random", "Brute", "Alchemist", "Expert", "Ghost", "Acrobat", "Technomancer"};
        
        public static int DropdownAsRealValue(int dropdownValue) => dropdownValue;
        
        public static int RealValueAsDropdown(int value) => value;
    }
}
