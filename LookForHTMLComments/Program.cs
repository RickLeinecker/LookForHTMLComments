using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace LookForHTMLComments
{
	class Program
	{
		static void Main(string[] args)
		{

			if( args.Length < 1 )
			{
				Console.WriteLine("You need to specify a file with the list of URLs.");
				return;
			}

			StreamReader reader;
			try
			{
				reader = File.OpenText(args[0]);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());
				return;
			}

			while( reader.Peek() > 0 )
			{
				string url = reader.ReadLine().Trim();
				if (url.Length == 0)
				{
					continue;
				}

				string HTMLData = getHTMLData(url);
				List<string> commentList = getListOfComments(HTMLData);

				if( commentList.Count == 0 )
				{
					Console.WriteLine("No comments found in {0}", url);
				}
				else
				{
					Console.WriteLine("{0} comments found in {1}", commentList.Count, url);
					foreach( string s in commentList )
					{
						Console.WriteLine(s);
					}
				}

				Console.WriteLine();



			}

			reader.Close();
		}

		static string getHTMLData( string url )
		{
			string ret = String.Empty;
			WebClient wc = new WebClient();
			try
			{
				ret = wc.DownloadString(url);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());
				return ret;
			}
			return ret;
		}

		static List<string> getListOfComments( string data )
		{
			List<string> ret = new List<string>();
			int index = 0;
			while( index < data.Length && data.IndexOf("<!--", index ) >= 0 )
			{
				index = data.IndexOf("<!--", index);
				int index2 = data.IndexOf("-->", index);
				if( index2 < 0 )
				{
					ret.Add(data.Substring(index+4).Trim());
					index = data.Length;
				}
				else
				{
					ret.Add(data.Substring(index+4, index2 - index - 4).Trim());
					index = index2;
				}
			}

			return ret;
		}

	}
}
