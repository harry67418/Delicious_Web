using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Repositories
{
    public interface IFolderPageRepository
    {
        //查詢會員所有收藏夾
        public List<TcollectionFolder> QueryAllMemberFolder(int memberId);
        //查詢會員所有收藏資訊
        public List<Trecipe> QueryAllMemberCollectionRecipe(int memberId);
        //查詢單筆收藏夾內的食譜
        public List<Trecipe> QueryRecipeInFolder(int memberId, int folderId);
        //查詢單筆食譜存的收藏夾
        public IQueryable QueryFolderWithRecipe(int memberId, int recipeId);
        //食譜加入收藏夾
        public bool InsertRecipeInFolder(Tcollection tcollection);
        //食譜移出收藏夾
        public bool MoveRecipeOutFolder(int recipeId, int folderId);
        //新增收藏夾
        public bool CreateFolder(TcollectionFolder tcollectionFolder);
        //編輯收藏夾名稱
        public bool EditFolderName(string folderName,int folderId);
        //刪除收藏夾
        public bool DeleteFolder(int folderId);
    }
}
