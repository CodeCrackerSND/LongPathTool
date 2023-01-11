/*
 * Created by SharpDevelop.
 * User: Bogdan
 * Date: 12/24/2022
 * Time: 3:28 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LongPathTool
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.treeViewFolders = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.ExportTxtButton = new System.Windows.Forms.Button();
			this.labelSearchPath = new System.Windows.Forms.Label();
			this.buttonBrowseSearch = new System.Windows.Forms.Button();
			this.textBoxLength = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.listViewLongPaths = new System.Windows.Forms.ListView();
			this.progressBarSearch = new System.Windows.Forms.ProgressBar();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.ButtonDelete = new System.Windows.Forms.Button();
			this.RenameButton = new System.Windows.Forms.Button();
			this.CopyToButton = new System.Windows.Forms.Button();
			this.DemoButton = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(603, 404);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.treeViewFolders);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(595, 378);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Delete/Rename/Copy";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// treeViewFolders
			// 
			this.treeViewFolders.ImageIndex = 0;
			this.treeViewFolders.ImageList = this.imageList1;
			this.treeViewFolders.Location = new System.Drawing.Point(6, 6);
			this.treeViewFolders.Name = "treeViewFolders";
			this.treeViewFolders.SelectedImageIndex = 0;
			this.treeViewFolders.Size = new System.Drawing.Size(586, 366);
			this.treeViewFolders.TabIndex = 0;
			this.treeViewFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewFoldersAfterSelect);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "FormMain_16_0.png");
			this.imageList1.Images.SetKeyName(1, "FormMain_16_1.png");
			this.imageList1.Images.SetKeyName(2, "FormMain_16_2.png");
			this.imageList1.Images.SetKeyName(3, "FormMain_16_3.png");
			this.imageList1.Images.SetKeyName(4, "FormMain_16_4.png");
			this.imageList1.Images.SetKeyName(5, "FormMain_16_5.png");
			this.imageList1.Images.SetKeyName(6, "FormMain_16_6.png");
			this.imageList1.Images.SetKeyName(7, "FormMain_16_7.png");
			this.imageList1.Images.SetKeyName(8, "FormMain_16_8.png");
			this.imageList1.Images.SetKeyName(9, "FormMain_16_9.png");
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.ExportTxtButton);
			this.tabPage2.Controls.Add(this.labelSearchPath);
			this.tabPage2.Controls.Add(this.buttonBrowseSearch);
			this.tabPage2.Controls.Add(this.textBoxLength);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.listViewLongPaths);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(595, 378);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Search Long Paths";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// ExportTxtButton
			// 
			this.ExportTxtButton.Location = new System.Drawing.Point(358, 1);
			this.ExportTxtButton.Name = "ExportTxtButton";
			this.ExportTxtButton.Size = new System.Drawing.Size(85, 23);
			this.ExportTxtButton.TabIndex = 5;
			this.ExportTxtButton.Text = "Export to *.txt";
			this.ExportTxtButton.UseVisualStyleBackColor = true;
			this.ExportTxtButton.Click += new System.EventHandler(this.ExportToTxtClick);
			// 
			// labelSearchPath
			// 
			this.labelSearchPath.Location = new System.Drawing.Point(6, 33);
			this.labelSearchPath.Name = "labelSearchPath";
			this.labelSearchPath.Size = new System.Drawing.Size(407, 28);
			this.labelSearchPath.TabIndex = 4;
			this.labelSearchPath.Text = "Search Path";
			// 
			// buttonBrowseSearch
			// 
			this.buttonBrowseSearch.Location = new System.Drawing.Point(257, 1);
			this.buttonBrowseSearch.Name = "buttonBrowseSearch";
			this.buttonBrowseSearch.Size = new System.Drawing.Size(95, 23);
			this.buttonBrowseSearch.TabIndex = 3;
			this.buttonBrowseSearch.Text = "Browse/Search";
			this.buttonBrowseSearch.UseVisualStyleBackColor = true;
			this.buttonBrowseSearch.Click += new System.EventHandler(this.ButtonBrowseSearchClick);
			// 
			// textBoxLength
			// 
			this.textBoxLength.Location = new System.Drawing.Point(151, 3);
			this.textBoxLength.Name = "textBoxLength";
			this.textBoxLength.Size = new System.Drawing.Size(100, 20);
			this.textBoxLength.TabIndex = 2;
			this.textBoxLength.Text = "100";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(153, 14);
			this.label1.TabIndex = 1;
			this.label1.Text = "Show files with name length > ";
			// 
			// listViewLongPaths
			// 
			this.listViewLongPaths.Location = new System.Drawing.Point(6, 64);
			this.listViewLongPaths.Name = "listViewLongPaths";
			this.listViewLongPaths.Size = new System.Drawing.Size(583, 308);
			this.listViewLongPaths.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.listViewLongPaths.TabIndex = 0;
			this.listViewLongPaths.UseCompatibleStateImageBehavior = false;
			this.listViewLongPaths.View = System.Windows.Forms.View.Details;
			// 
			// progressBarSearch
			// 
			this.progressBarSearch.Location = new System.Drawing.Point(441, 422);
			this.progressBarSearch.Name = "progressBarSearch";
			this.progressBarSearch.Size = new System.Drawing.Size(100, 23);
			this.progressBarSearch.TabIndex = 1;
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
			// 
			// ButtonDelete
			// 
			this.ButtonDelete.Location = new System.Drawing.Point(20, 422);
			this.ButtonDelete.Name = "ButtonDelete";
			this.ButtonDelete.Size = new System.Drawing.Size(75, 23);
			this.ButtonDelete.TabIndex = 2;
			this.ButtonDelete.Text = "Delete";
			this.ButtonDelete.UseVisualStyleBackColor = true;
			this.ButtonDelete.Click += new System.EventHandler(this.ButtonDeleteClick);
			// 
			// RenameButton
			// 
			this.RenameButton.Location = new System.Drawing.Point(101, 422);
			this.RenameButton.Name = "RenameButton";
			this.RenameButton.Size = new System.Drawing.Size(75, 23);
			this.RenameButton.TabIndex = 3;
			this.RenameButton.Text = "Rename";
			this.RenameButton.UseVisualStyleBackColor = true;
			this.RenameButton.Click += new System.EventHandler(this.ButtonRenameClick);
			// 
			// CopyToButton
			// 
			this.CopyToButton.Location = new System.Drawing.Point(182, 422);
			this.CopyToButton.Name = "CopyToButton";
			this.CopyToButton.Size = new System.Drawing.Size(75, 23);
			this.CopyToButton.TabIndex = 4;
			this.CopyToButton.Text = "Copy to ...";
			this.CopyToButton.UseVisualStyleBackColor = true;
			this.CopyToButton.Click += new System.EventHandler(this.ButtonCopyToClick);
			// 
			// DemoButton
			// 
			this.DemoButton.Location = new System.Drawing.Point(263, 422);
			this.DemoButton.Name = "DemoButton";
			this.DemoButton.Size = new System.Drawing.Size(75, 23);
			this.DemoButton.TabIndex = 5;
			this.DemoButton.Text = "Demo";
			this.DemoButton.UseVisualStyleBackColor = true;
			this.DemoButton.Click += new System.EventHandler(this.ButtondemoClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(620, 457);
			this.Controls.Add(this.DemoButton);
			this.Controls.Add(this.CopyToButton);
			this.Controls.Add(this.RenameButton);
			this.Controls.Add(this.ButtonDelete);
			this.Controls.Add(this.progressBarSearch);
			this.Controls.Add(this.tabControl1);
			this.Name = "MainForm";
			this.Text = "Long Path Tool";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button ExportTxtButton;
		private System.Windows.Forms.Button DemoButton;
		private System.Windows.Forms.Button CopyToButton;
		private System.Windows.Forms.Button RenameButton;
		private System.Windows.Forms.Button ButtonDelete;
		private System.Windows.Forms.TextBox textBoxLength;
		private System.Windows.Forms.Label labelSearchPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonBrowseSearch;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.ProgressBar progressBarSearch;
		private System.Windows.Forms.TreeView treeViewFolders;
		private System.Windows.Forms.ListView listViewLongPaths;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
		
		


	}
}
