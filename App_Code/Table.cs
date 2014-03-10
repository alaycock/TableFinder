using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Table
/// </summary>
public class Table
{
    public List<Chair> chairs;
    public int tableNumber;

	public Table(int tableNumber)
	{
        this.chairs = new List<Chair>();
        this.tableNumber = tableNumber;
	}

    public bool addSeat(Chair newChair)
    {
        if (isFull())
            return false;
        chairs.Add(newChair);
        return true;
    }

    public int seatsTaken()
    {
        return chairs.Count;
    }

    public bool isFull()
    {
        return chairs.Count == Config.SEATS_PER_TABLE;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Table objAsTable = obj as Table;
        if (objAsTable == null)
            return false;
        else
            return Equals(objAsTable);
    }

    public bool Equals(Table other)
    {
        if (other == null)
            return false;
        return (this.tableNumber.Equals(other.tableNumber));
    }

    public override int GetHashCode()
    {
        return this.tableNumber;
    }
}