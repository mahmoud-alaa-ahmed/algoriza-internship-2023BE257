using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vezeeta.Core.Helpers
{
	public static class EmailValidation
	{
		public static bool CheckEmailRegex(string email)
		{
			String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
								   + "@"
								   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
			var check = Regex.IsMatch(email, theEmailPattern);

			return check;
		}
	}
}
