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

        public bool ValidateAccessor(DescriberEnum validAccessors)
        {
            ushort ushortValue = (ushort)value;
            ushort accessorValue = (ushort)validAccessors;

            for (byte i = 0; i < 16; i++)
                if (ReadBit(ushortValue, i) && !ReadBit(accessorValue, i))
                    return false;

            return true;
        }

        private bool ReadBit(ushort number, byte index)
        {
            int bit = 1;
            for (ushort i = 0; i < index; i++)
                bit = bit * 2; 

            return (number & bit) == bit;
        }
    }
}
