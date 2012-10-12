using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.XPath;
using System.Xml;
using System.ServiceModel.Dispatcher;
using System.Security.Principal;

namespace Service
{
    class AuthorizationManager : ServiceAuthorizationManager
    {
        public override bool CheckAccess(OperationContext operationContext, ref Message message)
        {
            base.CheckAccess(operationContext, ref message);
            string action = operationContext.IncomingMessageHeaders.Action;

            if (action == "urn:msdnmag/IService/GetRoles")
            {
                // messags in WCF are always read-once
                // we create one copy to work with, and one copy to return back to the plumbing
                MessageBuffer buffer = operationContext.RequestContext.RequestMessage.CreateBufferedCopy(int.MaxValue);
                message = buffer.CreateMessage();
                
                // get the username vale using XPath
                XPathNavigator nav = buffer.CreateNavigator();
                StandardNamespaceManager nsm = new StandardNamespaceManager(nav.NameTable);
                nsm.AddNamespace("msdn", "urn:msdnmag");

                XPathNavigator node = 
                    nav.SelectSingleNode("s:Envelope/s:Body/msdn:GetRoles/msdn:username", nsm);
                string parameter = node.InnerXml;

                // check authorization
                if (operationContext.ServiceSecurityContext.PrimaryIdentity.Name == parameter)
                {
                    return true;
                }
                else
                {
                    return (GetPrincipal(operationContext).IsInRole("administrators"));
                }
            }
            
            return true;
        }

        private IPrincipal GetPrincipal(OperationContext operationContext)
        {
            return operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;
        }
    }

    public class StandardNamespaceManager : XmlNamespaceManager
    {
        public StandardNamespaceManager(XmlNameTable nameTable)
            : base(nameTable)
        {
            this.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
            this.AddNamespace("s11", "http://schemas.xmlsoap.org/soap/envelope/");
            this.AddNamespace("s12", "http://www.w3.org/2003/05/soap-envelope");
            this.AddNamespace("wsaAugust2004", "http://schemas.xmlsoap.org/ws/2004/08/addressing");
            this.AddNamespace("wsa10", "http://www.w3.org/2005/08/addressing");
        }
    }
}
