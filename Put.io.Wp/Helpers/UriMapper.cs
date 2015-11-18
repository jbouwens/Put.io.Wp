using System;
using System.Windows.Navigation;
using Windows.Phone.Storage.SharedAccess;

namespace Put.io.Wp.Helpers
{
    public class UriMapper : UriMapperBase
    {
        private string _tempUri;

        public override Uri MapUri(Uri uri)
        {
            _tempUri = uri.ToString();

            if (_tempUri.Contains("/FileTypeAssociation"))
            {
                var fileIdIndex = _tempUri.IndexOf("fileToken=", StringComparison.Ordinal) + 10;
                var fileId = _tempUri.Substring(fileIdIndex);
                var incomingFileName =
                    SharedStorageAccessManager.GetSharedFileName(fileId);
                var extensionIndex = incomingFileName.LastIndexOf('.') + 1;
                var incomingFileType =
                    incomingFileName.Substring(extensionIndex).ToLower();

                switch (incomingFileType)
                {
                    case "torrent":
                        return new Uri("/Views/MainPage.xaml?fileToken=" + fileId, UriKind.Relative);
                    default:
                        return uri;
                }
            }
            if (_tempUri.Contains("magnet:"))
            {
                return uri;
            }
            return uri;
        }
    }
}