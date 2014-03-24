using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Chari
/// </summary>
[Serializable]
public class Chair
{
    public Person occupant;

	public Chair(Person occupant)
	{
        this.occupant = occupant;
	}
}