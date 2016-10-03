using Starcounter;

namespace SA1
{
    partial class MasterPage : Page
    {
        protected override void OnData() {
            base.OnData();
        }

        protected override void HasChanged(Starcounter.Templates.TValue property) {
            base.HasChanged(property);
        }
        /*
        [MasterPage_json.Sener]
        partial class MasterItemPage {
            protected override void OnData() {
                base.OnData();
//                this.Url = string.Format("/invoicedemo/invoices/{0}", this.InvoiceNo);
            }

            protected override void HasChanged(Starcounter.Templates.TValue property) {
                base.HasChanged(property);
            }

        }*/
    }
}
