using System;
using System.Text;
using System.IO;

using CloudFile.CommonObjects;
using CloudFile.FilterControl;

namespace CloudFileS3Demo
{
    /// <summary>
    /// this is the struture of a file in the directory list.
    /// </summary>
    public struct FileEntry
    {
        public uint EntryLength;
        public uint Flags;        
        public uint FileAttributes;
        public long FileId;
        public long FileSize;
        public long CreationTime;
        public long LastAccessTime;
        public long LastWriteTime;
        public uint TagDataLength;
        public uint FileNameLength;
        public string FileName;
        public byte[] TagData;
    }

    public class LocalVirtualFileHandler
    {
        static System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
        static string AssemblyPath = Path.GetDirectoryName(assembly.Location);

        /// <summary>
        /// This is the cache folder to store the directory list cache files.
        /// </summary>
        public static string CacheFolder = AssemblyPath + "\\cacheFolder";
        /// <summary>
        /// this is the folder to store all the physical files for the virtual folder test.
        /// </summary>
        public static string TestSourceFolder = AssemblyPath + "\\TestSourceFolder";
        /// <summary>
        /// This is the test cloud folder which will simulate the files from the cloud.
        /// </summary>
        public static string TestCloudFolder = AssemblyPath + "\\TestCloudFolder";
        /// <summary>
        /// the cache directory file list name of the cloud folder.
        /// </summary>
        public static string DirFileListName = "dirFileList.dat";


        public static bool CreateTestFiles()
        {
            bool retVal = true;

            try
            {
                if (!Directory.Exists(TestCloudFolder))
                {
                    Directory.CreateDirectory(TestCloudFolder);
                }

                if (!Directory.Exists(CacheFolder))
                {
                    Directory.CreateDirectory(CacheFolder);
                }

                if (!Directory.Exists(TestSourceFolder))
                {
                    Directory.CreateDirectory(TestSourceFolder);
                }

                //create the test file here.                    
                for (int i = 1; i < 11; i++)
                {
                    string testStr = string.Empty;
                    byte[] buffer = new byte[i * 2048];
                    for (int j = 0; j < buffer.Length; j++)
                    {
                        int rem = 0;
                        Math.DivRem(j, 26, out rem);
                        testStr += (char)('a' + rem);
                        if (rem == 0)
                        {
                            testStr += Environment.NewLine;
                        }
                    }

                    string testFileName = Path.Combine(TestSourceFolder, "cloudFile." + i.ToString() + ".txt");
                    File.AppendAllText(testFileName, testStr);
                }


            }
            catch (Exception ex)
            {
                retVal = false;
                Console.WriteLine("Clear cache file failed with error " + ex.Message);
            }

            return retVal;
        }

        private static string GetFolderListCacheFileName(string directoryName)
        {
            try
            {
                string cacheFolder = TestSourceFolder;

                string cacheDirName = directoryName.Replace(":\\", "\\");
                cacheDirName = Path.Combine(cacheFolder, cacheDirName);

                if (!Directory.Exists(cacheDirName))
                {
                    Directory.CreateDirectory(cacheDirName);
                }

                cacheDirName = Path.Combine(cacheDirName, DirFileListName);

                return cacheDirName;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(60, "GetFolderListCacheFileName", EventLevel.Error, "Get cloud folder:" + directoryName + " file name exception:" + ex.Message);
                return "";
            }
        }

        private static void WriteFileEntry(FileStream fs, FileEntry fileEntry)
        {
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(fileEntry.EntryLength);
            bw.Write(fileEntry.Flags);
            bw.Write(fileEntry.FileAttributes);
            bw.Write(fileEntry.FileId);
            bw.Write(fileEntry.FileSize);
            bw.Write(fileEntry.CreationTime);
            bw.Write(fileEntry.LastAccessTime);
            bw.Write(fileEntry.LastWriteTime);
            bw.Write(fileEntry.TagDataLength);
            bw.Write(fileEntry.FileNameLength);
            if (fileEntry.FileNameLength > 0)
            {
                byte[] fileNameArray = UnicodeEncoding.Unicode.GetBytes(fileEntry.FileName);
                bw.Write(fileNameArray);
            }
            if (fileEntry.TagDataLength > 0)
            {
                bw.Write(fileEntry.TagData);
            }

        }


        public static  bool GetFileList(string directoryName, ref string cacheDirName )
        {
            FileStream fs = null;

            try
            {
                //for the test, we only get the file list from the source folder.
                //you can get the file list from your database or remote site.
                string sourceDirectoryName = TestSourceFolder;

                if (directoryName.ToLower().StartsWith(TestCloudFolder.ToLower()))
                {
                    if (directoryName.Length > TestCloudFolder.Length)
                    {
                        sourceDirectoryName = Path.Combine(sourceDirectoryName, directoryName.Substring(TestCloudFolder.Length + 1));
                    }
                }
                else
                {
                    EventManager.WriteMessage(120, "GetFileList", EventLevel.Error, directoryName + " doesn't match with the CloudFolder:" + TestCloudFolder);
                    return false;
                }

                cacheDirName = GetFolderListCacheFileName(directoryName);

                fs = new FileStream(cacheDirName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

                int currerentFolderFileId = 0;

                if (Directory.Exists(sourceDirectoryName))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectoryName);
                    int fileCount = 0;
                    int dirCount = 0;
                    foreach (DirectoryInfo dirInfo in directoryInfo.GetDirectories())
                    {
                        FileEntry fileEntry = new FileEntry();

                        fileEntry.Flags = 0; //reserve field
                        fileEntry.FileId = ++currerentFolderFileId;
                        fileEntry.FileAttributes = (uint)FileAttributes.Directory;
                        fileEntry.FileName = dirInfo.Name;
                        fileEntry.FileNameLength = (uint)dirInfo.Name.Length * 2;
                        fileEntry.FileSize = 0;
                        fileEntry.CreationTime = dirInfo.CreationTime.ToFileTime();
                        fileEntry.LastAccessTime = dirInfo.LastAccessTime.ToFileTime();
                        fileEntry.LastWriteTime = dirInfo.LastWriteTime.ToFileTime();

                        //this is the custom tag data which associated to every file, you can put data here,
                        //when the user is going to read the virtual file, the tag data will be provided by the filter driver.
                        fileEntry.TagData = UnicodeEncoding.Unicode.GetBytes("\\??\\" + dirInfo.FullName);
                        fileEntry.TagDataLength = (uint)fileEntry.TagData.Length;


                        //the whole file entry size 
                        uint FILE_ENTRY_STRUCT_SIZE = 4/*entryLength*/ + 4/*flags*/ + 4/*FileAttributes*/ + 8/*Index*/+ 8/*FileSize*/ + 8/*creationTime*/
                                                  + 8/*LastAccessTime*/ + 8/*LastWriteTime*/  + 4/*TagDataLength*/ + 4/*FileNameLength*/ ;

                        fileEntry.EntryLength = FILE_ENTRY_STRUCT_SIZE + (uint)dirInfo.Name.Length * 2 + (uint)fileEntry.TagDataLength;

                        dirCount++;

                        WriteFileEntry(fs, fileEntry);

                    }

                    foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                    {
                        FileEntry fileEntry = new FileEntry();

                        fileEntry.Flags = 0; //reserve field
                        fileEntry.FileId = ++currerentFolderFileId;
                        fileEntry.FileAttributes = (uint)fileInfo.Attributes | (uint)FileAttributes.Offline;
                        fileEntry.FileName = fileInfo.Name;
                        fileEntry.FileNameLength = (uint)fileInfo.Name.Length * 2;
                        fileEntry.FileSize = fileInfo.Length;
                        fileEntry.CreationTime = fileInfo.CreationTime.ToFileTime();
                        fileEntry.LastAccessTime = fileInfo.LastAccessTime.ToFileTime();
                        fileEntry.LastWriteTime = fileInfo.LastWriteTime.ToFileTime();

                        //this is the custom tag data which associated to every file, you can put data here,
                        //when the user is going to read the cloud file, the tag data will be provided by the filter driver.
                        fileEntry.TagData = UnicodeEncoding.Unicode.GetBytes("\\??\\" + fileInfo.FullName);
                        fileEntry.TagDataLength = (uint)fileEntry.TagData.Length;


                        //the whole file entry size 
                        uint FILE_ENTRY_STRUCT_SIZE = 4/*entryLength*/ + 4/*flags*/ + 4/*FileAttributes*/ + 8/*Index*/+ 8/*FileSize*/ + 8/*creationTime*/
                                                  + 8/*LastAccessTime*/ + 8/*LastWriteTime*/  + 4/*TagDataLength*/ + 4/*FileNameLength*/ ;

                        fileEntry.EntryLength = FILE_ENTRY_STRUCT_SIZE + (uint)fileInfo.Name.Length * 2 + (uint)fileEntry.TagDataLength;

                        fileCount++;

                        WriteFileEntry(fs, fileEntry);

                    }

                   Console.WriteLine("Request test cloud folder " + directoryName + "\nFile list cache file name:" + cacheDirName + "\ntotalFolders:" + dirCount + ",totalFiles:" + fileCount + "\r\n");
                }
                else
                {
                    EventManager.WriteMessage(200, "GetFileList", EventLevel.Error, "Get directory " + directoryName + " source folder:" + sourceDirectoryName + " doesn't exist");
                }

                return true;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(210, "GetFileList", EventLevel.Error, "Get directory " + directoryName + " file list got exception:" + ex.Message);
                return false;
            }
            finally
            {
                if (null != fs)
                {
                    fs.Flush();
                    fs.Close();
                }
            }

        }       
      
    }

}
