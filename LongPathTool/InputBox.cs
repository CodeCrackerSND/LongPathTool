/*
 * Created by SharpDevelop.
   User: Bogdan
 * Date: 12/22/2022
 * Time: 3:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LongPathTool
{

	public class InputBoxResult
	{
		public DialogResult ReturnCode;

		public string Text;
	}
	
	public class InputBox
	{
		private static Button btnCancel;

		private static Button btnOK;

		private static Form frmInputDialog;

		private static Label lblPrompt;

		private static TextBox txtInput;

		private static string _defaultValue = string.Empty;

		private static string _formCaption = string.Empty;

		private static string _formPrompt = string.Empty;

		private static InputBoxResult _outputResponse = new InputBoxResult();

		private static int _xPos = -1;

		private static int _yPos = -1;

		private static string DefaultValue
		{
			set
			{
				InputBox._defaultValue = value;
			}
		}

		private static string FormCaption
		{
			set
			{
				InputBox._formCaption = value;
			}
		}

		private static string FormPrompt
		{
			set
			{
				InputBox._formPrompt = value;
			}
		}

		private static InputBoxResult OutputResponse
		{
			get
			{
				return InputBox._outputResponse;
			}
			set
			{
				InputBox._outputResponse = value;
			}
		}

		private static int XPosition
		{
			set
			{
				if (value >= 0)
				{
					InputBox._xPos = value;
				}
			}
		}

		private static int YPosition
		{
			set
			{
				if (value >= 0)
				{
					InputBox._yPos = value;
				}
			}
		}

		private static void btnCancel_Click(object sender, EventArgs e)
		{
			InputBox.OutputResponse.ReturnCode = DialogResult.Cancel;
			InputBox.OutputResponse.Text = string.Empty;
			InputBox.frmInputDialog.Dispose();
		}

		private static void btnOK_Click(object sender, EventArgs e)
		{
			InputBox.OutputResponse.ReturnCode = DialogResult.OK;
			InputBox.OutputResponse.Text = InputBox.txtInput.Text;
			InputBox.frmInputDialog.Dispose();
		}

		private static void InitializeComponent()
		{
			InputBox.frmInputDialog = new Form();
			InputBox.frmInputDialog.StartPosition = FormStartPosition.CenterParent;
			InputBox.lblPrompt = new Label();
			InputBox.btnOK = new Button();
			InputBox.btnCancel = new Button();
			InputBox.txtInput = new TextBox();
			InputBox.frmInputDialog.SuspendLayout();
			InputBox.lblPrompt.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			InputBox.lblPrompt.BackColor = SystemColors.Control;
			InputBox.lblPrompt.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			InputBox.lblPrompt.Location = new Point(12, 9);
			InputBox.lblPrompt.Name = "lblPrompt";
			InputBox.lblPrompt.Size = new Size(302, 82);
			InputBox.lblPrompt.TabIndex = 3;
			InputBox.btnOK.DialogResult = DialogResult.OK;
			InputBox.btnOK.FlatStyle = FlatStyle.Popup;
			InputBox.btnOK.Location = new Point(326, 8);
			InputBox.btnOK.Name = "btnOK";
			InputBox.btnOK.Size = new Size(64, 24);
			InputBox.btnOK.TabIndex = 1;
			InputBox.btnOK.Text = "&OK";
			InputBox.btnOK.Click += new EventHandler(InputBox.btnOK_Click);
			InputBox.btnCancel.DialogResult = DialogResult.Cancel;
			InputBox.btnCancel.FlatStyle = FlatStyle.Popup;
			InputBox.btnCancel.Location = new Point(326, 40);
			InputBox.btnCancel.Name = "btnCancel";
			InputBox.btnCancel.Size = new Size(64, 24);
			InputBox.btnCancel.TabIndex = 2;
			InputBox.btnCancel.Text = "&Cancel";
			InputBox.btnCancel.Click += new EventHandler(InputBox.btnCancel_Click);
			InputBox.txtInput.Location = new Point(8, 100);
			InputBox.txtInput.Name = "txtInput";
			InputBox.txtInput.Size = new Size(379, 20);
			InputBox.txtInput.TabIndex = 0;
			InputBox.txtInput.Text = "";
			InputBox.frmInputDialog.AutoScaleBaseSize = new Size(5, 13);
			InputBox.frmInputDialog.ClientSize = new Size(398, 128);
			InputBox.frmInputDialog.Controls.Add(InputBox.txtInput);
			InputBox.frmInputDialog.Controls.Add(InputBox.btnCancel);
			InputBox.frmInputDialog.Controls.Add(InputBox.btnOK);
			InputBox.frmInputDialog.Controls.Add(InputBox.lblPrompt);
			InputBox.frmInputDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
			InputBox.frmInputDialog.MaximizeBox = false;
			InputBox.frmInputDialog.MinimizeBox = false;
			InputBox.frmInputDialog.Name = "InputBoxDialog";
			InputBox.frmInputDialog.ResumeLayout(false);
		}

		private static void LoadForm()
		{
			InputBox.OutputResponse.ReturnCode = DialogResult.Ignore;
			InputBox.OutputResponse.Text = string.Empty;
			InputBox.txtInput.Text = InputBox._defaultValue;
			InputBox.lblPrompt.Text = InputBox._formPrompt;
			InputBox.frmInputDialog.Text = InputBox._formCaption;
			Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
			if (InputBox._xPos >= 0 && InputBox._xPos < workingArea.Width - 100 && InputBox._yPos >= 0 && InputBox._yPos < workingArea.Height - 100)
			{
				InputBox.frmInputDialog.StartPosition = FormStartPosition.Manual;
				InputBox.frmInputDialog.Location = new Point(InputBox._xPos, InputBox._yPos);
			}
			else
			{
				InputBox.frmInputDialog.StartPosition = FormStartPosition.CenterScreen;
			}
			string text = InputBox.lblPrompt.Text;
			int num = 0;
			int startIndex = 0;
			while (text.IndexOf("\n", startIndex) > -1)
			{
				startIndex = text.IndexOf("\n", startIndex) + 1;
				num++;
			}
			if (num == 0)
			{
				num = 1;
			}
			Point location = InputBox.txtInput.Location;
			location.Y += num * 4;
			InputBox.txtInput.Location = location;
			Size size = InputBox.frmInputDialog.Size;
			size.Height += num * 4;
			InputBox.frmInputDialog.Size = size;
			InputBox.txtInput.SelectionStart = 0;
			InputBox.txtInput.SelectionLength = InputBox.txtInput.Text.Length;
			InputBox.txtInput.Focus();
		}

		public static InputBoxResult Show(string Prompt)
		{
			InputBox.InitializeComponent();
			InputBox.FormPrompt = Prompt;
			InputBox.LoadForm();
			InputBox.frmInputDialog.ShowDialog();
			return InputBox.OutputResponse;
		}

		public static InputBoxResult Show(string Prompt, string Title)
		{
			InputBox.InitializeComponent();
			InputBox.FormCaption = Title;
			InputBox.FormPrompt = Prompt;
			InputBox.LoadForm();
			InputBox.frmInputDialog.ShowDialog();
			return InputBox.OutputResponse;
		}

		public static InputBoxResult Show(string Prompt, string Title, string Default)
		{
			InputBox.InitializeComponent();
			InputBox.FormCaption = Title;
			InputBox.FormPrompt = Prompt;
			InputBox.DefaultValue = Default;
			InputBox.LoadForm();
			InputBox.frmInputDialog.ShowDialog();
			return InputBox.OutputResponse;
		}

		public static InputBoxResult Show(string Prompt, string Title, string Default, int XPos, int YPos)
		{
			InputBox.InitializeComponent();
			InputBox.FormCaption = Title;
			InputBox.FormPrompt = Prompt;
			InputBox.DefaultValue = Default;
			InputBox.XPosition = XPos;
			InputBox.YPosition = YPos;
			InputBox.LoadForm();
			InputBox.frmInputDialog.ShowDialog();
			return InputBox.OutputResponse;
		}
	}
}

