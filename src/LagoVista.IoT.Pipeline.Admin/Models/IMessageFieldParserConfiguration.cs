using LagoVista.Core.Models;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public interface IMessageFieldParserConfiguration
    {
        string Id { get; }
        EntityHeader<ParserStrategies> ParserStrategy { get; }
        string Key { get; }
        string KeyName { get; }
        string XPath { get; }
        string ValueName { get; }
        int DelimitedColumnIndex { get; }
        int StartIndex { get; }
        int Length { get; }



        string RegExLocator { get; }
        string RegExGroupName { get; }
        string RegExValidation { get; }
        string Delimiter { get; }
        bool QuotedText { get; }
    }
}
