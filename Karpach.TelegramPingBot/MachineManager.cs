using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Win32;

namespace TelegramPingBot
{
	public class MachineManager
	{
		public string GetIpAddress()
		{
			return NetworkInterface
						.GetAllNetworkInterfaces()
						.FirstOrDefault(ni =>
							ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet
							&& ni.OperationalStatus == OperationalStatus.Up
							&& ni.GetIPProperties().GatewayAddresses.FirstOrDefault() != null
							&& ni.GetIPProperties().UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork) != null
						)
						?.GetIPProperties()
						.UnicastAddresses
						.FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
						?.Address
						?.ToString()
					?? string.Empty;
		}

		public void Restart()
		{
			System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
		}

		public void SetAutoStart(bool autoStart, string applicationPath)

		{
			RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			if (autoStart)
			{
				// Add the value in the registry so that the application runs at startup
				rkApp?.SetValue("TelegramBotClient", applicationPath);
			}
			else
			{
				rkApp?.DeleteValue("TelegramBotClient", false);
			}
		}
	}
}