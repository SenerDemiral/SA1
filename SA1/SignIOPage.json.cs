using System;
using Starcounter;

namespace SA1
{
    partial class SignIOPage : Json
    {
        void Handle(Input.sic action)
        {
            var u = Db.SQL<User>("SELECT u FROM User u WHERE u.FirstName = ? AND u.Password = ?", sin, sip).First;
            var mPage = Parent as MasterPage;
            if (u == null) {
                mPage.sia = false;
                mPage.sif = "";
                mPage.sii = "";
            }
            else
            {
                mPage.sia = true;
                mPage.sif = u.FullName;
                mPage.sii = u.GetObjectID();
                //mPage.SignIO = null;
                //mPage.siv = false;
            }
            siv = false;
            RedirectUrl = "/invoicedemo";
        }
        void Handle(Input.soc action)
        {
            sin = "sener";
            siv = false;
            var mPage = Parent as MasterPage;
            mPage.sia = false;
            RedirectUrl = "/invoicedemo";

        }
    }
}
