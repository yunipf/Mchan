using System;
using System.Runtime.InteropServices;
using UPNPLib;

namespace Mchan
{
    class UPnPControlPoint
    {
        
        private UPnPService Service { get; set; }

        private UPnPDevice GetDevice(IUPnPDeviceFinder finder, string typeUri)
        {
            foreach (UPnPDevice item in finder.FindByType(typeUri, 0))
            {
                return item;
            }
            return null;
        }

        private UPnPDevice GetDevice(IUPnPDeviceFinder finder)
        {
            UPnPDevice device = this.GetDevice(finder, "urn:schemas-upnp-org:service:WANPPPConnection:1");
            if (device == null)
            {
                device = this.GetDevice(finder, "urn:schemas-upnp-org:service:WANIPConnection:1");
            }
            return device;
        }

        private UPnPService GetService(UPnPDevice device, string serviceId)
        {
            try
            {
                return device.Services[serviceId];
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private UPnPService GetService(UPnPDevice device)
        {
            UPnPService service = this.GetService(device, "urn:upnp-org:serviceId:WANPPPConn1");
            if (service == null)
            {
                service = this.GetService(device, "urn:upnp-org:serviceId:WANIPConn1");
            }
            return service;
        }
        
        private UPnPService GetService()
        {
            UPnPDevice device = this.GetDevice(new UPnPDeviceFinder());
            if (device == null)
            {
                return null;
            }
            return this.GetService(device);
        }
        
        public UPnPControlPoint()
        {
            this.Service = GetService();
        }

        private object InvokeAction(string bstrActionName, object vInActionArgs)
        {
            if (Service == null)
            {
                return null;
            }
            try
            {
                object result = new object();
                Service.InvokeAction(bstrActionName, vInActionArgs, ref result);
                return result;
            }
            catch (COMException)
            {
                return null;
            }
        }

        public string GetExternalIPAddress()
        {
            object result = InvokeAction("GetExternalIPAddress", new object[] { });
            if (result == null)
            {
                return null;
            }
            return (string)((object[])result)[0];
        }

        public void AddPortMapping(string remoteHost, ushort externalPort, string protocol, ushort internalPort, string internalClient, string description)
        {
            var arguments = new object[] { remoteHost, externalPort, protocol, internalPort, internalClient, true, description, 0 };
            InvokeAction("AddPortMapping", arguments);
        }

        public void DeletePortMapping(string remoteHost, ushort externalPort, string protocol)
        {
            var arguments = new object[] { remoteHost, externalPort, protocol };
            InvokeAction("DeletePortMapping", arguments);
        }
    }

}
