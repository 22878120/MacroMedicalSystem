#region License

// Copyright (c) 2013, ClearCanvas Inc.
// All rights reserved.
// http://www.ClearCanvas.ca
//
// This file is part of the ClearCanvas RIS/PACS open source project.
//
// The ClearCanvas RIS/PACS open source project is free software: you can
// redistribute it and/or modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// The ClearCanvas RIS/PACS open source project is distributed in the hope that it
// will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General
// Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// the ClearCanvas RIS/PACS open source project.  If not, see
// <http://www.gnu.org/licenses/>.

#endregion

namespace Macro.Enterprise.Core
{
	/// <summary>
	/// Defines the interface to an update context.  An update context allows the application read
	/// data from a persistent store, and to synchronize the persistent store with changes made to that data.
	/// </summary>
	public interface IUpdateContext : IPersistenceContext
	{
		/// <summary>
		/// Gets or sets the change-set recorder that the context will use to create
		/// a record of the changes that were made.
		/// </summary>
		/// <remarks>
		/// Setting this property to null will effectively disable this auditing.
		/// </remarks>
		IEntityChangeSetRecorder ChangeSetRecorder { get; set; }

		/// <summary>
		/// Attempts to flush and commit all changes made within this update context to the persistent store.
		/// </summary>
		/// <remarks>
		/// If this operation succeeds, the state of the persistent store will be syncrhonized with the state
		/// of all domain objects that are attached to this context, and the context can continue to be used
		/// for read operations only. If the operation fails, an exception will be thrown.
		/// </remarks>
		void Commit();
	}
}
