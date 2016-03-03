using System;
//Written by Ryan Serva
public class SegmentationException : Exception
{
    public SegmentationException()
    {

    }
    public SegmentationException(string message)
        : base(message)
    {
    }
    public SegmentationException(string message, Exception inner)
        : base(message, inner)
    {
    } 

}

