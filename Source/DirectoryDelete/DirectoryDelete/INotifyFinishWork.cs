using System;

namespace DirectoryDelete
{
    interface INotifyDeleteWorkStatus
    {
        event Action OnDeleteWorkStart;

        event Action OnDeleteWorkFinished;
    }
}
