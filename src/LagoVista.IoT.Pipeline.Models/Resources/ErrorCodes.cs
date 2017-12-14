using LagoVista.IoT.Logging;
using LagoVista.IoT.Pipeline.Models.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin.Resources
{
    public class ErrorCodes
    {
        public static ErrorCode CouldNotLoadListenerConfig => new ErrorCode() { Code = "SLN2001", Message = PipelineAdminResources.Err_CouldNotLoadListener };
        public static ErrorCode CouldNotLoadInputTranslator => new ErrorCode() { Code = "SLN2002", Message = PipelineAdminResources.Err_CouldNotLoadInputTranslator };
        public static ErrorCode CouldNotLoadPlanner => new ErrorCode() { Code = "SLN2003", Message = PipelineAdminResources.Err_CouldNotLoadPlanner };
        public static ErrorCode CouldNotLoadSentinel => new ErrorCode() { Code = "SLN2004", Message = PipelineAdminResources.Err_CouldNotLoadSentinel };
        public static ErrorCode CouldNotLoadOutputTranslator => new ErrorCode() { Code = "SLN2005", Message = PipelineAdminResources.Err_CouldNotLoadOutputTranslator };
        public static ErrorCode CouldNotLoadTransmitter => new ErrorCode() { Code = "SLN2006", Message = PipelineAdminResources.Err_CouldNotLoadTransmitter };
        public static ErrorCode CouldNotLoadCustomModule => new ErrorCode() { Code = "SLN2007", Message = PipelineAdminResources.Err_CouldNotLoadCustomModule };
    }
}
