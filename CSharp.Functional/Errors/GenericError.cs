﻿namespace CSharp.Functional.Errors
{
    public class GenericError : Error
    {
        #region Properties

        override public string Message { get; }

        #endregion

        #region Constructors

        public GenericError(string message)
        {
            Message = message;
        }

        #endregion
    }
}
