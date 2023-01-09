#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class registerSuperAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
               $"INSERT INTO AspNetRoles VALUES ('0DB45C30-2FEE-47C6-AF34-7849A62B8856','SuperAdmin','SUPERADMIN','0DB45C30-2FEE-47C6-AF34-7849A62B8856')" +
               $"INSERT INTO AspNetRoles VALUES ('4e3b804b-59a8-49d0-bf8b-2c71e46e7921','CompanyAdmin','CompanyAdmin',NEWID())" +
               $"INSERT INTO AspNetRoles VALUES ('b7ee852b-fecd-4a82-ba16-40aba1fbcf28','CompanyManager','CompanyManager',NEWID())" +
               $"INSERT INTO AspNetRoles VALUES ('06b353c6-37e8-4082-81b2-306236b9fc44', 'CompanyStaff', 'CompanyStaff', NEWID())");

            migrationBuilder.Sql(
                $"INSERT INTO [dbo].[AspNetUsers] ([Id], [Discriminator], [FirstName], [LastName], [Address], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed]," +
                $"[PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]," +
                $"[MiddleName], [CompanyBranchId], [DateRegistered])" +
                $"VALUES(N'65de30ed-e458-4557-a1ac-dcdee04d8660', N'ApplicationUser', NULL, NULL, NULL, N'bivisofttest@gmail.com', N'BIVISOFTTEST@GMAIL.COM', N'bivisofttest@gmail.com', N'BIVISOFTTEST@GMAIL.COM', 0," +
                $"N'AQAAAAEAACcQAAAAEN/dkSmyab743y8KvSQiBvvRUgDcc55vqFEM6rsLlQbsmR+KeUom7pGeUqLfwqJB5A==', N'KR5BOFOCQUBRI6BRMQD4MV6YXM3KDAR3', N'8ee86907-91e3-44ad-bf7a-c2247a7f319c', NULL, 0,0,NULL,1,0," +
                $"NULL, NULL, N'2022-10-26 10:25:48')");

            migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'65de30ed-e458-4557-a1ac-dcdee04d8660', N'0DB45C30-2FEE-47C6-AF34-7849A62B8856')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
