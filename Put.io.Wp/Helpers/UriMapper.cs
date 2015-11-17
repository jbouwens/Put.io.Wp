using System;
using System.Windows.Navigation;
using Windows.Phone.Storage.SharedAccess;

namespace Helpers
{


public class UriMapper : UriMapperBase
{
	private string tempUri;

	public override Uri MapUri(Uri uri)
	{
		tempUri = uri.ToString();

		// File association launch
		if (tempUri.Contains("/FileTypeAssociation"))
		{
			// Get the file ID (after "fileToken=").
			int fileIDIndex = tempUri.IndexOf("fileToken=") + 10;
			string fileID = tempUri.Substring(fileIDIndex);

			// Get the file name.
			string incomingFileName =
				SharedStorageAccessManager.GetSharedFileName(fileID);

			// Get the file extension.
			int extensionIndex = incomingFileName.LastIndexOf('.') + 1;
			string incomingFileType =
				incomingFileName.Substring(extensionIndex).ToLower();

			// Map the .log files to the appropriate pages.
			switch (incomingFileType)
			{
				case "torrent":
					return new Uri("/Views/MainPage.xaml?fileToken=" + fileID, UriKind.Relative);
				default:
					return new Uri("/App.xaml", UriKind.Relative);
			}

		}
        if (tempUri.Contains("magnet:"))
        {
            // Otherwise perform normal launch.
            return uri;
        }
            // Otherwise perform normal launch.
            return uri;
	}
}
}