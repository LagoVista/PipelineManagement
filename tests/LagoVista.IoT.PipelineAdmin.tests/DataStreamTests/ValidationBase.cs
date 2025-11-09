// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 355448187a2ba0244e3d7b63f98429ac414c77239131f7a0a14b91ad1bc733ef
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Linq;
using LagoVista.Core.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    public class ValidationBase
    {
        protected void AssertInvalidError(ValidationResult result, params string[] errs)
        {
            Console.WriteLine("Errors (at least some are expected)");

            foreach (var err in result.Errors)
            {
                Console.WriteLine(err.Message);
            }

            foreach (var err in errs)
            {
                Assert.IsTrue(result.Errors.Where(msg => msg.Message == err).Any(), $"Could not find error [{err}]");
            }

            Assert.AreEqual(errs.Length, result.Errors.Count, "Validation error mismatch between");

            Assert.IsFalse(result.Successful, "Validated as successful but should have failed.");
        }

        protected void AssertSuccessful(Core.Validation.ValidationResult result)
        {
            if (result.Errors.Any())
            {
                Console.WriteLine("unexpected errors");
            }

            foreach (var err in result.Errors)
            {
                Console.WriteLine("\t" + err.Message);
            }

            Assert.IsTrue(result.Successful);
        }

    }
}
