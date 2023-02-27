using System;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.Describers
{
    internal struct Describer
    {
        private readonly DescriberEnum value;

        public Describer(DescriberEnum _value)
        {
            value = _value;
        }

        public bool CheckDescription(DescriberEnum external)
        {
            ushort ushortValue = (ushort)value;
            ushort accessorValue = (ushort)external;

            for (byte i = 0; i < 16; i++)
                if (ReadBit(ushortValue, i) && !ReadBit(accessorValue, i))
                    return false;

            return true;
        }

        public bool ValidateDescriber(DescriberEnum validAccessors) => CheckDescription(validAccessors);

        private bool ReadBit(ushort number, byte index)
        {
            int bit = 1;
            for (ushort i = 0; i < index; i++)
                bit = bit * 2; 

            return (number & bit) == bit;
        }
    }
}
