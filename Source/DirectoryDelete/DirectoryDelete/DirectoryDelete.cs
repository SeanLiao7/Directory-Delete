using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryDelete
{
    public class DirectoryDelete : INotifyDeleteWorkStatus
    {
        string _targetDirectoryPath;
        string _targetDeleteString;

        public event Action OnDeleteWorkStart;
        public event Action OnDeleteWorkFinished;

        public DirectoryDelete ( string targetDirectoryPath, string targetDeleteString )
        {
            _targetDirectoryPath = targetDirectoryPath;
            _targetDeleteString = targetDeleteString;
        }

        public void delete ( )
        {
            if ( checkInputIsValid ( ) )
            {
                OnDeleteWorkStart?.Invoke ( );
                Task.Factory.StartNew ( ( ) =>
                {
                    deleteDirectoryRecursive ( new DirectoryInfo ( _targetDirectoryPath ) );
                } ).ContinueWith ( deleteTask =>
                {
                    MessageBox.Show ( "Delete Completed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification );
                    OnDeleteWorkFinished?.Invoke ( );
                } );
            }
            else
                MessageBox.Show ( $"Directory Path and Target can not be empty!\nCurrent Path：{ _targetDirectoryPath }\nCurrent Target：{ _targetDeleteString }", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification );
        }

        private bool checkInputIsValid ( ) => _targetDirectoryPath != string.Empty && _targetDeleteString != string.Empty;

        private void deleteDirectoryRecursive ( DirectoryInfo directoryInfo )
        {
            foreach ( var dirInfo in directoryInfo.GetDirectories ( ) )
                deleteDirectoryRecursive ( dirInfo );

            if ( directoryInfo.FullName.Contains ( _targetDeleteString ) )
            {
                foreach ( var file in directoryInfo.GetFiles ( ) )
                {
                    file.Attributes = FileAttributes.Normal;
                    file.Delete ( );
                }

                directoryInfo.Attributes = FileAttributes.Normal;
                directoryInfo.Delete ( );
            }
        }
    }
}


