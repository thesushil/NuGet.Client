// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.IO.MemoryMappedFiles;

namespace NuGet.Packaging
{
    public static class StreamExtensions
    {

        /**
        Only files smaller than this value will be mmap'ed
        */
        private const long MAX_MMAP_SIZE = 10 * 1024 * 1024;
        public static string CopyToFile(this Stream inputStream, string fileFullPath) => CopyToFile(inputStream, fileFullPath, null);
        public static string CopyToFile(this Stream inputStream, string fileFullPath, long? size)
        {
            if (Path.GetFileName(fileFullPath).Length == 0)
            {
                Directory.CreateDirectory(fileFullPath);
                return fileFullPath;
            }

            var directory = Path.GetDirectoryName(fileFullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(fileFullPath))
            {
                // Log and skip adding file
                return fileFullPath;
            }

            // For files of a certain size, we can do some Cleverness and mmap them instead
            // of writing directly to disk. This can increase performance by a LOT, at the
            // cost of files taking longer to get to disk -- but that's fine!
            if (size > 0 && size <= MAX_MMAP_SIZE)
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fileFullPath, FileMode.Create, null, (long)size))
                {
                    MemoryMappedViewStream mmstream = mmf.CreateViewStream();
                    inputStream.CopyTo(mmstream);
                    return fileFullPath;
                }
            }

            using (var outputStream = NuGetExtractionFileIO.CreateFile(fileFullPath))
            {
                inputStream.CopyTo(outputStream);
                return fileFullPath;
            }
        }
    }
}
