using System.Diagnostics.CodeAnalysis;

namespace C8S.Common.Attributes;

// copied from the System.ComponentModel.DescriptionAttribute
[AttributeUsage(AttributeTargets.All)]
public class LabelAttribute : Attribute
{
    /// <summary>
    /// Specifies the default value for the <see cref='LabelAttribute'/>,
    /// which is an empty string (""). This <see langword='static'/> field is read-only.
    /// </summary>
    public static readonly LabelAttribute Default = new LabelAttribute();

    public LabelAttribute() : this(string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='LabelAttribute'/> class.
    /// </summary>
    public LabelAttribute(string label)
    {
        LabelValue = label;
    }

    /// <summary>
    /// Gets the label stored in this attribute.
    /// </summary>
    public virtual string Label => LabelValue;

    /// <summary>
    /// Read/Write property that directly modifies the string stored in the label
    /// attribute. The default implementation of the <see cref="Label"/> property
    /// simply returns this value.
    /// </summary>
    protected string LabelValue { get; set; }

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is LabelAttribute other && other.Label == Label;

    public override int GetHashCode() => Label?.GetHashCode() ?? 0;

    public override bool IsDefaultAttribute() => Equals(Default);
}