using System;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Enums;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;


namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

internal abstract class ParentableDataType : DataType
{
    private IDataTypeCollection parent;
    public override IDataTypeCollection Parent { get => parent; }

    private readonly Describer describer;
    public override Describer Describers { get => describer; }

    private readonly ReferenceCollection references;
    public override ReferenceCollection References { get => references; }

    protected ParentableDataType(string _name, Describer _describer, ReferenceCollection _references) : base(_name)
    {
        describer = _describer;
        references = new ReferenceCollection(_references);
    }
    
    public override IReferencable GetParent() { return parent; }

    public override void ReferenceParent()
    {
        foreach (var child in children)
        {
            child.Value.ReferenceParent(this);
            child.Value.ReferenceParent();
        }
    }

    public override void ReferenceParent(IReferencable parentReference)
    {
        references.WithReference(parentReference);

        foreach (var child in children)
            child.Value.ReferenceParent(parentReference);
    }
    
    public override void SetParent(IDataTypeCollection typeParent)
    {
        if (parent != null)
            throw new DoubleParentAssignementException();

        parent = typeParent;

        foreach (var child in children)
            child.Value.SetParent(this);
    }

    public override DataType TryFindDataType(string value)
    {
        children.TryGetValue(value, out var val);
        return val;
    }
    
    public override IDataTypeCollection AddEntity(DataType dataType)
    {
        if (TryFindDataType(dataType.Name) != null)
            children.Add(dataType.Name, dataType);

        return this;
    }
    
    public override IReferencable[] GetChildReference(string value)
    {
        foreach (var child in children)
            if (child.Key == value)
                return new IReferencable[] { child.Value };

        return null;
    }

    public override void ReferenceGeneric(GenericReference generic)
    { 
        genericCount++;
        references.WithReference(generic);
    } 

    public override bool MatchDescriber(Describer toMatch) => Describer.ValidateDescriber(toMatch, describer);
    public override bool ValidateDescribers() => Describer.ValidateDescription(describer, DescriberEnum.AccessModifiers);
}