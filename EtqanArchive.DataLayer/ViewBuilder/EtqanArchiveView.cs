using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Tazkara.DataLayer.ViewBuilder
{
    internal class EtqanArchiveView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Create_UserInfoView
            migrationBuilder.Sql("Create VIEW [security].[UserInfoView] " +
                "AS " +
                "SELECT [User].UserId, [User].UserFullName, [User].UserAltFullName " +
                "FROM [security].[User] AS [User]");

            //Create_ProjectFileView
            migrationBuilder.Sql("Create VIEW [ProjectFileView] " +
                "AS " +
                "SELECT ProjectFile.ProjectFileId, ProjectFile.FileName, ProjectFile.FilePath, ProjectFile.ContentTitle, " +
                "ProjectFile.ContentAltTitle, ProjectFile.ContentDescription, ProjectFile.ContentAltDescription, ProjectFile.KeyWords, " +
                "ProjectFile.Note, ProjectFile.ProjectId, Project.ProjectName, Project.ProjectAltName, FileExtension.ContentTypeId, " +
                "ContentType.ContentTypeName, ContentType.ContentTypeAltName, ProjectFile.CategoryId, Category.CategoryName, Category.CategoryAltName, " +
                "ProjectFile.FileExtensionId, FileExtension.FileExtensionName, FileExtension.FileExtensionAltName, ProjectFile.ProductionDate, " +
                "ProjectFile.IsBlock, (CASE WHEN ProjectFile.IsBlock = 1 THEN 'true' ELSE 'false' END) AS IsBlock_str, " +
                "ProjectFile.IsDeleted, ProjectFile.CreateUserId, ProjectFile.CreateDate, ProjectFile.ModifyUserId, ProjectFile.ModifyDate, " +
                "CreateUser.UserFullName AS CreateUser_FullName, CreateUser.UserAltFullName AS CreateUser_FullAltName, " +
                "ModifyUser.UserFullName AS ModifyUser_FullName, ModifyUser.UserAltFullName AS ModifyUser_FullAltName " +
                "FROM dbo.ProjectFile AS ProjectFile INNER JOIN " +
                "dbo.Project AS Project ON ProjectFile.ProjectId = Project.ProjectId LEFT OUTER JOIN " +
                "dbo.Category AS Category ON ProjectFile.CategoryId = Category.CategoryId LEFT OUTER JOIN " +
                "dbo.FileExtension AS FileExtension ON ProjectFile.FileExtensionId = FileExtension.FileExtensionId LEFT OUTER JOIN " +
                "dbo.ContentType AS ContentType ON FileExtension.ContentTypeId = ContentType.ContentTypeId LEFT OUTER JOIN " +
                "security.UserInfoView AS CreateUser ON ProjectFile.CreateUserId = CreateUser.UserId LEFT OUTER JOIN " +
                "security.UserInfoView AS ModifyUser ON ProjectFile.ModifyUserId = ModifyUser.UserId ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [UserInfoView]");

            migrationBuilder.Sql("DROP VIEW [ProjectFileView]");
        }

    }

}
