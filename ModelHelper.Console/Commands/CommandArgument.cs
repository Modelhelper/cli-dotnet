namespace ModelHelper.Commands
{
    public class CommandArgument
    {
        public string Key { get; set; }
        public bool IsMandatory { get; set; }
        public bool ParameterMustExist { get; set; }
    }
}