using Lykke.Cqrs.MessageCancellation.Services.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Cqrs.MessageCancellation.Services
{
    public class MessageCancellationService : IMessageCancellationService
    {
        private ReaderWriterLockSlim _readWriterLockSlim;
        private HashSet<string> _operationsToCancel;

        public MessageCancellationService()
        {
            _readWriterLockSlim = new ReaderWriterLockSlim();
            _operationsToCancel = new HashSet<string>();
        }

        public Task RequestMessageCancellationAsync(string messageId)
        {
            _readWriterLockSlim.EnterWriteLock();

            try
            {
                _operationsToCancel.Add(messageId);
            }
            finally
            {
                if (_readWriterLockSlim.IsWriteLockHeld)
                    _readWriterLockSlim.ExitWriteLock();
            }

            return Task.CompletedTask;
        }

        public Task RemoveMessageFromCancellationAsync(string messageId)
        {
            _readWriterLockSlim.EnterWriteLock();

            try
            {
                _operationsToCancel.Remove(messageId);
            }
            finally
            {
                if (_readWriterLockSlim.IsWriteLockHeld)
                    _readWriterLockSlim.ExitWriteLock();
            }
            return Task.CompletedTask;
        }

        public Task<bool> CheckIfOperationRequiresCancellationAsync(string messageId)
        {
            _readWriterLockSlim.EnterReadLock();

            try
            {
                bool operationRequiresCancellation = _operationsToCancel.Contains(messageId);

                return Task.FromResult(operationRequiresCancellation);
            }
            finally
            {
                if (_readWriterLockSlim.IsReadLockHeld)
                    _readWriterLockSlim.ExitReadLock();
            }
        }

        public Task<IEnumerable<string>> GetAllMessagesToCancellAsync()
        {
            _readWriterLockSlim.EnterReadLock();

            try
            {
                var enumerator = _operationsToCancel.ToImmutableArray();

                return Task.FromResult((IEnumerable<string>)enumerator);
            }
            finally
            {
                if (_readWriterLockSlim.IsReadLockHeld)
                    _readWriterLockSlim.ExitReadLock();
            }
        }
    }
}
