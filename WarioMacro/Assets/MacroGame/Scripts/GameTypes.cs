using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace GameTypes
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class SpecialistType
    {
        public const int Brute = 2; 
        public const int Alchemist = 3; 
        public const int Expert = 4; 
        public const int Ghost = 5; 
        public const int Acrobat = 6; 
        public const int Technomancer = 7;
        
        public static string[] GetTypeNames() => new[]
            {"Brute", "Alchemist", "Expert", "Ghost", "Acrobat", "Technomancer"};
        
        public static int DropdownAsRealValue(int dropdownValue) => dropdownValue + 2;
        
        public static int RealValueAsDropdown(int value) => value - 2;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class NodeDomainType : SpecialistType
    {
        public const int None = 0;
        public const int Random = 1;

        public new static string[] GetTypeNames()
        {
            string[] specialistTypeNames = SpecialistType.GetTypeNames();
            string[] typeNames = new string[2 + specialistTypeNames.Length];
            typeNames[0] = "None";
            typeNames[1] = "Random";
            for (var index = 0; index < specialistTypeNames.Length; index++)
            {
                typeNames[index + 2] = specialistTypeNames[index];
            }

            return typeNames;
        }
        
        public new static int DropdownAsRealValue(int dropdownValue) => dropdownValue;
        
        public new static int RealValueAsDropdown(int value) => value;
    }

    public enum NodeBehaviour
    {
        White,
        Drawer,
        Computer,
        Chest
    }
}
