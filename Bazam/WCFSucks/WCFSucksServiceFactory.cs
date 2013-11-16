using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web;

namespace Bazam.WCFSucks
{
    public class WCFSucksServiceFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            // declare servicey things
            string scheme = (HttpContext.Current == null ? baseAddresses[0].Scheme : HttpContext.Current.Request.Url.Scheme);
            ServiceHost host = new ServiceHost(serviceType, baseAddresses);
            ServiceMetadataBehavior metadata = new ServiceMetadataBehavior();
            
            // set up servicey things
            if (scheme == "https")
                metadata.HttpsGetEnabled = true;
            else
                metadata.HttpGetEnabled = true;
            host.Description.Behaviors.Add(metadata);

            foreach(Uri address in baseAddresses) {
                if(address.Scheme == scheme) {
                    BasicHttpBinding binding = new BasicHttpBinding();
                    if(scheme == "https") {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                    }

                    host.AddServiceEndpoint(serviceType, binding, address);
                    break;
                }
            }

            return host;
        }
    }
}
