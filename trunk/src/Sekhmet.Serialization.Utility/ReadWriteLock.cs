using System;
using System.Threading;

namespace Sekhmet.Serialization.Utility
{
	/// <summary>
	/// Helper class for using a <see cref="ReaderWriterLockSlim"/> in an easy fashion (supports the using-statement).
	/// </summary>
	public sealed class ReadWriteLock : IDisposable
	{
		/// <summary>
		/// The lock to use.
		/// </summary>
		private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			if (_rwLock == null)
				return;

			_rwLock.Dispose();
			_rwLock = null;
		}

		/// <summary>
		/// Enters the read scope.
		/// </summary>
		/// <returns></returns>
		public ReadLockScope EnterReadScope()
		{
			return new ReadLockScope(_rwLock);
		}

		/// <summary>
		/// Enters the upgradeable read scope.
		/// </summary>
		/// <returns></returns>
		public UpgradeableReadLockScope EnterUpgradeableReadScope()
		{
			return new UpgradeableReadLockScope(_rwLock);
		}

		/// <summary>
		/// Enters the write scope.
		/// </summary>
		/// <returns></returns>
		public WriteLockScope EnterWriteScope()
		{
			return new WriteLockScope(_rwLock);
		}
	}
}