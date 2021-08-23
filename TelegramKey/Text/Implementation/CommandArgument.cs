namespace TelegramKey.Text.Implementation
{
    internal class CommandArgument : ICommandArgument
    {
        public string Value { get; }

        public CommandArgument(string value)
        {
            this.Value = value;
        }
    }
}