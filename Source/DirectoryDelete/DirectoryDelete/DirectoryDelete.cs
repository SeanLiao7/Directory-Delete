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

        public event Action OnDeleteWorkFinished;
        public event Action OnDeleteWorkStart;

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
                    MessageBox.Show ( "刪除完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification );
                    OnDeleteWorkFinished?.Invoke ( );
                } );
            }
            else
                MessageBox.Show ( $"必須選擇目標資料夾與欲刪除字元\n當前資料夾路徑：{ _targetDirectoryPath }\n當前刪除字元：{ _targetDeleteString }", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification );
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


