/*
 * Created by SharpDevelop.
User: Bogdan
 * Date: 12/22/2022
 * Time: 3:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace LongPathTool
{
	
	public class LongPathsSearchEventArgs : EventArgs
	{
		private bool complete = false;

		private string filePath = "";

		public bool Complete
		{
			get
			{
				return this.complete;
			}
			set
			{
				this.complete = value;
			}
		}

		public string FilePath
		{
			get
			{
				return this.filePath;
			}
			set
			{
				this.filePath = value;
			}
		}

		public LongPathsSearchEventArgs()
		{
		}

		public LongPathsSearchEventArgs(string filePath, bool complete)
		{
			this.filePath = filePath;
			this.complete = complete;
		}
	}
	
	internal class LongPathsSearch
	{
		public delegate void NewLongPathEventHandler(object sender, LongPathsSearchEventArgs e);

		private bool halt = false;

		public event LongPathsSearch.NewLongPathEventHandler foundNewLongPath;

		public bool Halt
		{
			get
			{
				return this.halt;
			}
			set
			{
				this.halt = value;
			}
		}

		public void Begin(List<string> searchPath, int fileLength)
		{
			foreach (string current in searchPath)
			{
				if (this.Halt)
				{
					break;
				}
				if (current.Length >= fileLength)
				{
					if (current.Length >= 255)
					{
						this.displayLongPath(current.Replace("\\\\?\\", ""), false);
					}
					else
					{
						this.displayLongPath(current, false);
					}
				}
			}
			this.displayLongPath("", true);
		}

		private void displayLongPath(string filePath, bool complete)
		{
			if (this.foundNewLongPath != null)
			{
				this.foundNewLongPath(this, new LongPathsSearchEventArgs(filePath, complete));
			}
		}
	}
}
