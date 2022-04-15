using System;

namespace CloudDataProtection.Core.Result
{
    public class BusinessResult<TResult>
    {
        public bool Success { get; protected set; }
        public TResult Data { get; protected set; }
        public string Message { get; protected set; }
        public ResultError? ErrorType { get; protected set; }

        public static BusinessResult<TResult> Error(string message)
        {
            return new BusinessResult<TResult>
            {
                Success = false,
                Message = message,
                ErrorType = ResultError.Unknown
            };
        }
        
        public static BusinessResult<TResult> Conflict(string message)
        {
            return new BusinessResult<TResult>
            {
                Success = false,
                Message = message,
                ErrorType = ResultError.Conflict
            };
        }

        public static BusinessResult<TResult> NotFound(string message)
        {
            return new BusinessResult<TResult>
            {
                Success = false,
                Message = message,
                ErrorType = ResultError.NotFound
            };
        }
        
        public static BusinessResult<TResult> Error(string message, TResult data)
        {
            return new BusinessResult<TResult>
            {
                Success = false,
                Data = data,
                Message = message,
                ErrorType = ResultError.Unknown
            };
        }

        public static BusinessResult<TResult> Ok(TResult data)
        {
            return new BusinessResult<TResult>
            {
                Success = true,
                Data = data
            };
        }

        public static BusinessResult<TResult> Ok()
        {
            return new BusinessResult<TResult>
            {
                Success = true
            };
        }
    }

    public class BusinessResult : BusinessResult<Object>
    {
        public new static BusinessResult Ok()
        {
            return new BusinessResult
            {
                Success = true
            };
        }

        public new static BusinessResult Error(string message)
        {
            return new BusinessResult
            {
                Success = false,
                Message = message,
                ErrorType = ResultError.Unknown
            };
        }
    }
}