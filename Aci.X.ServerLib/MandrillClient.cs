using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.ServerLib.Mandrill
{
  public class MandrillClient : ExternalApiClient
  {
    private string _strApiKey;

    public MandrillClient(CallContext context, string strApiKey)
      : base("Mandrill", context)
    {
      _strApiKey = strApiKey;
    }

    public DBUser DBUser
    {
      get { return _context.DBUser; }
    }

    public DBSite DBSite
    {
      get { return _context.DBSite; }
    }

    public static MandrillMessage NewMessage(
      CallContext context, 
      string strTemplateName, 
      string strVarName = null,
      object oVarValue = null)
    {
      var client = new MandrillClient(context, WebServiceConfig.MandrillApiKey);
      var msg = new MandrillMessage(client, strTemplateName);
      if (strVarName != null && oVarValue != null)
      {
        msg.AddVariable(strVarName, oVarValue);
      }
      return msg;
    }

    public string SendMessageByTemplate(string strTemplateName, MandrillMessage msg, IEnumerable<Type> knownTypes = null)
    {
      var root = new RootObject
      {
        key = _strApiKey,
        template_name = strTemplateName,
        message = msg,
        template_content = new List<MandrillVar>()
      };

      string strResponse = base.ExecuteApiRequest(
        strMethod: "POST",
        strBody: JsonObjectSerializer.Serialize(root, knownTypes),
        strResource: "https://mandrillapp.com/api/1.0/messages/send-template.json");
      return strResponse;
    }

    protected override HttpWebRequest GetApiRequest(string strMethod, string strBody, string strResource, string strQuery = null, string strAuthorization = null, params string[] strParams)
    {
      HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(String.Format(strResource, strParams));
      req.Method = strMethod;
      req.ContentType = "application/json";
      if ((strMethod == "POST" || strMethod == "PUT") && strBody != null)
      {
        byte[] tReqBytes = Encoding.UTF8.GetBytes(strBody);
        req.ContentLength = tReqBytes.Length;
        System.IO.Stream reqStream = req.GetRequestStream();
        reqStream.Write(tReqBytes, 0, tReqBytes.Length);
        reqStream.Close();
      }

      return req;
    }
  }


  [DataContract]
  public class MandrillMessage
  {
    [IgnoreDataMember]
    private MandrillClient _client;
    [IgnoreDataMember]
    private string _strTemplateName;
    [IgnoreDataMember]
    private MandrillRecipientVar _recipientVars;
    [IgnoreDataMember]
    private List<Type> _listKnownTypes = new List<Type>();

    #region Serialized members
    [DataMember(EmitDefaultValue = false)]
    public string html;
    [DataMember(EmitDefaultValue = false)]
    public string text;
    [DataMember(EmitDefaultValue = false)]
    public string subject;
    [DataMember(EmitDefaultValue = false)]
    public string from_email;
    [DataMember(EmitDefaultValue = false)]
    public string from_name;
    [DataMember(EmitDefaultValue = false)]
    public List<MandrillRecipient> to = new List<MandrillRecipient>();
    [DataMember(EmitDefaultValue = false)]
    public MandrillHeaders headers;
    [DataMember(EmitDefaultValue = false)]
    public bool important;
    [DataMember(EmitDefaultValue = false)]
    public object track_opens;
    [DataMember(EmitDefaultValue = false)]
    public object track_clicks;
    [DataMember(EmitDefaultValue = false)]
    public object auto_text;
    [DataMember(EmitDefaultValue = false)]
    public object auto_html;
    [DataMember(EmitDefaultValue = false)]
    public object inline_css;
    [DataMember(EmitDefaultValue = false)]
    public object url_strip_qs;
    [DataMember(EmitDefaultValue = false)]
    public object preserve_recipients;
    [DataMember(EmitDefaultValue = false)]
    public object view_content_link;
    [DataMember(EmitDefaultValue = false)]
    public string bcc_address;
    [DataMember(EmitDefaultValue = false)]
    public object tracking_domain;
    [DataMember(EmitDefaultValue = false)]
    public object signing_domain;
    [DataMember(EmitDefaultValue = false)]
    public object return_path_domain;
    [DataMember(EmitDefaultValue = false)]
    public bool merge;
    [DataMember(EmitDefaultValue = false)]
    public string merge_language;
    [DataMember(EmitDefaultValue = false)]
    public List<MandrillVar> global_merge_vars = new List<MandrillVar>();
    [DataMember(EmitDefaultValue = false)]
    public List<MandrillRecipientVar> merge_vars =
      new List<MandrillRecipientVar>();
    [DataMember(EmitDefaultValue = false)]
    public List<string> tags = new List<string>();
    [DataMember(EmitDefaultValue = false)]
    public string subaccount;
    [DataMember(EmitDefaultValue = false)]
    public List<string> google_analytics_domains = new List<string>();
    [DataMember(EmitDefaultValue = false)]
    public string google_analytics_campaign;
    [DataMember(EmitDefaultValue = false)]
    public MandrillMetadata metadata;
    [DataMember(EmitDefaultValue = false)]
    public List<MandrillRecipientMetadata> recipient_metadata =
      new List<MandrillRecipientMetadata>();
    [DataMember(EmitDefaultValue = false)]
    public List<MandrillAttachment> attachments = new List<MandrillAttachment>();
    [DataMember(EmitDefaultValue = false)]
    public List<MandrillImage> images = new List<MandrillImage>();
    #endregion // Serialized Members

    public MandrillMessage()
    {
      
    }

    public MandrillMessage(MandrillClient client, string strTemplateName)
    {
      _client = client;
      _strTemplateName = strTemplateName;
      auto_text = true;
      inline_css = true;
      merge = true;
      merge_language = "handlebars";
      merge_vars = new List<MandrillRecipientVar>();
      to.Add(new MandrillRecipient
      {
        email = client.DBUser.EmailAddress,
        name = client.DBUser.FirstName + " " + client.DBUser.LastName
      });

      global_merge_vars.Add(new MandrillVar
      {
        name = "SiteName",
        content = client.DBSite.SiteName
      });
      global_merge_vars.Add(new MandrillVar
      {
        name = "BaseUrl",
        content = client.DBSite.BaseUrl
      });
      _recipientVars = new MandrillRecipientVar
      {
        rcpt = client.DBUser.EmailAddress
      };
      _recipientVars.vars.Add(new MandrillVar
      {
        name = "FirstName",
        content = client.DBUser.FirstName
      });
      _recipientVars.vars.Add(new MandrillVar
      {
        name = "LastName",
        content = client.DBUser.LastName
      });
      merge_vars.Add(_recipientVars);

    }

    public void AddVariable(string strVarName, object oVarValue)
    {
      _listKnownTypes.Add(oVarValue.GetType());
      _recipientVars.vars.Add(new MandrillVar
      {
        name = strVarName,
        content = oVarValue
      });
    }

    public void Send()
    {
      _client.SendMessageByTemplate(
        strTemplateName: _strTemplateName,
        msg: this,
        knownTypes: _listKnownTypes);
    }
  }

  [DataContract]
  public class MandrillRecipient
  {
    [DataMember(EmitDefaultValue = false)] public string email;
    [DataMember(EmitDefaultValue = false)] public string name;
    [DataMember(EmitDefaultValue = false)] public string type;
  }

  [DataContract]
  public class MandrillHeaders
  {
    [DataMember(Name = "Reply-To", EmitDefaultValue = false)] public string ReplyTo;
  }

  [DataContract]
  public class MandrillVar
  {
    [DataMember(EmitDefaultValue = false)] public string name;
    [DataMember(EmitDefaultValue = false)] public object content;
  }

  [DataContract]
  public class MandrillRecipientVar
  {
    [DataMember(EmitDefaultValue = false)] public string rcpt;
    [DataMember(EmitDefaultValue = false)] public List<MandrillVar> vars = new List<MandrillVar>();
  }

  [DataContract]
  public class MandrillMetadata
  {
    [DataMember(EmitDefaultValue = false)] public string website;
  }

  [DataContract]
  public class MandrillValues
  {
    [DataMember(EmitDefaultValue = false)] public int user_id;
  }

  [DataContract]
  public class MandrillRecipientMetadata
  {
    [DataMember(EmitDefaultValue = false)] public string rcpt;
    [DataMember(EmitDefaultValue = false)] public MandrillValues values;
  }

  [DataContract]
  public class MandrillAttachment
  {
    [DataMember(EmitDefaultValue = false)] public string type;
    [DataMember(EmitDefaultValue = false)] public string name;
    [DataMember(EmitDefaultValue = false)] public string content;
  }

  [DataContract]
  public class MandrillImage
  {
    [DataMember(EmitDefaultValue = false)] public string type;
    [DataMember(EmitDefaultValue = false)] public string name;
    [DataMember(EmitDefaultValue = false)] public string content;
  }

  [DataContract]
  public class RootObject
  {
    [DataMember(EmitDefaultValue = false)] public string key;
    [DataMember(EmitDefaultValue = false)] public MandrillMessage message;
    [DataMember(EmitDefaultValue = false)] public bool async;
    [DataMember(EmitDefaultValue = false)] public string ip_pool;
    [DataMember(EmitDefaultValue = false)] public string send_at;
    [DataMember(EmitDefaultValue = false)] public string template_name;
    [DataMember(EmitDefaultValue = false)] public List<MandrillVar> template_content = new List<MandrillVar>();
  }
}

