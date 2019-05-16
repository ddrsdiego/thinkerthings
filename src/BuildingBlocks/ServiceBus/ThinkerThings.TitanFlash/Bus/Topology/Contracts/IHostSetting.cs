using System;

namespace ThinkerThings.TitanFlash.Bus.Topology.Contracts
{
    public interface IHostSetting
    {
        string HostName { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }
        string VirtualHost { get; }
        bool AutomaticRecoveryEnabled { get; }

        Uri BuildUri();
    }
}
