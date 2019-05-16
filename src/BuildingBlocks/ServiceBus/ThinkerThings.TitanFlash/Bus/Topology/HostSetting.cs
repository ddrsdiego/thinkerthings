using System;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.Bus.Topology
{
    public class HostSetting : IHostSetting
    {
        private const int PORT_DEFAULT = 5672;
        private const string USERNAME_DEFAULT = "guest";
        private const string PASSWORD_DEFAULT = "guest";
        private const string VIRTUALHOST_DEFAULT = "/";
        private const bool AUTOMATIC_RECOVERY_ENABLED = true;

        public HostSetting(string hostName, int port, string userName, string password, string virtualHost)
        {
            HostName = hostName;
            Port = port;
            UserName = userName;
            Password = password;
            VirtualHost = virtualHost;
            AutomaticRecoveryEnabled = AUTOMATIC_RECOVERY_ENABLED;

            ValidateParameters();
        }

        public HostSetting(string hostName, int port, string userName, string password, string virtualHost, bool automaticRecoveryEnabled) :
             this(hostName, port, userName, password, virtualHost)
        {
            AutomaticRecoveryEnabled = automaticRecoveryEnabled;
        }

        public HostSetting(string hostName) :
             this(hostName, PORT_DEFAULT, USERNAME_DEFAULT, PASSWORD_DEFAULT, VIRTUALHOST_DEFAULT, AUTOMATIC_RECOVERY_ENABLED)
        {
        }

        public HostSetting(string hostName, string userName, string password)
            : this(hostName, PORT_DEFAULT, userName, password, VIRTUALHOST_DEFAULT, AUTOMATIC_RECOVERY_ENABLED)
        {
        }

        public HostSetting(string hostName, string userName, string password, bool automaticRecoveryEnabled)
          : this(hostName, PORT_DEFAULT, userName, password, VIRTUALHOST_DEFAULT, automaticRecoveryEnabled)
        {
        }

        public HostSetting(string hostName, int port, string userName, string password)
            : this(hostName, port, userName, password, VIRTUALHOST_DEFAULT, AUTOMATIC_RECOVERY_ENABLED)
        {
        }

        public HostSetting(string hostName, int port, string userName, string password, bool automaticRecoveryEnabled)
          : this(hostName, port, userName, password, VIRTUALHOST_DEFAULT, automaticRecoveryEnabled)
        {
        }

        public string HostName { get; private set; }
        public int Port { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string VirtualHost { get; private set; }
        public bool AutomaticRecoveryEnabled { get; private set; }

        private void ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(HostName))
            {
                throw new ArgumentNullException(nameof(HostName));
            }

            if (Port < 0)
            {
                throw new ArgumentNullException(nameof(Port));
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new ArgumentNullException(nameof(UserName));
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException(nameof(Password));
            }

            if (string.IsNullOrWhiteSpace(VirtualHost))
            {
                throw new ArgumentNullException(nameof(VirtualHost));
            }
        }

        public Uri BuildUri()
        {
            var port = Port == default(int) ? PORT_DEFAULT : Port;

            return new Uri($"amqp://{UserName}:{Password}@{HostName}:{port}{VirtualHost}");
        }
    }
}
