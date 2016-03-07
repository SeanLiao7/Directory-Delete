using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryDelete
{
    public partial class DirectoryDeleteForm : Form
    {
        public DirectoryDeleteForm ( )
        {
            InitializeComponent ( );
        }

        private void btnChooseDirectory_Click ( object sender, EventArgs e )
        {
            var folderDialog = new FolderBrowserDialog ( );
            var dialogResult = folderDialog.ShowDialog ( );

            if ( dialogResult == DialogResult.OK )
                txtTargetDirectoryPath.Text = folderDialog.SelectedPath;
        }

        private void btnDelete_Click ( object sender, EventArgs e )
        {
            var deleteTargetDirectoryPath = txtTargetDirectoryPath.Text;
            var targetDeleteString = txtDeleteTarget.Text;
            var dirDelete = new DirectoryDelete ( deleteTargetDirectoryPath, targetDeleteString );
            dirDelete.OnDeleteWorkStart += DirDelete_OnDeleteWorkStart;
            dirDelete.OnDeleteWorkFinished += DirDelete_OnDeleteWorkFinished;
            dirDelete.delete ( );
        }

        private void DirDelete_OnDeleteWorkStart ( )
        {
            if ( InvokeRequired )
                Invoke ( new Action ( ( ) => pnlMain.Enabled = false ) );
            else
                pnlMain.Enabled = false;
        }

        private void DirDelete_OnDeleteWorkFinished ( )
        {
            if ( InvokeRequired )
                Invoke ( new Action ( ( ) => pnlMain.Enabled = true ) );
            else
                pnlMain.Enabled = true;
        }
    }
}
