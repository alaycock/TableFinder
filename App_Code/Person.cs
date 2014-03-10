using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for Person
/// </summary>
public class Person
{
    public string name;
    public string school;
    public string phone;
    public string comment;
    public string email;


	public Person(string name, string email, string school, string phone, string comment)
	{
        this.name = name;
        this.email = email;
        this.school = school;
        this.phone = phone;
        this.comment = comment;
	}

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Person objAsPerson = obj as Person;
        if (objAsPerson == null)
            return false;
        else
            return Equals(objAsPerson);
    }

    public bool Equals(Person other)
    {
        if (other == null)
            return false;
        return (this.email.Equals(other.email));
    }

    public override string GetHashCode()
    {
        return this.email;
    }
}