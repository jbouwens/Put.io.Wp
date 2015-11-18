using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Put.io.Core.Extensions
{
    internal abstract class FileExtensions
    {
        public static async Task<byte[]> ReadFromFile(IStorageFile file)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var reader = new DataReader(stream.GetInputStreamAt(0));

            var streamSize = (uint) stream.Size;
            await reader.LoadAsync(streamSize);
            var buffer = new byte[streamSize];
            reader.ReadBytes(buffer);
            return buffer;
        }
    }
}