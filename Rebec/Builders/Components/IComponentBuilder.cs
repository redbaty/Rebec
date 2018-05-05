namespace Rebec.Builders.Components
{
    public interface IComponentBuilder
    {
        string Build(IReportBuilderContext builderContext, object value);
    }
}