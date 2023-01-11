/*
 * Created by SharpDevelop.
 * User: Bogdan
 * Date: 12/24/2022
 * Time: 3:28 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Management;

namespace LongPathTool
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			PopulateDriveList();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		private static string GetFirstDriveLetter()
		{
			string result;
			try
			{
				SelectQuery query = new SelectQuery("select name, FreeSpace from win32_logicaldisk where drivetype=3");
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
				string text = "c:\\";
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						text = managementObject["name"] + "\\";
					}
				}
				result = text;
			}
			catch
			{
				result = "c:\\";
			}
			return result;
		}

		
		private static string GetDemoDirectoryRootName()
		{
			string text = "abcdefghijklmnopqrssttuvwxy";
			for (int i = 0; i < 3; i++)
			{
				text += text;
			}
			return text;
		}
		
		private static string demoPath;
		private static string GetDemoPath()
		{
			string firstDriveLetter = GetFirstDriveLetter();
			string demoDirectoryRootName = GetDemoDirectoryRootName();
			string text = firstDriveLetter;
			for (int i = 0; i < 6; i++)
			{
				try
				{
					Directory.CreateDirectory(Path.Combine(text, demoDirectoryRootName));
				}
				catch
				{
					LongPath.CreateDirectoryEx("\\\\?\\" + text, "\\\\?\\" + Path.Combine(text, demoDirectoryRootName), IntPtr.Zero);
				}
				text = Path.Combine(text, demoDirectoryRootName);
				demoPath = text;
			}
			return text;
		}
		
		private static string GetFinalDemoPath()
		{
			return Path.Combine(GetDemoPath(), "Longpath.abc");
		}
		
		void ButtondemoClick(object sender, System.EventArgs e)
		{

			FolderBrowser folderBrowser = new FolderBrowser();
			folderBrowser.Title = "Select path where to copy";
			string finalDemoPath = GetFinalDemoPath();
			LongPath.TestCreateAndWrite(finalDemoPath);
			demoPath = GetDemoDirectoryRootName();
			MessageBox.Show("File " + finalDemoPath + " created!\n\n\n Try to delete it in windows; if no success try this tool!");

		}
		
		
		public void WritetxtFileSource(string txtFileSource, ListView.ListViewItemCollection items)
		{
			try
			{
				StreamWriter streamWriter = new StreamWriter(txtFileSource, false, Encoding.Default);
				if (txtFileSource == "")
				{
					MessageBox.Show("Please browse for the path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				foreach (ListViewItem listViewItem in items)
				{
					streamWriter.WriteLine(listViewItem.Text);
				}
				streamWriter.Close();
			}
			catch
			{
			}
		}
	
		private SaveFileDialog sfDialog = new SaveFileDialog();
		void ExportToTxtClick(object sender, System.EventArgs e)
		{
	if (this.listViewLongPaths.Items.Count == 0)
	{
		MessageBox.Show("There are no files!", "Error occured");
	}
	else
	{
		this.sfDialog.Title = "Save As";
		this.sfDialog.Filter = " Text Files | *.txt | All Files | *.*";
		this.sfDialog.FilterIndex = 1;
		this.sfDialog.FileName = "Long Path Files List";
		this.sfDialog.RestoreDirectory = true;
		this.sfDialog.InitialDirectory = Application.StartupPath;
		if (this.sfDialog.ShowDialog() == DialogResult.OK)
		{
			this.WritetxtFileSource(this.sfDialog.FileName, this.listViewLongPaths.Items);
		}
	}
		}
		
		protected bool isFile(string filePath)
		{
			FileAttributes attributes = File.GetAttributes(filePath);
			return (attributes & FileAttributes.Directory) != FileAttributes.Directory;
		}
				
		protected bool ForceDeleteLockedFile(string filePath)
		{
			bool result;
			try
			{
				//DeleteLockFile(filePath);
				result = true;
				return result;
			}
			catch (Exception exc)
			{
			}
			result = false;
			return result;
		}
		
		private enum MoveFileFlags
		{
			MOVEFILE_REPLACE_EXISTING = 1,
			MOVEFILE_COPY_ALLOWED,
			MOVEFILE_DELAY_UNTIL_REBOOT = 4,
			MOVEFILE_WRITE_THROUGH = 8,
			MOVEFILE_CREATE_HARDLINK = 16,
			MOVEFILE_FAIL_IF_NOT_TRACKABLE = 32
		}
				
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);

		
		protected bool DeleteLockedFile(string filePath)
		{
			bool result;
			try
			{
				MoveFileEx(filePath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);
				result = true;
				return result;
			}
			catch
			{
			}
			result = false;
			return result;
		}
		
protected bool DeleteFile(string filePath)
{
	bool result;
	try
	{
		if (!this.isFile(filePath))
		{
			Directory.Delete(filePath, true);
		}
		else
		{
			File.Delete(filePath);
		}
		result = true;
		return result;
	}
	catch (ArgumentException argExc)
	{
	}
	catch (FileNotFoundException FileNotFoundEx)
	{
	}
	catch (PathTooLongException pathToLongExc)
	{
		List<string> list = LongPath.FindFilesAndDirs("\\\\?\\" + filePath);
		foreach (string current in list)
		{
			LongPath.DeleteFolderPathTooLong(current);
		}
		result = LongPath.DeleteFolderPathTooLong("\\\\?\\" + filePath);
		return result;
	}
	catch (IOException ioExc)
	{
		if (!this.isFile(filePath))
		{
			List<string> list = LongPath.FindFilesAndDirs("\\\\?\\" + filePath);
			foreach (string current in list)
			{
				LongPath.DeleteFolderPathTooLong(current);
			}
			result = LongPath.DeleteFolderPathTooLong("\\\\?\\" + filePath);
			return result;
		}
		DialogResult dialogResult = MessageBox.Show("File is locked by process.Press yes to stop process and delete file or no to delete after reboot(recommended)?", "Potential hazardous operation", MessageBoxButtons.YesNo);
		if (dialogResult == DialogResult.No)
		{
			if (this.DeleteLockedFile(filePath))
			{
				MessageBox.Show("The file will be removed only after reboot!");
			}
		}
		else if (dialogResult == DialogResult.Yes)
		{
			result = this.ForceDeleteLockedFile(filePath);
			return result;
		}
	}
	catch (UnauthorizedAccessException unathorizedExc)
	{
	}
	catch (Exception exc)
	{
	}
	result = false;
	return result;
}

		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		protected ManagementObjectCollection getDrives()
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * From Win32_LogicalDisk ");
			return managementObjectSearcher.Get();
		}

		private void PopulateDriveList()
		{
			this.Cursor = Cursors.WaitCursor;
			this.treeViewFolders.Nodes.Clear();
			TreeNode treeNode = new TreeNode("My Computer", 0, 0);
			this.treeViewFolders.Nodes.Add(treeNode);
			TreeNodeCollection nodes = treeNode.Nodes;
			ManagementObjectCollection drives = this.getDrives();
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = drives.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					int imageIndex;
					int selectedImageIndex;
					switch (int.Parse(managementObject["DriveType"].ToString()))
					{
					case 2:
						imageIndex = 7;
						selectedImageIndex = 7;
						break;
					case 3:
						imageIndex = 1;
						selectedImageIndex = 1;
						break;
					case 4:
						imageIndex = 2;
						selectedImageIndex = 2;
						break;
					case 5:
						imageIndex = 3;
						selectedImageIndex = 3;
						break;
					default:
						imageIndex = 5;
						selectedImageIndex = 6;
						break;
					}
					treeNode = new TreeNode(managementObject["Name"].ToString() + "\\", imageIndex, selectedImageIndex);
					nodes.Add(treeNode);
				}
			}
			this.Cursor = Cursors.Default;
		}
		
		
		protected string getFullPath(string stringPath)
		{
			return stringPath.Replace("My Computer\\", "");
		}

		protected string GetPathName(string stringPath)
		{
			string[] array = stringPath.Split(new char[]
			{
				'\\'
			});
			int num = array.Length;
			return array[num - 1];
		}
		
		public static bool CheckSignature(string filepath, int signatureSize, string expectedSignature)
		{
			bool result;
			try
			{
				if (string.IsNullOrEmpty(filepath))
				{
					throw new ArgumentException("Must specify a filepath");
				}
				if (string.IsNullOrEmpty(expectedSignature))
				{
					throw new ArgumentException("Must specify a value for the expected file signature");
				}
				using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					if (fileStream.Length < (long)signatureSize)
					{
						result = false;
					}
					else
					{
						byte[] array = new byte[signatureSize];
						int i = signatureSize;
						int num = 0;
						while (i > 0)
						{
							int num2 = fileStream.Read(array, num, i);
							i -= num2;
							num += num2;
						}
						string a = BitConverter.ToString(array);
						if (a == expectedSignature)
						{
							result = true;
						}
						else
						{
							result = false;
						}
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		
		protected void PopulateDirectory(TreeNode nodeCurrent, TreeNodeCollection nodeCurrentCollection)
		{
			int imageIndex = 5;
			int selectedImageIndex = 6;
			int num = 8;
			if (nodeCurrent.SelectedImageIndex != 0)
			{
				try
				{
					if (this.getFullPath(nodeCurrent.FullPath).Length > 244)
					{
						List<string> list = LongPath.FindFilesAndDirs("\\\\?\\" + this.getFullPath(nodeCurrent.FullPath));
						List<string> list2 = LongPath.FindFiles("\\\\?\\" + this.getFullPath(nodeCurrent.FullPath));
						if (list.Count != 0)
						{
							string text = list[list.Count - 1];
							string pathName = this.GetPathName(text);
							TreeNode node;
							if (list2.Contains(text))
							{
								node = new TreeNode(pathName, num, num);
							}
							else
							{
								node = new TreeNode(pathName, imageIndex, selectedImageIndex);
							}
							nodeCurrentCollection.Add(node);
						}
					}
					else if (!Directory.Exists(this.getFullPath(nodeCurrent.FullPath)))
					{
						if (!File.Exists(this.getFullPath(nodeCurrent.FullPath)))
						{
						}
					}
					else
					{
						string[] directories = Directory.GetDirectories(this.getFullPath(nodeCurrent.FullPath));
						string[] array = directories;
						for (int i = 0; i < array.Length; i++)
						{
							string text2 = array[i];
							string text = text2;
							string pathName = this.GetPathName(text);
							TreeNode node = new TreeNode(pathName, imageIndex, selectedImageIndex);
							nodeCurrentCollection.Add(node);
						}
						string[] files = Directory.GetFiles(this.getFullPath(nodeCurrent.FullPath));
						array = files;
						for (int i = 0; i < array.Length; i++)
						{
							string text3 = array[i];
							string text = text3;
							string pathName2 = this.GetPathName(text);
							num = (((text.EndsWith("gz") && CheckSignature(text, 3, "1F-8B-08")) || (text.EndsWith("zip") && CheckSignature(text, 4, "50-4B-03-04")) || (text.EndsWith("rar") && CheckSignature(text, 7, "52-61-72-21-1A-07-00"))) ? 9 : 8);
							TreeNode node = new TreeNode(pathName2, num, num);
							nodeCurrentCollection.Add(node);
						}
					}
				}
				catch (IOException ioExc)
				{
				}
				catch (UnauthorizedAccessException unathorizedExc)
				{
				}
				catch (Exception exc)
				{
				}
			}
		}
		
		void TreeViewFoldersAfterSelect(object sender, TreeViewEventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				TreeNode node = e.Node;
				node.Nodes.Clear();
				if (node.SelectedImageIndex == 0)
				{
					this.PopulateDriveList();
				}
				else
				{
					this.PopulateDirectory(node, node.Nodes);
				}
				this.Cursor = Cursors.Default;
			}
			catch
			{
			}
		}
		
		
		private void DeleteFiles()
		{
			DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete selected path?", "Question", MessageBoxButtons.YesNo);
			if (dialogResult != DialogResult.No)
			{
				string text = this.tabControl1.SelectedTab.Text;
				if (text != null)
				{
					if (!(text == "Delete/Rename/Copy"))
					{
						if (text == "Search Long Paths")
						{
							if (this.listViewLongPaths.SelectedItems.Count == 0)
							{
								MessageBox.Show("No path(s) selected!");
							}
							else
							{
								foreach (ListViewItem listViewItem in this.listViewLongPaths.SelectedItems)
								{
									if (this.DeleteFile(listViewItem.Text))
									{
										listViewItem.Remove();
										this.listViewLongPaths.Refresh();
									}
								}
							}
						}
					}
					else if (this.treeViewFolders.SelectedNode == null)
					{
						MessageBox.Show("No path selected!");
					}
					else if (this.DeleteFile(this.getFullPath(this.treeViewFolders.SelectedNode.FullPath)))
					{
						this.treeViewFolders.SelectedNode.Remove();
						this.treeViewFolders.Refresh();
					}
				}
			}
		}
		
		void ButtonDeleteClick(object sender, EventArgs e)
		{
		this.DeleteFiles();
		}
		
		private int fileLength;
		private bool reachedmaximum = false;
		
		public void AddLongPath(string path, bool finish)
		{
			try
			{
				if (path != "")
				{
					ListViewItem value = new ListViewItem(new string[]
					{
						path
					}, 8);
					this.listViewLongPaths.Items.Add(value);
					if (!this.reachedmaximum)
					{
						this.progressBarSearch.Value++;
						this.reachedmaximum = (this.progressBarSearch.Value == this.progressBarSearch.Maximum);
					}
					else
					{
						this.progressBarSearch.Value--;
						this.reachedmaximum = (this.progressBarSearch.Value != this.progressBarSearch.Minimum);
					}
				}
			}
			catch
			{
			}
		}

		
		public void CheckLongPath(List<string> searchPath, int fileLength)
		{
			foreach (string current in searchPath)
			{
				if (current.Length >= fileLength)
				{
					if (current.Length >= 255)
					{
						this.AddLongPath(current.Replace("\\\\?\\", ""), false);
					}
					else
					{
						this.AddLongPath(current, false);
					}
				}
			}
			this.AddLongPath("", true);
		}
		
		private bool Halt = false;
		
		public List<string> GetFilesRecursive(string path)
		{
			List<string> list = new List<string>();
			Stack<string> stack = new Stack<string>();
			stack.Push(path);
			while (stack.Count > 0)
			{
				string text = stack.Pop();
				try
				{
					if (this.Halt)
					{
						this.Halt = false;
						break;
					}
					string[] files = Directory.GetFiles(text, "*.*");
					List<string> list2 = new List<string>();
					list2.AddRange(files);
					list.AddRange(files);
					this.SetText("Scanning: " + text);
					this.CheckLongPath(list2, this.fileLength);
					string[] directories = Directory.GetDirectories(text);
					for (int i = 0; i < directories.Length; i++)
					{
						string item = directories[i];
						stack.Push(item);
					}
				}
				catch (PathTooLongException var_6_BE)
				{
					List<string> list3 = LongPath.FindFilesAndDirs("\\\\?\\" + text.Replace(":\\", ":\\\\"));
					List<string> list4 = LongPath.FindFiles("\\\\?\\" + text.Replace(":\\", ":\\\\"));
					if (list3.Count == 0)
					{
						break;
					}
					if (list4.Count > 0)
					{
						list.AddRange(list4.ToArray());
					}
					foreach (string current in list3)
					{
						list4 = LongPath.FindFiles(current);
						if (list4.Count > 0)
						{
							list.AddRange(list4.ToArray());
						}
					}
				}
				catch (Exception var_10_19F)
				{
				}
			}
			ScanFinished();
			return list;
		}
		
		private void ScanFinished()
		{
			SetText("Scan is finished");
			buttonBrowseSearch.Text = "Browse/Search";
		}

		
		private List<string> searchPath;
		private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			string path = (string)e.Argument;
			this.searchPath = this.GetFilesRecursive(path);
		}

		private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBarSearch.Value = e.ProgressPercentage;
		}

		private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progressBarSearch.Value = this.progressBarSearch.Maximum;
		}

		
		private void RunLongPathFilesSearchingThread(string path)
		{
			this.backgroundWorker1.WorkerReportsProgress = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.RunWorkerAsync(path);
			this.labelSearchPath.Text = "Search Path is: " + path;
			this.buttonBrowseSearch.Text = "Stop";
			this.listViewLongPaths.Clear();
			this.listViewLongPaths.Columns.Add("File Path", 2 * this.listViewLongPaths.Width);
		}
		

		
		
		private delegate void SetTextCallback2(string text);
		
		private void SetText(string text)
		{
			if (this.labelSearchPath.InvokeRequired)
			{
				SetTextCallback2 method = new SetTextCallback2(this.SetText);
				base.Invoke(method, new object[]
				{
					text
				});
			}
			else
			{
				this.labelSearchPath.Text = text;
			}
		}

		private LongPathsSearch longPathsSearch;
		void ButtonBrowseSearchClick(object sender, EventArgs e)
		{
			int.TryParse(this.textBoxLength.Text, out this.fileLength);
			if (this.fileLength == 0)
			{
				MessageBox.Show("Invalid file length; fileLength!", "Error");
			}
			else if (this.longPathsSearch != null || this.buttonBrowseSearch.Text == "Stop")
			{
				this.Halt = true;
			}
			else if (this.buttonBrowseSearch.Text == "Stop")
			{
				this.buttonBrowseSearch.Text = "Browse/Search";
				this.progressBarSearch.Visible = false;
				this.backgroundWorker1.CancelAsync();
				this.backgroundWorker1 = null;
				this.backgroundWorker1 = new BackgroundWorker();
				Thread.Sleep(1000);
			}
			else
			{
				FolderBrowser folderBrowser = new FolderBrowser();
				folderBrowser.Title = "Select path where to search";
				DialogResult dialogResult = folderBrowser.ShowDialog(this);
				if (dialogResult == DialogResult.OK)
				{
					try
					{
						this.RunLongPathFilesSearchingThread(folderBrowser.DirectoryPath);
					}
					catch
					{
					}
				}
			}
		}
		

		protected bool RenameFileA(string sourcefilePath)
		{
			bool result;
			try
			{
				InputBoxResult inputBoxResult = InputBox.Show("Change the file name", "Rename", sourcefilePath.Substring(sourcefilePath.LastIndexOf("\\") + 1));
				if (inputBoxResult.ReturnCode == DialogResult.OK)
				{
					result = MoveFileEx("\\\\?\\" + sourcefilePath, "\\\\?\\" + sourcefilePath.Substring(0, sourcefilePath.LastIndexOf("\\") + 1) + inputBoxResult.Text, MoveFileFlags.MOVEFILE_COPY_ALLOWED);
					return result;
				}
				result = false;
				return result;
			}
			catch (ArgumentException var_1_75)
			{
			}
			catch (FileNotFoundException var_2_7A)
			{
			}
			catch (PathTooLongException var_3_7F)
			{
			}
			catch (IOException var_4_84)
			{
			}
			catch (UnauthorizedAccessException var_5_8A)
			{
			}
			catch (Exception var_6_90)
			{
			}
			result = false;
			return result;
		}
		
		private void RenameFiles()
		{
			try
			{
				string text = this.tabControl1.SelectedTab.Text;
				if (text != null)
				{
					if (!(text == "Delete/Rename/Copy"))
					{
						if (text == "Search Long Paths")
						{
							if (this.listViewLongPaths.SelectedItems.Count == 0)
							{
								MessageBox.Show("No file was selected!");
							}
							else
							{
								foreach (ListViewItem listViewItem in this.listViewLongPaths.SelectedItems)
								{
									this.RenameFileA(listViewItem.Text);
								}
							}
						}
					}
					else if (this.treeViewFolders.SelectedNode == null)
					{
						MessageBox.Show("No file was selected!");
					}
					else if (this.RenameFileA(this.getFullPath(this.treeViewFolders.SelectedNode.FullPath)))
					{
						this.treeViewFolders.CollapseAll();
					}
				}
			}
			catch
			{
			}
		}
		
		void ButtonRenameClick(object sender, EventArgs e)
		{
			this.RenameFiles();
		}
		
		private static void WriteToLogStream(string message)
		{
			using (StreamWriter streamWriter = File.AppendText("log.txt"))
			{
				streamWriter.WriteLine(message);
			}
		}

		public static void copyDirectory(string Src, string Dst)
		{
			try
			{
				if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
				{
					Dst += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(Dst))
				{
					Directory.CreateDirectory(Dst);
				}
				string[] fileSystemEntries = Directory.GetFileSystemEntries(Src);
				string[] array = fileSystemEntries;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					try
					{
						if (Directory.Exists(text))
						{
							copyDirectory(text, Dst + Path.GetFileName(text));
						}
						else
						{
							File.Copy(text, Dst + Path.GetFileName(text), true);
						}
					}
					catch
					{
						WriteToLogStream("Failed to copy: " + text);
					}
				}
			}
			catch
			{
			}
		}
		
		protected bool CopyFileA(string sourcefilePath, string destinationDirectory)
		{
			bool result;
			try
			{
				if (!this.isFile(sourcefilePath))
				{
					if (Directory.Exists(Path.Combine(destinationDirectory, Path.GetFileName(sourcefilePath))) && MessageBox.Show("Destination path exists.Overwrite?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						result = false;
						return result;
					}
					copyDirectory(sourcefilePath, Path.Combine(destinationDirectory, Path.GetFileName(sourcefilePath)));
				}
				else
				{
					if (File.Exists(Path.Combine(destinationDirectory, Path.GetFileName(sourcefilePath))) && MessageBox.Show("Destination path exists.Overwrite?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						result = false;
						return result;
					}
					File.Copy(sourcefilePath, Path.Combine(destinationDirectory, Path.GetFileName(sourcefilePath)), true);
				}
				result = true;
				return result;
			}
			catch (Exception var_0_AD)
			{
				try
				{
					CopyRename copyRename = new CopyRename();
					copyRename.XCopy("\\\\?\\" + sourcefilePath, Path.Combine(destinationDirectory, Path.GetFileName(sourcefilePath)));
				}
				catch
				{
					WriteToLogStream("Failed to copy: " + sourcefilePath);
				}
			}
			result = false;
			return result;
		}
		
		private void CopyFiles()
		{
			try
			{
				FolderBrowser folderBrowser = new FolderBrowser();
				folderBrowser.Title = "Select path where to copy";
				DialogResult dialogResult = folderBrowser.ShowDialog(this);
				if (dialogResult == DialogResult.OK)
				{
					string text = this.tabControl1.SelectedTab.Text;
					if (text != null)
					{
						if (!(text == "Delete/Rename/Copy"))
						{
							if (text == "Search Long Paths")
							{
								if (this.listViewLongPaths.SelectedItems.Count == 0)
								{
									MessageBox.Show("No file was selected!");
								}
								else
								{
									foreach (ListViewItem listViewItem in this.listViewLongPaths.SelectedItems)
									{
										this.CopyFileA(listViewItem.Text, folderBrowser.DirectoryPath);
									}
								}
							}
						}
						else if (this.treeViewFolders.SelectedNode == null)
						{
							MessageBox.Show("No file was selected!");
						}
						else
						{
							this.CopyFileA(this.getFullPath(this.treeViewFolders.SelectedNode.FullPath), folderBrowser.DirectoryPath);
						}
					}
				}
			}
			catch
			{
			}
		}

		void ButtonCopyToClick(object sender, EventArgs e)
		{
			this.CopyFiles();
		}
		
		
		

	
	}
}
