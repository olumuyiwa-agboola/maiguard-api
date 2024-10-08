﻿using Dapper;
using Maiguard.Core.Abstractions.IRepositories;
using Maiguard.Core.Models.Residents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maiguard.Infrastructure.Repositories
{
    public class ResidentRepository(IDbContext _dbContext) : IResidentRepository
    {
        public int ActivateResident(ResidentActivationRequest request)
        {
            int result = 0;
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("ResidentId", request.ResidentId);
            parameters.Add("IsActiveLastUpdatedBy", request.ActivatedBy);

            string query = @"
                    IF EXISTS (SELECT 1 FROM [Maiguard].[dbo].[Residents] WHERE ResidentId = @ResidentId and IsVerified = 0)
	                    BEGIN
		                    SELECT 4000;  -- Return 4000 if the resident has not been verified
	                    END
                    ELSE IF EXISTS (SELECT 1 FROM [Maiguard].[dbo].[Residents] WHERE ResidentId = @ResidentId and IsActive = 1)
	                    BEGIN
		                    SELECT 5000;  -- Return 5000 if the resident is already active
	                    END
                    ELSE
	                    BEGIN
		                    UPDATE [Maiguard].[dbo].[Residents]
		                    SET IsActive = 1
		                    WHERE ResidentId = @ResidentId;

		                    SELECT 1;
	                END";

            using (IDbConnection dbConnection = _dbContext.MaiguardDbConnection())
            {
                result = dbConnection.Query<int>(query, parameters).FirstOrDefault();
            }

            return result;
        }

        public int DeactivateResident(ResidentDeactivationRequest request)
        {
            int result = 0;
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("ResidentId", request.ResidentId);
            parameters.Add("IsActiveLastUpdatedBy", request.DeactivatedBy);

            string query = @"
                    IF EXISTS (SELECT 1 FROM [Maiguard].[dbo].[Residents] WHERE ResidentId = @ResidentId and IsVerified = 0)
	                    BEGIN
		                    SELECT 4000;  -- Return 4000 if the resident has not been verified
	                    END
                    ELSE IF EXISTS (SELECT 1 FROM [Maiguard].[dbo].[Residents] WHERE ResidentId = @ResidentId and IsActive = 0)
	                    BEGIN
		                    SELECT 5000;  -- Return 5000 if the resident is already inactive
	                    END
                    ELSE
	                    BEGIN
		                    UPDATE [Maiguard].[dbo].[Residents]
		                    SET IsActive = 0
		                    WHERE ResidentId = @ResidentId;

		                    SELECT 1;
	                END";

            using (IDbConnection dbConnection = _dbContext.MaiguardDbConnection())
            {
                result = dbConnection.Query<int>(query, parameters).FirstOrDefault();
            }

            return result;
        }

        public int AddResident(ResidentRegistrationRequest request, string residentId)
        {
            int result = 0;
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("ResidentId", residentId);
            parameters.Add("LastName", request.LastName);
            parameters.Add("FirstName", request.FirstName);
            parameters.Add("PhoneNumber", request.PhoneNumber);
            parameters.Add("OnboardedBy", request.OnboardedBy);
            parameters.Add("CommunityId", request.CommunityId);
            parameters.Add("EmailAddress", request.EmailAddress);
            parameters.Add("RelativeAddress", request.RelativeAddress);
            parameters.Add("RecordLastUpdatedBy", request.OnboardedBy);
            parameters.Add("IsActiveLastUpdatedBy", request.OnboardedBy);

            string query = @"
                            IF EXISTS (SELECT 1 FROM [Maiguard].[dbo].[Residents] WHERE ResidentId = @ResidentId)
                                BEGIN
                                    SELECT 1000;  -- Return 1000 if the resident Id already exists
                                END                        
                            ELSE IF EXISTS (SELECT 1 FROM [Maiguard].[dbo].[Residents] WHERE EmailAddress = @EmailAddress)
                                BEGIN
                                    SELECT 2000;  -- Return 2000 if the email address already exists
                                END
                            ELSE IF EXISTS (SELECT 1 FROM [Maiguard].[dbo].[Residents] WHERE PhoneNumber = @PhoneNumber)
                                BEGIN
                                    SELECT 3000;  -- Return 3000 if the phone number already exists
                                END
                            ELSE
                                BEGIN
                                    INSERT INTO [Maiguard].[dbo].[Residents] (ResidentId, CommunityId, FirstName, LastName, EmailAddress, PhoneNumber, RelativeAddress, OnboardedBy, IsActiveLastUpdatedBy, RecordLastUpdatedBy)
                                    VALUES (@ResidentId, @CommunityId, @FirstName, @LastName, @EmailAddress, @PhoneNumber, @RelativeAddress, @OnboardedBy, @IsActiveLastUpdatedBy, @RecordLastUpdatedBy);

                                    SELECT 1;
                                END";

            using (IDbConnection dbConnection = _dbContext.MaiguardDbConnection())
            {
                result = dbConnection.Query<int>(query, parameters).FirstOrDefault();
            }

            return result;
        }
    }
}
