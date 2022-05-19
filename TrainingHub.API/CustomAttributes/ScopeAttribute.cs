namespace TrainingHub.API.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ScopeAttribute : Attribute
    {
        private string name;
        public ScopeAttribute(string name)
        {
            this.name = name;
        }
    }
}
