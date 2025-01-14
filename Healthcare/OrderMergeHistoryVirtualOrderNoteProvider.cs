﻿#region License

// Copyright (c) 2013, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
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
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare.Brokers;

namespace ClearCanvas.Healthcare
{
	[ExtensionOf(typeof(VirtualOrderNoteProviderExtensionPoint))]
	public class OrderMergeHistoryVirtualOrderNoteProvider : IOrderNoteProvider
	{
		#region Helper classes

		/// <summary>
		/// Represents a tuple of (DateTime, Staff)
		/// </summary>
		internal struct TimeStaffPair : IEquatable<TimeStaffPair>
		{
			internal Staff Staff { get; set; }
			internal DateTime Time { get; set; }

			public bool Equals(TimeStaffPair other)
			{
				return Equals(other.Staff, Staff) && other.Time.Equals(Time);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (obj.GetType() != typeof(TimeStaffPair)) return false;
				return Equals((TimeStaffPair)obj);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return (Staff.GetHashCode() * 397) ^ Time.GetHashCode();
				}
			}
		}

		#endregion

		/// <summary>
		/// Gets the list of notes for the specified order.
		/// </summary>
		/// <param name="order"></param>
		/// <param name="categories"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public IList<OrderNote> GetNotes(Order order, IList<string> categories, IPersistenceContext context)
		{
			if (categories != null && !categories.Contains(OrderNote.Categories.General))
				return new List<OrderNote>();


			var notes = new List<OrderNote>();
			if (order.Status == OrderStatus.MG)
			{
				notes.Add(CreateMergeSourceNote(order));
			}
			if (order.Status == OrderStatus.RP)
			{
				notes.Add(CreateReplaceSourceNote(order));
			}

			if (order.MergeSourceOrders.Count > 0)
			{
				notes.AddRange(CreateMergeDestinationNotes(order));
			}

			notes.AddRange(CreateReplaceDestinationNotes(order, context));

			foreach (var note in notes)
			{
				note.Body = "Auto-generated note: " + note.Body;
			}

			return notes;
		}

		private static List<OrderNote> CreateMergeDestinationNotes(Order order)
		{
			var groups = CollectionUtils.GroupBy(order.MergeSourceOrders,
				o => new TimeStaffPair { Staff = o.MergeInfo.MergedBy, Time = o.MergeInfo.MergedTime.Value });

			return CollectionUtils.Map(groups,
				(KeyValuePair<TimeStaffPair, List<Order>> kvp)
					=> CreateMergeDestinationNote(kvp.Value, order, kvp.Key.Staff, kvp.Key.Time));
		}

		private static OrderNote CreateMergeDestinationNote(ICollection<Order> sourceOrders, Order order, Staff staff, DateTime time)
		{
			var text = sourceOrders.Count == 1 ?
						string.Format("Order {0} is merged into this order.", CollectionUtils.FirstElement(sourceOrders).AccessionNumber)
						: string.Format("Orders {0} are merged into this order.", StringUtilities.Combine(sourceOrders, ", ", o => o.AccessionNumber));

			return OrderNote.CreateVirtualNote(order, OrderNote.Categories.General, staff, text, time);
		}

		private static OrderNote CreateMergeSourceNote(Order order)
		{
			var destOrder = order.MergeInfo.MergeDestinationOrder;
			var text = string.Format("This order is merged into order {0}.", destOrder.AccessionNumber);
			return OrderNote.CreateVirtualNote(
				order,
				OrderNote.Categories.General,
				order.MergeInfo.MergedBy,
				text,
				order.MergeInfo.MergedTime.Value);
		}

		private static List<OrderNote> CreateReplaceDestinationNotes(Order order, IPersistenceContext context)
		{
			var where = new OrderSearchCriteria();
			where.CancelInfo.ReplacementOrder.EqualTo(order);

			// expect 0 or 1 results
			var replacedOrder = CollectionUtils.FirstElement(context.GetBroker<IOrderBroker>().Find(where));
			if (replacedOrder == null)
				return new List<OrderNote>();

			var text = string.Format("Order {0} is replaced by this order.", replacedOrder.AccessionNumber);
			var note = OrderNote.CreateVirtualNote(
				order,
				OrderNote.Categories.General,
				replacedOrder.CancelInfo.CancelledBy,
				text,
				replacedOrder.EndTime.Value);

			return new List<OrderNote> { note };
		}

		private static OrderNote CreateReplaceSourceNote(Order order)
		{
			var replacementOrder = order.CancelInfo.ReplacementOrder;
			var reason = order.CancelInfo.Reason;
			var text = string.Format("This order is replaced by order {0}. Reason: {1}",
				replacementOrder.AccessionNumber,
				reason.Value);
			
			return OrderNote.CreateVirtualNote(
				order,
				OrderNote.Categories.General,
				order.CancelInfo.CancelledBy,
				text,
				order.EndTime.Value);
		}

	}
}
