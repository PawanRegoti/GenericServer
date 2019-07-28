using System;

namespace Sample.Shared.Exceptions
{
  public class SampleException : Exception
  {
    public string DeveloperHint { get; set; }
    public string ErrorCode { get; set; }
    public string Parameter { get; set; }
    public string ParameterValue { get; set; }

    public SampleException(string message)
            : base(message) { }
    public SampleException(string message, string parameter, string errorCode, string parameterValue) : base(message)
    {
      Parameter = parameter;
      ErrorCode = errorCode;
      ParameterValue = parameterValue;
    }
  }
}
