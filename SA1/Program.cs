using System;
using System.Diagnostics;
using Starcounter;
using SDB;  // SharedDB

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
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            long[] et = new long[10];

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/invoicedemo", (Request request) => {
                //Console.WriteLine("Request " + request.ToString());

                var sw = Stopwatch.StartNew();

                et[0] = sw.ElapsedTicks;

                MasterPage master;
                et[1] = sw.ElapsedTicks;

                if (Session.Current != null) {
                    master = (MasterPage)Session.Current.Data;
                } else {
                    master = new MasterPage() {
                        Html = "/InvoiceDemo/MasterPage.html"
                    };
                    master.Session = new Session(SessionOptions.PatchVersioning);
                    et[2] = sw.ElapsedTicks; 
                    master.RecentInvoices = new InvoicesPage() {
                        Html = "/InvoiceDemo/InvoicesPage.html"
                    };
                    et[3] = sw.ElapsedTicks; //pt.DurationMcs;
                }

                ((InvoicesPage)master.RecentInvoices).RefreshData();
                master.FocusedInvoice = null;
                //master.SIO.sia = true;
                //master.SIO.sin = "";
                /*
                // Static
                // It's better if using Handle.GET("/invoicedemo/gmap", ...)
                master.GmapHtml = Db.Scope<GmapPage>(() => {
                    var page = new GmapPage() {
                        Html = "/InvoiceDemo/GmapPage.html"
                    };
                    page.Sener = Db.SQL<Gmap>("SELECT i FROM Gmap i");

                    return page;
                });
                */

                et[4] = sw.ElapsedTicks; //pt.DurationMcs;

                double ticks = et[4];// - et[0];
                //double ets = ticks * 1000000.0 / Stopwatch.Frequency;
                double ets = ticks / Stopwatch.Frequency;
                double ets1 = Math.Floor(ets);
                double ets2 = Math.Floor(ets * 1000.0);
                double ets3 = Math.Floor(ets * 1000000.0) - ets2 * 1000.0;
                Console.WriteLine(string.Format("Elapsed: {0:n0}:{1:n0}:{2:n0} {3:n6}", ets1, ets2, ets3, ets));
                return master;
            });

            Handle.GET("/invoicedemo/signio", () => 
            {
                MasterPage master = Self.GET<MasterPage>("/invoicedemo");
                master.SignIO = Db.Scope<SignIOPage>(() => {
                    var page = new SignIOPage() {
                        //Html = "/InvoiceDemo/SignIOPage.html",
                        //Data = new Invoice()
                        siv = true
                };

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
            Handle.GET("/invoicedemo/new-invoice", (Request request) => 
            {
                //Console.WriteLine("Request " + request.ToString());

                MasterPage master = Self.GET<MasterPage>("/invoicedemo");

                if (master.sia) {
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
                }
                return master;
            });

            Handle.GET("/invoicedemo/invoices/{?}", (int InvoiceNo, Request request) => {
                //Console.WriteLine("Request " + request.ToString());
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
                Db.SlowSQL("DELETE FROM User");

                new User() {
                    FirstName = "Dilara",
                    LastName = "Demiral",
                    Password = "dilara"
                };

                new User() {
                    FirstName = "Can",
                    LastName = "Demiral",
                    Password = "can"
                };
            });
            
            /*
            Db.Transact(() => {
                Db.SlowSQL("DELETE FROM Gmap");

                new Gmap() {
                    EXD = DateTime.Now.AddHours(2.2),
                    Lat = "37.779",
                    Lng = "-122.3892",
                    Inf = "A1"
                };

                new Gmap() {
                    EXD = DateTime.Now.AddHours(3.3),
                    Lat = "37.804",
                    Lng = "-122.2711",
                    Inf = "B2"
                };

                new Gmap() {
                    EXD = DateTime.Now.AddHours(1.1),
                    Lat = "37.386",
                    Lng = "-122.0837",
                    Inf = "C3"
                };

            });
            
            Db.Transact(() => {
                IotOwner io1 = new IotOwner() {
                    Id = "22222222222",
                    Name = "Sener DEMIRAL"
                };
                IotOwner io2 = new IotOwner() {
                    Id = "11111111111",
                    Name = "Can DEMIRAL"
                };
                IotOwner io3 = new IotOwner() {
                    Id = "33333333333",
                    Name = "Dilara DEMIRAL"
                };
                Iot i1 = new Iot() {
                    iotOwner = io1,
                    Id = "i1"
                };
                Iot i2 = new Iot() {
                    iotOwner = io1,
                    Id = "i2"
                };

            });*/
            /*
                        Db.Transact(() => {
                            Db.SlowSQL("DELETE FROM Customer");

                            PrivateCustomer pc1 = new PrivateCustomer() {
                                Name = "Sener",
                                Gender = "M"
                            };
                            Customer pc2 = new Customer() {
                                Name = "Can"
                            };
                            CorporateCustomer pc3 = new CorporateCustomer() {
                                Name = "Dilara",
                                VatNumber = "12345"
                            };
                            CorporateCustomer pc4 = new CorporateCustomer() {
                                VatNumber = "99999"
                            };

                        });
            */
            Db.Transact(() => {
                //Db.SlowSQL("DELETE FROM TA");
                
                TA a1 = new TA() {
                    fa = "Aa",
                };
                TB b1 = new TB() {
                    fa = "Ba",
                    fb = "Bn"
                };
                TC c1 = new TC() {
                    fa = "Ca",
                    fb = "Cb",
                    fc = "Cc"
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