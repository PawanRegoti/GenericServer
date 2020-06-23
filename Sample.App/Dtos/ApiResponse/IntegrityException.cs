using System;

namespace Sample.App.Models.ApiResponse
{
	//	Thrown when integrity constraint has been violated
	//	---
	//	Temporary implementation. Exceptions should be standardized
	public class IntegrityException : Exception
	{
		//	Used in model binding
		public string ErrorCode { get; }

		public string DeveloperHint { get; }

		public IntegrityException(string code, string message) : base(message)
		{
			ErrorCode = code;
		}

		public IntegrityException(string code, string message, string developerHint) : base(message)
		{
			ErrorCode = code;
			DeveloperHint = developerHint;
		}
	}
}
