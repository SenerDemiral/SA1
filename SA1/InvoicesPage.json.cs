using Starcounter;

namespace SA1
{
    partial class InvoicesPage : Page
    {
        public void RefreshData()
        {
            Invoices = Db.SQL("SELECT i FROM Invoice i");
        }

        // This attribute indicates which part of JSON tree 
        // is represented by this class
        [InvoicesPage_json.Invoices]
        partial class InvoicesItemPage
        {
            protected override void OnData()
            {
                base.OnData();
                this.Url = string.Format("/invoicedemo/invoices/{0}", this.InvoiceNo);
            }
        }
    }
}
