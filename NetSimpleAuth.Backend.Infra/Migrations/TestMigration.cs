using FluentMigrator;
using NetPOC.Backend.Application.Helpers;

namespace NetPOC.Backend.Infra.Migrations
{
    [Migration(1)]
    public class TestMigration : Migration
    {
        public override void Up()
        {
            const string DEFAULT_USER = "admin";
            const string DEFAULT_PASSWORD = "password";
            
            Create.Table("Log")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Ip").AsString(19)
                .WithColumn("App").AsString(50).Nullable()
                .WithColumn("User").AsString(50).Nullable()
                .WithColumn("Date").AsDateTime()
                .WithColumn("RequestType").AsString(25)
                .WithColumn("RequestUrl").AsString(100)
                .WithColumn("RequestProtocol").AsString(10)
                .WithColumn("StatusCode").AsInt32()
                .WithColumn("ContentSize").AsInt32().Nullable()
                .WithColumn("ResponseUrl").AsString(200).Nullable()
                .WithColumn("UserAgent").AsString(200).Nullable();

            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(100)
                .WithColumn("LastName").AsString(100)
                .WithColumn("Email").AsString(100)
                .WithColumn("UserName").AsString(100)
                .WithColumn("Password").AsString(300)
                .WithColumn("PasswordSalt").AsString(100);

            var salt = CryptographyService.CreateSalt(64);
            var password = CryptographyService.HashPassword(DEFAULT_PASSWORD + salt);
            
            Insert.IntoTable("User")
                .Row(new
                {
                    FirstName = "Dan", LastName = "Coelho", Email = "dancoelho.contact@gmail.com",
                    UserName = DEFAULT_USER, Password = password,
                    PasswordSalt = salt
                });
            
            Create.Table("RefreshToken")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("User", "Id")
                .WithColumn("Token").AsString(200)
                .WithColumn("Expires").AsDateTime()
                .WithColumn("Created").AsDateTime()
                .WithColumn("CreatedByIp").AsString(19)
                .WithColumn("Revoked").AsDateTime().Nullable()
                .WithColumn("RevokedByIp").AsString(19).Nullable()
                .WithColumn("ReplacedByToken").AsString(200).Nullable()
                .WithColumn("IsActive").AsBoolean();
        }
        
        public override void Down()
        {
            Delete.Table("Log");
            Delete.Table("RefreshToken");
            Delete.Table("User");
        }
    }
}