﻿using System;
//Written by Ryan Serva
public class SyntaxErrorException:Exception
{
	public SyntaxErrorException()
	{

	}
    public SyntaxErrorException(string message)
        : base(message){
    }
    public SyntaxErrorException(string message, Exception inner)
        : base(message, inner){
    }

}
