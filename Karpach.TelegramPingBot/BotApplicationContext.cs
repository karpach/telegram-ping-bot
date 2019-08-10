using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Telegram.Bot;
using TelegramPingBot.Properties;

namespace TelegramPingBot
{
	public class BotApplicationContext: ApplicationContext
	{
		private readonly NotifyIcon _trayIcon;		
		private TelegramBotClient _bot;

		public BotApplicationContext()
		{
			// Initialize Tray Icon
			var contextMenu = new ContextMenuStrip();
			contextMenu.Items.Add(new ToolStripMenuItem("Exit", Resources.Close, Exit));
			_trayIcon = new NotifyIcon
			{
				Icon = Resources.App,
				ContextMenuStrip = contextMenu,
				Visible = true
			};

			_bot = new TelegramBotClient(Settings.Default.TelegramBotToken);			
			_bot.OnMessage += Bot_OnMessage;			
			_bot.StartReceiving();
		}

		private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
		{
			var manager = new MachineManager();
			if (string.Equals(e.Message.Text, "ip", StringComparison.InvariantCultureIgnoreCase))
			{				
				_bot.SendTextMessageAsync(e.Message.Chat.Id, $"Time: {DateTime.Now} {manager.GetIpAddress()}").ConfigureAwait(false).GetAwaiter().GetResult();
			}
			if (string.Equals(e.Message.Text, "restart", StringComparison.InvariantCultureIgnoreCase))
			{
				_bot.SendTextMessageAsync(e.Message.Chat.Id, $"Time: {DateTime.Now} Restarting ...").ConfigureAwait(false).GetAwaiter().GetResult();
				manager.Restart();
			}
			if (string.Equals(e.Message.Text, "autostart on", StringComparison.InvariantCultureIgnoreCase))
			{
				_bot.SendTextMessageAsync(e.Message.Chat.Id, "Setting auto start on").ConfigureAwait(false).GetAwaiter().GetResult();
				manager.SetAutoStart(true, Application.ExecutablePath);
			}
			if (string.Equals(e.Message.Text, "autostart off", StringComparison.InvariantCultureIgnoreCase))
			{
				_bot.SendTextMessageAsync(e.Message.Chat.Id, "Setting auto start off").ConfigureAwait(false).GetAwaiter().GetResult();
				manager.SetAutoStart(false, Application.ExecutablePath);
			}
			if (string.Equals(e.Message.Text, "help", StringComparison.InvariantCultureIgnoreCase))
			{
				_bot.SendTextMessageAsync(e.Message.Chat.Id, "ip - you machine ip address \nrestart - restart your machine\nautostart on/off - auto starts bot after reboot").ConfigureAwait(false).GetAwaiter().GetResult();				
			}
		}

		void Exit(object sender, EventArgs e)
		{
			// Hide tray icon, otherwise it will remain shown until user mouses over it
			_trayIcon.Visible = false;			
			Application.Exit();
		}		
	}
}