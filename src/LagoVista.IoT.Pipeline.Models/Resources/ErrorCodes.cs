// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 826a793e53d6540cbaa0a99bb3a5a34cd4fe1ab523c8f49b63fc8a1cb5030ab2
// IndexVersion: 2
// --- END CODE INDEX META ---
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
        public static ErrorCode CouldNotLoadDataStreamModule => new ErrorCode() { Code = "SLN2008", Message = PipelineAdminResources.Err_CouldNotLoadDataStream };
    }
}
