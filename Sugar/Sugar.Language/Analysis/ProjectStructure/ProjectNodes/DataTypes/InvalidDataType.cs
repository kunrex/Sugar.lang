using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

internal sealed class InvalidDataType : DataType
{
    public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.InvalidDataType; }

    public override Describer Describers { get => throw new NotImplementedException(); }
    public override IDataTypeCollection Parent { get => throw new NotImplementedException(); }
    public override ReferenceCollection References { get => throw new NotImplementedException(); }

    public InvalidDataType() : base(null)
    {
        
    }
    
    public InvalidDataType(string _name) : base(_name)
    {
        
    }
    
    public override IReferencable GetParent() { return null; }

    public override void ReferenceParent() { }

    public override void ReferenceParent(IReferencable parentReference) { }
    
    public override void SetParent(IDataTypeCollection typeParent) { }
    
    public override DataType TryFindDataType(string value) { return null; }
    public override IDataTypeCollection AddEntity(DataType dataType) { return null; }
    
    public override IReferencable[] GetChildReference(string value) { return null; }

    public override void ReferenceGeneric(GenericReference generic) { }

    public override bool ValidateDescribers() => false;
    public override bool MatchDescriber(Describer toMatch) => false;
}