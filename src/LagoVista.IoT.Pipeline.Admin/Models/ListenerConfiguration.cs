using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Resources;
using LagoVista.IoT.Pipeline.Admin.Resources;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public enum ListenerTypes
    {
        [EnumLabel(ListenerConfiguration.ListenerTypes_AzureServiceBus, PipelineAdminResources.Names.Connection_Type_AzureServiceBus, typeof(PipelineAdminResources))]
        AzureServiceBus,
        [EnumLabel(ListenerConfiguration.ListenerTypes_AzureServiceBus, PipelineAdminResources.Names.Connection_Type_AzureEventHub, typeof(PipelineAdminResources))]
        AzureEventHub,
        [EnumLabel(ListenerConfiguration.ListenerTypes_AzureIoTHub, PipelineAdminResources.Names.Connection_Type_AzureIoTHub, typeof(PipelineAdminResources))]
        AzureIoTHub,
        [EnumLabel(ListenerConfiguration.ListenerTypes_REST, PipelineAdminResources.Names.Connection_Type_Rest, typeof(PipelineAdminResources))]
        Rest,
        [EnumLabel(ListenerConfiguration.ListenerTypes_RawTCP, PipelineAdminResources.Names.Connection_Type_TCP, typeof(PipelineAdminResources))]
        RawTCP,
        [EnumLabel(ListenerConfiguration.ListenerTypes_RawUdp, PipelineAdminResources.Names.Connection_Type_UDP, typeof(PipelineAdminResources))]
        RawUDP,
        [EnumLabel(ListenerConfiguration.ListenerTypes_AMQP, PipelineAdminResources.Names.Connection_Type_AMQP, typeof(PipelineAdminResources))]
        AMQP,
        [EnumLabel(ListenerConfiguration.ListenerTypes_MQTT, PipelineAdminResources.Names.Connection_Type_MQTT, typeof(PipelineAdminResources))]
        MQTT,
        [EnumLabel(ListenerConfiguration.ListenerTypes_MQTT_Client, PipelineAdminResources.Names.Connection_Type_MQTT_Client, typeof(PipelineAdminResources))]
        MQTTHosted,
        /*
            Soap,
        
        [EnumLabel(ListenerConfiguration.ListenerTypes_SOAP, PipelineAdminResources.Names.Connection_Type_Soap, typeof(PipelineAdminResources))]
        
         * [EnumLabel(ListenerConfiguration.ListenerTypes_POP3Server, PipelineAdminResources.Names.Connection_Type_POP3Server, typeof(PipelineAdminResources))]
                POP3Server,
                [EnumLabel(ListenerConfiguration.ListenerTypes_Custom, PipelineAdminResources.Names.Connection_Type_Custom, typeof(PipelineAdminResources))]
                Custom*/
    }

    public enum MessageLengthSize
    {
        [EnumLabel(ListenerConfiguration.MessageLengthSize_One, PipelineAdminResources.Names.Listener_MessageLength_1_Byte, typeof(PipelineAdminResources))]
        OneByte,
        [EnumLabel(ListenerConfiguration.MessageLengthSize_Two, PipelineAdminResources.Names.Listener_MessageLength_2_Bytes, typeof(PipelineAdminResources))]
        TwoBytes,
        [EnumLabel(ListenerConfiguration.MessageLengthSize_Four, PipelineAdminResources.Names.Listener_MessageLength_4_Bytes, typeof(PipelineAdminResources))]
        FourBytes
    }


    [EntityDescription(PipelineAdminDomain.PipelineAdmin, PipelineAdminResources.Names.InputTranslator_Title, PipelineAdminResources.Names.InputTranslator_Help, PipelineAdminResources.Names.InputTranslator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(PipelineAdminResources))]
    public class ListenerConfiguration : PipelineModuleConfiguration
    {

        public const string MessageLengthSize_One = "one";
        public const string MessageLengthSize_Two = "two";
        public const string MessageLengthSize_Four = "four";

        public const string ListenerTypes_AzureServiceBus = "azureserivcebus";
        public const string ListenerTypes_AzureEventHub = "azureeventhub";
        public const string ListenerTypes_AzureIoTHub = "azureiothub";
        public const string ListenerTypes_REST = "rest";
        public const string ListenerTypes_SOAP = "soap";
        public const string ListenerTypes_RawTCP = "rawtcp";
        public const string ListenerTypes_RawUdp = "raw_udp";
        public const string ListenerTypes_AMQP = "amqp";
        public const string ListenerTypes_MQTT = "mqtt";
        public const string ListenerTypes_MQTT_Client = "mqttclient";
        public const string ListenerTypes_POP3Server = "pop3server";
        public const string ListenerTypes_Custom = "custom";

        public override string ModuleType => PipelineModuleType_Listener;


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ListenerType, EnumType: (typeof(ListenerTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(PipelineAdminResources), WaterMark: PipelineAdminResources.Names.Connection_Select_Type, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<ListenerTypes> ListenerType { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ListenOnPort, HelpResource: PipelineAdminResources.Names.Listener_Port_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int ListenOnPort { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_DelimitedWithSOH_EOT, HelpResource: PipelineAdminResources.Names.Listener_DelimitedWithSOH_EOT_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool DelimitedWithSOHEOT { get; set; }

        [FormField(LabelResource: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType, HelpResource: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType_Help, FieldType: FieldTypes.Picker, WaterMark: DeviceMessagingAdminResources.Names.DeviceMessage_ContentType_Select, EnumType: typeof(MessageContentTypes), ResourceType: typeof(DeviceMessagingAdminResources), IsRequired: true)]
        public EntityHeader<MessageContentTypes> ContentType { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MessageContainsLength, HelpResource: PipelineAdminResources.Names.Listener_MessageContainsLength_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool MessageLengthInMessage { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_Location, HelpResource: PipelineAdminResources.Names.Listener_Length_Location_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MessageLengthLocation { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_LocationByteLength, HelpResource: PipelineAdminResources.Names.Listener_Length_LocationByteLength_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_Length_Endiness_Select, EnumType: typeof(EndianTypes), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<MessageLengthSize> MessageLengthSize { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Length_Endiness, HelpResource: PipelineAdminResources.Names.Listener_Length_Endiness_Help, FieldType: FieldTypes.Picker, WaterMark: PipelineAdminResources.Names.Listener_MessageLength_Select, EnumType: typeof(MessageLengthSize), ResourceType: typeof(PipelineAdminResources), IsRequired: false)]
        public EntityHeader<EndianTypes> MessageLengthByteCountEndiness { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_UserName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string UserName { get; set; }



        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Password, FieldType: FieldTypes.Password, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Password { get; set; }

        public string SecurePasswordId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_HostName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string HostName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKeyName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKeyName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_AccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string AccessKey { get; set; }

        public string SecureAccessKeyId { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Subscription, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Subscription { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Topic, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Topic { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_Endpoint, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string Endpoint { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ResourceName, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string ResourceName { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_EventHub_ConsumerGroup, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string ConsumerGroup { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_ConnectToPort, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string ConnectToPort { get; set; }


        [FormField(LabelResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReply, FieldType: FieldTypes.CheckBox, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public bool KeepAliveToSendReply { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReply_Timeout, HelpResource: PipelineAdminResources.Names.Listener_KeepAliveToSendReplyTimeout_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int KeepAliveToSendReplyTimeoutMS { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_StartMessageSequence, HelpResource: PipelineAdminResources.Names.Listener_StartMessageSequence_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string StartMessageSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_EndMessageSequence, HelpResource: PipelineAdminResources.Names.Listener_EndMessageSequence_Help, FieldType: FieldTypes.Text, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public string EndMessageSequence { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MessageReceivedTimeout, HelpResource: PipelineAdminResources.Names.Listener_MessageReceivedTimeout_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MessageReceiveTimeoutMS { get; set; }

        [FormField(LabelResource: PipelineAdminResources.Names.Listener_MaxMessageSize, HelpResource: PipelineAdminResources.Names.Listener_MaxMessageSize_Help, FieldType: FieldTypes.Integer, ResourceType: typeof(PipelineAdminResources), IsRequired: false, IsUserEditable: true)]
        public int? MaxMessageSize { get; set; }
    }
}