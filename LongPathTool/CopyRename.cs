/*
 * Created by SharpDevelop.
 * User: Bogdan
 * Date: 12/22/2022
 * Time: 4:09 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;

namespace LongPathTool
{
	public class CopyRename
	{
		[Flags]
		private enum CopyFileFlags : uint
		{
			COPY_FILE_FAIL_IF_EXISTS = 1u,
			COPY_FILE_RESTARTABLE = 2u,
			COPY_FILE_OPEN_SOURCE_FOR_WRITE = 4u,
			COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 8u
		}

		private enum CopyProgressCallbackReason : uint
		{
			CALLBACK_CHUNK_FINISHED,
			CALLBACK_STREAM_SWITCH
		}

		private enum CopyProgressResult : uint
		{
			PROGRESS_CONTINUE,
			PROGRESS_CANCEL,
			PROGRESS_STOP,
			PROGRESS_QUIET
		}

		private delegate CopyRename.CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyRename.CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

		private int pbCancel;

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyRename.CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref int pbCancel, CopyRename.CopyFileFlags dwCopyFlags);

		private CopyRename.CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize, long StreamByteTrans, uint dwStreamNumber, CopyRename.CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
		{
			return CopyRename.CopyProgressResult.PROGRESS_CONTINUE;
		}

		public void XCopy(string oldFile, string newFile)
		{
			CopyRename.CopyFileEx(oldFile, newFile, new CopyRename.CopyProgressRoutine(this.CopyProgressHandler), IntPtr.Zero, ref this.pbCancel, CopyRename.CopyFileFlags.COPY_FILE_RESTARTABLE);
		}
	}
}

