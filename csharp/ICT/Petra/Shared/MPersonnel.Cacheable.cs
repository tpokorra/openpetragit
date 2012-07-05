// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2012 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//

using System;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Personnel Module, specifically Unit submodule
    /// </summary>
    public enum TCacheableUnitTablesEnum
    {
        /// <summary>
        /// Contains the different position which exist within our organisation, e.g. Field Leader, Book Keeper, Computer support
        /// </summary>
        PositionList,

        /// <summary>
        /// Describes whether a person is full-time, part-time, etc
        /// </summary>
        JobAssignmentTypeList,

        /// <summary>
        /// Describes the reason a person left a particular position
        /// </summary>
        LeavingCodeList,
        /// <summary>
        /// List of all outreaches
        /// </summary>
        OutreachList,

        /// <summary>
        /// List of all conferences
        /// </summary>
        ConferenceList
    };
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Personnel Module, specifically Person submodule
    /// </summary>
    public enum TCacheablePersonTablesEnum
    {
        /// <summary>
        /// Contains the statuses that are be used for commitments
        /// </summary>
        CommitmentStatusList,

        /// <summary>
        /// Contains the codes that indicate the types of documents for a person
        /// </summary>
        DocumentTypeList,

        /// <summary>
        /// Contains the codes that indicate the categories of document types (grouping)
        /// </summary>
        DocumentTypeCategoryList,

        /// <summary>
        /// Contains the codes that indicate the areas of ability for a Person
        /// </summary>
        AbilityAreaList,

        /// <summary>
        /// Contains the degrees to which an ability is possessed, e.g. a little or professional
        /// </summary>
        AbilityLevelList,

        /// <summary>
        /// Contains the different codes that indicate where an applicant is in the application continuum
        /// </summary>
        ApplicantStatusList,

        /// <summary>
        /// Contains the different codes that indicate the type of application
        /// </summary>
        ApplicationTypeList,

        /// <summary>
        /// Contains the codes used to indicate where the conferee is arriving or departing
        /// </summary>
        ArrivalDeparturePointList,

        /// <summary>
        /// Contains the codes that indicate a person's role and/or position at conferences and outreaches
        /// </summary>
        EventRoleList,

        /// <summary>
        /// Contains the various methods by which a person learns of our organsiation. This table can be changed to fit any field's partiular awareness programs
        /// </summary>
        ContactList,

        /// <summary>
        /// Contains the codes that indicate the level of ability a drive has
        /// </summary>
        DriverStatusList,

        /// <summary>
        /// Contains the degrees to which a language is spoken, e.g. a little, phrases, fluent
        /// </summary>
        LanguageLevelList,

        /// <summary>
        /// Contains the codes indicating the leadership potential of someone
        /// </summary>
        LeadershipRatingList,

        /// <summary>
        /// Contains the codes indicating whether groups should be together and for how long
        /// </summary>
        PartyTypeList,

        /// <summary>
        /// Contains the codes that indicate the type of passport a person holds
        /// </summary>
        PassportTypeList,

        /// <summary>
        /// Contains the codes that indicate the mode of travel being used
        /// </summary>
        TransportTypeList,

        /// <summary>
        /// Contains the areas in which a person may posess a qualification, e.g. computing or accountancy
        /// </summary>
        QualificationAreaList,

        /// <summary>
        /// Contains the levels to which a qualifications is possessed, e.g. Secondary education, Master's Degree
        /// </summary>
        QualificationLevelList,

        /// <summary>
        /// Contains the categories in which a person may posess a skill, e.g. music or leading small groups
        /// </summary>
        SkillCategoryList,

        /// <summary>
        /// Contains the levels to which a skill is possessed, e.g. beginner or accomplished
        /// </summary>
        SkillLevelList,

        /// <summary>
        /// Describes the importance of the country and target choices
        /// </summary>
        OutreachPreferenceLevelList,
        /// <summary>
        /// Contains application types for event (short term) applications
        /// </summary>
        EventApplicationTypeList,

        /// <summary>
        /// Contains application types for field (long term) applications
        /// </summary>
        FieldApplicationTypeList
    };
}
