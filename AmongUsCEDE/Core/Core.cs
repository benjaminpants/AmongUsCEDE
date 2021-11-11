using System;
using System.Collections.Generic;
using System.Text;

namespace AmongUsCEDE.Core
{
	class CriticalClientException : Exception // An Exception that should be thrown if the client encounters an error that it can not recover from, and thus disconnect it
	{
		private void DisconnectClient()
		{
			AmongUsClient.Instance.EnqueueDisconnect(DisconnectReasons.Custom, "Critical Error(" + this.Source + "):" + this.Message);
		}

		public CriticalClientException() : base("A Critical Error has occured!")
		{
			DisconnectClient();
		}

		public CriticalClientException(string message) : base(message)
		{
			DisconnectClient();
		}

		public CriticalClientException(string message, Exception inner) : base(message, inner)
		{
			DisconnectClient();
		}
	}
}
