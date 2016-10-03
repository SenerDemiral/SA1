﻿using System;
using Starcounter;

[Database]
public class Person
{
    public string FirstName;
    public string LastName { get; set; }
    public string FullName { get { return FirstName + " " + LastName; } }
}

[Database]
public class Dogs
{
    public string Name;
}

namespace SA1 { 
    class Program {
        static void Main() {
            Console.WriteLine("HelloWorldsssss");

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/invoicedemo", () => {
                MasterPage master;
                if (Session.Current != null) {
                    master = (MasterPage)Session.Current.Data;
                } else {
                    master = new MasterPage() {
                        Html = "/InvoiceDemo/MasterPage.html"
                    };
                    master.Session = new Session(SessionOptions.PatchVersioning);
                    master.RecentInvoices = new InvoicesPage() {
                       Html = "/InvoiceDemo/InvoicesPage.html"
                    };
                }

                ((InvoicesPage)master.RecentInvoices).RefreshData();
                master.FocusedInvoice = null;

                // Static
                // It's better if using Handle.GET("/invoicedemo/gmap", ...)
                master.GmapHtml = Db.Scope<GmapPage>(() => {
                    var page = new GmapPage() {
                        Html = "/InvoiceDemo/GmapPage.html"
                    };
                    page.Sener = Db.SQL<Gmap>("SELECT i FROM Gmap i");

                    return page;
                });

                return master;
            });
//---------------------------------------------------------
            Handle.GET("/invoicedemo/gmap", () => 
            {
                MasterPage master = Self.GET<MasterPage>("/invoicedemo");

                master.GmapHtml = Db.Scope<GmapPage>(() => 
                {
                    var page = new GmapPage() {
                        Html = "/InvoiceDemo/GmapPage.html"
                    };
                    page.Sener = Db.SQL<Gmap>("SELECT i FROM Gmap i");

                    return page;
                });

               //((GmapPage)master.GmapHtml).RefreshData(); //Static data, no need refresh

                return master;
            });
//---------------------------------------------------------
            Handle.GET("/invoicedemo/new-invoice", () => 
            {
                MasterPage master = Self.GET<MasterPage>("/invoicedemo");
                master.FocusedInvoice = Db.Scope<InvoicePage>(() => {
                    var page = new InvoicePage() {
                        Html = "/InvoiceDemo/InvoicePage.html",
                        Data = new Invoice()
                    };

                    page.Saved += (s, a) => {
                        ((InvoicesPage)master.RecentInvoices).RefreshData();
                    };

                    return page;
                });
                return master;
            });

            Handle.GET("/invoicedemo/invoices/{?}", (int InvoiceNo) => {
                MasterPage master = Self.GET<MasterPage>("/invoicedemo");
                master.FocusedInvoice = Db.Scope<InvoicePage>(() => {
                    var page = new InvoicePage() {
                        Html = "/InvoiceDemo/InvoicePage.html",
                        Data = Db.SQL<Invoice>("SELECT i FROM Invoice i WHERE InvoiceNo = ?", InvoiceNo).First
                    };

                    page.Saved += (s, a) => {
                        ((InvoicesPage)master.RecentInvoices).RefreshData();
                    };

                    page.Deleted += (s, a) => {
                        ((InvoicesPage)master.RecentInvoices).RefreshData();
                    };

                    return page;
                });
                return master;
            });
            
            Db.Transact(() => {
                Db.SlowSQL("DELETE FROM Gmap");

                new Gmap() {
                    Lat = "37.779",
                    Lng = "-122.3892",
                    Inf = "A1"
                };

                new Gmap() {
                    Lat = "37.804",
                    Lng = "-122.2711",
                    Inf = "B2"
                };

                new Gmap() {
                    Lat = "37.386",
                    Lng = "-122.0837",
                    Inf = "C3"
                };

            });


            /*
            Handle.GET("/invoicedemo", () => {
                InvoicePage page;
                if (Session.Current != null)
                {
                    page = (InvoicePage)Session.Current.Data;
                }
                else
                {
                    page = Db.Scope<InvoicePage>(() => {
                        return new InvoicePage()
                        {
                            Html = "/InvoiceDemo/InvoicePage.html",
                            Data = new Invoice()
                        };
                    });
                    page.Session = new Session(SessionOptions.PatchVersioning);
                }
                return page;
            });
            */

            //string gcd = System.IO.Directory.GetCurrentDirectory();
            //Console.WriteLine("HelloWorldsssss" + gcd);

            //string lines = "First line.\r\nSecond line.\r\nThird line.";
            //System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Starcounter\\test.txt");
            //file.WriteLine(lines);
            //file.Close();

            /*
            Db.Transact(() =>
            {
                for (int i = 0; i < 100000; i++)
                    {
                        Person prs = new Person();
                        prs.FirstName = "Can " + i.ToString();
                        prs.LastName = "DEMIRAL";

                    }
            });
            */
            /*
            Db.Transact(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Dogs d = new Dogs();
                    d.Name = "Balu " + i.ToString();
                }
            });
            */
        }
    }
}