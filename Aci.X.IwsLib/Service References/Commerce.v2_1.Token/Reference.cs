﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aci.X.IwsLib.Commerce.v2_1.Token {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", ConfigurationName="Commerce.v2_1.Token.TokenServicesPortType")]
    public interface TokenServicesPortType {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iwscommerce.intelius.com/commerce/2.1/token.php/ProtectCreditCard", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardResponse ProtectCreditCard(Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://iwscommerce.intelius.com/commerce/2.1/token.php/ProtectCreditCard", ReplyAction="*")]
        System.Threading.Tasks.Task<Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardResponse> ProtectCreditCardAsync(Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iwscommerce.intelius.com/commerce/2.1/token.php/RetrievePartnerForm", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormResponse RetrievePartnerForm(Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://iwscommerce.intelius.com/commerce/2.1/token.php/RetrievePartnerForm", ReplyAction="*")]
        System.Threading.Tasks.Task<Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormResponse> RetrievePartnerFormAsync(Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl")]
    public partial class ClientAuthType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string clientIDField;
        
        private string authIDField;
        
        private string authKeyField;
        
        /// <remarks/>
        public string ClientID {
            get {
                return this.clientIDField;
            }
            set {
                this.clientIDField = value;
                this.RaisePropertyChanged("ClientID");
            }
        }
        
        /// <remarks/>
        public string AuthID {
            get {
                return this.authIDField;
            }
            set {
                this.authIDField = value;
                this.RaisePropertyChanged("AuthID");
            }
        }
        
        /// <remarks/>
        public string AuthKey {
            get {
                return this.authKeyField;
            }
            set {
                this.authKeyField = value;
                this.RaisePropertyChanged("AuthKey");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl")]
    public partial class ResponseDetailType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string messageField;
        
        private int subCodeField;
        
        private bool subCodeFieldSpecified;
        
        private string detailField;
        
        /// <remarks/>
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("Message");
            }
        }
        
        /// <remarks/>
        public int SubCode {
            get {
                return this.subCodeField;
            }
            set {
                this.subCodeField = value;
                this.RaisePropertyChanged("SubCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SubCodeSpecified {
            get {
                return this.subCodeFieldSpecified;
            }
            set {
                this.subCodeFieldSpecified = value;
                this.RaisePropertyChanged("SubCodeSpecified");
            }
        }
        
        /// <remarks/>
        public string Detail {
            get {
                return this.detailField;
            }
            set {
                this.detailField = value;
                this.RaisePropertyChanged("Detail");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl")]
    public partial class AuthContextType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int partnerIDField;
        
        private string authIDField;
        
        private string authKeyField;
        
        /// <remarks/>
        public int PartnerID {
            get {
                return this.partnerIDField;
            }
            set {
                this.partnerIDField = value;
                this.RaisePropertyChanged("PartnerID");
            }
        }
        
        /// <remarks/>
        public string AuthID {
            get {
                return this.authIDField;
            }
            set {
                this.authIDField = value;
                this.RaisePropertyChanged("AuthID");
            }
        }
        
        /// <remarks/>
        public string AuthKey {
            get {
                return this.authKeyField;
            }
            set {
                this.authKeyField = value;
                this.RaisePropertyChanged("AuthKey");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl")]
    public partial class CompletionResponseType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string completionCodeField;
        
        private string responseMessageField;
        
        private string responseDetailField;
        
        /// <remarks/>
        public string CompletionCode {
            get {
                return this.completionCodeField;
            }
            set {
                this.completionCodeField = value;
                this.RaisePropertyChanged("CompletionCode");
            }
        }
        
        /// <remarks/>
        public string ResponseMessage {
            get {
                return this.responseMessageField;
            }
            set {
                this.responseMessageField = value;
                this.RaisePropertyChanged("ResponseMessage");
            }
        }
        
        /// <remarks/>
        public string ResponseDetail {
            get {
                return this.responseDetailField;
            }
            set {
                this.responseDetailField = value;
                this.RaisePropertyChanged("ResponseDetail");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ProtectCreditCard", WrapperNamespace="https://api.intelius.com/commerce/2.1/wsdl", IsWrapped=true)]
    public partial class ProtectCreditCardRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=0)]
        public Aci.X.IwsLib.Commerce.v2_1.Token.ClientAuthType ClientAuth;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=1)]
        public string CreditCard;
        
        public ProtectCreditCardRequest() {
        }
        
        public ProtectCreditCardRequest(Aci.X.IwsLib.Commerce.v2_1.Token.ClientAuthType ClientAuth, string CreditCard) {
            this.ClientAuth = ClientAuth;
            this.CreditCard = CreditCard;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ProtectCreditCardResponse", WrapperNamespace="https://api.intelius.com/commerce/2.1/wsdl", IsWrapped=true)]
    public partial class ProtectCreditCardResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=0)]
        public Aci.X.IwsLib.Commerce.v2_1.Token.CompletionResponseType CompletionResponse;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=1)]
        public string Token;
        
        public ProtectCreditCardResponse() {
        }
        
        public ProtectCreditCardResponse(Aci.X.IwsLib.Commerce.v2_1.Token.CompletionResponseType CompletionResponse, string Token) {
            this.CompletionResponse = CompletionResponse;
            this.Token = Token;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RetrievePartnerForm", WrapperNamespace="https://api.intelius.com/commerce/2.1/wsdl", IsWrapped=true)]
    public partial class RetrievePartnerFormRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=0)]
        public Aci.X.IwsLib.Commerce.v2_1.Token.AuthContextType AuthContext;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=1)]
        public string AccessToken;
        
        public RetrievePartnerFormRequest() {
        }
        
        public RetrievePartnerFormRequest(Aci.X.IwsLib.Commerce.v2_1.Token.AuthContextType AuthContext, string AccessToken) {
            this.AuthContext = AuthContext;
            this.AccessToken = AccessToken;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RetrievePartnerFormResponse", WrapperNamespace="https://api.intelius.com/commerce/2.1/wsdl", IsWrapped=true)]
    public partial class RetrievePartnerFormResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=0)]
        public int ResponseCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=1)]
        public Aci.X.IwsLib.Commerce.v2_1.Token.ResponseDetailType ResponseDetail;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="https://api.intelius.com/commerce/2.1/wsdl", Order=2)]
        public string Payload;
        
        public RetrievePartnerFormResponse() {
        }
        
        public RetrievePartnerFormResponse(int ResponseCode, Aci.X.IwsLib.Commerce.v2_1.Token.ResponseDetailType ResponseDetail, string Payload) {
            this.ResponseCode = ResponseCode;
            this.ResponseDetail = ResponseDetail;
            this.Payload = Payload;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TokenServicesPortTypeChannel : Aci.X.IwsLib.Commerce.v2_1.Token.TokenServicesPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TokenServicesPortTypeClient : System.ServiceModel.ClientBase<Aci.X.IwsLib.Commerce.v2_1.Token.TokenServicesPortType>, Aci.X.IwsLib.Commerce.v2_1.Token.TokenServicesPortType {
        
        public TokenServicesPortTypeClient() {
        }
        
        public TokenServicesPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TokenServicesPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TokenServicesPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TokenServicesPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardResponse Aci.X.IwsLib.Commerce.v2_1.Token.TokenServicesPortType.ProtectCreditCard(Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardRequest request) {
            return base.Channel.ProtectCreditCard(request);
        }
        
        public Aci.X.IwsLib.Commerce.v2_1.Token.CompletionResponseType ProtectCreditCard(Aci.X.IwsLib.Commerce.v2_1.Token.ClientAuthType ClientAuth, string CreditCard, out string Token) {
            Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardRequest inValue = new Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardRequest();
            inValue.ClientAuth = ClientAuth;
            inValue.CreditCard = CreditCard;
            Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardResponse retVal = ((Aci.X.IwsLib.Commerce.v2_1.Token.TokenServicesPortType)(this)).ProtectCreditCard(inValue);
            Token = retVal.Token;
            return retVal.CompletionResponse;
        }
        
        public System.Threading.Tasks.Task<Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardResponse> ProtectCreditCardAsync(Aci.X.IwsLib.Commerce.v2_1.Token.ProtectCreditCardRequest request) {
            return base.Channel.ProtectCreditCardAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormResponse Aci.X.IwsLib.Commerce.v2_1.Token.TokenServicesPortType.RetrievePartnerForm(Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormRequest request) {
            return base.Channel.RetrievePartnerForm(request);
        }
        
        public int RetrievePartnerForm(Aci.X.IwsLib.Commerce.v2_1.Token.AuthContextType AuthContext, string AccessToken, out Aci.X.IwsLib.Commerce.v2_1.Token.ResponseDetailType ResponseDetail, out string Payload) {
            Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormRequest inValue = new Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormRequest();
            inValue.AuthContext = AuthContext;
            inValue.AccessToken = AccessToken;
            Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormResponse retVal = ((Aci.X.IwsLib.Commerce.v2_1.Token.TokenServicesPortType)(this)).RetrievePartnerForm(inValue);
            ResponseDetail = retVal.ResponseDetail;
            Payload = retVal.Payload;
            return retVal.ResponseCode;
        }
        
        public System.Threading.Tasks.Task<Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormResponse> RetrievePartnerFormAsync(Aci.X.IwsLib.Commerce.v2_1.Token.RetrievePartnerFormRequest request) {
            return base.Channel.RetrievePartnerFormAsync(request);
        }
    }
}
