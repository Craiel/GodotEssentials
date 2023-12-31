namespace Craiel.Essentials.Exceptions;

using System;

public class IllegalStateException : Exception
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public IllegalStateException()
    {
    }

    public IllegalStateException(string message) : base(message)
    {
    }

    public IllegalStateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}