namespace EmuWarface.Server.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryAttribute : Attribute
    {
        public QueryAttribute(params string[] names)
        {
            Names = names;
        }

        public string[] Names { get; }
    }
}