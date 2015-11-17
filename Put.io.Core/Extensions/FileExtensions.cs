using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Put.io.Core.Extensions
{
	class FileExtensions
	{
		public static async Task<FileStream> ReadFromFile(string fileName)
		{
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
			return (await file.OpenReadAsync()).AsStreamForRead() as FileStream;
		}
	}
}
