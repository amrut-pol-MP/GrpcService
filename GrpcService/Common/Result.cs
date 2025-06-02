using Microsoft.AspNetCore.Mvc;

namespace GrpcService.Common
{
    public class Result<TResultValue> : EmptyResult
    {
        public TResultValue? Value { get; }

        protected Result()
        {
        }

        public Result(TResultValue value)
            : this()
        {
            Value = value;
        }
    }
}
