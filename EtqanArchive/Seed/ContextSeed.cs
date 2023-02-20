using DataLayer.Security.TableEntity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Classes.Common;
using Classes.Comparer;
using GenericRepositoryCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using EtqanArchive.DataLayer;
using EtqanArchive.DataLayer.TableEntity;

namespace EtqanArchive.BackEnd.Seed
{
    public static class ContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            EtqanArchiveDBContext context = new EtqanArchiveDBContext();
            context.Database.Migrate();
            var adminUserId = new Guid("7E0E3860-9CA8-4816-AD6B-AAE73A310FDA");
            await SeedSecurityAsync(userManager, context, adminUserId);
            await SeedAdministrationAsync(context, adminUserId, configuration);
            await SeedTestingDataAsync(userManager, context, adminUserId, configuration);

            context.SaveChanges();
        }

        private static async Task SeedSecurityAsync(UserManager<User> userManager, EtqanArchiveDBContext context, Guid adminUserId)
        {

            #region User Type
            List<UserType> userTypes = new List<UserType>()
            {
                new UserType()
                {
                    UserTypeId = DBEnums.UserType.Super_System_Administrator,
                    UserTypeName = "Super System Administrator",
                    UserTypeAltName = "مدير النظام الأعلى",
                },
                new UserType()
                {
                    UserTypeId = DBEnums.UserType.System_Administrator,
                    UserTypeName = "System Administrator",
                    UserTypeAltName = "مدير النظام"
                },
            };

            SeedEntities(userTypes, context, new UserTypeComparer(), addItems: true, deleteItems: false, updateItems: false);
            #endregion

            #region Role
            List<Role> roles = new List<Role>()
            {

                new Role()
                {
                    Id = new Guid("3231F175-643B-432B-9C3B-08DA32523BA1"),
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                },
            };

            //SeedEntities(roles, context, new RoleComparer(), addItems: false, deleteItems: false, updateItems: false);
            SeedEntities(roles, context, new RoleComparer(), addItems: true,  deleteItems: false, updateItems: false);
            #endregion

            #region AdminUser
            //Seed Default User
            var adminUser = new User
            {
                Id = adminUserId,
                UserName = "admin@etqan.com",
                Email = "admin@etqan.com",
                UserFullName = "Etqan Admin",
                UserAltFullName = "مشرف إتقان",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                AllowAccess = true,
                UserTypeId = DBEnums.UserType.Super_System_Administrator,
                CreateUserId = adminUserId,
                CreateDate = DateTime.Now,
            };

            if (!userManager.Users.Any(u => u.Id == adminUser.Id))
            {
                var user = await userManager.FindByEmailAsync(adminUser.Email);
                if (user == null)
                {
                    IdentityResult result = await userManager.CreateAsync(adminUser, "123456789");
                    if (result.Succeeded)
                    {
                        User admin = await userManager.FindByIdAsync(adminUserId.ToString());
                        await userManager.AddToRoleAsync(admin, DBEnums.Roles.Admin);
                    }
                }
            }

            #endregion
        }

        private static async Task SeedAdministrationAsync(EtqanArchiveDBContext context, Guid adminUserId, IConfiguration configuration)
        {
            string fileStorageUrl = configuration["AppFile:FileStorageUrl"];

            #region Category
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    CategoryId = Guid.Parse("24FEB5EF-0C52-4882-99D1-1C6ED10852EF"),
                    CategoryName = "أنيمشن 3D",
                    CategoryAltName = "أنيمشن 3D",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new Category()
                {
                    CategoryId = Guid.Parse("68E419B7-0D9B-4D93-989C-609F9A17C428"),
                    CategoryName = "تصوير كاميرا",
                    CategoryAltName = "تصوير كاميرا",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
            };

            SeedEntities(categories, context, new CategoryComparer(), updateItems: true);
            #endregion

            #region ContentType
            Guid imageContentTypeId = Guid.Parse("EE8A314F-2DBF-4567-BD81-7A60202AADF4");
            

            List<ContentType> contentTypes = new List<ContentType>()
            {
                new ContentType()
                {
                    ContentTypeId = DBEnums.ContentType.Other,
                    ContentTypeName = "Other",
                    ContentTypeAltName = "Other",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new ContentType()
                {
                    ContentTypeId = DBEnums.ContentType.Video,
                    ContentTypeName = "فيديو",
                    ContentTypeAltName = "فيديو",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new ContentType()
                {
                    ContentTypeId = imageContentTypeId,
                    ContentTypeName = "صورة",
                    ContentTypeAltName = "صورة",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new ContentType()
                {
                    ContentTypeId = Guid.Parse("1A484414-87E2-4791-82F9-39D3F82AD138"),
                    ContentTypeName = "ملف كتابى",
                    ContentTypeAltName = "ملف كتابى",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new ContentType()
                {
                    ContentTypeId = Guid.Parse("F0876A9F-7C00-44B2-B113-08593642DB65"),
                    ContentTypeName = "Project 3D",
                    ContentTypeAltName = "Project 3D",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new ContentType()
                {
                    ContentTypeId = Guid.Parse("B4CAE285-6225-49B0-98D8-86FB55FD5264"),
                    ContentTypeName = "Design",
                    ContentTypeAltName = "Design",
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                
            };

            SeedEntities(contentTypes, context, new ContentTypeComparer(), updateItems: true);
            #endregion

            #region FileExtension
            List<FileExtension> fileExtensions = new List<FileExtension>()
            {
                new FileExtension()
                {
                    FileExtensionId = DBEnums.FileExtension.Unknown,
                    FileExtensionName = "unknown",
                    FileExtensionAltName = "unknown",
                    ContentTypeId = DBEnums.ContentType.Other,
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new FileExtension()
                {
                    FileExtensionId = Guid.Parse("BF508A23-764D-4ECC-8F83-C0AB39B30CBA"),
                    FileExtensionName = "mp4",
                    FileExtensionAltName = "mp4",
                    ContentTypeId = DBEnums.ContentType.Video,
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new FileExtension()
                {
                    FileExtensionId = Guid.Parse("9DDA90F2-E7A2-4B7D-AC90-DD2273B39402"),
                    FileExtensionName = "png",
                    FileExtensionAltName = "png",
                    ContentTypeId = imageContentTypeId,
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },

                new FileExtension()
                {
                    FileExtensionId = Guid.Parse("B8B8700B-2DB0-4267-AEF3-05458FA026EE"),
                    FileExtensionName = "txt",
                    FileExtensionAltName = "txt",
                    ContentTypeId = Guid.Parse("1A484414-87E2-4791-82F9-39D3F82AD138"),
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
                new FileExtension()
                {
                    FileExtensionId = Guid.Parse("6CC68FA6-98F2-4C80-A67F-D050928F6EE0"),
                    FileExtensionName = "docx",
                    FileExtensionAltName = "docx",
                    ContentTypeId = Guid.Parse("1A484414-87E2-4791-82F9-39D3F82AD138"),
                    CreateDate = DateTime.Now,
                    CreateUserId = adminUserId,
                },
            };
            SeedEntities(fileExtensions, context, new FileExtensionComparer());
            #endregion


            await Task.FromResult(0);
        }

        private static async Task SeedTestingDataAsync(UserManager<User> userManager, EtqanArchiveDBContext context, Guid adminUserId, IConfiguration configuration)
        {

            #region Project
            List<Project> projects = new List<Project>()
            {
                new Project()
                {
                    ProjectId = Guid.Parse("4A65A56D-9BFD-4069-A95E-A68C42B6248C"),
                    ProjectName = "درب الحرمين",
                    ProjectAltName = "درب الحرمين",
                    ProjectLocation = "مكة المكرمة , السعودية",
                    ProjectAltLocation = "مكة المكرمة , السعودية",
                    CreateUserId = adminUserId,
                    CreateDate = DateTime.Now,
                },
                new Project()
                {
                    ProjectId = Guid.Parse("26B424F2-957B-41F4-95CD-D45D4E266FD2"),
                    ProjectName = "فندق الساعة",
                    ProjectAltName = "فندق الساعة",
                    ProjectLocation = "مكة المكرمة , السعودية",
                    ProjectAltLocation = "مكة المكرمة , السعودية",
                    CreateUserId = adminUserId,
                    CreateDate = DateTime.Now,
                },
            };
            SeedEntities(projects, context, new ProjectComparer());
            #endregion

            #region ProjectFile
            List<ProjectFile> projectFiles = new List<ProjectFile>()
            {
                new ProjectFile()
                {
                    ProjectFileId = Guid.Parse("0E8C94A2-A492-43EA-AFDD-B7F0B2A4CF0B"),
                    ProjectId = Guid.Parse("26B424F2-957B-41F4-95CD-D45D4E266FD2"),
                    CategoryId = Guid.Parse("68E419B7-0D9B-4D93-989C-609F9A17C428"),
                    //ContentTypeId = Guid.Parse("EE8A314F-2DBF-4567-BD81-7A60202AADF4"),
                    FileExtensionId = Guid.Parse("9DDA90F2-E7A2-4B7D-AC90-DD2273B39402"),
                    ContentTitle = "منظر داخلى كامل لفندق الساعة",
                    ContentAltTitle = "منظر داخلى كامل لفندق الساعة",
                    ContentDescription = "تفاصيل منظر داخلى كامل لفندق الساعة",
                    ContentAltDescription  = "تفاصيل منظر داخلى كامل لفندق الساعة",
                    KeyWords = "png,image,picture",
                    FileName = "1.png",
                    FilePath = "C:\\Users\\Ahmed Shams\\Pictures\\Screenshots\\1.png",
                    Note = "ملاحظات",
                    ProductionDate = DateTime.Now,
                    CreateUserId = adminUserId,
                    CreateDate = DateTime.Now,
                },
            };
            SeedEntities(projectFiles, context, new ProjectFileComparer());
            #endregion

            await Task.FromResult(0);
        }

        private static void SeedEntities<T>(List<T> newEntities, EtqanArchiveDBContext context, IEntityEqualityComparer<T> comparer, bool addItems = true, bool deleteItems = false, bool updateItems = false) where T : class, new()
        {
            var genericRepo = new GenericRepositoryCore<T>(context);
            var dbEntities = genericRepo.GetAll().ToList();
            //insert entities
            if (addItems)
            {
                var insertEntities = newEntities.Except(dbEntities, comparer);
                if (insertEntities.Any())
                {
                    genericRepo.Insert(insertEntities, saveChanges: false);
                }
            }

            //delete entities
            if (deleteItems)
            {
                var deleteEntities = dbEntities.Except(newEntities, comparer);
                if (deleteEntities.Any())
                {
                    genericRepo.DeleteForever_Entities(deleteEntities, saveChanges: false);
                }
            }

            //updated entities
            if (updateItems)
            {
                var existEntities = dbEntities.Intersect(newEntities, comparer);
                if (existEntities.Any())
                {
                    foreach (var oldItem in existEntities)
                    {
                        T newItem = newEntities.SingleOrDefault(comparer.GetEquailtyPredicate(oldItem));
                        if (comparer.IsChanged(oldItem, newItem))
                        {
                            genericRepo.Update(oldItem, newItem, saveChanges: false);
                        }
                    }
                }
            }

            context.SaveChanges();

        }
    }
}
