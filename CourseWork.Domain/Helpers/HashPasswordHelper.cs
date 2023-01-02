using SHA3.Net;
using System;
using System.Text;

namespace CourseWork.Domain.Helpers
{
	public static class HashPasswordHelper
	{
		public static string HashPassword(string password)
		{
			using (var sha3256 = Sha3.Sha3256())
			{
				var bytes = sha3256.ComputeHash(Encoding.UTF8.GetBytes(password));
				var hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();

				return hash;
			}
		}
	}
}
