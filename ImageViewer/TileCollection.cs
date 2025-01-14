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

using System;
using Macro.Common.Utilities;

namespace Macro.ImageViewer
{
	/// <summary>
	/// A collection of <see cref="ITile"/> objects.
	/// </summary>
	public class TileCollection : ObservableList<ITile>
	{
		private bool _locked;

		/// <summary>
		/// Initializes a new instance of <see cref="TileCollection"/>.
		/// </summary>
		internal TileCollection()
		{
		}

		//NOTE: opted not to override base.IsReadOnly b/c the semantic is slightly different.
		//This is not a 'read only' list, rather the parent object has temporarily locked it.
		internal bool Locked
		{
			get { return _locked; }
			set { _locked = value; }
		}

		/// <summary>
		/// Adds the specified item to the list.
		/// </summary>
		public override void Add(ITile item)
		{
			if (_locked)
				throw new InvalidOperationException("The tile collection is locked.");

			base.Add(item);
		}

		/// <summary>
		/// Inserts <paramref name="item"/> at the specified <paramref name="index"/>.
		/// </summary>
		public override void Insert(int index, ITile item)
		{
			if (_locked)
				throw new InvalidOperationException("The tile collection is locked.");
				
			base.Insert(index, item);
		}

		/// <summary>
		/// Removes the specified <paramref name="item"/> from the list.
		/// </summary>
		/// <returns>True if the item was in the list and was removed.</returns>
		public override bool Remove(ITile item)
		{
			if (_locked)
				throw new InvalidOperationException("The tile collection is locked.");
				
			return base.Remove(item);
		}

		/// <summary>
		/// Removes the item at the specified <paramref name="index"/>.
		/// </summary>
		public override void RemoveAt(int index)
		{
			if (_locked)
				throw new InvalidOperationException("The tile collection is locked.");
				
			base.RemoveAt(index);
		}

		/// <summary>
		/// Clears the list.
		/// </summary>
		public override void Clear()
		{
			if (_locked)
				throw new InvalidOperationException("The tile collection is locked.");
				
			base.Clear();
		}

		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		public override ITile this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				if (_locked)
					throw new InvalidOperationException("The tile collection is locked.");
					
				base[index] = value;
			}
		}
	}
}
