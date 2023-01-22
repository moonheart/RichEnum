namespace RichEnum.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Enum)]
    public class RichEnumAttribute: System.Attribute
    {
        public bool EnableLocalization { get; set; }
    
        public string? ResourceManager { get; set; }
    }
}