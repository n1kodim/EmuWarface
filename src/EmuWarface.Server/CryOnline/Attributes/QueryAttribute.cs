namespace EmuWarface.Server.CryOnline.Attributes.Query
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