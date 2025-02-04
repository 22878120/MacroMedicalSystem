#region License

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

using System.Collections.Generic;
using System.Runtime.Serialization;
using ClearCanvas.Common.Serialization;
using ClearCanvas.Enterprise.Common;
using System;

namespace ClearCanvas.Ris.Application.Common
{
	[DataContract]
	public class ProcedureDetail : DataContractBase
	{
		[DataMember]
		public EntityRef ProcedureRef;

		[DataMember]
		public EnumValueInfo Status;

		[DataMember]
		public ProcedureTypeSummary Type;

		[DataMember]
		public DateTime? ScheduledStartTime;

		[DataMember]
		public EnumValueInfo SchedulingCode;

		[DataMember]
		public DateTime? StartTime;

		[DataMember]
		public DateTime? EndTime;

		[DataMember]
		public DateTime? CheckInTime;

		[DataMember]
		public DateTime? CheckOutTime;

		[DataMember]
		public FacilitySummary PerformingFacility;

		[DataMember]
		public DepartmentSummary PerformingDepartment;

		[DataMember]
		public EnumValueInfo Laterality;

		[DataMember]
		public EnumValueInfo ImageAvailability;

		[DataMember]
		public bool Portable;

		[DataMember]
		public List<ProcedureStepDetail> ProcedureSteps;

		[DataMember]
		public ProtocolDetail Protocol;

		[DataMember]
		public string StudyInstanceUid;
	}
}