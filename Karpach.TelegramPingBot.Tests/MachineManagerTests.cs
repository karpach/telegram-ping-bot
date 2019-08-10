using System;
using NUnit.Framework;

namespace TelegramPingBot.Tests
{	
	public class MachineManagerTests
	{
		private MachineManager _instance;

		[SetUp]
		public void Setup()
		{
			_instance = new MachineManager();
		}

		[Test]
		public void GetIpAddressTest()
		{
			// Act
			string result = _instance.GetIpAddress();

			// Assert
			Console.WriteLine(result);
			Assert.IsNotEmpty(result);
		}
	}
}
