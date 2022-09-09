using CSharp.Functional.Errors;

namespace BOC.Dtos
{
    public class ResultDto<T>
    {

        #region Properties

        public T Result { get; }
        public IEnumerable<Error> Errors { get; }
        public bool IsValid { get; }

        #endregion

        #region Constructors

        public ResultDto(T result)
        {
            if (result == null)
                throw new ArgumentNullException("result");
            IsValid = true;
            Result = result;
            Errors = new List<Error>();
        }

        public ResultDto(IEnumerable<Error> errs)
        {
            if (errs is null || errs.Count() == 0)
                throw new InvalidOperationException("Errors can't be empty");
            IsValid = false;
            Errors = errs;
            Result = default(T);
        }


        #endregion

        #region Methods

        public static implicit operator ResultDto<T>(T result) => new ResultDto<T>(result);
        public static implicit operator ResultDto<T>(List<Error> errs) => new ResultDto<T>(errs);

        #endregion





    }
}
