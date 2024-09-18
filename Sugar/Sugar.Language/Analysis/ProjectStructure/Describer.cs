using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

namespace Sugar.Language.Analysis.ProjectStructure
{
    internal struct Describer
    {
        private readonly DescriberEnum value;

        public Describer(DescriberEnum _value)
        {
            value = _value;
        }
        
        private static bool ReadBit(ushort number, byte index)
        {
            int bit = 1;
            for (ushort i = 0; i < index; i++)
                bit = bit * 2;

            return (number & bit) == bit;
        }
        
        public static bool ValidateDescriber(Describer describer1, Describer describer2)
        {
            return ValidateDescription(describer1, describer2.value);
        }
        
        public static bool ValidateDescription(Describer describer, DescriberEnum external)
        {
            ushort ushortValue = (ushort)describer.value;
            ushort accessorValue = (ushort)external;

            for (byte i = 0; i < 16; i++)
                if (ReadBit(ushortValue, i) && !ReadBit(accessorValue, i))
                    return false;

            return true;
        }
    }
}
