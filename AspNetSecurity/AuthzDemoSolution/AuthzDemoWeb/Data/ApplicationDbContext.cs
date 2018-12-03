using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelLibrary;

namespace AuthzDemoWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private List<Guid> _easyGuids0 = new List<Guid>()
        {
            Guid.Parse("{00000000-0000-0000-0000-000000000001}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000002}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000003}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000004}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000005}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000006}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000007}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000008}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000009}"),
            Guid.Parse("{00000000-0000-0000-0000-00000000000A}"),
            Guid.Parse("{00000000-0000-0000-0000-00000000000B}"),
            Guid.Parse("{00000000-0000-0000-0000-00000000000C}"),
            Guid.Parse("{00000000-0000-0000-0000-00000000000D}"),
            Guid.Parse("{00000000-0000-0000-0000-00000000000E}"),
            Guid.Parse("{00000000-0000-0000-0000-00000000000F}"),
            Guid.Parse("{00000000-0000-0000-0000-000000000010}"),
        };

        private List<Guid> _easyGuids1 = new List<Guid>()
        {
            Guid.Parse("{00000000-1111-0000-0000-000000000001}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000002}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000003}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000004}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000005}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000006}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000007}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000008}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000009}"),
            Guid.Parse("{00000000-1111-0000-0000-00000000000A}"),
            Guid.Parse("{00000000-1111-0000-0000-00000000000B}"),
            Guid.Parse("{00000000-1111-0000-0000-00000000000C}"),
            Guid.Parse("{00000000-1111-0000-0000-00000000000D}"),
            Guid.Parse("{00000000-1111-0000-0000-00000000000E}"),
            Guid.Parse("{00000000-1111-0000-0000-00000000000F}"),
            Guid.Parse("{00000000-1111-0000-0000-000000000010}"),
        };

        private List<Guid> _easyGuids2 = new List<Guid>()
        {
            Guid.Parse("{00000000-2222-0000-0000-000000000001}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000002}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000003}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000004}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000005}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000006}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000007}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000008}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000009}"),
            Guid.Parse("{00000000-2222-0000-0000-00000000000A}"),
            Guid.Parse("{00000000-2222-0000-0000-00000000000B}"),
            Guid.Parse("{00000000-2222-0000-0000-00000000000C}"),
            Guid.Parse("{00000000-2222-0000-0000-00000000000D}"),
            Guid.Parse("{00000000-2222-0000-0000-00000000000E}"),
            Guid.Parse("{00000000-2222-0000-0000-00000000000F}"),
            Guid.Parse("{00000000-2222-0000-0000-000000000010}"),
        };

        private List<Guid> _easyGuids3 = new List<Guid>()
        {
            Guid.Parse("{00000000-3333-0000-0000-000000000001}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000002}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000003}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000004}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000005}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000006}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000007}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000008}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000009}"),
            Guid.Parse("{00000000-3333-0000-0000-00000000000A}"),
            Guid.Parse("{00000000-3333-0000-0000-00000000000B}"),
            Guid.Parse("{00000000-3333-0000-0000-00000000000C}"),
            Guid.Parse("{00000000-3333-0000-0000-00000000000D}"),
            Guid.Parse("{00000000-3333-0000-0000-00000000000E}"),
            Guid.Parse("{00000000-3333-0000-0000-00000000000F}"),
            Guid.Parse("{00000000-3333-0000-0000-000000000010}"),
        };

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Article>()
                .HasData(new Article()
                {
                    Id = _easyGuids1[0],
                    Name = "Arduino nano",
                    Price = 12,
                    Owner = "adult@vevy.com",
                    Maturity = Maturity.Adolescent,
                    State = ArticleState.ListedForSelling,
                    Buyer = null,
                },
                new Article()
                {
                    Id = _easyGuids1[1],
                    Name = "Rum Legendario",
                    Price = 25,
                    Owner = "john@email.com",
                    Maturity = Maturity.Adult,
                    State = ArticleState.Returned,
                    Buyer = "buyer@email.com",
                },
                new Article()
                {
                    Id = _easyGuids1[2],
                    Name = "Lego Mindstorm",
                    Price = 75,
                    Owner = "jane@email.com",
                    Maturity = Maturity.Child,
                    State = ArticleState.Sold,
                    Buyer = "buyer@email.com",
                },
                new Article()
                {
                    Id = _easyGuids1[3],
                    Name = "Tales of mistery and imagination by Edgar Allan Poe",
                    Price = 8,
                    Owner = "karl@email.com",
                    Maturity = Maturity.Adolescent,
                    State = ArticleState.ListedForSelling,
                    Buyer = null,
                },
                new Article()
                {
                    Id = _easyGuids1[4],
                    Name = "Beluga Vodka",
                    Price = 22,
                    Owner = "igor@email.ru",
                    Maturity = Maturity.Adult,
                    State = ArticleState.ListedForSelling,
                    Buyer = null,
                },
                new Article()
                {
                    Id = _easyGuids1[5],
                    Name = "Crystal Ball",
                    Price = 3,
                    Owner = "abaco@email.com",
                    Maturity = Maturity.Child,
                    State = ArticleState.ListedForSelling,
                    Buyer = null,
                },
                new Article()
                {
                    Id = _easyGuids1[6],
                    Name = "Lego Bricks",
                    Price = 25,
                    Owner = "boy@email.com",
                    Maturity = Maturity.Child,
                    State = ArticleState.ListedForSelling,
                    Buyer = null,
                },
                new Article()
                {
                    Id = _easyGuids1[7],
                    Name = "Three men in a boat",
                    Price = 25,
                    Owner = "dog@email.com",
                    Maturity = Maturity.Child,
                    State = ArticleState.Sold,
                    Buyer = "buyer2@email.com",
                }
                );


            builder
                .Entity<Permission>()
                .HasData(new Permission()
                {
                    Id = _easyGuids2[0],
                    User = 1,
                    PermissionList = "admin;",
                }
                );


        }
    }
}
