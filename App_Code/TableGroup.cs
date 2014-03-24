using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Table
/// </summary>
[Serializable]
public class TableGroup
{
    public List<Chair> chairs;
    public int tableNumber;

    public TableGroup(int tableNumber)
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

    public int seatsAvailable()
    {
        return Config.SEATS_PER_TABLE - seatsTaken();
    }

    public bool isFull()
    {
        return chairs.Count == Config.SEATS_PER_TABLE;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        TableGroup objAsTable = obj as TableGroup;
        if (objAsTable == null)
            return false;
        else
            return Equals(objAsTable);
    }

    public bool Equals(TableGroup other)
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