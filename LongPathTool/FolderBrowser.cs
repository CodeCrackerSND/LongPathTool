/*
 * Created by SharpDevelop.
   User: Bogdan
 * Date: 12/22/2022
 * Time: 3:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LongPathTool
{
	public class FolderBrowser : FolderNameEditor
	{
		private FolderNameEditor.FolderBrowser m_obBrowser = null;

		private string m_strDescription;

		public string DirectoryPath
		{
			get
			{
				return this.m_obBrowser.DirectoryPath;
			}
		}

		public string Title
		{
			set
			{
				this.m_strDescription = value;
			}
		}

		public FolderBrowser()
		{
			this.m_strDescription = "Select folder";
			this.m_obBrowser = new FolderNameEditor.FolderBrowser();
		}

		public DialogResult ShowDialog(IWin32Window owner)
		{
			this.m_obBrowser.Description = this.m_strDescription;
			return this.m_obBrowser.ShowDialog(owner);
		}
	}
}

