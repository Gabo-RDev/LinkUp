using LinkUp.Domain.Enum;
using LinkUp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Context;

public class LinkUpDbContext(DbContextOptions<LinkUpDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    #region Models

    public DbSet<User> Users { get; set; }

    public DbSet<Admin> Admins { get; set; }

    public DbSet<Interest> Interests { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<PostInterest> PostInterests { get; set; }

    public DbSet<UserInterest> UserInterests { get; set; }

    public DbSet<PostLike> PostLikes { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Code> Codes { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Tables

        modelBuilder.Entity<User>().ToTable("Users");

        modelBuilder.Entity<Admin>().ToTable("Admins");

        modelBuilder.Entity<Interest>().ToTable("Interests");

        modelBuilder.Entity<Post>().ToTable("Posts");

        modelBuilder.Entity<PostInterest>().ToTable("PostInterests");

        modelBuilder.Entity<UserInterest>().ToTable("UserInterests");

        modelBuilder.Entity<PostLike>().ToTable("PostLikes");

        modelBuilder.Entity<Comment>().ToTable("Comments");

        modelBuilder.Entity<Code>().ToTable("Codes");

        modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");

        modelBuilder.Entity<PostCategory>().ToTable("PostCategories");

        #endregion

        #region PK

        modelBuilder.Entity<User>().HasKey(x => x.Id).HasName("PK_Users");

        modelBuilder.Entity<Admin>().HasKey(x => x.Id).HasName("PK_Admins");

        modelBuilder.Entity<Interest>().HasKey(x => x.Id).HasName("PK_Interests");

        modelBuilder.Entity<Post>().HasKey(x => x.Id).HasName("PK_Posts");

        modelBuilder.Entity<PostInterest>().HasKey(x => x.Id).HasName("PK_PostInterests");

        modelBuilder.Entity<UserInterest>().HasKey(x => x.Id).HasName("PK_UserInterests");

        modelBuilder.Entity<PostLike>().HasKey(x => x.Id).HasName("PK_PostLikes");

        modelBuilder.Entity<Comment>().HasKey(x => x.Id).HasName("PK_Comments");

        modelBuilder.Entity<Code>().HasKey(x => x.Id).HasName("PK_Codes");

        modelBuilder.Entity<RefreshToken>().HasKey(x => x.Id).HasName("PK_RefreshTokens");

        modelBuilder.Entity<PostCategory>().HasKey(x => x.Id).HasName("PK_PostCategories");

        modelBuilder.Entity<UserInterest>().HasKey(x => x.Id).HasName("PK_UserInterests");

        modelBuilder.Entity<PostInterest>().HasKey(x => x.Id).HasName("PK_PostInterests");

        modelBuilder.Entity<PostLike>().HasKey(x => x.Id).HasName("PK_PostLike");

        #endregion

        #region Relationships

        modelBuilder.Entity<Admin>()
            .HasMany(ad => ad.Posts)
            .WithOne(p => p.Admin)
            .HasForeignKey(p => p.AdminId)
            .IsRequired()
            .HasConstraintName("FKAdminsPosts")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Code>()
            .HasOne(co => co.User)
            .WithMany(u => u.Codes)
            .HasForeignKey(c => c.UserId)
            .IsRequired()
            .HasConstraintName("FKCodesUsers")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .IsRequired()
            .HasConstraintName("FKCommentsPosts")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.UserId)
            .IsRequired()
            .HasConstraintName("FKCommentsUser")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Interest>()
            .HasMany(interest => interest.UserInterests)
            .WithOne(ui => ui.Interest)
            .HasForeignKey(ui => ui.InterestId)
            .IsRequired()
            .HasConstraintName("FKInterestsUserInterests")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Interest>()
            .HasMany(interest => interest.PostInterests)
            .WithOne(ui => ui.Interest)
            .HasForeignKey(ui => ui.InterestId)
            .IsRequired()
            .HasConstraintName("FKInterestsPostInterests")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(rt => rt.RefreshToken)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired()
            .HasConstraintName("FKRefreshTokensUsers")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PostCategory>()
            .HasMany(pc => pc.Posts)
            .WithOne(po => po.Category)
            .HasForeignKey(po => po.CategoryId)
            .IsRequired()
            .HasConstraintName("FKPostCategoriesPosts")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
            .HasMany(po => po.PostLikes)
            .WithOne(pl => pl.Post)
            .HasForeignKey(pl => pl.PostId)
            .IsRequired()
            .HasConstraintName("FKPostsPostLikes")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(po => po.PostLikes)
            .WithOne(pl => pl.User)
            .HasForeignKey(pl => pl.UserId)
            .IsRequired()
            .HasConstraintName("FKUsersPostLikes")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(us => us.UserInterests)
            .WithOne(ui => ui.User)
            .HasForeignKey(ui => ui.UserId)
            .IsRequired()
            .HasConstraintName("FKUsersUserInterests")
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Users

        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(25);

            user.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(25);

            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(25);

            user.HasIndex(u => u.UserName)
                .IsUnique();

            user.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(50);

            user.HasIndex(u => u.Email)
                .IsUnique();

            user.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            user.Property(u => u.ProfilePhoto)
                .HasMaxLength(200);

            user.Property(u => u.CoverPhoto)
                .HasMaxLength(200);

            user.Property(u => u.Biography)
                .HasColumnType("text");

            user.Property(u => u.Birthday)
                .HasColumnType("date");

            user.Property(u => u.LastLoginAt)
                .HasColumnType("timestamp");

            user.Property(u => u.ConfirmedAccount)
                .HasDefaultValue(false);

            user.Property(u => u.Status)
                .HasConversion<string>()
                .HasDefaultValue(UserStatus.Active);

            user.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            user.Property(u => u.UpdatedAt)
                .HasColumnType("timestamp");

            user.Property(u => u.Deleted)
                .HasDefaultValue(false);

            user.Property(u => u.DeletedAt)
                .HasColumnType("timestamp");
        });

        #endregion

        #region Admin

        modelBuilder.Entity<Admin>(admin =>
        {
            admin.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(25);

            admin.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(25);

            admin.Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(25);

            admin.HasIndex(a => a.UserName)
                .IsUnique();

            admin.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(50);

            admin.HasIndex(a => a.Email)
                .IsUnique();

            admin.Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(100);

            admin.Property(a => a.ProfilePhoto)
                .HasMaxLength(200);

            admin.Property(a => a.LastLoginAt)
                .HasColumnType("timestamp");

            admin.Property(a => a.Status)
                .HasConversion<string>()
                .HasDefaultValue(AdminStatus.Active);
            
            admin.Property(a => a.ConfirmedAccount)
                .HasDefaultValue(false);

            admin.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            admin.Property(a => a.UpdatedAt)
                .HasColumnType("timestamp");

            admin.Property(a => a.Deleted)
                .HasDefaultValue(false);

            admin.Property(a => a.DeletedAt)
                .HasColumnType("timestamp");
        });

        #endregion

        #region RefreshToken

        modelBuilder.Entity<RefreshToken>(refreshToken =>
        {
            refreshToken.Property(rt => rt.Value)
                .IsRequired()
                .HasColumnType("text");

            refreshToken.Property(rt => rt.Used)
                .HasDefaultValue(false);

            refreshToken.Property(rt => rt.Expiration)
                .IsRequired()
                .HasColumnType("timestamp");

            refreshToken.Property(rt => rt.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            refreshToken.Property(rt => rt.Revoked)
                .HasDefaultValue(false);
        });

        #endregion

        #region Code

        modelBuilder.Entity<Code>(code =>
        {
            code.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(6);

            code.Property(c => c.Expiration)
                .IsRequired()
                .HasColumnType("timestamp");

            code.Property(c => c.Type)
                .HasConversion<string>();

            code.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            code.Property(c => c.Revoked)
                .HasDefaultValue(false);
        });

        #endregion

        #region Posts

        modelBuilder.Entity<Post>(post =>
        {
            post.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            post.Property(p => p.Content)
                .IsRequired()
                .HasColumnType("text");

            post.Property(p => p.AdminId);

            post.Property(p => p.CategoryId);

            post.Property(p => p.LikesCount)
                .HasDefaultValue(0);

            post.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            post.Property(p => p.UpdatedAt)
                .HasColumnType("timestamp");

            post.Property(p => p.Deleted)
                .HasDefaultValue(false);

            post.Property(p => p.DeletedAt)
                .HasColumnType("timestamp");
        });

        #endregion

        #region PostCategory

        modelBuilder.Entity<PostCategory>(category =>
        {
            category.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(25);
        });

        #endregion

        #region PostInterest

        modelBuilder.Entity<PostInterest>(pi =>
        {
            pi.Property(p => p.PostId)
                .IsRequired();

            pi.Property(p => p.InterestId)
                .IsRequired();

            pi.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            pi.Property(p => p.UpdatedAt)
                .HasColumnType("timestamp");
        });

        #endregion

        #region Interest

        modelBuilder.Entity<Interest>(interest =>
        {
            interest.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50);

            interest.Property(i => i.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            interest.Property(i => i.UpdatedAt)
                .HasColumnType("timestamp");

            interest.Property(i => i.Deleted)
                .HasDefaultValue(false);
        });

        #endregion

        #region UserInterest

        modelBuilder.Entity<UserInterest>(ui =>
        {
            ui.Property(u => u.UserId)
                .IsRequired();

            ui.Property(u => u.InterestId)
                .IsRequired();

            ui.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            ui.Property(u => u.UpdatedAt)
                .HasColumnType("timestamp");
        });

        #endregion

        #region PostLike

        modelBuilder.Entity<PostLike>(pl =>
        {
            pl.Property(p => p.PostId)
                .IsRequired();

            pl.Property(p => p.UserId)
                .IsRequired();

            pl.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");
        });

        #endregion

        #region Comment

        modelBuilder.Entity<Comment>(comment =>
        {
            comment.Property(c => c.Description)
                .IsRequired()
                .HasColumnType("text");

            comment.Property(c => c.PostId)
                .IsRequired();

            comment.Property(c => c.UserId)
                .IsRequired();

            comment.Property(c => c.Edited)
                .HasDefaultValue(false);

            comment.Property(c => c.IsPinned)
                .HasDefaultValue(false);

            comment.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            comment.Property(c => c.UpdatedAt)
                .HasColumnType("timestamp");

            comment.Property(c => c.Deleted)
                .HasDefaultValue(false);

            comment.Property(c => c.DeletedAt)
                .HasColumnType("timestamp");
        });

        #endregion

        #region Filter

        modelBuilder.Entity<User>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<Admin>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<Interest>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<Post>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<Comment>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<Post>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<UserInterest>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<PostLike>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<PostInterest>().HasQueryFilter(u => !u.Deleted);

        modelBuilder.Entity<PostInterest>().HasQueryFilter(u => !u.Deleted);

        #endregion
    }
}