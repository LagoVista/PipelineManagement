using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Resources;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public enum ParserStrategies
    {
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeRegEx, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_RegEx, typeof(PipelineAdminResources))]
        RegEx,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeJsonProperty, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_JsonProperty, typeof(PipelineAdminResources))]
        JSONProperty,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeXmlProperty, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_XMLProperty, typeof(PipelineAdminResources))]
        XMLProperty,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeDelimitedColumn, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_Delimited, typeof(PipelineAdminResources))]
        DelimitedColumn,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypePosition, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_Position, typeof(PipelineAdminResources))]
        Position,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeHeader, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_Header, typeof(PipelineAdminResources))]
        Header,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeHeaderWithRegEx, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_RegExHeader, typeof(PipelineAdminResources))]
        HeaderWithRegEx,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeScript, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_Script, typeof(PipelineAdminResources))]
        Script,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeUriPath, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_URIPath, typeof(PipelineAdminResources))]
        UriPath,
        [EnumLabel(MessageFieldParserConfiguration.ParserTypeUriQueryString, PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_QueryString, typeof(PipelineAdminResources))]
        QueryString,
    }

    public enum ParseBinaryValueType
    {
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_String, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_String, typeof(PipelineAdminResources))]
        String,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Boolean, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Boolean, typeof(PipelineAdminResources))]
        Boolean,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Char, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Char, typeof(PipelineAdminResources))]
        Char,

        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_UInt8, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_UInt8, typeof(PipelineAdminResources))]
        UInt8,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Int8, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Int8, typeof(PipelineAdminResources))]
        Int8,


        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_UInt16LittleEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_UInt16LittleEndian, typeof(PipelineAdminResources))]
        UInt16LittleEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_UInt16BigEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_UInt16BigEndian, typeof(PipelineAdminResources))]
        UInt16BigEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Int16LittleEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Int16LittleEndian, typeof(PipelineAdminResources))]
        Int16LittleEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Int16BigEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Int16BigEndian, typeof(PipelineAdminResources))]
        Int16BigEndian,

        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_UInt32LittleEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_UInt32LittleEndian, typeof(PipelineAdminResources))]
        UInt32LittleEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_UInt32BigEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_UInt32BigEndian, typeof(PipelineAdminResources))]
        UInt32BigEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Int32LittleEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Int32LittleEndian, typeof(PipelineAdminResources))]
        Int32LittleEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Int32BigEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Int32BigEndian, typeof(PipelineAdminResources))]
        Int32BigEndian,

        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_UInt64LittleEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_UInt64LittleEndian, typeof(PipelineAdminResources))]
        UInt64LittleEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_UInt64BigEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_UInt64BigEndian, typeof(PipelineAdminResources))]
        UInt64BigEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Int64LittleEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Int64LittleEndian, typeof(PipelineAdminResources))]
        Int64LittleEndian,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_Int64BigEndian, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_Int64BigEndian, typeof(PipelineAdminResources))]
        Int64BigEndian,

        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_SinglePrecisionFloatingPoint, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_SinglePrecisionFloatingPoint, typeof(PipelineAdminResources))]
        SinglePrecisionFloatingPoint,
        [EnumLabel(MessageFieldParserConfiguration.ParserBinaryType_DoublePrecisionFloatingPoint, PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_DoublePrecisionFloatingPoint, typeof(PipelineAdminResources))]
        DoublePrecisionFloatingPoint,
    }

    public enum ParseOutputType
    {
        [EnumLabel(MessageFieldParserConfiguration.ParseOutputType_String, PipelineAdminResources.Names.MessageFieldParserConfiguration_OutputType_String, typeof(PipelineAdminResources))]
        String,
        [EnumLabel(MessageFieldParserConfiguration.ParseOutputType_integer, PipelineAdminResources.Names.MessageFieldParserConfiguration_OutputType_Integer, typeof(PipelineAdminResources))]
        Integer,
        [EnumLabel(MessageFieldParserConfiguration.ParseOutputType_floatingpoint, PipelineAdminResources.Names.MessageFieldParserConfiguration_OutputType_FloatingPoint, typeof(PipelineAdminResources))]
        FloatingPoint,
        [EnumLabel(MessageFieldParserConfiguration.ParseOutputType_boolean, PipelineAdminResources.Names.MessageFieldParserConfiguration_OutputType_Boolean, typeof(PipelineAdminResources))]
        Boolean
    }

    public enum StringParsingType
    {
        [EnumLabel(MessageFieldParserConfiguration.StringParser_NullTerminated, PipelineAdminResources.Names.MessageFieldParserConfiguration_StringParserType_LeadingLength, typeof(PipelineAdminResources))]
        LeadingLength,
        [EnumLabel(MessageFieldParserConfiguration.StringParser_LeadingLength, PipelineAdminResources.Names.MessageFieldParserConfiguration_StringParserType_NullTerminated, typeof(PipelineAdminResources))]
        NullTerminated
    }

    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.MessageFieldParserConfiguration_Title, PipelineAdminResources.Names.MessageFieldParserConfiguration_Help, PipelineAdminResources.Names.MessageFieldParserConfiguration_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class MessageFieldParserConfiguration : IMessageFieldParserConfiguration
    {
        public const string ParserTypeRegEx = "regex";
        public const string ParserTypeJsonProperty = "jsonproperty";
        public const string ParserTypeXmlProperty = "xmlproperty";
        public const string ParserTypeDelimitedColumn = "delimitedcolumn";
        public const string ParserTypePosition = "position";
        public const string ParserTypeHeader = "header";
        public const string ParserTypeHeaderWithRegEx = "headerwithregex";
        public const string ParserTypeScript = "script";
        public const string ParserTypeUriPath = "uripath";
        public const string ParserTypeUriQueryString = "querystring";

        public const string ParserBinaryType_String = "string";
        public const string ParserBinaryType_Boolean = "boolean";
        public const string ParserBinaryType_Char = "char";

        public const string ParserBinaryType_UInt8 = "uint8";
        public const string ParserBinaryType_Int8 = "int8";

        public const string ParserBinaryType_UInt16LittleEndian = "uint16littleendian";
        public const string ParserBinaryType_UInt16BigEndian = "uint16bigendian";
        public const string ParserBinaryType_Int16LittleEndian = "int16littleendian";
        public const string ParserBinaryType_Int16BigEndian = "int16bigendian";

        public const string ParserBinaryType_UInt32LittleEndian = "uint32littleendian";
        public const string ParserBinaryType_UInt32BigEndian = "uint32bigendian";
        public const string ParserBinaryType_Int32BigEndian = "int32bigendian";
        public const string ParserBinaryType_Int32LittleEndian = "int32littleendian";

        public const string ParserBinaryType_UInt64LittleEndian = "uint64littleendian";
        public const string ParserBinaryType_UInt64BigEndian = "uint64Bigendian";
        public const string ParserBinaryType_Int64LittleEndian = "int64littleendian";
        public const string ParserBinaryType_Int64BigEndian = "int64bigendian";

        public const string ParserBinaryType_SinglePrecisionFloatingPoint = "singleprecisionfloatingpoint";
        public const string ParserBinaryType_DoublePrecisionFloatingPoint = "dingleprecisionfloatingpoint";

        public const string ParseOutputType_String = "string";
        public const string ParseOutputType_integer = "integer";
        public const string ParseOutputType_floatingpoint = "floatingpoint";
        public const string ParseOutputType_boolean = "boolean";

        public const string StringParser_NullTerminated = "nullterminated";
        public const string StringParser_LeadingLength = "leadinglength";


        public string Id { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy, EnumType: (typeof(ParserStrategies)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_Select, HelpResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_Help, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<ParserStrategies> ParserStrategy { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy, EnumType: (typeof(ParseBinaryValueType)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_SelectDataType, HelpResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_BinaryType_SelectDataType_Help, IsRequired: false, IsUserEditable: true)]
        public EntityHeader<ParseBinaryValueType> BinaryDataType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy, EnumType: (typeof(ParseOutputType)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.MessageFieldParserConfiguration_OutputType_SelectDataType, HelpResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_OutputType_SelectDataType_Help, IsRequired: false, IsUserEditable: true)]
        public EntityHeader<ParseOutputType> OutputDataType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_StringParserType, EnumType: (typeof(ParseOutputType)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.MessageFieldParserConfiguration_StringParserType_Select, HelpResource: PipelineAdminResources.Names.MessageFieldParserConfiguration_StringParserType_Help, IsRequired: false, IsUserEditable: true)]
        public EntityHeader<StringParsingType> StringParsingType { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_StringParser_NumberBytes, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_StringParser_NumberBytes_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int StringLengthBytes { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_Name, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Name { get; set; }


        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_Key, HelpResource: Resources.PipelineAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.PipelineAdminResources.Names.Common_Key_Validation, ResourceType: typeof(PipelineAdminResources), IsRequired: true)]
        public string Key { get; set; }


        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_DelimitedColumnIndex, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_DelimitedColumnIndex_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int DelimitedColumnIndex { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_StartIndex, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_SubString_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int StartIndex { get; set; }
        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_Length, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_SubString_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public int Length { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_Delimiter, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_Delimitor_Help, FieldType: FieldTypes.Text, MaxLength: 2, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Delimiter { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_RegExLocator, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_RegExLocator_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string RegExLocator { get; set; }
        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_RegExGroupName, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_RegExGroupName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string RegExGroupName { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_PathLocator, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_PathLocator_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string PathLocator { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_QueryStringField, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_ParserStrategy_QueryStringField_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string QueryStringField { get; set; }



        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_QuotedText, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_QuotedText_Help, FieldType: FieldTypes.Bool, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public bool QuotedText { get; set; }


        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_KeyName, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_KeyName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string KeyName { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_XPath, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_XPath_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string XPath { get; set; }


        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_ValueName, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_ValueName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string ValueName { get; set; }


        /// <summary>
        /// A RegEx Value Used to Validate the Value Found is indeed the one that is expected.
        /// </summary>
        [FormField(LabelResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_RegExValidation, HelpResource: Resources.PipelineAdminResources.Names.MessageFieldParserConfiguration_RegExValidation_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string RegExValidation { get; set; }

        /// <summary>
        /// The Order within the list 
        /// </summary>
        /* Will be maintained programatically */
        public int Priority { get; set; }

        [FormField(LabelResource: Resources.PipelineAdminResources.Names.Common_Notes, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public string Notes { get; set; }
    }
}
