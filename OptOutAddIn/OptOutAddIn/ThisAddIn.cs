using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace OptOutAddIn
{
  public partial class ThisAddIn
  {
    private Outlook.MailItem _mailItem;

    private void ThisAddIn_Startup(object sender, System.EventArgs e)
    {
      //Outlook.Inspectors inspectors = Application.Inspectors;
      //inspectors.NewInspector += inspectors_NewInspector;

      //Application.ItemLoad += Application_ItemLoad;


      //foreach (Outlook.Explorer explorer in Application.Explorers)
      //{
      //  explorer.SelectionChange += explorer_SelectionChange;
      //}
    }

    //void Application_ItemLoad(object Item)
    //{
    //  var mailItem = Item as Outlook.MailItem;
    //  if (mailItem != null)
    //  {
    //    _mailItem = mailItem;
    //    mailItem.Open += mailItem_Open;
    //    mailItem.Read += mailItem_Read;
    //  }
    //}

    //void mailItem_Read()
    //{
    //  string strSubject = _mailItem.Subject;
    //  string strBody = _mailItem.Body;
    //  string strSender = _mailItem.Sender.Address;
    //  DateTime dtSent = _mailItem.SentOn;



    //  const string PROP_NAME = "OptOutDate";

    //  if (strSender == "ahardwick@intelius.com")
    //  {
    //    Outlook.ItemProperty prop = _mailItem.ItemProperties[PROP_NAME];

    //    if (prop == null)
    //    {
    //      _mailItem.ItemProperties.Add(PROP_NAME, Outlook.OlUserPropertyType.olDateTime);
    //      _mailItem.ItemProperties[PROP_NAME].Value = DateTime.Now;
    //      _mailItem.Save();
    //    }
    //    DateTime dtOptOut = _mailItem.ItemProperties[PROP_NAME].Value;


    //    foreach (Outlook.Action action in _mailItem.Actions)
    //    {
    //      if (action.Name == "OPT OUT!")
    //        return;
    //    }
    //    _mailItem.CustomAction += _mailItem_CustomAction;

    //    Outlook.Action optOutAction = _mailItem.Actions.Add();
    //    optOutAction.Name = "OPT OUT!";
    //    optOutAction.ReplyStyle = Outlook.OlActionReplyStyle.olUserPreference;
    //    optOutAction.ResponseStyle = Outlook.OlActionResponseStyle.olOpen;
    //    optOutAction.CopyLike = Outlook.OlActionCopyLike.olReply;
    //    _mailItem.Save();
    //  }
    //}

    //void _mailItem_CustomAction(object Action, object Response, ref bool Cancel)
    //{
    //  int intNOP = 0;

    //}

    //void mailItem_Open(ref bool Cancel)
    //{
    //  string strSubject = _mailItem.Subject;
    //  string strBody = _mailItem.Body;
    //  Outlook.AddressEntry addr = _mailItem.Sender;
    //  string strSender = addr == null ? null : addr.Address;
    //}

    //void explorer_SelectionChange()
    //{
    //}

    //void inspectors_NewInspector(Outlook.Inspector inspector)
    //{
    //  var currentItem = inspector.CurrentItem;
    //  int intNOP = 0;
    //}

    private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
    {
    }

    #region VSTO generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
      this.Startup += new System.EventHandler(ThisAddIn_Startup);
      this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
    }

    #endregion
  }
}
