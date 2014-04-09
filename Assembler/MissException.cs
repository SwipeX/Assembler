using System;
//Written by Ryan Serva
public class MissException : Exception
{
    public MissException()
    {

    }
    public MissException(string message)
        : base(message)
    {
    }
    public MissException(string message, Exception inner)
        : base(message, inner)
    {
    }

}