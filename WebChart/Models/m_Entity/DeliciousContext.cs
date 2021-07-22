using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebChart.Models
{
    public partial class DeliciousContext : DbContext
    {
        public DeliciousContext()
        {
        }

        public DeliciousContext(DbContextOptions<DeliciousContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Taccusation> Taccusations { get; set; }
        public virtual DbSet<TaccuseContent> TaccuseContents { get; set; }
        public virtual DbSet<Tadmin> Tadmins { get; set; }
        public virtual DbSet<Tcollection> Tcollections { get; set; }
        public virtual DbSet<TcollectionFolder> TcollectionFolders { get; set; }
        public virtual DbSet<TcommentSection> TcommentSections { get; set; }
        public virtual DbSet<TcustomerChat> TcustomerChats { get; set; }
        public virtual DbSet<Tdistinct> Tdistincts { get; set; }
        public virtual DbSet<Tfeedback> Tfeedbacks { get; set; }
        public virtual DbSet<TfeedbackCategory> TfeedbackCategories { get; set; }
        public virtual DbSet<TfeedbackProgress> TfeedbackProgresses { get; set; }
        public virtual DbSet<Thashtag> Thashtags { get; set; }
        public virtual DbSet<ThashtagRecord> ThashtagRecords { get; set; }
        public virtual DbSet<Tingredient> Tingredients { get; set; }
        public virtual DbSet<TingredientCategory> TingredientCategories { get; set; }
        public virtual DbSet<TingredientRecord> TingredientRecords { get; set; }
        public virtual DbSet<TlikeComment> TlikeComments { get; set; }
        public virtual DbSet<TlikeRecipe> TlikeRecipes { get; set; }
        public virtual DbSet<Tmember> Tmembers { get; set; }
        public virtual DbSet<TmemberRank> TmemberRanks { get; set; }
        public virtual DbSet<TmerchandisePicture> TmerchandisePictures { get; set; }
        public virtual DbSet<Torder> Torders { get; set; }
        public virtual DbSet<TorderDetail> TorderDetails { get; set; }
        public virtual DbSet<TphotoWall> TphotoWalls { get; set; }
        public virtual DbSet<TphotoWallCategory> TphotoWallCategories { get; set; }
        public virtual DbSet<Trecipe> Trecipes { get; set; }
        public virtual DbSet<TrecipeCategory> TrecipeCategories { get; set; }
        public virtual DbSet<TshoppingCart> TshoppingCarts { get; set; }
        public virtual DbSet<TshowingPic> TshowingPics { get; set; }
        public virtual DbSet<Tstep> Tsteps { get; set; }
        public virtual DbSet<TwishList> TwishLists { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=msit130delicious.database.windows.net;Initial Catalog=Delicious;Persist Security Info=True;User ID=msit13001;Password=msit130-01");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<Taccusation>(entity =>
            {
                entity.HasKey(e => e.AccusationRightId)
                    .HasName("PK_Accusation_Table");

                entity.ToTable("TAccusation");

                entity.Property(e => e.AccusationRightId).HasColumnName("AccusationRightID");

                entity.Property(e => e.AccuseId).HasColumnName("AccuseID");

                entity.Property(e => e.AccusedAvatar)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.AccusedId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("AccusedID")
                    .IsFixedLength(true);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProgressId)
                    .HasColumnName("ProgressID")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Accuse)
                    .WithMany(p => p.Taccusations)
                    .HasForeignKey(d => d.AccuseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accusation_Table_AccuseContent_Table");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Taccusations)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accusation_Table_Member_Table");
            });

            modelBuilder.Entity<TaccuseContent>(entity =>
            {
                entity.HasKey(e => e.AccuseId)
                    .HasName("PK_AccuseContent_Table");

                entity.ToTable("TAccuseContent");

                entity.Property(e => e.AccuseId).HasColumnName("AccuseID");

                entity.Property(e => e.Accusation)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Tadmin>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK_Admin_Table_1");

                entity.ToTable("TAdmin");

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.AdminName)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tcollection>(entity =>
            {
                entity.HasKey(e => e.CollectionId)
                    .HasName("PK_Collection_Table");

                entity.ToTable("TCollection");

                entity.Property(e => e.CollectionId).HasColumnName("CollectionID");

                entity.Property(e => e.CollectionFolderId).HasColumnName("CollectionFolderID");

                entity.Property(e => e.Datetime).HasColumnType("datetime");

                entity.Property(e => e.ReicipeId).HasColumnName("ReicipeID");

                entity.HasOne(d => d.CollectionFolder)
                    .WithMany(p => p.Tcollections)
                    .HasForeignKey(d => d.CollectionFolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Collection_Table_CollectionFolder_Table");

                entity.HasOne(d => d.Reicipe)
                    .WithMany(p => p.Tcollections)
                    .HasForeignKey(d => d.ReicipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Collection_Table_Recipe_Table");
            });

            modelBuilder.Entity<TcollectionFolder>(entity =>
            {
                entity.HasKey(e => e.CollectionFolderId)
                    .HasName("PK_CollectionFolder_Table");

                entity.ToTable("TCollectionFolder");

                entity.Property(e => e.CollectionFolderId).HasColumnName("CollectionFolderID");

                entity.Property(e => e.CollectionFolder)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TcollectionFolders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionFolder_Table_Member_Table");
            });

            modelBuilder.Entity<TcommentSection>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK_CommentSection_Table");

                entity.ToTable("TCommentSection");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PostTime).HasColumnType("datetime");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.Video).HasMaxLength(255);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TcommentSections)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentSection_Table_Member_Table");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.TcommentSections)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentSection_Table_Recipe_Table");
            });

            modelBuilder.Entity<TcustomerChat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TcustomerChat");

                entity.Property(e => e.SendPicture)
                    .HasMaxLength(250)
                    .HasColumnName("sendPicture");

                entity.Property(e => e.Senddate)
                    .HasMaxLength(50)
                    .HasColumnName("senddate");

                entity.Property(e => e.SenderName)
                    .HasMaxLength(50)
                    .HasColumnName("senderName");

                entity.Property(e => e.Sendmessage)
                    .HasMaxLength(250)
                    .HasColumnName("sendmessage");
            });

            modelBuilder.Entity<Tdistinct>(entity =>
            {
                entity.HasKey(e => e.CountyId);

                entity.ToTable("TDistinct");

                entity.Property(e => e.CountyId).HasColumnName("CountyID");

                entity.Property(e => e.DeliveryCounty)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.DeliveryDistinct)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<Tfeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("PK_Feedback_Table");

                entity.ToTable("TFeedback");

                entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FeedbackCategoryId).HasColumnName("FeedbackCategoryID");

                entity.Property(e => e.FeedbackContent).HasMaxLength(50);

                entity.Property(e => e.FeedbackDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProgressId).HasColumnName("ProgressID");

                entity.HasOne(d => d.FeedbackCategory)
                    .WithMany(p => p.Tfeedbacks)
                    .HasForeignKey(d => d.FeedbackCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Table_Feedback_Category_Table1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Tfeedbacks)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Table_Member_Table");

                entity.HasOne(d => d.Progress)
                    .WithMany(p => p.Tfeedbacks)
                    .HasForeignKey(d => d.ProgressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Table_Feedback_Progress_Table");
            });

            modelBuilder.Entity<TfeedbackCategory>(entity =>
            {
                entity.HasKey(e => e.FeedbackCategoryId)
                    .HasName("PK_Feedback_Category_Table");

                entity.ToTable("TFeedbackCategory");

                entity.Property(e => e.FeedbackCategoryId).HasColumnName("FeedbackCategoryID");

                entity.Property(e => e.FeedbackCategory)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TfeedbackProgress>(entity =>
            {
                entity.HasKey(e => e.ProgressId)
                    .HasName("PK_Feedback_Progress_Table");

                entity.ToTable("TFeedbackProgress");

                entity.Property(e => e.ProgressId).HasColumnName("ProgressID");

                entity.Property(e => e.ProgressContent)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Thashtag>(entity =>
            {
                entity.HasKey(e => e.HashtagId)
                    .HasName("PK_Hashtag_Table");

                entity.ToTable("THashtag");

                entity.Property(e => e.HashtagId).HasColumnName("HashtagID");

                entity.Property(e => e.Hasgtag)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<ThashtagRecord>(entity =>
            {
                entity.HasKey(e => e.HashTagFolderId)
                    .HasName("PK_Hashtag_Record_Table");

                entity.ToTable("THashtag_Record");

                entity.Property(e => e.HashTagFolderId).HasColumnName("HashTagFolderID");

                entity.Property(e => e.HashTagId).HasColumnName("HashTagID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.HasOne(d => d.HashTag)
                    .WithMany(p => p.ThashtagRecords)
                    .HasForeignKey(d => d.HashTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hashtag_Record_Table_Hashtag_Table");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.ThashtagRecords)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hashtag_Record_Table_Recipe_Table");
            });

            modelBuilder.Entity<Tingredient>(entity =>
            {
                entity.HasKey(e => e.IngredientId)
                    .HasName("PK_Ingredient_Table");

                entity.ToTable("TIngredient");

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.Property(e => e.Ingredient)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.IngredientCategoryId).HasColumnName("IngredientCategoryID");

                entity.Property(e => e.IngredientUnit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'1公斤')");

                entity.Property(e => e.MerchandiseDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'無')");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IngredientCategory)
                    .WithMany(p => p.Tingredients)
                    .HasForeignKey(d => d.IngredientCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ingredient_Table_IngredientCategory_Table");
            });

            modelBuilder.Entity<TingredientCategory>(entity =>
            {
                entity.HasKey(e => e.IngredientCategoryId)
                    .HasName("PK_IngredientCategory_Table");

                entity.ToTable("TIngredientCategory");

                entity.Property(e => e.IngredientCategoryId).HasColumnName("IngredientCategoryID");

                entity.Property(e => e.IngredientCategory)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TingredientRecord>(entity =>
            {
                entity.HasKey(e => e.IngredientRecordId)
                    .HasName("PK_Ingredient_Record_Table");

                entity.ToTable("TIngredientRecord");

                entity.Property(e => e.IngredientRecordId).HasColumnName("IngredientRecordID");

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.Unt)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.TingredientRecords)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ingredient_Record_Table_Ingredient_Table");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.TingredientRecords)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ingredient_Record_Table_Recipe_Table");
            });

            modelBuilder.Entity<TlikeComment>(entity =>
            {
                entity.HasKey(e => e.CommentLikedId)
                    .HasName("PK_LikeComment_Table");

                entity.ToTable("TLikeComment");

                entity.Property(e => e.CommentLikedId).HasColumnName("CommentLikedID");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TlikeComments)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikeComment_Table_CommentSection_Table");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TlikeComments)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikeComment_Table_Member_Table");
            });

            modelBuilder.Entity<TlikeRecipe>(entity =>
            {
                entity.HasKey(e => e.RecipeLikedId)
                    .HasName("PK_LikeRecipe_Table");

                entity.ToTable("TLikeRecipe");

                entity.Property(e => e.RecipeLikedId).HasColumnName("RecipeLikedID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TlikeRecipes)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikeRecipe_Table_Member_Table");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.TlikeRecipes)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikeRecipe_Table_Recipe_Table");
            });

            modelBuilder.Entity<Tmember>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK_Member_Table");

                entity.ToTable("TMember");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CellConfirm)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CellNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ConfirmedOrNotEmail).HasColumnName("ConfirmedOrNot_email");

                entity.Property(e => e.ConfirmedOrNotPhone).HasColumnName("ConfirmedOrNot_phone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailConfirm)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Figure).IsRequired();

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RankId).HasColumnName("RankID");

                entity.Property(e => e.RegisterTime).HasColumnType("datetime");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Tmembers)
                    .HasForeignKey(d => d.RankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TMember_TMemberRank1");
            });

            modelBuilder.Entity<TmemberRank>(entity =>
            {
                entity.HasKey(e => e.RankId)
                    .HasName("PK_Member_Rank_Table");

                entity.ToTable("TMemberRank");

                entity.Property(e => e.RankId).HasColumnName("RankID");

                entity.Property(e => e.RankName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TmerchandisePicture>(entity =>
            {
                entity.HasKey(e => e.MerchandisePicId)
                    .HasName("PK_Merchandise_Picture_Table");

                entity.ToTable("TMerchandisePicture");

                entity.Property(e => e.MerchandisePicId).HasColumnName("MerchandisePicID");

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.Property(e => e.MerchandisePicture).IsRequired();

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.TmerchandisePictures)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Merchandise_Picture_Table_Ingredient_Table1");
            });

            modelBuilder.Entity<Torder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK_Order_Table");

                entity.ToTable("TOrder");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.DeliveredDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.DeliveryCounty)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.DeliveryDistrict)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MerchantTradeNo).HasMaxLength(20);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderStatus)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PayMethod)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.RecieveMethod)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Reciever).HasMaxLength(20);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Torders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Table_Member_Table");
            });

            modelBuilder.Entity<TorderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK_Order_Detail_Table");

                entity.ToTable("TOrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("Order_DetailID");

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.TorderDetails)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Detail_Table_Ingredient_Table");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TorderDetails)
                    .HasForeignKey(d => d.OrderiD)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Detail_Table_Order_Table");
            });

            modelBuilder.Entity<TphotoWall>(entity =>
            {
                entity.HasKey(e => e.PictureId);

                entity.ToTable("TPhotoWall");

                entity.Property(e => e.PictureId).HasColumnName("PictureID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ContributeTime).HasColumnType("date");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Picture).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TphotoWalls)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TPhotoWall_TPhotoWallCategory");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TphotoWalls)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TPhotoWall_TMember");
            });

            modelBuilder.Entity<TphotoWallCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("TPhotoWallCategory");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HtmlClassName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Trecipe>(entity =>
            {
                entity.HasKey(e => e.RecipeId)
                    .HasName("PK_Recipe_Table");

                entity.ToTable("TRecipe");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Picture).IsRequired();

                entity.Property(e => e.PostTime).HasColumnType("datetime");

                entity.Property(e => e.RecipeCategoryId).HasColumnName("RecipeCategoryID");

                entity.Property(e => e.RecipeDescription)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RecipeName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Video).HasMaxLength(255);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Trecipes)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_Table_Member_Table");

                entity.HasOne(d => d.RecipeCategory)
                    .WithMany(p => p.Trecipes)
                    .HasForeignKey(d => d.RecipeCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_Table_RecipeCategory_Table");
            });

            modelBuilder.Entity<TrecipeCategory>(entity =>
            {
                entity.HasKey(e => e.RecipeCategoryId)
                    .HasName("PK_Category_Table");

                entity.ToTable("TRecipeCategory");

                entity.Property(e => e.RecipeCategoryId).HasColumnName("RecipeCategoryID");

                entity.Property(e => e.RecipeCategory)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<TshoppingCart>(entity =>
            {
                entity.HasKey(e => e.CartId)
                    .HasName("PK_Shopping_Cart_table");

                entity.ToTable("TShoppingCart");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.TshoppingCarts)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shopping_Cart_table_Ingredient_Table");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TshoppingCarts)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shopping_Cart_table_Member_Table");
            });

            modelBuilder.Entity<TshowingPic>(entity =>
            {
                entity.HasKey(e => e.ShowingPicsId);

                entity.ToTable("TShowingPics");

                entity.Property(e => e.ShowingPicsId).HasColumnName("ShowingPicsID");

                entity.Property(e => e.ShowingPicsDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShowingPicsHyperLink).HasMaxLength(50);

                entity.Property(e => e.ShowingPicsPathRoad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShowingPicsTitle)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Tstep>(entity =>
            {
                entity.HasKey(e => e.StepsId)
                    .HasName("PK_Steps_Table");

                entity.ToTable("TSteps");

                entity.Property(e => e.StepsId).HasColumnName("StepsID");

                entity.Property(e => e.Picture).IsUnicode(false);

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.Steps)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Tsteps)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Steps_Table_Recipe_Table");
            });

            modelBuilder.Entity<TwishList>(entity =>
            {
                entity.HasKey(e => e.WishListId)
                    .HasName("PK_Wish_List_Table");

                entity.ToTable("TWishList");

                entity.Property(e => e.WishListId).HasColumnName("WishListID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TwishLists)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wish_List_Table_Member_Table");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.TwishLists)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TWishList_TRecipe");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
