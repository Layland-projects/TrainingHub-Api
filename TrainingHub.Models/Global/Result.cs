using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Models.Global
{
    public class Result
    {
        protected Result() { }

        public ResultStatus Status { get; protected set; }
        public ICollection<string> Errors { get; protected set; } = new List<string>();

        public static Result Success()
        {
            return new Result { Status = ResultStatus.Success };
        }

        public static Result Failure()
        {
            return new Result { Status = ResultStatus.Failed };
        }

        public static Result Failure(params string[] messages)
        {
            return new Result { Errors = messages.ToList(), Status = ResultStatus.Failed };
        }

        public static Result Error(params string[] errors)
        {
            var res = Failure(errors); 
            res.Status = ResultStatus.Error;
            return res;
        }

        public static Result Error(Exception ex)
        {
            return new Result
            {
                Errors = new List<string>
                {
                    ex.ToString(),
                },
                Status = ResultStatus.Error
            };
        }
    }

    public sealed class Result<T> : Result
    {
        private Result() : base()
        {

        }

        public T Data { get; set; }

        public static Result<T> SuccessFrom(T data)
        {
            return new Result<T> { Data = data, Status = ResultStatus.Success };
        }

        public static Result<T> FailureFrom(T data, params string[] messages)
        {
            return new Result<T> { Data = data, Status = ResultStatus.Failed, Errors = messages.ToList() };
        }

        public static Result<T> ErrorFrom(T data, params string[] messages)
        {
            return new Result<T> { Data = data, Status = ResultStatus.Error, Errors = messages.ToList() };
        }

        public static Result<T> ErrorFrom(T data, Exception ex)
        {
            return new Result<T>
            {
                Data = data,
                Status = ResultStatus.Error,
                Errors = new List<string>
                {
                    ex.ToString()
                }
            };
        }
    }
}
