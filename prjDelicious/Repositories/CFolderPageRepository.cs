using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Repositories
{
    public class CFolderPageRepository : IFolderPageRepository
    {
        private readonly DeliciousContext _deliciousContext;
        public CFolderPageRepository(DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
        }
        //查詢此會員所有收藏夾
        public List<TcollectionFolder> QueryAllMemberFolder(int memberId)
        {
            return _deliciousContext.TcollectionFolders.Where(t => t.MemberId == memberId).Select(t => t).ToList();
        }
        //查詢此會員所有收藏食譜
        public List<Trecipe> QueryAllMemberCollectionRecipe(int memberId)
        {
            return _deliciousContext.Tcollections.Where(t => t.CollectionFolder.MemberId == memberId).Select(t => t.Reicipe).Distinct().ToList();
        }
        //查詢收藏夾內所有食譜
        public List<Trecipe> QueryRecipeInFolder(int memberId, int folderId)
        {
            return _deliciousContext.Tcollections.Where(t => t.CollectionFolder.MemberId == memberId && t.CollectionFolderId == folderId).Select(t => t.Reicipe).Distinct().ToList();
        }
        //查詢該食譜的收藏夾
        public IQueryable QueryFolderWithRecipe(int memberId, int recipeId)
        {
            return _deliciousContext.Tcollections.Where(t => t.CollectionFolder.MemberId == memberId && t.ReicipeId == recipeId).Select(t => new
            {
                t.CollectionFolder.CollectionFolder,
                t.CollectionFolderId,
                t.CollectionId
            });
        }
        //食譜加入收藏夾
        public bool InsertRecipeInFolder(Tcollection tcollection)
        {
            _deliciousContext.Tcollections.Add(tcollection);
            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //食譜移除收藏夾
        public bool MoveRecipeOutFolder(int recipeId, int folderId)
        {
            var tcollection = _deliciousContext.Tcollections.Where(t => t.ReicipeId == recipeId && t.CollectionFolderId == folderId).FirstOrDefault();

            _deliciousContext.Tcollections.Remove(tcollection);
            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //新增收藏夾
        public bool CreateFolder(TcollectionFolder tcollectionFolder)
        {
            _deliciousContext.TcollectionFolders.Add(tcollectionFolder);
            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //編輯收藏夾名稱
        public bool EditFolderName(string folderName,int folderId)
        {
            var folder = _deliciousContext.TcollectionFolders.Where(t => t.CollectionFolderId == folderId).FirstOrDefault();
            folder.CollectionFolder = folderName;
            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //刪除收藏夾
        public bool DeleteFolder(int folderId)
        {
            var folder = _deliciousContext.TcollectionFolders.Where(t => t.CollectionFolderId == folderId).FirstOrDefault();

            var tcollections = _deliciousContext.Tcollections.Where(t => t.CollectionFolder.CollectionFolderId == folderId).Select(t => t);

            if (folder != null)
            {
                _deliciousContext.TcollectionFolders.Remove(folder);
            }
            if (tcollections != null)
            {
                _deliciousContext.Tcollections.RemoveRange(tcollections);
            }
            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
