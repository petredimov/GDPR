using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices
{
	public class ContextFactory
	{
		public static int PageSize = 20;
		private static bool testing = false;
		private static DatabaseContext context = null;

		public static void SetContext(DatabaseContext test)
		{
			testing = true;
			context = test;
		}

		public static DatabaseContext GetContext()
		{
			if (!testing)
			{
				return new DatabaseContext();
			}

			return context;
		}
	}
}