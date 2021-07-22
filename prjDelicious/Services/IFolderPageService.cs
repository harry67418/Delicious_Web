using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Services
{
    public interface IFolderPageService
    {
        //查詢此會員所有收藏夾和收藏食譜   
        public CFolderPageViewModel QueryAllMemberFolderAndRecipes(int memberId);
        //查詢收藏夾內所有食譜
        public CFolderPageViewModel QueryRecipeInFolder(int memberId, int folderId);
        //查詢該食譜的收藏夾
        public IQueryable QueryFolderWithRecipe(int memberId, int recipeId);
        //食譜加入收藏夾
        public bool InsertRecipeInFolder(int recipeId, int folderId);
        //食譜移出收藏夾
        public bool MoveRecipeOutFolder(int recipeId, int folderId);
        //新增收藏夾
        public bool CreateFolder(string folderName, int memberId);
        //編輯收藏夾名稱
        public bool EditFolderName(string folderName, int folderId);
        //刪除收藏夾
        public bool DeleteFolder(int folderId);
    }
}
