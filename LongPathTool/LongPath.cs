/*
 * Created by SharpDevelop.
 * User: Bogdan
 * Date: 12/22/2022
 * Time: 2:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace LongPathTool
{
	public static class LongPath
	{
		public enum ECreationDisposition : uint
		{
			New = 1u,
			CreateAlways,
			OpenExisting,
			OpenAlways,
			TruncateExisting
		}

		[Flags]
		public enum EFileAccess : uint
		{
			GenericRead = 2147483648u,
			GenericWrite = 1073741824u,
			GenericExecute = 536870912u,
			GenericAll = 268435456u
		}

		[Flags]
		public enum EFileAttributes : uint
		{
			Readonly = 1u,
			Hidden = 2u,
			System = 4u,
			Directory = 16u,
			Archive = 32u,
			Device = 64u,
			Normal = 128u,
			Temporary = 256u,
			SparseFile = 512u,
			ReparsePoint = 1024u,
			Compressed = 2048u,
			Offline = 4096u,
			NotContentIndexed = 8192u,
			Encrypted = 16384u,
			Write_Through = 2147483648u,
			Overlapped = 1073741824u,
			NoBuffering = 536870912u,
			RandomAccess = 268435456u,
			SequentialScan = 134217728u,
			DeleteOnClose = 67108864u,
			BackupSemantics = 33554432u,
			PosixSemantics = 16777216u,
			OpenReparsePoint = 2097152u,
			OpenNoRecall = 1048576u,
			FirstPipeInstance = 524288u
		}

		[Flags]
		public enum EFileShare : uint
		{
			None = 0u,
			Read = 1u,
			Write = 2u,
			Delete = 4u
		}

		internal struct FILETIME
		{
			internal uint dwHighDateTime;

			internal uint dwLowDateTime;
		}

		public struct SECURITY_ATTRIBUTES
		{
			public int bInheritHandle;

			public IntPtr lpSecurityDescriptor;

			public int nLength;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WIN32_FIND_DATA
		{
			internal FileAttributes dwFileAttributes;
			
			internal LongPath.FILETIME ftCreationTime;

			internal LongPath.FILETIME ftLastAccessTime;

			internal LongPath.FILETIME ftLastWriteTime;
			
			internal int nFileSizeHigh;

			internal int nFileSizeLow;
			
			internal int dwReserved0;
			internal int dwReserved1;
			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string cFileName;
			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			internal string cAlternate;

    public uint dwFileType;
    public uint dwCreatorType;
    public uint wFinderFlags;






		}

		internal static int FILE_ATTRIBUTE_DIRECTORY = 16;

		internal static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		internal const int MAX_PATH = 260;

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool CreateDirectoryEx(string lpTemplateDirectory, string lpNewDirectory, IntPtr lpSecurityAttributes);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string lpFileName, LongPath.EFileAccess dwDesiredAccess, LongPath.EFileShare dwShareMode, IntPtr lpSecurityAttributes, LongPath.ECreationDisposition dwCreationDisposition, LongPath.EFileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteFile(string lpFileName);

		public static bool DeleteFolderPathTooLong(string fPath)
		{
			bool result;
			try
			{
				LongPath.DeleteFile(fPath);
				LongPath.RemoveDirectory(fPath);
				result = true;
				return result;
			}
			catch
			{
			}
			result = false;
			return result;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool FindClose(IntPtr hFindFile);

		public static List<string> FindFiles(string dirName)
		{
			List<string> list = new List<string>();
			LongPath.WIN32_FIND_DATA wIN32_FIND_DATA;
			IntPtr intPtr = LongPath.FindFirstFile(dirName + "\\*", out wIN32_FIND_DATA);
			if (intPtr != LongPath.INVALID_HANDLE_VALUE)
			{
				do
				{
					string cFileName = wIN32_FIND_DATA.cFileName;
					if ((wIN32_FIND_DATA.dwFileAttributes & (FileAttributes)LongPath.FILE_ATTRIBUTE_DIRECTORY) != (FileAttributes)0)
					{
						if (cFileName != "." && cFileName != "..")
						{
							List<string> list2 = LongPath.FindFilesAndDirs(Path.Combine(dirName, cFileName));
						}
					}
					else
					{
						list.Add(Path.Combine(dirName, cFileName));
					}
				}
				while (LongPath.FindNextFile(intPtr, out wIN32_FIND_DATA));
			}
			LongPath.FindClose(intPtr);
			return list;
		}

		public static List<string> FindFilesAndDirs(string dirName)
		{
			List<string> list = new List<string>();
			LongPath.WIN32_FIND_DATA wIN32_FIND_DATA;
			IntPtr intPtr = LongPath.FindFirstFile(dirName + "\\*", out wIN32_FIND_DATA);
			if (intPtr != LongPath.INVALID_HANDLE_VALUE)
			{
				do
				{
					string cFileName = wIN32_FIND_DATA.cFileName;
					if ((wIN32_FIND_DATA.dwFileAttributes & (FileAttributes)LongPath.FILE_ATTRIBUTE_DIRECTORY) != (FileAttributes)0)
					{
						if (cFileName != "." && cFileName != "..")
						{
							List<string> collection = LongPath.FindFilesAndDirs(Path.Combine(dirName, cFileName));
							list.AddRange(collection);
							list.Add(Path.Combine(dirName, cFileName));
						}
					}
					else
					{
						list.Add(Path.Combine(dirName, cFileName));
					}
				}
				while (LongPath.FindNextFile(intPtr, out wIN32_FIND_DATA));
			}
			LongPath.FindClose(intPtr);
			return list;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr FindFirstFile(string lpFileName, out LongPath.WIN32_FIND_DATA lpFindFileData);

		public static List<string> FindFolders(string dirName)
		{
			List<string> list = new List<string>();
			LongPath.WIN32_FIND_DATA wIN32_FIND_DATA;
			IntPtr intPtr = LongPath.FindFirstFile(dirName + "\\*", out wIN32_FIND_DATA);
			if (intPtr != LongPath.INVALID_HANDLE_VALUE)
			{
				do
				{
					string cFileName = wIN32_FIND_DATA.cFileName;
					if ((wIN32_FIND_DATA.dwFileAttributes & (FileAttributes)LongPath.FILE_ATTRIBUTE_DIRECTORY) != (FileAttributes)0)
					{
						if (cFileName != "." && cFileName != "..")
						{
							List<string> collection = LongPath.FindFilesAndDirs(Path.Combine(dirName, cFileName));
							list.AddRange(collection);
							list.Add(Path.Combine(dirName, cFileName));
						}
					}
				}
				while (LongPath.FindNextFile(intPtr, out wIN32_FIND_DATA));
			}
			LongPath.FindClose(intPtr);
			return list;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool FindNextFile(IntPtr hFindFile, out LongPath.WIN32_FIND_DATA lpFindFileData);

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool RemoveDirectory(string lpPathName);

		public static void TestCreateAndWrite(string fileName)
		{
			string lpFileName = "\\\\?\\" + fileName;
			SafeFileHandle safeFileHandle = LongPath.CreateFile(lpFileName, LongPath.EFileAccess.GenericWrite, LongPath.EFileShare.None, IntPtr.Zero, LongPath.ECreationDisposition.CreateAlways, (LongPath.EFileAttributes)0u, IntPtr.Zero);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (safeFileHandle.IsInvalid)
			{
				throw new Win32Exception(lastWin32Error);
			}
			using (FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write))
			{
				fileStream.WriteByte(80);
				fileStream.WriteByte(81);
				fileStream.WriteByte(83);
				fileStream.WriteByte(84);
			}
		}
	}
}
