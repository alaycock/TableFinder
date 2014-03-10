using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Chari
/// </summary>
public class Chair
{

    public Person occupant;
    public bool isTaken;

	public Chair(Person occupant)
	{
        this.occupant = occupant;
	}
}