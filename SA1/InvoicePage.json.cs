using System;
using Starcounter;
/*
[InvoicePage_json.Items]
partial class BElement : Json
{
    protected override void HasChanged(Starcounter.Templates.TValue property)
    {
        //Transaction.Commit();
        base.HasChanged(property);

        ((this.Parent as Starcounter.Arr<BElement>).Parent as InvoicePage).Pending++;
    }
}
*/
partial class InvoicePage : Json, IBound<Invoice>
{
    public event EventHandler Saved;
    public event EventHandler Deleted;

    protected override void OnData()
    {
        base.OnData();
        Console.WriteLine("OnData ");
    }

    protected override void HasChanged(Starcounter.Templates.TValue property)
    {
        //Transaction.Commit();
        base.HasChanged(property);
        //Pending++;
        Console.WriteLine("Padding");
    }

    void Handle(Input.Name inp)
    {
        var a = inp.OldValue;
        var b = inp.Value;
        Pending++;
    }

    void Handle(Input.AddRow action)
    {
        new InvoiceRow()
        {
            Invoice = Data
        };
        
        Pending++;

        Items[0].Quantity = 2;
    }

    void Handle(Input.Save action)
    {
        bool isUnsavedInvoice = (InvoiceNo == 0); // A new invoice
        if (isUnsavedInvoice)
        {
            InvoiceNo = (int)Db.SQL<long>("SELECT max(i.InvoiceNo) FROM Invoice i").First + 1;
        }

        Transaction.Commit();
        OnSaved();
        
        Pending = 0;

        //redirect to the new URL
        RedirectUrl = "/invoicedemo/invoices/" + InvoiceNo;
    }

    void Handle(Input.Cancel action)
    {
        bool isUnsavedInvoice = (InvoiceNo == 0); // A new invoice
        Transaction.Rollback(); // Bir onceki Commit'den sonrakiler iptal
        if (isUnsavedInvoice)
        {
            Data = new Invoice();
        }
    }

    void Handle(Input.Delete action)
    {
        if (Data == null) // Nothing to delete.
            return;

        Invoice invoice = this.Data;

        // We have to clean reference from view-model to database object manually 
        // before this bug is fixed: https://github.com/Starcounter/Starcounter/issues/2814
        this.Data = null;

        foreach (var row in Data.Items)
        {
            row.Delete();
        }

        invoice.Delete();
        Transaction.Commit();
        OnDeleted();

        Pending = 0;

        RedirectUrl = "/invoicedemo"; //redirect to the home URL
    }

    protected void OnDeleted()
    {
        if (this.Deleted != null)
        {
            this.Deleted(this, EventArgs.Empty);
        }
    }

    protected void OnSaved()
    {
        if (this.Saved != null)
        {
            this.Saved(this, EventArgs.Empty);
        }
    }

    [InvoicePage_json.Items]
    partial class ItemsItem : Json, IBound<InvoiceRow>
    {
        protected override void HasChanged(Starcounter.Templates.TValue property)
        {
            //Transaction.Commit();
            base.HasChanged(property);

            //((this.Parent as Starcounter.Arr<ItemsItem>).Parent as InvoicePage).Pending++;
            var page = Parent.Parent as InvoicePage;
            page.Pending++;
        }


    }


}