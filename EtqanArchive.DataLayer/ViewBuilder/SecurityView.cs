using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkara.DataLayer.ViewBuilder
{
    internal abstract class SecurityView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Security View Scripts

            //Create_UserInfoView
            migrationBuilder.Sql("Create VIEW [security].[UserInfoView] " +
                "AS " +
                "SELECT [User].UserId, [User].UserFullName, [User].UserAltFullName " +
                "FROM [security].[User] AS [User]");
          
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            #region Security View Scripts

            migrationBuilder.Sql("DROP VIEW [UserInfoView]");

            #endregion
        }
    }
}
