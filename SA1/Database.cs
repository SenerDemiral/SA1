using System;
using Starcounter;

[Database]
public class User
{
    public string FirstName;
    public string LastName;
    public string Password;
    public string FullName { get { return FirstName + " " + LastName; } }
}

[Database]
public class Gmap
{
    public DateTime EXD;
    public string Lat;
    public string Lng;
    public string Inf;
}

[Database]
public class Invoice
{
    public int InvoiceNo;
    public string Name;
    public decimal Total {
        get {
            return Db.SQL<decimal>("SELECT sum(r.Total) FROM InvoiceRow r WHERE r.Invoice=?", this).First;
        }
    }
    public QueryResultRows<InvoiceRow> Items {
        get {
            return Db.SQL<InvoiceRow>("SELECT r FROM InvoiceRow r WHERE r.Invoice=?", this);
        }
    }

    public Invoice() {
        new InvoiceRow() {
            Invoice = this
        };
    }
}

[Database]
public class InvoiceRow {
    public Invoice Invoice;
    public string Description;
    public int Quantity;
    public decimal Price;
    public decimal Total {
        get {
            return Quantity * Price;
        }
    }

    public InvoiceRow() {
        Quantity = 1;
    }
}

[Database]
public class Customer
{
    public string Name;
}

public class PrivateCustomer : Customer
{
    public string Gender;
}

public class CorporateCustomer : Customer
{
    public string VatNumber;
}

[Database]
public class TA
{
    public string fa;
}

public class TB : TA
{
    public string fb;
}

public class TC : TB
{
    public string fc;
}
